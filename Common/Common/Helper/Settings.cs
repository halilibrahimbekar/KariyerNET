using System.Collections.Generic;

namespace Common.Common.Helper
{
    public static class Settings
    {
        public static Dictionary<string, string> Items { get; set; }

        public static string GetValue(string key)
        {
            if (Items.ContainsKey(key))
            {
                return Items[key];
            }

            return string.Empty;
        }
    }
}
