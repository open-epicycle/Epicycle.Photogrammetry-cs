// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Photogrammetry-cs
// ]]]]

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