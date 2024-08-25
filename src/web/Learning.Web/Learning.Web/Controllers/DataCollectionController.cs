using Learning.Business.Requests.DataCollection.ContactUsRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Web.Controllers;

[Route("api/data-collection")]
public class DataCollectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public DataCollectionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add-contact-info")]
    public async Task<IActionResult> AddContactInformation([FromBody] AddContactInformationCommand request)
    {
        var data = await _mediator.Send(request);
        return Ok(data);
    }

}
