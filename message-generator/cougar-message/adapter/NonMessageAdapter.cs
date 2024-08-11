using System.Text.Json;
using CougarMessage.Adapter;
using Net.CougarMessage.Parser.MessageTypes.Interfaces;

namespace Net.CougarMessage.Adapter
{
    public class NonMessageAdapter : MessageAdapter
    {
        public NonMessageAdapter(IMessage messageAdapt) : base(messageAdapt)
        {
        }

        public override bool IsNonMessage => true;
    }
}