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
using Moq;
using NUnit.Framework;

namespace Epicycle.Photogrammetry
{
    [TestFixture]
    public sealed class TransformedImageModelTest : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void CropVisible_returns_correct_result_when_LocalModel_CropVisible_returns_not_null()
        {
            var point = new Vector3(3.141, 0.577, 2.718);

            var localModelMock = new Mock<IImageModel>();
            localModelMock.Setup(model => model.CropVisible(It.IsAny<Segment3>())).Returns((Segment3 s) => new Segment3(s.Start, point));

            var localToGlobal = new RotoTranslation3((Rotation3)new Quaternion(1.1, 2.2, 3.3, 4.4), new Vector3(1.23, 4.56, 2.56));

            var globalModel = new TransformedImageModel(localModelMock.Object, localToGlobal);

            var segment = new Segment3(new Vector3(7, 8, 9), new Vector3(-1, -2, -4));

            var crop = globalModel.CropVisible(segment);

            var transPoint = localToGlobal.Apply(point);

            Expect(crop != null);
            Expect(Vector3.Distance(crop.Value.Start, segment.Start), Is.LessThan(_tolerance));
            Expect(Vector3.Distance(crop.Value.End,   transPoint   ), Is.LessThan(_tolerance));
        }

        [Test]
        public void CropVisible_returns_null_when_LocalModel_CropVisible_returns_null()
        {
            var localModel = Mock.Of<IImageModel>(model => model.CropVisible(It.IsAny<Segment3>()) == null);

            var localToGlobal = new RotoTranslation3((Rotation3)new Quaternion(1.1, 2.2, 3.3, 4.4), new Vector3(1.23, 4.56, 2.56));

            var globalModel = new TransformedImageModel(localModel, localToGlobal);

            var segment = new Segment3(new Vector3(7, 8, 9), new Vector3(-1, -2, -4));

            var crop = globalModel.CropVisible(segment);

            Expect(crop == null);
        }
    }
}
