using OpenCvSharp.Internal;
using OpenCvSharp.Internal.Vectors;

namespace OpenCvSharp.PpfMatch3D;

/// <summary>
/// Groups pose hypotheses that contribute to the same transformation.
/// </summary>
public sealed class PoseCluster3D : CvObject
{
    private PoseCluster3D(IntPtr ptr)
    {
        SetSafeHandle(new OpenCvPtrSafeHandle(
            ptr,
            ownsHandle: true,
            releaseAction: static p => NativeMethods.HandleException(
                NativeMethods.surface_matching_PoseCluster3D_delete(p))));
    }

    /// <summary>
    /// Creates an empty pose cluster.
    /// </summary>
    public PoseCluster3D()
        : this(CreateEmpty())
    {
    }

    /// <summary>
    /// Creates a pose cluster containing one pose.
    /// </summary>
    /// <param name="pose">Initial pose.</param>
    public PoseCluster3D(Pose3D pose)
        : this(pose, id: 0)
    {
    }

    /// <summary>
    /// Creates a pose cluster containing one pose and assigns its identifier.
    /// </summary>
    /// <param name="pose">Initial pose.</param>
    /// <param name="id">Cluster identifier.</param>
    public PoseCluster3D(Pose3D pose, int id)
        : this(CreateNative(pose, id))
    {
    }

    private static IntPtr CreateEmpty()
    {
        NativeMethods.HandleException(
            NativeMethods.surface_matching_PoseCluster3D_new1(out var ptr));
        return ptr;
    }

    private static IntPtr CreateNative(Pose3D pose, int id)
    {
        ArgumentNullException.ThrowIfNull(pose);
        pose.ThrowIfDisposed();

        NativeMethods.HandleException(
            NativeMethods.surface_matching_PoseCluster3D_new3(
                pose.SmartPtr,
                id,
                out var ptr));
        GC.KeepAlive(pose);
        return ptr;
    }

    /// <summary>
    /// Total number of votes represented by the cluster.
    /// </summary>
    public nuint NumVotes
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_PoseCluster3D_getNumVotes(
                    Handle,
                    out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_PoseCluster3D_setNumVotes(
                    Handle,
                    value));
        }
    }

    /// <summary>
    /// Cluster identifier.
    /// </summary>
    public int Id
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_PoseCluster3D_getId(
                    Handle,
                    out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_PoseCluster3D_setId(
                    Handle,
                    value));
        }
    }

    /// <summary>
    /// Adds a pose to the cluster and adds its votes to <see cref="NumVotes"/>.
    /// </summary>
    /// <param name="pose">Pose to add.</param>
    public void AddPose(Pose3D pose)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(pose);
        pose.ThrowIfDisposed();

        NativeMethods.HandleException(
            NativeMethods.surface_matching_PoseCluster3D_addPose(
                Handle,
                pose.SmartPtr));
        GC.KeepAlive(pose);
    }

    /// <summary>
    /// Gets independently disposable wrappers for the poses in this cluster.
    /// </summary>
    /// <returns>Poses that share native ownership with the cluster. The caller must dispose every returned pose.</returns>
    public Pose3D[] GetPoses()
    {
        ThrowIfDisposed();

        using var poses = new VectorOfPose3D();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_PoseCluster3D_getPoses(
                Handle,
                poses.Handle));
        return poses.ToArray();
    }

    /// <summary>
    /// Writes this cluster in OpenCV's binary pose-cluster format.
    /// </summary>
    /// <param name="fileName">Destination file name.</param>
    /// <returns>Zero on success; otherwise -1.</returns>
    public int WritePoseCluster(string fileName)
    {
        ThrowIfDisposed();
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName));

        NativeMethods.HandleException(
            NativeMethods.surface_matching_PoseCluster3D_writePoseCluster(
                Handle,
                fileName,
                out var returnValue));
        return returnValue;
    }

    /// <summary>
    /// Reads this cluster from OpenCV's binary pose-cluster format.
    /// </summary>
    /// <param name="fileName">Source file name.</param>
    /// <returns>Zero on success; otherwise -1.</returns>
    public int ReadPoseCluster(string fileName)
    {
        ThrowIfDisposed();
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName));

        NativeMethods.HandleException(
            NativeMethods.surface_matching_PoseCluster3D_readPoseCluster(
                Handle,
                fileName,
                out var returnValue));
        return returnValue;
    }
}
