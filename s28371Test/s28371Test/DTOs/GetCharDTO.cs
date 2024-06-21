namespace s28371Test.DTOs;

public class GetCharDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int CurrentWeight { get; set; }
    public int MaxWeight { get; set; }
    public ICollection<ItemInBackpackDTO> BackpackItems { get; set; } = new List<ItemInBackpackDTO>();



    public ICollection<TitleDTO> Titles { get; set; } = new List<TitleDTO>();


}
public class ItemInBackpackDTO
{
    public string ItemName { get; set; } = string.Empty;
    public int ItemWeight { get; set; }
    public int Amount { get; set; }
}
public class TitleDTO
{
    public string Title { get; set; } = string.Empty;
    public DateTime AcquiredAt { get; set; }
    
}