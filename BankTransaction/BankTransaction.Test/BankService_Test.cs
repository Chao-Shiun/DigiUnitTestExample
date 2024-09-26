using BankTransaction.Repository;
using BankTransaction.Service;
using BankTransaction.Repository.Interface;
using FluentAssertions;
using Moq;

namespace BankTransaction.Test
{
    [TestFixture]
    public class BankService_Test
    {
        private Mock<IBalanceStoreRepository> _mockBalanceStore;
        private Mock<IBalanceStoreRepository> _mockWithdrawStore;
        private BankService _bankService;

        [SetUp]
        public void Setup()
        {
            _mockBalanceStore = new Mock<IBalanceStoreRepository>();
            _mockWithdrawStore = new Mock<IBalanceStoreRepository>();
            _bankService = new BankService(_mockBalanceStore.Object, _mockWithdrawStore.Object);
        }

        [Test]
        public void MultipleTransactions_CorrectBalance()
        {
            // Arrange
            var initialBalance = _bankService.GetBalance();

            // Act
            _bankService.PerformTransaction(100);
            _bankService.PerformTransaction(50);
            _bankService.PerformTransaction(-30);
            _bankService.PerformTransaction(200);
            _bankService.PerformTransaction(-100);

            var finalBalance = _bankService.GetBalance();

            // Assert
            initialBalance.Should().Be(0);
            finalBalance.Should().Be(220);
        }

        [Test]
        public void Deposit_SuccessfulTransaction()
        {
            // Act
            var result = _bankService.PerformTransaction(100);

            // Assert
            result.Should().Be("交易成功：存款 $100");
            _bankService.GetBalance().Should().Be(100);
        }

        [Test]
        public void Withdraw_SuccessfulTransaction()
        {
            // Arrange
            _bankService.PerformTransaction(200);

            // Act
            var result = _bankService.PerformTransaction(-50);

            // Assert
            result.Should().Be("交易成功：取款 $50");
            _bankService.GetBalance().Should().Be(150);
        }

        [Test]
        public void Withdraw_InsufficientFunds()
        {
            // Arrange
            _bankService.PerformTransaction(100);

            // Act
            var result = _bankService.PerformTransaction(-150);

            // Assert
            result.Should().Be("交易失敗：餘額不足");
            _bankService.GetBalance().Should().Be(100);
        }

        [Test]
        public void ZeroAmountTransaction_NoEffect()
        {
            // Act
            var result = _bankService.PerformTransaction(0);

            // Assert
            result.Should().Be("交易成功：存款 $0");
            _bankService.GetBalance().Should().Be(0);
        }
    }
}