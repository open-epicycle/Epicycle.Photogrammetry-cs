using Epicycle.Commons;
using Epicycle.Commons.Collections;
using Epicycle.Math.Geometry;
using System.Collections.Generic;

namespace Epicycle.Photogrammetry
{
    public static class ImageModelUtils
    {
        public static Ray3? Ray(this IImageModel @this, Vector2 imagePoint)
        {
            var los = @this.LineOfSight(imagePoint);

            if (los == null)
            {
                return null;
            }
            else
            {
                return new Ray3(@this.OpticalCenter, los);
            }            
        }

        public static bool IsVisibleOn(this Vector2 @this, IImageModel model)
        {
            return model.Domain.Contains(@this);
        }

        public static bool IsVisibleOn(this Vector2? @this, IImageModel model)
        {
            return @this.HasValue && @this.Value.IsVisibleOn(model);
        }

        public static bool IsVisibleOn(this Vector3 @this, IImageModel model)
        {
            return model.Project(@this).IsVisibleOn(model);
        }

        public static Vector2? StrictlyProject(this IImageModel @this, Vector3 point)
        {
            var projection = @this.Project(point);

            if (projection.IsVisibleOn(@this))
            {
                return projection;
            }
            else
            {
                return null;
            }
        }

        public static Vector3 ReverseProject(this IImageModel @this, Vector2 point, Plane plane)
        {
            var ray = @this.Ray(point);

            if (ray == null)
            {
                return null;
            }
            else
            {
                return ray.Value.Intersect(plane);
            }
        }

        public static IEnumerable<Plane> VisibilityBoundaries(this IImageModel @this)
        {
            var vertices = @this.Domain.Vertices;

            for (var i = 0; i < 4; i++)
            {
                var los1 = @this.LineOfSight(vertices[i]);
                var los2 = @this.LineOfSight(vertices.ElementAtCyclic(i + 1));

                yield return Plane.Parametric(@this.OpticalCenter, los2, los1);
            }
        }
    }
}