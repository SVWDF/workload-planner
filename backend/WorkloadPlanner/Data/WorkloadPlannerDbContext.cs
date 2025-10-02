using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WorkloadPlanner.Data
{
    public class WorkloadPlannerDbContext : IdentityDbContext
    {
        public WorkloadPlannerDbContext() { }

        public WorkloadPlannerDbContext(DbContextOptions<WorkloadPlannerDbContext> options) : base(options) { }
    }
}
