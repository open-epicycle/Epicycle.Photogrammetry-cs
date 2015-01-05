using Epicycle.Math.Geometry;

namespace Epicycle.Photogrammetry
{
    public static class CameraModelUtils
    {
        public sealed class YamlSerialization
        {
            public double FocalLength { get; set; }
            public Vector2Utils.YamlSerialization PrincipalPoint { get; set; }
            public Box2Utils.YamlSerialization Domain { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(CameraModel cameraModel)
            {
                FocalLength = cameraModel.FocalLength;
                PrincipalPoint = new Vector2Utils.YamlSerialization(cameraModel.PrincipalPoint);
                Domain = new Box2Utils.YamlSerialization(cameraModel.Domain);
            }

            public CameraModel Deserialize()
            {
                return new CameraModel(FocalLength, PrincipalPoint.Deserialize(), Domain.Deserialize());
            }
        }
    }
}
