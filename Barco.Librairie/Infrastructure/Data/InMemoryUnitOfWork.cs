using Barco.Librairie.Application.Services; // For IUnitOfWork
using System.Threading.Tasks;

namespace Barco.Librairie.Infrastructure.Data
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync()
        {
            // In a real scenario, this would commit the transaction.
            // For an in-memory store where changes are often immediate or managed by the repository itself,
            // this might not do much other than signify completion.
            // Returning 1 to simulate one "unit" of work being saved.
            return Task.FromResult(1);
        }
    }
}
