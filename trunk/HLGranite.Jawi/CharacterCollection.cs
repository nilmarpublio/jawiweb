using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Character collection class.
    /// </summary>
    public class CharacterCollection : PathCollection
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public CharacterCollection()
            : base("characters")
        {
        }
        /// <summary>
        /// Return number match exactly otherwise 0.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int Match(string name)
        {
            return base.MatchExactly(name);
        }
    }
}