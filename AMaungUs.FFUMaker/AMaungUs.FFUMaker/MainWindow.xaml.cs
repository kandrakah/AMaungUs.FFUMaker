using AMaungUs.FFUMaker.ViewModels;
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

        private void GridPrerequesite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PrerequisitePage pageToDisplay = new PrerequisitePage();
            pageToDisplay.DataContext = ((MainWindowViewModel)this.DataContext).PreReqVM;
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }

        private void GridCreateWorkSpace_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WorkspacePage pageToDisplay = new WorkspacePage();
            pageToDisplay.DataContext = ((MainWindowViewModel)this.DataContext).WorkspaceVM;
            ((MainWindowViewModel)this.DataContext).WorkspaceVM.SelectedWorkspace = null;
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }

        private void GridCreateProduct_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DisplaySelectedWorkspace();
        }
        private void DisplaySelectedWorkspace()
        {
            ProductPage pageToDisplay = new ProductPage();
            pageToDisplay.DataContext = new ProductViewModel(((MainWindowViewModel)this.DataContext).WorkspaceVM.SelectedWorkspace);
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(pageToDisplay);
        }
        private void GridDeviceInformation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BSPPage pageToDisplay = new BSPPage();
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

        private void GridCreateProduct_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GridDeviceInformation_MouseDown(sender, null);
            DisplaySelectedWorkspace();
        }
    }
}
