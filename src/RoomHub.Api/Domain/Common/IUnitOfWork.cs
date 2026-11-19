namespace RoomHub.Api.Domain.Common;

// Saves all the changes of one request together.
public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct);
}
