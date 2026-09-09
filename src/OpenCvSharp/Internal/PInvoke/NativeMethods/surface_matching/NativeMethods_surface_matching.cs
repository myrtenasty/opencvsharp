using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable 1591
#pragma warning disable CA1401
#pragma warning disable CA2101
#pragma warning disable IDE1006

namespace OpenCvSharp.Internal;

static partial class NativeMethods
{
    #region Pose3DPtr vector

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_vector_Pose3DPtr_new1(out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_vector_Pose3DPtr_delete(IntPtr obj);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_vector_Pose3DPtr_getSize(
        OpenCvSafeHandle obj,
        out nuint returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_vector_Pose3DPtr_getAt(
        OpenCvSafeHandle obj,
        nuint index,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_vector_Pose3DPtr_pushBack(
        OpenCvSafeHandle obj,
        IntPtr value);

    #endregion

    #region Pose3D

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_new2(
        double alpha,
        nuint modelIndex,
        nuint numVotes,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_delete(IntPtr obj);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_get(
        IntPtr obj,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_updatePose1(
        OpenCvSafeHandle obj,
        in InputArrayProxy newPose);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_updatePose2(
        OpenCvSafeHandle obj,
        in InputArrayProxy newRotation,
        Vec3d newTranslation);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_updatePoseQuat(
        OpenCvSafeHandle obj,
        Vec4d quaternion,
        Vec3d newTranslation);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_appendPose(
        OpenCvSafeHandle obj,
        in InputArrayProxy incrementalPose);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_printPose(OpenCvSafeHandle obj);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_clone(
        OpenCvSafeHandle obj,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_writePose(
        OpenCvSafeHandle obj,
        [MarshalAs(UnmanagedType.LPStr)] string fileName,
        out int returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_readPose(
        OpenCvSafeHandle obj,
        [MarshalAs(UnmanagedType.LPStr)] string fileName,
        out int returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getAlpha(
        OpenCvSafeHandle obj,
        out double returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_setAlpha(
        OpenCvSafeHandle obj,
        double value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getResidual(
        OpenCvSafeHandle obj,
        out double returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_setResidual(
        OpenCvSafeHandle obj,
        double value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getModelIndex(
        OpenCvSafeHandle obj,
        out nuint returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_setModelIndex(
        OpenCvSafeHandle obj,
        nuint value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getNumVotes(
        OpenCvSafeHandle obj,
        out nuint returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_setNumVotes(
        OpenCvSafeHandle obj,
        nuint value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getPose(
        OpenCvSafeHandle obj,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getAngle(
        OpenCvSafeHandle obj,
        out double returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_setAngle(
        OpenCvSafeHandle obj,
        double value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getT(
        OpenCvSafeHandle obj,
        out Vec3d returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_setT(
        OpenCvSafeHandle obj,
        Vec3d value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_getQ(
        OpenCvSafeHandle obj,
        out Vec4d returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_Pose3D_setQ(
        OpenCvSafeHandle obj,
        Vec4d value);

    #endregion

    #region PoseCluster3D

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_new1(out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_new3(
        IntPtr pose,
        int id,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_delete(IntPtr obj);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_addPose(
        OpenCvSafeHandle obj,
        IntPtr pose);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_getPoses(
        OpenCvSafeHandle obj,
        OpenCvSafeHandle returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_getNumVotes(
        OpenCvSafeHandle obj,
        out nuint returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_setNumVotes(
        OpenCvSafeHandle obj,
        nuint value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_getId(
        OpenCvSafeHandle obj,
        out int returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_setId(
        OpenCvSafeHandle obj,
        int value);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_writePoseCluster(
        OpenCvSafeHandle obj,
        [MarshalAs(UnmanagedType.LPStr)] string fileName,
        out int returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PoseCluster3D_readPoseCluster(
        OpenCvSafeHandle obj,
        [MarshalAs(UnmanagedType.LPStr)] string fileName,
        out int returnValue);

    #endregion

    #region PPF3DDetector

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_new1(out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_new2(
        double relativeSamplingStep,
        double relativeDistanceStep,
        double numAngles,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_delete(IntPtr obj);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_setSearchParams(
        OpenCvSafeHandle obj,
        double positionThreshold,
        double rotationThreshold,
        int useWeightedClustering);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_trainModel(
        OpenCvSafeHandle obj,
        in InputArrayProxy model);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_match(
        OpenCvSafeHandle obj,
        in InputArrayProxy scene,
        OpenCvSafeHandle results,
        double relativeSceneSampleStep,
        double relativeSceneDistance);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_trainPrepared(
        OpenCvSafeHandle obj,
        in InputArrayProxy modelPoints,
        double distanceBinMetres,
        double numberOfAngles);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_PPF3DDetector_matchPrepared(
        OpenCvSafeHandle obj,
        in InputArrayProxy scenePoints,
        OpenCvSafeHandle results,
        double sceneReferenceFraction,
        double distanceBinMetres);

    #endregion

    #region ICP

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_ICP_new1(out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_ICP_new2(
        int iterations,
        float tolerance,
        float rejectionScale,
        int numberOfLevels,
        int samplingType,
        int numberOfMaxCorrespondences,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_ICP_delete(IntPtr obj);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_ICP_registerModelToScene(
        OpenCvSafeHandle obj,
        in InputArrayProxy sourcePointCloud,
        in InputArrayProxy destinationPointCloud,
        out double residual,
        out IntPtr pose,
        out int returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_ICP_registerModelToScene_Poses(
        OpenCvSafeHandle obj,
        in InputArrayProxy sourcePointCloud,
        in InputArrayProxy destinationPointCloud,
        OpenCvSafeHandle poses,
        out int returnValue);

    #endregion

    #region PPF helpers

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_loadPLYSimple(
        [MarshalAs(UnmanagedType.LPStr)] string fileName,
        int withNormals,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_writePLY(
        in InputArrayProxy pointCloud,
        [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_writePLYVisibleNormals(
        in InputArrayProxy pointCloud,
        [MarshalAs(UnmanagedType.LPStr)] string fileName);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_samplePCUniform(
        in InputArrayProxy pointCloud,
        int sampleStep,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_computeBboxStd(
        in InputArrayProxy pointCloud,
        out Vec2f xRange,
        out Vec2f yRange,
        out Vec2f zRange);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_samplePCByQuantization(
        in InputArrayProxy pointCloud,
        Vec2f xRange,
        Vec2f yRange,
        Vec2f zRange,
        float sampleStepRelative,
        int weightByCenter,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_transformPCPose(
        in InputArrayProxy pointCloud,
        in InputArrayProxy pose,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_getRandomPose(out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_addNoisePC(
        in InputArrayProxy pointCloud,
        double scale,
        out IntPtr returnValue);

    [LibraryImport(DllExtern), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ExceptionStatus surface_matching_computeNormalsPC3d(
        in InputArrayProxy pointCloud,
        in OutputArrayProxy pointCloudWithNormals,
        int numberOfNeighbors,
        int flipViewpoint,
        Vec3f viewpoint,
        out int returnValue);

    #endregion
}
