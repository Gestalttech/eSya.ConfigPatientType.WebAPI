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
    public class PatientTypesRepository: IPatientTypesRepository
    {
        private readonly IStringLocalizer<PatientTypesRepository> _localizer;
        public PatientTypesRepository(IStringLocalizer<PatientTypesRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Patient Type & Category Link with Param
        public async Task<List<DO_ApplicationCodes>> GetSubledgerTypes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsulgs
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_ApplicationCodes
                        {
                            ApplicationCode = r.SubledgerGroup,
                            CodeDesc = r.SubledgerType
                        }).OrderBy(o => o.CodeDesc).ToList();

                    var dist = ds.GroupBy(x => new { x.CodeDesc }).Select(g => g.First()).ToList();
                    return dist.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_ApplicationCodes>> GetPatientCategorybySubledgerType(string subledgertype)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsulgs
                        .Where(w => w.ActiveStatus && w.SubledgerType == subledgertype && w.ActiveStatus)
                        .Select(r => new DO_ApplicationCodes
                        {
                            ApplicationCode = r.SubledgerGroup,
                            CodeDesc = r.SubledgerDesc
                        }).OrderBy(o => o.CodeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_PatientAttributes> GetAllPatientTypesforTreeView(int CodeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    DO_PatientAttributes obj = new DO_PatientAttributes();
                    obj.l_PatientType = await db.GtEcapcds.Where(x => x.CodeType == CodeType)
                                   .Select(s => new DO_PatientTypeAttribute()
                                   {
                                       PatientTypeId = s.ApplicationCode,
                                       //Description = s.ApplicationCode + " - " + s.CodeDesc,
                                       Description =  s.CodeDesc,
                                       ActiveStatus = s.ActiveStatus
                                   }).ToListAsync();
                    obj.l_PatienTypeCategory = await db.GtEcptches.Join(db.GtEcsulgs,
                        x => x.PatientCategoryId,
                        y => y.SubledgerGroup,
                       (x, y) => new DO_PatientTypCategoryAttribute
                       {
                           PatientTypeId = x.PatientTypeId,
                           PatientCategoryId = x.PatientCategoryId,
                           //Description = x.PatientCategoryId.ToString() + " - " + y.CodeDesc,
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

        public async Task<DO_PatientTypCategoryAttribute> GetPatientCategoryInfo(int PatientTypeId, int PatientCategoryId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var pa_categories = db.GtEcptches
                    .Where(x => x.PatientTypeId == PatientTypeId && x.PatientCategoryId == PatientCategoryId)
                    .Select(pc => new DO_PatientTypCategoryAttribute
                    {
                        PatientTypeId = pc.PatientTypeId,
                        PatientCategoryId = pc.PatientCategoryId,
                        ActiveStatus = pc.ActiveStatus,
                        Description=db.GtEcsulgs.Where(x=>x.SubledgerGroup== PatientCategoryId).FirstOrDefault().SubledgerType,
                        l_ptypeparams = db.GtEcpapcs.Where(h => h.PatientTypeId == PatientTypeId
                        && h.PatientCategoryId == PatientCategoryId).Select(p => new DO_eSyaParameter
                        {
                            ParameterID = p.ParameterId,
                            ParmAction = p.ParmAction,
                        }).ToList()
                    }).FirstOrDefaultAsync();
                    return await pa_categories;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertPatientCategory(DO_PatientTypCategoryAttribute obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEcptch _pcat = db.GtEcptches.Where(x => x.PatientTypeId == obj.PatientTypeId && x.PatientCategoryId == obj.PatientCategoryId).FirstOrDefault();
                        if (_pcat == null)
                        {
                            _pcat = new GtEcptch
                            {
                                PatientTypeId = obj.PatientTypeId,
                                PatientCategoryId = obj.PatientCategoryId,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcptches.Add(_pcat);

                            foreach (DO_eSyaParameter ip in obj.l_ptypeparams)
                            {
                                var _parm = new GtEcpapc
                                {
                                    PatientTypeId = obj.PatientTypeId,
                                    PatientCategoryId = obj.PatientCategoryId,
                                    ParameterId = ip.ParameterID,
                                    ParmPerc = 0,
                                    ParmDesc = null,
                                    ParmValue = 0,
                                    ParmAction = ip.ParmAction,
                                    ActiveStatus = ip.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,
                                };
                                db.GtEcpapcs.Add(_parm);
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

                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;

                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdatePatientCategory(DO_PatientTypCategoryAttribute obj)
        {

            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcptch pat_cat = db.GtEcptches.Where(x => x.PatientTypeId == obj.PatientTypeId && x.PatientCategoryId == obj.PatientCategoryId).FirstOrDefault();
                        if (pat_cat != null)
                        {


                            pat_cat.ActiveStatus = obj.ActiveStatus;
                            pat_cat.ModifiedBy = obj.UserID;
                            pat_cat.ModifiedOn = System.DateTime.Now;
                            pat_cat.ModifiedTerminal = obj.TerminalID;

                            foreach (DO_eSyaParameter ip in obj.l_ptypeparams)
                            {
                                GtEcpapc sPar = db.GtEcpapcs.Where(x => x.PatientTypeId == obj.PatientTypeId && x.PatientCategoryId == obj.PatientCategoryId && x.ParameterId == ip.ParameterID).FirstOrDefault();
                                if (sPar != null)
                                {
                                    sPar.ParmPerc = 0;
                                    sPar.ParmDesc = null;
                                    sPar.ParmValue = 0;
                                    sPar.ParmAction = ip.ParmAction;
                                    sPar.ActiveStatus = obj.ActiveStatus;
                                    sPar.ModifiedBy = obj.UserID;
                                    sPar.ModifiedOn = System.DateTime.Now;
                                    sPar.ModifiedTerminal = obj.TerminalID;
                                }
                                else
                                {
                                    var parms = new GtEcpapc
                                    {
                                        PatientTypeId = obj.PatientTypeId,
                                        PatientCategoryId = obj.PatientCategoryId,
                                        ParameterId = ip.ParameterID,
                                        //ParamAction = ip.ParmAction,
                                        ParmPerc = 0,
                                        ParmDesc = null,
                                        ParmValue = 0,
                                        ParmAction = ip.ParmAction,
                                        ActiveStatus = ip.ActiveStatus,
                                        FormId = obj.FormID,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,
                                    };
                                    db.GtEcpapcs.Add(parms);
                                }
                            }
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0205", Message = string.Format(_localizer[name: "W0205"]) };

                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
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
