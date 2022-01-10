using GraphQL.EntityFramework;
using GraphQL.Types;

static class ArgumentAppender
{
    internal static QueryArgument<ListGraphType<NonNullGraphType<WhereExpressionGraph>>> whereArgument()
    {
        return new()
        {
            Name = "where"
        };
    }

    internal static QueryArgument<ListGraphType<NonNullGraphType<OrderByGraph>>> orderByArgument()
    {
        return new()
        {
            Name = "orderBy"
        };
    }

    internal static QueryArgument<ListGraphType<NonNullGraphType<IdGraphType>>> idsArgument()
    {
        return new()
        {
            Name = "ids"
        };
    }

    internal static QueryArgument<IdGraphType> idArgument()
    {
        return new()
        {
            Name = "id"
        };
    }

    internal static QueryArgument<IntGraphType> skipArgument()
    {
        return new()
        {
            Name = "skip"
        };
    }

    internal static QueryArgument<IntGraphType> takeArgument()
    {
        return new()
        {
            Name = "take"
        };
    }

    public static void AddWhereArgument(this FieldType field, bool hasId, IEnumerable<QueryArgument>? extra)
    {
        var arguments = field.Arguments!;
        arguments.AddIfNotExist(whereArgument());
        arguments.AddIfNotExist(orderByArgument());
        if (hasId)
        {
            arguments.AddIfNotExist(idsArgument());
        }
        if (extra is not null)
        {
            foreach (var argument in extra)
            {
                arguments.AddIfNotExist(argument);
            }
        }
    }

    public static QueryArguments GetQueryArguments(IEnumerable<QueryArgument>? extra, bool hasId, bool applyOrder)
    {
        var arguments = new QueryArguments();
        if (hasId)
        {
            arguments.Add(idArgument());
            arguments.Add(idsArgument());
        }

        arguments.Add(whereArgument());
        if (applyOrder)
        {
            arguments.Add(orderByArgument());
            arguments.Add(skipArgument());
            arguments.Add(takeArgument());
        }

        if (extra is not null)
        {
            foreach (var argument in extra)
            {
                arguments.AddIfNotExist(argument);
            }
        }

        return arguments;
    }

    public static QueryArguments GetQueryFieldArguments(IEnumerable<QueryArgument>? extra, bool hasId)
    {
        var arguments = new QueryArguments();
        if (extra is null)
        {
            AddDefaultArguments(hasId, false, arguments);
        }
        else
        {
            foreach (var argument in extra)
            {
                arguments.AddIfNotExist(argument);
            }
        }

        return arguments;
    }

    public static QueryArguments GetSingleFieldArguments(IEnumerable<QueryArgument>? extra, bool hasId)
    {
        var arguments = new QueryArguments();
        if (extra is null)
        {
            AddDefaultArguments(hasId, true, arguments);
        }
        else
        {
            foreach (var argument in extra)
            {
                arguments.AddIfNotExist(argument);
            }
        }

        return arguments;
    }

    private static void AddDefaultArguments(bool hasId, bool isSingleField, QueryArguments arguments)
    {
        if (hasId)
        {
            arguments.AddIfNotExist(idArgument());
            if (!isSingleField)
                arguments.AddIfNotExist(idsArgument());
        }

        arguments.AddIfNotExist(whereArgument());

        if (!isSingleField)
        {
            arguments.AddIfNotExist(orderByArgument());
            arguments.AddIfNotExist(skipArgument());
            arguments.AddIfNotExist(takeArgument());
        }
    }
}