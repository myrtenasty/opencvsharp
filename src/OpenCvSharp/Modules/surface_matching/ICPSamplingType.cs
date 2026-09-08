namespace OpenCvSharp.PpfMatch3D;

/// <summary>
/// Point sampling strategy requested by <see cref="ICP"/>.
/// </summary>
/// <remarks>
/// OpenCV currently applies uniform sampling regardless of this setting.
/// </remarks>
public enum ICPSamplingType
{
    /// <summary>
    /// Uniform point sampling.
    /// </summary>
    Uniform = 0,

    /// <summary>
    /// Gelfand point sampling.
    /// </summary>
    Gelfand = 1,
}
