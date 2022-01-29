using Microsoft.EntityFrameworkCore;
using NodaTime;
using Quartz;
using voonex.api.Models;

namespace voonex.api.Services.Jobs;

public class SessionCleanUp : IJob
{
    private readonly IDbContextFactory<VoonexDbContext> _dbContextFactory;

    public SessionCleanUp(IDbContextFactory<VoonexDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var timeout = SystemClock.Instance.GetCurrentInstant().Minus(Duration.FromDays(30));
        
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var timedOutSessions = await dbContext.Set<Session>().AsQueryable()
            .Where(x => x.CreatedAt <= timeout)
            .ToListAsync();

        dbContext.Set<Session>().RemoveRange(timedOutSessions);
        await dbContext.SaveChangesAsync();
    }
}