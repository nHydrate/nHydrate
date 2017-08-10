using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nHydrate.Generator
{
    /// <summary>
    /// I use this so I can catch these events
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtendedList<T> : List<T>
    {
        private Guid _id = Guid.NewGuid();

        public new void Add(T item)
        {
            base.Add(item);
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
        }
    }
}
