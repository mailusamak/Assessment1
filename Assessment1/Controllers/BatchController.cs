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
using Assessment1.Repository;
using Assessment1.IBusiness;

namespace Assessment1.Controllers
{
    [Route("api/{controller}/{action}/{id?}")]
    [ApiController]
    public class BatchController : ControllerBase
    {

        private readonly ApplicationDBContext _db;
        private readonly IBatchBusiness _objbatchBusiness;

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
        public IActionResult CreateNew(BatchVM batchvm)
        {
            Error objerror = new Error();
            List<errors> lsterrors = new List<errors>();
            objerror.correlationId = Guid.NewGuid().ToString();
            try
            {
                if (!ModelState.IsValid)
                {
                    lsterrors.AddRange(ModelState.SelectMany(state => state.Value.Errors.Select(error => new errors { source = "Batch", description = error.ErrorMessage })));
                    objerror.errors = lsterrors.ToArray();
                    return NotFound(objerror);
                }

                if (!string.IsNullOrWhiteSpace(batchvm.BusinessUnit))
                {
                    batchvm.BatchId = _objbatchBusiness.insertBatchData(batchvm);

                    if (string.IsNullOrWhiteSpace(batchvm.BatchId))
                    {
                        lsterrors.Add(new errors { source = "Batch", description = "Invalid data" });
                        objerror.errors = lsterrors.ToArray();
                        return NotFound(objerror);
                    }
                    return Ok(new { batchId = batchvm.BatchId });

                }
                else
                {
                    lsterrors.Add(new errors { source = "Batch", description = "BusinessUnit required!!!" });
                    objerror.errors = lsterrors.ToArray();
                    return NotFound(objerror);
                }

            }
            catch (Exception ex)
            {

                lsterrors.Add(new errors { source = "Batch", description = ex.Message });
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

                lsterrors.Add(new errors { source = "Batch", description = ex.Message });
                objerror.errors = lsterrors.ToArray();
                return NotFound(objerror);
            }
            return NotFound(objerror);
        }
    }
}
