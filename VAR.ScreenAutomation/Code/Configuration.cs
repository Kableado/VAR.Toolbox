using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VAR.ScreenAutomation.Code
{
    public class Configuration
    {
        private Dictionary<string, string> _configItems = new Dictionary<string, string>();

        private string _name = null;

        public Configuration(string name = null)
        {
            _name = name;
        }

        private static string GetConfigFileName(string name = null)
        {
            string location = System.Reflection.Assembly.GetEntryAssembly().Location;
            string path = Path.GetDirectoryName(location);
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(location);
            string configFile;
            if (string.IsNullOrEmpty(name))
            {
                configFile = string.Format("{0}/{1}.cfg", path, filenameWithoutExtension);
            }
            else
            {
                configFile = string.Format("{0}/{1}_{2}.cfg", path, filenameWithoutExtension, name);
            }
            return configFile;
        }

        private static string[] GetConfigurationLines(string name = null)
        {
            string configFile = GetConfigFileName(name);
            string[] config;
            if (File.Exists(configFile) == false)
            {
                config = new string[0];
            }
            else
            {
                config = File.ReadAllLines(configFile);
            }
            return config;
        }

        public void Load()
        {
            _configItems.Clear();
            string[] configLines = GetConfigurationLines(_name);
            foreach (string configLine in configLines)
            {
                int idxSplit = configLine.IndexOf('|');
                if (idxSplit < 0) { continue; }
                string configName = configLine.Substring(0, idxSplit);
                string configData = configLine.Substring(idxSplit + 1);

                if (_configItems.ContainsKey(configName))
                {
                    _configItems[configName] = configData;
                }
                else
                {
                    _configItems.Add(configName, configData);
                }
            }
        }

        public void Save()
        {
            StringBuilder sbConfig = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in _configItems)
            {
                sbConfig.AppendFormat("{0}|{1}\n", pair.Key, pair.Value);
            }
            string configFileName = GetConfigFileName(_name);
            File.WriteAllText(configFileName, sbConfig.ToString());
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
