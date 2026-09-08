using OpenCvSharp.PpfMatch3D;
using Xunit;

namespace OpenCvSharp.Tests.SurfaceMatching;

public class PoseCluster3DTest : TestBase
{
    [Fact]
    public void AddPoseSharesOwnershipAndAccumulatesVotes()
    {
        using var first = new Pose3D(alpha: 0.25, modelIndex: 1, numVotes: 3);
        using var second = new Pose3D(alpha: 0.5, modelIndex: 2, numVotes: 5);
        using var cluster = new PoseCluster3D(first, id: 17);

        cluster.AddPose(second);
        var poses = cluster.GetPoses();
        try
        {
            Assert.Equal(17, cluster.Id);
            Assert.Equal((nuint)8, cluster.NumVotes);
            Assert.Equal(2, poses.Length);
            Assert.Equal((nuint)1, poses[0].ModelIndex);
            Assert.Equal((nuint)2, poses[1].ModelIndex);

            poses[0].Alpha = 0.75;
            Assert.Equal(0.75, first.Alpha);
        }
        finally
        {
            foreach (var pose in poses)
                pose.Dispose();
        }
    }

    [Fact]
    public void BinarySerializationRoundTripsCluster()
    {
        var fileName = Path.Combine(
            Path.GetTempPath(),
            $"opencvsharp_pose_cluster_{Guid.NewGuid():N}.bin");

        try
        {
            using var pose = new Pose3D(alpha: 0.25, modelIndex: 4, numVotes: 7);
            using var poseMatrix = Mat.FromArray(new double[,]
            {
                { 1, 0, 0, 1.5 },
                { 0, 1, 0, -2.0 },
                { 0, 0, 1, 0.75 },
                { 0, 0, 0, 1 },
            });
            pose.UpdatePose(poseMatrix);
            pose.Residual = 0.125;

            using (var source = new PoseCluster3D(pose, id: 23))
                Assert.Equal(0, source.WritePoseCluster(fileName));

            using var restored = new PoseCluster3D();
            Assert.Equal(0, restored.ReadPoseCluster(fileName));
            Assert.Equal(23, restored.Id);
            Assert.Equal((nuint)7, restored.NumVotes);

            var restoredPoses = restored.GetPoses();
            try
            {
                var restoredPose = Assert.Single(restoredPoses);
                Assert.Equal((nuint)4, restoredPose.ModelIndex);
                Assert.Equal((nuint)7, restoredPose.NumVotes);
                Assert.Equal(0.125, restoredPose.Residual);
                Assert.Equal(new Vec3d(1.5, -2.0, 0.75), restoredPose.T);
            }
            finally
            {
                foreach (var restoredPose in restoredPoses)
                    restoredPose.Dispose();
            }
        }
        finally
        {
            File.Delete(fileName);
        }
    }
}
