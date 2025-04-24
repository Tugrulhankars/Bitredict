using AutoMapper;
using Bitredict.DataAccess.Abstract;
using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;
using Bitredict.Models;
using Bitredict.Services.Abstracts;

namespace Bitredict.Services.Manager;

public class UserManager : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserManager(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<bool> CreateUser(CreateUserRequest createUserRequest)
    {
       
            var user = _mapper.Map<User>(createUserRequest);
            await _userRepository.Add(user);
            return true;
       

    }

    public async Task<bool> GetUser(string userWalletPublicAddress)
    {
        bool permission =false;
        User user = await _userRepository.GetUserByWalletPublicAddress(userWalletPublicAddress);
        if (user==null)
        {
            return false;
        }
        if (user.IsActive == false)
        {
            permission = false;
        }
        else if (user.IsActive == true)
        {
            permission = true;
        }
        else if (user.ActiveFinishDate.Day - user.ActiveStartDate.Day == 0)
        {
           
            permission = false;
        }

        return permission;

        
    }


}
