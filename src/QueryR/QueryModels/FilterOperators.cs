using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryR.QueryModels
{
    public static class FilterOperators
    {
        public static readonly FilterOperator Equal = new FilterOperator(
            "eq",
            nameof(Equal),
            (property, target) => Expression.Equal(property, target)
        );
        public static readonly FilterOperator GreaterThan = new FilterOperator(
            "gt",
            nameof(GreaterThan),
            (property, target) => Expression.GreaterThan(property, target)
        );
        public static readonly FilterOperator GreaterThanOrEqual = new FilterOperator(
            "gte",
            nameof(GreaterThanOrEqual),
            (property, target) => Expression.GreaterThanOrEqual(property, target)
        );
        public static readonly FilterOperator LessThan = new FilterOperator(
            "lt",
            nameof(LessThan),
            (property, target) => Expression.LessThan(property, target)
        );
        public static readonly FilterOperator LessThanOrEqual = new FilterOperator(
            "lte",
            nameof(LessThanOrEqual),
            (property, target) => Expression.LessThanOrEqual(property, target)
        );
        public static readonly FilterOperator NotEqual = new FilterOperator(
            "ne",
            nameof(NotEqual),
            (property, target) => Expression.NotEqual(property, target)
        );
        public static readonly FilterOperator Contains = new FilterOperator(
            "ct",
            nameof(Contains),
            (property, target) => Expression.Call(
                property,
                typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                target)
        );
        public static readonly FilterOperator In = new FilterOperator(
            "in",
            nameof(In),
            (property, target) => Expression.Call(
                target,
                target.Type.GetMethod("Contains", new Type[] { property.Type }),
                property)
        );
        public static readonly FilterOperator StartsWith = new FilterOperator(
            "sw",
            nameof(StartsWith),
            (property, target) => Expression.Call(
                property,
                typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                target)
        );
        public static readonly FilterOperator EndsWith = new FilterOperator(
            "ew",
            nameof(EndsWith),
            (property, target) => Expression.Call(
                property,
                typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                target)
        );

        public static readonly IReadOnlyList<FilterOperator> Items = new List<FilterOperator>
        {
            Equal,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqual,
            NotEqual,
            Contains,
            In,
            StartsWith,
            EndsWith,
        };

        public static readonly Dictionary<string, FilterOperator> ToItem = Items.ToDictionary(item => item.Code, item => item);
    }
}
