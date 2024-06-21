using s28371Test.DTOs;

namespace s28371Test.Services;

public interface IDbService
{
    Task<bool> ExistsCharById(int characterId);
    Task<GetCharDTO> GetCharInfo(int characterId);
    Task<bool> ExistsItemById(int itemItemId);
    Task<bool> AllItemsFitForChar(ICollection<PostItemDto> command, int characterId);
    Task<ICollection<PostToReturnDTO>> AddItemsToChar(ICollection<PostItemDto> command, int characterId);
    Task<int> GetAllItemsWeight(ICollection<PostItemDto> command);
}