using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface IUnitOfWork
    {
        ICityRepository CityRepository { get; }

        Task<bool> SaveAsync();
    }
}
