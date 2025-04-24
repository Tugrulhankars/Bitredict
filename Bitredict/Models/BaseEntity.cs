namespace Bitredict.Models;

public class BaseEntity
{
    public DateOnly CreatedDate { get; set; }
    public DateOnly? UpdatedDate { get; set; }
    public DateOnly? DeletedDate { get; set; }



}