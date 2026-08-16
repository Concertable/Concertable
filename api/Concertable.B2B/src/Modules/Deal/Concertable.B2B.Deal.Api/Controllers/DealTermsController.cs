using Concertable.B2B.Deal.Application.Interfaces;
using Concertable.B2B.Deal.Contracts;
using Reunion.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Concertable.B2B.Deal.Api.Controllers;

[ApiController]
[Route("api/Deal")]
internal sealed class DealTermsController : ControllerBase
{
    private readonly IDealTermsService dealTermsService;

    public DealTermsController(IDealTermsService dealTermsService)
    {
        this.dealTermsService = dealTermsService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IDealTerms>> GetById(int id) =>
        (await dealTermsService.GetByIdAsync(id)).ToActionResult(value => new ActionResult<IDealTerms>(value));
}
