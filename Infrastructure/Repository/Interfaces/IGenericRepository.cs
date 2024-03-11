using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Models.GenericDtos;
using MongoDB.Driver;

namespace Infrastructure.Repository.Interfaces
{
    public interface IGenericRepository<T>
    {
        public Task<Guid> CreateAsync (T record);
        public Task<IEnumerable<T>> GenericRepositoryGetAllAsync();
        public Task<IEnumerable<T>> FindAllSkipTakeAsync(GetAllDto getAllDto);
        public Task<IEnumerable<T>> GetManyAsync(GetAllDto getAllDto);
        public  Task<T> FindOneAsync(Expression<Func<T, bool>> expression);
        public void Update(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition);
        public Guid Delete(Expression<Func<T, bool>> expression);
        public void SoftDelete(Expression<Func<T, bool>> expression,UpdateDefinition<T> updateDefinition);

        public Task<IEnumerable<T>> GetByCustomerId(Guid customerId);
    }
}