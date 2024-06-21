using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using s28371Test.DTOs;
using s28371Test.Services;

namespace s28371Test.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CharactersController : ControllerBase
{
    private readonly IDbService _service;

    public CharactersController(IDbService service)
    {
        _service = service;
    }


    [HttpGet("{characterId}")]
    public async Task<IActionResult> GetCharInfo(int characterId)
    {
        if (!await _service.ExistsCharById(characterId))
            return NotFound("No char with this id exists");
        return Ok(await _service.GetCharInfo(characterId));

    }

    [HttpPost("{characterId}/backpacks")]
    public async Task<IActionResult> AddItemsToInventory(int characterId, ICollection<PostItemDto> command)
    {
        if (!await _service.ExistsCharById(characterId))
            return NotFound("No char with this id exists");
        foreach (var item in command)
        {
            if (! await _service.ExistsItemById(item.ItemId))
            {
                return NotFound($"No item with id {item.ItemId} exists!");

            }
        }

        if (!await _service.AllItemsFitForChar(command, characterId))
            return BadRequest("Too much weight for char to carry");


        return Ok(await _service.AddItemsToChar(command, characterId));


        // •additems to a character's inventory and update the current weight of a character's items•
        // if the character already has such an item, update its quantity






    }
}