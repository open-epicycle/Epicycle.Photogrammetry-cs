using Epicycle.Math.Geometry;

namespace Epicycle.Photogrammetry
{
    public interface IImageModel
    {
        Vector3 OpticalCenter { get; }

        Box2 Domain { get; }

        // might return null when spacePoint is outside of visible region
        // when spacePoint is outside of visible region but result is not null, it is guaranteed to be outside of Domain 
        Vector2? Project(Vector3 spacePoint);

        // applies differential of projection
        // might return null when spacePoint is outside of visible region
        // when spacePoint is outside of visible region but result is not null, it is guaranteed to be outside of Domain
        Vector2? ProjectTangent(Vector3 spacePoint, Vector3 tangent);

        // might return null when imagePoint is outside of visible region
        Vector3 LineOfSight(Vector2 imagePoint);

        // interesects segment with visible region
        // returns null when segment is outside of visible region
        Segment3? CropVisible(Segment3 segment);
        
        // interesects segment with visible region, projects the result and returns the endpoints of the resulting curve
        // returns null when segment is outside of visible region
        Segment2? Project(Segment3 segment);               
    }
}
