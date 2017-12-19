using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastXmlSerialization.UnitTest
{
    public class FastXmlIgnoreEntity
    {
        public string StringValue { get; set; }

        [FastXmlIgnore]
        public Int32 Int32Value { get; set; }
    }
}
