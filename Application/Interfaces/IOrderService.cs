using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;
using Domain.RequestModels;
using Infrastructure.Models.Request;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderModel> GetByIdAsync(Guid id);
        public Task<IEnumerable<OrderModel>> GetAllAsync();
        public Task<IEnumerable<OrderModel>> GetAllSkipTakeAsync(GetAllDto getAllDto);
        public Task<Guid> InsertAsync(OrderModel orderModel);
        public Task<OrderModel> Update(Guid guid, UpdateDto updateDto);
        public Guid Delete(Guid guid);
        public void SoftDelete(Guid guid, SoftDeleteDto softDeleteDto);
        public StatusDto ChangeStatus(Guid id, StatusDto statusDto);
        public Task<IEnumerable<OrderModel>> DeleteOrdersByCustomerId(Guid id);
    }
}