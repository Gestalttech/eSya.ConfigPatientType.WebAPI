﻿using eSya.ConfigPatientType.DL.Entities;
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
    public class BusinessRepository: IBusinessRepository
    {
        private readonly IStringLocalizer<BusinessRepository> _localizer;
        public BusinessRepository(IStringLocalizer<BusinessRepository> localizer)
        {
            _localizer = localizer;
        }


        #region Patient Type & Category Business Link
        //public async Task<List<DO_PatientTypeCategoryBusinessLink>> GetAllPatientCategoryBusinessLink(int businesskey)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var pa_link = db.GtEcptches.Where(x=>x.ActiveStatus)
        //                 .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.PatientType),
        //                  pl => new { pl.PatientTypeId },
        //                  pt => new { PatientTypeId = pt.ApplicationCode },
        //                 (pl, pt) => new { pl, pt })
        //                 .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.PatientCategory),
        //                 pll => new { pll.pl.PatientCategoryId },
        //                 pc => new { PatientCategoryId = pc.ApplicationCode },
        //                 (pll, pc) => new { pll, pc })
        //                 .GroupJoin(db.GtEcptcbs.Where(m => m.BusinessKey == businesskey),
        //                  plink => new { plink.pll.pl.PatientTypeId, plink.pll.pl.PatientCategoryId },
        //                  r => new { r.PatientTypeId, r.PatientCategoryId },
        //                 (plink, r) => new { plink, r })
        //                .SelectMany(z => z.r.DefaultIfEmpty(),
        //                  (a, b) => new DO_PatientTypeCategoryBusinessLink
        //                  {
        //                      PatientTypeId = a.plink.pll.pl.PatientTypeId,
        //                      PatientTypeDesc = a.plink.pll.pt.CodeDesc,
        //                      PatientCategoryId = a.plink.pll.pl.PatientCategoryId,
        //                      PatientCategoryDesc = a.plink.pc.CodeDesc,
        //                      BusinessKey = b != null ?b.BusinessKey : businesskey,
        //                      RateType = b == null ? 0 : b.RateType,
        //                      ActiveStatus = b == null ? false : b.ActiveStatus,
        //                  }).ToList();

        //            var Distinct = pa_link.GroupBy(x => new { x.PatientTypeId, x.PatientCategoryId })
        //                      .Select(y => y.First())
        //                      .ToList();
        //            return Distinct;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task<List<DO_PatientTypeCategoryBusinessLink>> GetAllPatientCategoryBusinessLink(int businesskey,int patienttypeId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var pa_link = db.GtEcptches.Where(x => x.ActiveStatus && x.PatientTypeId==patienttypeId)
                         .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.PatientCategory),
                          pl => new { pl.PatientCategoryId },
                          pt => new { PatientCategoryId = pt.ApplicationCode },
                         (pl, pt) => new { pl, pt })
                         .Join(db.GtEcpapcs.Where(w => w.ParameterId == 4 && w.ParmAction && w.ActiveStatus),
                         pll => new { pll.pl.PatientTypeId,pll.pl.PatientCategoryId },
                         pc => new { pc.PatientTypeId,pc.PatientCategoryId},
                         (pll, pc) => new { pll, pc })
                         .GroupJoin(db.GtEcptcbs.Where(m => m.BusinessKey == businesskey),
                          plink => new { plink.pll.pl.PatientTypeId, plink.pll.pl.PatientCategoryId },
                          r => new { r.PatientTypeId, r.PatientCategoryId },
                         (plink, r) => new { plink, r })
                        .SelectMany(z => z.r.DefaultIfEmpty(),
                          (a, b) => new DO_PatientTypeCategoryBusinessLink
                          {
                              PatientTypeId = a.plink.pll.pl.PatientTypeId,
                              PatientTypeDesc = a.plink.pll.pt.CodeDesc,
                              PatientCategoryId = a.plink.pll.pl.PatientCategoryId,
                              PatientCategoryDesc = a.plink.pll.pt.CodeDesc,
                              BusinessKey = b != null ? b.BusinessKey : businesskey,
                              RateType = b == null ? 0 : b.RateType,
                              ActiveStatus = b == null ? false : b.ActiveStatus,
                          }).ToList();

                    var Distinct = pa_link.GroupBy(x => new { x.PatientTypeId, x.PatientCategoryId })
                              .Select(y => y.First())
                              .ToList();
                    return Distinct;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdatePatientCategoryBusinessLink(List<DO_PatientTypeCategoryBusinessLink> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var pt in obj)
                        {
                            GtEcptcb pt_link = db.GtEcptcbs.Where(x => x.PatientTypeId == pt.PatientTypeId
                                            && x.PatientCategoryId == pt.PatientCategoryId && x.BusinessKey == pt.BusinessKey).FirstOrDefault();
                            if (pt_link == null)
                            {
                                var plink = new GtEcptcb
                                {
                                    BusinessKey = pt.BusinessKey,
                                    PatientTypeId = pt.PatientTypeId,
                                    PatientCategoryId = pt.PatientCategoryId,
                                    RateType = pt.RateType,
                                    ActiveStatus = pt.ActiveStatus,
                                    FormId = pt.FormID,
                                    CreatedBy = pt.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = pt.TerminalID
                                };
                                db.GtEcptcbs.Add(plink);
                            }
                            else
                            {
                                pt_link.RateType = pt.RateType;
                                pt_link.ActiveStatus = pt.ActiveStatus;
                                pt_link.ModifiedBy = pt.UserID;
                                pt_link.ModifiedOn = System.DateTime.Now;
                                pt_link.ModifiedTerminal = pt.TerminalID;
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
