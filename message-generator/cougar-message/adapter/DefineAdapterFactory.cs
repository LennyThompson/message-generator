using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Adapter
{
    public static class DefineAdapterFactory
    {
        public static DefineAdapter CreateDefineAdapter(IDefine defineFrom)
        {
            return new DefineAdapter(defineFrom);
        }

        public static List<DefineAdapter> CreateDefineAdapters(IMessageSchema schemaFrom)
        {
            return schemaFrom.Defines().Select(CreateDefineAdapter).ToList();
        }
    }
}