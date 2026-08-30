using NUnit.Framework;

// Enable parallel execution across test fixtures and scenarios in NUnit
[assembly: Parallelizable(ParallelScope.Children)]

// Define default degree of parallelism (number of concurrent worker threads)
[assembly: LevelOfParallelism(4)]
