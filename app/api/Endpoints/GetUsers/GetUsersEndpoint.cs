using api.Endpoints.GetUsers;
using api.Models.Views;
using api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace api.Endpoints.GetAllUsers
{
    public class GetUsersEndpoint : Endpoint<EmptyRequest, GetUserResponse>
    {
        public IUserService userService { get; set; }

        public GetUsersEndpoint(IUserService userService)
        {
            this.userService = userService;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/users");
            AllowAnonymous();
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var data = await userService.GetUsersAsync();
            await SendAsync(new GetUserResponse()
            {
                Data = data
            }, StatusCodes.Status200OK, ct);
        }
    }
}
