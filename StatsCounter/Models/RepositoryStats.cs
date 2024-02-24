using System.Collections.Generic;

namespace StatsCounter.Models;

public class RepositoryStats
{
    public string Owner { get; set; }
    public HashSet<string> Languages { get; set; }
    public long Size { get; set; }
    public int Repositories { get; set; }
    public double AvgWatchers { get; set; }
    public double AvgForks { get; set; }
}