namespace Bitredict.Dtos.Response;

public class CreateUserResponse
{
    public string WalletPublicAddress { get; set; }
    public bool IsActive { get; set; }
    public DateOnly ActiveStartDate { get; set; }
    public DateOnly ActiveFinishDate { get; set; }
}
