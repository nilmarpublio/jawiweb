using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Counter class for Jawi name generated everytime.
    /// </summary>
    class Counter
    {
        //private List<Item> items;
        //public List<Item> Items { get { return this.items; } }
        private DataTable table;
        const string FILE_NAME = "jawicounter.xml";

        public Counter()
        {
            Initialize();
        }
        private void Initialize()
        {
            //this.items = new List<Item>();
            DataSet dataSet = new DataSet();
            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FILE_NAME))
            if (File.Exists(FILE_NAME))
            {
                dataSet.ReadXml(FILE_NAME);
                if (dataSet.Tables.Count > 0)
                {
                    this.table = dataSet.Tables[0].Copy();
                    //foreach (DataRow row in this.table.Rows)
                    //    this.items.Add(new Item(row["name"].ToString(), Convert.ToInt32(row["count"])));
                }
            }
            dataSet.Dispose();

            if (this.table == null) CreateSchema();
        }
        private void CreateSchema()
        {
            this.table = new DataTable();
            DataColumn col1 = new DataColumn("name", typeof(String));
            DataColumn col2 = new DataColumn("count", typeof(int));
            table.Columns.Add(col1);
            table.Columns.Add(col2);
            table.AcceptChanges();
        }
        /// <summary>
        /// Increase 1 counter.
        /// </summary>
        /// <param name="name"></param>
        public void Add(string name)
        {
            //Item item = this.items.Where(f => f.Name.Contains(name)).FirstOrDefault();
            //if (item == null)
            //    this.items.Add(new Item(name, 1));
            //else
            //    item.Count++;

            bool found = false;
            foreach (DataRow row in this.table.Rows)
            {
                if (row["name"].ToString().CompareTo(name) == 0)
                {
                    found = true;
                    int count = Convert.ToInt32(row["count"]);
                    row["count"] = count + 1;
                }
            }

            if (!found)
            {
                DataRow row = this.table.NewRow();
                row["name"] = name;
                row["count"] = 1;
                this.table.Rows.Add(row);
            }
        }
        /// <summary>
        /// Write into a static file.
        /// </summary>
        public void Store()
        {
            //this.items.ToDataTable();
            this.table.AcceptChanges();
            if (this.table != null)
            {
                this.table.WriteXml(FILE_NAME);
                this.table.Dispose();
            }
        }

        /// <summary>
        /// Item for counter.
        /// </summary>
        public class Item
        {
            private string name;
            /// <summary>
            /// Refer to counter name, here is a jawi name.
            /// </summary>
            public string Name { get { return this.name; } }

            private int count;
            /// <summary>
            /// Count for a repeating name.
            /// </summary>
            public int Count { get { return this.count; } set { this.count = value; } }
            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="count"></param>
            public Item(string name, int count)
            {
                this.name = name;
                this.count = count;
            }
        }
    }
}