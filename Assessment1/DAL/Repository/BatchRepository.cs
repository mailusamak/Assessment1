using Assessment1.DAL.IRepository;
using Assessment1.Data;
using Assessment1.Models;
using Assessment1.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.DAL.Repository
{
    public class BatchRepository : IBatchRepository
    {
        private readonly ApplicationDBContext _db;


        public BatchRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public object getBatchData(string batchId)
        {
            object obj = (from b in _db.Batch
                          join a in _db.attributes
                          on b.BatchId equals a.BatchId
                          join usrread in _db.ACLreadUsers
                          on b.BatchId equals usrread.BatchId
                          join usrgroup in _db.ACLreadGroups
                          on b.BatchId equals usrgroup.BatchId
                          where b.BatchId == batchId
                          select new
                          {
                              BatchId = b.BatchId,
                              expiryDate = b.expiryDate,
                              BusinessUnit = b.BusinessUnit,
                              batchpublishedDate = b.batchpublishedDate,
                              key = a.key,
                              value = a.value,
                              readUsers = usrread.readUsers,
                              readGroups = usrgroup.readGroups
                          }).Distinct().ToList();

            return obj;
        }

        public Batch getBatchById(string batchId)
        {
            try
            {
                Batch batch = (from b in _db.Batch join bu in _db.BusinessUnitMaster on b.BusinessUnitId equals bu.Id where b.BatchId == batchId select new Batch { BatchId = b.BatchId, BusinessUnit = bu.BusinessUnit, BusinessUnitId = b.BusinessUnitId, expiryDate = b.expiryDate, batchpublishedDate = b.batchpublishedDate }).FirstOrDefault();
                return batch;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public attributes[] GetAttributesById(string batchId)
        {
            attributes[] attribute = _db.attributes.Where(x => x.BatchId == batchId).Select(x => new attributes { key = x.key, value = x.value }).ToArray();
            return attribute;
        }

        public string[] GetReadUsersById(string batchId)
        {
            string[] readUser = _db.ACLreadUsers.Where(x => x.BatchId == batchId).Select(x => x.readUsers).ToArray();
            return readUser;
        }
        public string[] GetReadGroupsById(string batchId)
        {
            string[] readGroup = _db.ACLreadGroups.Where(x => x.BatchId == batchId).Select(x => x.readGroups).ToArray();
            return readGroup;
        }

        public BatchVM GetBatchVMById(string batchId)
        {
            try
            {
                Batch batch = getBatchById(batchId);
                if (batch == null)
                {
                    throw new NullReferenceException("Batch Not found");
                }
                attributes[] attribute = GetAttributesById(batchId);
                acl objacl = new acl();
                objacl.readUsers = GetReadUsersById(batchId);
                objacl.readGroups = GetReadGroupsById(batchId);

                BatchVM batchVM = new BatchVM();

                batchVM.BatchId = batch.BatchId;
                batchVM.BusinessUnitId = batch.BusinessUnitId;
                batchVM.BusinessUnit = batch.BusinessUnit;
                batchVM.expiryDate = Convert.ToString(batch.expiryDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'"));
                batchVM.batchpublishedDate = batch.batchpublishedDate;
                batchVM.attributes = attribute;
                batchVM.acl = objacl;

                return batchVM;
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
                Guid guid = Guid.NewGuid();
                batchvm.BatchId = guid.ToString();
                var batchId = batchvm.BatchId;

                Batch batch = new Batch();
                batch.BatchId = batchId;
                batch.BusinessUnitId = batchvm.BusinessUnitId;
                batch.expiryDate = Convert.ToDateTime(batchvm.expiryDate);
                batch.batchpublishedDate = DateTime.Now;

                List<ACLreadUsers> aCLreadUsers = new List<ACLreadUsers>();
                List<ACLreadGroups> aCLreadGroups = new List<ACLreadGroups>();
                List<attributes> lstattributes = new List<attributes>();
                if (batchvm.acl.readUsers != null && batchvm.acl.readUsers.Count() > 0)
                {
                    aCLreadUsers.AddRange(batchvm.acl.readUsers.Select(item => new ACLreadUsers() { readUsers = item, BatchId = batchId }));
                }
                if (batchvm.acl.readGroups != null && batchvm.acl.readGroups.Count() > 0)
                {
                    aCLreadGroups.AddRange(batchvm.acl.readGroups.Select(item => new ACLreadGroups() { readGroups = item, BatchId = batchId }));
                }

                if (batchvm.attributes != null && batchvm.attributes.Count() > 0)
                {
                    lstattributes.AddRange(batchvm.attributes.Select(item => new attributes() { key = item.key, value = item.value, BatchId = batchId }));
                }
                _db.Batch.Add(batch);
                _db.SaveChanges();

                if (lstattributes.Count > 0)
                {
                    _db.attributes.AddRange(lstattributes);
                    _db.SaveChanges();
                }

                if (aCLreadUsers.Count > 0)
                {
                    _db.ACLreadUsers.AddRange(aCLreadUsers);
                    _db.SaveChanges();
                }

                if (aCLreadGroups.Count > 0)
                {
                    _db.ACLreadGroups.AddRange(aCLreadGroups);
                    _db.SaveChanges();
                }

                string strBatchId = Convert.ToString(_db.Batch.Where(x => x.BatchId.Equals(batchId)).Select(x => x.BatchId).SingleOrDefault());
                return strBatchId;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int getBusinessUnitID(string businessUnit)
        {
            try
            {
                int result = _db.BusinessUnitMaster.Where(x => x.BusinessUnit.ToUpper() == businessUnit.ToUpper()).Select(x => x.Id).SingleOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool batchIdExist(string batchId)
        {
            try
            {
                bool retval = false;
                string strBatchId = _db.Batch.Where(x => x.BatchId == batchId).Select(x => x.BatchId).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(strBatchId))
                    retval = true;
                return retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
