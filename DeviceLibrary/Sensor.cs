using DeviceLibrary.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLibrary
{
   public  class Sensor
    {  

        public SensorValues GetSensorValue(string deviceId)
        {
            SensorValues s = new SensorValues();
            s.DeviceId = deviceId;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();                
                s.Temp = getTemperature();
                sw.Stop();

                s.Timewatch= sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));

                s.Volt = getVolts();
                s.StatusDevice = "On";

                return s;
            }
            catch (Exception exc)
            {
                s.StatusDevice = "Error: " + exc.Message;

                s.Temp = "nd";
                s.Volt = "nd";
                return s;
            }
            finally
            { }

            return null;
        }


        /**************************************************************************************/
        /******************                Metodi Statici                **********************/
        /**************************************************************************************/


        public string getTemperature()
        {
            try
            {
                double avgTemp = 21; // m/s
                Random rand = new Random();

                //double currentTemperature = avgTemp + rand.NextDouble() * 4 - 2;
                double currentTemperature = SensorHelper.GetSensorTemperature();
                return currentTemperature.ToString("F");

            }
            catch (Exception exc)
            {
                return "nd";
            }


        }
        private string getVolts()
        {
            try
            {
                double avgVolt = 220; // m/s
                Random rand = new Random();

                double currentVolts = avgVolt + rand.NextDouble() * 4 - 4;

                return currentVolts.ToString("F");

            }
            catch (Exception exc)
            {
                return "nd";
            }


        }

    }
}
