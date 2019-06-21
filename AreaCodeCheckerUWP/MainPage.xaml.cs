using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AreaCodeCheckerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private delegate Task<string> GetCityDelegate();
        private delegate void PopulateCityDelegate(string city);
        public MainPage()
        {
            InitializeComponent();
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string city = "";
            AreaCode area = new AreaCode(txt_Code.Text);
            city = await area.GetCityAsync();

            txt_City.Text = city;
        }

        private void Txt_Code_KeyUp(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            //WindowsFunctions.SetWindowTopmost();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //WindowsFunctions.SetWindowNotTopmost();
        }

        private void chkTransparent_Checked(object sender, RoutedEventArgs e)
        {

            //WindowsFunctions.SetWindowTransparency(100);
        }

        private void chkTransparent_Unchecked(object sender, RoutedEventArgs e)
        {
            //WindowsFunctions.SetWindowTransparency(0);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();

        }
    }
}
