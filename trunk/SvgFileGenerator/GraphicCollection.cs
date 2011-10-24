using System.Collections.ObjectModel;
namespace HLGranite.Jawi
{
    public class GraphicCollection
    {
        protected GraphicViewModel selectedGraphic;
        protected ObservableCollection<GraphicViewModel> items;
        /// <summary>
        /// Graphic collection whether is Path or Color element in collection.
        /// </summary>
        public ObservableCollection<GraphicViewModel> Items
        {
            get { return this.items; }
            set { this.items = value; }
        }
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GraphicCollection()
        {
            this.items = new ObservableCollection<GraphicViewModel>();
        }
        protected void Select(GraphicViewModel graphicObject)
        {
            this.selectedGraphic = graphicObject;
            foreach (GraphicViewModel item in this.items)
            {
                item.IsChecked = (item.Name == graphicObject.Name)
                    ? true : false;
            }
        }
    }
}