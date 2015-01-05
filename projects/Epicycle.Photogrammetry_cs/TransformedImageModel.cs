using Epicycle.Math.Geometry;

namespace Epicycle.Photogrammetry
{
    public sealed class TransformedImageModel : IImageModel
    {
        public TransformedImageModel(IImageModel localModel, RotoTranslation3 localToGlobal)
        {
            _localModel = localModel;
            _localToGlobal = localToGlobal;
        }

        private readonly IImageModel _localModel;
        private readonly RotoTranslation3 _localToGlobal;

        public IImageModel LocalModel
        {
            get { return _localModel; }
        }

        public RotoTranslation3 LocalToGlobal
        {
            get { return _localToGlobal; }
        }

        public Vector3 OpticalCenter
        {
            get { return LocalToGlobal.Apply(LocalModel.OpticalCenter); }
        }

        public Box2 Domain
        {
            get { return LocalModel.Domain; }
        }

        public Vector2? Project(Vector3 spacePoint)
        {
            var localPoint = LocalToGlobal.ApplyInv(spacePoint);

            return LocalModel.Project(localPoint);
        }

        public Vector2? ProjectTangent(Vector3 spacePoint, Vector3 tangent)
        {
            var localPoint = LocalToGlobal.ApplyInv(spacePoint);
            var localTangent = LocalToGlobal.Rotation.ApplyInv(tangent);

            return LocalModel.ProjectTangent(localPoint, localTangent);
        }

        public Vector3 LineOfSight(Vector2 imagePoint)
        {
            var localLos = LocalModel.LineOfSight(imagePoint);

            return LocalToGlobal.Rotation.Apply(localLos);
        }

        public Segment3? CropVisible(Segment3 segment)
        {
            var localSegment = LocalToGlobal.ApplyInv(segment);

            var localCrop = LocalModel.CropVisible(localSegment);

            if (localCrop == null)
            {
                return null;
            }
            else
            {
                return LocalToGlobal.Apply(localCrop.Value);
            }
        }

        public Segment2? Project(Segment3 segment)
        {
            var localSegment = LocalToGlobal.ApplyInv(segment);

            return LocalModel.Project(localSegment);
        }
    }
}
