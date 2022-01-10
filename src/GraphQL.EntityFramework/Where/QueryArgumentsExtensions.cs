using GraphQL.Types;

static class QueryArgumentsExtensions
{
    public static void AddIfNotExist(this QueryArguments arguments, QueryArgument argument)
    {
        if (arguments.Find(argument.Name) is null)
            arguments.Add(argument);
    }
}
