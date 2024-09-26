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
        // Mock 對象：用於模擬和驗證與真實對象的交互
        private Mock<IDepositRepository> _mockDepositRepository;
        private Mock<IWithdrawRepository> _mockWithdrawRepository;
        private BankService _bankService;

        [SetUp]
        public void Setup()
        {
            // 創建 Mock 對象
            _mockDepositRepository = new Mock<IDepositRepository>();
            _mockWithdrawRepository = new Mock<IWithdrawRepository>();
            // 使用 Mock 對象創建被測試的服務
            _bankService = new BankService(_mockDepositRepository.Object, _mockWithdrawRepository.Object);
        }

        [Test]
        public void PerformTransaction_Deposit_Successful()
        {
            // Arrange
            decimal amount = 100m;
            // Stub：設置 Mock 對象的行為
            _mockDepositRepository.Setup(r => r.Deposit(amount)).Returns((true, amount));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Deposit);

            // Assert
            result.Should().Be("Transaction successful deposit $100");
            // Mock：驗證方法是否被調用
            _mockDepositRepository.Verify(r => r.Deposit(amount), Times.Once);
        }

        [Test]
        public void PerformTransaction_Deposit_Failed()
        {
            // Arrange
            decimal amount = 100m;
            // Stub：設置 Mock 對象的行為
            _mockDepositRepository.Setup(r => r.Deposit(amount)).Returns((false, 0m));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Deposit);

            // Assert
            result.Should().Be("Transaction failed");
            // Mock：驗證方法是否被調用
            _mockDepositRepository.Verify(r => r.Deposit(amount), Times.Once);
        }

        [Test]
        public void PerformTransaction_Withdraw_Successful()
        {
            // Arrange
            decimal amount = 50m;
            // Stub：設置 Mock 對象的行為
            _mockWithdrawRepository.Setup(r => r.Withdraw(amount)).Returns((true, amount));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Withdraw);

            // Assert
            result.Should().Be("Transaction successful withdraw $50");
            // Mock：驗證方法是否被調用
            _mockWithdrawRepository.Verify(r => r.Withdraw(amount), Times.Once);
        }

        [Test]
        public void PerformTransaction_Withdraw_Failed()
        {
            // Arrange
            decimal amount = 50m;
            // Stub：設置 Mock 對象的行為
            _mockWithdrawRepository.Setup(r => r.Withdraw(amount)).Returns((false, 0m));

            // Act
            var result = _bankService.PerformTransaction(amount, TransactionType.Withdraw);

            // Assert
            result.Should().Be("Transaction failed");
            // Mock：驗證方法是否被調用
            _mockWithdrawRepository.Verify(r => r.Withdraw(amount), Times.Once);
        }
    }
}