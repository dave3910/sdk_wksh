using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Company oCompany = new Company();

                //DATOS DE SERVIDOR
                oCompany.Server = "NDB@192.168.1.14:30013"; //IP O EL HOSTNAME
                oCompany.DbServerType = BoDataServerTypes.dst_HANADB;
                oCompany.CompanyDB = "01_ARENUVA_PRODUCCION";
                oCompany.DbUserName = "SYSTEM";
                oCompany.DbPassword = "HANAB1Admin";

                //DATOS DE USUARIO SAP
                oCompany.UserName = "Desarrollo4";
                oCompany.Password = "1234";

                oCompany.language = BoSuppLangs.ln_Spanish_La;

                if (oCompany.Connect() != 0)
                    throw new Exception(oCompany.GetLastErrorDescription());


                Console.WriteLine($"Conectado con la sociedad {oCompany.CompanyDB}");

                Console.WriteLine("DATOS MAESTROS - SOCIOS DE NEGOCIO");

                //SOCIOS DE NEGOCIO

                Console.WriteLine("SOCIOS DE NEGOCIO - OPERACIÓN GET");
                //LECTURA (OBTENCIÓN DE DATOS)
                BusinessPartners bp = oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                if (!bp.GetByKey("PL45800996"))
                    throw new Exception("El socio PL45800996 no está registrado en la BD");

                

                Console.WriteLine($"Hola {bp.CardName}");
                
                string nombre = bp.CardName;
                string direccion = bp.Address;
                string cuenta = bp.AgentCode;
                //.....

                string xml = bp.GetAsXML();



                //ACTUALIZACIÓN(OBTENCIÓN DEL REGISTRO + ACTUALIZACIÓN)

                //CREACIÓN DE SOCIO

                //ELIMINACIÓN DE SOCIO

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
