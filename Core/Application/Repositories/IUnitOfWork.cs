using Application.Repositories.CourierRepository;

namespace Application.Repositories;

public interface IUnitOfWork
{
    IReadCourierRepository ReadCourierRepository { get;  }
    IWriteCourierRepository WriteCourierRepository { get;  }  
}
