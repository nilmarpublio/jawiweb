using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace HLGranite.Jawi
{
    public class Workspace
    {
        #region Properties
        private Path selectedPath;
        /// <summary>
        /// Gets or sets current selected path.
        /// </summary>
        public Path SelectedPath { get { return this.selectedPath; } }
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
        /// <summary>
        /// Select this path to indicate this path is toggle on then toggle off the rest.
        /// </summary>
        /// <param name="path"></param>
        public void Select(Path path)
        {
            this.selectedPath = path;
        }
        #endregion
    }
}