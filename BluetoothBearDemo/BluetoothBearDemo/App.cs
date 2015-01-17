using BluetoothBearDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BluetoothBearDemo
{
    public class App : Application
    {

        private ViewModelLocator _locator;

        public App()
        {
            _locator = new ViewModelLocator();
            MainPage = new NavigationPage(new ScanPage());
        }

        public ViewModelLocator Locator
        {
            get
            {
                return _locator;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
