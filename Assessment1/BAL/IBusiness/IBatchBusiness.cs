using Assessment1.Models;
using Assessment1.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.BAL.IBusiness
{
    public interface IBatchBusiness
    {
        public object getBatchData(string batchId);
        public Batch getBatchById(string batchId);
        public attributes[] GetAttributesById(string batchId);
        public string[] GetReadUsersById(string batchId);
        public string[] GetReadGroupsById(string batchId);
        public BatchVM GetBatchVMById(string batchId);
        public string insertBatchData(BatchVM batchvm);
        public int getBusinessUnitID(string businessUnit);
        public bool batchIdExist(string batchId);
    }
}
