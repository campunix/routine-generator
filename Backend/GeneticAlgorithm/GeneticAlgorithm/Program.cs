namespace GeneticAlgorithm;

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Random rand = new Random();

    // Define class, teacher, and room objects
    static List<string> classes = new List<string> { "Math", "Science", "English", "History", "PE" };
    static List<string> teachers = new List<string> { "Teacher A", "Teacher B", "Teacher C" };
    static List<string> rooms = new List<string> { "Room 101", "Room 102" };
    static int totalTimeSlots = 20; // 5 days, 4 periods per day

    static void Main(string[] args)
    {
        // Initialize population
        List<Schedule> population = InitializePopulation(10);

        int generation = 0;
        while (generation < 1000) // max generations
        {
            // Evaluate fitness
            foreach (var schedule in population)
                schedule.CalculateFitness();

            // Sort population by fitness
            population = population.OrderByDescending(s => s.Fitness).ToList();

            // If best solution found, break
            if (population[0].Fitness == 1.0)
            {
                Console.WriteLine("Optimal schedule found:");
                PrintSchedule(population[0]);
                break;
            }

            // Selection
            List<Schedule> newPopulation = SelectBestPopulation(population);

            // Crossover
            newPopulation = PerformCrossover(newPopulation);

            // Mutation
            newPopulation = PerformMutation(newPopulation);

            population = newPopulation;
            generation++;
        }

        Console.WriteLine("Final schedule after generations:");
        PrintSchedule(population[0]);
    }

    // Initialize population with random schedules
    static List<Schedule> InitializePopulation(int size)
    {
        List<Schedule> population = new List<Schedule>();
        for (int i = 0; i < size; i++)
            population.Add(new Schedule(totalTimeSlots, classes, teachers, rooms));
        return population;
    }

    // Select best individuals for the next generation
    static List<Schedule> SelectBestPopulation(List<Schedule> population)
    {
        return population.Take(5).ToList(); // Select top 5 schedules
    }

    // Perform crossover between schedules
    static List<Schedule> PerformCrossover(List<Schedule> population)
    {
        List<Schedule> newPopulation = new List<Schedule>(population);
        for (int i = 0; i < population.Count - 1; i += 2)
        {
            Schedule parent1 = population[i];
            Schedule parent2 = population[i + 1];
            Schedule child = parent1.Crossover(parent2, classes, teachers, rooms);
            newPopulation.Add(child);
        }
        return newPopulation;
    }

    // Perform mutation to maintain genetic diversity
    static List<Schedule> PerformMutation(List<Schedule> population)
    {
        foreach (var schedule in population)
        {
            if (rand.NextDouble() < 0.1) // Mutation rate of 10%
                schedule.Mutate();
        }
        return population;
    }

    // Print the schedule
    static void PrintSchedule(Schedule schedule)
    {
        for (int i = 0; i < schedule.Genome.Count; i++)
        {
            var gene = schedule.Genome[i];
            Console.WriteLine($"Time Slot {i + 1}: Class - {gene.Class}, Teacher - {gene.Teacher}, Room - {gene.Room}");
        }
    }
}

// Schedule class represents a chromosome in the population
class Schedule
{
    public List<Gene> Genome { get; private set; }

    public double Fitness { get; private set; }

    private static readonly Random rand = new Random();

    public Schedule(int totalTimeSlots, List<string> classes, List<string> teachers, List<string> rooms)
    {

        Genome = new List<Gene>();
        for (int i = 0; i < totalTimeSlots; i++)
        {
            Genome.Add(new Gene
            {
                Class = classes[rand.Next(classes.Count)],
                Teacher = teachers[rand.Next(teachers.Count)],
                Room = rooms[rand.Next(rooms.Count)],
                TimeSlot = i % 6,
            });
        }
    }

    public void CalculateFitness()
    {
        int conflicts = 0;
        for (int i = 0; i < Genome.Count; i++)
        {
            for (int j = i + 1; j < Genome.Count; j++)
            {
                if (Genome[i].TimeSlot == Genome[j].TimeSlot)
                {
                    if (Genome[i].Teacher == Genome[j].Teacher) conflicts++;
                    if (Genome[i].Room == Genome[j].Room) conflicts++;
                }
            }
        }

        Fitness = 1.0 / (1 + conflicts); // Fitness is inversely proportional to the number of conflicts
    }

    public Schedule Crossover(Schedule other, List<string> classes, List<string> teachers, List<string> rooms)
    {
        Schedule child = new Schedule(Genome.Count, classes: classes, teachers: teachers, rooms: rooms);
        for (int i = 0; i < Genome.Count; i++)
        {
            child.Genome[i] = rand.NextDouble() < 0.5 ? this.Genome[i] : other.Genome[i];
        }
        return child;
    }

    public void Mutate()
    {
        int index = rand.Next(Genome.Count);
        Genome[index] = new Gene
        {
            Class = Genome[index].Class,
            Teacher = Genome[index].Teacher,
            Room = Genome[index].Room,
            TimeSlot = rand.Next(6),
        };
    }
}

// Gene class represents a single gene in a chromosome (schedule)
class Gene
{
    public string Class { get; set; }
    public string Teacher { get; set; }
    public string Room { get; set; }
    public int TimeSlot { get; set; }
}
