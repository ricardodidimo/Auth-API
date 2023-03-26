using api.Models.Views;
using System.Collections.Generic;

namespace api.Endpoints.GetUsers
{
    public class GetUserResponse
    {
        public List<UserViewModel> Data { get; set; }
    }
}
