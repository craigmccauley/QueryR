# QueryR

![QueryR Logo](./assets/logo.png)

[![.NET](https://github.com/craigmccauley/QueryR/actions/workflows/dotnet.yml/badge.svg)](https://github.com/craigmccauley/QueryR/actions/workflows/dotnet.yml)

QueryR provides a simple interface for executing ad hoc queries against `IQueryable<T>` implementations.

This is useful in situations where there is a need to provide end users with the ability to create custom queries without increasing the complexity of the solution.

In practice you will have your own domain query criteria object that collects what you want to query. You will perform a map to the `QueryR.Query` object and send it to the `IQueryable<T>.Query` method.

If you intend to use QueryR with EntityFrameworkCore, please use [QueryR.EntityFrameworkCore](https://github.com/craigmccauley/QueryR.EntityFrameworkCore).

## Basic Functionality Example

```CSharp
var kerbals = new List<Kerbal>
{
    new Kerbal { FirstName = "Bob", LastName = "Kerman" },
    new Kerbal { FirstName = "Bill", LastName = "Kerman" },
    new Kerbal { FirstName = "Jeb", LastName = "Kerman" },
    new Kerbal { FirstName = "Val", LastName = "Kerman" },
};

//Note: 
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

## QueryR Full Functionality Example

QueryR can perform the following `IQueryAction`s.

- Filter - Reduce the amount of records returned by specifying conditions. Does not support "OR", if you need "OR", run a seond query.
- Paging
- Sort
- Sparse Fieldsets - Restrict the fields returned for a specified entity.

```CSharp
var (Count, Items) = kerbals.AsQueryable().Query(new Query
{
    Filters = new List<Filter>
    {
        new Filter
        {
            PropertyName = nameof(Kerbal.FirstName),
            Operator = FilterOperators.Contains,
            Value = "l"
        },
    },
    PagingOptions = new PagingOptions
    {
        PageNumber = 2,
        PageSize = 1
    },
    Sorts = new List<Sort>
    {
        new Sort
        {
            IsAscending = false,
            PropertyName = nameof(Kerbal.FirstName)
        },
    },
    SparseFields = new List<SparseField>
    {
        new SparseField
        {
            EntityName = nameof(Kerbal),
            PropertyNames = new List<string> { nameof(Kerbal.FirstName) }
        },
    }
}).GetCountAndList();

Console.WriteLine($"{Count} Kerbal(s) match filter, {Items.Count} Kerbal(s) returned:");
foreach(var item in queryResult)
{
    Console.WriteLine($" - {item.FirstName} {item.LastName} was found.");
}

// Expected output:
// 2 Kerbal(s) match filter, 1 Kerbal(s) returned:
// - Bill  was found.
```

## QueryR Filters

QueryR provides the following filters:

- Equal
- GreaterThan
- GreaterThanOrEqual
- LessThan
- LessThanOrEqual
- NotEqual
- Contains
- In
- StartsWith
- EndsWith
- CollectionContains

Extra filters can be defined and used as if they were a part of QueryR.
For example, if a string length filter was needed.

We could do

```CSharp
public static class ExtendedFilterOperators
{
    public static readonly FilterOperator LengthLessThan = new FilterOperator("llt", nameof(LengthLessThan),
        (property, target) => Expression.LessThan(Expression.Property(property, nameof(string.Length)), target));
}
```

and use it like so

```CSharp
var queryResult = kerbals.AsQueryable().Query(new Filter
{
    PropertyName = nameof(Kerbal.FirstName),
    Operator = ExtendedFilterOperators.LengthLessThan,
    Value = 4
}).ToList();

Console.WriteLine($"Filter matched {queryResult.Count} Kerbal(s). They are:");
foreach(var item in queryResult)
{
    Console.WriteLine($" - {item.FirstName}");
}

// Expected Output :
// Filter matched 3 Kerbal(s). They are:
//  - Bob
//  - Jeb
//  - Val
```

## Working Example

For a working example of QueryR in action, check out the [ConsoleApp](https://github.com/craigmccauley/QueryR/tree/main/src/QueryR.Examples.ConsoleApp) example.
