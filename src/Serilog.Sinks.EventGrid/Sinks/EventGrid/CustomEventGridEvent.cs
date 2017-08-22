using System;

namespace Serilog.Sinks.EventGrid.Sinks.EventGrid
{
  [Serializable]
  public class CustomEventGridEvent
  {
    public string Id { get; set; }
    public string EventType { get; set; }
    public string Subject { get; set; }
    public string EventTime { get; set; }
    public object Data { get; set; }

    public CustomEventGridEvent()
    {
      Id = Guid.NewGuid().ToString();
      EventTime = DateTime.UtcNow.ToString("o");
    }
  }
}
