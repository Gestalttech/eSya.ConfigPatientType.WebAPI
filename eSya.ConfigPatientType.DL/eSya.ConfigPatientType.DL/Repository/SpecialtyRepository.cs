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
    public class SpecialtyRepository: ISpecialtyRepository
    {
        private readonly IStringLocalizer<SpecialtyRepository> _localizer;
        public SpecialtyRepository(IStringLocalizer<SpecialtyRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Patient Type & Category Specialty Link
        public async Task<DO_PatientAttributes> GetPatientCategoriesforTreeViewbyPatientType(int PatientTypeId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    DO_PatientAttributes obj = new DO_PatientAttributes();
                    
                    obj.l_PatienTypeCategory = await db.GtEcptches.Where(x=>x.ActiveStatus && x.PatientTypeId== PatientTypeId)
                        .Join(db.GtEcsulgs.Where(w => w.SubledgerType == "C"||w.SubledgerType=="P" && w.ActiveStatus),
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
        public async Task<List<DO_PatientTypeCategorySpecialtyLink>> GetPatientTypeCategorySpecialtyInfo(int businesskey, int PatientTypeId, int PatientCategoryId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var sp_restricted = db.GtEcptches
                    .Where(x => x.PatientTypeId == PatientTypeId && x.PatientCategoryId == PatientCategoryId)
                     .Join(db.GtEcpapcs.Where(w => w.ParameterId == 8 && w.ParmAction && w.ActiveStatus),
                         pt => new { pt.PatientTypeId, pt.PatientCategoryId },
                         p => new { p.PatientTypeId, p.PatientCategoryId },
                         (pt, p) => new { pt, p }).ToList();

                    if (sp_restricted.Count > 0)
                    {
                        //need to bring specialty grid

                        var pa_link = db.GtEsspbls.Where(x => x.BusinessKey == businesskey && x.ActiveStatus)
                             .Join(db.GtEsspcds,
                              spl => new { spl.SpecialtyId },
                              sm => new {sm.SpecialtyId},
                             (spl, sm) => new { spl, sm })
                             .GroupJoin(db.GtEcptsps.Where(m => m.BusinessKey == businesskey && m.PatientTypeId== PatientTypeId && m.PatientCategoryId == PatientCategoryId),
                              plink => new { plink.sm.SpecialtyId},
                              r => new { r.SpecialtyId },
                             (plink, r) => new { plink, r })
                            .SelectMany(z => z.r.DefaultIfEmpty(),
                              (a, b) => new DO_PatientTypeCategorySpecialtyLink
                              {
                                  SpecialtyId =a.plink.spl.SpecialtyId,
                                  SpecialtyDesc=a.plink.sm.SpecialtyDesc,
                                  ActiveStatus = b == null ? false : b.ActiveStatus,
                              }).ToList();

                        var Distinct = pa_link.GroupBy(x => new { x.SpecialtyId })
                                  .Select(y => y.First())
                                  .ToList();
                        return Distinct;
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
        public async Task<DO_ReturnParameter> InsertOrUpdatePatientCategorySpecialtyLink(List<DO_PatientTypeCategorySpecialtyLink> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var pt in obj)
                        {
                            GtEcptsp sp_link = db.GtEcptsps.Where(x => x.PatientTypeId == pt.PatientTypeId
                                            && x.PatientCategoryId == pt.PatientCategoryId && x.BusinessKey == pt.BusinessKey && x.SpecialtyId==pt.SpecialtyId).FirstOrDefault();
                            if (sp_link == null)
                            {
                                var splink = new GtEcptsp
                                {
                                    BusinessKey = pt.BusinessKey,
                                    PatientTypeId = pt.PatientTypeId,
                                    PatientCategoryId = pt.PatientCategoryId,
                                    SpecialtyId = pt.SpecialtyId,
                                    ActiveStatus = pt.ActiveStatus,
                                    FormId = pt.FormID,
                                    CreatedBy = pt.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = pt.TerminalID
                                };
                                db.GtEcptsps.Add(splink);
                            }
                            else
                            {
                                sp_link.SpecialtyId = pt.SpecialtyId;
                                sp_link.ActiveStatus = pt.ActiveStatus;
                                sp_link.ModifiedBy = pt.UserID;
                                sp_link.ModifiedOn = System.DateTime.Now;
                                sp_link.ModifiedTerminal = pt.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
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
