using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Personal.Domain.Helpers
{
    public interface IFileHelper
    {
        void DeleteFile(string destination_folder, string file_name);
        string GetFileName(IFormFile file, string file_prefix = "");
        bool IsImageValid(string file_name);
        Task<string> SaveImageAndGetFileName(IFormFile file,string destination_folder,string file_prefix="");
        string MoveImageFromTempPathToDestination(string image_name,string destination_folder);
        bool IsExcelFileValid(string file_name);
        void ImageResize(string input_image_path,string output_image_path,int new_width);
        bool IsImageSizeLessThan1Mb(IFormFile file);
    }
}
