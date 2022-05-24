using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargeControlBluetoothMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChargePage : ContentPage
    {
        public ChargePage()
        {
            InitializeComponent();
        }
        private IDevice conDevice;
        private async void GetCharge()
        {
            IService service;
            conDevice = (IDevice)BindingContext;
            try
            {
                service = await conDevice.GetServiceAsync(Guid.Parse("0000180F-0000-1000-8000-00805F9B34FB"));
            }
            catch (Exception ex)
            {
               await DisplayAlert("Error", ex.Message,"Ok");
            }
            var characteristics = await service.GetCharacteristicsAsync();
            var characteristic = characteristics[0];

            ChargeText.Text = characteristic.StringValue;
        }

        private void GetCharge_btn(object sender, EventArgs e)
        {
            GetCharge();
        }
    }
}