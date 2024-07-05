using System.Data;

namespace Questao5.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction Transaction { get; }
        Task CommitAsync();
        void Rollback();
    }
}
