using System.Text.Json;
using CougarMessage.Adapter;
using CougarMessage.Parser.MessageTypes.Interfaces;

namespace CougarMessage.Adapter
{
    public class NonMessageAdapter : MessageAdapter
    {
        public NonMessageAdapter(IMessage messageAdapt) : base(messageAdapt)
        {
        }

        public bool IsNonMessage => true;
    }
}