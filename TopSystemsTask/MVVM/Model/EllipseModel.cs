using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSystemsTask.MVVM.Model
{
    public sealed class EllipseModel : ShapesModel
    {
        public EllipseModel()
        {
        }
        public EllipseModel(EllipseModel copyModel)
        {
            ShapeName = copyModel.ShapeName;
            ShapeType = copyModel.ShapeType;
            Height = copyModel.Height;
            Width = copyModel.Width;
        }
        public EllipseModel(double _height, double _width)
        {
            Height = _height;
            Width = _width;
        }
    }
}
