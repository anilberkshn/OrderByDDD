using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.GenericDtos;
using Infrastructure.Repository.Interfaces;
using MongoDB.Driver;


namespace Infrastructure.Repository
{
    public class OrderRepository: GenericRepository<OrderModel>, IOrderRepository
    {
        public OrderRepository(IContext context, string collectionName = "Order ") : base(context, collectionName)
        {
        }

        public async Task<OrderModel> GetByIdAsync(Guid id)
        {
            var order = await FindOneAsync(x => x.Id == id);
            return order;
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return await GenericRepositoryGetAllAsync();
        }
        public async Task<IEnumerable<OrderModel>> GetAllSkipTakeAsync(GetAllDto getAllDto)
        {
            return await GetManyAsync(getAllDto);
        }

        public async Task<Guid> InsertAsync(OrderModel orderModel)
        {
            return await CreateAsync(orderModel);
        }

        public async Task<OrderModel> Update(Guid id, OrderModel orderModel)
        {
            var update = Builders<OrderModel>.Update
                // .Set(x=>  x.Id, orderModel.Id)
                .Set(x => x.Quantity, orderModel.Quantity)
                .Set(x => x.Price, orderModel.Price)
                .Set(x => x.Status, orderModel.Status)
                .Set(x => x.Product, orderModel.Product)
                // .Set(x => x.Address, orderModel.Address)
                ;

            Update(x => x.Id == id, update);
            return await GetByIdAsync(id);
        }

        public Guid Delete(Guid id)
        {
          return  Delete(x => x.Id == id);
        }

        public void SoftDelete(Guid id, OrderModel orderModel)
        {
            var softDelete  = Builders<OrderModel>.Update
                    .Set(x => x.DeletedTime, orderModel.DeletedTime)
                    .Set(x => x.IsDeleted, orderModel.IsDeleted)
                ;

            SoftDelete(x => x.Id == id, softDelete);
        }
        
        public OrderModel ChangeStatus(Guid id, OrderModel orderModel)
        {
            var status  = Builders<OrderModel>.Update
                    .Set(x => x.Status, orderModel.Status)
                    ;

            Update(x => x.Id == id, status);
            return orderModel;
        }

        public async Task<IEnumerable<OrderModel>> DeleteOrdersByCustomerId(Guid id)
        {
            return  await GetByCustomerId(id);  ;
        }
    }
}