using OpenCvSharp.PpfMatch3D;

namespace OpenCvSharp.Internal.Vectors;

/// <summary>
/// Native std::vector of cv::ppf_match_3d::Pose3DPtr values.
/// </summary>
internal sealed class VectorOfPose3D : CvObject, IStdVector<Pose3D>
{
    /// <summary>
    /// Creates an empty vector.
    /// </summary>
    public VectorOfPose3D()
    {
        NativeMethods.HandleException(
            NativeMethods.surface_matching_vector_Pose3DPtr_new1(out var ptr));
        SetSafeHandle(new OpenCvPtrSafeHandle(
            ptr,
            ownsHandle: true,
            releaseAction: static p => NativeMethods.HandleException(
                NativeMethods.surface_matching_vector_Pose3DPtr_delete(p))));
    }

    /// <summary>
    /// Creates a vector that shares ownership of the supplied poses.
    /// </summary>
    /// <param name="poses">Poses to add.</param>
    public VectorOfPose3D(IEnumerable<Pose3D> poses)
        : this()
    {
        ArgumentNullException.ThrowIfNull(poses);

        try
        {
            foreach (var pose in poses)
            {
                ArgumentNullException.ThrowIfNull(pose);
                pose.ThrowIfDisposed();
                NativeMethods.HandleException(
                    NativeMethods.surface_matching_vector_Pose3DPtr_pushBack(
                        Handle,
                        pose.SmartPtr));
                GC.KeepAlive(pose);
            }
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    /// <summary>
    /// Number of poses in the vector.
    /// </summary>
    public int Size
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_vector_Pose3DPtr_getSize(
                    Handle,
                    out var size));
            return checked((int)size);
        }
    }

    /// <summary>
    /// Converts the vector to independently disposable managed wrappers.
    /// </summary>
    /// <returns>Pose wrappers that share ownership of the native poses.</returns>
    public Pose3D[] ToArray()
    {
        var result = new Pose3D[Size];
        var initialized = 0;
        try
        {
            for (; initialized < result.Length; initialized++)
            {
                NativeMethods.HandleException(
                    NativeMethods.surface_matching_vector_Pose3DPtr_getAt(
                        Handle,
                        (nuint)initialized,
                        out var pose));
                result[initialized] = new Pose3D(pose);
            }
            return result;
        }
        catch
        {
            for (var i = 0; i < initialized; i++)
                result[i].Dispose();
            throw;
        }
    }
}
