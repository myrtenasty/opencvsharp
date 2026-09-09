# Prepared PPF upgrade: 5.0.0.20260909-ppfscale

The local feed at C:/Users/Myrtenasty/nuget-local-packages contains OpenCvSharp5 and OpenCvSharp5.runtime.win at version 5.0.0.20260909-ppfscale, plus the managed symbol package. The eight existing ppfhash package/symbol files were verified unchanged by SHA-256. Copies of the new packages are in artifacts/.

The managed assembly reports InformationalVersion 5.0.0.20260909-ppfscale+531699afee03268ffa877b4a0c710fd6cf43cb8b and AssemblyVersion 5.0.0.0. Version was supplied at pack time; assembly version constants were not changed.

## API contract

TrainPrepared takes an Nx6 CV_32F matrix of XYZ coordinates in metres and unit normals, a positive finite distanceBinMetres, and an optional numberOfAngles (default 30). It owns an exact copy of the model rows, including separate normal groups at coincident positions. It performs no quantization or normal modification.

MatchPrepared uses every supplied scene row as a voting partner. Its sceneReferenceFraction is dimensionless, in (0, 1], and selects reference rows at stride floor(1/fraction), capped at the scene row count. A partial final stride is included. The distanceBinMetres value must exactly equal the training value, and the detector must have been trained with TrainPrepared.

Inputs must contain at least two rows, finite coordinates, and finite normals whose squared length differs from 1 by at most 1e-4. Invalid inputs, unsafe distance-bin integer ranges, mismatched bins, and incompatible training state raise explicit errors. Prepared angle computations clamp cosine roundoff without modifying the supplied normals.

The relative constructor, TrainModel, Match, and SetSearchParams remain available. The optimized multi-value hash remains shared by both training paths. Translation clustering uses the point-cloud units (metres here); rotation clustering uses radians.

```csharp
using var detector = new OpenCvSharp.PpfMatch3D.PPF3DDetector();
detector.SetSearchParams(positionThreshold: 0.02, rotationThreshold: 0.05);
detector.TrainPrepared(modelPoints, distanceBinMetres: 0.03);

var poses = detector.MatchPrepared(
    scenePoints,
    sceneReferenceFraction: 0.2,
    distanceBinMetres: 0.03);
try
{
    // Consume poses. Each pose owns native resources.
}
finally
{
    foreach (var pose in poses)
        pose.Dispose();
}
```

## Verification

- All 21 managed SurfaceMatching tests passed.
- Native acceptance checks passed: exact retained model rows/normals, non-contiguous input, caller-independent training storage, every scene voting row, final partial reference stride, distant scene bounds, invalid inputs, training modes, and finite poses for rounded unit normals.
- The deterministic partial-visibility/clutter fixture changed its best vote count from 105 with a 0.005-metre bin to 496 with a 0.04-metre bin.
- Two instances separated by 2 metres and 5 degrees of yaw verified narrow/wide translation and rotation thresholds, including weighted clustering.
- Prepared training on a 1280-point repeated-feature plane took approximately 1.06 seconds in the measured run; the acceptance bound is 12 seconds. This is a local regression check, not a general performance guarantee.
- A package-only consumer restored both new packages into an isolated cache, checked InformationalVersion, called Prepared and relative APIs, and verified mismatched bins produce OpenCVException.
- All 824 native exports from the old runtime remain. Seven exports were added: the two Prepared entry points and five string helpers needed by native exception conversion.
- The new native patch was replayed against the hash-optimized baseline and the resulting source was compared with the implementation.

## Native build scope and reproduction

This upgrade retains the existing ppfhash runtime's core/flann/surface_matching module scope. It is a PPF-focused local runtime, not a full-module OpenCvSharp runtime. The baseline CMake trees are under C:/Users/Myrtenasty/AppData/Local/Temp/opencvsharp-smcheck/. The repository src/build directory contained staged DLLs without a CMake cache, so the normal cmake --build src/build command could not run; the baseline OpenCV and extern CMake builds were used instead.

The baseline extern configuration now also compiles src/OpenCvSharpExtern/std_string.cpp, which is required to surface rejected native inputs as OpenCVException, and uses /utf-8. This corrects a missing export in the earlier local runtime.

The native source changes are preserved in patches/opencv_contrib/0002-ppf-prepared-metre-bin.patch. scripts/build_opencv_windows.ps1 applies it after the existing hash patch. The opencv_contrib checkout contains the corresponding uncommitted source changes.

```powershell
$ppfBuild = "$env:TEMP/opencvsharp-smcheck"
cmake --build "$ppfBuild/opencv-build" --config Release --target opencv_surface_matching -j 4
cmake --build "$ppfBuild/extern-build" --config Release --target OpenCvSharpExtern -j 4
cmake -S test/NativePpfPrepared -B artifacts/ppfscale-native -G "Visual Studio 18 2026" -A x64 "-DOpenCV_DIR=$ppfBuild/opencv-build"
cmake --build artifacts/ppfscale-native --config Release
ctest --test-dir artifacts/ppfscale-native -C Release --output-on-failure
Copy-Item "$ppfBuild/extern-build/Release/OpenCvSharpExtern.dll" test/OpenCvSharp.Tests/OpenCvSharpExtern.dll
Copy-Item "$ppfBuild/extern-build/Release/OpenCvSharpExtern.dll" src/build/OpenCvSharpExtern/Release/OpenCvSharpExtern.dll
dotnet test test/OpenCvSharp.Tests/OpenCvSharp.Tests.csproj -c Release --filter FullyQualifiedName~SurfaceMatching
```

For a new release, choose a new version and pass the same -p:Version value to dotnet pack for src/OpenCvSharp/OpenCvSharp.csproj and packaging/nuget/OpenCvSharp5.runtime.win.csproj. Do not overwrite a published local package version.

## Package checksums

- OpenCvSharp5.5.0.0.20260909-ppfscale.nupkg: 1A10308865E5B5DAF3E448FFC53414754DE504C54DE9185C18A4F5B7DFF995BE
- OpenCvSharp5.runtime.win.5.0.0.20260909-ppfscale.nupkg: 1E47C2B8B9E5FA1EE6168238611DD2624A93CEF03422B2BEE667E37E71662925
- Embedded OpenCvSharpExtern.dll: ADF53F470EBD48278209A6D0ECD6B4A2C3D5593A74355F9828999175CEAADCB8

Builds completed successfully with existing analyzer warnings, temporary-build MSBuild warnings, and the runtime packaging project's NU5128 framework-group warning. The full repository test suite was not run because this baseline native DLL only contains the PPF-focused module subset.
