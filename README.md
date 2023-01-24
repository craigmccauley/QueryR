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