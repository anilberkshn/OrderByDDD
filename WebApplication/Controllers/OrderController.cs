using System;
using System.Threading.Tasks;
using Application.Common.MessageQ;
using Application.Interfaces;
using Domain.Entities;
using Domain.RequestModels;
using Domain.ResponseModels;
using Infrastructure.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController] 
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        

        public OrderController(IOrderService orderService,IMessageProducer messagePublisher)
        {
            _orderService = orderService;
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDto createDto)
        {
            
            // IMapper kullanımı ile yapımı 
            var order = new OrderModel()
            {
                CustomerId = createDto.CustomerId,
                Quantity = createDto.Quantity,
                Price = createDto.Price,
                Product = createDto.Product
            };

            await _orderService.InsertAsync(order);

            var response = new CreateResponse()
            {
                Id = order.Id
            };
            
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var findOne = await _orderService.GetByIdAsync(id);
            return Ok(findOne);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAsync()
        {
            var getAll = await _orderService.GetAllAsync();
            return Ok(getAll);
        }

        [HttpGet("AllWithSkipTake")]
        public async Task<IActionResult> GetAllSkipTakeAsync([FromQuery] GetAllDto getAllDto)
        {
            var getAll = await _orderService.GetAllSkipTakeAsync(getAllDto);
            return Ok(getAll);
        }


        [HttpDelete("HardDelete")]
        public async Task<IActionResult> HardDeleteAsync(Guid id)
        {
            var byId = await _orderService.GetByIdAsync(id);
            _orderService.Delete(byId.Id);
            
            return Ok(id);  // geriye boş sınıf 
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateDto updateDto)
        {
            var result = await _orderService.Update(id, updateDto);
            return Ok(result);
        }

        [HttpPut("SoftDelete")]
        public async Task<IActionResult> SoftDeleteAsync(Guid id, [FromBody] SoftDeleteDto softDeleteDto)
        {
            var order = await _orderService.GetByIdAsync(id);
            _orderService.SoftDelete(order.Id, softDeleteDto);
            return Ok(id);
        }
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] StatusDto statusDto)
        {
            var order = await _orderService.GetByIdAsync(id);
            var orderResult=  _orderService.ChangeStatus(order.Id, statusDto);
           return Ok(id);
        }
        
        [HttpPost("Customer/{customerId}")]
        public async Task<IActionResult> OrdersByCustomerId(Guid customerId)
        {
            var findList = await _orderService.DeleteOrdersByCustomerId(customerId);
            return Ok(findList);
        }
    }
}