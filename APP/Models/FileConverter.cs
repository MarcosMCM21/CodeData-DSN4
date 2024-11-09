namespace CodeData_Connection.Models
{
    public class FileConverter
    {
        public static async Task<string> ConvertIFormFileToBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                Console.WriteLine("IFORMFILE: " + Convert.ToBase64String(fileBytes));

                return Convert.ToBase64String(fileBytes);
            }
        }
    }
          
}
