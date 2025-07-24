using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace JsonMasking
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
        /// <param name="blacklistPartial">Optional. Dictionary where the key is the property to be masked and the value is the function to apply to the property</param>
        /// <returns></returns>
        public static string MaskFields(this string json, string[] blacklist, string mask, Dictionary<string, Func<string, string>> blacklistPartial = null)
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

            JsonNode deserializedObject;
            try
            {
                deserializedObject = JsonNode.Parse(json);
            }
            catch (JsonException ex) when (ex.GetType().Name == "JsonReaderException")
            {
                // Re-throw as JsonException to maintain compatibility with the test expectations
                throw new JsonException(ex.Message, ex);
            }

            if (deserializedObject is JsonArray jsonArray)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    MaskFieldsFromJsonNode(jsonArray[i], blacklist, mask, i.ToString());
                }

                return deserializedObject.ToString();
            }

            var jsonObject = deserializedObject.AsObject();

            if (blacklistPartial != null)
            {
                MaskFieldsFromJsonNode(jsonObject, blacklist, mask, blacklistPartial, "");
            }
            else
            {
                MaskFieldsFromJsonNode(jsonObject, blacklist, mask, "");
            }

            return jsonObject.ToString();
        }

        /// <summary>
        /// Mask fields from JsonNode
        /// </summary>
        /// <param name="node"></param>
        /// <param name="blacklist"></param>
        /// <param name="mask"></param>
        /// <param name="currentPath"></param>
        private static void MaskFieldsFromJsonNode(JsonNode node, string[] blacklist, string mask, string currentPath = "")
        {
            if (node == null)
            {
                return; // abort recursive
            }

            if (node is JsonObject jsonObject)
            {
                var propertiesToMask = new List<string>();

                foreach (var property in jsonObject)
                {
                    var path = string.IsNullOrEmpty(currentPath) ? property.Key : $"{currentPath}.{property.Key}";
                    var matching = blacklist.Any(item => IsMatch(path, item));

                    if (matching)
                    {
                        propertiesToMask.Add(property.Key);
                    }

                    // call recursive 
                    MaskFieldsFromJsonNode(property.Value, blacklist, mask, path);
                }

                // replace 
                foreach (var propertyName in propertiesToMask)
                {
                    jsonObject[propertyName] = JsonValue.Create(mask);
                }
            }
            else if (node is JsonArray jsonArray)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    var path = string.IsNullOrEmpty(currentPath) ? i.ToString() : $"{currentPath}.{i}";
                    MaskFieldsFromJsonNode(jsonArray[i], blacklist, mask, path);
                }
            }
        }

        /// <summary>
        /// Mask fields completely or partially from JsonNode
        /// </summary>
        /// <param name="node"></param>
        /// <param name="blacklist"></param>
        /// <param name="mask"></param>
        /// <param name="blacklistPartial"></param>
        /// <param name="currentPath"></param>
        private static void MaskFieldsFromJsonNode(JsonNode node, string[] blacklist, string mask, Dictionary<string, Func<string, string>> blacklistPartial, string currentPath = "")
        {
            if (node == null)
            {
                return; // abort recursive
            }

            if (node is JsonObject jsonObject)
            {
                var propertiesToMask = new List<(string key, string path)>();

                foreach (var property in jsonObject)
                {
                    var path = string.IsNullOrEmpty(currentPath) ? property.Key : $"{currentPath}.{property.Key}";
                    var matching = blacklist.Any(item => IsMatch(path, item));

                    if (matching)
                    {
                        propertiesToMask.Add((property.Key, path));
                    }

                    // call recursive 
                    MaskFieldsFromJsonNode(property.Value, blacklist, mask, blacklistPartial, path);
                }

                foreach (var (key, path) in propertiesToMask)
                {
                    if (blacklistPartial.TryGetValue(blacklistPartial.GetKey(path), out var maskFunc))
                    {
                        var value = jsonObject[key]?.ToString() ?? "";
                        try
                        {
                            var valueMasked = (maskFunc != null) ? maskFunc(value) : mask;
                            jsonObject[key] = JsonValue.Create((valueMasked != value) ? valueMasked : mask);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                $"An error occurred while executing the function in the dictionary value. {ex.Message}");
                        }
                    }
                    else
                    {
                        jsonObject[key] = JsonValue.Create(mask);
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    var path = string.IsNullOrEmpty(currentPath) ? i.ToString() : $"{currentPath}.{i}";
                    MaskFieldsFromJsonNode(jsonArray[i], blacklist, mask, blacklistPartial, path);
                }
            }
        }

        private static string GetKey(this Dictionary<string, Func<string, string>> blacklistPartial, string key)
        {
            var result = blacklistPartial.Keys.FirstOrDefault(dictionaryKey =>
            {
                return IsMatch(key, dictionaryKey);
            });

            return result ?? key;
        }

        private static bool IsMatch(string key, string value)
        {
            return Regex.IsMatch(key, WildCardToRegular(value), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        private static string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
        }
    }
}
