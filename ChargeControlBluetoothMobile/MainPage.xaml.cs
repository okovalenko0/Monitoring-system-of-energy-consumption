using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargeControlBluetoothMobile
{
    public partial class MainPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;

        private IDevice device;

        public MainPage()
        {
            InitializeComponent();

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();

            DevicesList.ItemsSource = deviceList;
        }



        private async void ScanDevice(object sender, EventArgs e)
        {

            if (ble.State == BluetoothState.Off)
            {
                await DisplayAlert("attention", "bluetooth off", "ok");
            }
            else
            {
                deviceList.Clear();

                adapter.ScanTimeout = 10000;
                adapter.ScanMode = ScanMode.Balanced;


                adapter.DeviceDiscovered += (obj, a) =>
                {
                    if (!deviceList.Contains(a.Device))
                        deviceList.Add(a.Device);
                };

                await adapter.StartScanningForDevicesAsync();

            }
        }

        private async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            device = DevicesList.SelectedItem as IDevice;

            var result = await DisplayAlert("WARNING", "Do you wish to connect to this device?", "Connect", "Cancel");

            if (!result)
                return;

            //Stop Scanner
            await adapter.StopScanningForDevicesAsync();

            try
            {
                await adapter.ConnectToDeviceAsync(device);

                await DisplayAlert("Connected", "Status:" + device.State, "OK");

                await Navigation.PushAsync(new ChargePage { BindingContext = device });

            }
            catch (DeviceConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

        }

       
    }
}
