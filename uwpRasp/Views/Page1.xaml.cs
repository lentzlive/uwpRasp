using DevExpress.UI.Xaml.Charts;
using DeviceLibrary;
using DeviceLibrary.Model;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace uwpRasp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1 : Page
    {
        public class MyCollection : ObservableCollection<Point>
        {

        }
        public static MyCollection data = new MyCollection();

        SensorHelper _sensor = null;
        public static double minData = 10000;
        public static double MaxData = 0;

        public static double sumTime =0;


        static DeviceClient deviceClient;
        static string iotHubUri = "IoTLentzLive.azure-devices.net";
        static string deviceKey = "beAhLKFzamrJtw6Y7EE0oJzjemFA/rvY7i1SBGB/JBY=";
        public string TextLines { get; set; }

        public int Timing
        {
            get
            {
                return _timing;
            }

            set
            {
                _timing = value;
            }
        }

        static string _deviceId = "myFirstDevice";
        static int indexcount2 = 0;
        private Timer periodicTimer;

        private int _timing = 100;
        DispatcherTimer timer;
        void OnLoaded(object sender, RoutedEventArgs e)
        {

            timer.Start();
        }
        public Page1()
        {

            this.InitializeComponent();



            ((DataSourceAdapter)chart.Series[0].Data).DataSource = data;
            rangeY.StartValue = 18.0;// minData - minData * 0.0002;
            rangeY.EndValue = 25.0;// MaxData + MaxData * 0.0002;
            data.Clear();
            try
            {
                _sensor = new SensorHelper();
                StatusText.Text = _sensor.Message;
                //StartProcess();
                // Start();
            }
            catch (Exception ex)
            {
                StatusText.Text = ex.Message;
            }

            //  periodicTimer = new Timer(this.Timer_Tick, null, 0, 1000);
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1000) };
            timer.Tick += OnTimerTick;
            Loaded += OnLoaded;
            Unloaded += MainPage_Unloaded;

        }

        void OnTimerTick(object sender, object e)
        {
            ReadADC();
        }


        private void Timer_Tick(object state)
        {

            ReadADC();
        }

        public void ReadADC()
        {

            try
            {
                DateTime t1 = DateTime.Now;
                Sensor s = new Sensor();
                SensorValues sValues = s.GetSensorValue(_deviceId);
                // 
                Stopwatch sw = new Stopwatch();
                sw.Start();

                // Do something you want to time

                sw.Stop();

                double microseconds = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
                //long nanoseconds = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L * 1000L));


                //DateTime t2 = DateTime.Now;
                //TimeSpan timeDiff = t2 - t1;
                //string diff = timeDiff.TotalMilliseconds.ToString();

                //sumTime += timeDiff.TotalMilliseconds;
                sumTime += microseconds;
                


                var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    textPlaceHolder.Text = sValues.Temp + " °C";

                    data.Add(
                        new Point
                        (
                              indexcount2++,
                            Convert.ToDouble(sValues.Temp)

                        ));

                    indexcount2++;
                 
                    txtTimeDiffMedio.Text = Convert.ToString(sumTime / indexcount2) + " us";
                    txtTimeDiff.Text = microseconds + " us";

                });
            }
            catch (Exception e)
            { StatusText.Text = e.Message; }


        }


        private async void StartProcess()
        {

            int indexcount = 0;
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2));

            while (true)
            {
                Sensor s = new Sensor();
                SensorValues sValues = s.GetSensorValue(_deviceId);
                textPlaceHolder.Text = sValues.Temp + " °C";

                data.Add(
                    new Point
                    (
                          indexcount++,
                        Convert.ToDouble(sValues.Temp)

                    ));

                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));

            }
        }

        private async void Start()
        {
            int indexcount = 0;
            while (true)
            {
                data.Add(
                    new Point
                    (
                          indexcount++,
                        Convert.ToDouble(Math.Sin(indexcount))

                    ));


                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.1));

            }
        }

        private void MainPage_Unloaded(object sender, object args)
        {
            timer.Stop();
            /* It's good practice to clean up after we're done */
            //if (SpiADC != null)
            //{
            //    SpiADC.Dispose();
            //}

            //if (ledPin != null)
            //{
            //    ledPin.Dispose();
            //}
        }

        private void button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            data.Clear();
        }

        private async void btnUpdatetiming_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //periodicTimer.Change(Timeout.Infinite, Timeout.Infinite);
            // test

            try
            {
                double timingInterval = double.Parse(cmbTiming.SelectionBoxItem.ToString(), CultureInfo.InvariantCulture);
                //double timingInterval = Convert.ToDouble(cmbTiming.SelectionBoxItem.ToString());
                //this.Timing = Convert.ToInt32(cmbTiming.SelectionBoxItem.ToString());
                timer.Interval = TimeSpan.FromMilliseconds(timingInterval); //TimeSpan(0, 0, 0, 0, Convert.ToInt32(cmbTiming.SelectionBoxItem.ToString()));

                //  periodicTimer.Change(0, this.Timing);
                data.Clear();
                indexcount2 = 0;
                sumTime = 0;
            }
            catch (Exception ex)
            { this.StatusText.Text = ex.Message; }
        }
    }
}
