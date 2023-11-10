using Application.Repositories;
using Application.Repositories.CourierRepository;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IReadCourierRepository readCourierRepository, IWriteCourierRepository writeCourierRepository)
    {
        ReadCourierRepository = readCourierRepository;
        WriteCourierRepository = writeCourierRepository;
    }

    public IReadCourierRepository ReadCourierRepository { get; }
    public IWriteCourierRepository WriteCourierRepository { get; }
}
