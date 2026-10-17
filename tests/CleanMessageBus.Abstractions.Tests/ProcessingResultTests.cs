namespace CleanMessageBus.Abstractions.Tests;

public sealed class ProcessingResultTests
{
    [Fact]
    public void Retry_ShouldCountAsFailedAttempt_WhenNotSpecified()
    {
        var exception = new InvalidOperationException();

        var result = ProcessingResult.Retry(TimeSpan.FromSeconds(1), exception);

        Assert.Equal(ProcessingOutcome.Retry, result.Outcome);
        Assert.Equal(TimeSpan.FromSeconds(1), result.RetryDelay);
        Assert.True(result.CountsAsFailedAttempt);
        Assert.Same(exception, result.Exception);
    }

    [Fact]
    public void Retry_ShouldThrow_WhenDelayIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ProcessingResult.Retry(TimeSpan.FromSeconds(-1)));
    }

    [Fact]
    public void DeadLetter_ShouldKeepReason_WhenCreated()
    {
        var result = ProcessingResult.DeadLetter(null, "broken payload");

        Assert.Equal(ProcessingOutcome.DeadLetter, result.Outcome);
        Assert.Equal("broken payload", result.Reason);
        Assert.False(result.CountsAsFailedAttempt);
    }

    [Fact]
    public void DeadLetter_ShouldThrow_WhenReasonIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => ProcessingResult.DeadLetter(null, " "));
    }

    [Fact]
    public void Succeeded_ShouldHaveNoException_WhenUsed()
    {
        Assert.Equal(ProcessingOutcome.Succeeded, ProcessingResult.Succeeded.Outcome);
        Assert.Null(ProcessingResult.Succeeded.Exception);
    }

    [Fact]
    public void RetryAfterException_ShouldThrow_WhenDelayIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new RetryAfterException(TimeSpan.FromSeconds(-1)));
    }
}