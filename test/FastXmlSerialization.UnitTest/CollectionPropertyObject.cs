using System.Collections.Generic;

namespace FastXmlSerialization.UnitTest
{
    internal class CollectionPropertyObject<T>
    {
        public List<T> Items { get; set; }
    }
}