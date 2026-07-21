using webapi.Model;
using webapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class ItemsController(IItemService service) : ControllerBase
{

    /// <summary>
    /// Henter alle oppgaver.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Item>>> Get()
    {
        var items = await service.GetAllAsync();

        return Ok(items);
    }

}