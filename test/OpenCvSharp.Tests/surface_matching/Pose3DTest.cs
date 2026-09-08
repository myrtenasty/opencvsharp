using OpenCvSharp.PpfMatch3D;
using Xunit;

namespace OpenCvSharp.Tests.SurfaceMatching;

public class Pose3DTest : TestBase
{
    [Fact]
    public void UpdatePoseSynchronizesPoseRepresentations()
    {
        using var pose = new Pose3D(alpha: 0.25, modelIndex: 3, numVotes: 7);
        using var poseMatrix = Mat.FromArray(new double[,]
        {
            { 1, 0, 0, 1.5 },
            { 0, 1, 0, -2.0 },
            { 0, 0, 1, 0.75 },
            { 0, 0, 0, 1 },
        });

        pose.UpdatePose(poseMatrix);

        Assert.Equal(0.25, pose.Alpha);
        Assert.Equal((nuint)3, pose.ModelIndex);
        Assert.Equal((nuint)7, pose.NumVotes);
        Assert.Equal(0, pose.Angle, 12);
        Assert.Equal(new Vec3d(1.5, -2.0, 0.75), pose.T);

        using var actualPose = pose.Pose;
        Assert.Equal(4, actualPose.Rows);
        Assert.Equal(4, actualPose.Cols);
        Assert.Equal(MatType.CV_64FC1, actualPose.Type());
        Assert.Equal(1.5, actualPose.Get<double>(0, 3));
        Assert.Equal(-2.0, actualPose.Get<double>(1, 3));
        Assert.Equal(0.75, actualPose.Get<double>(2, 3));
    }

    [Fact]
    public void RotationAndQuaternionOverloadsUpdatePoseMatrix()
    {
        using var pose = new Pose3D();
        using var identityRotation = Mat.FromArray(new double[,]
        {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 },
        });

        pose.UpdatePose(identityRotation, new Vec3d(1, 2, 3));
        using (var rotationPose = pose.Pose)
        {
            Assert.Equal(1.0, rotationPose.Get<double>(0, 3));
            Assert.Equal(2.0, rotationPose.Get<double>(1, 3));
            Assert.Equal(3.0, rotationPose.Get<double>(2, 3));
        }

        pose.UpdatePoseQuat(new Vec4d(1, 0, 0, 0), new Vec3d(-1, -2, -3));
        using (var quaternionPose = pose.Pose)
        {
            Assert.Equal(-1.0, quaternionPose.Get<double>(0, 3));
            Assert.Equal(-2.0, quaternionPose.Get<double>(1, 3));
            Assert.Equal(-3.0, quaternionPose.Get<double>(2, 3));
        }

        using var increment = Mat.FromArray(new double[,]
        {
            { 1, 0, 0, 0.5 },
            { 0, 1, 0, 1.0 },
            { 0, 0, 1, 1.5 },
            { 0, 0, 0, 1 },
        });
        pose.AppendPose(increment);
        using var appendedPose = pose.Pose;
        Assert.Equal(-0.5, appendedPose.Get<double>(0, 3));
        Assert.Equal(-1.0, appendedPose.Get<double>(1, 3));
        Assert.Equal(-1.5, appendedPose.Get<double>(2, 3));
    }

    [Fact]
    public void CloneCreatesIndependentPoseWrapper()
    {
        using var pose = new Pose3D(alpha: 0.5, modelIndex: 4, numVotes: 9);
        using var identity = Mat.FromArray(new double[,]
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 },
        });
        pose.UpdatePose(identity);

        using var clone = pose.Clone();
        pose.Alpha = 1.0;

        Assert.Equal(0.5, clone.Alpha);
        Assert.Equal((nuint)4, clone.ModelIndex);
        Assert.Equal((nuint)9, clone.NumVotes);
        using var clonedPose = clone.Pose;
        Assert.Equal(1.0, clonedPose.Get<double>(3, 3));

        pose.PrintPose();
    }

    [Fact]
    public void BinarySerializationRoundTripsPose()
    {
        var fileName = Path.Combine(
            Path.GetTempPath(),
            $"opencvsharp_pose_{Guid.NewGuid():N}.bin");

        try
        {
            using var source = new Pose3D(alpha: 0.25, modelIndex: 3, numVotes: 7);
            using var poseMatrix = Mat.FromArray(new double[,]
            {
                { 1, 0, 0, 1.5 },
                { 0, 1, 0, -2.0 },
                { 0, 0, 1, 0.75 },
                { 0, 0, 0, 1 },
            });
            source.UpdatePose(poseMatrix);
            source.Residual = 0.125;
            Assert.Equal(0, source.WritePose(fileName));

            using var restored = new Pose3D();
            Assert.Equal(0, restored.ReadPose(fileName));
            Assert.Equal((nuint)3, restored.ModelIndex);
            Assert.Equal((nuint)7, restored.NumVotes);
            Assert.Equal(0.125, restored.Residual);
            Assert.Equal(new Vec3d(1.5, -2.0, 0.75), restored.T);
        }
        finally
        {
            File.Delete(fileName);
        }
    }
}
