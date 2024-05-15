using Microsoft.EntityFrameworkCore;

namespace BackgorundJobsWithHangFire.Context
{
    public class HangFireContext : DbContext
    {
        public HangFireContext(DbContextOptions options) : base(options)
        {
        }
    }
}
