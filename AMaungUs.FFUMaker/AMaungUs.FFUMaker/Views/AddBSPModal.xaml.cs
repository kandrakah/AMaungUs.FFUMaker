using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMaungUs.FFUMaker.Views
{
    /// <summary>
    /// Interaction logic for DeviceInformationPage.xaml
    /// </summary>
    public partial class AddBSPModal : UserControl
    {
        public AddBSPModal()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            
            if (text != null)
                switch (text.ToLower())
                {
                    case "raspberry pi":
                        //imgDevice.Source = new BitmapImage(new Uri(@"/AMaungUs.FFUMaker;component/Assets/boards/raspberrypi.png"));
                        break;
                    case "dragon board 410c":
                        //imgDevice.Source = new BitmapImage(new Uri("Assets/boards/dragonboard.png"));
                        break;
                    default:
                        //imgDevice.Source = new BitmapImage(new Uri("Assets/boards/intelbsw.png"));
                        break;
                }
        }
    }
}
