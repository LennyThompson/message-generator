using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CougarMessage.Adapter
{
    public class EnumAdapter
    {
        private class EnumValueAdapter
        {
            private IEnumValue _enumValue;

            public EnumValueAdapter(IEnumValue enumValue)
            {
                _enumValue = enumValue;
            }

            public string GetName()
            {
                return _enumValue.Name;
            }

            public int GetValue()
            {
                return _enumValue.Value();
            }
        }

        private IEnum _enumAdapt;
        private List<EnumValueAdapter> _listEnumValueAdapters;

        public EnumAdapter(IEnum enumAdapt)
        {
            _enumAdapt = enumAdapt;
            _listEnumValueAdapters = _enumAdapt.Values().Select(value => new EnumValueAdapter(value)).ToList();
        }

        public string GetName()
        {
            return _enumAdapt.Name;
        }

        public string GetShortName()
        {
            if (_enumAdapt.Name[0] == 'E')
            {
                return _enumAdapt.Name.Substring(1);
            }
            return _enumAdapt.Name;
        }

        public List<EnumValueAdapter> GetValues()
        {
            return _listEnumValueAdapters;
        }

        public string GetCSharpEnumFileName()
        {
            return GetName() + MessageSchemaAdapter.CSHARP_EXT;
        }

        public int GetMaximumEnumLength()
        {
            return GetValues().Max(adapter => adapter.GetName().Length);
        }
    }
}