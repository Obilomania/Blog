using Blog.Data;
using Blog.Models;
using Blog.Repository.IRepository;

namespace Blog.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private ApplicationDbContext _db;
        public PostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Post post)
        {
            _db.Update(post);
        }
    }
}
