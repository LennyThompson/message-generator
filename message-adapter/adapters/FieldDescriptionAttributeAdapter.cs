using System.Text.Json;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public class FieldDescriptionAttributeAdapter : AttributeAdapter
    {
        public FieldDescriptionAttributeAdapter(IAttribute attrAdapt) : base(attrAdapt)
        {
        }

        public string FieldName =>  ((IFielddescAttribute)AttrAdapt).FieldName;
        public string ShortDescription => ((IFielddescAttribute)AttrAdapt).ShortDescription;
        public string LongDescription => ((IFielddescAttribute)AttrAdapt).LongDescription;
    }
}