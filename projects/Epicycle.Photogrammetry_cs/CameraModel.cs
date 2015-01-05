using Epicycle.Math.Geometry;

namespace Epicycle.Photogrammetry
{
    public sealed class CameraModel : IImageModel
    {
        public static readonly CameraModel Default = new CameraModel(1, Vector2.Zero, new Box2(new Vector2(-0.5, -0.5), new Vector2(1, 1)));

        public CameraModel(double focalLength, Vector2 principalPoint, Box2 domain)
        {
            _domain = domain;
            _focalLength = focalLength;
            _principalPoint = principalPoint;
        }

        private readonly Box2 _domain;
        private readonly double _focalLength;
        private readonly Vector2 _principalPoint;

        #region properties

        Vector3 IImageModel.OpticalCenter
        {
            get { return Vector3.Zero; }
        }

        public Box2 Domain
        {
            get { return _domain; }
        }

        public double FocalLength
        {
            get { return _focalLength; }
        }

        public Vector2 PrincipalPoint
        {
            get { return _principalPoint; }
        }

        #endregion

        #region methods

        public Vector2? Project(Vector3 spacePoint)
        {
            if (spacePoint.Z <= 0)
            {
                return null;
            }

            return (_focalLength / spacePoint.Z) * spacePoint.XY + _principalPoint;
        }

        public Vector2? ProjectTangent(Vector3 spacePoint, Vector3 tangent)
        {
            var w = _focalLength / spacePoint.Z;
            var dw = (-w / spacePoint.Z) * tangent.Z;

            return new Vector2(w * tangent.X + dw * spacePoint.X, w * tangent.Y + dw * spacePoint.Y);
        }

        public Vector3 LineOfSight(Vector2 imagePoint)
        {
            return new Vector3(imagePoint - _principalPoint, _focalLength);
        }

        public Segment3? CropVisible(Segment3 segment)
        {
            var vertices = Domain.Vertices;
            Segment3? answer = segment;

            foreach (var plane in this.VisibilityBoundaries())
            {
                answer = answer.Value.CropBy(plane);

                if (answer == null)
                {
                    return null;
                }
            }

            return answer;
        }

        public Segment2? Project(Segment3 segment)
        {
            var crop = CropVisible(segment);

            if (crop == null)
            {
                return null;
            }
            else
            {
                return new Segment2(Project(crop.Value.Start).Value, Project(crop.Value.End).Value);
            }
        }

        #endregion
    }
}
