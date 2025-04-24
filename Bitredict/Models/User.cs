namespace Bitredict.Models;

public class User :  UserBaseEntity
{
    public int UserId { get; set; }
    public string WalletPublicAddress { get; set; }
    public bool IsActive { get; set; }

    public User()
    {
        ActiveStartDate = DateOnly.FromDateTime(DateTime.Now);
        ActiveFinishDate = ActiveStartDate.AddMonths(1);
    }
}
