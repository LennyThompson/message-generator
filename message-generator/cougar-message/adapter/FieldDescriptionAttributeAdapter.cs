using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Adapter
{
    public class FieldDescriptionAttributeAdapter : AttributeAdapter
    {
        public FieldDescriptionAttributeAdapter(IAttribute attrAdapt) : base(attrAdapt)
        {
        }

        public string GetFieldName()
        {
            return ((IFielddescAttribute)AttrAdapt).FieldName();
        }

        public string GetShortDescription()
        {
            return ((IFielddescAttribute)AttrAdapt).ShortDescription();
        }

        public string GetLongDescription()
        {
            return ((IFielddescAttribute)AttrAdapt).LongDescription();
        }
    }
}