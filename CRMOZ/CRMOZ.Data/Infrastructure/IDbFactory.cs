using System;

namespace CRMOZ.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        OZChatDbContext Init();
    }
}