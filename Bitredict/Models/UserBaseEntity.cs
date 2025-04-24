namespace Bitredict.Models;

public class UserBaseEntity
{
    public DateOnly CreatedDate { get; init; }
    public DateOnly? UpdatedDate { get; set; }
    public DateOnly? DeletedDate { get; set; }
    public DateOnly ActiveStartDate { get; set; }
    public DateOnly ActiveFinishDate { get; set; }
}
