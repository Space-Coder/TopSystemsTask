using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSystemsTask.MVVM.Model
{
    public sealed class RectangleModel : ShapesModel
    {
        public RectangleModel()
        {
        }
        public RectangleModel(RectangleModel copyModel)
        {
            ShapeName = copyModel.ShapeName;
            ShapeType = copyModel.ShapeType;
            Height = copyModel.Height;
            Width = copyModel.Width;
        }
        public RectangleModel(double _height, double width)
        {
            Height = _height;
            Width = width;
        }
    }
}
