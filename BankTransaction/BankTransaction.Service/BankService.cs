using BankTransaction.Repository.Interface;

namespace BankTransaction.Service;

public class BankService
{
    private readonly IBalanceStoreRepository balanceStoreRepository;

    public BankService(IBalanceStoreRepository balanceStore, IBalanceStoreRepository balanceStoreRepository)
    {
        this.balanceStoreRepository = balanceStoreRepository;
    }

    public string PerformTransaction(decimal amount)
    {
        decimal currentBalance = balanceStoreRepository.GetBalance();
        
        if (amount < 0 && Math.Abs(amount) > currentBalance)
        {
            return "交易失敗：餘額不足";
        }

        balanceStoreRepository.UpdateBalance(amount);
        string transactionType = amount >= 0 ? "存款" : "取款";
        return $"交易成功：{transactionType} ${Math.Abs(amount)}";
    }

    public decimal GetBalance()
    {
        return balanceStoreRepository.GetBalance();
    }
}