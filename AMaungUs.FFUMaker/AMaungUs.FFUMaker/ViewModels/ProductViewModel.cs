﻿using AMaungUs.FFUMaker.Models;
using AMaungUs.FFUMaker.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        ObservableCollection<Workspace> workspaces;
        Workspace _workspace;
        public string Title
        {
            get
            {
                return SelectedWorkspace.Name + " - Product(s)";
            }
        }
        public Workspace SelectedWorkspace
        {
            get
            {
                return _workspace == null ? new Workspace() : _workspace;
            }
            set
            {
                SetProperty(ref _workspace, value);
                OnPropertyChanged("Title");
            }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct == null ? new Product(): selectedProduct; }
            set { SetProperty(ref selectedProduct, value); }
        }
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products == null ? new ObservableCollection<Product>() : products; }
            set { SetProperty(ref products, value); }
        }
        public ProductViewModel(Workspace workspace)
        {
            SelectedWorkspace = workspace;
            var savedjson = Properties.Settings.Default.Workspaces;
            var _workspaces = JsonConvert.DeserializeObject<ObservableCollection<Workspace>>(savedjson);
            workspaces = _workspaces;
            LoadProducts();
        }
        System.Windows.Input.ICommand selectCommand;
        public ICommand SelectCommand
        {
            get { return selectCommand == null ? new DelegateCommand<object>(this.SelectCommandExec, x => true) : selectCommand; }
            set { SetProperty(ref selectCommand, value); }
        }
        private void SelectCommandExec(object parm)
        {
            SelectedProduct = (Product)parm;
        }
        private void LoadProducts()
        {
            var productsPath = SelectedWorkspace.Path + "\\" + SelectedWorkspace.Name + "\\Source-Arm\\Products";
            if (Directory.Exists(productsPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(productsPath);
                Products = new ObservableCollection<Product>(directoryInfo.GetDirectories().Select(p => new Product() { Path = p.FullName }));
            }
        }
        System.Windows.Input.ICommand createproductcommand;
        public ICommand CreateProductCommand
        {
            get { return createproductcommand == null ? new DelegateCommand<object>(this.CreateProductCmdExec, x => true) : createproductcommand; }
            set { SetProperty(ref createproductcommand, value); }
        }
        private void CreateProductCmdExec(object parm)
        {
            CreateProductModal createModal = new CreateProductModal();
            ((CreateProductViewModel)createModal.DataContext).ws = SelectedWorkspace;
            Nullable<bool> dialogresult = createModal.ShowDialog();
            if (dialogresult.Value == true)
            {
                LoadProducts();
            }
        }
        System.Windows.Input.ICommand deleteproductcommand;
        public ICommand DeleteProductCommand
        {
            get { return deleteproductcommand == null ? new DelegateCommand<object>(this.DelProductCommandExec, x => true) : deleteproductcommand; }
            set { SetProperty(ref deleteproductcommand, value); }
        }
        System.Windows.Input.ICommand createtestimagecommand;
        public ICommand CreateTestImageCommand
        {
            get { return createtestimagecommand == null ? new DelegateCommand<object>(this.CreateTestImageCommandExec, x => true) : createtestimagecommand; }
            set { SetProperty(ref createtestimagecommand, value); }
        }
        private void CreateTestImageCommandExec(object parm)
        {
            if (SelectedProduct != null)
            {
                CreateTestImageModal createModal = new CreateTestImageModal();
                ((CreateTestImageViewModel)createModal.DataContext).ws = SelectedWorkspace;
                ((CreateTestImageViewModel)createModal.DataContext).ProductName = SelectedProduct.ProductName;
                Nullable<bool> dialogresult = createModal.ShowDialog();
                if (dialogresult.Value == true)
                {
                    LoadProducts();
                }
            }
        }
        private void DelProductCommandExec(object parm)
        {
            if (parm is Product)
            {
                var product = (Product)parm;
                if (Directory.Exists(product.Path))
                {
                    try
                    {
                        Directory.Delete(product.Path, true);
                    }
                    catch(Exception ex)
                    {

                    }
                    finally
                    {
                        Products.Remove(product);
                    }
                }
            }
        }
    }
}
