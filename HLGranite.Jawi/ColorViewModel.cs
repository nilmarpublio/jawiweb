using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Color view model.
    /// </summary>
    public class ColorViewModel : GraphicViewModel
    {
        private SolidColorBrush color;
        /// <summary>
        /// Color value in this view model.
        /// </summary>
        public SolidColorBrush Color { get { return this.color; } }
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ColorViewModel()
        {
        }
        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="name"></param>
        public ColorViewModel(SolidColorBrush color, string name)
            : base(name)
        {
            this.color = color;
        }
    }
}