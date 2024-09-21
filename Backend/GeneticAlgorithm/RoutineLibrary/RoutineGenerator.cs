using Newtonsoft.Json;

namespace RoutineLibrary;

public class RoutineGenerator
{
    public List<Gene> AvailableGenes = [
        new("CSE-203", "EI",  "2-1", 1, true),
        new("CSE-205", "NAR", "2-1", 1, false),
        new("CSE-206", "GM",  "2-1", 1, false),
        new("CSE-207", "MMB", "2-1", 1, false),
        new("CSE-208", "MZR", "2-1", 1, false),
        new("CSE-209", "MAI", "2-1", 1, false),
        new("CSE-210", "MAI", "2-1", 1, true),
        new("CSE-212", "EI",  "2-1", 1, true),
        new("CSE-303", "SKS", "3-1", 2, false),
        new("CSE-305", "BA",  "3-1", 2, false),
        new("CSE-307", "JKD", "3-1", 2, false),
        new("CSE-309", "AKA", "3-1", 2, false),
        new("CSE-314", "SB",  "3-1", 2, true),
        new("CSE-304", "SKS", "3-1", 2, true),
    ];

    public int TotalPopulation = 10;

    private readonly Random _random = new();

    public Chromosome Generate()
    {
        var chromosomes = InitializePopulation();

        int generation = 0;
        while (generation < 1000) // max generations
        {
            // Evaluate fitness
            foreach (var chromosome in chromosomes)
            {
                chromosome.CalculateFitness();
            }

            // Sort population by fitness
            chromosomes = [.. chromosomes.OrderByDescending(s => s.Fitness)];

            // If best solution found, break
            if (chromosomes[0].Fitness == 1.0)
            {
                Console.WriteLine("Optimal schedule found:");
                PrintSchedule(chromosomes[0]);

                return chromosomes[0];
            }

            // Selection
            List<Chromosome> newChromosomes = SelectBestPopulation(chromosomes);

            newChromosomes = PerformCrossover(newChromosomes);

            newChromosomes = PerformMutation(newChromosomes);

            chromosomes = newChromosomes;
            generation++;
        }

        return chromosomes[0];
    }

    private List<Chromosome> InitializePopulation()
    {
        List<Chromosome> chromosomes = [];
        for (int i = 0; i < TotalPopulation; i++)
        {
            chromosomes.Add(new Chromosome(AvailableGenes));
        }

        return chromosomes;
    }

    private static List<Chromosome> SelectBestPopulation(
        List<Chromosome> population)
    {
        return population.Take(5).ToList(); // Select top 5 schedules
    }

    private static List<Chromosome> PerformCrossover(List<Chromosome> population)
    {
        List<Chromosome> newPopulation = new(population);
        for (int i = 0; i < population.Count - 1; i += 2)
        {
            Chromosome parent1 = population[i];
            Chromosome parent2 = population[i + 1];
            Chromosome child = parent1.Crossover(parent2);
            newPopulation.Add(child);
        }

        return newPopulation;
    }

    // Perform mutation to maintain genetic diversity
    private List<Chromosome> PerformMutation(List<Chromosome> population)
    {
        foreach (var schedule in population)
        {
            if (_random.NextDouble() < 0.1) // Mutation rate of 10%
                schedule.Mutate();
        }
        return population;
    }

    // Print the schedule
    private static void PrintSchedule(Chromosome schedule)
    {
        Console.WriteLine(JsonConvert.SerializeObject(schedule.Genes));

        var ordered = schedule.Genes.OrderBy(x => x.CellNumber).ToList();
        for (int i = 0; i < schedule.Genes.Count; i++)
        {
            var gene = ordered[i];
            Console.WriteLine($"Time Slot {i + 1}: Class - {gene.CourseCode}, Teacher - {gene.CourseTeacher}, CellNumber - {gene.CellNumber}, (row, col) = ({gene.CellNumber / 5}, {gene.CellNumber % 5})");
        }
    }
}