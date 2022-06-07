using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Model;

namespace OrderPublisher.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IBus _iBus;

        public OrderController(IBus iBus)
        {
            _iBus = iBus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            if (ticket == null) return BadRequest();

            Uri uri = new Uri("rabbitmq://localhost/orderTicketQueue");
            var endPoint = await _iBus.GetSendEndpoint(uri);

            ticket.Booked = DateTime.Now;
            await endPoint.Send(ticket);

            return Ok();
        }
    }
}
