using DeviceLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using uwpRasp.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uwpRasp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageHighSpeed : Page
    {
        public MainViewModel viewModel
        {
            get
            {
                return this.DataContext as MainViewModel;
            }
        }

        SensorHelper _sensor = null;

        public PageHighSpeed()
        {
            this.InitializeComponent();
            _sensor = new SensorHelper();
            this.viewModel.Dispatcher = this.Dispatcher;
            this.Unloaded += MainPage_Unloaded;

        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.viewModel.Disconnect.CanExecute(null))
            {
                this.viewModel.Disconnect.Execute(null);
            }
        }

    }
}
