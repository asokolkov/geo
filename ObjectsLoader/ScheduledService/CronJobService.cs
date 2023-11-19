using Cronos;
using Microsoft.Extensions.Hosting;

namespace ObjectsLoader.ScheduledService;

public abstract class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer? timer;
        private readonly Cronos.CronExpression expression;
        private readonly TimeZoneInfo timeZoneInfo;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            expression = Cronos.CronExpression.Parse(cronExpression);
            this.timeZoneInfo = timeZoneInfo;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = expression.GetNextOccurrence(DateTimeOffset.Now, timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken);
                }
                timer = new System.Timers.Timer(delay.TotalMilliseconds);
                timer.Elapsed += async (_, _) =>
                {
                    timer.Dispose();  // reset and dispose timer
                    timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                timer.Start();
            }
            await Task.CompletedTask;
        }

        public abstract Task DoWork(CancellationToken cancellationToken);

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }