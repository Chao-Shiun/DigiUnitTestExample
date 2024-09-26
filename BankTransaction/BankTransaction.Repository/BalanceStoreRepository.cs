using BankTransaction.Repository.Interface;

namespace BankTransaction.Repository;

public class BalanceStoreRepository : IBalanceStoreRepository
{
    public decimal GetBalance()
    {
        // 從DB取得資料
        return 0;
    }

    public void UpdateBalance(decimal amount)
    {
        // 計算並更新金額到DB
    }
}