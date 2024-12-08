using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Services;

public class PdfReportService
{
     public void GenerateAttendanceReport(string filePath, List<Student> students, Teacher teacher, Subject subject, IEnumerable<Group> groups, string title)
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

            
             float[] columnWidths = new float[3 + subject.LectureNumber + 1]; 


             columnWidths[0] = 1; // "Nr."
             columnWidths[1] = 3; // "Vardas pavarde"
             columnWidths[2] = 1; // "Paskaitos"


             for (int i = 3; i < 3 + subject.LectureNumber; i++)
             {
                 columnWidths[i] = 1; 
             }
             
             columnWidths[^1] = 1;
             
             Table table = new Table(UnitValue.CreatePercentArray(columnWidths))
                 .SetWidth(UnitValue.CreatePercentValue(100));

             List<string> headers = new List<string> { "Nr.", "Vardas pavarde", "Paskaitos" };
             
             for (int i = 1; i <= subject.LectureNumber; i++)
             {
                 headers.Add(i.ToString());
             }
             
             headers.Add("Procentai");
             
             foreach (var header in headers)
             {
                 table.AddHeaderCell(new Cell()
                     .Add(new Paragraph(header))
                     .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                     .SetTextAlignment(TextAlignment.CENTER));
             }

             var groupStudents = students.Where(gs => gs.GroupId == group.GroupID);
             
             int rowNumber = 1;
             
             foreach (var student in groupStudents)
             {
                 table.AddCell(new Cell().Add(new Paragraph(rowNumber.ToString())));
                 table.AddCell(new Cell().Add(new Paragraph($"{student.Name} {student.LastName}")));
                 table.AddCell(new Cell().Add(new Paragraph("")));
                 
                 for(int i = 0; i < subject.LectureNumber; i++)
                 {
                    table.AddCell(new Cell().Add(new Paragraph("Taip")));
                 }

                 table.AddCell(new Cell().Add(new Paragraph("30%")));
                 
                 rowNumber++;
             }

             document.Add(table);
             
             document.Add(new Paragraph("Ataskaita sugeneruota: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
                 .SetTextAlignment(TextAlignment.RIGHT)
                 .SetFontSize(10));
             
             document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
         }
         
         document.Close();
     }
}