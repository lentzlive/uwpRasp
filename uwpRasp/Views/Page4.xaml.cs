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



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpRasp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page4 : Page
    {
        #region static
        public static readonly DependencyProperty DataProperty;
        static Page4()
        {
            DataProperty = DependencyProperty.Register("Data", typeof(SinDataGenerator), typeof(Page4), new PropertyMetadata(null));
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
        public Page4()
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


     public static   int i = 0;

        void OnTimerTick(object sender, object e)
        {

            Data.Refresh();
           
            txtCount.Text = Convert.ToString(3 * i);
            i++;
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

        double ReturnCount()
        {
            return count;
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
