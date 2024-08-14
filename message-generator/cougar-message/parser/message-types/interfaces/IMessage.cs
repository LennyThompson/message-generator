using System;
using System.Collections.Generic;
using System.Linq;

namespace CougarMessage.Parser.MessageTypes.Interfaces
{
    public interface IMessage
    {
        string Name { get; }
        int Ordinal { get; }
        List<IAttribute>? Attributes { get; }
        List<IMember> Members { get; }
        IDefine? Define { get; }
        string BaseName { get; }

        // Find a member matching the predicate that is closest to the top of the memory in the message struct
        bool FindTopMostMember(Predicate<IMember> memberTest, Stack<IMember> stackMembers);

        // Find a member matching the predicate that is highest in the member hierarchy
        bool FindMember(Predicate<IMember> memberTest, Stack<IMember> stackMembers);

        // Find all members matching the predicate as a member hierarchy
        // The top of each deque will be the left most member
        bool FindAllMembers(Predicate<IMember> memberTest, List<Queue<IMember>> stackMembers);

        // Find all members with a message type, matching the predicate as a member hierarchy
        // The top of each deque will be the left most member
        bool FindAllMessageMembers(Predicate<IMessage> messageMemberTest, List<Queue<IMember>> stackMembers);

        // Determine if the message type has a name clash when spurious characters are stripped from the members
        bool HasStrippedNameMemberClash { get; }

        // Attribute interface
        string? PrimaryDescription { get; }
        string? ExtendedDescription { get; }
        string? Category { get; }
        string? Generator { get; }
        string[]? Generators { get; }
        string Consumer { get; }
        string[]? Consumers { get; }
        string AlertLevel { get; }
        string? WabFilter { get; }
        string[]? WabFilters { get; }
        IAttribute? WabFilterAttribute { get; }
        string? Reason { get; }

        bool IsNonMessage { get; }

        int MessageByteSize { get; }

        void AddConsumer(string name);
        void AddGeneratedMessages(List<IMessage> listMessages);
        List<IMessage>? GetGeneratedMessages();
        void AddTraceMember(TraceAssociation traceAssociation, ExternalKeyGenerator? externalKey);
        List<TraceAssociation>? TraceMembers { get; }
        ExternalKeyGenerator? ExternalKey { get; }

        void SetTimestampFilter(TimestampFilter timestampFilter);
        bool GetUseTimestampRange(IMessage messageFor);
    }
}

