namespace One.MessageTracing;

public interface IMessageTraceWriter
{
    Task WriteAsync(MessageTraceInfo messageTraceInfo);
}
