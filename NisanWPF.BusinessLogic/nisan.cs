using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using HLGranite.Jawi;

namespace NisanWPF.BusinessLogic
{
    public partial class nisan
    {
        public static MuslimCalendar Calendar;

        /// <summary>
        /// Gets total amount of nisan orders.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public decimal totalSales
        {
            get
            {
                if (this.ordersView == null)
                    return this.Orders.Sum(o => o.price);
                else
                    return this.ordersView.Cast<nisanOrder>().Sum(o => o.price);
            }
        }

        /// <summary>
        /// Get total found based on filter peformed.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public int totalFound
        {
            get
            {
                if (this.ordersView == null)
                    return this.Orders.Count;
                else
                    return this.ordersView.Cast<nisanOrder>().Count();
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ObservableCollection<nisanOrder> Orders { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ObservableCollection<nisanInvoice> Invoices { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ObservableCollection<nisanPurchase> Purchases { get; set; }

        private ICollectionView ordersView;
        public ICollectionView OrdersView
        {
            get
            {
                this.ordersView = CollectionViewSource.GetDefaultView(this.Orders);
                return this.ordersView;
            }
        }

        private FilterPendingOrderCommand filterPendingOrderCommand;
        public FilterPendingOrderCommand FilterPendingOrderCommand { get { return this.filterPendingOrderCommand; } }
        public void FilterPendingOrder()
        {
            System.Diagnostics.Debug.WriteLine("FilterPendingOrder");
            this.ordersView.Filter = item =>
                {
                    return string.IsNullOrEmpty((item as nisanOrder).delivered);
                };
            this.OnPropertyChanged("totalSales");
            this.OnPropertyChanged("totalFound");
        }

        private FilterNameCommand filterNameCommand;
        public FilterNameCommand FilterNameCommand { get { return this.filterNameCommand; } }
        /// <summary>
        /// Search on nisan name.
        /// </summary>
        /// <param name="name"></param>
        public void FilterName(string name)
        {
            System.Diagnostics.Debug.WriteLine("FilterName(" + name + ")");
            if (string.IsNullOrEmpty(name))
            {
                ResetFilter();
            }
            else
            {
                this.ordersView.Filter = item =>
                    {
                        return (item as nisanOrder).name.ToLower().Contains(name);
                    };
                this.OnPropertyChanged("totalSales");
                this.OnPropertyChanged("totalFound");
            }
        }

        private ResetFilterCommand resetFilterCommand;
        public ResetFilterCommand ResetFilterCommand { get { return this.resetFilterCommand; } }
        public void ResetFilter()
        {
            System.Diagnostics.Debug.WriteLine("ResetFilter");
            // didn't work
            //this.ordersView.Refresh();
            this.ordersView.Filter = item => { return true; };
            this.OnPropertyChanged("totalSales");
            this.OnPropertyChanged("totalFound");
        }

        /// <summary>
        /// nisan class constructor
        /// </summary>
        public nisan()
        {
            this.itemsField = new ObservableCollection<object>();
            this.Orders = new ObservableCollection<nisanOrder>();
            this.Invoices = new ObservableCollection<nisanInvoice>();
            this.Purchases = new ObservableCollection<nisanPurchase>();
            this.createOrderCommand = new CreateOrderCommand(this);
            this.removeOrderCommand = new RemoveOrderCommand(this);
            this.resetFilterCommand = new ResetFilterCommand(this);
            this.filterPendingOrderCommand = new FilterPendingOrderCommand(this);
            this.filterNameCommand = new FilterNameCommand(this);
            Calendar = new MuslimCalendar("muslimcal.xml");
        }
        
        public static void Initialize(nisan nisan)
        {
            foreach (object obj in nisan.Items)
            {
                if (obj is nisanOrder)
                    nisan.Orders.Add(obj as nisanOrder);
                else if (obj is nisanInvoice)
                    nisan.Invoices.Add(obj as nisanInvoice);
                else if (obj is nisanPurchase)
                    nisan.Purchases.Add(obj as nisanPurchase);
            }
        }

        private CreateOrderCommand createOrderCommand;
        public CreateOrderCommand CreateOrderCommand { get { return this.createOrderCommand; } }
        public void CreateOrder()
        {
            // TODO: Use last nisanOrder stock object. Mostly new orders are in same stock bulk to a customer.
            nisanOrder order = new nisanOrder();
            order.soldto = "";
            order.item = "";
            order.date = DateTime.Today.ToString("yyyy-MM-dd");
            order.delivered = "";
            order.price = 0m;
            order.death = "";
            order.name = "";
            this.itemsField.Add(order);
            this.Orders.Add(order);
        }

        private RemoveOrderCommand removeOrderCommand;
        public RemoveOrderCommand RemoveOrderCommand { get { return this.removeOrderCommand; } }
        public void RemoveOrder(nisanOrder order)
        {
            this.itemsField.Remove(order);
            this.Orders.Remove(order);
        }

        /// <summary>
        /// TODO: Commit nisan.xml to svn repo.
        /// </summary>
        public void Commit()
        {
        }
    }

    public class CreateOrderCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            manager.CreateOrder();
        }

        private nisan manager;
        public CreateOrderCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class RemoveOrderCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            this.manager.RemoveOrder((nisanOrder)parameter);
        }

        private nisan manager;
        public RemoveOrderCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class FilterPendingOrderCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter is bool)
            {
                if (Convert.ToBoolean(parameter) == true)
                {
                    manager.FilterPendingOrder();
                }
            }
            
        }
        private nisan manager;
        public FilterPendingOrderCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class FilterNameCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.FilterName(parameter.ToString());
        }

        private nisan manager;
        public FilterNameCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class ResetFilterCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            // bind to checkbox use
            if (parameter is bool)
            {
                if (Convert.ToBoolean(parameter) == true)
                {
                    manager.ResetFilter();
                }

                return;
            }

            // bind to button use
            manager.ResetFilter();
        }
        private nisan manager;
        public ResetFilterCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

}