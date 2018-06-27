using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace FastXmlSerialization.UnitTest
{
    public class ObjectXmlSerializer_Deserialize_Test
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ObjectXmlSerializer_Deserialize_Test(ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }


        [Fact]
        public void Int_Empty_Element_Deserialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>0</output>";

            var result = xmlSerializer.Deserialize<int>(input);

            Assert.Equal(0, result);

            input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
    <value></value>
</output>";

            var objectResult = xmlSerializer.Deserialize<ComplexPropertyObject<int>>(input);

            Assert.NotNull(objectResult);
            Assert.Equal(0, objectResult.Value);

            input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
    <value />
</output>";

            objectResult = xmlSerializer.Deserialize<ComplexPropertyObject<int>>(input);

            Assert.NotNull(objectResult);
            Assert.Equal(0, objectResult.Value);
        }

        [Fact]
        public void DateTime_Deserialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>1956-01-02 03:04:05</output>";

            var result = xmlSerializer.Deserialize<DateTime>(input);

            var expected = new DateTime(1956, 1, 2, 3, 4, 5);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Complex_Deserialize()
        {
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
  <stringvalue>就诊编号</stringvalue>
  <int32value>101</int32value>
  <int64value>1111119999</int64value>
  <other>1111119999</other>
  <datetimevalue>1956-09-10 04:45:55</datetimevalue>
  <decimalvalue>56652333.56</decimalvalue>
  <nullableint32value>102</nullableint32value>
  <nullableint64value>1111129999</nullableint64value>
  <nullabledatetimevalue>1956-09-20 04:45:55</nullabledatetimevalue>
  <nullabledecimalvalue>256652333.56</nullabledecimalvalue>
</output>";

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();

            var result = xmlSerializer.Deserialize<MyEntity>(input);

            Assert.NotNull(result);
            Assert.Equal("就诊编号", result.StringValue);
            Assert.Equal(101, result.Int32Value);
            Assert.Equal(1111119999L, result.Int64Value);
            Assert.Equal(new DateTime(1956, 9, 10, 4, 45, 55), result.DateTimeValue);
            Assert.Equal(56652333.56M, result.DecimalValue);
            Assert.Equal("就诊编号", result.StringValue);
            Assert.Equal(102, result.NullableInt32Value);
            Assert.Equal(1111129999L, result.NullableInt64Value);
            Assert.Equal(new DateTime(1956, 9, 20, 4, 45, 55), result.NullableDateTimeValue);
            Assert.Equal(256652333.56M, result.NullableDecimalValue);
        }

        [Fact]
        public void Empty_Element_Deserialize()
        {
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
  <stringvalue />
</output>";

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();

            var result = xmlSerializer.Deserialize<MyEntity>(input);

            Assert.NotNull(result);
            Assert.Null(result.StringValue);

        }

        [Fact]
        public void Complex_Null_Deserialize()
        {
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
</output>";

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();

            var result = xmlSerializer.Deserialize<MyEntity>(input);

            Assert.Null(result);
        }

        [Fact]
        public void String_Deserialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>就诊编号</output>";

            var result = xmlSerializer.Deserialize<string>(input);

            Assert.Equal("就诊编号", result);
        }

        [Fact]
        public void Object_Deserialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output></output>";
            ;

            var result = xmlSerializer.Deserialize<object>(input);

            Assert.Null(result);

            input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output />";
            ;

            result = xmlSerializer.Deserialize<object>(input);

            Assert.Null(result);
        }

        [Fact(Skip = "金额类型不能直接序列化")]
        public void Decimal_Deserialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>1563233.75</output>";

            var result = xmlSerializer.Deserialize<decimal>(input);

            Assert.Equal(1563233.75M, result);

            input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>1563233.76</output>";

            var result2 = xmlSerializer.Deserialize<decimal?>(input);

            Assert.Equal(1563233.76M, result2);
        }


        [Fact]
        public void Collection_Of_String_Deserialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
  <sqldata >
    <row>A</row>
    <row>B</row>
  </sqldata >
</output>";
            var result = xmlSerializer.Deserialize<List<string>>(input);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("A", result[0]);
            Assert.Equal("B", result[1]);

            var arrayResult = xmlSerializer.Deserialize<string[]>(input);
            Assert.NotNull(result);
            Assert.Equal(2, arrayResult.Length);
            Assert.Equal("A", result[0]);
            Assert.Equal("B", result[1]);
        }


        [Fact]
        public void Collection_Of_Object_Deserialize()
        {
            for (int i = 0; i < 10000; i++) {
                ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
                var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
  <sqldata>
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
    <row>
      <stringvalue>就诊编号</stringvalue>
      <int32value>103</int32value>
      <int64value>1111119999</int64value>
      <datetimevalue>1956-09-10 04:45:55</datetimevalue>
      <decimalvalue>56652333.56</decimalvalue>
      <nullableint32value>102</nullableint32value>
      <nullableint64value>1111129999</nullableint64value>
      <nullabledatetimevalue>1956-09-20 04:45:55</nullabledatetimevalue>
      <nullabledecimalvalue>256652333.56</nullabledecimalvalue>
    </row>
  </sqldata>
</output>";
                var result = xmlSerializer.Deserialize<List<MyEntity>>(input);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("就诊编号", result[0].StringValue);
                Assert.Equal(103, result[1].Int32Value);
            }


        }


        [Fact]
        public void Collection_Property_Deserialize()
        {


            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
  <sqldata>
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
    <row>
      <stringvalue>就诊编号</stringvalue>
      <int32value></int32value>
      <int64value>1111119999</int64value>
      <datetimevalue>1956-09-10 04:45:55</datetimevalue>
      <decimalvalue>56652333.56</decimalvalue>
      <nullableint32value>102</nullableint32value>
      <nullableint64value>1111129999</nullableint64value>
      <nullabledatetimevalue>1956-09-20 04:45:55</nullabledatetimevalue>
      <nullabledecimalvalue>256652333.56</nullabledecimalvalue>
    </row>
  </sqldata>
</output>";
            var result = xmlSerializer.Deserialize<CollectionPropertyObject<MyEntity>>(input);

            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("就诊编号", result.Items[0].StringValue);
            Assert.Equal(0, result.Items[1].Int32Value);
        }




        [Fact]
        public void Complex_Property_Deserialize()
        {

            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
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
</output>";

            var result = xmlSerializer.Deserialize<ComplexPropertyObject<MyEntity>>(input);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            Assert.Equal("就诊编号", result.Value.StringValue);
        }



        [Fact]
        public void Error_Format_Deserialize()
        {
            ObjectXmlSerializer xmlSerializer = new ObjectXmlSerializer();
            var input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<input>就诊编号</input>";

            Assert.Throws<XmlSerializeException>(() => xmlSerializer.Deserialize<string>(input));


            input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
  <dataset>
    <row>
      <stringvalue>就诊编号</stringvalue>
    </row>
  </dataset>
</output>";
            Assert.Throws<XmlSerializeException>(() => xmlSerializer.Deserialize<List<MyEntity>>(input));

            input = @"<?xml version=""1.0"" encoding=""GBK"" standalone=""yes""?>
<output>
  <sqldata>
    <row>
      <stringvalue>就诊编号</stringvalue>
    </row>
    <row1>
      <stringvalue>就诊编号</stringvalue>
    </row1>
  </sqldata>
</output>";

            Assert.Throws<XmlSerializeException>(() => xmlSerializer.Deserialize<List<MyEntity>>(input));

        }
    }
}
