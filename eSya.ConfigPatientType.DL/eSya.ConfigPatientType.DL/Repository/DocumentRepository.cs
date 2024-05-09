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
    public class DocumentRepository: IDocumentRepository
    {
        private readonly IStringLocalizer<DocumentRepository> _localizer;
        public DocumentRepository(IStringLocalizer<DocumentRepository> localizer)
        {
            _localizer = localizer;
        }


        #region Patient Type & Category Document Link
        //public async Task<List<DO_PatientTypeCategoryDocumentLink>> GetAllPatientCategoryDocumentLink(int businesskey)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var pa_link = db.GtEcptches
        //                 .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.PatientType),
        //                  pl => new { pl.PatientTypeId },
        //                  pt => new { PatientTypeId = pt.ApplicationCode },
        //                 (pl, pt) => new { pl, pt })
        //                 .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.PatientCategory),
        //                 pll => new { pll.pl.PatientCategoryId },
        //                 pc => new { PatientCategoryId = pc.ApplicationCode },
        //                 (pll, pc) => new { pll, pc })
        //                 .GroupJoin(db.GtEcptcds.Where(m => m.BusinessKey == businesskey),
        //                  plink => new { plink.pll.pl.PatientTypeId, plink.pll.pl.PatientCategoryId },
        //                  r => new { r.PatientTypeId, r.PatientCategoryId },
        //                 (plink, r) => new { plink, r = r.FirstOrDefault() })
        //                 .Where(w => w.plink.pll.pl.ActiveStatus)
        //                  .Select(l => new DO_PatientTypeCategoryDocumentLink
        //                  {
        //                      PatientTypeId = l.plink.pll.pl.PatientTypeId,
        //                      PatientTypeDesc = l.plink.pll.pt.CodeDesc,
        //                      PatientCategoryId = l.plink.pll.pl.PatientCategoryId,
        //                      PatientCategoryDesc = l.plink.pc.CodeDesc,
        //                      ActiveStatus = l.r != null ? l.r.ActiveStatus : false,
        //                      BusinessKey = l.r != null ? l.r.BusinessKey : businesskey,
        //                      PatientCatgDocId = l.r != null ? l.r.PatientCatgDocId : 0,

        //                  }).ToListAsync();
        //            return await pa_link;
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public async Task<List<DO_PatientTypeCategoryDocumentLink>> GetAllPatientCategoryDocumentLink(int businesskey, int patienttypeId)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var pa_link = db.GtEcptches.Where(x => x.ActiveStatus && x.PatientTypeId == patienttypeId)
        //                 .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.PatientCategory),
        //                  pl => new { pl.PatientCategoryId },
        //                  pt => new { PatientCategoryId = pt.ApplicationCode },
        //                 (pl, pt) => new { pl, pt })
        //                 .Join(db.GtEcpapcs.Where(w => w.ParameterId == 7 && w.ParmAction && w.ActiveStatus),
        //                 pll => new { pll.pl.PatientTypeId, pll.pl.PatientCategoryId },
        //                 pc => new { pc.PatientTypeId, pc.PatientCategoryId },
        //                 (pll, pc) => new { pll, pc })
        //                 .GroupJoin(db.GtEcptcds.Where(m => m.BusinessKey == businesskey),
        //                  plink => new { plink.pll.pl.PatientTypeId, plink.pll.pl.PatientCategoryId },
        //                  r => new { r.PatientTypeId, r.PatientCategoryId },
        //                 (plink, r) => new { plink, r })
        //                .SelectMany(z => z.r.DefaultIfEmpty(),
        //                  (a, b) => new DO_PatientTypeCategoryDocumentLink
        //                  {
        //                      PatientTypeId = a.plink.pll.pl.PatientTypeId,
        //                      PatientTypeDesc = a.plink.pll.pt.CodeDesc,
        //                      PatientCategoryId = a.plink.pll.pl.PatientCategoryId,
        //                      PatientCategoryDesc = a.plink.pll.pt.CodeDesc,
        //                      BusinessKey = b != null ? b.BusinessKey : businesskey,
        //                      PatientCatgDocId = b == null ? 0 : b.PatientCatgDocId,
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
        //public async Task<DO_ReturnParameter> InsertOrUpdatePatientCategoryDocumentLink(List<DO_PatientTypeCategoryDocumentLink> obj)
        //{
        //    using (var db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                foreach (var pt in obj)
        //                {
        //                    GtEcptcd dt_link = db.GtEcptcds.Where(x => x.PatientTypeId == pt.PatientTypeId
        //                                    && x.PatientCategoryId == pt.PatientCategoryId && x.BusinessKey == pt.BusinessKey).FirstOrDefault();
        //                    if (dt_link == null)
        //                    {
        //                        var dlink = new GtEcptcd
        //                        {
        //                            BusinessKey = pt.BusinessKey,
        //                            PatientTypeId = pt.PatientTypeId,
        //                            PatientCategoryId = pt.PatientCategoryId,
        //                            PatientCatgDocId = pt.PatientCatgDocId,
        //                            ActiveStatus = pt.ActiveStatus,
        //                            FormId = pt.FormID,
        //                            CreatedBy = pt.UserID,
        //                            CreatedOn = System.DateTime.Now,
        //                            CreatedTerminal = pt.TerminalID
        //                        };
        //                        db.GtEcptcds.Add(dlink);
        //                    }
        //                    else
        //                    {
        //                        dt_link.PatientCatgDocId = pt.PatientCatgDocId;
        //                        dt_link.ActiveStatus = pt.ActiveStatus;
        //                        dt_link.ModifiedBy = pt.UserID;
        //                        dt_link.ModifiedOn = System.DateTime.Now;
        //                        dt_link.ModifiedTerminal = pt.TerminalID;
        //                    }
        //                    await db.SaveChangesAsync();
        //                }
        //                dbContext.Commit();
        //                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
        //            }

        //            catch (Exception ex)
        //            {
        //                dbContext.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //}
        #endregion

        #region Patient Type & Category Document Link
       
        public async Task<List<DO_PatientTypeCategoryDocumentLink>> GetPatientTypeCategoryDocumentInfo(int businesskey, int PatientTypeId, int PatientCategoryId,int codetype)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var doc_restricted = db.GtEcptches
                    .Where(x => x.PatientTypeId == PatientTypeId && x.PatientCategoryId == PatientCategoryId)
                     .Join(db.GtEcpapcs.Where(w => w.ParameterId == 7 && w.ParmAction && w.ActiveStatus),
                         pt => new { pt.PatientTypeId, pt.PatientCategoryId },
                         p => new { p.PatientTypeId, p.PatientCategoryId },
                         (pt, p) => new { pt, p }).ToList();

                    if (doc_restricted.Count > 0)
                    {
                        //need to bring Document from Application grid

                        var pa_link = db.GtEcapcds.Where(x => x.ActiveStatus && x.CodeType== codetype)
                           
                             .GroupJoin(db.GtEcptcds.Where(m => m.BusinessKey == businesskey && m.PatientTypeId == PatientTypeId && m.PatientCategoryId == PatientCategoryId),
                              p => new { p.ApplicationCode },
                              r => new { ApplicationCode= r.PatientCatgDocId },
                             (p, r) => new { p, r })
                            .SelectMany(z => z.r.DefaultIfEmpty(),
                              (a, b) => new DO_PatientTypeCategoryDocumentLink
                              {
                                  PatientCatgDocId =a.p.ApplicationCode,
                                  PatientCatgoryDocumentDesc = a.p.CodeDesc,
                                  ActiveStatus = b == null ? false : b.ActiveStatus,
                              }).ToList();

                        var Distinct = pa_link.GroupBy(x => new { x.PatientCatgDocId })
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
        public async Task<DO_ReturnParameter> InsertOrUpdatePatientCategoryDocumentLink(List<DO_PatientTypeCategoryDocumentLink> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var pt in obj)
                        {
                            GtEcptcd dt_link = db.GtEcptcds.Where(x => x.PatientTypeId == pt.PatientTypeId
                                            && x.PatientCategoryId == pt.PatientCategoryId && x.BusinessKey == pt.BusinessKey && x.PatientCatgDocId== pt.PatientCatgDocId).FirstOrDefault();
                            if (dt_link == null)
                            {
                                var dlink = new GtEcptcd
                                {
                                    BusinessKey = pt.BusinessKey,
                                    PatientTypeId = pt.PatientTypeId,
                                    PatientCategoryId = pt.PatientCategoryId,
                                    PatientCatgDocId = pt.PatientCatgDocId,
                                    ActiveStatus = pt.ActiveStatus,
                                    FormId = pt.FormID,
                                    CreatedBy = pt.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = pt.TerminalID
                                };
                                db.GtEcptcds.Add(dlink);
                            }
                            else
                            {
                                dt_link.ActiveStatus = pt.ActiveStatus;
                                dt_link.ModifiedBy = pt.UserID;
                                dt_link.ModifiedOn = System.DateTime.Now;
                                dt_link.ModifiedTerminal = pt.TerminalID;
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
