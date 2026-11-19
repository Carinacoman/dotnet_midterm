using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class EfUnitOfWork(RoomHubDbContext db) : IUnitOfWork
{
    public Task CommitAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
