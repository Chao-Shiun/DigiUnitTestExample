using BankTransaction.Repository.Interface;
using BankTransaction.Service;
using FluentAssertions;
using Moq;

namespace BankTransaction.Test
{
    [TestFixture]
    public class BankService_Test
    {
        private FakeBalanceStoreRepository _fakeBalanceStoreRepository;
        private Mock<IBalanceStoreRepository> _mockBalanceStoreRepository;
        private Mock<INotifyRepository> _mockNotifyRepository;
        private BankService _bankService;

        [SetUp]
        public void Setup()
        {
            _fakeBalanceStoreRepository = new FakeBalanceStoreRepository();
            _mockBalanceStoreRepository = new Mock<IBalanceStoreRepository>();
            _mockNotifyRepository = new Mock<INotifyRepository>();
            _bankService = new BankService(_mockBalanceStoreRepository.Object, _mockNotifyRepository.Object);
        }

        [Test]
        public void MultipleTransactions_CorrectBalance()
        {
            // Arrange
            decimal balance = 0;
            _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(() => balance);
            _mockBalanceStoreRepository.Setup(m => m.UpdateBalance(It.IsAny<decimal>()))
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
            _mockNotifyRepository.Verify(m => m.SendMessage(It.IsAny<string>()), Times.Exactly(5));
        }
        
        [Test]
        public void MultipleTransactions_CorrectBalance_Another_Solution()
        {
            // Arrange
            _bankService = new BankService(_fakeBalanceStoreRepository, _mockNotifyRepository.Object);

            // Act
            _bankService.PerformTransaction(100);
            _bankService.PerformTransaction(50);
            _bankService.PerformTransaction(-30);
            _bankService.PerformTransaction(200);
            _bankService.PerformTransaction(-100);

            var finalBalance = _bankService.GetBalance();

            // Assert
            finalBalance.Should().Be(220);
            _mockNotifyRepository.Verify(m => m.SendMessage(It.IsAny<string>()), Times.Exactly(5));
        }

        [Test]
        public void Deposit_SuccessfulTransaction()
        {
            // Arrange
            decimal balance = 0;
            _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(() => balance);
            _mockBalanceStoreRepository.Setup(m => m.UpdateBalance(It.IsAny<decimal>()))
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
            _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(200);
            _mockBalanceStoreRepository.Setup(m => m.UpdateBalance(It.IsAny<decimal>()))
                .Callback<decimal>(amount => _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(200 + amount));

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
            _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(100);

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
            _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(0);

            // Act
            var result = _bankService.PerformTransaction(0);

            // Assert
            result.Should().Be("交易成功：存款 $0");
            _bankService.GetBalance().Should().Be(0);
            _mockBalanceStoreRepository.Verify(m => m.UpdateBalance(0), Times.Once);
        }

        [Test]
        public void NotifyRepository_CalledOnTransaction()
        {
            // Arrange
            decimal balance = 0;
            _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(() => balance);
            _mockBalanceStoreRepository.Setup(m => m.UpdateBalance(It.IsAny<decimal>()))
                .Callback<decimal>(amount => balance += amount);

            // Act
            _bankService.PerformTransaction(100);

            // Assert
            _mockNotifyRepository.Verify(m => m.SendMessage("交易成功：存款 $100"), Times.Once);
        }

        [Test]
        public void NotifyRepository_NotCalledOnFailedTransaction()
        {
            // Arrange
            _mockBalanceStoreRepository.Setup(m => m.GetBalance()).Returns(50);

            // Act
            var result = _bankService.PerformTransaction(-100);

            // Assert
            result.Should().Be("交易失敗：餘額不足");
            _mockNotifyRepository.Verify(m => m.SendMessage(It.IsAny<string>()), Times.Never);
        }
    }
}