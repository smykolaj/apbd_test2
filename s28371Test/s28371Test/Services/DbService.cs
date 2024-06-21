using System.Transactions;
using Microsoft.EntityFrameworkCore;
using s28371Test.Context;
using s28371Test.DTOs;
using s28371Test.Models;

namespace s28371Test.Services;

public class DbService : IDbService
{

    private readonly ApplContext _context;

    public DbService(ApplContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsCharById(int characterId)
    {
        return await _context.Characters.AnyAsync(c => c.Id.Equals(characterId));
    }

    public async Task<GetCharDTO> GetCharInfo(int characterId)
    {
        return await
            _context
                .Characters
                .Where(c => c.Id.Equals(characterId))
                .Select( c => new GetCharDTO()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    CurrentWeight = c.CurrentWeight,
                    MaxWeight = c.MaxWeight,
                    BackpackItems = c.Backpacks.Select(b => new ItemInBackpackDTO()
                    {
                        ItemName = b.Item.Name,
                        ItemWeight = b.Item.Weight,
                        Amount = b.Amount
                    }).ToList(),
                    Titles = c.CharacterTitles.Select(ct => new TitleDTO()
                    {
                        Title = ct.Title.Name,
                        AcquiredAt = ct.AcquiredAt
                    }).ToList()

                }).FirstAsync();

    }

    public async Task<bool> ExistsItemById(int itemItemId)
    {
        return await _context.Items.AnyAsync(i => i.Id.Equals(itemItemId));
    }

    public async Task<bool> AllItemsFitForChar(ICollection<PostItemDto> command, int characterId)
    { var current = await _context
            .Characters
            .Where(c => c.Id.Equals(characterId))
            .Select(c => c.CurrentWeight)
            .FirstAsync();
        var max = await _context
            .Characters
            .Where(c => c.Id.Equals(characterId))
            .Select(c => c.MaxWeight)
            .FirstAsync();

        

        return max >= current + await GetAllItemsWeight(command);

    }

    public async Task<int> GetAllItemsWeight(ICollection<PostItemDto> command)
    {
        var allItemsWeight = 0;
        foreach (var item in command)
        {
            allItemsWeight +=
                await
                    _context
                        .Items
                        .Where(i => i.Id.Equals(item.ItemId))
                        .Select(i => i.Weight)
                        .FirstAsync() * item.Amount;
        }

        return allItemsWeight;

    }

    public async Task<ICollection<PostToReturnDTO>> AddItemsToChar(ICollection<PostItemDto> command, int characterId)
    {
        var output = new List<PostToReturnDTO>();
        
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var character =  await _context
                .Characters
                .Where(c => c.Id.Equals(characterId))
                .Include(character => character.Backpacks)
                .FirstAsync();

            foreach (var item in command)
            {
                if (character.Backpacks.Any(i => i.ItemId.Equals(item.ItemId)))
                {
                    var itemToUpdate = character.Backpacks.First(i => i.ItemId.Equals(item.ItemId));
                    itemToUpdate.Amount += item.Amount;
                    await _context.SaveChangesAsync();
                    output.Add(new PostToReturnDTO()
                    {
                        ItemId = itemToUpdate.ItemId,
                        Amount = itemToUpdate.Amount,
                        CharacterId = characterId
                    });
                    continue;
                }
                if (! character.Backpacks.Any(i => i.ItemId.Equals(item.ItemId)))
                {
                    var newItem = new Backpack()
                    {

                        ItemId = item.ItemId,
                        CharacterId = characterId,
                        Amount = item.Amount
                    };
                   await  _context.Backpacks.AddAsync(newItem);
                   await _context.SaveChangesAsync();
                   output.Add(new PostToReturnDTO()
                   {
                       ItemId = newItem.ItemId,
                       Amount = newItem.Amount,
                       CharacterId = characterId
                   });
                }
            }

            character.CurrentWeight += await GetAllItemsWeight(command);

             await _context.SaveChangesAsync();
    
            scope.Complete();
        }

        return output;

    }
}