using eSya.ConfigPatientType.DL.Entities;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.DL.Repository
{
    public class CommonDataRepository: ICommonDataRepository
    {
        public async Task<List<DO_ApplicationCodes>> GetApplicationCodesbyCodeType(int codetype)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcds.Where(x=>x.ActiveStatus && x.CodeType==codetype)
                        .Select(r => new DO_ApplicationCodes
                        {
                            ApplicationCode = r.ApplicationCode,
                            CodeDesc = r.CodeDesc,
                        }).OrderBy(o => o.CodeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcds
                        .Where(w => w.ActiveStatus
                        && l_codeType.Contains(w.CodeType))
                        .Select(r => new DO_ApplicationCodes
                        {
                            CodeType = r.CodeType,
                            ApplicationCode = r.ApplicationCode,
                            CodeDesc = r.CodeDesc
                        }).OrderBy(o => o.CodeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            LocationDescription = r.BusinessName + "-" + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_PatientTypCategoryAttribute>> GetActivePatientTypes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var pt = db.GtEcptches
                        .Where(w => w.ActiveStatus)
                        .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.PatientType),
                          pl => new { pl.PatientTypeId },
                          pt => new { PatientTypeId = pt.ApplicationCode },
                         (pl, pt) => new { pl, pt })
                        .Select(r => new DO_PatientTypCategoryAttribute
                        {
                            PatientTypeId = r.pl.PatientTypeId,
                            Description = r.pt.CodeDesc
                        }).OrderBy(o => o.Description).ToList();
                        var Distinct = pt.GroupBy(x => new { x.PatientTypeId })
                           .Select(y => y.First()).ToList();

                    return Distinct;
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ApplicationCodes>> GetPatientCategory()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsulgs
                        .Where(w => w.ActiveStatus && w.SubledgerType=="P" || w.SubledgerType=="C" && w.ActiveStatus)
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
    }
}
