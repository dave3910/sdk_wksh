using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN
{
    public class SAPBusinessPartners
    {
        public Company MyCompany{ get; set; }
        public BusinessPartners MyBP { get; set; }

        public SAPBusinessPartners(Company company)
        {
            MyCompany = company;
        }

        public void GetBPInfo(string codigo)
        {
            try
            {

                MyBP = MyCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                if (!MyBP.GetByKey(codigo))
                    throw new Exception($"El socio {codigo} no está registrado en la BD");

                //CAMPO NATIVO EN ESPECÍFICO
                Console.WriteLine($"Nombre: {MyBP.CardName}");
                
                //OBJETO DEPENDIENTE (COLECCIÓN)
                
                Console.WriteLine("DIRECCIONES: ");

                for (int i = 0; i < MyBP.Addresses.Count; i++)
                {
                    MyBP.Addresses.SetCurrentLine(i);
                    string tipoDireccion = MyBP.Addresses.AddressType == BoAddressType.bo_BillTo ? "FACTURACION" : "ALMACEN";
                    
                    Console.WriteLine($"ID Dirección: {MyBP.Addresses.AddressName}");
                    Console.WriteLine($"Tipo Dirección: { tipoDireccion}");
                    Console.WriteLine($"Calle: {MyBP.Addresses.Street}");
                    Console.WriteLine($"Provincia: {MyBP.Addresses.City}");

                    //LISTA[i].ATRIBUTO 1, LISTA.GETELEMENTAT[i]
                }

                //CAMPOS DE USUARIO

                Console.WriteLine("ALGUNOS CAMPOS DE USUARIO: ");
                Console.WriteLine($"Nombre de aval: {MyBP.UserFields.Fields.Item("U_VS_AVALNOM").Value}");
                Console.WriteLine($"Dirección de aval: {MyBP.UserFields.Fields.Item("U_VS_AVALDIR").Value}");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
