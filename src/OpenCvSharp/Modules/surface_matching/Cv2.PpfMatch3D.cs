using OpenCvSharp.Internal;

namespace OpenCvSharp;

public static partial class Cv2
{
    /// <summary>
    /// Surface-matching helpers in the native cv::ppf_match_3d namespace.
    /// </summary>
    public static class PpfMatch3D
    {
        /// <summary>
        /// Loads a point cloud from a PLY file.
        /// </summary>
        /// <param name="fileName">PLY file to read.</param>
        /// <param name="withNormals">Whether normal vectors in the file should be loaded.</param>
        /// <returns>The loaded point cloud.</returns>
        public static Mat LoadPLYSimple(string fileName, bool withNormals = false)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            NativeMethods.HandleException(
                NativeMethods.surface_matching_loadPLYSimple(
                    fileName,
                    withNormals ? 1 : 0,
                    out var returnValue));
            return new Mat(returnValue);
        }

        /// <summary>
        /// Writes a point cloud to a PLY file.
        /// </summary>
        /// <param name="pointCloud">Point cloud to write.</param>
        /// <param name="fileName">Destination PLY file.</param>
        public static void WritePLY(InputArray pointCloud, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            NativeMethods.HandleException(
                NativeMethods.surface_matching_writePLY(
                    pointCloud.Proxy,
                    fileName));
            GC.KeepAlive(pointCloud.Source);
        }

        /// <summary>
        /// Writes a point cloud and visible normal-vector tips to a PLY file for debugging.
        /// </summary>
        /// <param name="pointCloud">Point cloud with normals as an N-by-6 CV_32F matrix.</param>
        /// <param name="fileName">Destination PLY file.</param>
        public static void WritePLYVisibleNormals(InputArray pointCloud, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            NativeMethods.HandleException(
                NativeMethods.surface_matching_writePLYVisibleNormals(
                    pointCloud.Proxy,
                    fileName));
            GC.KeepAlive(pointCloud.Source);
        }

        /// <summary>
        /// Samples a point cloud by taking every Nth point.
        /// </summary>
        /// <param name="pointCloud">Input point cloud.</param>
        /// <param name="sampleStep">Keep one point every <paramref name="sampleStep"/> rows.</param>
        /// <returns>The uniformly subsampled point cloud.</returns>
        public static Mat SamplePCUniform(InputArray pointCloud, int sampleStep)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sampleStep);
            NativeMethods.HandleException(
                NativeMethods.surface_matching_samplePCUniform(
                    pointCloud.Proxy,
                    sampleStep,
                    out var returnValue));
            GC.KeepAlive(pointCloud.Source);
            return new Mat(returnValue);
        }

        /// <summary>
        /// Computes the axis-aligned bounding box of a point cloud.
        /// </summary>
        /// <param name="pointCloud">Input point cloud. At least one XYZ row is required.</param>
        /// <param name="xRange">Output minimum and maximum X coordinates.</param>
        /// <param name="yRange">Output minimum and maximum Y coordinates.</param>
        /// <param name="zRange">Output minimum and maximum Z coordinates.</param>
        public static void ComputeBboxStd(
            InputArray pointCloud,
            out Vec2f xRange,
            out Vec2f yRange,
            out Vec2f zRange)
        {
            NativeMethods.HandleException(
                NativeMethods.surface_matching_computeBboxStd(
                    pointCloud.Proxy,
                    out xRange,
                    out yRange,
                    out zRange));
            GC.KeepAlive(pointCloud.Source);
        }

        /// <summary>
        /// Samples a point cloud by spatial quantization.
        /// </summary>
        /// <param name="pointCloud">Input point cloud.</param>
        /// <param name="xRange">Minimum and maximum X coordinates of the model bounding box.</param>
        /// <param name="yRange">Minimum and maximum Y coordinates of the model bounding box.</param>
        /// <param name="zRange">Minimum and maximum Z coordinates of the model bounding box.</param>
        /// <param name="sampleStepRelative">Minimum point distance relative to the model diameter.</param>
        /// <param name="weightByCenter">Whether quantized points are weighted by their distance to the origin.</param>
        /// <returns>The sampled point cloud.</returns>
        public static Mat SamplePCByQuantization(
            InputArray pointCloud,
            Vec2f xRange,
            Vec2f yRange,
            Vec2f zRange,
            float sampleStepRelative,
            bool weightByCenter = false)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sampleStepRelative);
            NativeMethods.HandleException(
                NativeMethods.surface_matching_samplePCByQuantization(
                    pointCloud.Proxy,
                    xRange,
                    yRange,
                    zRange,
                    sampleStepRelative,
                    weightByCenter ? 1 : 0,
                    out var returnValue));
            GC.KeepAlive(pointCloud.Source);
            return new Mat(returnValue);
        }

        /// <summary>
        /// Transforms a point cloud by a homogeneous pose matrix.
        /// </summary>
        /// <param name="pointCloud">Input N-by-3 or N-by-6 CV_32F point cloud. Normals, when present, are rotated as well.</param>
        /// <param name="pose">4-by-4 CV_64F homogeneous pose matrix.</param>
        /// <returns>The transformed point cloud.</returns>
        public static Mat TransformPCPose(InputArray pointCloud, InputArray pose)
        {
            NativeMethods.HandleException(
                NativeMethods.surface_matching_transformPCPose(
                    pointCloud.Proxy,
                    pose.Proxy,
                    out var returnValue));
            GC.KeepAlive(pointCloud.Source);
            GC.KeepAlive(pose.Source);
            return new Mat(returnValue);
        }

        /// <summary>
        /// Generates a random 4-by-4 CV_64F homogeneous pose matrix.
        /// </summary>
        /// <returns>The generated pose.</returns>
        public static Mat GetRandomPose()
        {
            NativeMethods.HandleException(
                NativeMethods.surface_matching_getRandomPose(out var returnValue));
            return new Mat(returnValue);
        }

        /// <summary>
        /// Adds uniformly distributed noise to a point cloud.
        /// </summary>
        /// <param name="pointCloud">Input CV_32F point cloud.</param>
        /// <param name="scale">Noise scale.</param>
        /// <returns>The noisy point cloud.</returns>
        public static Mat AddNoisePC(InputArray pointCloud, double scale)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(scale);
            NativeMethods.HandleException(
                NativeMethods.surface_matching_addNoisePC(
                    pointCloud.Proxy,
                    scale,
                    out var returnValue));
            GC.KeepAlive(pointCloud.Source);
            return new Mat(returnValue);
        }

        /// <summary>
        /// Computes surface normals for a point cloud using local plane fitting.
        /// </summary>
        /// <param name="pointCloud">Input point cloud as an N-by-3 CV_32F matrix.</param>
        /// <param name="pointCloudWithNormals">Output N-by-6 CV_32F matrix containing XYZ coordinates and normals.</param>
        /// <param name="numberOfNeighbors">Number of neighboring points used for each local plane fit.</param>
        /// <param name="flipViewpoint">Whether normals should be oriented toward <paramref name="viewpoint"/>.</param>
        /// <param name="viewpoint">Viewpoint used when <paramref name="flipViewpoint"/> is true.</param>
        /// <returns>One on success, matching the OpenCV implementation.</returns>
        public static int ComputeNormalsPC3d(
            InputArray pointCloud,
            OutputArray pointCloudWithNormals,
            int numberOfNeighbors = 6,
            bool flipViewpoint = false,
            Vec3f viewpoint = default)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfNeighbors);

            NativeMethods.HandleException(
                NativeMethods.surface_matching_computeNormalsPC3d(
                    pointCloud.Proxy,
                    pointCloudWithNormals.Proxy,
                    numberOfNeighbors,
                    flipViewpoint ? 1 : 0,
                    viewpoint,
                    out var returnValue));

            GC.KeepAlive(pointCloud.Source);
            GC.KeepAlive(pointCloudWithNormals.Source);
            return returnValue;
        }
    }
}
