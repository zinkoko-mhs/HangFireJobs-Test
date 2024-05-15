using BackgorundJobsWithHangFire.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackgorundJobsWithHangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost("CreateBackgroundJob")]
        public IActionResult CreateBackgroundJob()
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Background Job Triggered"));

            BackgroundJob.Enqueue<TestJob>(x => x.WriteLog("Log Writed"));
            return Ok();
        }

        [HttpPost("CreateScheduledJob")]
        public IActionResult CreateScheduledJob()
        {
            var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);

            //API Called Time
            BackgroundJob.Enqueue(() => Console.WriteLine($"API Called At {DateTime.UtcNow}"));

            BackgroundJob.Schedule(() => Console.WriteLine($"Scheduled Job Triggered At {scheduledDateTime}"), dateTimeOffSet);
            return Ok();
        }

        [HttpPost("CreateContinuationJob")]
        public IActionResult CreateContinuationJob()
        {
            var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);

            //Scheduled Job
            var scheduledJobId = BackgroundJob.Schedule(() => Console.WriteLine("Scheduled Job Triggered"), dateTimeOffSet);

            //Continuation Jobs
            var con1JobId = BackgroundJob.ContinueJobWith(scheduledJobId, () => Console.WriteLine($"ContinuationJob 1 after {scheduledJobId}"));
            var con2JobId = BackgroundJob.ContinueJobWith(con1JobId, () => Console.WriteLine($"ContinuationJob 2 after {con1JobId}"));
            var con3JobId = BackgroundJob.ContinueJobWith(con2JobId, () => Console.WriteLine($"ContinuationJob 3 after {con2JobId}"));

            return Ok();
        }

        [HttpPost("CreateRecurringJob")]
        public IActionResult CreateRecurringJob()
        {
                                    // Recurring Job Id  ,  Method   , cron expression (this one runs every minute)        
            RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("Recurring Job Triggered"), "* * * * *");
            return Ok();
        }
    }
}
