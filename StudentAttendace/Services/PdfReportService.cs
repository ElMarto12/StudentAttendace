using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Services;

public class PdfReportService
{
     public void GenerateAttendanceReport(string filePath, List<Student> students, Teacher teacher, Subject subject, IEnumerable<Group> groups, string title, IEnumerable<StudentsLecture> studentLectures, IEnumerable<SubjectAttendance> subjectAttendances)
     { 
         using var pdfWriter = new PdfWriter(filePath);
         using var pdfDoc = new PdfDocument(pdfWriter);
         var document = new Document(pdfDoc);

        foreach (var group in groups)
        {
            document.Add(new Paragraph(title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetMarginBottom(20));

            document.Add(new Paragraph($"Destytojas: {teacher.Name} {teacher.LastName}")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12)
                .SetMarginBottom(2));

            document.Add(new Paragraph($"Dalykas: {subject.SubjectName}")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12)
                .SetMarginBottom(2));

            document.Add(new Paragraph($"Grupe: {group.GroupName}")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12)
                .SetMarginBottom(2));
            
            float[] columnWidths = { 1, 3, 3, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths))
                .SetWidth(UnitValue.CreatePercentValue(100));

            // Lentelės antraštės
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Nr."))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Vardas pavarde"))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Lankyta paskaitu"))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Procentai"))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));

            var groupStudents = students.Where(gs => gs.GroupId == group.GroupID);
            int rowNumber = 1;

            foreach (var student in groupStudents)
            {
                var studentSubjectAttendance = subjectAttendances.FirstOrDefault(sa => sa.StudentId == student.StudentID);
                
                var studentLecturesForGroup = studentLectures
                    .Where(sl => sl.StudentId == student.StudentID);

                int attendedCount = studentLecturesForGroup
                    .Count(sl => sl.IsParticipating);
                
                table.AddCell(new Cell().Add(new Paragraph(rowNumber.ToString())));
                table.AddCell(new Cell().Add(new Paragraph($"{student.Name} {student.LastName}")));
                table.AddCell(new Cell().Add(new Paragraph($"{attendedCount}/{subject.LectureNumber}")));

                table.AddCell(new Cell().Add(new Paragraph($"{studentSubjectAttendance.AttendancePercentage}")));
                
                rowNumber++;
                
            }
            
            document.Add(table);

            document.Add(new Paragraph("Ataskaita sugeneruota: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(10));  

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        document.Close();
     }

     public void GenerateStudentAttendanceReportByTeacher(string filePath, Teacher teacher, Student student, IEnumerable<Subject> subjects,  Group group, string title, IEnumerable<StudentsLecture> studentLectures, IEnumerable<SubjectAttendance> subjectAttendances, IEnumerable<Lecture> lectures) {
        
         using var pdfWriter = new PdfWriter(filePath);
    using var pdfDoc = new PdfDocument(pdfWriter);
    var document = new Document(pdfDoc);

    document.Add(new Paragraph(title)
        .SetTextAlignment(TextAlignment.CENTER)
        .SetFontSize(18)
        .SetMarginBottom(20));

    document.Add(new Paragraph($"Dėstytojas: {teacher.Name} {teacher.LastName}")
        .SetTextAlignment(TextAlignment.LEFT)
        .SetFontSize(12)
        .SetMarginBottom(2));

    document.Add(new Paragraph($"Studentas: {student.Name} {student.LastName}")
        .SetTextAlignment(TextAlignment.LEFT)
        .SetFontSize(12)
        .SetMarginBottom(2));

    foreach (var subject in subjects)
    {
        if (subject.TeacherId == teacher.TeacherID)
        {
            document.Add(new Paragraph($"Dalykas: {subject.SubjectName}")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(14)
                .SetMarginBottom(5));

            var studentLecturesForSubject = studentLectures
                .Where(sl => sl.StudentId == student.StudentID)
                .Join(
                    lectures,
                    sl => sl.LectureId,
                    l => l.LectureId,
                    (sl, l) => new { sl, l })
                .Where(slLecture => slLecture.l.SubjectId == subject.SubjectId)
                .Select(slLecture => slLecture.sl);

            var studentSubjectAttendance = subjectAttendances
                .FirstOrDefault(sa => sa.StudentId == student.StudentID && sa.SubjectId == subject.SubjectId);

            float[] columnWidths = { 1, 3, 3, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths))
                .SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Nr."))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Vardas pavardė"))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Lankyta paskaitų"))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            table.AddHeaderCell(new Cell()
                .Add(new Paragraph("Procentai"))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER));

            int attendedCount = studentLecturesForSubject.Count(sl => sl.IsParticipating);

            table.AddCell(new Cell().Add(new Paragraph("1")));
            table.AddCell(new Cell().Add(new Paragraph($"{student.Name} {student.LastName}")));
            table.AddCell(new Cell().Add(new Paragraph($"{attendedCount}/{subject.LectureNumber}")));
            table.AddCell(new Cell().Add(new Paragraph($"{studentSubjectAttendance?.AttendancePercentage ?? 0}")));

            document.Add(table);
        }
    }

    document.Add(new Paragraph("Ataskaita sugeneruota: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
        .SetTextAlignment(TextAlignment.RIGHT)
        .SetFontSize(10));

    document.Close();
    }   
}