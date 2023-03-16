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
                ConecctionManager cnxMngr = new ConecctionManager("NDB@192.168.1.14:30013", "01_ARENUVA_PRODUCCION", "SYSTEM", "HANAB1Admin", "Desarrollo4", "1234") ;
                cnxMngr.GetConnection();

                Console.WriteLine("DATOS MAESTROS - SOCIOS DE NEGOCIO");

                //SOCIOS DE NEGOCIO
                Console.WriteLine("SOCIOS DE NEGOCIO - OPERACIÓN GET");

                //LECTURA (OBTENCIÓN DE DATOS)
                SAPBusinessPartners bp = new SAPBusinessPartners(cnxMngr.MyCompany);
                BusinessPartners oBp = bp.GetBPInfo("PL45800996");



                Console.WriteLine($"Hola {oBp.CardName}");

                Console.WriteLine("");


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
