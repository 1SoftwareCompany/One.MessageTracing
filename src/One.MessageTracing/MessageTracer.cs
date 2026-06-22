namespace One.MessageTracing;

public sealed class MessageTracer
{
    private readonly IEnumerable<IMessageTracer> _tracers;
    private readonly IEnumerable<IMessageTraceWriter> messageTraceWriters;

    public MessageTracer(IEnumerable<IMessageTracer> tracers, IEnumerable<IMessageTraceWriter> messageTraceWriters)
    {
        _tracers = tracers;
        this.messageTraceWriters = messageTraceWriters;
    }

    public async ValueTask<MessageTraceInfo> CreateTraceAsync(string messageId = null)
    {
        MessageTraceInfo traceInfo = null;
        foreach (IMessageTracer tracer in _tracers)
        {
            traceInfo = tracer.CreateTrace(messageId);
            if (IsFirstMessage(traceInfo) == false)
                break;
        }

        List<Task> writersTasks = new List<Task>();
        foreach (IMessageTraceWriter writer in messageTraceWriters)
        {
            writersTasks.Add(writer.WriteAsync(traceInfo));
        }
        await Task.WhenAll(writersTasks).ConfigureAwait(false);

        return traceInfo;
    }

    public void Record(string incomingMessageId, string correlationId = null)
    {
        foreach (IMessageTracer tracer in _tracers)
        {
            tracer.Record(incomingMessageId, correlationId);
        }
    }

    private static bool IsFirstMessage(MessageTraceInfo messageTraceInfo)
    {
        return
            messageTraceInfo.MessageId.Equals(messageTraceInfo.CausationId, StringComparison.OrdinalIgnoreCase)
            && messageTraceInfo.MessageId.Equals(messageTraceInfo.CorrelationId, StringComparison.OrdinalIgnoreCase);
    }
}