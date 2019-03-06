using AMaungUs.FFUMaker.Views;
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

namespace AMaungUs.FFUMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GridCheckList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckListPage pageToDisplay = new CheckListPage();
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }

        private void GridCreateWorkSpace_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DeviceSelectionPage pageToDisplay = new DeviceSelectionPage();
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }

        private void GridDeviceInformation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DeviceInformationPage pageToDisplay = new DeviceInformationPage();
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }

        private void GridTestImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateTestImagePage pageToDisplay = new CreateTestImagePage();
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }

        private void GridInfoImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AboutPage pageToDisplay = new AboutPage();
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }
    }
}
