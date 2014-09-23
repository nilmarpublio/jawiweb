using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace NisanWPF.BusinessLogic
{
    public partial class nisan
    {
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ObservableCollection<nisanOrder> Orders { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ObservableCollection<nisanInvoice> Invoices { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ObservableCollection<nisanPurchase> Purchases { get; set; }

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
            nisanOrder order = new nisanOrder();
            order.date = DateTime.Today.ToString("yyyy-MM-dd");
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
        /// Gets total amount of nisan orders.
        /// </summary>
        public decimal totalSales
        {
            get
            {
                return this.Orders.Sum(o => o.price);
            }
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
}