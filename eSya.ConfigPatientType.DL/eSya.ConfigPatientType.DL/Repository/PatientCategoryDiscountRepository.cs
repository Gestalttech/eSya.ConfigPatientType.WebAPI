using eSya.ConfigPatientType.DL.Entities;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.DL.Repository
{
    public class PatientCategoryDiscountRepository: IPatientCategoryDiscountRepository
    {
        private readonly IStringLocalizer<PatientCategoryDiscountRepository> _localizer;
        public PatientCategoryDiscountRepository(IStringLocalizer<PatientCategoryDiscountRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Patient Category Discount
        public async Task<List<DO_PatientCategoryDiscount>> GetActivePatientCategoriesbyBusinessKey(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcptcbs
                        .Join(db.GtEcapcds,
                        b => new {b.PatientCategoryId},
                        c => new { PatientCategoryId=c.ApplicationCode},
                        (b, c) => new {b,c}).Where(x =>x.b.BusinessKey== businesskey && x.b.ActiveStatus && x.c.ActiveStatus)
                        .Select(r => new DO_PatientCategoryDiscount
                        {
                            PatientCategoryId = r.b.PatientCategoryId,
                            PatientCategoryDesc = r.c.CodeDesc,
                        }).OrderBy(o => o.PatientCategoryDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_PatientCategoryDiscount>> GetPatientCategoryDiscountbyDiscountAt(int businesskey, int patientcategoryId, int discountfor, bool serviceclass)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (serviceclass)
                    {
                        var ds_cl = db.GtEssrbls.Where(x => x.BusinessKey == businesskey && x.ActiveStatus)
                         .Join(db.GtEssrms.Where(w => w.ActiveStatus),
                               l => new { l.ServiceId },
                               c => new { c.ServiceId },
                               (l, c) => new { l, c })
                            .Join(db.GtEssrcls.Where(w=> w.ActiveStatus),
                                lc => new { lc.c.ServiceClassId },
                                o => new { o.ServiceClassId },
                                (lc, o) => new { lc, o })
                        .GroupJoin(db.GtEcpcsls.Where(w => w.BusinessKey == businesskey && w.PatientCategoryId == patientcategoryId && w.DiscountFor==discountfor),
                                  lco => new { lco.lc.l.BusinessKey, lco.o.ServiceClassId},
                                  d => new { d.BusinessKey, d.ServiceClassId },
                                  (lco, d) => new { lco, d })
                        .SelectMany(z => z.d.DefaultIfEmpty(),
                         (a, b) => new DO_PatientCategoryDiscount
                         {
                             ServiceClassId=a.lco.o.ServiceClassId,
                             ServiceClassDesc=a.lco.o.ServiceClassDesc,
                             ServiceChargePerc=b==null?0:b.ServiceChargePerc,
                             DiscountRule=b==null?"":b.DiscountRule,
                             DiscountPerc = b == null ? 0 : b.DiscountPerc,
                             ActiveStatus = b == null ? false : b.ActiveStatus
                         }).ToList();
                        var Distserviceclass = ds_cl.GroupBy(x => new { x.ServiceClassId}).Select(g => g.First()).ToList();
                        return Distserviceclass.ToList();
                    }
                    else
                    {
                        var ds_sm = db.GtEssrbls.Where(x => x.BusinessKey == businesskey && x.ActiveStatus)
                           .Join(db.GtEssrms.Where(w => w.ActiveStatus),
                                 l => new { l.ServiceId },
                                 c => new {c.ServiceId },
                                 (l, c) => new { l, c })
                          .GroupJoin(db.GtEcpcscs.Where(w => w.BusinessKey == businesskey && w.PatientCategoryId == patientcategoryId && w.DiscountFor == discountfor),
                                    lco => new { lco.l.BusinessKey, lco.c.ServiceId },
                                    d => new { d.BusinessKey, d.ServiceId },
                                    (lco, d) => new { lco, d })
                          .SelectMany(z => z.d.DefaultIfEmpty(),
                           (a, b) => new DO_PatientCategoryDiscount
                           {
                               ServiceId = a.lco.c.ServiceId,
                               ServiceClassDesc = a.lco.c.ServiceDesc,
                               ServiceChargePerc = b == null ? 0 : b.ServiceChargePerc,
                               DiscountRule = b == null ? "" : b.DiscountRule,
                               DiscountPerc = b == null ? 0 : b.DiscountPerc,
                               ActiveStatus = b == null ? false : b.ActiveStatus
                           }).ToList();
                        var Distservicemaster = ds_sm.GroupBy(x => new { x.ServiceId }).Select(g => g.First()).ToList();
                        return Distservicemaster.ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertPatientCategoryDiscount(DO_PatientCategoryDiscount obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcpcdh _header = db.GtEcpcdhs.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                            && x.DiscountFor == obj.DiscountFor).FirstOrDefault();
                        if (_header == null)
                        {
                            _header = new GtEcpcdh
                            {
                                BusinessKey = obj.BusinessKey,
                                PatientCategoryId = obj.PatientCategoryId,
                                DiscountFor = obj.DiscountFor,
                                DiscountAt=obj.DiscountAt,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcpcdhs.Add(_header);

                        }

                        if (obj.serviceclass)
                        {
                           
                             GtEcpcsl _sercl = db.GtEcpcsls.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                            && x.DiscountFor==obj.DiscountFor && x.ServiceClassId==obj.ServiceClassId).FirstOrDefault();
                            if (_sercl == null)
                            {
                                _sercl = new GtEcpcsl
                                {
                                    BusinessKey=obj.BusinessKey,
                                    PatientCategoryId = obj.PatientCategoryId,
                                    DiscountFor=obj.DiscountFor,
                                    ServiceClassId = obj.ServiceClassId,
                                    ServiceChargePerc=obj.ServiceChargePerc,
                                    DiscountRule=obj.DiscountRule,
                                    DiscountPerc=obj.DiscountPerc,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEcpcsls.Add(_sercl);

                                foreach (DO_eSyaParameter ip in obj.l_discountparams)
                                {
                                    var _parm = new GtEcpad
                                    {
                                        PatientCategoryId = obj.PatientCategoryId,
                                        DiscountFor = obj.DiscountFor,
                                        DiscountAt=obj.DiscountAt,
                                        ServiceClassId=obj.ServiceClassId,
                                        ServiceId=0,
                                        ParameterId = ip.ParameterID,
                                        ParmPerc = ip.ParmPerc,
                                        ParmDesc = ip.ParmDesc,
                                        ParmValue = ip.ParmValue,
                                        ParmAction = ip.ParmAction,
                                        ActiveStatus = ip.ActiveStatus,
                                        FormId = obj.FormID,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,
                                    };
                                    db.GtEcpads.Add(_parm);
                                }

                                await db.SaveChangesAsync();
                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0204", Message = string.Format(_localizer[name: "W0204"]) };
                            }
                        }

                        else
                        {
                            GtEcpcsc _ser = db.GtEcpcscs.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                           && x.DiscountFor == obj.DiscountFor && x.ServiceId == obj.ServiceId).FirstOrDefault();
                            if (_ser == null)
                            {
                                _ser = new GtEcpcsc
                                {
                                    BusinessKey = obj.BusinessKey,
                                    PatientCategoryId = obj.PatientCategoryId,
                                    DiscountFor = obj.DiscountFor,
                                    ServiceId = obj.ServiceId,
                                    ServiceChargePerc = obj.ServiceChargePerc,
                                    DiscountRule = obj.DiscountRule,
                                    DiscountPerc = obj.DiscountPerc,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEcpcscs.Add(_ser);

                                foreach (DO_eSyaParameter ip in obj.l_discountparams)
                                {
                                    var _parm = new GtEcpad
                                    {
                                        PatientCategoryId = obj.PatientCategoryId,
                                        DiscountFor = obj.DiscountFor,
                                        DiscountAt = obj.DiscountAt,
                                        ServiceClassId =obj.ServiceClassId,
                                        ServiceId=obj.ServiceId,
                                        ParameterId = ip.ParameterID,
                                        ParmPerc = ip.ParmPerc,
                                        ParmDesc = ip.ParmDesc,
                                        ParmValue = ip.ParmValue,
                                        ParmAction = ip.ParmAction,
                                        ActiveStatus = ip.ActiveStatus,
                                        FormId = obj.FormID,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,
                                    };
                                    db.GtEcpads.Add(_parm);
                                }

                                await db.SaveChangesAsync();
                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0204", Message = string.Format(_localizer[name: "W0204"]) };
                            }
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

        public async Task<DO_ReturnParameter> UpdatePatientCategoryDiscount(DO_PatientCategoryDiscount obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcpcdh _header = db.GtEcpcdhs.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                            && x.DiscountFor == obj.DiscountFor).FirstOrDefault();
                        if (_header != null)
                        {
                            _header.DiscountAt = obj.DiscountAt;
                            _header.ActiveStatus = obj.ActiveStatus;
                            _header.ModifiedBy = obj.UserID;
                            _header.ModifiedOn = System.DateTime.Now;
                            _header.ModifiedTerminal = obj.TerminalID;
                        
                        }

                        if (obj.serviceclass)
                        {

                            GtEcpcsl _sercl = db.GtEcpcsls.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                           && x.DiscountFor == obj.DiscountFor && x.ServiceClassId == obj.ServiceClassId).FirstOrDefault();
                            if (_sercl != null)
                            {


                                _sercl.ServiceChargePerc = obj.ServiceChargePerc;
                                _sercl.DiscountRule = obj.DiscountRule;
                                _sercl.DiscountPerc = obj.DiscountPerc;
                                _sercl.ActiveStatus = obj.ActiveStatus;
                                _sercl.ModifiedBy = obj.UserID;
                                _sercl.ModifiedOn = System.DateTime.Now;
                                _sercl.ModifiedTerminal = obj.TerminalID;


                                foreach (DO_eSyaParameter ip in obj.l_discountparams)
                                {
                                    GtEcpad sPar = db.GtEcpads.Where(x => x.PatientCategoryId == obj.PatientCategoryId && x.DiscountFor==obj.DiscountFor 
                                    && x.DiscountAt==obj.DiscountAt && x.ServiceClassId==obj.ServiceClassId && x.ParameterId == ip.ParameterID).FirstOrDefault();
                                    if (sPar != null)
                                    {
                                        sPar.ParmPerc =ip.ParmPerc;
                                        sPar.ParmDesc = ip.ParmDesc;
                                        sPar.ParmValue = ip.ParmValue;
                                        sPar.ParmAction = ip.ParmAction;
                                        sPar.ActiveStatus = obj.ActiveStatus;
                                        sPar.ModifiedBy = obj.UserID;
                                        sPar.ModifiedOn = System.DateTime.Now;
                                        sPar.ModifiedTerminal = obj.TerminalID;
                                    }
                                    else
                                    {
                                        var parms = new GtEcpad
                                        {
                                            PatientCategoryId = obj.PatientCategoryId,
                                            DiscountFor=obj.DiscountFor,
                                            DiscountAt=obj.DiscountAt,
                                            ServiceClassId=obj.ServiceClassId,
                                            ServiceId=0,
                                            ParameterId = ip.ParameterID,
                                            //ParamAction = ip.ParmAction,
                                            ParmPerc = ip.ParmPerc,
                                            ParmDesc = ip.ParmDesc,
                                            ParmValue = ip.ParmValue,
                                            ParmAction = ip.ParmAction,
                                            ActiveStatus = ip.ActiveStatus,
                                            FormId = obj.FormID,
                                            CreatedBy = obj.UserID,
                                            CreatedOn = System.DateTime.Now,
                                            CreatedTerminal = obj.TerminalID,
                                        };
                                        db.GtEcpads.Add(parms);
                                    }
                                }

                                await db.SaveChangesAsync();
                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0204", Message = string.Format(_localizer[name: "W0204"]) };
                            }
                        }

                        else
                        {
                            GtEcpcsc _ser = db.GtEcpcscs.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                           && x.DiscountFor == obj.DiscountFor && x.ServiceId == obj.ServiceId).FirstOrDefault();
                            if (_ser != null)
                            {


                                _ser.ServiceChargePerc = obj.ServiceChargePerc;
                                _ser.DiscountRule = obj.DiscountRule;
                                _ser.DiscountPerc = obj.DiscountPerc;
                                _ser.ActiveStatus = obj.ActiveStatus;
                                _ser.ModifiedBy = obj.UserID;
                                _ser.ModifiedOn = System.DateTime.Now;
                                _ser.ModifiedTerminal = obj.TerminalID;

                                foreach (DO_eSyaParameter ip in obj.l_discountparams)
                                {
                                    GtEcpad sPar = db.GtEcpads.Where(x => x.PatientCategoryId == obj.PatientCategoryId && x.DiscountFor == obj.DiscountFor
                                    && x.DiscountAt == obj.DiscountAt && x.ServiceId == obj.ServiceId && x.ParameterId == ip.ParameterID).FirstOrDefault();
                                    if (sPar != null)
                                    {
                                        sPar.ParmPerc = ip.ParmPerc;
                                        sPar.ParmDesc = ip.ParmDesc;
                                        sPar.ParmValue = ip.ParmValue;
                                        sPar.ParmAction = ip.ParmAction;
                                        sPar.ActiveStatus = obj.ActiveStatus;
                                        sPar.ModifiedBy = obj.UserID;
                                        sPar.ModifiedOn = System.DateTime.Now;
                                        sPar.ModifiedTerminal = obj.TerminalID;
                                    }
                                    else
                                    {
                                        var parms = new GtEcpad
                                        {
                                            PatientCategoryId = obj.PatientCategoryId,
                                            DiscountFor = obj.DiscountFor,
                                            DiscountAt = obj.DiscountAt,
                                            ServiceClassId = obj.ServiceClassId,
                                            ServiceId = obj.ServiceId,
                                            ParameterId = ip.ParameterID,
                                            //ParamAction = ip.ParmAction,
                                            ParmPerc = ip.ParmPerc,
                                            ParmDesc = ip.ParmDesc,
                                            ParmValue = ip.ParmValue,
                                            ParmAction = ip.ParmAction,
                                            ActiveStatus = ip.ActiveStatus,
                                            FormId = obj.FormID,
                                            CreatedBy = obj.UserID,
                                            CreatedOn = System.DateTime.Now,
                                            CreatedTerminal = obj.TerminalID,
                                        };
                                        db.GtEcpads.Add(parms);
                                    }
                                }

                                await db.SaveChangesAsync();
                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0204", Message = string.Format(_localizer[name: "W0204"]) };
                            }
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

        public async Task<DO_PatientCategoryDiscount> GetPatientPatientCategoryDiscountInfo(DO_PatientCategoryDiscount obj)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (obj.serviceclass)
                    {
                        var pa_sercls = db.GtEcpcsls
                                           .Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                                           && x.ServiceClassId==obj.ServiceClassId && x.DiscountFor==obj.DiscountFor)
                                           .Select(pc => new DO_PatientCategoryDiscount
                                           {
                                               ServiceChargePerc = pc.ServiceChargePerc,
                                               DiscountRule=pc.DiscountRule,
                                               DiscountPerc=pc.DiscountPerc,
                                               ActiveStatus = pc.ActiveStatus,
                                               l_discountparams = db.GtEcpads.Where(h => h.PatientCategoryId == obj.PatientCategoryId
                                                && h.DiscountFor==obj.DiscountFor && h.DiscountAt==obj.DiscountAt && h.ServiceClassId==obj.ServiceClassId)
                                               .Select(p => new DO_eSyaParameter
                                               {
                                                   ParameterID = p.ParameterId,
                                                   ParmAction = p.ParmAction,
                                               }).ToList()
                                           }).FirstOrDefaultAsync();
                                          return await pa_sercls;
                    }
                    else
                    {
                        var pa_ser = db.GtEcpcscs
                                          .Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                                          && x.ServiceId == obj.ServiceId && x.DiscountFor == obj.DiscountFor)
                                          .Select(pc => new DO_PatientCategoryDiscount
                                          {
                                              ServiceChargePerc = pc.ServiceChargePerc,
                                              DiscountRule = pc.DiscountRule,
                                              DiscountPerc = pc.DiscountPerc,
                                              ActiveStatus = pc.ActiveStatus,
                                              l_discountparams = db.GtEcpads.Where(h => h.PatientCategoryId == obj.PatientCategoryId
                                               && h.DiscountFor == obj.DiscountFor && h.DiscountAt == obj.DiscountAt && h.ServiceId == obj.ServiceId)
                                              .Select(p => new DO_eSyaParameter
                                              {
                                                  ParameterID = p.ParameterId,
                                                  ParmAction = p.ParmAction,
                                              }).ToList()
                                          }).FirstOrDefaultAsync();
                        return await pa_ser;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> ActiveOrDeActivePatientCategoryDiscount(bool status, DO_PatientCategoryDiscount obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.serviceclass)
                        {
                            var pa_sercls = db.GtEcpcsls
                                          .Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                                          && x.ServiceClassId == obj.ServiceClassId && x.DiscountFor == obj.DiscountFor).FirstOrDefault();
                            if (pa_sercls == null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0206", Message = string.Format(_localizer[name: "W0206"]) };
                            }

                            pa_sercls.ActiveStatus = status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            if (status == true)
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                            else
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                        }
                        else
                        {
                            var pa_ser = db.GtEcpcscs
                                          .Where(x => x.BusinessKey == obj.BusinessKey && x.PatientCategoryId == obj.PatientCategoryId
                                          && x.ServiceId == obj.ServiceId && x.DiscountFor == obj.DiscountFor).FirstOrDefault();
                            if (pa_ser == null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0206", Message = string.Format(_localizer[name: "W0206"]) };
                            }

                            pa_ser.ActiveStatus = status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            if (status == true)
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                            else
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

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
