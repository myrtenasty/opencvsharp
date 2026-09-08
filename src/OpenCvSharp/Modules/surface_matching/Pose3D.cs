using OpenCvSharp.Internal;

namespace OpenCvSharp.PpfMatch3D;

/// <summary>
/// Stores a 3D pose in matrix, translation, and quaternion representations.
/// </summary>
public sealed class Pose3D : CvPtrObject
{
    /// <summary>
    /// Creates an empty pose.
    /// </summary>
    public Pose3D()
        : this(0.0)
    {
    }

    /// <summary>
    /// Creates a pose with voting metadata.
    /// </summary>
    /// <param name="alpha">Rotation around the reference point normal.</param>
    /// <param name="modelIndex">Index of the reference point in the sampled model.</param>
    /// <param name="numVotes">Number of votes received by the pose.</param>
    public Pose3D(double alpha, nuint modelIndex = 0, nuint numVotes = 0)
        : this(CreateNative(alpha, modelIndex, numVotes))
    {
    }

    internal Pose3D(IntPtr smartPtr)
        : base(
            smartPtr,
            GetRawPtr(smartPtr),
            static p => NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_delete(p)))
    {
    }

    private static IntPtr CreateNative(double alpha, nuint modelIndex, nuint numVotes)
    {
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_new2(
                alpha,
                modelIndex,
                numVotes,
                out var smartPtr));
        return smartPtr;
    }

    private static IntPtr GetRawPtr(IntPtr smartPtr)
    {
        if (smartPtr == IntPtr.Zero)
            throw new OpenCvSharpException("Failed to create Pose3D");

        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_get(
                smartPtr,
                out var rawPtr));
        if (rawPtr == IntPtr.Zero)
            throw new OpenCvSharpException("Failed to create Pose3D");
        return rawPtr;
    }

    /// <summary>
    /// Rotation around the reference point normal used during PPF voting.
    /// </summary>
    public double Alpha
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getAlpha(Handle, out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_setAlpha(Handle, value));
        }
    }

    /// <summary>
    /// Registration residual, typically populated by <see cref="ICP.RegisterModelToScene(InputArray, InputArray, IEnumerable{Pose3D})"/>.
    /// </summary>
    public double Residual
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getResidual(Handle, out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_setResidual(Handle, value));
        }
    }

    /// <summary>
    /// Index of the reference point in the sampled model.
    /// </summary>
    public nuint ModelIndex
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getModelIndex(Handle, out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_setModelIndex(Handle, value));
        }
    }

    /// <summary>
    /// Number of votes received by the pose.
    /// </summary>
    public nuint NumVotes
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getNumVotes(Handle, out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_setNumVotes(Handle, value));
        }
    }

    /// <summary>
    /// Gets or sets the 4-by-4 CV_64F homogeneous pose matrix.
    /// The caller owns and must dispose the matrix returned by the getter.
    /// Setting this property also updates <see cref="Angle"/>, <see cref="T"/>, and <see cref="Q"/>.
    /// </summary>
    public Mat Pose
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getPose(Handle, out var value));
            return new Mat(value);
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            UpdatePose(value);
        }
    }

    /// <summary>
    /// Rotation angle represented by the pose, in radians.
    /// </summary>
    public double Angle
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getAngle(Handle, out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_setAngle(Handle, value));
        }
    }

    /// <summary>
    /// Translation component of the pose.
    /// </summary>
    public Vec3d T
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getT(Handle, out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_setT(Handle, value));
        }
    }

    /// <summary>
    /// Quaternion rotation component of the pose.
    /// </summary>
    public Vec4d Q
    {
        get
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_getQ(Handle, out var value));
            return value;
        }
        set
        {
            ThrowIfDisposed();
            NativeMethods.HandleException(
                NativeMethods.surface_matching_Pose3D_setQ(Handle, value));
        }
    }

    /// <summary>
    /// Replaces this pose with a 4-by-4 CV_64F homogeneous transformation.
    /// </summary>
    /// <param name="newPose">New pose matrix.</param>
    public void UpdatePose(InputArray newPose)
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_updatePose1(Handle, newPose.Proxy));
        GC.KeepAlive(newPose.Source);
    }

    /// <summary>
    /// Replaces this pose using a rotation matrix and translation vector.
    /// </summary>
    /// <param name="newRotation">New 3-by-3 CV_64F rotation matrix.</param>
    /// <param name="newTranslation">New translation vector.</param>
    public void UpdatePose(InputArray newRotation, Vec3d newTranslation)
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_updatePose2(
                Handle,
                newRotation.Proxy,
                newTranslation));
        GC.KeepAlive(newRotation.Source);
    }

    /// <summary>
    /// Replaces this pose using a quaternion and translation vector.
    /// </summary>
    /// <param name="quaternion">Quaternion rotation.</param>
    /// <param name="newTranslation">New translation vector.</param>
    public void UpdatePoseQuat(Vec4d quaternion, Vec3d newTranslation)
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_updatePoseQuat(
                Handle,
                quaternion,
                newTranslation));
    }

    /// <summary>
    /// Left-multiplies this pose by an incremental 4-by-4 CV_64F transformation.
    /// </summary>
    /// <param name="incrementalPose">Incremental pose matrix.</param>
    public void AppendPose(InputArray incrementalPose)
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_appendPose(
                Handle,
                incrementalPose.Proxy));
        GC.KeepAlive(incrementalPose.Source);
    }

    /// <summary>
    /// Prints the pose to the native standard output stream.
    /// </summary>
    public void PrintPose()
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_printPose(Handle));
    }

    /// <summary>
    /// Creates a native copy of this pose.
    /// </summary>
    /// <returns>The copied pose.</returns>
    public Pose3D Clone()
    {
        ThrowIfDisposed();
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_clone(Handle, out var value));
        return new Pose3D(value);
    }

    /// <summary>
    /// Writes this pose in OpenCV's binary pose format.
    /// </summary>
    /// <param name="fileName">Destination file name.</param>
    /// <returns>Zero on success; otherwise -1.</returns>
    public int WritePose(string fileName)
    {
        ThrowIfDisposed();
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName));
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_writePose(
                Handle,
                fileName,
                out var returnValue));
        return returnValue;
    }

    /// <summary>
    /// Reads this pose from OpenCV's binary pose format.
    /// </summary>
    /// <param name="fileName">Source file name.</param>
    /// <returns>Zero on success; otherwise -1.</returns>
    public int ReadPose(string fileName)
    {
        ThrowIfDisposed();
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName));
        NativeMethods.HandleException(
            NativeMethods.surface_matching_Pose3D_readPose(
                Handle,
                fileName,
                out var returnValue));
        return returnValue;
    }
}
