using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Common.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Categories: Delete")
            .WithSummary("Exclui uma categoria")
            .WithDescription("Exclui uma categoria")
            .WithOrder(3)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
    {
        var request = new DeleteCategoryRequest 
        {
            Id = id,
            UserId = "teste@blazor.com"            
        };
        var result = await handler.DeleteAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
