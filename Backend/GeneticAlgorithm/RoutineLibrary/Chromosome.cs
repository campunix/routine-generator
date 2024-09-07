using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineLibrary;

public class Chromosome
{
    public List<Gene> Genes { get; set; }

    public double Fitness { get; set; }

    private static readonly Random rand = new();

    public Chromosome()
    {
        Genes = [];
    }

    public Chromosome(List<Gene> availableGenes)
    {
        Genes ??= [];
        foreach (var gene in availableGenes)
        {
            // Can be initialize different way

            //(2 + (3-1 * 5) = 12
            //(4 * 4 * 5) = 80

            gene.CellNumber = CalculateCellNumber(gene);
            Genes.Add(gene);
            //Console.WriteLine(JsonConvert.SerializeObject(gene));
        }
    }

    public void CalculateFitness()
    {
        int conflicts = 0;

        for (int i = 0; i < Genes.Count; i++)
        {
            for (int j = i + 1; j < Genes.Count; j++)
            {
                if (Genes[i].CellNumber != Genes[j].CellNumber)
                {
                    continue;
                }

                if (Genes[i].CourseCode == Genes[j].CourseCode
                    || Genes[i].CourseTeacher == Genes[j].CourseTeacher
                    || Genes[i].Semester == Genes[i].Semester)
                {
                    conflicts++;
                }
            }
        }

        Fitness = 1.0 / (1 + conflicts);
    }

    public Chromosome Crossover(Chromosome other)
    {
        var child = new Chromosome();

        for (int i = 0; i < Genes.Count; i++)
        {
            var gene = rand.NextDouble() < 0.5 ? Genes[i] : other.Genes[i];
            child.Genes.Add(gene);
        }

        return child;
    }

    public void Mutate()
    {
        int index = rand.Next(Genes.Count);
        Genes[index].CellNumber = CalculateCellNumber(Genes[index]);
    }

    private static int CalculateCellNumber(Gene gene)
    {
        int totalSlot = 5;
        int totalSemester = 2;
        int currentSemester = gene.SememsterNumber; // Assume 2-1 == 3 , 4-1 == 1, 3-1 == 2

        int cellNumber = (rand.Next(5) + ((currentSemester - 1) * totalSlot))
            + (rand.Next(5) * totalSemester * totalSlot);

        return cellNumber;
    }
}