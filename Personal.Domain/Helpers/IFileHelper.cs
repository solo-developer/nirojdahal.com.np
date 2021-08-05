using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Personal.Domain.Helpers
{
    public interface IFileHelper
    {
        void deleteFile(string destination_folder, string file_name);
        string getFileName(IFormFile file, string file_prefix = "");
        bool isImageValid(string file_name);
        Task<string> saveImageAndGetFileName(IFormFile file,string destination_folder,string file_prefix="");
        string moveImageFromTempPathToDestination(string image_name,string destination_folder);
        bool isExcelFileValid(string file_name);
        void imageResize(string input_image_path,string output_image_path,int new_width);
        bool isImageSizeLessThan1Mb(IFormFile file);
    }
}
