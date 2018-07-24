using System.Collections.Generic;
using System.Linq;

namespace Plugins.Collections
{
    public static class CollectionExtensions
    {
        public static T Front<T>(this Deque<T> deque) => deque.First();
        public static T Back<T>(this Deque<T> deque) => deque.Last();
    }
}