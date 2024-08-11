using System.Collections.Generic;
using System.Text.Json;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Adapter
{
    public static class MemberAdapterFactory
    {
        public static MemberAdapter CreateMemberAdapter(IMember memberAdapt, List<MessageAdapter> listMessages)
        {
            if (memberAdapt is IVariableArrayMember variableArrayMember)
            {
                return new VariableArrayMemberAdapter(variableArrayMember, listMessages);
            }
            return new MemberAdapter(memberAdapt, listMessages);
        }
    }
}