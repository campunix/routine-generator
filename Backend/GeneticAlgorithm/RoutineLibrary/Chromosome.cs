namespace RoutineLibrary;

public class Chromosome
{
    public int TotalSlot = 5;
    public int TotalSemester = 2;
    public int Conflicts = 0;

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

            gene.CellNumber = CalculateCellNumber(gene);
            Genes.Add(gene);
            //Console.WriteLine(JsonConvert.SerializeObject(gene));
        }
    }

    public void CalculateFitness(bool showConflicts)
    {
        int conflicts = 0;

        for (int i = 0; i < Genes.Count; i++)
        {
            for (int j = 0; j < Genes.Count; j++)
            {
                if (i == j) continue;

                if (Genes[i].CellNumber == Genes[j].CellNumber)
                {
                    conflicts++;
                }

                if ((Genes[i].IsLab && Genes[i].IsLastSlot(TotalSlot))
                    || (Genes[j].IsLab && Genes[j].IsLastSlot(TotalSlot)))
                {
                    conflicts++;
                }

                if (Genes[i].HasSameCourseTeacherOf(Genes[j])
                    && Genes[i].IsInSameSlotOnSameDayOf(Genes[j], TotalSlot, TotalSemester))
                {
                    conflicts++;
                }

                if (Genes[i].IsLab
                    && Genes[i].HasSameCourseTeacherOf(Genes[j])
                    && Genes[i].IsInPreviousSlotOnSameDayOf(Genes[j], TotalSlot, TotalSemester))
                {
                    conflicts++;
                }

                // TODO: Room specific constraint will be added in the following condition
                if (Genes[i].IsLab
                    && Genes[i].HasSameSemesterOf(Genes[j])
                    && Genes[i].IsInPreviousSlotOnSameDayOf(Genes[j], TotalSlot, TotalSemester))
                {
                    conflicts++;
                    if (showConflicts)
                    {
                        Console.WriteLine("No empty slot after lab");
                    }
                }
            }
        }

        Conflicts = conflicts;
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

    private int CalculateCellNumber(Gene gene)
    {
        int totalCellInADay = TotalSemester * TotalSlot;
        int currentSemester = gene.SememsterNumber;

        int cellNumber = (rand.Next(5) + ((currentSemester - 1) * TotalSlot))
            + (rand.Next(5) * totalCellInADay);

        return cellNumber;
    }
}