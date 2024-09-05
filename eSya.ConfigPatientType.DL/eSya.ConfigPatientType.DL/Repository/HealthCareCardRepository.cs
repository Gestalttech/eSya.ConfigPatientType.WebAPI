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
    public class HealthCareCardRepository: IHealthCareCardRepository
    {
        private readonly IStringLocalizer<HealthCareCardRepository> _localizer;
        public HealthCareCardRepository(IStringLocalizer<HealthCareCardRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Care Card Details
        public async Task<List<DO_PatientTypeAttribute>> GetPatientTypesbyBusinessKey(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcptcbs.Where(x => x.ActiveStatus && x.BusinessKey == businesskey)
                        .Join(db.GtEcapcds.Where(w => w.ActiveStatus),
                        x => x.PatientTypeId,
                        y => y.ApplicationCode,
                       (x, y) => new DO_PatientTypeAttribute
                       {
                           PatientTypeId = x.PatientTypeId,
                           Description = y.CodeDesc,
                       }).ToListAsync();
                    var dist = ds.GroupBy(x => new { x.PatientTypeId }).Select(g => g.First()).ToList();
                    return dist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_PatientTypCategoryAttribute>> GetPatientCategoriesbyPatientType(int businesskey,int patienttypeID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcptcbs.Where(x => x.ActiveStatus && x.BusinessKey == businesskey && x.PatientTypeId==patienttypeID)
                        .Join(db.GtEcsulgs.Where(w => w.ActiveStatus),
                        x => x.PatientCategoryId,
                        y => y.SubledgerGroup,
                       (x, y) => new DO_PatientTypCategoryAttribute
                       {
                           PatientCategoryId = x.PatientCategoryId,
                           Description = y.SubledgerDesc,
                       }).ToListAsync();
                    var dist = ds.GroupBy(x => new { x.PatientTypeId }).Select(g => g.First()).ToList();
                    return dist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_HealthCareCard>> GetHealthCareCards(int businesskey,int patienttypeID,int patientcategoryID,int healthcardID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcptccs
                        .Where(w => w.BusinessKey == businesskey && w.PatientTypeId == patienttypeID &&
                         w.PatientCategoryId==patientcategoryID && w.HealthCardId==healthcardID)
                        .Select(r => new DO_HealthCareCard
                        {
                            OfferStartDate = r.OfferStartDate,
                            OfferEndDate = r.OfferEndDate,
                            CardValidityInMonths = r.CardValidityInMonths,
                            CareCardNoPattern=r.CareCardNoPattern,
                            IsSpecialtySpecific=r.IsSpecialtySpecific,
                            ActiveStatus=r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_HealthCareCardSpecialtyLink>> GetSpecialtiesLinkedHealthCareCard(int businesskey, int healthcardID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var pa_link = db.GtEsspbls.Where(x => x.BusinessKey == businesskey && x.ActiveStatus)
                       .Join(db.GtEsspcds,
                       spl => new { spl.SpecialtyId },
                       sm => new { sm.SpecialtyId },
                       (spl, sm) => new { spl, sm })
                      .GroupJoin(db.GtEcptcs.Where(m => m.BusinessKey == businesskey && m.HealthCardId == healthcardID),
                        plink => new { plink.sm.SpecialtyId },
                        r => new { r.SpecialtyId },
                        (plink, r) => new { plink, r })
                       .SelectMany(z => z.r.DefaultIfEmpty(),
                      (a, b) => new DO_HealthCareCardSpecialtyLink
                      {
                           SpecialtyId = a.plink.spl.SpecialtyId,
                           SpecialtyDesc = a.plink.sm.SpecialtyDesc,
                           ActiveStatus = b == null ? false : b.ActiveStatus,
                           }).ToList();

                    var Distinct = pa_link.GroupBy(x => new { x.SpecialtyId })
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

        public async Task<List<DO_HealthCareCardRates>> GetHealthCareCardRates(int businesskey, int healthcardID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcptdts
                        .Where(w => w.BusinessKey == businesskey && w.HealthCardId == healthcardID)
                        .Join(db.GtEccucos.Where(x=>x.ActiveStatus),
                         x => new { x.CurrencyCode },
                         y => new { y.CurrencyCode },
                        (x, y) => new { x, y })
                        .Select(r => new DO_HealthCareCardRates
                        {
                            CurrencyCode = r.x.CurrencyCode,
                            EffectiveFrom = r.x.EffectiveFrom,
                            EffectiveTill = r.x.EffectiveTill,
                            CardCharges = r.x.CardCharges,
                            ActiveStatus = r.x.ActiveStatus,
                            CurrencyName=r.y.CurrencyName
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateHealthCareCard(DO_HealthCareCard obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var carecard = db.GtEcptccs.Where(x => x.BusinessKey == obj.BusinessKey && x.PatientTypeId == obj.PatientTypeId && x.PatientCategoryId == obj.PatientCategoryId
                        && x.HealthCardId == obj.HealthCardId && x.OfferStartDate.Date == obj.OfferStartDate.Date).FirstOrDefault();
                        if(carecard!=null)
                        {
                            carecard.OfferEndDate = obj.OfferEndDate;
                            carecard.CardValidityInMonths = obj.CardValidityInMonths;
                            carecard.CareCardNoPattern = obj.CareCardNoPattern;
                            carecard.IsSpecialtySpecific = obj.IsSpecialtySpecific;
                            carecard.ActiveStatus=obj.ActiveStatus;
                            carecard.ModifiedBy = obj.UserID;
                            carecard.ModifiedOn = System.DateTime.Now;
                            carecard.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            if (obj.lstspecialty != null)
                            {
                            foreach (var sp in obj.lstspecialty)
                            {
                                GtEcptc sp_link = db.GtEcptcs.Where(x => x.BusinessKey == sp.BusinessKey
                                                && x.HealthCardId == sp.HealthCardId && x.SpecialtyId == sp.SpecialtyId).FirstOrDefault();
                                if (sp_link != null)
                                {
                                    db.GtEcptcs.Remove(sp_link);
                                    await db.SaveChangesAsync();
                                }
                                else if (sp.ActiveStatus && obj.IsSpecialtySpecific)
                                {

                                    var splink = new GtEcptc
                                    {
                                        BusinessKey = sp.BusinessKey,
                                        HealthCardId = sp.HealthCardId,
                                        SpecialtyId = sp.SpecialtyId,
                                        ActiveStatus = sp.ActiveStatus,
                                        FormId = sp.FormID,
                                        CreatedBy = sp.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = sp.TerminalID
                                    };
                                    db.GtEcptcs.Add(splink);


                                }
                                await db.SaveChangesAsync();
                            }
                            }
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                            
                        }
                        else
                        {
                            var card = new GtEcptcc()
                            {
                                BusinessKey = obj.BusinessKey,
                                PatientTypeId = obj.PatientTypeId,
                                PatientCategoryId = obj.PatientCategoryId,
                                HealthCardId = obj.HealthCardId,
                                OfferStartDate = obj.OfferStartDate,
                                OfferEndDate = obj.OfferEndDate,
                                CardValidityInMonths = obj.CardValidityInMonths,
                                CareCardNoPattern = obj.CareCardNoPattern,
                                IsSpecialtySpecific = obj.IsSpecialtySpecific,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                            };
                            db.GtEcptccs.Add(card);
                            await db.SaveChangesAsync();
                            if (obj.lstspecialty != null)
                            {
                                foreach (var sp in obj.lstspecialty)
                                {
                                    GtEcptc sp_link = db.GtEcptcs.Where(x => x.BusinessKey == sp.BusinessKey
                                                    && x.HealthCardId == sp.HealthCardId && x.SpecialtyId == sp.SpecialtyId).FirstOrDefault();
                                    if (sp_link != null)
                                    {
                                        db.GtEcptcs.Remove(sp_link);
                                        await db.SaveChangesAsync();
                                    }
                                    else if (sp.ActiveStatus && obj.IsSpecialtySpecific)
                                    {

                                        var splink = new GtEcptc
                                        {
                                            BusinessKey = sp.BusinessKey,
                                            HealthCardId = sp.HealthCardId,
                                            SpecialtyId = sp.SpecialtyId,
                                            ActiveStatus = sp.ActiveStatus,
                                            FormId = sp.FormID,
                                            CreatedBy = sp.UserID,
                                            CreatedOn = System.DateTime.Now,
                                            CreatedTerminal = sp.TerminalID
                                        };
                                        db.GtEcptcs.Add(splink);


                                    }
                                    await db.SaveChangesAsync();
                                }
                            }
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
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
        public async Task<DO_ReturnParameter> InsertOrUpdateHealthCareCardRates(DO_HealthCareCardRates obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Check if a record already exists with the same keys and an open period (EffectiveTill is null)
                        var cardRatesExist = db.GtEcptdts
                            .Where(w => w.BusinessKey == obj.BusinessKey &&
                                        w.HealthCardId == obj.HealthCardId &&
                                         w.CurrencyCode == obj.CurrencyCode &&
                                        w.EffectiveTill == null)
                            .FirstOrDefault();

                        if (cardRatesExist != null)
                        {
                            if (obj.EffectiveFrom != cardRatesExist.EffectiveFrom)
                            {
                                if (obj.EffectiveFrom < cardRatesExist.EffectiveFrom)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0208", Message = string.Format(_localizer[name: "W0208"]) };
                                }

                                // Close the existing record by setting the EffectiveTill date
                                cardRatesExist.EffectiveTill = obj.EffectiveFrom.AddDays(-1);
                                cardRatesExist.ModifiedBy = obj.UserID;
                                cardRatesExist.ModifiedOn = DateTime.Now;
                                cardRatesExist.ModifiedTerminal = obj.TerminalID;
                                cardRatesExist.ActiveStatus = false;

                                // Insert the new record
                                var cRates = new GtEcptdt
                                {
                                    BusinessKey = obj.BusinessKey,
                                    HealthCardId = obj.HealthCardId,
                                    CurrencyCode=obj.CurrencyCode,
                                    EffectiveFrom = obj.EffectiveFrom,
                                    CardCharges=obj.CardCharges,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEcptdts.Add(cRates);
                            }
                            else
                            {
                                // Update the existing record
                                cardRatesExist.EffectiveTill = obj.EffectiveTill;
                                cardRatesExist.CardCharges = obj.CardCharges;
                                cardRatesExist.ActiveStatus = obj.ActiveStatus;
                                cardRatesExist.ModifiedBy = obj.UserID;
                                cardRatesExist.ModifiedOn = DateTime.Now;
                                cardRatesExist.ModifiedTerminal = obj.TerminalID;
                            }
                        }
                        else
                        {
                            // No existing record found, so insert the new record

                            var cratesnew = new GtEcptdt
                            {
                                BusinessKey = obj.BusinessKey,
                                HealthCardId = obj.HealthCardId,
                                CurrencyCode = obj.CurrencyCode,
                                EffectiveFrom = obj.EffectiveFrom,
                                CardCharges = obj.CardCharges,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcptdts.Add(cratesnew);

                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        return new DO_ReturnParameter() { Status = false, Message = ex.Message };
                    }
                }
            }
        }
        #endregion
    }
}
