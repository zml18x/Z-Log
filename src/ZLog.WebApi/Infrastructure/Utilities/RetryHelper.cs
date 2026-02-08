namespace ZLog.WebApi.Infrastructure.Utilities;

public static class RetryHelper
{
    public static async Task ExecuteAsync(
        Func<Task> action, 
        int maxRetries, 
        TimeSpan delay, 
        Action<Exception, int>? onRetry = null)
    {
        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                await action();
                return;
            }
            catch (Exception ex)
            {
                if (attempt == maxRetries)
                    throw; 
                
                onRetry?.Invoke(ex, attempt);
                
                await Task.Delay(delay);
            }
        }
    }
}