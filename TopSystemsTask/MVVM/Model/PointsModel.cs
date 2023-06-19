using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSystemsTask.MVVM.ViewModel;

namespace TopSystemsTask.MVVM.Model
{
    public class PointsModel : ViewModelBase
    {
        public PointsModel(double x, double y)
        {
            X = x;
			Y = y;
        }
        private double _x;

		public double X
		{
			get { return _x; }
			set { 
				_x = value;
				OnPropertyChanged(nameof(X));
			}
		}

		private double _y;

		public double Y
		{
			get { return _y; }
			set { 
				_y = value;
				OnPropertyChanged(nameof(Y));
			}
		}

	}
}
