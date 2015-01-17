using BluetoothBear;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BluetoothBearDemo
{
    public class ScanViewModel : ViewModelBase
    {

        IBleManager manager;
        public string MainText
        {
            get { return "MCCMVMM"; }
        }


        public ScanViewModel()
        {
            try
            {
                manager = DependencyService.Get<IBleManager>();
                manager.BleDeviceFound += HandleBleDeviceFound;

            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                Debug.WriteLine(s);
                // return null;
            }

        }

        void HandleBleDeviceFound(IBleManager sender, BleDevice deviceFound)
        {

            foreach (XBleDevice dev in devices)
            {
                if (dev.ID == deviceFound.ID)
                {
                    return;
                }
            }

            if (deviceFound.Name == "Speed and Cadence" || deviceFound.Name == "CSC Sensor")
            {
                devices.Add(new BleCyclingSensor(deviceFound));
            }
            else
            {
                devices.Add(new XBleDevice(deviceFound));
            }


        }

        XBleDevice selectedDevice = null;
        public XBleDevice SelectedDevice
        {
            get { return selectedDevice; }
            set
            {

                if (selectedDevice != value)
                {
                    selectedDevice = value;
                    RaisePropertyChanged(() => SelectedDevice);
                }
            }
        }


        ObservableCollection<XBleDevice> devices = new ObservableCollection<XBleDevice>();



        public ObservableCollection<XBleDevice> Devices
        {
            get { return devices; }
        }

        private RelayCommand scanCommand;

        Task scanTask = null;



        public RelayCommand ScanCommand
        {
            get
            {
                if (scanCommand != null)
                {
                    return scanCommand;
                }
                else
                {
                    scanCommand = new RelayCommand(
                     () =>
                     {
                         scanTask = new Task(async () =>
                         {
                             manager.StartBleScan();
                             await Task.Delay(5000);
                             manager.StopBleScan();
                             scanTask = null;
                         });
                         scanTask.Start();

                     }, () =>
                     {



                         return scanTask == null;


                     });
                    return scanCommand;
                }


            }
        }

        private RelayCommand itemSelectedCommand;
        public RelayCommand ItemSelectedCommand
        {
            get
            {
                if (itemSelectedCommand != null)
                {
                    return itemSelectedCommand;
                }
                else
                {
                    itemSelectedCommand = new RelayCommand(
                        () =>
                        {
                            if (SelectedDevice.ConnectionState == BleConnectionState.Disconnected)
                            {
                                BleCyclingSensor sensor = SelectedDevice as BleCyclingSensor;
                                if (sensor != null)
                                {
                                    if (sensor.ConnectionState != BleConnectionState.Connected)
                                    {
                                        sensor.ConnectionStateChanged -= sensor_ConnectionStateChanged;
                                        sensor.ConnectionStateChanged += sensor_ConnectionStateChanged;
                                        sensor.Connect();

                                    }
                                }
                            }

                        }, () =>
                        {
                            return true; //return SelectedDevice != null;
                        });
                    return itemSelectedCommand;
                }


            }
        }

        void sensor_ConnectionStateChanged(IBleDevice sender, BleConnectionStateChangedEventArgs e)
        {

            if (e.NewState == BleConnectionState.Disconnected)
            {
                this.SelectedDevice = null;

            }
        }



    }
}
