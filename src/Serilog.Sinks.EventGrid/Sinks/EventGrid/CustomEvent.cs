using System;

namespace Serilog.Sinks.EventGrid
{
  public enum CustomEventAuth
  {
    Key,
    SAS
  }

  [Serializable]
  class CustomEvent
  {
    public string Id { get; set; }
    public string EventType { get; set; }
    public string Subject { get; set; }
    public string EventTime { get; set; }
    public object Data { get; set; }

    public CustomEvent()
    {
      Id = Guid.NewGuid().ToString();
      EventTime = DateTime.UtcNow.ToString("o");
    }
  }
}
