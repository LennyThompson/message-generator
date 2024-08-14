using System.Collections.Generic;
using System.Text.Json;
using CougarMessage.Parser.MessageTypes;
using CougarMessage.Parser.MessageTypes.Interfaces;
using Interfaces;

namespace CougarMessage.Parser.Builders
{
    public class CategoryAttributeBuilder : AttributeBuilderBase
    {
        public CategoryAttributeBuilder(ParserObjectBuilder? builderParent) : base(builderParent)
        {
            AttrBuild.Name = "category";
            AttrBuild.Type = IAttribute.AttributeType.CATEGORY;
        }

        public override void ExitCategoryAttribute(CougarParser.Category_attributeContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void ExitCategoryList(CougarParser.Category_listContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitCategoryName(CougarParser.Category_nameContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }
    }
}