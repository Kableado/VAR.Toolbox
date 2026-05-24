using System;
using System.Collections.Generic;
using System.Linq;

namespace VAR.Toolbox.Code.Configuration
{
    public class MemoryBackedConfiguration : IConfiguration
    {
        private readonly Dictionary<string, string> _configItems = new();

        public IEnumerable<string> GetKeys()
        {
            return _configItems.Select(p => p.Key);
        }

        public void Clear()
        {
            _configItems.Clear();
        }

        public string Get(string key, string defaultValue)
        {
            return _configItems.TryGetValue(key, out string? item) ? item : defaultValue;
        }

        public int Get(string key, int defaultValue)
        {
            if (_configItems.TryGetValue(key, out string? item))
            {
                return int.TryParse(item, out int value) ? value : defaultValue;
            }

            return defaultValue;
        }

        public bool Get(string key, bool defaultValue)
        {
            if (_configItems.TryGetValue(key, out string? value))
            {
                return value == "true";
            }

            return defaultValue;
        }

        public void Set(string key, string value)
        {
            _configItems[key] = value;
        }

        public void Set(string key, int value)
        {
            _configItems[key] = Convert.ToString(value);
        }

        public void Set(string key, bool value)
        {
            _configItems[key] = value ? "true" : "false";
        }
    }
}