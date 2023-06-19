using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TopSystemsTask.MVVM.Model
{
    public sealed class PolygonModel : ShapesModel
    {
        public PolygonModel()
        {
        }
        public PolygonModel(PolygonModel copyModel)
        {
            ShapeName = copyModel.ShapeName;
            ShapeType = copyModel.ShapeType;
            Height = copyModel.Height;
            Width = copyModel.Width;
            Points = copyModel.Points;
        }
        public PolygonModel(ObservableCollection<PointsModel> _points, double _height, double _width)
        {
            PointCollection pointCollection = new PointCollection();
            Height = _height;
            Width = _width;
            foreach (var p in _points)
            {
                pointCollection.Add(new Point(p.X, p.Y));
            }
            Points = pointCollection;
        }
        public PointCollection Points { get; set; }
    }
}
