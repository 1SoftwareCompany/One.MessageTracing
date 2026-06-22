namespace One.MessageTracing;

public record class MessageTraceInfo
{
    public MessageTraceInfo(string messageId, string causationId, string correlationId)
    {
        MessageId = messageId;
        CausationId = causationId;
        CorrelationId = correlationId;
    }

    public string MessageId { get; private set; }
    public string CausationId { get; private set; }
    public string CorrelationId { get; private set; }
}