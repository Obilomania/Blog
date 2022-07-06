using Blog.Models;

namespace Blog.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        void Update(Post post);
        void Save();
    }
}
