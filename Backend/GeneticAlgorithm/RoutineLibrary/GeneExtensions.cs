namespace RoutineLibrary;

public static class GeneExtensions
{
    public static bool HasSameCourseCodeOf(this Gene first, Gene second)
    {
        return first.CourseCode == second.CourseCode;
    }

    public static bool HasSameCourseTeacherOf(this Gene first, Gene second)
    {
        return first.CourseTeacher == second.CourseTeacher;
    }

    public static bool HasSameSemesterOf(this Gene first, Gene second)
    {
        return first.Semester == second.Semester;
    }

    public static bool IsInSameSlotOnSameDay(this Gene first, Gene second, int totalSlot, int totalSemester)
    {
        return IsInSameDay(first, second, totalSlot, totalSemester)
            && first.IsInSameSlotOf(second, totalSlot);
    }

    public static bool IsInPreviousSlotOnSameDay(this Gene first, Gene second, int totalSlot, int totalSemester)
    {
        return IsInSameDay(first, second, totalSlot, totalSemester)
            && first.IsInPreviousSlotOf(second, totalSlot);
    }

    public static bool IsInNextSlotOnSameDay(this Gene first, Gene second, int totalSlot, int totalSemester)
    {
        return IsInSameDay(first, second, totalSlot, totalSemester)
            && first.IsInPreviousSlotOf(second, totalSlot);
    }

    private static bool IsInSameSlotOf(this Gene first, Gene second, int totalSlot)
    {
        return first.CellNumber % totalSlot == second.CellNumber % totalSlot;
    }

    private static bool IsInPreviousSlotOf(this Gene first, Gene second, int totalSlot)
    {
        return first.CellNumber % totalSlot == (second.CellNumber - 1) % totalSlot;
    }

    private static bool IsInNextSlotOf(this Gene first, Gene second, int totalSlot)
    {
        return first.CellNumber % totalSlot == (second.CellNumber + 1) % totalSlot;
    }

    private static bool IsInSameDay(this Gene first, Gene second, int totalSlot, int totalSemester)
    {
        int totalCellInADay = totalSlot * totalSemester;
        int cell1Day = first.CellNumber / totalCellInADay;
        int cell2Day = second.CellNumber / totalCellInADay;
        return cell1Day == cell2Day;
    }
}
