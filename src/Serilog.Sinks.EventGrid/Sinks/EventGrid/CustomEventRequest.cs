using System;

namespace Serilog.Sinks.EventGrid
{
  public enum CustomEventRequestAuth
  {
    Key,
    SAS
  }

  [Serializable]
  class CustomEventRequest
  {
    public string Id { get; set; }
    public string EventType { get; set; }
    public string Subject { get; set; }
    public string EventTime { get; set; }
    public object Data { get; set; }

    public CustomEventRequest()
    {
      Id = Guid.NewGuid().ToString();
      EventTime = DateTime.UtcNow.ToString("o");
    }
  }
}
