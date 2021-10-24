using Domain;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CosmosWriteRepository<TEntity> : ICosmosWriteRepository<TEntity> where TEntity : class, new()
    {

        protected readonly CosmosDbContext _cosmosDbContext;

        public CosmosWriteRepository(CosmosDbContext cosmosDbContext)
        {
            _cosmosDbContext = cosmosDbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            await _cosmosDbContext.AddAsync(entity);
            await _cosmosDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            _cosmosDbContext.Update(entity);
            await _cosmosDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            _cosmosDbContext.Remove(entity);
            await _cosmosDbContext.SaveChangesAsync();
        }
    }
}
