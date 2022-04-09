using System.Collections.Generic;

namespace VAR.ScreenAutomation.Interfaces
{
    public interface IConfiguration
    {
        IEnumerable<string> GetKeys();
        void Clear();
        bool Get(string key, bool defaultValue);
        int Get(string key, int defaultValue);
        string Get(string key, string defaultValue);
        void Set(string key, bool value);
        void Set(string key, int value);
        void Set(string key, string value);
    }
}