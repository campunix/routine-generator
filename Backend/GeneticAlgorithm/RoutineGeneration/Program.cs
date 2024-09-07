using RoutineLibrary;

namespace RoutineGeneration;

public class Program
{
    static void Main(string[] args)
    {
        var generator = new RoutineGenerator();
        generator.Generate();
    }
}