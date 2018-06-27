using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace FastXmlSerialization.UnitTest
{
    public class ObjectXmlSerializer_Serialize_Test
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ObjectXmlSerializer_Serialize_Test(ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }



        [Fact]
        public void Complex_Null_Serialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(null);
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input />";
            Assert.Equal(expected, output);
        }


        [Fact]
        public void Nullable_Not_Null_Serialize()
        {
            var entity = new MyEntity
            {
                StringValue = "就诊编号",
                Int32Value = 101,
                Int64Value = 1111119999L,
                DateTimeValue = new DateTime(1956, 9, 10, 4, 45, 55),
                DecimalValue = 56652333.5633M,
                NullableInt32Value = 102,
                NullableInt64Value = 1111129999L,
                NullableDateTimeValue = new DateTime(1956, 9, 20, 4, 45, 55),
                NullableDecimalValue = 256652333.56M
            };
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(entity);
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <stringvalue>就诊编号</stringvalue>
  <int32value>101</int32value>
  <int64value>1111119999</int64value>
  <datetimevalue>1956-09-10 04:45:55</datetimevalue>
  <decimalvalue>56652333.56</decimalvalue>
  <nullableint32value>102</nullableint32value>
  <nullableint64value>1111129999</nullableint64value>
  <nullabledatetimevalue>1956-09-20 04:45:55</nullabledatetimevalue>
  <nullabledecimalvalue>256652333.56</nullabledecimalvalue>
</input>";
            Assert.Equal(expected, output);
        }


        [Fact]
        public void Nullable_Null_Serialize()
        {
            var entity = new MyEntity
            {
                StringValue = "就诊编号",
                Int32Value = 101,
                Int64Value = 1111119999L,
                DateTimeValue = new DateTime(1956, 9, 10, 4, 45, 55),
                DecimalValue = 56652333.5633M,
                NullableInt32Value = null,
                NullableInt64Value = null,
                NullableDateTimeValue = null,
                NullableDecimalValue = null
            };
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(entity);
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <stringvalue>就诊编号</stringvalue>
  <int32value>101</int32value>
  <int64value>1111119999</int64value>
  <datetimevalue>1956-09-10 04:45:55</datetimevalue>
  <decimalvalue>56652333.56</decimalvalue>
  <nullableint32value></nullableint32value>
  <nullableint64value></nullableint64value>
  <nullabledatetimevalue></nullabledatetimevalue>
  <nullabledecimalvalue></nullabledecimalvalue>
</input>";
            Assert.Equal(expected, output);
        }


        [Fact]
        public void String_Serialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize("就诊编号");
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>就诊编号</input>";
            Assert.Equal(expected, output);
        }

        [Fact]
        public void Object_Serialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(new object());
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input></input>";
            Assert.Equal(expected, output);
        }

        [Fact]
        public void DateTime_Serialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(new DateTime(1956, 1, 2, 3, 4, 5));
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>1956-01-02 03:04:05</input>";
            Assert.Equal(expected, output);
        }

        [Fact(Skip = "金额类型不能直接序列化")]
        public void Decimal_Serialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(1563233.7523M);
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>1563233.75</input>";

            Assert.Equal(expected, output);

            output = xmlSerializer.Serialize(1563233.755M);
            expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>1563233.76</input>";
            Assert.Equal(expected, output);
        }

        [Fact]
        public void Collection_Of_String_Serialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(new List<string> {
                "A",
                "B"
            });
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <dataset>
    <row>A</row>
    <row>B</row>
  </dataset>
</input>";
            Assert.Equal(expected, output);
        }


        [Fact]
        public void Collection_Of_Object_Serialize()
        {
            var entity = new MyEntity
            {
                StringValue = "就诊编号",
                Int32Value = 101,
                Int64Value = 1111119999L,
                DateTimeValue = new DateTime(1956, 9, 10, 4, 45, 55),
                DecimalValue = 56652333.5633M,
                NullableInt32Value = 102,
                NullableInt64Value = 1111129999L,
                NullableDateTimeValue = new DateTime(1956, 9, 20, 4, 45, 55),
                NullableDecimalValue = 256652333.56M
            };

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(new List<MyEntity> {
                entity
            });
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <dataset>
    <row>
      <stringvalue>就诊编号</stringvalue>
      <int32value>101</int32value>
      <int64value>1111119999</int64value>
      <datetimevalue>1956-09-10 04:45:55</datetimevalue>
      <decimalvalue>56652333.56</decimalvalue>
      <nullableint32value>102</nullableint32value>
      <nullableint64value>1111129999</nullableint64value>
      <nullabledatetimevalue>1956-09-20 04:45:55</nullabledatetimevalue>
      <nullabledecimalvalue>256652333.56</nullabledecimalvalue>
    </row>
  </dataset>
</input>";
            Assert.Equal(expected, output);
        }




        [Fact]
        public void Collection_Property_Serialize()
        {
            var entity = new MyEntity
            {
                StringValue = "就诊编号",
                Int32Value = 101,
                Int64Value = 1111119999L,
                DateTimeValue = new DateTime(1956, 9, 10, 4, 45, 55),
                DecimalValue = 56652333.5633M,
                NullableInt32Value = 102,
                NullableInt64Value = 1111129999L,
                NullableDateTimeValue = new DateTime(1956, 9, 20, 4, 45, 55),
                NullableDecimalValue = 256652333.56M
            };

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(new CollectionPropertyObject<MyEntity>
            {
                Items = new List<MyEntity> {
                    entity
                }
            });
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <dataset>
    <row>
      <stringvalue>就诊编号</stringvalue>
      <int32value>101</int32value>
      <int64value>1111119999</int64value>
      <datetimevalue>1956-09-10 04:45:55</datetimevalue>
      <decimalvalue>56652333.56</decimalvalue>
      <nullableint32value>102</nullableint32value>
      <nullableint64value>1111129999</nullableint64value>
      <nullabledatetimevalue>1956-09-20 04:45:55</nullabledatetimevalue>
      <nullabledecimalvalue>256652333.56</nullabledecimalvalue>
    </row>
  </dataset>
</input>";
            //this._testOutputHelper.WriteLine(output);
            Assert.Equal(expected, output);
        }




        [Fact]
        public void Complex_Property_Serialize()
        {
            var entity = new MyEntity
            {
                StringValue = "就诊编号",
                Int32Value = 101,
                Int64Value = 1111119999L,
                DateTimeValue = new DateTime(1956, 9, 10, 4, 45, 55),
                DecimalValue = 56652333.5633M,
                NullableInt32Value = 102,
                NullableInt64Value = 1111129999L,
                NullableDateTimeValue = new DateTime(1956, 9, 20, 4, 45, 55),
                NullableDecimalValue = 256652333.56M
            };

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(new ComplexPropertyObject<MyEntity>
            {
                Value = entity
            });
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <value>
    <stringvalue>就诊编号</stringvalue>
    <int32value>101</int32value>
    <int64value>1111119999</int64value>
    <datetimevalue>1956-09-10 04:45:55</datetimevalue>
    <decimalvalue>56652333.56</decimalvalue>
    <nullableint32value>102</nullableint32value>
    <nullableint64value>1111129999</nullableint64value>
    <nullabledatetimevalue>1956-09-20 04:45:55</nullabledatetimevalue>
    <nullabledecimalvalue>256652333.56</nullabledecimalvalue>
  </value>
</input>";

            Assert.Equal(expected, output);
        }

        [Fact]
        public void Amount_Test()
        {
            AmountEntity amountEntity = new AmountEntity
            {
                DetailAmount = 12356.12343M,
                SettlementAmount = 12356.12343M
            };

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(amountEntity);
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <detailamount>12356.1234</detailamount>
  <settlementamount>12356.12</settlementamount>
</input>";

            Assert.Equal(expected, output);
        }

        [Fact]
        public void FastXmlIgnoreAttribute_Test()
        {
            FastXmlIgnoreEntity amountEntity = new FastXmlIgnoreEntity
            {
                StringValue = "大明王朝",
                Int32Value = 12356
            };

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var output = xmlSerializer.Serialize(amountEntity);
            var expected = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>
  <stringvalue>大明王朝</stringvalue>
</input>";

            Assert.Equal(expected, output);
        }
    }
}