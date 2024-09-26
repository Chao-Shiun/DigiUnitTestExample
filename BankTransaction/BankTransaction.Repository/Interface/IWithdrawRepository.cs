namespace BankTransaction.Repository.Interface;

public interface IWithdrawRepository
{
    (bool,decimal) Withdraw(decimal amount);
}