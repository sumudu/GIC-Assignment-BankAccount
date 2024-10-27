
using AwesomeGIC.Domain;
using System;
using Xunit;

namespace AwesomeGICTest
{
    public class BankAccountTests
    {
        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            // Arrange
            var account = new BankAccount("12345");
            var transactionId = "20241027-01";

            // Act
            account.Deposit(100m, DateTime.Now, transactionId);

            // Assert
            Assert.Equal(100m, account.Balance);
        }

        [Fact]
        public void Deposit_NegativeAmount_ShouldThrowArgumentException()
        {
            // Arrange
            var account = new BankAccount("12345");
            var transactionId = "20241027-01";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => account.Deposit(-100m, DateTime.Now, transactionId));
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalance()
        {
            // Arrange
            var account = new BankAccount("12345");
            var transactionId = "20241027-01";
            account.Deposit(100m, DateTime.Now, transactionId);

            // Act
            account.Withdraw(50m, DateTime.Now, transactionId);

            // Assert
            Assert.Equal(50m, account.Balance);
        }

        [Fact]
        public void Withdraw_InsufficientBalance_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var account = new BankAccount("12345");
            var transactionId = "20241027-01";
            account.Deposit(50m, DateTime.Now, transactionId);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(100m, DateTime.Now, transactionId));
        }

        [Fact]
        public void GetTransactions_ShouldReturnReadOnlyList()
        {
            // Arrange
            var account = new BankAccount("12345");
            var transactionId = "20241027-01";
            account.Deposit(100m, DateTime.Now, transactionId);

            // Act
            var transactions = account.GetTransactions();

            // Assert
            Assert.Single(transactions);
        }
    }
}
