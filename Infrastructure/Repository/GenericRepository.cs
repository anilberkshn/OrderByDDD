using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Common;
using Infrastructure.Models.Request;
using Infrastructure.Repository.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : GenericDocument
    {
        private readonly IMongoCollection<T> _collection;

        public GenericRepository(IContext context, string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(T).Name;
            }

            _collection = context.DbMongoCollectionSet<T>(collectionName);
        }

        public async Task<Guid> CreateAsync(T record)
        {
            record.Id = Guid.NewGuid();
            record.CreatedTime = DateTime.Now;
            record.UpdatedTime = DateTime.Now;

            await _collection.InsertOneAsync(record);
            return record.Id;
        }

        public async Task<IEnumerable<T>> GenericRepositoryGetAllAsync()
        {
            var record = await _collection.AsQueryable().ToListAsync();
            return record;
        }

        public async Task<IEnumerable<T>> FindAllSkipTakeAsync(GetAllDto getAllDto)
        {
            var record = _collection.AsQueryable().Skip(getAllDto.Skip).Take(getAllDto.Take);
            return await Task.Run(() => record.ToList());
        }
        public async Task<IEnumerable<T>> GetManyAsync(GetAllDto getAllDto)
        {
           var filter = Builders<T>.Filter.Eq(x => x.IsDeleted, false);
           var result = await _collection
               .Find(filter)
               .Skip(getAllDto.Skip)
               .Limit(getAllDto.Take)
               .ToListAsync();
            return result;
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> expression)
        {
            var record = await _collection.Find(expression).FirstOrDefaultAsync();
            return record;
        }

        public void Update(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition)
        {
            var filter = Builders<T>.Filter.Where(expression);
            var update = updateDefinition.Set(x => x.UpdatedTime, DateTime.Now);
            _collection.FindOneAndUpdate<T>(filter, update);
        }

        public Guid Delete(Expression<Func<T, bool>> expression)
        {
            var record = _collection.FindOneAndDelete(expression);
            return record.Id;
        }

        public void SoftDelete(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition)
        {
            var filter = Builders<T>.Filter.Where(expression);

            var update = updateDefinition.Set(x => x.DeletedTime, DateTime.Now)
                .Set(x => x.IsDeleted, true);
            _collection.FindOneAndUpdate<T>(filter, update);
        }
        
        public async Task<IEnumerable<T>> GetByCustomerId(Guid customerId) // GetOrderByCustomerId  name chance
        {
            var filter = Builders<T>.Filter.Eq("CustomerId",customerId);
            var result = await _collection
                .Find(filter)
                .ToListAsync();
            return result;
        }
    }
}