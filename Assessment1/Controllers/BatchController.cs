using Assessment1.Models;
using Assessment1.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Assessment1.Data;
using Assessment1.BAL.IBusiness;
using NLog;
using Microsoft.Extensions.Logging;
using System.Reflection;
using NLog.Web;
using Assessment1.Validation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Assessment1.ConcreteClass;

namespace Assessment1.Controllers
{
    [Route("api/{controller}/{action}/{id?}")]
    [ApiController]
    public class BatchController : ControllerBase
    {

        private readonly ApplicationDBContext _db;
        private readonly IBatchBusiness _objbatchBusiness;
        private Logger _logger = NLog.LogManager.Setup().GetCurrentClassLogger();
        public BatchController(ApplicationDBContext db, IBatchBusiness objbatchBusiness)
        {
            _db = db;
            _objbatchBusiness = objbatchBusiness;
        }

        [HttpPost]
        public void Create(dynamic dynamic)
        {

        }
        [HttpPost]
        [Route("/batch")]
        public IActionResult CreateNewBatch(BatchVM batchvm)
        {
            Error objerror = new Error();
            List<errors> lsterrors = new List<errors>();
            objerror.correlationId = Guid.NewGuid().ToString();
            try
            {
                if (batchvm == null)
                {
                    lsterrors.Add(new errors { source = "Batch", description = "Invalid Data!!!" });
                    objerror.errors = lsterrors.ToArray();
                    _logger.Info($"{MethodBase.GetCurrentMethod().DeclaringType.FullName}, {JsonConvert.SerializeObject(objerror)}");
                    return BadRequest(objerror);
                }

                var validator = new BatchVMValidator();
                var result = validator.Validate(batchvm);

                if (!result.IsValid)
                {
                    lsterrors.AddRange(result.Errors.Select(failure => new errors { source = failure.PropertyName, description = failure.ErrorMessage }));
                    objerror.errors = lsterrors.ToArray();
                    return BadRequest(objerror);
                }

                if (!string.IsNullOrWhiteSpace(batchvm.BusinessUnit))
                {
                    int businessunitId = _objbatchBusiness.getBusinessUnitID(batchvm.BusinessUnit);
                    if (businessunitId == 0)
                    {
                        lsterrors.Add(new errors { source = "Batch", description = "BusinessUnit doesnt exist!!!" });
                        objerror.errors = lsterrors.ToArray();
                        _logger.Info($"{MethodBase.GetCurrentMethod().DeclaringType.FullName}, {JsonConvert.SerializeObject(objerror)}");
                        return NotFound(objerror);
                    }
                    batchvm.BusinessUnitId = businessunitId;
                    batchvm.BatchId = _objbatchBusiness.insertBatchData(batchvm);

                    if (string.IsNullOrWhiteSpace(batchvm.BatchId))
                    {
                        lsterrors.Add(new errors { source = "Batch", description = "Invalid data" });
                        objerror.errors = lsterrors.ToArray();
                        _logger.Info($"{MethodBase.GetCurrentMethod().DeclaringType.FullName}, {JsonConvert.SerializeObject(objerror)}");
                        return BadRequest(objerror);
                    }
                    return Ok(new { batchId = batchvm.BatchId });

                }
                else
                {
                    lsterrors.Add(new errors { source = "Batch", description = "BusinessUnit required!!!" });
                    objerror.errors = lsterrors.ToArray();
                    _logger.Warn($"{MethodBase.GetCurrentMethod().DeclaringType.FullName} BusinessUnit required!!!");
                    return BadRequest(objerror);
                }

            }
            catch (Exception ex)
            {

                lsterrors.Add(new errors { source = "Batch", description = "Bad Request!!!" });
                objerror.errors = lsterrors.ToArray();
                _logger.Error(ex, $"Error Occured in {MethodBase.GetCurrentMethod().DeclaringType.FullName}");
                return NotFound(objerror);
            }
            return Ok(new { batchId = batchvm.BatchId });
        }

        [HttpGet]
        [Route("/batch/{batchId}")]
        public IActionResult FindBatch(string batchId)
        {
            Error objerror = new Error();
            List<errors> lsterrors = new List<errors>();
            objerror.correlationId = Guid.NewGuid().ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(batchId))
                {
                    lsterrors.Add(new errors { source = "Batch", description = "Please provide BatchId" });
                    objerror.errors = lsterrors.ToArray();
                    return NotFound(objerror);
                }
                BatchVM batchVM = _objbatchBusiness.GetBatchVMById(batchId);
                var attribute = batchVM.attributes.Select(x => new { x.key, x.value });
                return Ok(new
                {
                    batchId = batchVM.BatchId,
                    businessUnit = batchVM.BusinessUnit,
                    batchPublishedDate = batchVM.batchpublishedDate,
                    expiryDate = batchVM.expiryDate,
                    status = "Complete",
                    attributes = attribute,
                    acl = batchVM.acl
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Batch Not found")
                    lsterrors.Add(new errors { source = "Batch", description = ex.Message });
                else
                    lsterrors.Add(new errors { source = "Batch", description = "Bad Request" });

                objerror.errors = lsterrors.ToArray();
                _logger.Error(ex, $"Error Occured in {MethodBase.GetCurrentMethod().DeclaringType.FullName} :: batchId : {batchId}");

                return BadRequest(objerror);
            }
            return NotFound(objerror);
        }

        [HttpPost]
        [Route("/batch/{batchId}/{filename}")]
        public IActionResult FileUpload(string batchId, string filename)
        {
            Error objerror = new Error();
            List<errors> lsterrors = new List<errors>();
            objerror.correlationId = Guid.NewGuid().ToString();
            try
            {
                Common common = new Common();
                string connectionString = common.getConnectionStringFromSecretForFileUploadContainer();
                //string connectionString = @"DefaultEndpointsProtocol=https;AccountName=assesmentonestorageacc;AccountKey=Y4p+JFXbk1Q7U6vzdAqPS4BBtHWYGBznWwJqN2vXe9HLhjVPRoBYOt74ZysJR+8vrfFlwBo+WWpDz0C4K1WcDg==;EndpointSuffix=core.windows.net";

                if (!_objbatchBusiness.batchIdExist(batchId))
                {
                    objerror.correlationId = batchId;
                    lsterrors.Add(new errors { source = "BatchFileUpload", description = "batchId doesn't exist!!!" });
                    objerror.errors = lsterrors.ToArray();
                    return BadRequest(objerror);
                }
                //string containerName = "testassesmentonecontainer";
                string containerName = batchId;

                var blobContainerClient = new BlobContainerClient(connectionString, containerName);
                blobContainerClient.CreateIfNotExists();

                //var files = Directory.GetFiles(@"D:\Usama\Assessment\TempFiles");
                var files = Directory.GetFiles(Common.GetConfigValue("KeyValue:FilePath"));

                List<string> lstFileNameExist = Common.GetExistFileName(blobContainerClient, files);
                if (lstFileNameExist != null && lstFileNameExist.Count > 0)
                {
                    lsterrors.AddRange(lstFileNameExist.Select(item => new errors { source = "BatchFileUpload", description = item + " already exist" }));
                    objerror.errors = lsterrors.ToArray();
                    return BadRequest(objerror);
                }

                Common.UploadFilesInContainer(blobContainerClient, files);
                return Ok("Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();


        }
    }
}
