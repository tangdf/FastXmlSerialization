using System;

namespace FastXmlSerialization.UnitTest
{
    public class TestEntity
    {
        /// <summary>
        /// 类型代码varchar6位及以下不能使用中文
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 各类中文使用varchar6位以上
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 时间类型YYYY-MM-DD HH:mm:ss
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 结算金额保留两位小数
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 明细金额保留4位小数
        /// </summary>
        public decimal DetailPrice { get; set; }

        //public static bool operator ==(TestEntity entityX, TestEntity entityY)
        //{
        //    return true;
        //}

        //public static bool operator !=(TestEntity entityX, TestEntity entityY)
        //{
        //    return !(entityX == entityY);
        //}
    }
}
