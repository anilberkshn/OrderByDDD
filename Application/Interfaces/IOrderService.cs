using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Models.Dto;
using Application.Common.Models.Request;
using Domain.Entities;
using Infrastructure.Models.GenericDtos;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderModel> GetByIdAsync(Guid id);
        public Task<IEnumerable<OrderModel>> GetAllAsync();
        public Task<IEnumerable<OrderModel>> GetAllSkipTakeAsync(GetAllDto getAllDto); // Todo:  Parametre
        public Task<Guid> InsertAsync(CreateOrderDto createOrderDto);
        public Task<OrderModel> Update(Guid guid, UpdateOrderDto updateOrderDto);
        public Guid Delete(Guid guid);
        public void SoftDelete(Guid guid, SoftDeleteOrderDto softDeleteOrderDto);
        public StatusOrderDto ChangeStatus(Guid id, StatusOrderDto statusOrderDto);
        public Task<IEnumerable<OrderModel>> DeleteOrdersByCustomerId(Guid id);
    }
}