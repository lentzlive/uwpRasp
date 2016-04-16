using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace DeviceLibrary
{
    public class SensorHelper
    {
        enum AdcDevice { MCP3008 };
        private const string SPI_CONTROLLER_NAME = "SPI0";
        private const Int32 SPI_CHIP_SELECT_LINE = 0;
        private static SpiDevice SpiADC;

        // channel 0= 0x80
        // channel 1= 0x90

        private static readonly byte[] MCP3008_CONFIG = { 0x01, 0x80 }; 

        private Timer periodicTimer;
        private double adcValue;

        //
        private string _temperatureFromSensor = string.Empty;
        private string _message = string.Empty;
        public string TemperatureFromSensor
        {
            get
            { return _temperatureFromSensor; }

            set
            { _temperatureFromSensor = value; }
        }
        public string Message
        {
            get
            { return _message; }

            set
            { _message = value; }
        }
        //
        private const int LED_PIN = 4; // Use pin 12 if you are using DragonBoard
        private const int LED_PIN2 = 5; // Use pin 12 if you are using DragonBoard

        private GpioPin ledPin;
    private GpioPin ledPin2;

        public SensorHelper()
        {
            InitAll();
        }

        private async void InitAll()
        {
            string status = "";
            //if (ADC_DEVICE == AdcDevice.NONE)
            //{
            //    status = "Please change the ADC_DEVICE variable to either MCP3002 or MCP3208, or MCP3008";
            //    Message = "Please change the ADC_DEVICE variable to either MCP3002 or MCP3208, or MCP3008";
            
            //    return;
            //}

            try
            {
                InitGpio();         /* Initialize GPIO to toggle the LED                          */
                await InitSPI();    /* Initialize the SPI bus for communicating with the ADC      */

            }
            catch (Exception ex)
            {
                status = ex.Message;
                //StatusText.Text = ex.Message;
                Message = ex.Message;
                return;
            }

          await  System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3));

        }

        private async Task InitSPI()
        {
            try
            {
           
                var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE);
                settings.ClockFrequency = 1000000;// 1000000;// 10000000;// 3600000;// 1000000;// 500000;
                settings.Mode = SpiMode.Mode0;

                string spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);
                var deviceInfo = await DeviceInformation.FindAllAsync(spiAqs);
                SpiADC = await SpiDevice.FromIdAsync(deviceInfo[0].Id, settings);               
             
                ledPin2.Write(GpioPinValue.High);
             
            }

            catch (Exception ex)
            {
                Message = ex.Message;
                //throw new Exception("Lux:", ex);
            }
        }
        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            /* Show an error if there is no GPIO controller */
            if (gpio == null)
            {
                throw new Exception("There is no GPIO controller on this device");
            }

            ledPin = gpio.OpenPin(LED_PIN);
            ledPin2 = gpio.OpenPin(LED_PIN2);
            /* GPIO state is initially undefined, so we assign a default value before enabling as output */
            ledPin.Write(GpioPinValue.High);
            ledPin2.Write(GpioPinValue.Low);
            ledPin.SetDriveMode(GpioPinDriveMode.Output);
            ledPin2.SetDriveMode(GpioPinDriveMode.Output);

        }

        private void Timer_Tick(object state)
        {
            //  ReadADC();
            string i = "0";
        }
        public void ReadADC()
        {
            byte[] readBuffer = new byte[3];
            byte[] writeBuffer = new byte[3] { 0x00, 0x00, 0x00 };

            writeBuffer[0] = MCP3008_CONFIG[0];
            writeBuffer[1] = MCP3008_CONFIG[1];

            SpiADC.TransferFullDuplex(writeBuffer, readBuffer);
            adcValue = ConvertTemperatureToDouble(readBuffer);

            //var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    StatusText.Text = "Lux:";
            //    textPlaceHolder.Text = valoreLux;  // lux in virgola mobile
            //    //textPlaceHolder.Text = adcValue.ToString();  // decommentare per il valore intero 
            //});
        }

        public static double GetSensorTemperature()
        {

            byte[] readBuffer = new byte[3]; /* Buffer to hold read data*/
            byte[] writeBuffer = new byte[3] { 0x00, 0x00, 0x00 };
            writeBuffer[0] = MCP3008_CONFIG[0];
            writeBuffer[1] = MCP3008_CONFIG[1];

            try
            {
                SpiADC.TransferFullDuplex(writeBuffer, readBuffer); /* Read data from the ADC                           */

                return ConvertTemperatureToDouble(readBuffer);
            }
            catch (Exception exc)
            {
                return 0.0;
            }

        }
        public static double ConvertTemperatureToDouble(byte[] data)
        {
            int result = 0;
            double TempDouble = 0.0;
            try
            {
                result = data[1] & 0x03;
                result <<= 8;
                result += data[2];
                double step = 3.3 / 1024;    // Step di lettura (tensione di riferimento 3,3 / 1024 livelli) 
                TempDouble = result * step * 100;  // Gradi centigradi 

                return TempDouble;
            }
            catch (Exception exc)
            {
                return 0.0;
            }

        }

    }
}
