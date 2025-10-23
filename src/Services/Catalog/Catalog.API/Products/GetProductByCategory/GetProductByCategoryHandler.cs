
namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(IDocumentSession session)
    //, ILogger<GetProductByCategoryQueryHandler> logger)
    : IRequestHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        //logger.LogInformation("GetProductByCategoryQueryHandler.Handle with {@query}", query);

        var products = await session.Query<Product>()
            .Where(c => c.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);
        //if (products is null) throw new ProductNotFoundException();

        return new GetProductByCategoryResult(products);
    }
}
