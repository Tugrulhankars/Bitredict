using Bitredict.DataAccess.Abstract;
using Bitredict.DataAccess.EntityFramework;
using Bitredict.Models;
using Microsoft.EntityFrameworkCore;

namespace Bitredict.DataAccess.Concrete;

public class UserRepository : EfRepositoryBase<User, BaseDbContext>, IUserRepository
{
    public UserRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<User> GetUserByWalletPublicAddress(string walletPublicAddress)
    {
        using (BaseDbContext context = new BaseDbContext())
        {
            User result = await context.Users.FirstOrDefaultAsync(op => op.WalletPublicAddress == walletPublicAddress);
            return result;


        }


    }
}
