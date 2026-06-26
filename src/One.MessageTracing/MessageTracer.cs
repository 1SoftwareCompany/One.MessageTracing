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
        try
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
        catch (Exception ex)
        {
            if (string.IsNullOrEmpty(messageId))
                messageId = Guid.NewGuid().ToString();

            return new MessageTraceInfo(messageId, messageId, messageId, string.Empty);
        }
    }

    public void Record(string incomingMessageId, string correlationId = null)
    {
        foreach (IMessageTracer tracer in _tracers)
        {
            try
            {
                tracer.Record(incomingMessageId, correlationId);
            }
            catch (Exception) { }
        }
    }

    private static bool IsFirstMessage(MessageTraceInfo messageTraceInfo)
    {
        return
            messageTraceInfo.MessageId.Equals(messageTraceInfo.CausationId, StringComparison.OrdinalIgnoreCase)
            && messageTraceInfo.MessageId.Equals(messageTraceInfo.CorrelationId, StringComparison.OrdinalIgnoreCase);
    }
}
