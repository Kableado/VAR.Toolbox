using System;
using System.Collections.Generic;
using System.Linq;
using VAR.ScreenAutomation.Interfaces;

namespace VAR.ScreenAutomation.Code
{
    public class MemoryBackedConfiguration : IConfiguration
    {
        private Dictionary<string, string> _configItems = new Dictionary<string, string>();

        public IEnumerable<string> GetKeys()
        {
            if (_configItems == null) { return new List<string>(); }
            return _configItems.Select(p => p.Key);
        }

        public void Clear()
        {
            _configItems.Clear();
        }

        public string Get(string key, string defaultValue)
        {
            if (_configItems == null) { return defaultValue; }
            if (_configItems.ContainsKey(key))
            {
                return _configItems[key];
            }
            return defaultValue;
        }

        public int Get(string key, int defaultValue)
        {
            if (_configItems == null) { return defaultValue; }
            if (_configItems.ContainsKey(key))
            {
                if (int.TryParse(_configItems[key], out int value))
                {
                    return value;
                }
                return defaultValue;
            }
            return defaultValue;
        }

        public bool Get(string key, bool defaultValue)
        {
            if (_configItems == null) { return defaultValue; }
            if (_configItems.ContainsKey(key))
            {
                string value = _configItems[key];
                return (value == "true");
            }
            return defaultValue;
        }

        public void Set(string key, string value)
        {
            if (_configItems == null) { return; }
            if (_configItems.ContainsKey(key))
            {
                _configItems[key] = value;
            }
            else
            {
                _configItems.Add(key, value);
            }
        }

        public void Set(string key, int value)
        {
            if (_configItems == null) { return; }
            if (_configItems.ContainsKey(key))
            {
                _configItems[key] = Convert.ToString(value);
            }
            else
            {
                _configItems.Add(key, Convert.ToString(value));
            }
        }

        public void Set(string key, bool value)
        {
            if (_configItems == null) { return; }
            if (_configItems.ContainsKey(key))
            {
                _configItems[key] = value ? "true" : "false";
            }
            else
            {
                _configItems.Add(key, value ? "true" : "false");
            }
        }

    }
}
