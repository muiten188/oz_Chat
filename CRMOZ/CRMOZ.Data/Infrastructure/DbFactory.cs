namespace CRMOZ.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private OZChatDbContext dbContext;

        public OZChatDbContext Init()
        {
            return dbContext ?? (dbContext = new OZChatDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}