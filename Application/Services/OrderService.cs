using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Clients;
using Application.Common.MessageQ;
using Application.Common.Models.Error;

using Application.Interfaces;
using Domain.Entities;
using Domain.RequestModels;
using Infrastructure.Repository.Interfaces;
using GetAllDto = Infrastructure.Models.Request.GetAllDto;

namespace Application.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerHttpClient _customerHttpClient;
        private readonly IMessageProducer _messagePublisher;
        
        public OrderService(IOrderRepository orderRepository, ICustomerHttpClient customerHttpClient, IMessageProducer messagePublisher)
        {
            _orderRepository = orderRepository;
            _customerHttpClient = customerHttpClient;
            _messagePublisher = messagePublisher;
        }
        public async Task<OrderModel> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            
            if (order == null)
            {
               throw new CustomException(HttpStatusCode.NotFound,"Sipariş bulunamadı.");
            }                   
            if (order.IsDeleted)    
            {
                throw new CustomException(HttpStatusCode.NotFound, "Sipariş bulunamadı.");
            }
            _messagePublisher.SendMessage(id);
            return order;
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            _messagePublisher.SendMessage("getall çalıştı.");
            return  await _orderRepository.GetAllAsync();
            
        }
      
        public async Task<IEnumerable<OrderModel>> GetAllSkipTakeAsync(GetAllDto getAllDto)
        {
            return await _orderRepository.GetAllSkipTakeAsync(getAllDto);
        }

        public async Task<Guid> InsertAsync(OrderModel orderModel)
        {
            var customer = await _customerHttpClient.GetCustomerFromCustomerApi(orderModel.CustomerId);
            // if (customer == null)
            // {
            //     throw new CustomException(HttpStatusCode.NotFound, "Customer bulunamadı. ");
            // }
            orderModel.Address = customer.Address;
            
            _messagePublisher.SendMessage(orderModel);
            
            return await _orderRepository.InsertAsync(orderModel);
        }

        public async Task<OrderModel> Update(Guid guid, UpdateDto updateDto)
        {
            var result = await _orderRepository.Update(guid, updateDto);
            _messagePublisher.SendMessage(result);
            return result;
        }

        public Guid Delete(Guid guid)
        {
            _messagePublisher.SendMessage(guid);
            return _orderRepository.Delete(guid);
        }

        public void SoftDelete(Guid guid,SoftDeleteDto softDeleteDto)
        {
            _messagePublisher.SendMessage(softDeleteDto);
            _orderRepository.SoftDelete(guid,softDeleteDto);
        }
        public StatusDto ChangeStatus(Guid id, StatusDto statusDto)
        {
           _messagePublisher.SendMessage(statusDto);
           return _orderRepository.ChangeStatus(id, statusDto);
        }
        
        public async Task<IEnumerable<OrderModel>> DeleteOrdersByCustomerId(Guid id)
        {
            //var customer = await _customerHttpClient.CheckCustomerId(id);


            var orders = await _orderRepository.DeleteOrdersByCustomerId(id);

            var ordersByCustomerId = orders.ToList();
            foreach (var order in ordersByCustomerId)
            {
                //  Delete(order.Id);
            }

            Console.WriteLine(ordersByCustomerId);
            return ordersByCustomerId;
            
        }
        
        
    }
}