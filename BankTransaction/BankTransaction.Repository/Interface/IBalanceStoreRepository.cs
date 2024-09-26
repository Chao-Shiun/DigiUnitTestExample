namespace BankTransaction.Repository.Interface;

public interface IBalanceStoreRepository
{
    decimal GetBalance();
    void UpdateBalance(decimal amount);
}