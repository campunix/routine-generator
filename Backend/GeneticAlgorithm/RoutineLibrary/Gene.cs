namespace RoutineLibrary;

public class Gene
{
    public string CourseCode { get; set; } = default!;

    public string CourseTeacher { get; set; } = default!;

    public string Semester { get; set; } = default!;

    public int SememsterNumber { get; set; }

    public int CellNumber { get; set; } // 1 - 30

    public Gene(string courseCode, string courseTeacher, string semester, int semesterNumber)
    {
        CourseCode = courseCode;
        CourseTeacher = courseTeacher;
        Semester = semester;
        SememsterNumber = semesterNumber;
    }
}
