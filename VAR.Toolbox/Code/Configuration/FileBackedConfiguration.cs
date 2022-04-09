using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VAR.Toolbox.Code.Configuration
{
    public class FileBackedConfiguration : IConfiguration
    {
        private readonly MemoryBackedConfiguration _config = new MemoryBackedConfiguration();

        private readonly string _name;

        public FileBackedConfiguration(string name = null)
        {
            _name = name;
        }

        private static string GetConfigFileName(string name = null)
        {
            string location = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            string path = Path.GetDirectoryName(location);
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(location);
            string configFile = string.IsNullOrEmpty(name)
                ? $"{path}/{filenameWithoutExtension}.cfg"
                : $"{path}/{filenameWithoutExtension}_{name}.cfg";
            return configFile;
        }

        private static string[] GetConfigurationLines(string name = null)
        {
            string configFile = GetConfigFileName(name);
            string[] config = File.Exists(configFile) == false
                ? Array.Empty<string>()
                : File.ReadAllLines(configFile);

            return config;
        }

        public void Load(IConfiguration other = null)
        {
            _config.Clear();
            if (other != null)
            {
                foreach (string key in other.GetKeys())
                {
                    _config.Set(key, other.Get(key, null));
                }
            }

            string[] configLines = GetConfigurationLines(_name);
            foreach (string configLine in configLines)
            {
                int idxSplit = configLine.IndexOf('|');
                if (idxSplit < 0) { continue; }

                string configName = configLine.Substring(0, idxSplit);
                string configData = configLine.Substring(idxSplit + 1);

                _config.Set(configName, configData);
            }
        }

        public void Save()
        {
            StringBuilder sbConfig = new StringBuilder();
            foreach (string key in _config.GetKeys())
            {
                sbConfig.AppendFormat("{0}|{1}\n", key, _config.Get(key, string.Empty));
            }

            string configFileName = GetConfigFileName(_name);
            File.WriteAllText(configFileName, sbConfig.ToString());
        }

        public IEnumerable<string> GetKeys()
        {
            return _config.GetKeys();
        }

        public void Clear()
        {
            _config.Clear();
        }

        public string Get(string key, string defaultValue)
        {
            return _config.Get(key, defaultValue);
        }

        public int Get(string key, int defaultValue)
        {
            return _config.Get(key, defaultValue);
        }

        public bool Get(string key, bool defaultValue)
        {
            return _config.Get(key, defaultValue);
        }

        public void Set(string key, string value)
        {
            _config.Set(key, value);
        }

        public void Set(string key, int value)
        {
            _config.Set(key, value);
        }

        public void Set(string key, bool value)
        {
            _config.Set(key, value);
        }
    }
}