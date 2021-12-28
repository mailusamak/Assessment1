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
using Assessment1.ModelView;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Assessment1.Test
{
    
    [TestFixture]
    public class CreateNewBatchTest
    {
        
        private IBatchRepository _objbatchRepository;
        private IBatchBusiness _objbatchBusiness;
        private ApplicationDBContext _db;
        private ILogger<BatchController> _logger;
        private static IConfiguration _configuration;

        public CreateNewBatchTest()
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
        public void CreateNewBatchWithSuccess()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithWrongBusinessUnit()
        {
            string json = @"{
    'BusinessUnit':'abc',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithBusinessUnitValidation()
        {
            string json = @"{
    'BusinessUnit':'',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutBusinessParameter()
        {
            string json = @"{
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithExpiryDateValidation()
        {
            string json = @"{
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithBlankExpiryDate()
        {
            string json = @"{
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': ''
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutExpiryDate()
        {
            string json = @"{
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ]
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithBlankreadUser()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            ''
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutreadUser()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithBlankreadGroup()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            ''
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutreadGroup()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-25 17:32:06.597'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutKey()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': '',
            'value': 'Testattributesvalue1'
        }
    ],
    'expiryDate': '2021-12-18'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutValue()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': 'testattributesKey1',
            'value': ''
        }
    ],
    'expiryDate': '2021-12-18'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutKeyAndValue()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'key': '',
            'value': ''
        }
    ],
    'expiryDate': '2021-12-18'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutKeyProp()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
            'value': 'testValueq'
        }
    ],
    'expiryDate': '2021-12-18'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutKeyAndValueProp()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'attributes': [
        {
        }
    ],
    'expiryDate': '2021-12-18'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void CreateNewBatchWithoutAttributes()
        {
            string json = @"{
    'BusinessUnit':'Accounts',
    'acl': {
        'readUsers': [
            'testreadUsers1'
        ],
        'readGroups': [
            'testreadGroups1'
        ]
    },
    'expiryDate': '2021-12-18'
}";

            BatchVM batchVm = JsonConvert.DeserializeObject<BatchVM>(json);

            BatchController batchController = new BatchController(_db, _objbatchBusiness);
            var result = batchController.CreateNewBatch(batchVm);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

}
    
}
