using Assessment1.IBusiness;
using Assessment1.Models;
using Assessment1.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assessment1.Repository;
using Assessment1.IRepository;
using Assessment1.Data;

namespace Assessment1.Business
{
    public class BatchBusiness : IBatchBusiness
    {
        private readonly IBatchRepository _objbatchRepository;
        private readonly ApplicationDBContext _db;

        public BatchBusiness(ApplicationDBContext db, IBatchRepository objbatchRepository)
        {
            _db = db;
            _objbatchRepository = objbatchRepository;
        }
        public attributes[] GetAttributesById(string batchId)
        {
            try
            {
                attributes[] attribute = _objbatchRepository.GetAttributesById(batchId);
                return attribute;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Batch getBatchById(string batchId)
        {
            try
            {
                Batch batch = _objbatchRepository.getBatchById(batchId);
                return batch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object getBatchData(string batchId)
        {
            try
            {
                object obj = _objbatchRepository.getBatchData(batchId);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BatchVM GetBatchVMById(string batchId)
        {
            try
            {
                BatchVM objbatchVM = _objbatchRepository.GetBatchVMById(batchId);
                return objbatchVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] GetReadGroupsById(string batchId)
        {
            try
            {
                string[] obj = _objbatchRepository.GetReadGroupsById(batchId);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] GetReadUsersById(string batchId)
        {
            try
            {
                string[] obj = _objbatchRepository.GetReadUsersById(batchId);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string insertBatchData(BatchVM batchvm)
        {
            try
            {
                string obj = _objbatchRepository.insertBatchData(batchvm);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
