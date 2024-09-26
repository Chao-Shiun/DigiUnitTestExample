using BankTransaction.Infrastructure.Enum;
using BankTransaction.Repository.Interface;

namespace BankTransaction.Service;

public class BankService
{
    private readonly IDepositRepository _depositRepository;
    private readonly IWithdrawRepository _withdrawRepository;

    public BankService(IDepositRepository depositRepository, IWithdrawRepository withdrawRepository)
    {
        _depositRepository = depositRepository;
        _withdrawRepository = withdrawRepository;
    }


    public string PerformTransaction(decimal amount, TransactionType transactionType)
    {
        bool isSuccess = false;
        string? transactionMsg = null;
        switch (transactionType)
        {
            case TransactionType.Deposit:
                (isSuccess, decimal depositAmount) = _depositRepository.Deposit(amount);
                transactionMsg = $"deposit ${depositAmount}";
                break;
            case TransactionType.Withdraw:
                (isSuccess, decimal withdrawAmount) = _withdrawRepository.Withdraw(amount);
                transactionMsg = $"withdraw ${withdrawAmount}"; // 將 "deposit" 改為 "withdraw"
                break;
        }


        return isSuccess ? $"Transaction successful {transactionMsg}" : "Transaction failed";
    }
}