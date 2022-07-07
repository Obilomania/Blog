namespace Blog.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPostRepository Post { get; }
        void Save();
    }
}
