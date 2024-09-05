using eSya.ConfigPatientType.DL.Entities;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.DL.Repository
{
    public class DependentRepository: IDependentRepository
    {
        private readonly IStringLocalizer<DependentRepository> _localizer;
        public DependentRepository(IStringLocalizer<DependentRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Patient Dependent
        public async Task<List<DO_Dependent>> GetAllDependents(int businesskey,int patienttypeID,int patientcategoryID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcptdps.Where(x => x.BusinessKey == businesskey && x.PatientTypeId==patienttypeID
                    && x.PatientCategoryId==patientcategoryID)
                         .Join(db.GtEcapcds.Where(w => w.ActiveStatus),
                         x => x.Relationship,
                         y => y.ApplicationCode,
                        (x, y) => new DO_Dependent
                        {
                            Relationship = x.Relationship,
                            RelationshipDesc = y.CodeDesc,
                            ActiveStatus=x.ActiveStatus
                        }).ToListAsync();
                   
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdatePatientDependent(DO_Dependent obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEcptdp _pdept = db.GtEcptdps.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientTypeId == obj.PatientTypeId
                        && x.PatientCategoryId==obj.PatientCategoryId && x.Relationship==obj.Relationship).FirstOrDefault();
                        if (_pdept == null)
                        {
                           var _dept = new GtEcptdp
                            {
                                BusinessKey = obj.BusinessKey,
                                PatientTypeId = obj.PatientTypeId,
                                PatientCategoryId = obj.PatientCategoryId,
                                Relationship = obj.Relationship,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcptdps.Add(_dept);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        else
                        {
                            _pdept.ActiveStatus = obj.ActiveStatus;
                            _pdept.ModifiedBy = obj.UserID;
                            _pdept.ModifiedOn = System.DateTime.Now;
                            _pdept.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                    }

                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;

                    }
                }
            }
        }
        #endregion
    }
}
