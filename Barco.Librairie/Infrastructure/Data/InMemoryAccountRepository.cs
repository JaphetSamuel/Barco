using Barco.Librairie.Application.Services; // For IAccountRepository
using Barco.Librairie.Domain.AggregateRoots;
using Barco.Librairie.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barco.Librairie.Infrastructure.Data
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = new List<Account>();

        public Task<Account?> GetByIdAsync(AccountId accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id.Equals(accountId));
            return Task.FromResult(account);
        }

        public Task AddAsync(Account account)
        {
            _accounts.Add(account);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Account account)
        {
            // For in-memory, GetByIdAsync returns a reference, so changes are directly reflected.
            // However, if we were cloning objects (e.g., for true immutability or snapshotting),
            // we would need to find and replace/update the item in the list here.
            // For simplicity, and as typical for basic in-memory stores, we assume direct object reference modification.
            var existingAccount = _accounts.FirstOrDefault(a => a.Id.Equals(account.Id));
            if (existingAccount != null)
            {
                // Optional: If Account was a struct or if we wanted to replace the instance
                // _accounts.Remove(existingAccount);
                // _accounts.Add(account);
                // But since Account is a class and we're modifying it in place via AccountService,
                // this explicit UpdateAsync might not even do much unless we add cloning.
                // For now, ensuring it exists is sufficient.
            }
            return Task.CompletedTask;
        }
    }
}
