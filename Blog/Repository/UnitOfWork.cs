using Blog.Data;
using Blog.Repository.IRepository;

namespace Blog.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Post = new PostRepository(_db);
        }

        public IPostRepository Post { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
