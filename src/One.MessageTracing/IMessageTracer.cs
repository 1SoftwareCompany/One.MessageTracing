namespace One.MessageTracing;

public interface IMessageTracer
{
    void Record(string incomingMessageId, string correlationId = null);

    MessageTraceInfo CreateTrace(string messageId = null);
}
