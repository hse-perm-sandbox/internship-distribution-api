using InternshipDistribution.Models;

namespace InternshipDistribution.Services
{
    public class FileStorageService
    {
        private readonly string _storagePath;
        private readonly string[] _allowedExtensions;
        private readonly long _maxFileSize;

        public FileStorageService(IConfiguration config)
        {
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(),config["FileStorage:ResumePath"]);

            _allowedExtensions = config.GetSection("FileStorage:AllowedExtensions").Get<string[]>();

            _maxFileSize = config.GetValue<long>("FileStorage:MaxFileSizeMB") * 1024 * 1024;

            Directory.CreateDirectory(_storagePath);
        }

        public async Task<string> SaveResume(IFormFile file, Student student)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException($"Недопустимый формат файла, допустимые форматы: {string.Join(", ", _allowedExtensions)}");

            if (file.Length > _maxFileSize)
                throw new InvalidOperationException($"Максимальный размер файла: {_maxFileSize / 1024 / 1024}MB");

            // Генерация имени файла
            var fileName = GenerateResumeName(student);
            var filePath = Path.Combine(_storagePath, fileName);

            var directory = Path.GetDirectoryName(filePath);

            if (directory == null)
                throw new InvalidOperationException("Директория для сохранения файла = null");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public FileStream GetResume(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public bool DeleteResume(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var filePath = Path.Combine(_storagePath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public string GenerateResumeName(Student student)
        {
            // Очистка имени от запрещенных символов
            var cleanFirstName = SanitizeFileName(student.Name);
            var cleanLastName = SanitizeFileName(student.Lastname);

            return $"{cleanFirstName}_{cleanLastName}_{student.Id}_resume.pdf";
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries))
                .Replace(" ", "_")
                .ToLower();
        }
    }
}
