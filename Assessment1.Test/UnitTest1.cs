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

namespace Assessment1.Test
{
    [TestFixture]
    public class Tests
    {
        //private IBatchRepository _objbatchRepository;
        //private IBatchBusiness _objbatchBusiness;
        //private ApplicationDBContext _db;

        //public ApplicationDBContext dBContext { get; set; }
        //public Tests(ApplicationDBContext db, IBatchRepository objbatchRepository)
        //{
        //    _db = db;
        //    _objbatchRepository = objbatchRepository;
        //}
        //public Tests()
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
        //    optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Assessment_One;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        //    _db = new ApplicationDBContext(optionsBuilder.Options);
        //    var mockRepo = new Mock<IBatchRepository>();
        //}

        //[SetUp]
        //public void Setup()
        //{

        //}

        ////[Test]
        ////public void Test1()
        ////{
        ////    Assert.Pass();
        ////}

        //[Test]
        //public void testProj1()
        //{
        //    BatchRepository batchRepository = new BatchRepository(_db);
        //    BatchBusiness objbatchBusiness = new BatchBusiness(_db, batchRepository);
        //    var batchBusiness = objbatchBusiness.GetBatchVMById("c4029368-865d-4ab7-bb6e-01866c832723");
        //    Assert.Pass();
        //}

        //[Test]
        //public void testProj()
        //{

        //    //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
        //    //var context = new ApplicationDBContext(optionsBuilder.Options);




        //    BatchRepository batchRepository = new BatchRepository(_db);
        //    var batchBusiness = batchRepository.GetBatchVMById("c4029368-865d-4ab7-bb6e-01866c832723");
        //    Assert.Pass();

        //    //var optionsbuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
        //    //var _dbContext = new ApplicationDBContext(optionsbuilder.Options);
        //    //var objbatchRepository = new Mock<IBatchRepository>();
        //    //BatchBusiness objbatchBusiness = new BatchBusiness(_dbContext, (IBatchRepository)objbatchRepository);
        //    //var batchBusiness = objbatchBusiness.GetBatchVMById("c4029368-865d-4ab7-bb6e-01866c832723");
        //    //Assert.Pass();
        //}

        ////public void TestCont()
        ////{

        ////    var service = new Mock<IService>();
        ////    var controller = new BatchController(_db,_objbatchBusiness);

        ////    service.Setup(service => service.GetAsync(1)).ReturnsAsync((MyType)null);

        ////    var result = controller.Get(1);

        ////    Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        ////}
        //[Test]
        //public void testApi()
        //{
        //    var mockRepository = new Mock<ILogger<BatchController>>();
        //    BatchRepository batchRepository = new BatchRepository(_db);
        //    _objbatchBusiness = new BatchBusiness(_db, batchRepository);
        //    //BatchController batchController = new BatchController(mockRepository.Object, dBContext, _objbatchBusiness);
        //    BatchController batchController = new BatchController(dBContext, _objbatchBusiness);
        //    var result = batchController.FindBatch("c4029368-865d-4ab7-bb6e-01866c832723");
        //    Assert.Pass();
        //}
    }
}