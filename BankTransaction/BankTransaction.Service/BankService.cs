using BankTransaction.Repository.Interface;

namespace BankTransaction.Service;

public class BankService
{
    private readonly IBalanceStoreRepository _balanceStoreRepository;

    public BankService(IBalanceStoreRepository balanceStoreRepository)
    {
        _balanceStoreRepository = balanceStoreRepository;
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
            return $"交易成功：取款 ${Math.Abs(amount)}";
        }
        else
        {
            _balanceStoreRepository.UpdateBalance(amount);
            return $"交易成功：存款 ${amount}";
        }
    }

    public decimal GetBalance()
    {
        return _balanceStoreRepository.GetBalance();
    }
}