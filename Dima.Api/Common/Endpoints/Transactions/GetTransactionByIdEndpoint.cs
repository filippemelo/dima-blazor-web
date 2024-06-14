using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Common.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: Get By ID")
            .WithSummary("Recupera uma transação")
            .WithDescription("Recupera uma transação")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var request = new GetTransactionByIdRequest
        {
            Id = id,
            UserId = "teste@blazor.com"
        };
        var result = await handler.GetByIdAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
