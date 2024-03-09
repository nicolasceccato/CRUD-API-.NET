using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Students;

public static class StudentsRoutes
{
    public static void AddStudentsRoutes(this WebApplication app)
    {
        var studentRoutes = app.MapGroup("students");

        studentRoutes.MapPost("", async (AddStudentRequest request, AppDbContext context, CancellationToken ct) =>
        {
            var exist = await context.Students.AnyAsync(student => student.Name == request.Name, ct);

            if (exist)
            {
                return Results.Conflict("Já existe um estudante com esse nome");
            }

            var newStudent = new Student(request.Name);
            await context.Students.AddAsync(newStudent, ct);
            await context.SaveChangesAsync(ct);

            var studentToReturn = new StudentDTO(newStudent.Id, newStudent.Name);
            
            return Results.Ok(studentToReturn);
        });

        studentRoutes.MapGet("", async (AppDbContext context, CancellationToken ct) =>
        {
            var students = await context
                .Students
                .Where(x => x.Active)
                .Select(x => new StudentDTO(x.Id, x.Name))
                .ToListAsync(ct);
            return students;
        });

        studentRoutes.MapPut("{id}", async (Guid id,UpdateStudentRequest request, AppDbContext context, CancellationToken ct) =>
        {
            var student = await context
                .Students
                .SingleOrDefaultAsync(student => student.Id == id, ct);

            if (student == null)
            {
                return Results.NotFound();
            }
            student.UpdateName(request.Name);
            await context.SaveChangesAsync(ct);
            return Results.Ok(new StudentDTO(student.Id, student.Name));
        });
        studentRoutes.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
        {
            var student = await context
                .Students
                .SingleOrDefaultAsync(x => x.Id == id, ct);

            if (student == null)
            {
                return Results.NotFound();
            }
            
            student.UpdateActive();
            await context.SaveChangesAsync(ct);
            return Results.Ok();
        });
    }
}