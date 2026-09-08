using OpenCvSharp.Internal;
using OpenCvSharp.Internal.Vectors;

namespace OpenCvSharp.PpfMatch3D;

/// <summary>
/// Trains and matches 3D point-pair-feature models.
/// </summary>
/// <remarks>
/// Implements the point-pair-feature algorithm described in B. Drost et al.,
/// "Model Globally, Match Locally: Efficient and Robust 3D Object Recognition."
/// </remarks>
public sealed class PPF3DDetector : CvObject
{
    private PPF3DDetector(IntPtr ptr)
    {
        SetSafeHandle(new OpenCvPtrSafeHandle(
            ptr,
            ownsHandle: true,
            releaseAction: static p => NativeMethods.HandleException(
                NativeMethods.surface_matching_PPF3DDetector_delete(p))));
    }

    /// <summary>
    /// Creates a detector with the OpenCV defaults.
    /// </summary>
    public PPF3DDetector()
        : this(CreateDefault())
    {
    }

    /// <summary>
    /// Creates a detector.
    /// </summary>
    /// <param name="relativeSamplingStep">Model sampling distance relative to the model diameter.</param>
    /// <param name="relativeDistanceStep">Point-pair distance discretization relative to the model diameter.</param>
    /// <param name="numberOfAngles">Number of subdivisions used to discretize point-pair orientations.</param>
    public PPF3DDetector(
        double relativeSamplingStep,
        double relativeDistanceStep = 0.05,
        double numberOfAngles = 30)
        : this(CreateNative(relativeSamplingStep, relativeDistanceStep, numberOfAngles))
    {
    }

    private static IntPtr CreateDefault()
    {
        NativeMethods.HandleException(
            NativeMethods.surface_matching_PPF3DDetector_new1(out var ptr));
        return ptr;
    }

    private static IntPtr CreateNative(
        double relativeSamplingStep,
        double relativeDistanceStep,
        double numberOfAngles)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(relativeSamplingStep);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(relativeDistanceStep);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfAngles);

        NativeMethods.HandleException(
            NativeMethods.surface_matching_PPF3DDetector_new2(
                relativeSamplingStep,
                relativeDistanceStep,
                numberOfAngles,
                out var ptr));
        return ptr;
    }

    /// <summary>
    /// Configures thresholds used to cluster similar pose hypotheses.
    /// </summary>
    /// <param name="positionThreshold">Translation similarity threshold. A negative value uses the model sampling step.</param>
    /// <param name="rotationThreshold">Rotation similarity threshold in radians. A negative value uses the detector's angular discretization.</param>
    /// <param name="useWeightedClustering">Whether pose averaging is weighted by vote count.</param>
    public void SetSearchParams(
        double positionThreshold = -1,
        double rotationThreshold = -1,
        bool useWeightedClustering = false)
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_PPF3DDetector_setSearchParams(
                Handle,
                positionThreshold,
                rotationThreshold,
                useWeightedClustering ? 1 : 0));
    }

    /// <summary>
    /// Trains the detector with a point-cloud model.
    /// </summary>
    /// <param name="model">Model point cloud as an N-by-6 CV_32F matrix containing XYZ coordinates and normals.</param>
    public void TrainModel(InputArray model)
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_PPF3DDetector_trainModel(
                Handle,
                model.Proxy));
        GC.KeepAlive(model.Source);
    }

    /// <summary>
    /// Matches the trained model against a scene.
    /// </summary>
    /// <param name="scene">Scene point cloud as an N-by-6 CV_32F matrix containing XYZ coordinates and normals.</param>
    /// <param name="relativeSceneSampleStep">Fraction controlling how many sampled scene reference points are used. The value must be in the range (0, 1].</param>
    /// <param name="relativeSceneDistance">Scene sampling distance relative to the trained model diameter.</param>
    /// <returns>Pose hypotheses ordered by the native matcher. The caller must dispose every returned pose.</returns>
    public Pose3D[] Match(
        InputArray scene,
        double relativeSceneSampleStep = 1.0 / 5.0,
        double relativeSceneDistance = 0.03)
    {
        ThrowIfDisposed();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(relativeSceneSampleStep);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(relativeSceneSampleStep, 1);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(relativeSceneDistance);

        using var results = new VectorOfPose3D();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_PPF3DDetector_match(
                Handle,
                scene.Proxy,
                results.Handle,
                relativeSceneSampleStep,
                relativeSceneDistance));
        GC.KeepAlive(scene.Source);
        return results.ToArray();
    }
}
