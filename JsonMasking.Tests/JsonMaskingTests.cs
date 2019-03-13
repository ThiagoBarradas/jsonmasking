using Newtonsoft.Json;
using JsonMasking;
using System;
using Xunit;

namespace JsonMasking.Tests
{
    public static class JsonMaskingTests
    {

        [Fact]
        public static void MaskFields_Should_Mask_No_Field_With_Empty_Blacklist()
        {
            // arrange
            var obj = new
            {
                Test = "1",
                Password = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = {};
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"Test\": \"1\",\n  \"Password\": \"somepass#here\"\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_No_Field_With_Json_Without_Property()
        {
            // arrange
            var obj = new
            {
                Test = "1",
                OtherField = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"Test\": \"1\",\n  \"OtherField\": \"somepass#here\"\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Single_Field()
        {
            // arrange
            var obj = new
            {
                Test = "1",
                Password = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "password" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"Test\": \"1\",\n  \"Password\": \"----\"\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Integer_Field()
        {
            // arrange
            var obj = new
            {
                Test = 1,
                Password = 123456
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"Test\": 1,\n  \"Password\": \"*******\"\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Depth_Field()
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
            string[] blacklist = { "*.password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"*******\"\n  }\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Multiple_Fields()
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
            string[] blacklist = { "*password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"Password\": \"*******\",\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"*******\"\n  }\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Multiple_Fields_With_Multiple_Blacklist()
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
            string[] blacklist = { "password", "*creditcardnumber" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"Password\": \"*******\",\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"CreditCardNumber\": \"*******\"\n  }\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_With_Null_Property()
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
            string[] blacklist = { "*password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"*******\"\n  }\n}", result.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Throw_Exception_When_Blacklist_Is_Null()
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
            Assert.Equal("Value cannot be null.\nParameter name: blacklist", ex.Message.Replace("\r\n","\n"));
        }

        [Fact]
        public static void MaskFields_Should_Throw_Exception_When_Json_Is_Null()
        {
            // arrange
            string json = null;
            string[] blacklist = { "password" };
            var mask = "*******";

            // act
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
                json.MaskFields(blacklist, mask));

            // assert
            Assert.Equal("Value cannot be null.\nParameter name: json", ex.Message.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Throw_Exception_When_Json_String_Is_Empty()
        {
            // arrange
            string json = "";
            string[] blacklist = { "password" };
            var mask = "*******";

            // act
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
                json.MaskFields(blacklist, mask));

            // assert
            Assert.Equal("Value cannot be null.\nParameter name: json", ex.Message.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Throw_Exception_When_Json_String_Is_Invalid()
        {
            // arrange
            var json = "invalid json";
            string[] blacklist = { "password" };
            string mask = "------";

            // act
            Exception ex = Assert.Throws<JsonReaderException>(() =>
                json.MaskFields(blacklist, mask));

            // assert
            Assert.StartsWith("Unexpected character encountered while parsing value", ex.Message);
        }

        [Fact]
        public static void MaskFields_Should_Mask_With_Wildcard()
        {
            // arrange
            var obj = new
            {
                DepthObject = new
                {
                    Test = "1",
                    Password = "somepass#here",
                    DepthObject = new
                    {
                        Test = "1",
                        Password = "somepass#here"
                    }
                },
                DepthObject2 = new
                {
                    Test = "1",
                    Password = "somepass#here",
                    DepthObject = new
                    {
                        Test = "1",
                        Password = "somepass#here",
                        DepthObject = new
                        {
                            Test = "1",
                            Password = new
                            {
                                Test1 = "1",
                                Password2 = "somepass#here"
                            }
                        }
                    }
                },
                Password = "somepass#here"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*.DepthObject*.Password" };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"somepass#here\",\n    \"DepthObject\": {\n      \"Test\": \"1\",\n      \"Password\": \"*******\"\n    }\n  },\n  \"DepthObject2\": {\n    \"Test\": \"1\",\n    \"Password\": \"somepass#here\",\n    \"DepthObject\": {\n      \"Test\": \"1\",\n      \"Password\": \"*******\",\n      \"DepthObject\": {\n        \"Test\": \"1\",\n        \"Password\": \"*******\"\n      }\n    }\n  },\n  \"Password\": \"somepass#here\"\n}", result.Replace("\r\n","\n"));
        }
    }
}
