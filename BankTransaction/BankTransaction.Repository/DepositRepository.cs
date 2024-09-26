using BankTransaction.Repository.Interface;

namespace BankTransaction.Repository;

public class DepositRepository : IDepositRepository
{
    public (bool, decimal) Deposit(decimal amount)
    {
        /*
         * 讀寫DB
         */
        return (true, amount);
    }
}