using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureTrainingExercise.BusinessLayer;
using AzureTrainingExercise.Models;

namespace AzureTrainingExercise.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase uploadFile)
        {
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }
            // Container Name - picture  
            BlobManager BlobManagerObj = new BlobManager("picture");
            string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);

            return RedirectToAction("Get");
        }

        public ActionResult Get()
        {
            // Container Name - picture  
            BlobManager BlobManagerObj = new BlobManager("picture");
            List<string> fileList = BlobManagerObj.BlobList();
            return View(fileList);
        }

        public ActionResult Delete(string uri)
        {
            // Container Name - picture  
            BlobManager BlobManagerObj = new BlobManager("picture");
            BlobManagerObj.DeleteBlob(uri);
            return RedirectToAction("Get");
        }

        public ActionResult TableIndex(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                // Get particular student info  
                TableManager TableManagerObj = new TableManager("person"); // 'person' is the name of the table  
                                                                           // pass query where RowKey eq value of id
                List<Student> SutdentListObj = TableManagerObj.RetrieveEntity<Student>("RowKey eq '" + id + "'");
                Student StudentObj = SutdentListObj.FirstOrDefault();
                return View(StudentObj);
            }
            return View(new Student());
        }

        [HttpPost]
        public ActionResult TableIndex(string id, FormCollection formData)
        {
            Student StudentObj = new Student();
            StudentObj.Name = formData["Name"] == "" ? null : formData["Name"];
            StudentObj.Department = formData["Department"] == "" ? null : formData["Department"];
            StudentObj.Email = formData["Email"] == "" ? null : formData["Email"];

            // Insert  
            if (string.IsNullOrEmpty(id))
            {
                StudentObj.PartitionKey = "Student";
                StudentObj.RowKey = Guid.NewGuid().ToString();

                TableManager TableManagerObj = new TableManager("person");
                TableManagerObj.InsertEntity<Student>(StudentObj, true);
            }
            // Update  
            else
            {
                StudentObj.PartitionKey = "Student";
                StudentObj.RowKey = id;

                TableManager TableManagerObj = new TableManager("person");
                TableManagerObj.InsertEntity<Student>(StudentObj, false);
            }
            return RedirectToAction("GetTableData");
        }

        public ActionResult GetTableData()
        {
            TableManager TableManagerObj = new TableManager("person");
            List<Student> SutdentListObj = TableManagerObj.RetrieveEntity<Student>();
            return View(SutdentListObj);
        }

        public ActionResult DeleteTableData(string id)
        {
            TableManager TableManagerObj = new TableManager("persons");
            List<Student> SutdentListObj = TableManagerObj.RetrieveEntity<Student>("RowKey eq '" + id + "'");
            Student StudentObj = SutdentListObj.FirstOrDefault();
            TableManagerObj.DeleteEntity<Student>(StudentObj);
            return RedirectToAction("GetTableData");
        }
    }
}