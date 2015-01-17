using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BluetoothBearDemo.Views
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            BindingContext = ((App)Application.Current).Locator.ScanVM;
            deviceListView.RowHeight = 400;
        }
    }
}
