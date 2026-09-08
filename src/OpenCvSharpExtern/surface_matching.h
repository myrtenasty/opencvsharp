#pragma once

#if !defined(NO_CONTRIB) && defined(HAVE_OPENCV_SURFACE_MATCHING)

#include "include_opencv.h"
#include <opencv2/surface_matching.hpp>
#include <opencv2/surface_matching/ppf_helpers.hpp>
#include <memory>

namespace
{
cv::Mat getMatFromProxy(const interop::InputArrayProxy& proxy)
{
    return static_cast<const cv::_InputArray&>(InProxy(proxy)).getMat();
}

template<typename Matx>
Matx toMatx(const interop::InputArrayProxy& proxy)
{
    const cv::Mat mat = getMatFromProxy(proxy);
    CV_Assert(mat.rows == Matx::rows && mat.cols == Matx::cols && mat.type() == CV_64FC1);
    return Matx(mat);
}

template<typename InteropVec, typename CvVec>
InteropVec toInterop(const CvVec& value)
{
    InteropVec result{};
    std::copy(std::begin(value.val), std::end(value.val), std::begin(result.val));
    return result;
}
}

#pragma region Pose3DPtr vector

CVAPI(ExceptionStatus) surface_matching_vector_Pose3DPtr_new1(
    std::vector<cv::ppf_match_3d::Pose3DPtr>** returnValue)
{
    return cvTry([&] {
        *returnValue = new std::vector<cv::ppf_match_3d::Pose3DPtr>();
    });
}

CVAPI(ExceptionStatus) surface_matching_vector_Pose3DPtr_delete(
    std::vector<cv::ppf_match_3d::Pose3DPtr>* obj)
{
    return cvTry([&] {
        delete obj;
    });
}

CVAPI(ExceptionStatus) surface_matching_vector_Pose3DPtr_getSize(
    const std::vector<cv::ppf_match_3d::Pose3DPtr>* obj,
    size_t* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->size();
    });
}

CVAPI(ExceptionStatus) surface_matching_vector_Pose3DPtr_getAt(
    const std::vector<cv::ppf_match_3d::Pose3DPtr>* obj,
    size_t index,
    cv::Ptr<cv::ppf_match_3d::Pose3D>** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::Ptr<cv::ppf_match_3d::Pose3D>(obj->at(index));
    });
}

CVAPI(ExceptionStatus) surface_matching_vector_Pose3DPtr_pushBack(
    std::vector<cv::ppf_match_3d::Pose3DPtr>* obj,
    const cv::Ptr<cv::ppf_match_3d::Pose3D>* value)
{
    return cvTry([&] {
        CV_Assert(value != nullptr && !value->empty());
        obj->push_back(*value);
    });
}

#pragma endregion

#pragma region Pose3D

CVAPI(ExceptionStatus) surface_matching_Pose3D_new2(
    double alpha,
    size_t modelIndex,
    size_t numVotes,
    cv::Ptr<cv::ppf_match_3d::Pose3D>** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::Ptr<cv::ppf_match_3d::Pose3D>(
            cv::makePtr<cv::ppf_match_3d::Pose3D>(alpha, modelIndex, numVotes));
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_delete(
    cv::Ptr<cv::ppf_match_3d::Pose3D>* obj)
{
    return cvTry([&] {
        delete obj;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_get(
    cv::Ptr<cv::ppf_match_3d::Pose3D>* obj,
    cv::ppf_match_3d::Pose3D** returnValue)
{
    return cvTry([&] {
        *returnValue = obj->get();
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_updatePose1(
    cv::ppf_match_3d::Pose3D* obj,
    const interop::InputArrayProxy* newPose)
{
    return cvTry([&] {
        auto pose = toMatx<cv::Matx44d>(*newPose);
        obj->updatePose(pose);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_updatePose2(
    cv::ppf_match_3d::Pose3D* obj,
    const interop::InputArrayProxy* newRotation,
    interop::Vec3d newTranslation)
{
    return cvTry([&] {
        auto rotation = toMatx<cv::Matx33d>(*newRotation);
        cv::Vec3d translation(newTranslation.val);
        obj->updatePose(rotation, translation);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_updatePoseQuat(
    cv::ppf_match_3d::Pose3D* obj,
    interop::Vec4d quaternion,
    interop::Vec3d newTranslation)
{
    return cvTry([&] {
        cv::Vec4d quaternionValue(quaternion.val);
        cv::Vec3d translationValue(newTranslation.val);
        obj->updatePoseQuat(quaternionValue, translationValue);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_appendPose(
    cv::ppf_match_3d::Pose3D* obj,
    const interop::InputArrayProxy* incrementalPose)
{
    return cvTry([&] {
        auto pose = toMatx<cv::Matx44d>(*incrementalPose);
        obj->appendPose(pose);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_printPose(
    cv::ppf_match_3d::Pose3D* obj)
{
    return cvTry([&] {
        obj->printPose();
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_clone(
    cv::ppf_match_3d::Pose3D* obj,
    cv::Ptr<cv::ppf_match_3d::Pose3D>** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::Ptr<cv::ppf_match_3d::Pose3D>(obj->clone());
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_writePose(
    cv::ppf_match_3d::Pose3D* obj,
    const char* fileName,
    int* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->writePose(std::string(fileName));
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_readPose(
    cv::ppf_match_3d::Pose3D* obj,
    const char* fileName,
    int* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->readPose(std::string(fileName));
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getAlpha(
    const cv::ppf_match_3d::Pose3D* obj,
    double* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->alpha;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_setAlpha(
    cv::ppf_match_3d::Pose3D* obj,
    double value)
{
    return cvTry([&] {
        obj->alpha = value;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getResidual(
    const cv::ppf_match_3d::Pose3D* obj,
    double* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->residual;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_setResidual(
    cv::ppf_match_3d::Pose3D* obj,
    double value)
{
    return cvTry([&] {
        obj->residual = value;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getModelIndex(
    const cv::ppf_match_3d::Pose3D* obj,
    size_t* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->modelIndex;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_setModelIndex(
    cv::ppf_match_3d::Pose3D* obj,
    size_t value)
{
    return cvTry([&] {
        obj->modelIndex = value;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getNumVotes(
    const cv::ppf_match_3d::Pose3D* obj,
    size_t* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->numVotes;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_setNumVotes(
    cv::ppf_match_3d::Pose3D* obj,
    size_t value)
{
    return cvTry([&] {
        obj->numVotes = value;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getPose(
    const cv::ppf_match_3d::Pose3D* obj,
    cv::Mat** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::Mat(obj->pose, true);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getAngle(
    const cv::ppf_match_3d::Pose3D* obj,
    double* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->angle;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_setAngle(
    cv::ppf_match_3d::Pose3D* obj,
    double value)
{
    return cvTry([&] {
        obj->angle = value;
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getT(
    const cv::ppf_match_3d::Pose3D* obj,
    interop::Vec3d* returnValue)
{
    return cvTry([&] {
        *returnValue = toInterop<interop::Vec3d>(obj->t);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_setT(
    cv::ppf_match_3d::Pose3D* obj,
    interop::Vec3d value)
{
    return cvTry([&] {
        obj->t = cv::Vec3d(value.val);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_getQ(
    const cv::ppf_match_3d::Pose3D* obj,
    interop::Vec4d* returnValue)
{
    return cvTry([&] {
        *returnValue = toInterop<interop::Vec4d>(obj->q);
    });
}

CVAPI(ExceptionStatus) surface_matching_Pose3D_setQ(
    cv::ppf_match_3d::Pose3D* obj,
    interop::Vec4d value)
{
    return cvTry([&] {
        obj->q = cv::Vec4d(value.val);
    });
}

#pragma endregion

#pragma region PoseCluster3D

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_new1(
    cv::ppf_match_3d::PoseCluster3D** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::ppf_match_3d::PoseCluster3D();
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_new3(
    const cv::Ptr<cv::ppf_match_3d::Pose3D>* pose,
    int id,
    cv::ppf_match_3d::PoseCluster3D** returnValue)
{
    return cvTry([&] {
        CV_Assert(pose != nullptr && !pose->empty());
        *returnValue = new cv::ppf_match_3d::PoseCluster3D(*pose, id);
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_delete(
    cv::ppf_match_3d::PoseCluster3D* obj)
{
    return cvTry([&] {
        delete obj;
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_addPose(
    cv::ppf_match_3d::PoseCluster3D* obj,
    const cv::Ptr<cv::ppf_match_3d::Pose3D>* pose)
{
    return cvTry([&] {
        CV_Assert(pose != nullptr && !pose->empty());
        obj->addPose(*pose);
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_getPoses(
    const cv::ppf_match_3d::PoseCluster3D* obj,
    std::vector<cv::ppf_match_3d::Pose3DPtr>* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->poseList;
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_getNumVotes(
    const cv::ppf_match_3d::PoseCluster3D* obj,
    size_t* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->numVotes;
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_setNumVotes(
    cv::ppf_match_3d::PoseCluster3D* obj,
    size_t value)
{
    return cvTry([&] {
        obj->numVotes = value;
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_getId(
    const cv::ppf_match_3d::PoseCluster3D* obj,
    int* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->id;
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_setId(
    cv::ppf_match_3d::PoseCluster3D* obj,
    int value)
{
    return cvTry([&] {
        obj->id = value;
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_writePoseCluster(
    cv::ppf_match_3d::PoseCluster3D* obj,
    const char* fileName,
    int* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->writePoseCluster(std::string(fileName));
    });
}

CVAPI(ExceptionStatus) surface_matching_PoseCluster3D_readPoseCluster(
    cv::ppf_match_3d::PoseCluster3D* obj,
    const char* fileName,
    int* returnValue)
{
    return cvTry([&] {
        // OpenCV 5's FILE* overload closes a successfully read stream, while its
        // filename overload closes the same stream a second time. Open it here so
        // the wrapper can avoid that double close and still preserve native parsing.
        std::unique_ptr<FILE, int(*)(FILE*)> file(fopen(fileName, "rb"), &fclose);
        if (!file)
        {
            *returnValue = -1;
            return;
        }

        *returnValue = obj->readPoseCluster(file.get());
        if (*returnValue == 0)
            file.release();
    });
}

#pragma endregion

#pragma region PPF3DDetector

CVAPI(ExceptionStatus) surface_matching_PPF3DDetector_new1(
    cv::ppf_match_3d::PPF3DDetector** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::ppf_match_3d::PPF3DDetector();
    });
}

CVAPI(ExceptionStatus) surface_matching_PPF3DDetector_new2(
    double relativeSamplingStep,
    double relativeDistanceStep,
    double numAngles,
    cv::ppf_match_3d::PPF3DDetector** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::ppf_match_3d::PPF3DDetector(
            relativeSamplingStep,
            relativeDistanceStep,
            numAngles);
    });
}

CVAPI(ExceptionStatus) surface_matching_PPF3DDetector_delete(
    cv::ppf_match_3d::PPF3DDetector* obj)
{
    return cvTry([&] {
        delete obj;
    });
}

CVAPI(ExceptionStatus) surface_matching_PPF3DDetector_setSearchParams(
    cv::ppf_match_3d::PPF3DDetector* obj,
    double positionThreshold,
    double rotationThreshold,
    int useWeightedClustering)
{
    return cvTry([&] {
        obj->setSearchParams(
            positionThreshold,
            rotationThreshold,
            useWeightedClustering != 0);
    });
}

CVAPI(ExceptionStatus) surface_matching_PPF3DDetector_trainModel(
    cv::ppf_match_3d::PPF3DDetector* obj,
    const interop::InputArrayProxy* model)
{
    return cvTry([&] {
        obj->trainModel(
            getMatFromProxy(*model));
    });
}

CVAPI(ExceptionStatus) surface_matching_PPF3DDetector_match(
    cv::ppf_match_3d::PPF3DDetector* obj,
    const interop::InputArrayProxy* scene,
    std::vector<cv::ppf_match_3d::Pose3DPtr>* results,
    double relativeSceneSampleStep,
    double relativeSceneDistance)
{
    return cvTry([&] {
        results->clear();
        obj->match(
            getMatFromProxy(*scene),
            *results,
            relativeSceneSampleStep,
            relativeSceneDistance);
    });
}

#pragma endregion

#pragma region ICP

CVAPI(ExceptionStatus) surface_matching_ICP_new1(cv::ppf_match_3d::ICP** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::ppf_match_3d::ICP();
    });
}

CVAPI(ExceptionStatus) surface_matching_ICP_new2(
    int iterations,
    float tolerance,
    float rejectionScale,
    int numLevels,
    int sampleType,
    int numMaxCorrespondences,
    cv::ppf_match_3d::ICP** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::ppf_match_3d::ICP(
            iterations,
            tolerance,
            rejectionScale,
            numLevels,
            sampleType,
            numMaxCorrespondences);
    });
}

CVAPI(ExceptionStatus) surface_matching_ICP_delete(cv::ppf_match_3d::ICP* obj)
{
    return cvTry([&] {
        delete obj;
    });
}

CVAPI(ExceptionStatus) surface_matching_ICP_registerModelToScene(
    cv::ppf_match_3d::ICP* obj,
    const interop::InputArrayProxy* sourcePointCloud,
    const interop::InputArrayProxy* destinationPointCloud,
    double* residual,
    cv::Mat** pose,
    int* returnValue)
{
    return cvTry([&] {
        cv::Matx44d poseValue;
        *returnValue = obj->registerModelToScene(
            getMatFromProxy(*sourcePointCloud),
            getMatFromProxy(*destinationPointCloud),
            *residual,
            poseValue);
        *pose = new cv::Mat(poseValue, true);
    });
}

CVAPI(ExceptionStatus) surface_matching_ICP_registerModelToScene_Poses(
    cv::ppf_match_3d::ICP* obj,
    const interop::InputArrayProxy* sourcePointCloud,
    const interop::InputArrayProxy* destinationPointCloud,
    std::vector<cv::ppf_match_3d::Pose3DPtr>* poses,
    int* returnValue)
{
    return cvTry([&] {
        *returnValue = obj->registerModelToScene(
            getMatFromProxy(*sourcePointCloud),
            getMatFromProxy(*destinationPointCloud),
            *poses);
    });
}

#pragma endregion

#pragma region PPF helpers

CVAPI(ExceptionStatus) surface_matching_loadPLYSimple(
    const char* fileName,
    int withNormals,
    cv::Mat** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::Mat(
            cv::ppf_match_3d::loadPLYSimple(fileName, withNormals));
    });
}

CVAPI(ExceptionStatus) surface_matching_writePLY(
    const interop::InputArrayProxy* pointCloud,
    const char* fileName)
{
    return cvTry([&] {
        cv::ppf_match_3d::writePLY(
            getMatFromProxy(*pointCloud),
            fileName);
    });
}

CVAPI(ExceptionStatus) surface_matching_writePLYVisibleNormals(
    const interop::InputArrayProxy* pointCloud,
    const char* fileName)
{
    return cvTry([&] {
        cv::ppf_match_3d::writePLYVisibleNormals(
            getMatFromProxy(*pointCloud),
            fileName);
    });
}

CVAPI(ExceptionStatus) surface_matching_samplePCUniform(
    const interop::InputArrayProxy* pointCloud,
    int sampleStep,
    cv::Mat** returnValue)
{
    return cvTry([&] {
        CV_Assert(sampleStep > 0);
        *returnValue = new cv::Mat(cv::ppf_match_3d::samplePCUniform(
            getMatFromProxy(*pointCloud),
            sampleStep));
    });
}

CVAPI(ExceptionStatus) surface_matching_computeBboxStd(
    const interop::InputArrayProxy* pointCloud,
    interop::Vec2f* xRange,
    interop::Vec2f* yRange,
    interop::Vec2f* zRange)
{
    return cvTry([&] {
        const cv::Mat pointCloudMat = getMatFromProxy(*pointCloud);
        CV_Assert(pointCloudMat.rows > 0 && pointCloudMat.cols >= 3);

        cv::Vec2f xRangeValue;
        cv::Vec2f yRangeValue;
        cv::Vec2f zRangeValue;
        cv::ppf_match_3d::computeBboxStd(
            pointCloudMat,
            xRangeValue,
            yRangeValue,
            zRangeValue);
        *xRange = toInterop<interop::Vec2f>(xRangeValue);
        *yRange = toInterop<interop::Vec2f>(yRangeValue);
        *zRange = toInterop<interop::Vec2f>(zRangeValue);
    });
}

CVAPI(ExceptionStatus) surface_matching_samplePCByQuantization(
    const interop::InputArrayProxy* pointCloud,
    interop::Vec2f xRange,
    interop::Vec2f yRange,
    interop::Vec2f zRange,
    float sampleStepRelative,
    int weightByCenter,
    cv::Mat** returnValue)
{
    return cvTry([&] {
        cv::Vec2f xRangeValue(xRange.val);
        cv::Vec2f yRangeValue(yRange.val);
        cv::Vec2f zRangeValue(zRange.val);
        *returnValue = new cv::Mat(cv::ppf_match_3d::samplePCByQuantization(
            getMatFromProxy(*pointCloud),
            xRangeValue,
            yRangeValue,
            zRangeValue,
            sampleStepRelative,
            weightByCenter));
    });
}

CVAPI(ExceptionStatus) surface_matching_transformPCPose(
    const interop::InputArrayProxy* pointCloud,
    const interop::InputArrayProxy* pose,
    cv::Mat** returnValue)
{
    return cvTry([&] {
        const auto poseValue = toMatx<cv::Matx44d>(*pose);
        *returnValue = new cv::Mat(cv::ppf_match_3d::transformPCPose(
            getMatFromProxy(*pointCloud),
            poseValue));
    });
}

CVAPI(ExceptionStatus) surface_matching_getRandomPose(cv::Mat** returnValue)
{
    return cvTry([&] {
        cv::Matx44d pose;
        cv::ppf_match_3d::getRandomPose(pose);
        *returnValue = new cv::Mat(pose, true);
    });
}

CVAPI(ExceptionStatus) surface_matching_addNoisePC(
    const interop::InputArrayProxy* pointCloud,
    double scale,
    cv::Mat** returnValue)
{
    return cvTry([&] {
        *returnValue = new cv::Mat(cv::ppf_match_3d::addNoisePC(
            getMatFromProxy(*pointCloud),
            scale));
    });
}

CVAPI(ExceptionStatus) surface_matching_computeNormalsPC3d(
    const interop::InputArrayProxy* pointCloud,
    const interop::OutputArrayProxy* pointCloudWithNormals,
    int numberOfNeighbors,
    int flipViewpoint,
    interop::Vec3f viewpoint,
    int* returnValue)
{
    return cvTry([&] {
        cv::Mat result;
        const cv::Vec3f viewpointValue(viewpoint.val);
        *returnValue = cv::ppf_match_3d::computeNormalsPC3d(
            getMatFromProxy(*pointCloud),
            result,
            numberOfNeighbors,
            flipViewpoint != 0,
            viewpointValue);
        result.copyTo(OutProxy(*pointCloudWithNormals));
    });
}

#pragma endregion

#endif
