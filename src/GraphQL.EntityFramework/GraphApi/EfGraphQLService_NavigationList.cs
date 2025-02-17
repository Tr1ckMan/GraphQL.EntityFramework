﻿using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.EntityFramework;

partial class EfGraphQLService<TDbContext>
    where TDbContext : DbContext
{
    public FieldType AddNavigationListField<TSource, TReturn>(
        ComplexGraphType<TSource> graph,
        string name,
        Func<ResolveEfFieldContext<TDbContext, TSource>, IEnumerable<TReturn>>? resolve = null,
        Type? itemGraphType = null,
        IEnumerable<QueryArgument>? arguments = null,
        IEnumerable<string>? includeNames = null,
        string? description = null,
        bool isNullable = false)
        where TReturn : class
    {
        Guard.AgainstWhiteSpace(nameof(name), name);

        var hasId = keyNames.ContainsKey(typeof(TReturn));
        var field = new FieldType
        {
            Name = name,
            Description = description,
            Type = MakeListGraphType<TReturn>(itemGraphType, isNullable),
            Arguments = ArgumentAppender.GetQueryFieldArguments(arguments, hasId),
        };

        IncludeAppender.SetIncludeMetadata(field, name, includeNames);

        if (resolve is not null)
        {
            field.Resolver = new AsyncFieldResolver<TSource, IEnumerable<TReturn>>(
                context =>
                {
                    var fieldContext = BuildContext(context);
                    var result = resolve(fieldContext);
                    result = result.ApplyGraphQlArguments(hasId, context);
                    return fieldContext.Filters.ApplyFilter(result, context.UserContext)!;
                });
        }

        return graph.AddField(field);
    }
}