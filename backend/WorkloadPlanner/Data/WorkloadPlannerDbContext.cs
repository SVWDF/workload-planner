using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkloadPlanner.Models;

namespace WorkloadPlanner.Data
{
    public class WorkloadPlannerDbContext : IdentityDbContext<ApplicationUser>
    {
        public WorkloadPlannerDbContext() { }

        public WorkloadPlannerDbContext(DbContextOptions<WorkloadPlannerDbContext> options) : base(options) { }

        public DbSet<ScrumBoard> ScrumBoards { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
