using BankTransaction.Repository.Interface;

namespace BankTransaction.Service;

public class BankService
{
    private readonly IBalanceStoreRepository _balanceStoreRepository;
    private readonly INotifyRepository _notifyRepository;

    public BankService(IBalanceStoreRepository balanceStoreRepository, INotifyRepository notifyRepository)
    {
        _balanceStoreRepository = balanceStoreRepository;
        _notifyRepository = notifyRepository;
    }

    public string PerformTransaction(decimal amount)
    {
        decimal currentBalance = _balanceStoreRepository.GetBalance();

        if (amount < 0)
        {
            if (Math.Abs(amount) > currentBalance)
            {
                return "交易失敗：餘額不足";
            }

            _balanceStoreRepository.UpdateBalance(amount);
            string msg = $"交易成功：取款 ${Math.Abs(amount)}";
            _notifyRepository.SendMessage(msg);
            return msg;
        }
        else
        {
            _balanceStoreRepository.UpdateBalance(amount);
            string msg = $"交易成功：存款 ${amount}";
            _notifyRepository.SendMessage(msg);
            return msg;
        }
    }

    public decimal GetBalance()
    {
        return _balanceStoreRepository.GetBalance();
    }
}