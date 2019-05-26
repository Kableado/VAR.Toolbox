using System;
using System.Collections.Generic;
using System.Linq;

namespace VAR.Toolbox.Code.TextCoders
{
    public class TextCoderFactory
    {
        private static Dictionary<string, Type> _dictTextCoderTypes = null;

        private static Dictionary<string, Type> GetDict()
        {
            if (_dictTextCoderTypes != null)
            {
                return _dictTextCoderTypes;
            }

            Type iTextCoder = typeof(ITextCoder);
            IEnumerable<Type> toolFormTypes = ReflectionUtils.GetTypesOfInterface(iTextCoder);
            _dictTextCoderTypes = toolFormTypes.ToDictionary(t =>
            {
                ITextCoder textCoder = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t) as ITextCoder;
                return textCoder.Name;
            });

            return _dictTextCoderTypes;
        }

        public static string[] GetSupportedCoders()
        {
            Dictionary<string, Type> dict = GetDict();
            return dict.Select(p => p.Key).ToArray();
        }

        public static ITextCoder CreateFromName(string name)
        {
            Dictionary<string, Type> dict = GetDict();
            if (dict.ContainsKey(name) == false)
            {
                throw new NotImplementedException(string.Format("Cant create ITextCoder with this name: {0}", name));
            }
            Type textCoder = dict[name];
            return Activator.CreateInstance(textCoder) as ITextCoder;
        }
    }
}
