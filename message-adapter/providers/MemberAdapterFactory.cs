using System.Collections.Generic;
using System.Text.Json;
using adapter_interface;
using adapter_interface.providers;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public class MemberAdapterFactory
    {
        public MemberAdapterFactory(ITypeAdapterProvider typeAdapterProvider)
        {
            _typeProvider = typeAdapterProvider;
        }
        public MemberAdapter CreateMemberAdapter(IMember memberAdapt, List<MessageAdapter> listMessages)
        {
            ITypeAdapter typeAdapter = _typeProvider.GetTypeAdapter(memberAdapt.Type);
            
            if (memberAdapt is IVariableArrayMember variableArrayMember)
            {
                return new VariableArrayMemberAdapter(variableArrayMember, typeAdapter);
            }
            return new MemberAdapter(memberAdapt, typeAdapter);
        }

        private ITypeAdapterProvider _typeProvider;
    }
}