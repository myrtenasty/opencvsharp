#include <opencv2/core.hpp>
#include <opencv2/surface_matching.hpp>
#include <chrono>
#include <cmath>
#include <iostream>
#include <limits>
#include <stdexcept>
#include <tuple>

using namespace cv;
using namespace cv::ppf_match_3d;

static void require(bool condition, const char* message)
{
    if (!condition)
        throw std::runtime_error(message);
}

template<class F> static void rejects(F action)
{
    try { action(); }
    catch (const cv::Exception&) { return; }
    throw std::runtime_error("Expected a cv::Exception");
}

class InspectableDetector : public PPF3DDetector
{
public:
    using PPF3DDetector::sampled_pc;
    using PPF3DDetector::num_ref_points;
    using PPF3DDetector::distance_step;
};

static Mat cloud(int width, int height, bool plane = false)
{
    Mat points(width * height, 6, CV_32F);
    for (int y = 0, i = 0; y < height; ++y)
        for (int x = 0; x < width; ++x, ++i)
        {
            float xf = x * 0.017f, yf = y * 0.019f;
            float z = plane ? 0.2f * xf : 0.6f * xf * xf + 0.4f * yf * yf + 0.1f * std::sin(3 * xf + yf);
            float dx = plane ? 0.2f : 1.2f * xf + 0.3f * std::cos(3 * xf + yf);
            float dy = plane ? 0.0f : 0.8f * yf + 0.1f * std::cos(3 * xf + yf);
            Vec3f normal(-dx, -dy, 1);
            normal *= 1.0f / static_cast<float>(norm(normal));
            float* row = points.ptr<float>(i);
            row[0] = xf; row[1] = yf; row[2] = z;
            for (int j = 0; j < 3; ++j) row[j + 3] = normal[j];
        }
    return points;
}

static std::vector<size_t> votes(const std::vector<Pose3DPtr>& poses)
{
    std::vector<size_t> result;
    for (const auto& pose : poses) result.push_back(pose->numVotes);
    return result;
}

int main()
{
    try
    {
        const Mat model = cloud(13, 9);
        InspectableDetector detector;
        detector.trainPrepared(model, 0.0130000000000001);
        require(detector.num_ref_points == model.rows, "Training changed row count");
        require(norm(detector.sampled_pc, model, NORM_INF) == 0, "Training modified points/normals");
        require(detector.sampled_pc.data != model.data, "Training did not own its copy");
        require(detector.distance_step == 0.0130000000000001, "Metre bin lost precision");
        std::vector<Pose3DPtr> poses;
        detector.setSearchParams(0, 0);
        detector.matchPrepared(model, poses, 1, 0.0130000000000001);
        require(poses.size() == static_cast<size_t>(model.rows), "Scene rows were quantized");
        require(poses.front()->numVotes > 0, "Self match did not vote");
        detector.matchPrepared(model, poses, 0.2, 0.0130000000000001);
        require(poses.size() == 24, "Last reference in partial stride was lost");
        detector.matchPrepared(model, poses, std::numeric_limits<double>::denorm_min(), 0.0130000000000001);
        require(poses.size() == 1, "Tiny reference fraction overflowed");
        rejects([&] { detector.matchPrepared(model, poses, 0.2, 0.013); });
        rejects([&] { detector.trainPrepared(model, 0); });
        rejects([&] { detector.trainPrepared(model, std::numeric_limits<double>::quiet_NaN()); });
        rejects([&] { detector.trainPrepared(model, 1e-300); });
        rejects([&] { detector.trainPrepared(model, 0.03, 0.5); });
        rejects([&] { detector.matchPrepared(model, poses, 0, 0.0130000000000001); });
        rejects([&] { detector.matchPrepared(model, poses, 1.1, 0.0130000000000001); });
        Mat bad = model.clone();
        bad.at<float>(0, 3) = 2;
        rejects([&] { detector.trainPrepared(bad, 0.03); });
        rejects([&] { detector.matchPrepared(bad, poses, 1, 0.0130000000000001); });
        bad = model.clone();
        bad.at<float>(0, 0) = std::numeric_limits<float>::infinity();
        rejects([&] { detector.trainPrepared(bad, 0.03); });
        rejects([&] { detector.trainPrepared(Mat(2, 5, CV_32F), 0.03); });
        rejects([&] { detector.trainPrepared(Mat(2, 6, CV_64F), 0.03); });
        rejects([&] { detector.trainPrepared(Mat(), 0.03); });
        rejects([&] { detector.trainPrepared(model.row(0), 0.03); });

        Mat roundedNormals = model.clone();
        for (int i = 0; i < roundedNormals.rows; ++i)
        {
            roundedNormals.at<float>(i, 3) = 1.00001f;
            roundedNormals.at<float>(i, 4) = 0;
            roundedNormals.at<float>(i, 5) = 0;
        }
        detector.trainPrepared(roundedNormals, 0.03);
        detector.matchPrepared(roundedNormals, poses, 0.2, 0.03);
        require(!poses.empty(), "Tolerated rounded normals produced no poses");
        for (const auto& pose : poses)
            for (double value : pose->pose.val)
                require(std::isfinite(value), "Tolerated rounded normal produced NaN pose");
        require(norm(detector.sampled_pc, roundedNormals, NORM_INF) == 0, "Rounded normals were modified");

        // Duplicate XYZ with distinct edge directions must remain distinct rows.
        Mat edge = model.clone();
        model.row(0).copyTo(edge.row(1));
        edge.at<float>(1, 3) = 1; edge.at<float>(1, 4) = 0; edge.at<float>(1, 5) = 0;
        detector.trainPrepared(edge, 0.03);
        require(norm(detector.sampled_pc, edge, NORM_INF) == 0, "Edge normals were merged");
        Mat wide(model.rows, 8, CV_32F);
        Mat roi = wide.colRange(1, 7);
        model.copyTo(roi);
        detector.trainPrepared(roi, 0.03);
        require(norm(detector.sampled_pc, model, NORM_INF) == 0, "Strided input changed");

        // Partial visibility, deterministic measurement noise and unrelated clutter.
        Mat scene = model.rowRange(0, 83).clone();
        for (int i = 0; i < scene.rows; ++i)
            scene.at<float>(i, 2) += 0.004f * std::sin(i * 1.7f);
        Mat clutter = cloud(7, 5);
        for (int i = 0; i < clutter.rows; ++i)
        {
            clutter.at<float>(i, 0) += 0.14f;
            clutter.at<float>(i, 1) -= 0.07f;
            clutter.at<float>(i, 2) += 0.06f;
        }
        scene.push_back(clutter);
        detector.trainPrepared(model, 0.005);
        detector.matchPrepared(scene, poses, 0.2, 0.005);
        const auto fine = votes(poses);
        detector.trainPrepared(model, 0.04);
        detector.matchPrepared(scene, poses, 0.2, 0.04);
        require(fine != votes(poses), "Changing distance bin did not change hypotheses");
        std::cout << "Bin sensitivity: fine top votes=" << fine.front()
                  << ", coarse top votes=" << poses.front()->numVotes << "\n";

        // Moving the overall scene bbox outwards cannot coarsen existing points.
        detector.trainPrepared(model, 0.03);
        detector.matchPrepared(model, poses, 1, 0.03);
        auto originalVotes = votes(poses);
        Mat expanded = model.clone();
        Mat distant = model.row(0).clone();
        distant.at<float>(0, 0) += 100;
        expanded.push_back(distant);
        detector.matchPrepared(expanded, poses, 1, 0.03);
        require(poses.size() == static_cast<size_t>(expanded.rows), "Expanded scene was resampled");
        auto expandedVotes = votes(poses);
        expandedVotes.pop_back(); // The distant reference has zero matching partners.
        require(expandedVotes == originalVotes, "Scene bbox changed target votes");

        detector.trainModel(model);
        rejects([&] { detector.matchPrepared(model, poses, 0.2, 0.03); });
        detector.match(model, poses, 0.2, 0.05);
        require(!poses.empty(), "Legacy matching failed");

        // Two instances separated by 2 metres and 5 degrees of yaw.
        // One reference per instance isolates clustering from reference count.
        Mat pairModel = cloud(10, 10);
        Mat yawScene = pairModel.clone();
        Mat instance = pairModel.clone();
        const double yaw = 5 * CV_PI / 180;
        for (int i = 0; i < instance.rows; ++i)
        {
            float* r = instance.ptr<float>(i);
            for (int offset : {0, 3})
            {
                const float x = r[offset], y = r[offset + 1];
                r[offset] = static_cast<float>(std::cos(yaw) * x - std::sin(yaw) * y);
                r[offset + 1] = static_cast<float>(std::sin(yaw) * x + std::cos(yaw) * y);
            }
            r[0] += 2;
        }
        yawScene.push_back(instance);
        detector.trainPrepared(pairModel, 0.01, 360);
        detector.setSearchParams(3, 0.02);
        detector.matchPrepared(yawScene, poses, 0.01, 0.01);
        require(poses.size() == 2, "Narrow radians threshold merged 5-degree yaw");
        detector.setSearchParams(3, 0.2);
        detector.matchPrepared(yawScene, poses, 0.01, 0.01);
        require(poses.size() == 1, "Wide radians threshold did not merge 5-degree yaw");
        detector.setSearchParams(1, 0.2);
        detector.matchPrepared(yawScene, poses, 0.01, 0.01);
        require(poses.size() == 2, "One-metre threshold merged two-metre translations");
        detector.setSearchParams(3, 0.2, true);
        detector.matchPrepared(yawScene, poses, 0.01, 0.01);
        require(poses.size() == 1, "Three-metre weighted clustering did not merge instances");
        std::cout << "Clustering: 5-degree yaw and 2-metre translation thresholds passed\n";

        Mat dense = cloud(40, 32, true);
        const auto start = std::chrono::steady_clock::now();
        detector.trainPrepared(dense, 0.03);
        const double seconds = std::chrono::duration<double>(std::chrono::steady_clock::now() - start).count();
        require(detector.num_ref_points == 1280, "Dense prepared model was sampled");
        require(seconds < 12, "Optimized hash regressed to duplicate-chain insertion");
        std::cout << "Prepared training: 1280 rows, " << seconds << " seconds\n";
        std::cout << "All native Prepared checks passed\n";
        return 0;
    }
    catch (const std::exception& error)
    {
        std::cerr << error.what() << "\n";
        return 1;
    }
}
