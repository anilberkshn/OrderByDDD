using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Clients;
using Application.Common.MessageQ;
using Application.Common.Models.Dto;
using Application.Common.Models.Error;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repository.Interfaces;
using GetAllDto = Infrastructure.Models.GenericDtos.GetAllDto;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerHttpClient _customerHttpClient;
        private readonly IMessageProducer _messagePublisher;

        public OrderService(IOrderRepository orderRepository, ICustomerHttpClient customerHttpClient,
            IMessageProducer messagePublisher)
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
                throw new CustomException(HttpStatusCode.NotFound, "The order could not be found.");
            }

            if (order.IsDeleted)
            {
                throw new CustomException(HttpStatusCode.NotFound, "The order could not be found.");
            }

            _messagePublisher.SendMessage(id);
            return order;
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            _messagePublisher.SendMessage("GetAll worked.");
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<OrderModel>> GetAllSkipTakeAsync(GetAllDto getAllDto)
        {
            return await _orderRepository.GetAllSkipTakeAsync(getAllDto);
        }

        public async Task<Guid> InsertAsync(CreateOrderDto createOrderDto)
        {
            var customer = await _customerHttpClient.GetCustomerFromCustomerApi(createOrderDto.CustomerId);
            var orderModel = new OrderModel()
            {
                Id = createOrderDto.OrderId,
                CustomerId = createOrderDto.CustomerId,
                Quantity = createOrderDto.Quantity,
                Price = createOrderDto.Price,
                Product = createOrderDto.Product,
            };
            // if (customer == null)
            // {
            //     throw new CustomException(HttpStatusCode.NotFound, "Customer bulunamadı. ");
            // }
            createOrderDto.Address = customer.Address;

            _messagePublisher.SendMessage(orderModel);

            return await _orderRepository.InsertAsync(orderModel);
        }

        public async Task<OrderModel> Update(Guid guid, UpdateOrderDto updateOrderDto)
        {
            var orderModel = new OrderModel()
            {
                Quantity = updateOrderDto.Quantity,
                Price = updateOrderDto.Price,
                Status = updateOrderDto.Status,
                Product = updateOrderDto.Product
            };

            var result = await _orderRepository.Update(guid, orderModel);
            _messagePublisher.SendMessage(result);
            return result;
        }

        public Guid Delete(Guid guid)
        {
            _messagePublisher.SendMessage(guid);
            return _orderRepository.Delete(guid);
        }

        public void SoftDelete(Guid guid, SoftDeleteOrderDto softDeleteOrderDto)
        {
            var orderModel = new OrderModel()
            {
                IsDeleted = softDeleteOrderDto.IsDeleted,
                DeletedTime = softDeleteOrderDto.DeletedTime
            };
            //_messagePublisher.SendMessage(softDeleteOrderDto);
            _orderRepository.SoftDelete(guid, orderModel);
        }

        public StatusOrderDto ChangeStatus(Guid id, StatusOrderDto statusOrderDto)
        {
            var orderModel = new OrderModel()
            {
                Status = statusOrderDto.Status
            };
            
            _messagePublisher.SendMessage(statusOrderDto);
            _orderRepository.ChangeStatus(id, orderModel);
            return statusOrderDto;
        }

        public async Task<IEnumerable<OrderModel>> DeleteOrdersByCustomerId(Guid id)
        {
            //var customer = await _customerHttpClient.CheckCustomerId(id);
            var orders = await _orderRepository.DeleteOrdersByCustomerId(id);

            var ordersByCustomerId = orders.ToList();
            foreach (var order in ordersByCustomerId)
            {
                //  Delete(order.OrderId);
            }

            Console.WriteLine(ordersByCustomerId);
            return ordersByCustomerId;
        }
    }
}