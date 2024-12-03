namespace ProniaWebApp.Helpers.Extensions
{
    public static class FileExtensions
    {
        public static string Upload(this IFormFile file, string rootPath, string folderName)
        {
            string fileName = file.FileName;

            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(0, 64);
            }
            fileName = Guid.NewGuid() + file.FileName;

            string path = Path.Combine(rootPath, folderName, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }


        public static bool Delete(string rootPath, string folderName, string fileName)
        {
            string path = Path.Combine(rootPath, folderName, fileName);
            if (!File.Exists(path))
            {
                return false;
            }
            File.Delete(path);
            return true;
        }
    }
}