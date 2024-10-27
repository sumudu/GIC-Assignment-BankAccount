using System;
using AwesomeGIC.Domain.Enum;
using AwesomeGIC.Domain;
using AwesomeGIC.Infrastructure.Interfaces;
using Moq;
using Xunit;
using AwesomeGIC.Application;
using AwesomeGIC.Application.Interfaces;

namespace AwesomeGICTest
{
    public class TransactionServiceTests
    {
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly Mock<IInterestService> _interestServiceMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            _interestServiceMock = new Mock<IInterestService>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();

            _transactionService = new TransactionService(
                _bankAccountRepositoryMock.Object,
                _interestServiceMock.Object,
                _transactionRepositoryMock.Object
            );
        }
       

        [Fact]
        public void GenerateTransactionId_ShouldCreateUniqueIdForDate()
        {
            // Arrange
            var date = new DateTime(2024, 10, 15);
            _transactionRepositoryMock.Setup(r => r.GetAllTransactions()).Returns(new List<Transaction>());

            // Act
            var transactionId = _transactionService.GenerateTransactionId(date);

            // Assert
            Assert.Equal("20241015-01", transactionId);
        }       
       

        [Fact]
        public void PrintStatement_AccountNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            string accountNumber = "12345";
            int year = 2024, month = 10;
            _bankAccountRepositoryMock.Setup(r => r.GetByAccountNumber(accountNumber)).Returns((BankAccount)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _transactionService.PrintStatement(accountNumber, year, month));
        }

    }
}