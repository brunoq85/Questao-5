using Moq;
using Questao5.Infrastructure.Repository;
using System.Data;
using Xunit;

namespace Questao5.Tests.Unidade
{
    public class UnitOfWorkTests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDbTransaction> _mockTransaction;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _mockTransaction = new Mock<IDbTransaction>();
            _mockConnection.Setup(c => c.BeginTransaction()).Returns(_mockTransaction.Object);

            _unitOfWork = new UnitOfWork(_mockConnection.Object);
        }

        [Fact]
        public async Task CommitAsync_CommitsTransaction()
        {
            // Act
            await _unitOfWork.CommitAsync();

            // Assert
            _mockTransaction.Verify(t => t.Commit(), Times.Once);
            _mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task CommitAsync_RollsBackTransaction_OnFailure()
        {
            // Arrange
            _mockTransaction.Setup(t => t.Commit()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _unitOfWork.CommitAsync());
            _mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }

        [Fact]
        public void Rollback_CallsTransactionRollback()
        {
            // Act
            _unitOfWork.Rollback();

            // Assert
            _mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }

        [Fact]
        public void Dispose_CallsTransactionDispose_AndClosesConnection()
        {
            // Act
            _unitOfWork.Dispose();

            // Assert
            _mockTransaction.Verify(t => t.Dispose(), Times.Once);
            _mockConnection.Verify(c => c.Close(), Times.Once);
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            // Act
            _unitOfWork.Dispose();
            _unitOfWork.Dispose();

            // Assert
            _mockTransaction.Verify(t => t.Dispose(), Times.Once);
            _mockConnection.Verify(c => c.Close(), Times.Once);
        }
    }
}
