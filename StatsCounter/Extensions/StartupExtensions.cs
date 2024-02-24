using System;
using Microsoft.Extensions.DependencyInjection;
using StatsCounter.Services;

namespace StatsCounter.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddGitHubService(
        this IServiceCollection services, Uri baseApiUrl)
    {
        services.AddHttpClient<IGitHubService, GitHubService>(c =>
        {
            c.BaseAddress = baseApiUrl;
            c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            c.DefaultRequestHeaders.Add("User-Agent", "StatsCounter");
        });
        return services;
    }
}