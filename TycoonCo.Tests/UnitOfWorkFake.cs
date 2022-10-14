using TycoonCo.Application;

namespace TycoonCo.Tests
{
    public class UnitOfWorkFake : IUnitOfWork
    {
        public Task Commit()
        {
            return Task.CompletedTask;
        }
    }
}