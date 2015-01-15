using Epicycle.Math.Geometry;
using Epicycle.Math.Geometry.Polytopes;
using NUnit.Framework;

namespace Epicycle.Photogrammetry
{
    [TestFixture]
    public sealed class CameraModelTest : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void Projection_of_point_with_zero_XY_is_PrincipalPoint()
        {
            var principalPoint = new Vector2(-3.14, 2.88);

            var camera = new CameraModel(focalLength: 7.13, principalPoint: principalPoint, domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            var point = new Vector3(xy: Vector2.Zero, z: 17);

            var projection = camera.Project(point);

            Expect(projection != null);
            Expect(Vector2.Distance(projection.Value, principalPoint), Is.LessThan(_tolerance));
        }

        [Test]
        public void Projection_of_point_behind_camera_is_null()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            var point = new Vector3(xy: Vector2.Zero, z: -17);

            var projection = camera.Project(point);

            Expect(projection == null);
        }

        [Test]
        public void Projection_of_point_in_front_of_camera_is_correct()
        {
            var camera = new CameraModel(focalLength: 3, principalPoint: new Vector2(-1, 2), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            var point = new Vector3(2, 4, 6);

            var projection = camera.Project(point);

            Expect(projection != null);
            Expect(projection.Value.X, Is.EqualTo(0).Within(_tolerance));
            Expect(projection.Value.Y, Is.EqualTo(4).Within(_tolerance));
        }

        [Test]
        public void ProjectTangent_equals_to_differential_of_Project()
        {
            var eps = 1e-5;

            var camera = new CameraModel(focalLength: 1.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 8, height: 7));

            var point = new Vector3(0.314, 0.288, 2.56);
            var tangent = new Vector3(-1.23, 4.56, -2.35);

            var fwdProj = camera.Project(point + eps * tangent / 2);
            var bwdProj = camera.Project(point - eps * tangent / 2);

            var numResult = (fwdProj - bwdProj) / eps;
            var analResult = camera.ProjectTangent(point, tangent);

            Expect(analResult != null);
            Expect(Vector2.Distance(analResult.Value, numResult.Value), Is.LessThan(1e-10));
        }

        [Test]
        public void Projection_of_a_point_on_the_LineOfSight_of_a_given_image_point_equals_to_the_image_point()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            var imagePoint = new Vector2(0.144, 2.71828);

            var spacePoint = 0.577 * camera.LineOfSight(imagePoint);

            var roundTrip = camera.Project(spacePoint);

            Expect(roundTrip != null);
            Expect(Vector2.Distance(roundTrip.Value, imagePoint), Is.LessThan(_tolerance));
        }

        private static void PrepareSegmentsForCropVisibleTest(CameraModel camera, Interval chordInterval, out Segment2 chord, out Segment2 segment2, out Segment3 segment3)
        {
            chord = new Segment2(camera.Domain.GetEdge(0).PointAt(0.177), camera.Domain.GetEdge(1).PointAt(0.643));

            segment2 = new Segment2(chord.PointAt(chordInterval.Min), chord.PointAt(chordInterval.Max));

            var spaceExteriorPoint1 = 2.56 * camera.LineOfSight(segment2.Start);
            var spaceExteriorPoint2 = 4.86 * camera.LineOfSight(segment2.End);

            segment3 = new Segment3(spaceExteriorPoint1, spaceExteriorPoint2);
        }

        private void ExpectSegmentsAreCollinear(Segment3 a, Segment3 b)
        {
            Expect((a.Start - b.Start).Cross(b.Vector).Norm, Is.LessThan(_tolerance));
            Expect((a.End -   b.Start).Cross(b.Vector).Norm, Is.LessThan(_tolerance));
        }

        [Test]
        public void CropVisible_of_Segment3_intersecting_2_visibility_boundary_planes_is_correct()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            Segment2 chord;
            Segment2 segment2;
            Segment3 segment3;

            PrepareSegmentsForCropVisibleTest(camera, new Interval(-0.14, 1.23), out chord, out segment2, out segment3);

            var crop = camera.CropVisible(segment3);

            Expect(crop != null);
            ExpectSegmentsAreCollinear(crop.Value, segment3);
            Expect(Vector2.Distance(camera.Project(crop.Value.Start).Value, chord.Start), Is.LessThan(_tolerance)); // crop.Start projects to correct point
            Expect(Vector2.Distance(camera.Project(crop.Value.End).Value,   chord.End  ), Is.LessThan(_tolerance)); // crop.End projects to correct point
        }

        [Test]
        public void CropVisible_of_Segment3_intersecting_1_visibility_boundary_plane_is_correct()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            Segment2 chord;
            Segment2 segment2;
            Segment3 segment3;

            PrepareSegmentsForCropVisibleTest(camera, new Interval(-0.14, 0.56), out chord, out segment2, out segment3);

            var crop = camera.CropVisible(segment3);

            Expect(crop != null);
            ExpectSegmentsAreCollinear(crop.Value, segment3);
            Expect(Vector2.Distance(camera.Project(crop.Value.Start).Value, chord.Start ), Is.LessThan(_tolerance)); // crop.Start projects to correct point
            Expect(Vector2.Distance(camera.Project(crop.Value.End).Value,   segment2.End), Is.LessThan(_tolerance)); // crop.End projects to correct point
        }

        [Test]
        public void CropVisible_of_Segment3_inside_visible_region_is_original_Segment3()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            Segment2 chord;
            Segment2 segment2;
            Segment3 segment3;

            PrepareSegmentsForCropVisibleTest(camera, new Interval(0.14, 0.56), out chord, out segment2, out segment3);

            var crop = camera.CropVisible(segment3);

            Expect(crop != null);
            ExpectSegmentsAreCollinear(crop.Value, segment3);
            Expect(Vector2.Distance(camera.Project(crop.Value.Start).Value, segment2.Start), Is.LessThan(_tolerance)); // crop.Start projects to correct point
            Expect(Vector2.Distance(camera.Project(crop.Value.End).Value,   segment2.End  ), Is.LessThan(_tolerance)); // crop.End projects to correct point
        }

        [Test]
        public void CropVisible_of_Segment3_outside_visible_region_is_null()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            Segment2 chord;
            Segment2 segment2;
            Segment3 segment3;

            PrepareSegmentsForCropVisibleTest(camera, new Interval(1.14, 1.56), out chord, out segment2, out segment3);

            var crop = camera.CropVisible(segment3);

            Expect(crop == null);
        }

        [Test]
        public void Projection_of_Segment3_intersecting_2_visibility_boundary_planes_is_correct()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            Segment2 chord;
            Segment2 segment2;
            Segment3 segment3;

            PrepareSegmentsForCropVisibleTest(camera, new Interval(-0.14, 1.23), out chord, out segment2, out segment3);

            var proj = camera.Project(segment3);

            Expect(proj != null);
            Expect(Vector2.Distance(proj.Value.Start, chord.Start), Is.LessThan(_tolerance));
            Expect(Vector2.Distance(proj.Value.End,   chord.End  ), Is.LessThan(_tolerance));
        }

        [Test]
        public void Projection_of_Segment3_outside_visible_region_is_null()
        {
            var camera = new CameraModel(focalLength: 7.13, principalPoint: new Vector2(-3.14, 2.88), domain: new Box2(minX: -4, minY: -2, width: 5, height: 8));

            Segment2 chord;
            Segment2 segment2;
            Segment3 segment3;

            PrepareSegmentsForCropVisibleTest(camera, new Interval(1.14, 1.56), out chord, out segment2, out segment3);

            var proj = camera.Project(segment3);

            Expect(proj == null);
        }
    }
}
