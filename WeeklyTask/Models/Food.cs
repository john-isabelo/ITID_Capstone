using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WeeklyTask.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }

        public void SaveImage(string uploadsFolderPath)
        {
            if (ImageFile != null)
            {
                // Delete the previous image file, if it exists.
                if (ImagePath != null)
                {
                    var imagePath = Path.Combine(uploadsFolderPath, ImagePath);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                // Generate a unique file name for the new image.
                ImagePath = GetUniqueFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolderPath, ImagePath);

                // Save the new image file to the server.
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                + "_"
                + System.Guid.NewGuid().ToString().Substring(0, 4)
                + Path.GetExtension(fileName);
        }
    }

}
