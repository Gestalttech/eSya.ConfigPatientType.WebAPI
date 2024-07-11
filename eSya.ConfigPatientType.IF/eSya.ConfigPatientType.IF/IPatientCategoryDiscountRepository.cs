using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface IPatientCategoryDiscountRepository
    {
        #region Patient Category Discount
        Task<List<DO_PatientCategoryDiscount>> GetActivePatientCategoriesbyBusinessKey(int businesskey);
        Task<List<DO_PatientCategoryDiscount>> GetPatientCategoryDiscountbyDiscountAt(int businesskey, int patientcategoryId, int discountfor, bool serviceclass);
        Task<DO_ReturnParameter> InsertPatientCategoryDiscount(DO_PatientCategoryDiscount obj);
        Task<DO_ReturnParameter> UpdatePatientCategoryDiscount(DO_PatientCategoryDiscount obj);
        Task<DO_PatientCategoryDiscount> GetPatientPatientCategoryDiscountInfo(DO_PatientCategoryDiscount obj);
        Task<DO_ReturnParameter> ActiveOrDeActivePatientCategoryDiscount( DO_PatientCategoryDiscount obj);
        #endregion
    }
}
