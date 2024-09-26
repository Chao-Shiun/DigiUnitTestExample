using BankTransaction.Repository.Interface;
using BankTransaction.Service;
using FluentAssertions;
using Moq;

namespace BankTransaction.Test
{
    [TestFixture]
    public class BankService_Test
    {
        private Mock<IBalanceStoreRepository> _mockBalanceStore;
        private BankService _bankService;

        [SetUp]
        public void Setup()
        {
            _mockBalanceStore = new Mock<IBalanceStoreRepository>();
            _bankService = new BankService(_mockBalanceStore.Object);
        }

        [Test]
        public void MultipleTransactions_CorrectBalance()
        {
            // Arrange
            decimal balance = 0;
            _mockBalanceStore.Setup(m => m.GetBalance()).Returns(() => balance);
            _mockBalanceStore.Setup(m => m.UpdateBalance(It.IsAny<decimal>()))
                .Callback<decimal>(amount => balance += amount);

            // Act
            _bankService.PerformTransaction(100);
            _bankService.PerformTransaction(50);
            _bankService.PerformTransaction(-30);
            _bankService.PerformTransaction(200);
            _bankService.PerformTransaction(-100);

            var finalBalance = _bankService.GetBalance();

            // Assert
            finalBalance.Should().Be(220);
        }

        [Test]
        public void Deposit_SuccessfulTransaction()
        {
            // Arrange
            decimal balance = 0;
            _mockBalanceStore.Setup(m => m.GetBalance()).Returns(() => balance);
            _mockBalanceStore.Setup(m => m.UpdateBalance(It.IsAny<decimal>()))
                .Callback<decimal>(amount => balance += amount);

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
            _mockBalanceStore.Setup(m => m.GetBalance()).Returns(200);
            _mockBalanceStore.Setup(m => m.UpdateBalance(It.IsAny<decimal>()))
                .Callback<decimal>(amount => _mockBalanceStore.Setup(m => m.GetBalance()).Returns(200 + amount));

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
            _mockBalanceStore.Setup(m => m.GetBalance()).Returns(100);

            // Act
            var result = _bankService.PerformTransaction(-150);

            // Assert
            result.Should().Be("交易失敗：餘額不足");
            _bankService.GetBalance().Should().Be(100);
        }

        [Test]
        public void ZeroAmountTransaction_NoEffect()
        {
            // Arrange
            _mockBalanceStore.Setup(m => m.GetBalance()).Returns(0);

            // Act
            var result = _bankService.PerformTransaction(0);

            // Assert
            result.Should().Be("交易成功：存款 $0");
            _bankService.GetBalance().Should().Be(0);
            _mockBalanceStore.Verify(m => m.UpdateBalance(0), Times.Once);
        }
    }
}