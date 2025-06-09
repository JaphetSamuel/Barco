using Barco.Librairie.Domain.AggregateRoots;
using Barco.Librairie.Domain.ValueObjects;
using System.Threading.Tasks;

namespace Barco.Librairie.Application.Services
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(AccountId accountId);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account); // Needed for persisting changes after deposit/withdrawal
    }
}
