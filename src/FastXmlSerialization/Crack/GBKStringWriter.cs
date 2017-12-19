using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace FastXmlSerialization
{
    [ExcludeFromCodeCoverage]
    internal sealed class GBKStringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return GBKEncoding.Instance; }
        }

        public GBKStringWriter(StringBuilder sb) : base(sb)
        {
        }
    }
}