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
