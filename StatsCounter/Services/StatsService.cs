using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatsCounter.Models;

namespace StatsCounter.Services
{
    public interface IStatsService
    {
        Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner);
    }

    public class StatsService : IStatsService
    {
        private readonly IGitHubService _gitHubService;

        public StatsService(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService ?? throw new ArgumentNullException(nameof(gitHubService));
        }

        public async Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner)
        {
            owner = owner?.Trim() ?? throw new ArgumentException("Owner cannot be null or empty.", nameof(owner));

            IEnumerable<RepositoryInfo> repositories = await _gitHubService.GetRepositoryInfosByOwnerAsync(owner) ?? Enumerable.Empty<RepositoryInfo>();

            if (repositories == null)
            {
                throw new Exception("Received null repository list from GitHub service.");
            }

            long totalSize = repositories.Sum(repo => repo.Size);
            double totalWatchers = repositories.Average(repo => repo.Watchers);
            double totalForks = repositories.Average(repo => repo.Forks);

            int repoCount = repositories.Count();
            HashSet<string> languages = new HashSet<string>();

            foreach(RepositoryInfo repo in repositories)
            {
                if (repo.Language != "" && repo.Language != null)
                {
                    languages.Add(repo.Language);
                }
            }
            
            double avgWatchers = repoCount > 0 ? (double)totalWatchers / repoCount : 0.0;
            double avgForks = repoCount > 0 ? (double)totalForks / repoCount : 0.0;

            return new RepositoryStats
            {
                Owner = owner,
                Languages = languages,
                Size = totalSize,
                Repositories = repoCount,
                AvgWatchers = avgWatchers,
                AvgForks = avgForks
            };
        }
    }
}