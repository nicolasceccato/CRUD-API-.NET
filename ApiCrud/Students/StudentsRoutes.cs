namespace ApiCrud.Students;

public static class StudentsRoutes
{
    public static void AddStudentsRoutes(this WebApplication app)
    {
        app.MapGet("students", () => "Hello Students");
    }
}