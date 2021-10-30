using System.Threading.Tasks;

namespace PostWebAPI.Repositories
{
    public interface IPagination
    {
        Task<PostResponse> GetPosts(int pageSize, string continuationToken);
    }
}

