using BankTransaction.Repository.Interface;

namespace BankTransaction.Repository;

public class WithdrawRepository : IWithdrawRepository
{
    public (bool, decimal) Withdraw(decimal amount)
    {
        /*
         * 讀寫DB
         */
        return (true, amount);
    }
}