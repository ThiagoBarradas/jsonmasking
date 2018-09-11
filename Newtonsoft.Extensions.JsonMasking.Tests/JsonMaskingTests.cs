using Newtonsoft.Json;
using Newtonsoft.Extensions.JsonMasking;
using System;
using Xunit;

namespace Newtonsoft.Extensions.JsonMasking.Tests
{
    public class JsonMaskingTests
    {

        [Fact]
        public void MaskFields_Should_Mask_No_Field_With_Empty_Blacklist()
        {
            // arrange
            var obj = new
            {
                Test = "1",
                Password = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"Test\": \"1\",\r\n  \"Password\": \"somepass#here\"\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Mask_No_Field_With_Json_Without_Property()
        {
            // arrange
            var obj = new
            {
                Test = "1",
                OtherField = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { "password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"Test\": \"1\",\r\n  \"OtherField\": \"somepass#here\"\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Mask_Single_Field()
        {
            // arrange
            var obj = new
            {
                Test = "1",
                Password = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { "password" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"Test\": \"1\",\r\n  \"Password\": \"----\"\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Mask_Integer_Field()
        {
            // arrange
            var obj = new
            {
                Test = 1,
                Password = 123456
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { "password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"Test\": 1,\r\n  \"Password\": \"*******\"\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Mask_Depth_Field()
        {
            // arrange
            var obj = new
            {
                DepthObject = new
                {
                    Test = "1",
                    Password = "somepass#here"
                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { "password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"DepthObject\": {\r\n    \"Test\": \"1\",\r\n    \"Password\": \"*******\"\r\n  }\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Mask_Multiple_Fields()
        {
            // arrange
            var obj = new
            {
                Password = "somepass#here",
                DepthObject = new
                {
                    Test = "1",
                    Password = "somepass#here2"
                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { "password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"Password\": \"*******\",\r\n  \"DepthObject\": {\r\n    \"Test\": \"1\",\r\n    \"Password\": \"*******\"\r\n  }\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Mask_Multiple_Fields_With_Multiple_Blacklist()
        {
            // arrange
            var obj = new
            {
                Password = "somepass#here",
                DepthObject = new
                {
                    Test = "1",
                    CreditCardNumber = "5555000011112222"
                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { "password", "creditcardnumber" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"Password\": \"*******\",\r\n  \"DepthObject\": {\r\n    \"Test\": \"1\",\r\n    \"CreditCardNumber\": \"*******\"\r\n  }\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Mask_With_Null_Property()
        {
            // arrange
            var obj = new
            {
                DepthObject = new
                {
                    Test = "1",
                    Password = (string)null,
                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var blacklist = new string[] { "password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\r\n  \"DepthObject\": {\r\n    \"Test\": \"1\",\r\n    \"Password\": \"*******\"\r\n  }\r\n}", result);
        }

        [Fact]
        public void MaskFields_Should_Throw_Exception_When_Blacklist_Is_Null()
        {
            // arrange
            var obj = new
            {
                Test = "1",
                Password = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = null;
            var mask = "*******";

            // act
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
                json.MaskFields(blacklist, mask));

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: blacklist", ex.Message);
        }

        [Fact]
        public void MaskFields_Should_Throw_Exception_When_Json_Is_Null()
        {
            // arrange
            string json = null;
            var blacklist = new string[] { "password" };
            var mask = "*******";

            // act
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
                json.MaskFields(blacklist, mask));

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: json", ex.Message);
        }

        [Fact]
        public void MaskFields_Should_Throw_Exception_When_Json_String_Is_Empty()
        {
            // arrange
            string json = "";
            var blacklist = new string[] { "password" };
            var mask = "*******";

            // act
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
                json.MaskFields(blacklist, mask));

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: json", ex.Message);
        }

        [Fact]
        public void MaskFields_Should_Throw_Exception_When_Json_String_Is_Invalid()
        {
            // arrange
            var json = "invalid json";
            var blacklist = new string[] { "password" };
            string mask = "------";

            // act
            Exception ex = Assert.Throws<JsonReaderException>(() =>
                json.MaskFields(blacklist, mask));

            // assert
            Assert.StartsWith("Unexpected character encountered while parsing value", ex.Message);
        }
    }
}
