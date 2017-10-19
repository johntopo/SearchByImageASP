using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using retrieveImageFeatures.UtilitiesClasses;
using retrieveImageFeatures.UtilitiesClasses.Database;
using Image = System.Drawing.Image;

namespace retrieveImageFeatures
{
    public partial class MainPage : Page
    {
        private const string PathSavedImagesDb = @"/dbImages/";
        private const string PathTempImages = @"/tempImages/";
        private const string TableName = "imagesdescriptortbl";
        private const double UsersImageWeight = 0.6;
        private readonly string[] _columns = {"name", "descriptor"};

        private readonly SqlQuerriesAdapter _adapterQuerries = new SqlQuerriesAdapter();
        private readonly SqlServerSetUp _sqlServer = new SqlServerSetUp();

        private readonly UtilityDescriptor _utilityDescriptor  = new UtilityDescriptor();

        protected void Page_Load(object sender, EventArgs e)
        {
            //force page_load when user pick a file with uploadfile through js 
            if (IsPostBack && fuSearch.PostedFile != null)
            {
                if (fuSearch.PostedFile.FileName.Length > 0)
                {
                    SearchImage();
                }
            }
            if (IsPostBack && fuUploadImagesToDb.PostedFile != null)
            {
                if (fuUploadImagesToDb.PostedFile.FileName.Length > 0)
                {
                    UpdateDb();
                    imageCheckBoxList.Items.Clear();
                }
            }
        }


        private void ShowMostSimilarImages(NpgsqlDataAdapter da)
        {
            _sqlServer.ds.Reset();


            da.Fill(_sqlServer.ds);
            //get table with similar images and descriptors
            _sqlServer.dt = _sqlServer.ds.Tables[0];


            //convert table to list name and descriptor in class RecordFromdt.
            List<DtRecord> recordList = new List<DtRecord>();
            for (int j = 0; j < _sqlServer.dt.Rows.Count; j++)
            {
                DtRecord dtRecord = new DtRecord();
                dtRecord.descriptor = DtRecord.ConvertFloatToDouble((float[]) _sqlServer.dt.Rows[j]["descriptor"]);
                dtRecord.name = _sqlServer.dt.Rows[j][_columns[0]].ToString();
                recordList.Add(dtRecord);
            }

            //show all images
            foreach (DtRecord record in recordList)
            {
                string tempNameImage = PathSavedImagesDb + record.name;

                ListItem listItem = new ListItem(string.Format("<img src='{0}' alt='{0}' />", tempNameImage),
                    tempNameImage);
                listItem.Attributes.Add("class", "col-xs-12 col-sm-6 col-md-4 col-lg-3");
                imageCheckBoxList.Items.Add(listItem);
            }
        }


        private void InitializeVisibilityOfElements()
        {
            imageCheckBoxList.Items.Clear();
            buttonResearch.Visible = false;
            imageUploadedByUser.ImageUrl = null;
        }


        protected void buttonResearch_Click(object sender, EventArgs e) // feedback new images button.
        {
            bool userSelectedImages = true;
            string sqlQuery = "";
            int numberOfImages = int.Parse(dplNumOfImagesToReturn.SelectedValue);

            List<string> namesImagesResearch = imageCheckBoxList.Items.Cast<ListItem>()
                .Where(li => li.Selected)
                .Select(li => li.Value)
                .ToList();

            imageCheckBoxList.Items.Clear();
            if (namesImagesResearch.Count == 0)
            {
                userSelectedImages = false;
            }


            double[] userImageDescriptor =
                _utilityDescriptor.GetCeddDescriptorForImage(Image.FromFile(Server.MapPath(imageUploadedByUser.ImageUrl)));
            if (userSelectedImages)
            {
                double[] researchDescriptor = GetImagesResearchDescriptor(namesImagesResearch);


                //caclulate final descriptor with weight
                for (int i = 0; i < researchDescriptor.Length; i++)
                {
                    researchDescriptor[i] =
                        Math.Round(UsersImageWeight*userImageDescriptor[i] +
                                   (1 - UsersImageWeight)*researchDescriptor[i]);
                }
                sqlQuery = _adapterQuerries.SelectByDistance(TableName, _columns[0], _columns[1],
                    researchDescriptor,
                    numberOfImages);
            }
            else
            {
                sqlQuery = _adapterQuerries.SelectByDistance(TableName, _columns[0], _columns[1],
                    userImageDescriptor,
                    numberOfImages);
            }
            _sqlServer.OpenConnection();


            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlQuery, _sqlServer.connection);

            ShowMostSimilarImages(da);

            _sqlServer.CloseConnection();

            ShowImagesUsedInResearch(namesImagesResearch);
        }

        private void ShowImagesUsedInResearch(List<string> images)
        {
            placeholderResearchImages.Controls.Clear();
            foreach (var image in images)
            {
                var im = new System.Web.UI.WebControls.Image();
                im.ImageUrl = image;
                im.Height = 100;
                placeholderResearchImages.Controls.Add(im);
            }
        }

        private double[] GetImagesResearchDescriptor(List<string> imagesSelectedByUser)
        {
            double[] resultArray = null;
            foreach (string name in imagesSelectedByUser)
            {
                if (resultArray == null)
                {
                    resultArray = _utilityDescriptor.GetCeddDescriptorForImage(Image.FromFile(Server.MapPath(name)));
                }
                else
                {
                    var ceddDiscriptor =
                        _utilityDescriptor.GetCeddDescriptorForImage(Image.FromFile(Server.MapPath(name)));
                    for (int i = 0; i < resultArray.Length; i++)
                    {
                        resultArray[i] = resultArray[i] + ceddDiscriptor[i];
                    }
                }
            }

            //calculate mean vector
            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray[i] = resultArray[i]/imagesSelectedByUser.Count;
            }
            return resultArray;
        }


        private void UpdateDb()
        {
            //open connection with server
            _sqlServer.OpenConnection();

            if (fuUploadImagesToDb.HasFile
                /*&&  (fileupload.FileName.Contains(".JPG") || fileupload.FileName.Contains(".jpg"))*/)
            {
                FileHandler fileHandler = new FileHandler();
                foreach (HttpPostedFile hfc in fuUploadImagesToDb.PostedFiles)
                {
                    //use savefile for every image from httpfilecollection.

                    fileHandler.hpf = hfc;
                    string newFileName = FileHandler.GetUniqueFileName(Server.MapPath(PathSavedImagesDb), fileHandler.hpf.FileName);
                    fileHandler.SaveAs(Server.MapPath(PathSavedImagesDb) + newFileName);
                    //now use ceddDisriptor for image
                    double[] ceddDiscriptor = _utilityDescriptor.GetCeddDescriptorForImage(
                        Image.FromFile(Server.MapPath(PathSavedImagesDb + newFileName)));


                    //create query with descriptor for database
                    string sqlQuery = _adapterQuerries.InsertInto(TableName, _columns, newFileName, ceddDiscriptor);


                    // data adapter making request from our connection
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlQuery, _sqlServer.connection);
                    _sqlServer.ds.Reset();
                    // filling DataSet with result from NpgsqlDataAdapter
                    da.Fill(_sqlServer.ds);
                }
            }
            //close connection with server
            _sqlServer.CloseConnection();
            buttonResearch.Visible = false;
        }


        private void SearchImage()
        {
            InitializeVisibilityOfElements();

            //take number of similar images to find
            int numberOfMostSimilarImages = int.Parse(dplNumOfImagesToReturn.SelectedValue);
            //create user's image derciptor
            double[] ceddDescriptor = _utilityDescriptor.GetCeddDescriptorForImage(fuSearch.FileContent);

            //save file and show user's image
            fuSearch.SaveAs(Server.MapPath(PathTempImages) + fuSearch.FileName);
            imageUploadedByUser.ImageUrl = PathTempImages + fuSearch.FileName;
            //open server connection
            _sqlServer.OpenConnection();
            //create query for similar images
            string sqlQuery = _adapterQuerries.SelectByDistance(TableName, _columns[0], _columns[1], ceddDescriptor,
                numberOfMostSimilarImages);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlQuery, _sqlServer.connection);
            ShowMostSimilarImages(da);
            _sqlServer.CloseConnection();
            buttonResearch.Visible = true;
        }
    }
}