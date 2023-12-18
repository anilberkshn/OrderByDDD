using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.RequestModels;
using Infrastructure.Models.Request;

namespace Infrastructure.Repository.Interfaces
{
   public interface IOrderRepository
    {
        public Task<OrderModel> GetByIdAsync(Guid id);
        public Task<IEnumerable<OrderModel>> GetAllAsync();
        public Task<IEnumerable<OrderModel>> GetAllSkipTakeAsync(GetAllDto getAllDto);
        public Task<Guid> InsertAsync(OrderModel orderModel);
        public Task<OrderModel> Update(Guid id, OrderModel updateDto);
        public Guid Delete(Guid id);
        public void SoftDelete(Guid id, SoftDeleteDto softDeleteDto);
        public StatusDto ChangeStatus(Guid id, StatusDto statusDto); 
        public Task<IEnumerable<OrderModel>> DeleteOrdersByCustomerId(Guid id);
    }
}