using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TopSystemsTask.MVVM.Commands;
using TopSystemsTask.MVVM.Model;
using TopSystemsTask.MVVM.View;

namespace TopSystemsTask.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Brush DefaultFill = new SolidColorBrush(Color.FromRgb(2, 136, 209));
        private readonly Brush DefaultStroke = new SolidColorBrush(Color.FromRgb(179, 229, 252));
        protected bool isDragging;
        private Point clickPosition;
        private TranslateTransform originPosition;
        private Window CurrentWindow;

        private ObservableCollection<ShapesModel> _shapes;
        public ObservableCollection<ShapesModel> Shapes
        {
            get { return _shapes; }
            set
            {
                _shapes = value;
                OnPropertyChanged(nameof(Shapes));
            }
        }

        public MainViewModel()
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window == App.Current.MainWindow)
                {
                    CurrentWindow = window;

                }
            }
            Shapes = new ObservableCollection<ShapesModel>()
            {
                new EllipseModel(DefaultValues.WH100, DefaultValues.WH100) { ShapeName = "Circle", ShapeType = typeof(Ellipse)},
                new EllipseModel(DefaultValues.WH60, DefaultValues.WH100) { ShapeName = "Ellipse", ShapeType = typeof(Ellipse)},
                new RectangleModel(DefaultValues.WH100, DefaultValues.WH100) {ShapeName = "Square", ShapeType=typeof(Rectangle)},
                new RectangleModel(DefaultValues.WH60, DefaultValues.WH100) {ShapeName = "Rectangle", ShapeType=typeof(Rectangle)},
                new PolygonModel(DefaultValues.Points, DefaultValues.WH100, DefaultValues.WH100) {ShapeName = "Triangle", ShapeType=typeof(Polygon)}
            };
        }


        private ObservableCollection<PointsModel> _coppoints;
        public ObservableCollection<PointsModel> CopPoints
        {
            get { return _coppoints; }
            set
            {
                _coppoints = value;
                OnPropertyChanged(nameof(CopPoints));
            }
        }


        private ShapesModel _selectedShape;
        public ShapesModel SelectedShape
        {
            get { return _selectedShape; }
            set
            {
                CopPoints?.Clear();
                if (value.ShapeType == typeof(Polygon))
                {
                    if (((PolygonModel)value).Points != null && ((PolygonModel)value).Points.Count > 0)
                    {
                        foreach (var item in ((PolygonModel)value).Points.ToList())
                        {
                            CopPoints ??= new ObservableCollection<PointsModel>();
                            CopPoints.Add(new PointsModel(item.X, item.Y));
                        }
                    }
                }
                _selectedShape = value;
                OnPropertyChanged(nameof(SelectedShape));
            }
        }

        private RelayCommand _addPointCommand;
        public RelayCommand AddPointCommand
        {
            get
            {
                return _addPointCommand ??= new RelayCommand(obj =>
                {
                    if (SelectedShape.ShapeType == typeof(Polygon))
                    {
                        CopPoints.Add(new PointsModel(0, 0));
                    }
                });
            }
        }

        private RelayCommand _saveShapeCommand;

        public RelayCommand SaveShapeCommand
        {
            get
            {
                return _saveShapeCommand ??= new RelayCommand(obj =>
                {
                    ShapesModel model;
                    if (SelectedShape.ShapeType == typeof(Ellipse))
                    {
                        model = new EllipseModel(SelectedShape as EllipseModel);
                    }
                    else if (SelectedShape.ShapeType == typeof(Rectangle))
                    {
                        model = new RectangleModel(SelectedShape as RectangleModel);
                    }
                    else
                    {
                        model = new PolygonModel(SelectedShape as PolygonModel);

                        ((PolygonModel)model).Points = new PointCollection();
                        foreach (var item in CopPoints)
                        {
                            ((PolygonModel)model).Points.Add(new Point(item.X, item.Y));
                        }
                    }
                    Shapes.Add(model);
                    SelectedShape = model;
                });
            }
        }


        private RelayCommand _drawShapeCommand;
        public RelayCommand DrawShapeCommand
        {
            get
            {
                return _drawShapeCommand ??= new RelayCommand(obj =>
                {
                     int shapeIndex = Shapes.IndexOf(SelectedShape);
                     if (SelectedShape.ShapeType == typeof(Polygon))
                     {
                         ((PolygonModel)Shapes[shapeIndex]).Points.Clear();
                         foreach (var item in CopPoints)
                         {
                             ((PolygonModel)Shapes[shapeIndex]).Points.Add(new Point(item.X, item.Y));
                         }
                     }
                    DrawShapes(SelectedShape, (Canvas)obj);
                });
            }
        }



        private RelayCommand _changeThemeCommand;
        public RelayCommand ChangeThemeCommand
        {
            get
            {
                return _changeThemeCommand ??= new RelayCommand(obj =>
                {
                    ResourceDictionary[] resourceDictionary = new ResourceDictionary[2] { new(), new() };
                    if (((ToggleButton)obj).IsChecked == true)
                    {
                        resourceDictionary[0].Source = new Uri("pack://application:,,,/Resources/Styles/Colors/Theme.Dark.Xaml");
                        resourceDictionary[1].Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.Blue.xaml");
                    }
                    else
                    {
                        resourceDictionary[0].Source = new Uri("pack://application:,,,/Resources/Styles/Colors/Theme.Light.Xaml");
                        resourceDictionary[1].Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.Blue.xaml");
                    }
                    foreach (var item in resourceDictionary)
                    {
                        App.Current.Resources.MergedDictionaries.Add(item);
                    }
                });
            }
        }



        private void DrawShapes(ShapesModel shape, Canvas canvas)
        {
            if (shape.ShapeType == typeof(Polygon))
            {
                ICollection<Point> enumPoint = new PointCollection();
                foreach (var item in ((PolygonModel)shape).Points)
                {
                    enumPoint.Add(new Point(item.X, item.Y));
                }
                Polygon polygonShape = new()
                {

                    Points = new PointCollection(enumPoint),
                    Stroke = DefaultStroke,
                    Height = shape.Height,
                    Width = shape.Width,
                    Stretch = Stretch.Uniform,
                    StrokeThickness = 2,
                    Fill = DefaultFill

                };
                polygonShape.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
                polygonShape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                polygonShape.MouseMove += Shape_MouseMove;
                canvas.Children.Add(polygonShape);
            }
            else if (shape.ShapeType == typeof(Ellipse))
            {
                Ellipse ellipseShape = new()
                {
                    Width = shape.Width,
                    Height = shape.Height,
                    Stroke = DefaultStroke,
                    StrokeThickness = 2,
                    Fill = DefaultFill
                };
                ellipseShape.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
                ellipseShape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                ellipseShape.MouseMove += Shape_MouseMove;
                canvas.Children.Add(ellipseShape);
            }
            else
            {
                Rectangle rectangleShape = new()
                {
                    Width = shape.Width,
                    Height = shape.Height,
                    Stroke = DefaultStroke,
                    StrokeThickness = 2,
                    Fill = DefaultFill
                };
                rectangleShape.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
                rectangleShape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                rectangleShape.MouseMove += Shape_MouseMove;
                canvas.Children.Add(rectangleShape);
            }
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var draggableControl = (Shape)sender;
            originPosition = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
            isDragging = true;
            clickPosition = e.GetPosition(CurrentWindow);
            draggableControl.CaptureMouse();
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = (Shape)sender;
            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(CurrentWindow);
                var transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X = originPosition.X + (currentPosition.X - clickPosition.X);
                transform.Y = originPosition.Y + (currentPosition.Y - clickPosition.Y);
                draggableControl.RenderTransform = new TranslateTransform(transform.X, transform.Y);
            }
        }

        private void Shape_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = (Shape)sender;
            draggable.ReleaseMouseCapture();
        }
    }
}
