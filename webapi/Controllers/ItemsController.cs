using webapi.Model;
using webapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController(IItemService service) : ControllerBase
{

    /// <summary>
    /// Henter alle elementer, med mulighet for filtrering på status, kategori og søk.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Item>>> Get(
        ItemStatus? status,
        string? category,
        string? q)
    {
        var items = await service.GetAllAsync(
            status,
            category,
            q
        );

        return Ok(items);
    }

    /// <summary>
    /// Henter ett element basert på id.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await service.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    /// <summary>
    /// Oppretter et nytt element.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Item), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest();

        if (request.Title.Length > 80)
            return BadRequest();


        var item = new Item
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            FoundLocation = request.FoundLocation
        };


        var created = await service.CreateAsync(item);


        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    /// <summary>
    /// Hent et element basert på id og merk det som hentet av en person.
    /// </summary>
    [HttpPost("{id}/claim")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Claim(Guid id, ClaimItemRequest request)
    {
        var item = await service.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        try
        {
            item.Claim(request.ClaimedBy);
            await service.UpdateAsync(item);
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }

        return Ok(item);
    }

    /// <summary>
    /// Hent et element basert på id og merk det som returnert.
    /// </summary>
    [HttpPost("{id}/return")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Return(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        if (item == null)
            return NotFound();

        try
        {
            item.Return();
            await service.UpdateAsync(item);
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
        return Ok(item);
    }

    /// <summary>
    /// Slett et element basert på id.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await service.GetByIdAsync(id);
        if (item == null)
            return NotFound();

        try
        {
            await service.DeleteAsync(item);
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
        return NoContent();
    }

    // GET filtering

}