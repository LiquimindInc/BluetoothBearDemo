using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluetoothBear;
using Xamarin.Forms;
using System.ComponentModel;

namespace BluetoothBearDemo
{
    public class BleCyclingSensor : XBleDevice, INotifyPropertyChanged

    {

        
        public BleCyclingSensor(BleDevice dev)
            : base(dev)
        {

            this.ConnectionStateChanged += BleCyclingSensor_ConnectionStateChanged;
            this.ServicesDiscovered += BleCyclingSensor_ServicesDiscovered;
            this.CharacteristicChanged += BleCyclingSensor_CharacteristicChanged;
            WheelRev = 0;
            WheelTime = 0;
            CrankRev = 0;
            CrankTime = 0;
          



        }

        void BleCyclingSensor_CharacteristicChanged(IBleDevice sender, BleCharacteristicChangedEventArgs e)
        {
            WheelRev = BitConverter.ToUInt32(e.CharacteristicValue, 1);
            WheelTime = BitConverter.ToUInt16(e.CharacteristicValue, 5);
            CrankRev = BitConverter.ToUInt16(e.CharacteristicValue, 7);
            CrankTime = BitConverter.ToUInt16(e.CharacteristicValue, 9);


            RaisePropertyChanged("WheelRev");
            RaisePropertyChanged("WheelTime");
            RaisePropertyChanged("CrankRev");
            RaisePropertyChanged("CrankTime");
           
       

        }



        void BleCyclingSensor_ServicesDiscovered(IBleDevice sender, BleServicesDiscoveredEventArgs e)
        {
            if (e.State == BleGattOperationState.Success)
            {
                object CSCMeasurement = this.GetCharacteristic(new Guid("00002a5b-0000-1000-8000-00805f9b34fb"));
                this.EnableNotifications(CSCMeasurement, true);
            }
        }

        void BleCyclingSensor_ConnectionStateChanged(IBleDevice sender, BleConnectionStateChangedEventArgs e)
        {
            RaisePropertyChanged("ConnectionState");
            if (ConnectionState == BleConnectionState.Connected)
            {
                this.DiscoverServices();
            }
        }






        public UInt64 WheelRev
        {
            get;
            set;
        }

        public UInt64 WheelTime
        {
            get;
            set;
        }

        public UInt64 CrankRev
        {
            get;
            set;
        }

        public UInt64 CrankTime
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        // Create the OnPropertyChanged method to raise the event 
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
