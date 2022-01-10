using GraphQL.EntityFramework;
using GraphQL.Types;

public static class DefaultArguments
{
    public static QueryArgument<ListGraphType<NonNullGraphType<WhereExpressionGraph>>> Where() => ArgumentAppender.whereArgument();
    public static QueryArgument<ListGraphType<NonNullGraphType<OrderByGraph>>> OrderBy() => ArgumentAppender.orderByArgument();
    public static QueryArgument<ListGraphType<NonNullGraphType<IdGraphType>>> Ids() => ArgumentAppender.idsArgument();
    public static QueryArgument<IdGraphType> Id() => ArgumentAppender.idArgument();
    public static QueryArgument<IntGraphType> Skip() => ArgumentAppender.skipArgument();
    public static QueryArgument<IntGraphType> Take() => ArgumentAppender.takeArgument();
}