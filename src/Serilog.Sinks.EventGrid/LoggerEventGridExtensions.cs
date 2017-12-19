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
    public static void Event(this ILogger logger, string eventType, string subject, string messageTemplate = "", params object[] props)
    {
      if (!string.IsNullOrEmpty(subject))
        logger = logger.ForContext("EventSubject", subject);

      if (!string.IsNullOrEmpty(eventType))
        logger = logger.ForContext("EventType", eventType);

      logger.Information(messageTemplate, props);
    }

    /// <summary>Log to Event Grid with custom subject and message template w/ custom args</summary>
    /// <param name="logger">The Serilog ILogger</param>
    /// <param name="subject">The event subject sent to EventGrid Grid</param>
    /// <param name="messageTemplate">The Serilog logger message template</param>
    /// <param name="props">The values references in the templace to be added to the EventGrid Grid data payload</param>
    public static void EventSubject(this ILogger logger, string subject, string messageTemplate, params object[] props)
    {
      logger.Event(null, subject, messageTemplate, props);
    }

    /// <summary>Log to Event Grid with custom type and message template</summary>
    /// <param name="logger">The Serilog ILogger</param>
    /// <param name="subject">The event subject sent to EventGrid Grid</param>
    /// <param name="messageTemplate">The Serilog logger message template</param>
    public static void EventSubject(this ILogger logger, string subject, string messageTemplate)
    {
      logger.Event(null, subject, messageTemplate, null);
    }

    /// <summary>Log to Event Grid with custom type and message template w/ custom args</summary>
    /// <param name="logger">The Serilog ILogger</param>
    /// <param name="eventType">The event type sent to EventGrid Grid</param>
    /// <param name="messageTemplate">The Serilog logger message template</param>
    /// <param name="props">The values references in the templace to be added to the EventGrid Grid data payload</param>
    public static void EventType(this ILogger logger, string eventType, string messageTemplate, params object[] props)
    {
      logger.Event(eventType, null, messageTemplate, props);
    }

    /// <summary>Log to Event Grid with custom type and message template</summary>
    /// <param name="logger">The Serilog ILogger</param>
    /// <param name="eventType">The event type sent to EventGrid Grid</param>
    /// <param name="messageTemplate">The Serilog logger message template</param>
    public static void EventType(this ILogger logger, string eventType, string messageTemplate)
    {
      logger.Event(eventType, null, messageTemplate, null);
    }
  }
}
