namespace RoutineLibrary;

public class Chromosome
{
    public int TotalSlot = 5;
    public int TotalSemester = 2;

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

    public void CalculateFitness()
    {
        int conflicts = 0;

        for (int i = 0; i < Genes.Count; i++)
        {
            for (int j = i + 1; j < Genes.Count; j++)
            {
                if (Genes[i].CellNumber == Genes[j].CellNumber
                    && (IsSameCourseCode(Genes[i], Genes[j])
                    || IsSameCourseTeacher(Genes[i], Genes[j])
                    || IsSameSemester(Genes[i], Genes[j])))
                {
                    conflicts++;
                }

                if (IsSameSlotOnSameDay(Genes[i], Genes[j])
                    && IsSameCourseTeacher(Genes[i], Genes[j]))
                {
                    conflicts++;
                }
            }
        }

        Fitness = 1.0 / (1 + conflicts);
    }

    private static bool IsSameCourseCode(Gene first, Gene second)
    {
        return first.CourseCode == second.CourseCode;
    }

    private static bool IsSameCourseTeacher(Gene first, Gene second)
    {
        return first.CourseTeacher == second.CourseTeacher;
    }

    private static bool IsSameSemester(Gene first, Gene second)
    {
        return first.Semester == second.Semester;
    }

    private bool IsSameSlotOnSameDay(Gene first, Gene second)
    {
        return IsSameDay(first, second)
            && IsSameSlot(first, second);
    }

    private bool IsSameSlot(Gene first, Gene second)
    {
        return first.CellNumber % TotalSlot == second.CellNumber % TotalSlot;
    }

    private bool IsSameDay(Gene first, Gene second)
    {
        int totalCellInADay = TotalSemester * TotalSlot;
        int cell1Day = first.CellNumber / totalCellInADay;
        int cell2Day = second.CellNumber / totalCellInADay;
        return cell1Day == cell2Day;
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
        int currentSemester = gene.SememsterNumber; // Assume 2-1 == 3 , 4-1 == 1, 3-1 == 2

        int cellNumber = (rand.Next(5) + ((currentSemester - 1) * TotalSlot))
            + (rand.Next(5) * totalCellInADay);

        return cellNumber;
    }
}