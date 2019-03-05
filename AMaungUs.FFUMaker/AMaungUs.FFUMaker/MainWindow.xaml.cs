﻿using AMaungUs.FFUMaker.Views;
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
            CheckListPage checkListPage = new CheckListPage();
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(checkListPage);
        }

        private void GridCreateWorkSpace_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LandingPage checkListPage = new LandingPage();
            MainDisplay.Children.Clear();
            MainDisplay.Children.Add(checkListPage);
        }
    }
}
