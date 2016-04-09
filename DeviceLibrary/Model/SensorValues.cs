using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceLibrary.Model
{
    public class SensorValues
    {


        private string _deviceId = string.Empty;
        private double _timewatch = 0.00;

        private string _Temp = string.Empty;
        private string _Volt = string.Empty;
        private string _statusDevice = string.Empty;

        public string DeviceId
        {
            get
            {
                return _deviceId;
            }

            set
            {
                _deviceId = value;
            }
        }

        public string Temp
        {
            get
            {
                return _Temp;
            }

            set
            {
                _Temp = value;
            }
        }

        public string Volt
        {
            get
            {
                return _Volt;
            }

            set
            {
                _Volt = value;
            }
        }

        public string StatusDevice
        {
            get
            {
                return _statusDevice;
            }

            set
            {
                _statusDevice = value;
            }
        }

        public double Timewatch
        {
            get
            {
                return _timewatch;
            }

            set
            {
                _timewatch = value;
            }
        }
    }
}
