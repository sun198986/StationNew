using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Station.Core;
using Station.EFCore.IbmDb;
using Station.RepositoryPattern;

namespace Station.Repository.Sap_Mara
{
    [ServiceDescriptor(typeof(ISap_MaraRepository), ServiceLifetime.Transient)]
    public class Sap_MaraRepository : RepositoryBase<Entity.DB2Admin.Sap_Mara>, ISap_MaraRepository
    {
        private readonly IbmDbContext _context;
        private readonly IApplicationContext _applicationContext;

        public Sap_MaraRepository (IbmDbContext context, IApplicationContext applicationContext) : base(context, applicationContext)
        { 
            _context = context;
            _applicationContext = applicationContext;
        }
    }
}
