using JsonMasking.Tests.Mocks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            string[] blacklist = { };
            var mask = "*******";

            // act
            var result = json.MaskFields(blacklist, mask);

            // assert
            Assert.Equal("{\n  \"Test\": \"1\",\n  \"Password\": \"somepass#here\"\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"Test\": \"1\",\n  \"OtherField\": \"somepass#here\"\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"Test\": \"1\",\n  \"Password\": \"----\"\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"Test\": 1,\n  \"Password\": \"*******\"\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"*******\"\n  }\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"Password\": \"*******\",\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"*******\"\n  }\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"Password\": \"*******\",\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"CreditCardNumber\": \"*******\"\n  }\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"*******\"\n  }\n}", result.Replace("\r\n", "\n"));
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
            Assert.Equal("Value cannot be null. (Parameter 'blacklist')", ex.Message.Replace("\r\n", "\n"));
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
            Assert.Equal("Value cannot be null. (Parameter 'json')", ex.Message.Replace("\r\n", "\n"));
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
            Assert.Equal("Value cannot be null. (Parameter 'json')", ex.Message.Replace("\r\n", "\n"));
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
            Assert.Equal("{\n  \"DepthObject\": {\n    \"Test\": \"1\",\n    \"Password\": \"somepass#here\",\n    \"DepthObject\": {\n      \"Test\": \"1\",\n      \"Password\": \"*******\"\n    }\n  },\n  \"DepthObject2\": {\n    \"Test\": \"1\",\n    \"Password\": \"somepass#here\",\n    \"DepthObject\": {\n      \"Test\": \"1\",\n      \"Password\": \"*******\",\n      \"DepthObject\": {\n        \"Test\": \"1\",\n        \"Password\": \"*******\"\n      }\n    }\n  },\n  \"Password\": \"somepass#here\"\n}", result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Partial_With_Single_Field()
        {
            // arrange
            const string EXPECTED_VALUE = "{\n  \"Test\": \"1\",\n  \"Card\": {\n    \"Number\": \"462294*****9865\",\n    \"Password\": \"somepass#here2\"\n  }\n}";

            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2"
                }
            };

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Partial_And_Completely_With_Single_Field()
        {
            // arrange
            const string EXPECTED_VALUE = "{\n  \"Test\": \"1\",\n  \"Card\": {\n    \"Number\": \"462294*****9865\",\n    \"Password\": \"----\"\n  },\n  \"Password\": \"----\"\n}";

            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2"
                },
                Password = "somepass#here2"
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number", "*password" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Partial_With_Multiple_Fields()
        {
            // arrange
            const string EXPECTED_VALUE = "{\n  \"Test\": \"1\",\n  \"Card\": {\n    \"Number\": \"462294*****9865\",\n    \"Password\": \"somepass#here2\",\n    \"Card\": {\n      \"Number\": \"462294*****9865\"\n    },\n    \"Teste\": {\n      \"Card\": {\n        \"Number\": \"462294*****9865\"\n      }\n    }\n  }\n}";

            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2",
                    Card = new
                    {
                        Number = "4622943127049865",
                    },
                    Teste = new
                    {
                        Card = new
                        {
                            Number = "4622943127049865",
                        }
                    }

                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Partial_And_Completely_With_Multiple_Field()
        {
            // arrange
            const string EXPECTED_VALUE = "{\n  \"Test\": \"1\",\n  \"Card\": {\n    \"Number\": \"462294*****9865\",\n    \"Password\": \"----\",\n    \"Card\": {\n      \"Number\": \"462294*****9865\",\n      \"Password\": \"----\"\n    },\n    \"Teste\": {\n      \"Card\": {\n        \"Number\": \"462294*****9865\",\n        \"Password\": \"----\"\n      }\n    }\n  }\n}";

            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2",
                    Card = new
                    {
                        Number = "4622943127049865",
                        Password = "somepass#here2"
                    },
                    Teste = new
                    {
                        Card = new
                        {
                            Number = "4622943127049865",
                            Password = "somepass#here2"
                        }
                    }
                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number", "*password" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Not_Mask_Partial_If_Property_IsNot_In_Blacklist()
        {
            // arrange
            const string EXPECTED_VALUE = "{\n  \"Test\": \"1\",\n  \"Card\": {\n    \"Number\": \"4622943127049865\",\n    \"Password\": \"somepass#here2\"\n  }\n}";

            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2"
                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Completely_If_Partial_Blacklist_Is_Empty()
        {
            // arrange
            const string EXPECTED_VALUE = "{\n  \"Test\": \"1\",\n  \"Card\": {\n    \"Number\": \"----\",\n    \"Password\": \"somepass#here2\"\n  }\n}";

            var blacklistPartialMock = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase){ };

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2"
                }
            };

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Mask_Completely_If_Delegate_Return_Same_Value()
        {
            // arrange
            const string EXPECTED_VALUE = "{\n  \"Test\": \"1\",\n  \"Card\": {\n    \"Number\": \"----\",\n    \"Password\": \"somepass#here2\"\n  }\n}";

            var blacklistPartialMock = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "*card.number", text => text }
            };

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2"
                }
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);

            // assert
            Assert.Equal(EXPECTED_VALUE, result.Replace("\r\n", "\n"));
        }

        [Fact]
        public static void MaskFields_Should_Throw_Exception_Correctly_When_Has_error_In_Delegate()
        {
            // arrange
            const string EXPECTED_ERROR = "An error occurred while executing the function in the dictionary value. startIndex cannot be larger than length of string. (Parameter 'startIndex')";

            var blacklistPartialMock = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase) 
            {
                { "*card.number", text => text.Substring(100) }
            };

            var obj = new
            {
                Test = "1",
                Card = new
                {
                    Number = "4622943127049865",
                    Password = "somepass#here2"
                }
            };

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number" };
            var mask = "----";

            // act
            Exception ex = Assert.Throws<InvalidOperationException>(() =>
                json.MaskFields(blacklist, mask, blacklistPartialMock));

            // assert
            Assert.Equal(EXPECTED_ERROR, ex.Message);
        }

        [Theory]
        [InlineData("4622943127049865", "462294*****9865")]
        [InlineData(" 462   29431270   49865 ", "462294*****9865")]
        [InlineData("46@22912704986@5", "----")]
        [InlineData("462.29431.2704.9865", "----")]
        [InlineData("46|2294312|70498|65", "----")]
        [InlineData("123", "----")]
        [InlineData("0123456789123456789012345", "012345*****2345")]
        public static void MaskFields_Should_Mask_Completely_and_Partially_Correctly(string received, string expected)
        {
            // arrange
            var blacklistPartialMock = BlacklistPartialMock.DefaultBlackListPartial;

            var obj = new
            {
                Card = new
                {
                    Number = received,
                }
            };

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            string[] blacklist = { "*card.number" };
            var mask = "----";

            // act
            var result = json.MaskFields(blacklist, mask, blacklistPartialMock);
            var jsonObject = JObject.Parse(result);
            var numberMasked = jsonObject["Card"]["Number"].ToString();

            // assert
            Assert.Equal(numberMasked, expected);
        }
    }
}
