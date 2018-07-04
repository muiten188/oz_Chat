namespace CRMOZ.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}