using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Orders;

[ApiController]
[Route("/api/order")]
public class OrderController : ControllerCustom
{
    private readonly ILogger<OrderController> _logger;
    private readonly OrderRepository _orderRepository;

    public OrderController(AuthenticationService authenticationService, OrderRepository orderRepository,
        ILogger<OrderController> logger) : base(authenticationService)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }


    #region DELETE

    [HttpDelete]
    [Route("delete/by-id", Name = nameof(DeleteOrderById))]
    public async Task<ActionResult> DeleteOrderById(Guid orderId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            await _orderRepository.DeleteById(orderId, user.Id);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting order by id {OrderId}", orderId);
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}