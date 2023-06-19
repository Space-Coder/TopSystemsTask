using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TopSystemsTask.MVVM.Model
{
    public class ShapesModel
    {
        public ShapesModel()
        {
            
        }
        public ShapesModel(ShapesModel copyModel)
        {
            Height = copyModel.Height;
            Width = copyModel.Width;
            ShapeName = copyModel.ShapeName;
            ShapeType = copyModel.ShapeType;
        }
        public double Height { get; set; }
        public double Width { get; set; }
        public string ShapeName { get; set; }
        public Type ShapeType { get; set; }

    }
}
