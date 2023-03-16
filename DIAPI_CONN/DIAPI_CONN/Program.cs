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
                ConecctionManager cnxMngr = new ConecctionManager("NDB@192.168.1.14:30013", "01_ARENUVA_PRODUCCION", "SYSTEM", "HANAB1Admin", "Desarrollo4", "1234");
                cnxMngr.GetConnection();

                Console.WriteLine("DATOS MAESTROS - SOCIOS DE NEGOCIO");

                //SOCIOS DE NEGOCIO
                Console.WriteLine("SOCIOS DE NEGOCIO - OPERACIÓN GET");

                //LECTURA (OBTENCIÓN DE DATOS)
                SAPBusinessPartners bp = new SAPBusinessPartners(cnxMngr.MyCompany);
                SAPItems oItem = new SAPItems(cnxMngr.MyCompany);
                //bp.GetBPInfo("PL45800996");

                //if (bp.ActualizarNombre("PL45800996", "LOPEZ RODRIGUEZ JORGE ARMANDO"))
                //    Console.WriteLine("SOCIO ACTUALIZADO CORRECTAMENTE");

                //if(bp.ActualizarDireccion("PL45800996", "DIR2", "CALLE ACTUALIZADA", "SAN LUIS"))
                //    Console.WriteLine("DIRECCIÓN ACTUALIZADA CORRECTAMENTE");

                //if(bp.ActualizarCampoUsuario("PL45800996", "U_VS_AVALNOM", "NOMBRE AVAL ACTUALIZADO"))
                //    Console.WriteLine("CAMPO DE USUARIO ACTUALIZADO CORRECTAMENTE");

                //if (bp.ActualizarCampoUsuario("PL45800996", "U_VS_AVALDIR", "DIRECCION AVAL ACTUALIZADO"))
                //    Console.WriteLine("CAMPO DE USUARIO ACTUALIZADO CORRECTAMENTE");

                //if(bp.CrearNuevoSocio())
                //    Console.WriteLine("SOCIO DE NEGOCIO CREADO CORRECTAMENTE");

                //if(bp.Eliminar("C55687416"))
                //    Console.WriteLine("SOCIO DE NEGOCIO ELIMINADO CORRECTAMENTE");

                if(oItem.Crear("ITEM02221545", "ARTICULO DE PRUEBA SDK"))
                    Console.WriteLine("ARTICULO CREADO CORRECTAMENTE");

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
