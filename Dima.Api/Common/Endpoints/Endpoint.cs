using Dima.Api.Common.Api;
using Dima.Api.Common.Endpoints.Categories;

namespace Dima.Api.Common.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(prefix: "");

        endpoints.MapGroup("v1/categories")
            .WithTags("Categories")
            //.RequireAuthorization()
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>()
            .MapEndpoint<GetCategoryByIdEndpoint>()
            .MapEndpoint<GetAllCategoriesEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
