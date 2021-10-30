using BandLab_Social.Entities;
using System.Threading.Tasks;

namespace BandLab_Social.Infrastructure.Repositories
{
    public interface IPagination
    {
        Task<PostPagination> GetPosts(int pageSize, string continuationToken);
    }
}

