using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace RajasthanTourCabN.Code.LIBS
{
    public static class SiteKey
    {
        public static string DomainName
        {
            get { return ConfigurationManager.AppSettings["DomainName"]; }
        }
        public static string CompanyName
        {
            get { return ConfigurationManager.AppSettings["CompanyName"]; }
        }
        public static string Address
        {
            get { return ConfigurationManager.AppSettings["Address"]; }
        }
        public static string PhoneNo
        {
            get { return ConfigurationManager.AppSettings["Mobileno"]; }
        }
        public static string EmailID
        {
            get { return ConfigurationManager.AppSettings["EmailID"]; }
        }
        public static string GoogleMapsApiKey
        {
            get { return ConfigurationManager.AppSettings["GoogleMapsApiKey"]; }
        }
        public static string Colorcode
        {
            get { return ConfigurationManager.AppSettings["ColorCode"]; }
        }
        public static string StyleName
        {
            get { return ConfigurationManager.AppSettings["ColorStyle"]; }
        }
        public static string Companylogo
        {
            get { return ConfigurationManager.AppSettings["Companylogo"]; }
        }
        public static string SqlConn
        {
            get { return ConfigurationManager.ConnectionStrings["sqlconn"].ConnectionString; }
        }
        public static string DomainNameInternalIP
        {
            get { return ConfigurationManager.AppSettings["DomainNameInternalIP"]; }
        }
        public static string MPlanToken
        {
            get { return ConfigurationManager.AppSettings["MPlanToken"]; }
        }
        public static string From { get { return ConfigurationManager.AppSettings["From"]; } }
        public static string To { get { return ConfigurationManager.AppSettings["To"]; } }
        public static string CC { get { return ConfigurationManager.AppSettings["CC"]; } }
        public static string BCC { get { return ConfigurationManager.AppSettings["BCC"]; } }
        public static string RefreshActivity { get { return ConfigurationManager.AppSettings["RefreshActivity"]; } }

        public static string ProjectClosureToEmail { get { return ConfigurationManager.AppSettings["ProjectClosureToEmail"]; } }
        public static string EscalatedToEmail { get { return ConfigurationManager.AppSettings["EscalatedToEmail"]; } }

        public static string CrmProjectList
        {
            get { return ConfigurationManager.AppSettings["crmapi"] + "projectdetail?type=projects&userid="; }
        }

        public static string CrmInvoiceList
        {
            get { return ConfigurationManager.AppSettings["crmapi"] + "updateInvoices?type=invoices&userid=@USERID&apipass=@APIPASSWORD&date=@INVOICEDATE"; }
        }

        public static string InvoiceDays
        {
            get { return ConfigurationManager.AppSettings["InvoiceDays"]; }
        }

        public static string UKDeveloperUID
        {
            get { return ConfigurationManager.AppSettings["UKDeveloperUID"]; }
        }
        public static string UKPMVirtualDeveloperID
        {
            get { return ConfigurationManager.AppSettings["UKPMVirtualDeveloperID"]; }
        }
        public static string PlanApiEnabled
        {
            get { return ConfigurationManager.AppSettings["PlanApiEnabled"]; }
        }
        public static string PlanApiID
        {
            get { return ConfigurationManager.AppSettings["PlanApiID"]; }
        }
        public static string PlanApiPassword
        {
            get { return ConfigurationManager.AppSettings["PlanapiPass"]; }
        }

        public static string FireBaseKey
        {
            get { return ConfigurationManager.AppSettings["FireBaseKey"]; }
        }
        public static string PinCheck
        { get { return ConfigurationManager.AppSettings["PinCheck"]; } }
        public static string PasswordSetAUTO
        { get { return ConfigurationManager.AppSettings["PasswordsetorAuto"]; } }

        public static string PREPAID
        { get { return ConfigurationManager.AppSettings["PREPAID"]; } }

        public static string POSTPAID
        { get { return ConfigurationManager.AppSettings["POSTPAID"]; } }

        public static string DTH
        { get { return ConfigurationManager.AppSettings["DTH"]; } }

        public static string ELECTRICITY
        { get { return ConfigurationManager.AppSettings["ELECTRICITY"]; } }

        public static string GAS
        { get { return ConfigurationManager.AppSettings["GAS"]; } }

        public static string LANDLINE
        { get { return ConfigurationManager.AppSettings["LANDLINE"]; } }

        public static string BROADBAND
        { get { return ConfigurationManager.AppSettings["BROADBAND"]; } }
        public static string WATER
        { get { return ConfigurationManager.AppSettings["WATER"]; } }

        public static string MONEY
        { get { return ConfigurationManager.AppSettings["MONEY"]; } }
        public static string ApiDomainName
        { get { return ConfigurationManager.AppSettings["ApiDomainName"]; } }


        public enum MessageType
        {
            Info,
            Warning,
            Error,
            Success
        }

        public static string UploadResumeFolderPath
        {
            get
            {
                string folderPath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "Upload", "Resume");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                return folderPath;
            }
        }
    }
}