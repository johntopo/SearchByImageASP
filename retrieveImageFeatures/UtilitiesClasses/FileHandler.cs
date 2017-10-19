using System.IO;
using System.Web;

namespace retrieveImageFeatures.UtilitiesClasses
{
    /// <summary>
    ///     Summary description for GetUniqueFileName
    /// </summary>
    public class FileHandler
    {

        public HttpPostedFile hpf {  get; set; }


        public bool SaveAs(string fileName)
        {
            if (hpf == null)
            {
                return false;
            }

            try
            {
                hpf.SaveAs(fileName);
                return true;
            }
            catch (HttpException e)
            {
                //sth went wrong
            
            }
            return false;
        }

        // use path,filename to check for dublicates and change the name from name->name_1 or name_2 etc.
        //So, ensure that name is correct and then save file.
        public static string GetUniqueFileName(string savePath,string fileName)
        {
     
            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath + fileName;

            // Create a temporary file name to use for checking duplicates.
            string tempfileName = "";

            // Check to see if a file already exists with the
            // same name as the file to upload.        
            if (File.Exists(pathToCheck))
            {
                int counter = 2;
                while (File.Exists(pathToCheck))
                {
                    // if a file with this name already exists,
                    // prefix the filename with a number.
                    tempfileName = Path.GetFileNameWithoutExtension(fileName) + "_" + counter + Path.GetExtension(fileName);
                    pathToCheck = savePath + tempfileName;
                    counter++;
                }

                fileName = tempfileName;

            
            }

            // Append the name of the file to upload to the path.
            
        

            
            return fileName;
        
        
        


        
        }
    }
}