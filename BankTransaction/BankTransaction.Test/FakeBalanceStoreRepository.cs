using BankTransaction.Repository.Interface;

namespace BankTransaction.Test
{
    public class FakeBalanceStoreRepository : IBalanceStoreRepository
    {
        private readonly List<decimal> _transactions = [];

        public decimal GetBalance()
        {
            return _transactions.Sum();
        }

        public void UpdateBalance(decimal amount)
        {
            _transactions.Add(amount);
        }
    }
}