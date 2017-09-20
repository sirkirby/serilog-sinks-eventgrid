namespace Serilog.Sinks.EventGrid
{
  public static class LoggerEventGridExtensions
  {
    /// <summary>Information log event helper for EventGrid sink</summary>
    /// <param name="logger">The Serilog ILogger</param>
    /// <param name="eventType">The event type sent to EventGrid Grid</param>
    /// <param name="subject">The event subject sent to EventGrid Grid</param>
    /// <param name="messageTemplate">The Serilog logger message template</param>
    /// <param name="props">The values references in the templace to be added to the EventGrid Grid data payload</param>
    public static void EventGrid(this ILogger logger, string eventType, string subject, string messageTemplate, params object[] props)
    {
      if (!string.IsNullOrEmpty(subject))
        logger = logger.ForContext("EventSubject", subject);
      if (!string.IsNullOrEmpty(eventType))
        logger = logger.ForContext("EventType", eventType);
      if (!string.IsNullOrEmpty(messageTemplate))
        logger.Information(messageTemplate, props);
    }

    public static void EventGrid(this ILogger logger, string eventType, string messageTemplate, params object[] props)
    {
      logger.EventGrid(eventType, null, messageTemplate, props); 
    }

    public static void EventGrid(this ILogger logger, string eventType, string messageTemplate)
    {
      logger.EventGrid(eventType, null, messageTemplate, null);
    }
  }
}
