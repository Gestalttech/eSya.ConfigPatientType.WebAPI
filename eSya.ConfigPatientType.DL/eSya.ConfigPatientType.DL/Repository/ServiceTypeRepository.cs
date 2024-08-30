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
    public class ServiceTypeRepository: IServiceTypeRepository
    {
        private readonly IStringLocalizer<ServiceTypeRepository> _localizer;
        public ServiceTypeRepository(IStringLocalizer<ServiceTypeRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Patient Type & Category Service Type Link
        public async Task<DO_PatientAttributes> GetPatientCategoriesforTreeViewbyPatientType(int PatientTypeId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    DO_PatientAttributes obj = new DO_PatientAttributes();

                    obj.l_PatienTypeCategory = await db.GtEcptches.Where(x => x.ActiveStatus && x.PatientTypeId == PatientTypeId)
                        .Join(db.GtEcsulgs.Where(w => w.SubledgerType == "C"||w.SubledgerType=="P"&& w.ActiveStatus),
                        x => x.PatientCategoryId,
                        y => y.SubledgerGroup,
                       (x, y) => new DO_PatientTypCategoryAttribute
                       {
                           PatientCategoryId = x.PatientCategoryId,
                           Description = y.SubledgerDesc,
                           ActiveStatus = x.ActiveStatus
                       }).ToListAsync();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_PatientTypeCategoryServiceTypeLink>> GetPatientTypeCategoryServiceTypeInfo(int businesskey, int PatientTypeId, int PatientCategoryId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var sp_restricted = db.GtEcptches
                    .Where(x => x.PatientTypeId == PatientTypeId && x.PatientCategoryId == PatientCategoryId)
                     .Join(db.GtEcpapcs.Where(w => w.ParameterId == 5 && w.ParmAction && w.ActiveStatus),
                         pt => new { pt.PatientTypeId, pt.PatientCategoryId },
                         p => new { p.PatientTypeId, p.PatientCategoryId },
                         (pt, p) => new { pt, p }).ToList();

                    if (sp_restricted.Count > 0)
                    {
                        //need to bring Service Type & Rate Type Grid

                        var rt_link =await db.GtEcptsrs.Where(x => x.BusinessKey == businesskey && x.PatientTypeId == PatientTypeId && x.PatientCategoryId == PatientCategoryId)
                             .Join(db.GtEssrties,
                              spl => new { spl.ServiceType },
                              sm => new { ServiceType = sm.ServiceTypeId },
                             (spl, sm) => new { spl, sm })
                             .Join(db.GtEcapcds.Where(m => m.CodeType == CodeTypeValue.ConfigPatientRateType),
                              spll => new { spll.spl.RateType },
                              r => new { RateType = r.ApplicationCode },
                             (spll, r) => new { spll, r })
                            .Select(x => new DO_PatientTypeCategoryServiceTypeLink{
                                BusinessKey=x.spll.spl.BusinessKey,
                                PatientTypeId=x.spll.spl.PatientTypeId,
                                PatientCategoryId=x.spll.spl.PatientCategoryId,
                                ServiceType= x.spll.spl.ServiceType,
                                RateType= x.spll.spl.RateType,
                                ActiveStatus= x.spll.spl.ActiveStatus,
                                ServiceTypeDesc=x.spll.sm.ServiceTypeDesc,
                                RateTypeDesc=x.r.CodeDesc
                                
                            }).ToListAsync();

                        return rt_link;
                    }
                    else
                    {
                        return null;


                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertPatientTypeCategoryServiceTypeLink(DO_PatientTypeCategoryServiceTypeLink obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEcptsr stype_link = db.GtEcptsrs.Where(x => x.PatientTypeId == obj.PatientTypeId
                                        && x.PatientCategoryId == obj.PatientCategoryId && x.BusinessKey == obj.BusinessKey && x.ServiceType == obj.ServiceType && x.RateType==obj.RateType).FirstOrDefault();
                        if (stype_link == null)
                        {
                            var stlink = new GtEcptsr
                            {
                                BusinessKey = obj.BusinessKey,
                                PatientTypeId = obj.PatientTypeId,
                                PatientCategoryId = obj.PatientCategoryId,
                                ServiceType = obj.ServiceType,
                                RateType = obj.RateType,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcptsrs.Add(stlink);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        else
                        {
                           
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0207", Message = string.Format(_localizer[name: "W0207"]) };
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
        public async Task<DO_ReturnParameter> UpdatePatientTypeCategoryServiceTypeLink(DO_PatientTypeCategoryServiceTypeLink obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEcptsr stype_link = db.GtEcptsrs.Where(x => x.PatientTypeId == obj.PatientTypeId
                                        && x.PatientCategoryId == obj.PatientCategoryId && x.BusinessKey == obj.BusinessKey && x.ServiceType == obj.ServiceType && x.RateType == obj.RateType).FirstOrDefault();
                        if (stype_link == null)
                        {
                            
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0206", Message = string.Format(_localizer[name: "W0206"]) };
                        }
                        else
                        {
                            stype_link.RateType = obj.RateType;
                            stype_link.ActiveStatus = obj.ActiveStatus;
                            stype_link.ModifiedBy = obj.UserID;
                            stype_link.ModifiedOn = System.DateTime.Now;
                            stype_link.ModifiedTerminal = obj.TerminalID;
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
        public async Task<List<DO_PatientTypeCategoryServiceTypeLink>> GetActiveServiceTypes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEssrties.Where(x => x.ActiveStatus)
                        .Select(r => new DO_PatientTypeCategoryServiceTypeLink
                        {
                            ServiceType = r.ServiceTypeId,
                            ServiceTypeDesc = r.ServiceTypeDesc,
                        }).OrderBy(o => o.ServiceTypeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
