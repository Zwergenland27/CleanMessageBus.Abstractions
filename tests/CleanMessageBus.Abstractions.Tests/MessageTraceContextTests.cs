using System.Diagnostics;
using System.Text.Json;

namespace CleanMessageBus.Abstractions.Tests;

public sealed class MessageTraceContextTests
{
    [Fact]
    public void Capture_ShouldReturnNull_WhenNoActivityIsGiven()
    {
        Assert.Null(MessageTraceContext.Capture(null));
    }

    [Fact]
    public void Capture_ShouldContainTraceAndSpanId_WhenActivityIsGiven()
    {
        using var activity = new Activity("test").SetIdFormat(ActivityIdFormat.W3C).Start();
        activity.TraceStateString = "vendor=value";

        var context = MessageTraceContext.Capture(activity)!;

        Assert.True(context.TryGetParent(out var parent));
        Assert.Equal(activity.TraceId, parent.TraceId);
        Assert.Equal(activity.SpanId, parent.SpanId);
        Assert.Equal("vendor=value", context.TraceState);
        Assert.Null(context.Baggage);
    }

    [Fact]
    public void GetBaggage_ShouldReturnDecodedEntries_WhenCapturedBaggageContainsSpecialCharacters()
    {
        using var activity = new Activity("test").SetIdFormat(ActivityIdFormat.W3C).Start();
        activity.AddBaggage("tenant", "tenant 1,a=b");

        var context = MessageTraceContext.Capture(activity)!;

        var entry = Assert.Single(context.GetBaggage());
        Assert.Equal("tenant", entry.Key);
        Assert.Equal("tenant 1,a=b", entry.Value);
    }

    [Fact]
    public void FromHeaders_ShouldRestoreContext_WhenWrittenWithWriteTo()
    {
        var context = new MessageTraceContext("00-0af7651916cd43dd8448eb211c80319c-b7ad6b7169203331-01", "a=b", "k=v");
        var headers = new Dictionary<string, string>();

        context.WriteTo(headers);

        Assert.Equal(context, MessageTraceContext.FromHeaders(headers));
    }

    [Fact]
    public void FromHeaders_ShouldReturnNull_WhenTraceParentIsMissing()
    {
        Assert.Null(MessageTraceContext.FromHeaders(new Dictionary<string, string>()));
    }

    [Fact]
    public void Deserialize_ShouldRestoreContext_WhenStoredAsJson()
    {
        var context = new MessageTraceContext("00-0af7651916cd43dd8448eb211c80319c-b7ad6b7169203331-01", baggage: "k=v");

        var json = JsonSerializer.Serialize(context);

        Assert.Equal(context, JsonSerializer.Deserialize<MessageTraceContext>(json));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTraceParentIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => new MessageTraceContext(" "));
    }
}