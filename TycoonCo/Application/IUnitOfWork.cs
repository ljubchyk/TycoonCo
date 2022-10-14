namespace TycoonCo.Application
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}