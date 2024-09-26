namespace BankTransaction.Repository.Interface;

public interface IDepositRepository
{
    (bool, decimal) Deposit(decimal amount);
}