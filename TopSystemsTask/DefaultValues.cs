﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TopSystemsTask.MVVM.Model;

namespace TopSystemsTask
{
    public class DefaultValues
    {
        public const double WH100 = 100;
        public const double WH60 = 60;
        public static ObservableCollection<PointsModel> Points = new()
        {
            new PointsModel(0,0),
            new PointsModel(50,0),
            new PointsModel(25,-50)
        };

    }
}
