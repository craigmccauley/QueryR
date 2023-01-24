using System;
using System.Linq.Expressions;

namespace QueryR.QueryModels
{
    public class FilterOperator
    {
        public string Code { get; }
        public string Name { get; }
        public Func<Expression, Expression, Expression> ExpressionMethod { get; }
        public FilterOperator(string code, string name, Func<Expression, Expression, Expression> expressionMethod)
        {
            Code = code;
            Name = name;
            ExpressionMethod = expressionMethod;
        }
    }
}
