using Bitredict.Models;

namespace Bitredict.DataAccess.Abstract;

public interface IUserRepository : IRepository<User>
{
    public Task<User> GetUserByWalletPublicAddress(string walletPublicAddress);
}
