using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;

using Epicycle.Math.Geometry;

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
