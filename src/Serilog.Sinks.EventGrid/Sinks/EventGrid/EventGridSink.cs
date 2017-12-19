using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.EventGrid
{
  public class EventGridSink : PeriodicBatchingSink
  {
    private readonly EventGridClient _client;

    public const int DefaultBatchPostingLimit = 10;
    public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(5);

    public EventGridSink(IFormatProvider formatProvider,
      string key,
      Uri topicUri,
      string customEventSubject,
      string customEventType,
      CustomEventRequestAuth customEventRequestAuth,
      string customSubjectPropertyName,
      string customTypePropertyName,
      int batchSizeLimit,
      TimeSpan period) : base(batchSizeLimit, period)
    {
      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentNullException("key");

      _client = new EventGridClient(key, topicUri, customEventSubject, customEventType, customEventRequestAuth, customSubjectPropertyName, customTypePropertyName);
    }

    protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
    {
      var tasks = events.Select(_client.SendEvent);
      await Task.WhenAll(tasks);
    }
  }
}
