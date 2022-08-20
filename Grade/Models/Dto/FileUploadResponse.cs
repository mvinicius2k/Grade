using Grade.Helpers;

namespace Grade.Models.Dto
{
    public class FileUploadResponse
    {
        public string Name { get; set; }
        public UploadResult Result { get; set; }
        public object ResourceResult {get;set;}

        

        
    }
}
