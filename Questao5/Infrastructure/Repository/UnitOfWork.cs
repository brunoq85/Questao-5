using Questao5.Domain.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbTransaction _transaction;
        private readonly IDbConnection _connection;
        private bool _disposed;

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IDbTransaction Transaction => _transaction;

        public async Task CommitAsync()
        {
            try
            {
                await Task.Run(() => _transaction.Commit());
            }
            catch
            {
                await Task.Run(() => _transaction.Rollback());
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction.Dispose();
                _connection.Close();
                _disposed = true;
            }
        }
    }
}
