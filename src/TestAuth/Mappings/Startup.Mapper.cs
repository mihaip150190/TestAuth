using TestAuth.Authentication;
using TestAuth.AuthModels;

namespace TestAuth
{
    public partial class Startup
    {
        public static void InitMapper()
        {
            AutoMapper.Mapper.CreateMap<BasicUser, User>();
            AutoMapper.Mapper.CreateMap<User, BasicUser>();
        }
    }
}
