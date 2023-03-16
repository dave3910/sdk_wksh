using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN
{
    public class ConecctionManager
    {
        public string Servidor { get; set; }
        public string BD { get; set; }
        public string DBUser { get; set; }
        public string DBPass { get; set; }
        public string SAPUser { get; set; }
        public string SAPPass { get; set; }

        public Company MyCompany { get; set; }

        public ConecctionManager(string servidor, string db, string dbuser, string dbpass, string sapUsr, string sapPass)
        {
            Servidor = servidor; 
            BD = db;
            DBPass = dbpass;
            DBPass = dbpass;
            SAPUser = sapUsr;
            SAPPass = sapPass;
        }

        public bool GetConnection()
        {
            try
            {
                MyCompany = new Company();

                //DATOS DE SERVIDOR
                MyCompany.Server = Servidor; // "NDB@192.168.1.14:30013"; //IP O EL HOSTNAME
                MyCompany.DbServerType = BoDataServerTypes.dst_HANADB;
                MyCompany.CompanyDB = BD; //"01_ARENUVA_PRODUCCION";
                MyCompany.DbUserName = DBUser; //"SYSTEM";
                MyCompany.DbPassword = DBPass; //"HANAB1Admin";

                //DATOS DE USUARIO SAP
                MyCompany.UserName = SAPUser; // "Desarrollo4";
                MyCompany.Password = SAPPass; // "1234";

                MyCompany.language = BoSuppLangs.ln_Spanish_La;

                if (MyCompany.Connect() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());


                Console.WriteLine($"Conectado con la sociedad {MyCompany.CompanyDB}");

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
