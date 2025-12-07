using Routes.Interfaces;
using Routes.Models;

namespace Routes.Managers
{
    public class VRPSolutionManager : IVRPSolutionManager
    {
        private readonly Random _rnd = new();

        public VRPSolution SolveVRP(double[][] distances, int vehicleCount, int iterations)
        {
            int n = distances.Length;

            var population = new List<List<int>>();
            for (int i = 0; i < 50; i++)
                population.Add(CreateRandomGenome(n, vehicleCount));

            List<int> bestEverGenome = null;
            double bestEverFitness = double.MaxValue;

            for (int iter = 0; iter < iterations; iter++)
            {
                var scored = population
                    .Select(g => (Genome: g, Fitness: Evaluate(g, distances)))
                    .OrderBy(x => x.Fitness)
                    .ToList();

                var currentBest = scored.First();
                if (currentBest.Fitness < bestEverFitness)
                {
                    bestEverFitness = currentBest.Fitness;
                    bestEverGenome = new List<int>(currentBest.Genome);
                }

                var newPop = scored.Take(10).Select(x => x.Genome).ToList();

                while (newPop.Count < 50)
                {
                    var parent1 = Tournament(scored);
                    var parent2 = Tournament(scored);

                    var child = Crossover(parent1, parent2);
                    Mutate(child);
                    Normalize(child, vehicleCount, n);

                    newPop.Add(child);
                }

                population = newPop;
            }

            return new VRPSolution
            {
                Routes = Split(bestEverGenome),
                TotalDistance = bestEverFitness
            };
        }

        private List<int> CreateRandomGenome(int n, int vehicles)
        {
            var points = Enumerable.Range(1, n - 1).ToList();
            points = points.OrderBy(_ => _rnd.Next()).ToList();

            for (int i = 0; i < vehicles - 1; i++)
                points.Insert(_rnd.Next(points.Count), -1);

            return points;
        }

        private double Evaluate(List<int> genome, double[][] distances)
        {
            double total = 0;
            int prev = 0;

            foreach (var gene in genome)
            {
                if (gene == -1)
                {
                    total += distances[prev][0];
                    prev = 0;
                    continue;
                }

                total += distances[prev][gene];
                prev = gene;
            }

            total += distances[prev][0];
            return total;
        }

        private List<int> Tournament(List<(List<int> Genome, double Fitness)> pop)
        {
            var a = pop[_rnd.Next(pop.Count)];
            var b = pop[_rnd.Next(pop.Count)];
            return a.Fitness < b.Fitness ? a.Genome : b.Genome;
        }

        private List<int> Crossover(List<int> a, List<int> b)
        {
            var pointsA = a.Where(x => x != -1).ToList();
            var pointsB = b.Where(x => x != -1).ToList();

            int n = pointsA.Count;
            var child = Enumerable.Repeat(-2, n).ToList();

            int start = _rnd.Next(n);
            int end = _rnd.Next(start, n);

            for (int i = start; i <= end; i++)
                child[i] = pointsA[i];

            int idx = 0;
            for (int i = 0; i < n; i++)
            {
                if (child[i] != -2) continue;

                while (child.Contains(pointsB[idx]))
                    idx++;

                child[i] = pointsB[idx];
            }

            return child;
        }

        private void Mutate(List<int> genome)
        {
            if (_rnd.NextDouble() > 0.1) return;

            int a = _rnd.Next(genome.Count);
            int b = _rnd.Next(genome.Count);

            (genome[a], genome[b]) = (genome[b], genome[a]);
        }

        private void Normalize(List<int> genome, int vehicles, int n)
        {
            var points = genome.Where(x => x != -1).ToList();

            genome.Clear();
            foreach (var p in points)
                genome.Add(p);

            for (int i = 0; i < vehicles - 1; i++)
                genome.Insert(_rnd.Next(genome.Count), -1);
        }

        private List<List<int>> Split(List<int> genome)
        {
            var res = new List<List<int>>();
            var cur = new List<int>();

            foreach (var g in genome)
            {
                if (g == -1)
                {
                    res.Add(cur);
                    cur = new List<int>();
                }
                else
                    cur.Add(g);
            }

            res.Add(cur);
            return res;
        }
    }
}
