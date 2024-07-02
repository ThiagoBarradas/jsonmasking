using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JsonMasking.Tests.Mocks
{
    public static class BlacklistPartialMock
    {
        private const string CARD_NUMBER_PATTERN = @"(\d{4,5})[ -|]?(\d{3,6})[ -|]?(\d{3,5})[ -|]?(\d{3,4})";

        private const string EMPTY_SPACE_PATTERN = @"\s+";

        public static Dictionary<string, Func<string, string>> DefaultBlackListPartial = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            {"*card.number", MaskCardNumber }
        };

        public static string MaskCardNumber(this string text)
        {
            const int MINIMAL_LENGTH = 10;

            text = Regex.Replace(text, EMPTY_SPACE_PATTERN, "");

            if (string.IsNullOrEmpty(text) || text.Length < MINIMAL_LENGTH || !ContainsCardNumber(text))
            {
                return text;
            }

            var firstSix = text[..6];
            var lastFour = text[^4..];
            if (firstSix.Length == 6 && lastFour.Length == 4)
            {
                return $"{firstSix}*****{lastFour}";
            }

            return text;
        }

        public static bool ContainsCardNumber(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            return Regex.IsMatch(text, CARD_NUMBER_PATTERN);
        }
    }
}
