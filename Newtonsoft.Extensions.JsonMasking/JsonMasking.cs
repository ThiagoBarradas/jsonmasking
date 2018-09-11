using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Extensions.JsonMasking
{
    /// <summary>
    /// Json Masking Extension
    /// </summary>
    public static class JsonMasking
    {
        /// <summary>
        /// Mask fields
        /// </summary>
        /// <param name="json">json to mask properties</param>
        /// <param name="blacklist">insensitive property array</param>
        /// <param name="mask">mask to replace property value</param>
        /// <returns></returns>
        public static string MaskFields(this string json, string[] blacklist, string mask)
        {
            if (string.IsNullOrWhiteSpace(json) == true)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (blacklist == null)
            {
                throw new ArgumentNullException(nameof(blacklist));
            }

            if (blacklist.Any() == false)
            {
                return json;
            }

            var jsonObject = (JObject) JsonConvert.DeserializeObject(json);
            MaskFieldsFromJToken(jsonObject, blacklist, mask);

            return jsonObject.ToString();
        }

        /// <summary>
        /// Mask fields from JToken
        /// </summary>
        /// <param name="token"></param>
        /// <param name="blacklist"></param>
        /// <param name="mask"></param>
        private static void MaskFieldsFromJToken(JToken token, string[] blacklist, string mask)
        {
            JContainer container = token as JContainer;
            if (container == null)
            {
                return;
            }

            List<JToken> removeList = new List<JToken>();
            foreach (JToken el in container.Children())
            {
                var prop = el as JProperty;

                if (prop != null && blacklist.Any(f => f.Equals(prop.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    removeList.Add(el);
                }
                MaskFieldsFromJToken(el, blacklist, mask);
            }

            foreach (JToken el in removeList)
            {
                var prop = (JProperty)el;
                prop.Value = mask;
            }
        }
    }
}
