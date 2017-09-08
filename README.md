# Azure Event Grid Sink for Serilog

All log events are sent to a custom event grid topic as an HTTP Post. For more information on Event Grid, see https://docs.microsoft.com/en-us/azure/event-grid/

## Targets

* [.NET Standard 2](https://github.com/dotnet/standard/blob/master/docs/versions.md) (netstandard2)
* .NET Framework 4.6.1 (net461)

## Usage

```csharp
Log.Logger = new LoggerConfiguration()
          .WriteTo.EventGrid("TopicKeyorSASTokenString", 
            "https://my-topic-name.westus2-1.eventgrid.azure.net/api/events",
            "my/custom/subject",
            "myCustomType",
            "SubjectPropertyName",
            "EventPropertyName",
            restrictedToMinimumLevel: LogEventLevel.Information)
          .CreateLogger()
```

### Required

**key** (string)

The primary or secondary topic key, recommended for the most trusted events, or, a secure access signature (sas) with the appropriate permission (generate via the azure cli).

**topicEndpoint** (string)

The full topic uri, containing custom topic name and region. You can see this in the azure portal or retrieve it via the azue cli.

### Optional

**customEventSubject** (string)

The subject of every event sent by this logger configuration

**customEventType** (string)

The type of every event sent by this logger configuration

**customSubjectPropertyName** (string)

Name of the property added to the Serilog log event that will contain the Subject of the event

```csharp
Log.ForContext("SubjectPropertyName", "my/custom/subject/id").Information("{@OtherData}", otherData)
```

**customTypePropertyName** (string)

Name of the property added to the Serilog log event that will contain the Type of the event

```csharp
Log.ForContext("EventPropertyName", "myCustomType").Information("{@OtherData}", otherData)
```

### Custom Attributes

In to specifying the subject and type through configuration or properties, you can rely on reflection at runtime using the `[EventGridSink]` Attribute. You can decorate any method or class with the attribute specifying the subject, type, or both. The Serilog log event called within the context of a method or class decorated with the attribute, will use those values when submitting the event.

```csharp
[EventGridSink("MyCustomType")]
public class MyLogicClass
{ 
  [EventGridSink(null, "MyCustomSubject/DoSomething")]
  public void DoSomething(UserContext user)
  {
    Log.Information("{@user}", user);
  }
}
```

In the above example, the information log event would have a subject of `MyCustomSubject/DoSomething` and a type of `MyCustomType`

### When you don't specify a custom subject or type

The simplest way to use the sink, is to let it set the subject and type for you. If neither was supplied using the above methods, then the sink will use reflection to walk back up the stack trace and attempt to add a meaningful subject and type for your event. The subject is based on the calling apps assembly name, class name, and method name in the following format: `AssemblyName/ClassName/MethodName`. The type will combine the calling method name and any parameter names for that method in the following format: `MethodName.param1Name.param2Name`. This is the resulting event based on the example class:

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
  "eventType": "DoSomething.user",
  "subject": "MyClassLib/MyLogicClass/DoSomething",
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



