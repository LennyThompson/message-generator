using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
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