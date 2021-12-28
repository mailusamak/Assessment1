using Assessment1.BAL.Business;
using Assessment1.BAL.IBusiness;
using Assessment1.Controllers;
using Assessment1.Test.DAL.IRepository;
using Assessment1.DAL.Repository;
using Assessment1.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using NLog;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Assessment1.Test
{
    
    [TestFixture]
    public class BatchControllerGetBatchTest
    {
        
        private IBatchRepository _objbatchRepository;
        private IBatchBusiness _objbatchBusiness;
        private ApplicationDBContext _db;
        private ILogger<BatchController> _logger;
        private static IConfiguration _configuration;
        public BatchControllerGetBatchTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Assessment_One;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            _db = new ApplicationDBContext(optionsBuilder.Options);

            var objbatchBusiness = new Mock<IBatchBusiness>();
            _objbatchBusiness = objbatchBusiness.Object;

            var logger = new Mock<ILogger<BatchController>>();
            _logger = logger.Object;
        }

        [SetUp]
        public void Setup()
        {
            BatchRepository batchRepository = new BatchRepository(_db);
            _objbatchBusiness = new BatchBusiness(_db, batchRepository);
        }

        [Test]
        [TestCase("353c72c1-e4b6-4c2d-bf39-c3da5629b283")]
        [TestCase("64eca158-821d-4eae-8141-2e4597e5390f")]
        [TestCase("a068f550-6527-4ef1-99b5-22c9f6031673")]
        public void FindBatchWithSuccess(string batchId)
        {
            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.FindBatch(batchId);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        [TestCase("353c72c1-e4b6-4c2d-bf39-c3da5629b28")]
        [TestCase("64eca158-821d-4eae-8141-2e4597e5390")]
        [TestCase("a068f550-6527-4ef1-99b5-22c9f603167")]
        public void FindBatchWithFailCase(string batchId)
        {
            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.FindBatch(batchId);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        [TestCase("")]
        [TestCase("")]
        [TestCase("")]
        public void FindBatchWithEmptyBatchId(string batchId)
        {
            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.FindBatch(batchId);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        [TestCase(null)]
        public void FindBatchWithNullBatchId(string? batchId)
        {
            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.FindBatch(batchId);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
        
}
    
}
