using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Aplicacao.Models.COMMON
{
    /// <summary>
    /// Classe que contem todos os enum necessários para a aplicacao
    /// </summary>
    public class EnumAPP
    {

        public enum Cargo
        {
            #region MEDICAL_DETAILING_CONSULTANT 
            [Description("SR MEDICAL DETAILING CONSULTANT")]
            SR_MEDICAL_DETAILING_CONSULTANT = 1            
            
            ,[Description("JR MEDICAL DETAILING CONSULTANT")]
            JR_MEDICAL_DETAILING_CONSULTANT
                      
            ,[Description("MEDICAL DETAILING CONSULTANT")]
            MEDICAL_DETAILING_CONSULTANT
            #endregion MEDICAL_DETAILING_CONSULTANT  
            #region HOSPITAL_CONSULTANT  
            ,[Description("HOSPITAL CONSULTANT")]
            HOSPITAL_CONSULTANT
            
            ,[Description("SR HOSPITAL CONSULTANT")]
            SR_HOSPITAL_CONSULTANT
            
            ,[Description("JR HOSPITAL CONSULTANT")]
            JR_HOSPITAL_CONSULTANT
            #endregion HOSPITAL_CONSULTANT

            ,[Description("DIGITAL PROMOTER")]
            DIGITAL_PROMOTER
            
            ,[Description("JR REPRESENTATIVE")]
            JR_REPRESENTATIVE
            
            ,[Description("JR SPECIALTIES CONSULTANT")]
            JR_SPECIALTIES_CONSULTANT
            
            ,[Description("TELESALES SPECIALIST")]
            TELESALES_SPECIALIST
            
            ,[Description("VACCINES KEY ACCOUNT MANAGER")]
            VACCINES_KEY_ACCOUNT_MANAGER
            
            ,[Description("ACCOUNT MANAGER")]
            ACCOUNT_MANAGER
            
            ,[Description("SR ACCOUNT MANAGER")]
            SR_ACCOUNT_MANAGER
            
            ,[Description("JR ACCOUNT MANAGER")]
            JR_ACCOUNT_MANAGER
            
            ,[Description("SR TRADE MARKETING ANALYST")]
            SR_TRADE_MARKETING_ANALYST
            
            ,[Description("ONCOLOGY CONSULTANT")]
            ONCOLOGY_CONSULTANT
            
            ,[Description("JR ONCOLOGY CONSULTANT")]
            JR_ONCOLOGY_CONSULTANT
            
            ,[Description("SR ONCOLOGY CONSULTANT")]
            SR_ONCOLOGY_CONSULTANT
            
            ,[Description("JR SALES CONSULTANT")]
            JR_SALES_CONSULTANT
            
            ,[Description("PL SALES CONSULTANT")]
            PL_SALES_CONSULTANT
                        
            ,[Description("SR SALES CONSULTANT")]
            SR_SALES_CONSULTANT
            
            ,[Description("SALES CONSULTANT")]
            SALES_CONSULTANT

            ,[Description("JR SALES CONSULTING")]
            JR_SALES_CONSULTING
                        
            ,[Description("SR SALES CONSULTING")]
            SR_SALES_CONSULTING

            ,[Description("MERCHANDISING PROMOTER")]
            MERCHANDISING_PROMOTER
            
            ,[Description("JR SALES REPRESENTATIVE")]
            JR_SALES_REPRESENTATIVE
                        
            ,[Description("GERENTE DE CONTAS")]
            GERENTE_DE_CONTAS
            
            ,[Description("SALES CONSULTING")]
            SALES_CONSULTING
            
            ,[Description("JR PRODUCT SPECIALIST IMMUNO")]
            JR_PRODUCT_SPECIALIST_IMMUNO
            
            ,[Description("PRODUCT SPECIALIST IMMUNO")]
            PRODUCT_SPECIALIST_IMMUNO
            
            ,[Description("SR PRODUCT SPECIALIST IMMUNO")]
            SR_PRODUCT_SPECIALIST_IMMUNO
            
            ,[Description("PRODUCT SPECIALIST")]
            PRODUCT_SPECIALIST
            
            ,[Description("PRODUCT JR SPECIALIST")]
            PRODUCT_JR_SPECIALIST
            
            ,[Description("SR PRODUCT SPECIALIST")]
            SR_PRODUCT_SPECIALIST
            
            ,[Description("PRODUCTS SPECIALIST")]
            PRODUCTS_SPECIALIST
            
            ,[Description("PRODUCT ESPECIALIST")]
            PRODUCT_ESPECIALIST
            
            ,[Description("JR PRODUCT SPECIALIST")]
            JR_PRODUCT_SPECIALIST
            
            ,[Description("SECURITY ASSISTANT I")]
            SECURITY_ASSISTANT_I
            
            ,[Description("HOSPITAL QUALITY MANAGER")]
            HOSPITAL_QUALITY_MANAGER
                        
            ,[Description("JR DEMAND REPRESENTATIVE")]
            JR_DEMAND_REPRESENTATIVE
            
            ,[Description("DEMAND & SALES JR REPRESENTATIVE")]
            DEMAND_AND_SALES_JR_REPRESENTATIVE
            
            ,[Description("DEMAND & SALES REPRESENTATIVE")]
            DEMAND_AND_SALES_REPRESENTATIVE
        }

        public enum Time
        {

        }
    }
}