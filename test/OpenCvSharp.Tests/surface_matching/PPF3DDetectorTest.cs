using OpenCvSharp.PpfMatch3D;
using Xunit;

namespace OpenCvSharp.Tests.SurfaceMatching;

public class PPF3DDetectorTest : TestBase
{
    [Fact]
    public void TrainMatchAndRefinePoses()
    {
        using var model = CreatePointCloudWithNormals();
        using var detector = new PPF3DDetector(
            relativeSamplingStep: 0.08,
            relativeDistanceStep: 0.08,
            numberOfAngles: 30);

        detector.SetSearchParams();
        detector.TrainModel(model);
        var results = detector.Match(
            model,
            relativeSceneSampleStep: 0.2,
            relativeSceneDistance: 0.08);

        try
        {
            Assert.NotEmpty(results);
            Assert.True(results[0].NumVotes > 0);

            using (var matchedPose = results[0].Pose)
            {
                Assert.Equal(4, matchedPose.Rows);
                Assert.Equal(4, matchedPose.Cols);
                Assert.Equal(MatType.CV_64FC1, matchedPose.Type());
            }

            using var icp = new ICP(
                iterations: 50,
                tolerance: 0.005f,
                rejectionScale: 2.5f,
                numberOfLevels: 4);
            var posesToRefine = results.Take(Math.Min(2, results.Length));
            Assert.Equal(0, icp.RegisterModelToScene(model, model, posesToRefine));
            Assert.True(double.IsFinite(results[0].Residual));
        }
        finally
        {
            foreach (var result in results)
                result.Dispose();
        }
    }

    [Fact]
    public void PointCloudHelpersReturnExpectedShapes()
    {
        using var pointCloud = CreatePointCloudWithNormals();
        Cv2.PpfMatch3D.ComputeBboxStd(
            pointCloud,
            out var xRange,
            out var yRange,
            out var zRange);
        using var sampled = Cv2.PpfMatch3D.SamplePCByQuantization(
            pointCloud,
            xRange,
            yRange,
            zRange,
            sampleStepRelative: 0.08f);
        using var uniform = Cv2.PpfMatch3D.SamplePCUniform(pointCloud, sampleStep: 4);
        using var translation = Mat.FromArray(new double[,]
        {
            { 1, 0, 0, 1.0 },
            { 0, 1, 0, -2.0 },
            { 0, 0, 1, 0.5 },
            { 0, 0, 0, 1 },
        });
        using var transformed = Cv2.PpfMatch3D.TransformPCPose(pointCloud, translation);
        using var noiseFree = Cv2.PpfMatch3D.AddNoisePC(pointCloud, 0);
        using var randomPose = Cv2.PpfMatch3D.GetRandomPose();

        Assert.InRange(sampled.Rows, 1, pointCloud.Rows);
        Assert.Equal(6, sampled.Cols);
        Assert.True(xRange.Item0 <= xRange.Item1);
        Assert.True(yRange.Item0 <= yRange.Item1);
        Assert.True(zRange.Item0 <= zRange.Item1);
        Assert.Equal(pointCloud.Rows / 4, uniform.Rows);
        Assert.Equal(pointCloud.Cols, uniform.Cols);
        Assert.Equal(pointCloud.Rows, transformed.Rows);
        Assert.Equal(pointCloud.Cols, transformed.Cols);
        Assert.Equal(pointCloud.Get<float>(0, 0) + 1.0f, transformed.Get<float>(0, 0), 5);
        Assert.Equal(pointCloud.Get<float>(0, 1) - 2.0f, transformed.Get<float>(0, 1), 5);
        Assert.Equal(pointCloud.Get<float>(0, 2) + 0.5f, transformed.Get<float>(0, 2), 5);
        Assert.Equal(pointCloud.Rows, noiseFree.Rows);
        Assert.Equal(pointCloud.Cols, noiseFree.Cols);
        Assert.Equal(4, randomPose.Rows);
        Assert.Equal(4, randomPose.Cols);
        Assert.Equal(MatType.CV_64FC1, randomPose.Type());
    }

    [Fact]
    public void PlyHelpersRoundTripPointCloud()
    {
        var pointCloudFile = Path.Combine(
            Path.GetTempPath(),
            $"opencvsharp_ppf_{Guid.NewGuid():N}.ply");
        var visibleNormalsFile = Path.Combine(
            Path.GetTempPath(),
            $"opencvsharp_ppf_normals_{Guid.NewGuid():N}.ply");

        try
        {
            using var pointCloud = CreatePointCloudWithNormals();
            Cv2.PpfMatch3D.WritePLY(pointCloud, pointCloudFile);
            using var restored = Cv2.PpfMatch3D.LoadPLYSimple(
                pointCloudFile,
                withNormals: true);

            Assert.Equal(pointCloud.Rows, restored.Rows);
            Assert.Equal(6, restored.Cols);
            Assert.Equal(MatType.CV_32FC1, restored.Type());

            Cv2.PpfMatch3D.WritePLYVisibleNormals(
                pointCloud,
                visibleNormalsFile);
            Assert.True(new FileInfo(visibleNormalsFile).Length > 0);
        }
        finally
        {
            File.Delete(pointCloudFile);
            File.Delete(visibleNormalsFile);
        }
    }

    private static Mat<float> CreatePointCloudWithNormals()
    {
        const int width = 24;
        const int height = 18;
        var points = new float[width * height, 6];
        var index = 0;

        for (var y = 0; y < height; y++)
        {
            var yf = (y - (height - 1) / 2f) / 5f;
            for (var x = 0; x < width; x++)
            {
                var xf = (x - (width - 1) / 2f) / 5f;
                var phase = 0.7f * xf + 0.2f * yf;
                var z = 0.15f * xf * xf + 0.08f * yf * yf +
                        0.03f * xf * yf + 0.1f * MathF.Sin(phase);
                var dzdx = 0.3f * xf + 0.03f * yf + 0.07f * MathF.Cos(phase);
                var dzdy = 0.16f * yf + 0.03f * xf + 0.02f * MathF.Cos(phase);
                var inverseLength = 1f / MathF.Sqrt(dzdx * dzdx + dzdy * dzdy + 1f);

                points[index, 0] = xf;
                points[index, 1] = yf;
                points[index, 2] = z;
                points[index, 3] = -dzdx * inverseLength;
                points[index, 4] = -dzdy * inverseLength;
                points[index, 5] = inverseLength;
                index++;
            }
        }

        return Mat.FromArray(points);
    }
}
