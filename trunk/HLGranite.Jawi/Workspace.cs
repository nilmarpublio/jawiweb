using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class Workspace
    {
        #region Properties
        /// <summary>
        /// Gets or sets PathViewModel collection for this workspace.
        /// </summary>
        public ObservableCollection<PathViewModel> Items { get; set; }
        /// <summary>
        /// Source folder to read from.
        /// </summary>
        protected string source;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Workspace(string source)
        {
            this.source = source;
            Initialize();
        }

        #region Methods
        protected void Initialize()
        {

        }
        public void Sort()
        {
        }
        public void Search()
        {
        }
        #endregion
    }
}