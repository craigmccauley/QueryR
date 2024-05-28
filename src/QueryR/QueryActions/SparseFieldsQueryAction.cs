using QueryR.Extensions;
using QueryR.QueryModels;
using QueryR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryR.QueryActions
{
    internal class SparseFieldsQueryAction : IQueryAction
    {
        private readonly IMaxDepthService maxDepthService;

        public SparseFieldsQueryAction(
            IMaxDepthService maxDepthService)
        {
            this.maxDepthService = maxDepthService;
        }

        public QueryResult<T> Execute<T>(Query query, QueryResult<T> queryResult)
        {

            if (query.SparseFields != null && query.SparseFields.Any())
            {
                var type = typeof(T);
                var parameter = Expression.Parameter(type, "input");
                var maxDepth = maxDepthService.GetMaxDepth(query);

                var init = GetSparseMemberInitExpression(type, query.SparseFields, parameter, new List<MemberExpression>(), maxDepth);
                var expression = Expression.Lambda<Func<T, T>>(init, parameter);

                queryResult.PagedQuery = queryResult.PagedQuery
                    .Select(expression);
            }
            return queryResult;
        }

        private Expression GetSparseMemberInitExpression(Type sparseType, List<SparseField> sparseFields, ParameterExpression parameter, List<MemberExpression> memberExpressions, int? maxDepth, int depth = 0)
        {
            var properties = sparseType.GetProperties().ToList();

            var sparseFieldsForThisType = sparseFields.FirstOrDefault(sf => sf.EntityName == sparseType.Name);
            if (sparseFieldsForThisType != null)
            {
                properties = properties.Where(p => sparseFieldsForThisType.PropertyNames.Contains(p.Name)).ToList();
            }

            List<MemberBinding> memberBindings = new List<MemberBinding>();
            foreach (var prop in properties)
            {
                var propExpression = memberExpressions.Any() ? (Expression) memberExpressions.Last() : parameter;
                var currentMemberExpression = Expression.Property(propExpression, prop.Name);
                memberExpressions.Add(currentMemberExpression);

                var property = sparseType.GetProperty(prop.Name);

                if (sparseFields.Any(sf => sf.EntityName == prop.PropertyType.Name))
                {
                    if (!maxDepth.HasValue || depth < maxDepth)
                    {
                        memberBindings.Add(GetNestedMemberBinding(property, sparseFields, parameter, memberExpressions, maxDepth, depth, currentMemberExpression));
                    }
                }
                else if (sparseFields.Any(sf => prop.PropertyType.GenericTypeArguments.Any(gta => gta.Name == sf.EntityName)))
                {
                    if (maxDepth.HasValue && depth < maxDepth)
                    {
                        memberBindings.Add(GetListMemberBinding(prop, sparseFields, currentMemberExpression, maxDepth, depth));
                    }
                }
                else
                {
                    memberBindings.Add(Expression.Bind(property, currentMemberExpression));
                }
                memberExpressions.Remove(currentMemberExpression);
            }
            return Expression.MemberInit(Expression.New(sparseType), memberBindings);
        }



        private MemberBinding GetNestedMemberBinding(PropertyInfo property, List<SparseField> sparseFields, ParameterExpression parameter, List<MemberExpression> memberExpressions, int? maxDepth, int depth, MemberExpression currentMemberExpression)
        {
            var defaultValue = Expression.Constant(property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null);
            var castDefaultValue = Expression.Convert(defaultValue, property.PropertyType);
            var nullCheck = Expression.Equal(Expression.Property(parameter, property.Name), Expression.Constant(null));
            var nestedMemberExpression = GetSparseMemberInitExpression(property.PropertyType, sparseFields, parameter, memberExpressions, maxDepth, depth + 1);
            var nestedWithNullCheck = Expression.Condition(nullCheck, castDefaultValue, nestedMemberExpression);
            return Expression.Bind(property, nestedWithNullCheck);
        }

        private MemberBinding GetListMemberBinding(PropertyInfo prop, List<SparseField> sparseFields, MemberExpression currentMemberExpression, int? maxDepth, int depth)
        {
            var listType = prop.PropertyType.GenericTypeArguments.First(gta => sparseFields.Any(sf => gta.Name == sf.EntityName));
            var selectMethodExpression = typeof(Enumerable).GetGenericMethod(nameof(Enumerable.Select), 2, 2, listType, listType);

            var listParameter = Expression.Parameter(listType, "listInput");
            var listInit = GetSparseMemberInitExpression(listType, sparseFields, listParameter, new List<MemberExpression>(), maxDepth, depth + 1);

            var lambda = typeof(Expression).GetGenericMethod(nameof(Expression.Lambda), 1, 2, listType);

            var listExpression = Expression.Lambda(listInit, listParameter);

            var callSelectExpression = Expression.Call(selectMethodExpression, currentMemberExpression, listExpression);

            var toListMethodExpression = typeof(Enumerable).GetGenericMethod(nameof(Enumerable.ToList), 1, 1, listType);

            var callToListExpression = Expression.Call(toListMethodExpression, callSelectExpression);

            return Expression.Bind(prop, callToListExpression);
        }
    }
}
