using System;
using System.Collections.Generic;
using System.Linq;
using VAR.ScreenAutomation.Bots;
using VAR.ScreenAutomation.Interfaces;

namespace VAR.ScreenAutomation.Code
{
    public class AutomationBotFactory
    {
        private static Dictionary<string, Type> _dictAutomationBots = null;

        private static Dictionary<string, Type> GetDict()
        {
            if (_dictAutomationBots != null)
            {
                return _dictAutomationBots;
            }

            Type iAutomationBot = typeof(IAutomationBot);
            IEnumerable<Type> automationBotTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    x.IsAbstract == false &&
                    x.IsInterface == false &&
                    iAutomationBot.IsAssignableFrom(x) &&
                    true);
            _dictAutomationBots = automationBotTypes.ToDictionary(t =>
            {
                IAutomationBot automationBot = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t) as IAutomationBot;
                return automationBot.Name;
            });

            return _dictAutomationBots;
        }

        public static object[] GetAllAutomationBots()
        {
            Dictionary<string, Type> dict = GetDict();
            string[] allAutomationBots = dict.Select(p => p.Key).ToArray();
            return allAutomationBots;
        }

        public static IAutomationBot CreateFromName(string name)
        {
            Dictionary<string, Type> dict = GetDict();
            if (string.IsNullOrEmpty(name))
            {
                return new DummyBot();
            }
            if (dict.ContainsKey(name) == false)
            {
                throw new NotImplementedException(string.Format("Can't create IAutomationBot with this name: {0}", name));
            }
            Type proxyCmdExecutorType = dict[name];

            IAutomationBot automationBot = Activator.CreateInstance(proxyCmdExecutorType) as IAutomationBot;

            return automationBot;
        }
    }
}
