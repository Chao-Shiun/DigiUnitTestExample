using BankTransaction.Infrastructure.Enum;
using BankTransaction.Repository.Interface;
using BankTransaction.Service;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BankTransaction.Test
{
    [TestFixture]
    public class BankService_Test
    {
        private Mock<IDepositRepository> _mockDepositRepository;
        private Mock<IWithdrawRepository> _mockWithdrawRepository;
        private BankService _bankService;

        [SetUp]
        public void Setup()
        {
            _mockDepositRepository = new Mock<IDepositRepository>();
            _mockWithdrawRepository = new Mock<IWithdrawRepository>();
            _bankService = new BankService(_mockDepositRepository.Object, _mockWithdrawRepository.Object);
        }

        [Test]
        public void PerformTransaction_Deposit_Successful()
        {
            // Arrange
            decimal amount = 100m;
            _mockDepositRepository.Setup(r => r.Deposit(amount)).Returns((true, amount));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Deposit);

            // Assert
            result.Should().Be("Transaction successful deposit $100");
            _mockDepositRepository.Verify(r => r.Deposit(amount), Times.Once);
        }

        [Test]
        public void PerformTransaction_Deposit_Failed()
        {
            // Arrange
            decimal amount = 100m;
            _mockDepositRepository.Setup(r => r.Deposit(amount)).Returns((false, 0m));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Deposit);

            // Assert
            result.Should().Be("Transaction failed");
            _mockDepositRepository.Verify(r => r.Deposit(amount), Times.Once);
        }

        [Test]
        public void PerformTransaction_Withdraw_Successful()
        {
            // Arrange
            decimal amount = 50m;
            _mockWithdrawRepository.Setup(r => r.Withdraw(amount)).Returns((true, amount));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Withdraw);

            // Assert
            result.Should().Be("Transaction successful withdraw $50");
            _mockWithdrawRepository.Verify(r => r.Withdraw(amount), Times.Once);
        }

        [Test]
        public void PerformTransaction_Withdraw_Failed()
        {
            // Arrange
            decimal amount = 50m;
            _mockWithdrawRepository.Setup(r => r.Withdraw(amount)).Returns((false, 0m));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Withdraw);

            // Assert
            result.Should().Be("Transaction failed");
            _mockWithdrawRepository.Verify(r => r.Withdraw(amount), Times.Once);
        }
    }
}