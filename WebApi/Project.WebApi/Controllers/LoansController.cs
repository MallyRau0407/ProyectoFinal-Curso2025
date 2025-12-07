using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.UseCases.Loans.Create;
using Project.Application.UseCases.Loans.Return;
using Project.Application.UseCases.Loans.GetAll;
using Project.Application.UseCases.Loans.GetByUser;

namespace Project.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoansController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
public IActionResult Create([FromBody] CreateLoanRequest request)
{
    var loan = new
    {
        Id = 1,
        UserId = request.UserId,
        ResourceId = request.ResourceId,
        LoanDate = DateTime.UtcNow
    };

    return Ok(loan);
}
public class CreateLoanRequest
{
    public int UserId { get; set; }
    public int ResourceId { get; set; }
}

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetAllLoansQuery(page, pageSize));
            return Ok(result);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUser(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new GetLoansByUserQuery(userId, page, pageSize));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            // Opcional: implementar GetById query/handler si lo deseas
            return Ok();
        }

    }
}
