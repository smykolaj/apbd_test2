namespace s28371Test.DTOs;

public class PostItemDto
{
    public int ItemId { get; set; }
    public int Amount { get; set; }
}

public class PostToReturnDTO
{
    public int ItemId { get; set; }
    public int Amount { get; set; }
    public int CharacterId { get; set; }
}