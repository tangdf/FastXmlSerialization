using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FastXmlSerialization
{
    /// <summary>
    /// 表示金额标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AmountAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDetail">表示是否为明细类金额</param>
        public AmountAttribute(bool isDetail)
        {
            this.IsDetail = isDetail;
        }

        /// <summary>
        /// 表示是否为明细类金额
        /// </summary>
        public bool IsDetail { get; }
    }
}