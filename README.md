# Azure Event Grid Sink for Serilog

All log events are sent to a custom event grid topic as an HTTP Post. For more information on Event Grid, see https://docs.microsoft.com/en-us/azure/event-grid/

[![NuGet](https://img.shields.io/nuget/v/Serilog.Sinks.EventGrid.svg)](https://www.nuget.org/packages/Serilog.Sinks.EventGrid/) [![Build status](https://ci.appveyor.com/api/projects/status/uxmd0qanuk1eltrg/branch/master?svg=true)](https://ci.appveyor.com/project/Authenticom/serilog-sinks-eventgrid/branch/master)

## Targets

* [.NET Standard 2](https://github.com/dotnet/standard/blob/master/docs/versions.md) (netstandard2)
* .NET Framework 4.6.1 (net461)

## Usage

```csharp
// Full
Log.Logger = new LoggerConfiguration()
          .WriteTo.EventGrid("TopicKeyorSASTokenString", 
            "https://my-topic-name.westus2-1.eventgrid.azure.net/api/events",
            "my/custom/subject",
            "myCustomType",
            "SubjectPropertyName",
            "EventPropertyName",
            restrictedToMinimumLevel: LogEventLevel.Information)
          .CreateLogger()
  
  // Min
  Log.Logger = new LoggerConfiguration().WriteTo.EventGrid().CreateLogger()
```

### Required

**key** (string)

The primary or secondary topic key, recommended for the most trusted events, or, a secure access signature (sas) with the appropriate permission (generate via the azure cli).

**topicEndpoint** (string)

The full topic uri, containing custom topic name and region. You can see this in the azure portal or retrieve it via the azue cli.

Alternatively, you can pass the **key** and **topicEndPoint** through your `<appSettings>`: 

```xml
<appSettings>
  <add key="EventGridTopicKey" value="TopicKeyorSASTokenString" />
  <add key="EventGridTopicUri" value="https://my-topic-name.westus2-1.eventgrid.azure.net/api/events" />
</appSettings>
```

### Optional

**customEventSubject** (string)

The subject of every event sent by this logger configuration

**customEventType** (string)

The type of every event sent by this logger configuration

**customSubjectPropertyName** (string)

Name of the property added to the Serilog log event that will contain the Subject of the event. By default, the property key it's looking for is `EventSubject`

```csharp
Log.ForContext("SubjectPropertyName", "my/custom/subject/id").Information("{@OtherData}", otherData)
```

**customTypePropertyName** (string)

Name of the property added to the Serilog log event that will contain the Type of the event.  By default, the property key it's looking for is `EventType`

```csharp
Log.ForContext("EventPropertyName", "myCustomType").Information("{@OtherData}", otherData)
```

**restrictedToMinimumLevel** (enum)

Specify the Serilog logging level. Default is `LogEventLevel.Information`

### Helper Extensions

The sink adds additional extension methods to `ILogger` which can help simplify your Event Grid logging code:

```csharp
using Serilog.Sinks.EventGrid
```

```csharp
// type, subject, and information event message with properties
Log.Event("myEventTypeName", "myEventSubjectName", "This is my Event {@MyContext}", myContext);
// type or subject with information event message and properties
Log.EventType("myEventTypeName", "This is my Event {@MyContext}", myContext);
Log.EventSubject("myEventSubjectName", "This is my Event {@MyContext}", myContext);
// type or subject with information event message
Log.EventType("myEventTypeName", "This is my Event");
Log.EventSubject("myEventSubjectName", "This is my Event");
```

### Custom Attributes

As an alternative to specifying the subject and type through log configuration or properties, you can decorate your code at design time with the `[EventGridSubject]` and `[EventGridType]` Attributes. Any method or class is supported, using one or both on each. The Serilog log event called within the context of a method or class decorated with either attribute, will use those values when submitting the event. The first ones closest to the log event call in the stack, win.

```csharp
[EventGridType("MyCustomType")]
public class MyLogicClass
{ 
  [EventGridSubject("MyCustomSubject/DoSomething")]
  public void DoSomething(UserContext user)
  {
    Log.Information("{@user}", user);
  }
}
```

In the above example, the information log event would have a subject of `MyCustomSubject/DoSomething` and a type of `MyCustomType`

### When you don't specify a custom subject or type

The simplest way to use the sink, is to let it set the subject and type for you. If neither was supplied using the above methods, then the sink will use reflection to walk back up the stack trace and attempt to add a meaningful subject and type for your event. The subject will be derived from the calling method's name, as well as it's parameter names, in the following format: `MethodName/param1/param2`. The type will be derived from the calling method's class name and the class's assembly name, in the following format: `AssemblyName/ClassName`. This is the resulting event based on the example class:

```csharp
public class MyLogicClass
{ 
  public void DoSomething(UserContext user)
  {
    Log.Information("{@user}", user);
  }
}
```

```json
[{
  "id": "15065e6e-5c0e-4258-836a-8cfc6f7b0efd",
  "eventType": "MyClassLib/MyLogicClass",
  "subject": "DoSomething/user",
  "eventTime": "2017-09-07T19:35:04.9853198Z",
  "data": [
    {
      "key": "user",
      "value": 
      {
        "name": "First Last",
        "username": "developer",
        "$typeTag": "UserContext"
      }
    }
  ],
  "topic" "subscriptionInfo"
}]
```



