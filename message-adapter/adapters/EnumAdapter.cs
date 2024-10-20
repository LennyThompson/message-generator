using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public class EnumValueAdapter
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
            return _enumValue.Value;
        }
    }
    
    public class EnumAdapter
    {

        private IEnum _enumAdapt;
        private List<EnumValueAdapter> _listEnumValueAdapters;

        public EnumAdapter(IEnum enumAdapt)
        {
            _enumAdapt = enumAdapt;
            _listEnumValueAdapters = _enumAdapt.Values.Select(value => new EnumValueAdapter(value)).ToList();
        }

        public string Name => _enumAdapt.Name;

        public string ShortName
        {
            get
            {
                if (_enumAdapt.Name[0] == 'E')
                {
                    return _enumAdapt.Name.Substring(1);
                }

                return _enumAdapt.Name;
            }
        }

        public List<EnumValueAdapter> Values => _listEnumValueAdapters;

        public string CSharpEnumFileName => Name + ".cs";

        public int MaximumEnumLength => Values.Max(adapter => adapter.GetName().Length);
    }
}