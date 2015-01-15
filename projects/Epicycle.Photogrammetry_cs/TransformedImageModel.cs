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
