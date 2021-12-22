using DAL.DBContext;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CosmosReadRepository<TEntity> : ICosmosReadRepository<TEntity> where TEntity : class, new()
    {

        protected readonly CosmosDbContext _cosmosDbContext;

        public CosmosReadRepository(CosmosDbContext cosmosDbContext)
        {
            _cosmosDbContext = cosmosDbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            var result = _cosmosDbContext.Set<TEntity>();

            if (result == null)
            {
                throw new Exception($"Couldn't retrieve entities of type: {typeof(TEntity)}.");
            }

            return result;
        }
    }
}
