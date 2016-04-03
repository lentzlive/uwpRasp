using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using DevExpress.UI.Xaml.Charts;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace uwpRasp.UC
{
    public sealed partial class ucRealTimeChart : UserControl
    {

        #region static
        public static readonly DependencyProperty DataProperty;
        static ucRealTimeChart()
        {
            DataProperty = DependencyProperty.Register("Data", typeof(SinDataGenerator), typeof(ucRealTimeChart), new PropertyMetadata(null));
        }



        #endregion

        #region dep props
        public SinDataGenerator Data
        {
            get { return (SinDataGenerator)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }


        #endregion


        DispatcherTimer timer;

        public ucRealTimeChart()
        {
            this.InitializeComponent();
            Data = new SinDataGenerator();
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1) };
            timer.Tick += OnTimerTick;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;


        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            timer.Start();
        }

        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            timer.Stop();
        }

        void OnTimerTick(object sender, object e)
        {
            Data.Refresh();
        }

    }


    public class SinDataGenerator : ChartDataAdapter
    {

        const int PointsCount = 1100;
        const double Divider = 500;
        const int NewPointsCount = 3;

        double count = 0;
        Random random = new Random();
        List<Point> points = new List<Point>();

        protected override int RowsCount { get { return points.Count; } }

        public SinDataGenerator()
        {
            for (int i = 0; i < PointsCount; i++)
            {
                points.Add(new Point(i, GetValue(count)));
                IncreaseCount();
            }
        }
        void IncreaseCount()
        {
            count++;
        }
        double GetValue(double count)
        {
            return (Math.Sin((count / Divider) * 2.0 * Math.PI) + random.NextDouble() - 0.5) * 33;
        }
        protected override DateTime GetDateTimeValue(int index, ChartDataMemberType dataMember)
        {
            return DateTime.MinValue;
        }
        protected override object GetKey(int index)
        {
            return null;
        }
        protected override double GetNumericalValue(int index, ChartDataMemberType dataMember)
        {
            if (dataMember == ChartDataMemberType.Argument)
                return points[index].X;
            return points[index].Y;
        }
        protected override string GetQualitativeValue(int index, ChartDataMemberType dataMember)
        {
            return null;
        }
        protected override ActualScaleType GetScaleType(ChartDataMemberType dataMember)
        {
            return ActualScaleType.Numerical;
        }
        public void Refresh()
        {
            for (int i = 0; i < NewPointsCount; i++)
            {
                points.RemoveAt(0);
                points.Add(new Point(count, GetValue(count)));
                IncreaseCount();
            }
            OnDataChanged(ChartDataUpdateType.Reset, -1);
        }


    }

}
