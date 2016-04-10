using DeviceLibrary;
using DeviceLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uwpRasp.Utilities;
using Windows.UI.Core;

namespace uwpRasp.Classes
{

    public class MainViewModel : ViewModelBase
    {
        SocketAsyncEventArgs socketEventArg;

        Timer receiveTimer;
        DelegateCommand _Receive;
        public DelegateCommand Receive
        {
            get
            {
                return _Receive;
            }
        }
        DelegateCommand _Disconnect;
        public DelegateCommand Disconnect
        {
            get
            {
                return _Disconnect;
            }
        }

        public CoreDispatcher Dispatcher { get; set; }

        string _Status;
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                RaisePropertyChanged("Status");
            }
        }

        public MainViewModel()
        {
            _Receive = new DelegateCommand(async (x) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                   

                    // this.Status = "Listening";
                });
                receiveTimer = new Timer(this.Receive_Timer_Tick, null, 0, System.Threading.Timeout.Infinite);
            }, (y) => { return true; });
        }

        private async void Receive_Timer_Tick(object state)
        {
            receiveTimer.Dispose();
            Sensor s = new Sensor();
            SensorValues sValues = s.GetSensorValue("myFirstDevice");

            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {


            });


        }
    }
}
