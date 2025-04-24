using Bitredict.Dtos.Request;

namespace Bitredict.Services.Abstracts;

public interface IUserService
{
    Task<bool> CreateUser(CreateUserRequest createUserRequest);
    Task<bool> GetUser(string userWalletPublicAddress);



}
