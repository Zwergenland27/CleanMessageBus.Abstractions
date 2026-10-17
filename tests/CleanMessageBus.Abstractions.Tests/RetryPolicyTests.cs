namespace CleanMessageBus.Abstractions.Tests;

public sealed class RetryPolicyTests
{
    [Fact]
    public void Fixed_ShouldRepeatInterval_WhenRetriesArePositive()
    {
        var policy = RetryPolicy.Fixed(3, TimeSpan.FromSeconds(5));

        Assert.Equal(3, policy.MaxRetries);
        Assert.All(policy.Intervals, interval => Assert.Equal(TimeSpan.FromSeconds(5), interval));
    }

    [Fact]
    public void Exponential_ShouldMultiplyInterval_WhenFactorIsGiven()
    {
        var policy = RetryPolicy.Exponential(4, TimeSpan.FromSeconds(1), factor: 3);

        Assert.Equal(
            new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(9), TimeSpan.FromSeconds(27) },
            policy.Intervals);
    }

    [Fact]
    public void None_ShouldHaveNoRetries_WhenUsed()
    {
        Assert.Equal(0, RetryPolicy.None.MaxRetries);
        Assert.False(RetryPolicy.None.TryGetRetryDelay(1, out _));
    }

    [Theory]
    [InlineData(1, true, 10)]
    [InlineData(2, true, 20)]
    [InlineData(3, false, 0)]
    public void TryGetRetryDelay_ShouldReturnIntervalOfFailedAttempt_WhenRetriesAreLeft(int failedAttempt, bool expected, int expectedSeconds)
    {
        var policy = new RetryPolicy([TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20)]);

        var result = policy.TryGetRetryDelay(failedAttempt, out var delay);

        Assert.Equal(expected, result);
        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), delay);
    }

    [Fact]
    public void TryGetRetryDelay_ShouldThrow_WhenAttemptIsLessThanOne()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RetryPolicy.Default.TryGetRetryDelay(0, out _));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenIntervalIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new RetryPolicy([TimeSpan.FromSeconds(-1)]));
    }

    [Fact]
    public void Constructor_ShouldCopyIntervals_WhenSourceChangesAfterwards()
    {
        var source = new List<TimeSpan> { TimeSpan.FromSeconds(1) };

        var policy = new RetryPolicy(source);
        source.Add(TimeSpan.FromSeconds(2));

        Assert.Equal(1, policy.MaxRetries);
    }
}