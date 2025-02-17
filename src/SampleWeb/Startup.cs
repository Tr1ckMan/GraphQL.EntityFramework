﻿using GraphiQl;
using GraphQL.EntityFramework;
using GraphQL;
using GraphQL.Types;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        EfGraphQLConventions.RegisterInContainer<SampleDbContext>(
            services,
            model: SampleDbContext.StaticModel);
        EfGraphQLConventions.RegisterConnectionTypesInContainer(services);

        foreach (var type in GetGraphQlTypes())
        {
            services.AddSingleton(type);
        }
        //TODO: re add for subscriptions
        //var graphQl = services.AddGraphQL(
        //    options => options.ExposeExceptions = true);
        //graphQl.AddWebSockets();

        var dbContextBuilder = new DbContextBuilder();
        services.AddSingleton<IHostedService>(dbContextBuilder);
        services.AddSingleton<Func<SampleDbContext>>(_ => dbContextBuilder.BuildDbContext);
        services.AddScoped(_ => dbContextBuilder.BuildDbContext());
        services.AddSingleton<IDocumentExecuter, EfDocumentExecuter>();
        services.AddSingleton<ISchema, Schema>();
        var mvc = services.AddMvc(option => option.EnableEndpointRouting = false);
        mvc.AddNewtonsoftJson();
    }

    static IEnumerable<Type> GetGraphQlTypes()
    {
        return typeof(Startup).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract &&
                        (typeof(IObjectGraphType).IsAssignableFrom(x) ||
                         typeof(IInputObjectGraphType).IsAssignableFrom(x)));
    }

    public void Configure(IApplicationBuilder builder)
    {
        builder.UseWebSockets();
        //builder.UseGraphQLWebSockets<ISchema>();
        builder.UseGraphiQl("/graphiql", "/graphql");
        builder.UseMvc();
    }
}