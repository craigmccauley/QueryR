# QueryR

![QueryR Logo](./assets/logo.png)

[![.NET](https://github.com/craigmccauley/QueryR/actions/workflows/dotnet.yml/badge.svg)](https://github.com/craigmccauley/QueryR/actions/workflows/dotnet.yml)

QueryR provides a simple interface for executing ad hoc queries against `IQueryable<T>` implementations.

This is useful in situations where there is a need to provide end users with the ability to create custom queries without increasing the complexity of the solution.

In practice, this could be an API route that performs an Ad Hoc Query on a database or in-memory objects.

## Console App Examples

See [QueryR Example Data](#queryr-example-data) for the data.

```CSharp
var queryResult = kerbals.AsQueryable().Query(new Filter
{
    PropertyName = nameof(Kerbal.FirstName),
    Operator = FilterOperators.StartsWith,
    Value = "B"
}).ToList();

Console.WriteLine($"Filter matched {queryResult.Count} Kerbal(s). They are:");
foreach(var item in queryResult)
{
    Console.WriteLine($" - {item.FirstName}");
}

// Expected Output :
// Filter matched 2 Kerbal(s). They are:
//  - Bob
//  - Bill

```

## Web API Example

See [QueryR.EntityFrameworkCore Example Data](#queryrentityframeworkcore-example-data) for the data.

Given

```CSharp
public class KerbalsGet : BaseController
{
    [HttpGet(Routes.Api.Kerbals.Url)]
    public async Task<IActionResult> Action(Command command) => await Mediator.Send(command);
    public class Command : IRequest<IActionResult>
    {
        [FromQuery(Name = "")]
        public QueryParameters QueryParameters { get; set; } = new();
    }
    public class ResponseObject
    {
        public int TotalCount { get; set; }
        public List<Kerbal> Kerbals { get; set; }
    }
    public class Handler : IRequestHandler<Command, IActionResult>
    {
        public readonly IQueryParameterMapper queryParameterMapper;
        public readonly KspDbContext kspDbContext;
        public Handler(
            IQueryParameterMapper queryParameterMapper,
            KspDbContext kspDbContext
        )
        {
            this.queryParameterMapper = queryParameterMapper;
            this.kspDbContext = kspDbContext;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var query = queryParameterMapper.MapToQuery(request.QueryParameters);

            var (totalCount, kerbals) = await kspDbContext.Set<Kerbal>().Query(query)
                .GetCountAndListAsync(cancellationToken);

            return new OkObjectResult(new ResponseObject {
                TotalCount = totalCount,
                Kerbals = kerbals
            });
        }
    }
}
```

We could query it like so.

```http
GET /api/kerbals?filter[FirstName][sw]=B
```

and get a response like

```http
OK
{
    "totalCount" : 2,
    "kerbals" : [
        {
            Id = 1,
            FirstName = "Bill",
            LastName = "Kermin",
            AssignedSpaceCraftId = 1,
        },
        {
            Id = 2,
            FirstName = "Bob",
            LastName = "Kermin",
            AssignedSpaceCraftId = 1,
        }
    ]
}
```

#### QueryR Example Data

```CSharp
var kerbals = new List<Kerbal>
{
    new Kerbal { FirstName = "Bob", LastName = "Kerman" },
    new Kerbal { FirstName = "Bill", LastName = "Kerman" },
    new Kerbal { FirstName = "Jeb", LastName = "Kerman" },
    new Kerbal { FirstName = "Val", LastName = "Kerman" },
};
```