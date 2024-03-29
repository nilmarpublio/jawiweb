﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

using SharpSvn;
using HLGranite.Jawi;

namespace NisanWPF.BusinessLogic
{
    public partial class nisan
    {
        private const string FILENAME = "nisan.xml";
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
        public ICollectionView OrdersView { get { return this.ordersView; } }

        private FilterPendingOrderCommand filterPendingOrderCommand;
        public FilterPendingOrderCommand FilterPendingOrderCommand { get { return this.filterPendingOrderCommand; } }
        public void FilterPendingOrder(DateTime from, DateTime to)
        {
            System.Diagnostics.Debug.WriteLine("FilterPendingOrder");
            this.ordersView.Filter = item =>
                {
                    return string.IsNullOrEmpty((item as nisanOrder).delivered)
                        && ((item as nisanOrder).date.CompareTo(from.ToString("yyyy-MM-dd")) >= 0)
                        && ((item as nisanOrder).date.CompareTo(to.ToString("yyyy-MM-dd")) <= 0);
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

        /// <summary>
        /// Filter soldto customer.
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="isPending"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <remarks>
        /// TODO: Combine all filter methods into one.
        /// </remarks>
        public void FilterSoldTo(string[] customers, bool isPending, DateTime from, DateTime to)
        {
            if (isPending)
            {
                System.Diagnostics.Debug.WriteLine("Filter pending customer");
                this.ordersView.Filter = item =>
                {
                    return customers.Contains((item as nisanOrder).soldto) && string.IsNullOrEmpty((item as nisanOrder).delivered)
                        && ((item as nisanOrder).date.CompareTo(from.ToString("yyyy-MM-dd")) >= 0)
                        && ((item as nisanOrder).date.CompareTo(to.ToString("yyyy-MM-dd")) <= 0);
                };
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("FilterCustomer");
                this.ordersView.Filter = item =>
                {
                    return customers.Contains((item as nisanOrder).soldto)
                        && ((item as nisanOrder).date.CompareTo(from.ToString("yyyy-MM-dd")) >= 0)
                        && ((item as nisanOrder).date.CompareTo(to.ToString("yyyy-MM-dd")) <= 0);
                };
            }
            this.OnPropertyChanged("totalSales");
            this.OnPropertyChanged("totalFound");
        }

        /// <summary>
        /// Custom filtering.
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>
        /// Dependent on Filter.
        /// </remarks>
        public void Filtering(Filter filter)
        {
            // Refine customer selection filter
            List<string> selectedCustomers = new List<string>();
            for (int i = 1; i < filter.Rules.Count - 1; i++)
            {
                if (filter.Rules[i].IsChecked)
                    selectedCustomers.Add(filter.Rules[i].Name);
            }

            if (filter.HasDateRange)
            {
                int last = filter.Rules.Count - 1;
                DateTime from = (filter.Rules[last] as FilterDateRule).From;
                DateTime to = (filter.Rules[last] as FilterDateRule).To;
                if (selectedCustomers.Count == 0)
                {
                    if (filter.IsPending)
                    {
                        FilterPendingOrder(from, to);
                    }
                }
                else
                {
                    FilterSoldTo(selectedCustomers.ToArray(), filter.IsPending, from, to);
                }
            }
            else
            {
                // Based on filter flag define the filter rules
                if (selectedCustomers.Count == 0)
                {
                    if (filter.IsPending)
                    {
                        FilterPendingOrder(DateTime.MinValue, DateTime.MaxValue);
                    }
                }
                else
                {
                    FilterSoldTo(selectedCustomers.ToArray(), filter.IsPending, DateTime.MinValue, DateTime.MaxValue);
                }
            }
        }

        private ResetFilterCommand resetFilterCommand;
        public ResetFilterCommand ResetFilterCommand { get { return this.resetFilterCommand; } }
        public void ResetFilter()
        {
            System.Diagnostics.Debug.WriteLine("ResetFilter");
            //this.ordersView.Refresh(); // didn't work
            this.ordersView.Filter = item => { return true; };
            this.ordersView.SortDescriptions.Clear();
            this.OnPropertyChanged("totalSales");
            this.OnPropertyChanged("totalFound");
        }

        private SortSoldToCommand sortSoldToCommand;
        public SortSoldToCommand SortSoldToCommand { get { return this.sortSoldToCommand; } }
        public void SortSoldTo()
        {
            bool hasAscending = false;
            foreach (SortDescription sortDesc in this.ordersView.SortDescriptions)
            {
                if (sortDesc.PropertyName == "soldto")
                    hasAscending = (sortDesc.Direction == ListSortDirection.Ascending);
            }

            this.ordersView.SortDescriptions.Clear();
            if (hasAscending)
            {
                System.Diagnostics.Debug.WriteLine("SortSoldTo desc");
                this.ordersView.SortDescriptions.Add(new SortDescription("soldto", ListSortDirection.Descending));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SortSoldTo asc");
                this.ordersView.SortDescriptions.Add(new SortDescription("soldto", ListSortDirection.Ascending));
            }
            this.ordersView.SortDescriptions.Add(new SortDescription("date", ListSortDirection.Ascending));

            this.OnPropertyChanged("totalSales");
            this.OnPropertyChanged("totalFound");
        }

        private SortItemCommand sortItemCommand;
        public SortItemCommand SortItemCommand { get { return this.sortItemCommand; } }
        public void SortItem()
        {
            bool hasAscending = false;
            foreach (SortDescription sortDesc in this.ordersView.SortDescriptions)
            {
                if (sortDesc.PropertyName == "item")
                    hasAscending = (sortDesc.Direction == ListSortDirection.Ascending);
            }

            this.ordersView.SortDescriptions.Clear();
            if (hasAscending)
            {
                System.Diagnostics.Debug.WriteLine("SortItem desc");
                this.ordersView.SortDescriptions.Add(new SortDescription("item", ListSortDirection.Descending));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SortItem asc");
                this.ordersView.SortDescriptions.Add(new SortDescription("item", ListSortDirection.Ascending));
            }
            this.ordersView.SortDescriptions.Add(new SortDescription("date", ListSortDirection.Ascending));

            this.OnPropertyChanged("totalSales");
            this.OnPropertyChanged("totalFound");
        }

        private SortDateCommand sortDateCommand;
        public SortDateCommand SortDateCommand { get { return this.sortDateCommand; } }
        public void SortDate()
        {
            bool hasAscending = false;
            foreach (SortDescription sortDesc in this.ordersView.SortDescriptions)
            {
                if (sortDesc.PropertyName == "date")
                    hasAscending = (sortDesc.Direction == ListSortDirection.Ascending);
            }

            this.ordersView.SortDescriptions.Clear();
            if (hasAscending)
            {
                System.Diagnostics.Debug.WriteLine("SortDate desc");
                this.ordersView.SortDescriptions.Add(new SortDescription("date", ListSortDirection.Descending));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SortDate asc");
                this.ordersView.SortDescriptions.Add(new SortDescription("date", ListSortDirection.Ascending));
            }
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
            this.Orders.CollectionChanged += Orders_CollectionChanged;
            this.NewItems = new ObservableCollection<nisanOrder>();
            this.Invoices = new ObservableCollection<nisanInvoice>();
            this.Purchases = new ObservableCollection<nisanPurchase>();

            this.createOrderCommand = new CreateOrderCommand(this);
            this.removeOrderCommand = new RemoveOrderCommand(this);
            this.generateSvgCommand = new GenerateSvgCommand(this);
            this.saveCommand = new SaveCommand(this);
            this.commitSvnCommand = new CommitSvnCommand(this);
            this.resetFilterCommand = new ResetFilterCommand(this);
            this.filterPendingOrderCommand = new FilterPendingOrderCommand(this);
            this.filterNameCommand = new FilterNameCommand(this);
            this.sortSoldToCommand = new SortSoldToCommand(this);
            this.sortItemCommand = new SortItemCommand(this);
            this.sortDateCommand = new SortDateCommand(this);

            Calendar = new MuslimCalendar("muslimcal.xml");
        }

        private void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("totalSales");
            this.OnPropertyChanged("totalFound");
        }

        public void Initialize(nisan nisan)
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
            this.ordersView = CollectionViewSource.GetDefaultView(this.Orders);

            try
            {
                // get last updated date
                string oldFile = ".svn" + Path.DirectorySeparatorChar + "text-base" + Path.DirectorySeparatorChar + FILENAME + ".svn-base";
                GetLastUpdated(oldFile);

                // create empty new item compare to last svn commit
                nisan old;
                nisan.LoadFromFile(oldFile, out old);
                //for (int i = nisan.Items.Count - 1; i < nisan.Items.Count - old.Items.Count; i--)
                //{
                //    if (nisan.Items[i] is nisanOrder)
                //        this.NewItems.Add(nisan.Items[i] as nisanOrder);
                //}
                int i = this.Orders.Count;
                while (this.NewItems.Count < nisan.Items.Count - old.Items.Count)
                {
                    this.NewItems.Add(this.Orders[i - 1]);
                    i--;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }
        }

        /// <summary>
        /// Return last updated date.
        /// </summary>
        private void GetLastUpdated(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            this.lastUpdated = fileInfo.LastWriteTime;
        }

        /// <summary>
        /// Collection of new nisanOrder.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ObservableCollection<nisanOrder> NewItems { get; set; }
        private DateTime lastUpdated;
        public DateTime LastUpdated { get { return this.lastUpdated; } }

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
            this.NewItems.Add(order);
        }

        private RemoveOrderCommand removeOrderCommand;
        public RemoveOrderCommand RemoveOrderCommand { get { return this.removeOrderCommand; } }
        public void RemoveOrder(nisanOrder order)
        {
            this.itemsField.Remove(order);
            this.Orders.Remove(order);
            this.NewItems.Remove(order);
        }

        private GenerateSvgCommand generateSvgCommand;
        public GenerateSvgCommand GenerateSvgCommand { get { return this.generateSvgCommand; } }
        public void GenerateSvg(nisanOrder order)
        {
            // check file exist if not only write
            string file = "Output" + System.IO.Path.DirectorySeparatorChar + order.name + ".svg";
            if (System.IO.File.Exists(file))
            {
                // Inkscape installed path not same at different computer.
                string filePath = @"C:\Program Files\Inkscape\inkscape.exe";
                if (System.IO.File.Exists(filePath))
                    filePath = "\"" + filePath + "\"";
                else
                {
                    filePath = filePath.Replace("Program Files", "Program Files (x86)");
                    if (System.IO.File.Exists(filePath))
                        filePath = "\"" + filePath + "\"";
                    else
                        return;
                }

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = filePath;
                startInfo.Arguments = "\"" + file + "\"";
                process.StartInfo = startInfo;
                process.Start();
                System.Diagnostics.Debug.WriteLine(filePath + " " + startInfo.Arguments);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Generating svg for " + order.name);
                string template = GetSvgTemplate(order.item);
                HLGranite.Jawi.nisanOrder jOrder = CloneOrder(order);
                SvgWriter writer = new SvgWriter(jOrder, template);
                writer.Write();
            }
        }
        private string GetSvgTemplate(string item)
        {
            string template = string.Empty;
            switch (item)
            {
                case "PV":
                    template = "tile_arabic.svg";
                    break;
                case "PV Budak":
                    template = "tile_arabic_budak.svg";
                    break;
                case "PV Egg":
                    template = "title_arabic_egg.svg";
                    break;
                case "BA":
                case "PA":
                    template = "tile_andalus.svg";
                    break;

                case "1½' Batu Batik(L)":
                case "1½' Batu Putih(L)":
                case "1½' Batu Hijau(L)":
                case "1½' Batu Hitam(L)":
                    template = "nisan_15L.svg";
                    break;
                case "1½' Batu Batik(P)":
                case "1½' Batu Putih(P)":
                case "1½' Batu Hijau(P)":
                case "1½' Batu Hitam(P)":
                    template = "nisan_15P.svg";
                    break;
                case "2' Batu Batik(L)":
                case "2' Batu Putih(L)":
                case "2' Batu Hijau(L)":
                case "2' Batu Hitam(L)":
                    template = "nisan_20L.svg";
                    break;
                case "2' Batu Batik(P)":
                case "2' Batu Putih(P)":
                case "2' Batu Hijau(P)":
                case "2' Batu Hitam(P)":
                    template = "nisan_20P.svg";
                    break;
                case "2½' Batu Batik(L)":
                case "2½' Batu Putih(L)":
                case "2½' Batu Hijau(L)":
                case "2½' Batu Hitam(L)":
                    template = "nisan_25L.svg";
                    break;
                case "2½' Batu Batik(P)":
                case "2½' Batu Putih(P)":
                case "2½' Batu Hijau(P)":
                case "2½' Batu Hitam(P)":
                    template = "nisan_25P.svg";
                    break;

                case "1½' Sticker(L)":
                    template = "sticker_15L.svg";
                    break;
                case "1½' Sticker(P)":
                    template = "sticker_15P.svg";
                    break;
                case "2' Sticker(L)":
                    template = "sticker_20L.svg";
                    break;
                case "2' Sticker(P)":
                    template = "sticker_20P.svg";
                    break;
                case "2½' Sticker(L)":
                    template = "sticker_25L.svg";
                    break;
                case "2½' Sticker(P)":
                    template = "sticker_25P.svg";
                    break;

                case "2' Tarazo(L)":
                    template = "tarazo_20L.svg";
                    break;
                case "2' Tarazo(P)":
                    template = "tarazo_20P.svg";
                    break;

                default:
                    template = "nisan_20L.svg";
                    break;
            }
            return template;
        }
        /// <summary>
        /// Clone nisanOrder to HLGranite.Jawi.nisanOrder.
        /// </summary>
        /// <remarks>
        /// TODO: Actually two classes are identical. Plan to unify these two.
        /// </remarks>
        /// <param name="source"></param>
        /// <returns></returns>
        private HLGranite.Jawi.nisanOrder CloneOrder(NisanWPF.BusinessLogic.nisanOrder source)
        {
            HLGranite.Jawi.nisanOrder target = new HLGranite.Jawi.nisanOrder();

            target.item = source.item;
            target.remarks = source.remarks;
            target.tags = source.tags;

            target.soldto = source.soldto;
            target.date = source.date;
            target.delivered = source.delivered;
            target.bill = source.bill;
            target.price = source.price;

            target.name = source.name;
            target.jawi = source.jawi;
            target.born = source.born;
            target.bornm = source.bornm;
            target.death = source.death;
            target.deathm = source.deathm;
            target.age = source.age;

            return target;
        }

        /// <summary>
        /// Mark done with delivery date or parcel no.
        /// </summary>
        public void MarkDone(nisanOrder order, DateTime delivery, string remarks)
        {
            System.Diagnostics.Debug.WriteLine("Marking done");
            order.delivered = delivery.ToString("yyyy-MM-dd");
            order.OnPropertyChanged("hasDeliver");
            if (!string.IsNullOrEmpty(remarks))
                order.remarks += remarks;
        }

        private SaveCommand saveCommand;
        public SaveCommand SaveCommand { get { return this.saveCommand; } }
        public void Save()
        {
            System.Diagnostics.Debug.WriteLine("Save");
            SaveToFile("nisan.xml");
        }

        private CommitSvnCommand commitSvnCommand;
        public CommitSvnCommand CommitSvnCommand { get { return this.commitSvnCommand; } }
        /// <summary>
        /// Commit nisan.xml to svn repo based on working copy path and authentication.
        /// </summary>
        /// <remarks>
        /// See https://sharpsvn.open.collab.net/ using ver 1.6.
        /// </remarks>
        public bool Commit()
        {
            // save to file first
            SaveToFile("nisan.xml");

            System.Diagnostics.Debug.WriteLine("svn commit nisan.xml -m \"Updating nisan order by NisanWPF.\"");
            //string url = "https://jawiweb.googlecode.com/svn/trunk/Samples";
            SvnCommitArgs args = new SvnCommitArgs();
            args.LogMessage = "Updating nisan order by NisanWPF.";
            using (SvnClient client = new SvnClient())
                return client.Commit("nisan.xml", args);//url, args);
        }

        /// <summary>
        /// Gets a list of sold to customer name.
        /// </summary>
        /// <returns></returns>
        public string[] GetSoldToList()
        {
            List<string> output = new List<string>();
            var customers = this.Orders.GroupBy(o => o.soldto).Select(group => group.First()).OrderBy(o => o.soldto);
            List<nisanOrder> collection = customers.ToList<nisanOrder>();
            foreach (nisanOrder o in collection)
                output.Add(o.soldto);
            return output.ToArray();
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
            if (parameter is nisanOrder)
                this.manager.RemoveOrder((nisanOrder)parameter);
            else if (parameter is ObservableCollection<object>)
            {
                for (int i = (parameter as ObservableCollection<object>).Count - 1; i >= 0; i--)
                {
                    nisanOrder order = (parameter as ObservableCollection<object>)[i] as nisanOrder;
                    this.manager.RemoveOrder(order);
                }
            }
        }

        private nisan manager;
        public RemoveOrderCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class GenerateSvgCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if (parameter is nisanOrder)
                this.manager.GenerateSvg((nisanOrder)parameter);
            else if (parameter is ObservableCollection<object>)
            {
                for (int i = (parameter as ObservableCollection<object>).Count - 1; i >= 0; i--)
                {
                    nisanOrder order = (parameter as ObservableCollection<object>)[i] as nisanOrder;
                    this.manager.GenerateSvg(order);
                }
            }
        }

        private nisan manager;
        public GenerateSvgCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class SaveCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.Save();
        }
        private nisan manager;
        public SaveCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class CommitSvnCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.Commit();

            // reset new item collection
            manager.NewItems.Clear();
        }
        private nisan manager;
        public CommitSvnCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class MarkDoneCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            //if (parameter is nisanOrder)
            //    this.manager.MarkDone((nisanOrder)parameter);
            //else if (parameter is ObservableCollection<object>)
            //{
            //    for (int i = (parameter as ObservableCollection<object>).Count - 1; i >= 0; i--)
            //    {
            //        nisanOrder order = (parameter as ObservableCollection<object>)[i] as nisanOrder;
            //        this.manager.MarkDone(order);
            //    }
            //}
        }

        private nisan manager;
        public MarkDoneCommand(nisan nisan)
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
                    manager.FilterPendingOrder(DateTime.MinValue, DateTime.MaxValue);
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

    public class SortSoldToCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.SortSoldTo();
        }
        private nisan manager;
        public SortSoldToCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class SortItemCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.SortItem();
        }
        private nisan manager;
        public SortItemCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

    public class SortDateCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.SortDate();
        }
        private nisan manager;
        public SortDateCommand(nisan nisan)
        {
            this.manager = nisan;
        }
    }

}