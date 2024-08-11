using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;
using Net.Interfaces;

namespace Net.CougarMessage.Parser.Builders
{
    public class CategoryAttributeBuilder : AttributeBuilderBase
    {
        public CategoryAttributeBuilder(ParserObjectBuilder builderParent) : base(builderParent)
        {
            AttrBuild.Name = "category";
            AttrBuild.Type = IAttribute.AttributeType.CATEGORY;
        }

        public override void ExitCategoryAttribute(Net.CougarMessage.Grammar.CougarParser.Category_attributeContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
            base.OnComplete(this);
        }

        public override void ExitCategoryList(Net.CougarMessage.Grammar.CougarParser.Category_listContext ctx)
        {
            if (ListValues.Count > 0)
            {
                AttrBuild.AddValues(ListValues);
                ListValues.Clear();
            }
        }

        public override void ExitCategoryName(Net.CougarMessage.Grammar.CougarParser.Category_nameContext ctx)
        {
            ListValues.Add(ctx.GetText());
        }
    }
}