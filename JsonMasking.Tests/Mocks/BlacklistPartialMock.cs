using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JsonMasking.Tests.Mocks
{
    public static class BlacklistPartialMock
    {
        private const string CARD_NUMBER_PATTER = @"(\d{4,5})[ -|]?(\d{3,6})[ -|]?(\d{3,5})[ -|]?(\d{3,4})";

        public static Dictionary<string, Func<string, string>> DefaultBlackListPartial = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            {"*card.number", MaskCardNumber }
        };

        public static string MaskCardNumber(this string text)
        {
            if (string.IsNullOrEmpty(text) || !ContainsCardNumber(text))
                return text;

            return Regex.Replace(
                text,
                CARD_NUMBER_PATTER,
                match => $"{match.Value.Substring(0, 6)}*****{match.Value.Substring(match.Value.Length - 4, 4)}");
        }

        public static bool ContainsCardNumber(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            return Regex.IsMatch(text, CARD_NUMBER_PATTER);
        }
    }
}
