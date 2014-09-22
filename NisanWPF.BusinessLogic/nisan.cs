using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NisanWPF.BusinessLogic
{
    public partial class nisan
    {
        public ObservableCollection<nisanOrder> Orders { get; set; }
        public ObservableCollection<nisanInvoice> Invoices { get; set; }
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
}