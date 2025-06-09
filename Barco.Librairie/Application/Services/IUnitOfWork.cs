using System.Threading.Tasks;

namespace Barco.Librairie.Application.Services
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
