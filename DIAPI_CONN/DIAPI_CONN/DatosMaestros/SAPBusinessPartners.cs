using SAPbobsCOM;
using System;

namespace DIAPI_CONN
{
    public class SAPBusinessPartners
    {
        public Company MyCompany { get; set; }
        public BusinessPartners MyBP { get; set; }

        public SAPBusinessPartners(Company company)
        {
            MyCompany = company;
        }

        //LECTURA DE DATOS
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
                //Marshal.ReleaseComObject(MyBP);
                //MyBP = null;
                //GC.Collect();
                throw;
            }
        }

        //ACTUALIZACIÓN DE DATOS (OBTENCIÓN DE REGISTRO + UPDATE)

        public bool ActualizarNombre(string codigo, string nuevoNombre)
        {
            try
            {
                MyBP = MyCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (!MyBP.GetByKey(codigo))
                    throw new Exception($"El socio {codigo} no está registrado en la BD");

                MyBP.CardName = nuevoNombre;

                if (MyBP.Update() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public bool ActualizarDireccion(string codigoSocio, string idDireccion, string calle, string distrito)
        {
            try
            {
                MyBP = MyCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (!MyBP.GetByKey(codigoSocio))
                    throw new Exception($"El socio {codigoSocio} no está registrado en la BD");

                //UBICAR LA DIRECCIÓN (2 ALTERNATIVA)
                //1: RECORRER LAS DIRECCIONES Y UBICAR LA DIRECCIÓN REQUERIDA
                //2: CONSULTAR EL ID UNICO DE LA DIRECCIÓN REQUERIDA

                for (int i = 0; i < MyBP.Addresses.Count; i++)
                {
                    MyBP.Addresses.SetCurrentLine(i);

                    if (MyBP.Addresses.AddressName == idDireccion)
                    {
                        MyBP.Addresses.Street = calle;
                        MyBP.Addresses.County = distrito;
                        break;
                    }
                }

                if (MyBP.Update() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public bool ActualizarCampoUsuario(string codigoSocio, string ID_campo, dynamic valor)
        {
            try
            {
                MyBP = MyCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (!MyBP.GetByKey(codigoSocio))
                    throw new Exception($"El socio {codigoSocio} no está registrado en la BD");

                MyBP.UserFields.Fields.Item(ID_campo).Value = valor;

                if (MyBP.Update() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public bool CrearNuevoSocio()
        {
            try
            {
                MyBP = MyCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                //ASIGNACIÓN DE VALORES
                MyBP.CardCode = "C55687416";
                MyBP.CardName = "KENNEDY VERGARAY";
                MyBP.CardType = BoCardTypes.cCustomer;
                MyBP.FederalTaxID = "55687416";

                MyBP.Addresses.AddressName = "FISCAL";
                MyBP.Addresses.AddressType = BoAddressType.bo_BillTo;
                MyBP.Addresses.Street = "JR. FISCAL 4570 - URB SAN LUIS";
                MyBP.Addresses.County = "SAN BORJA";

                MyBP.Addresses.Add();

                MyBP.Addresses.AddressName = "ALMACEN";
                MyBP.Addresses.AddressType = BoAddressType.bo_ShipTo;
                MyBP.Addresses.Street = "JR. FISCAL 4570 - URB SAN LUIS";
                MyBP.Addresses.County = "LOS OLIVOS";

                MyBP.Addresses.Add();

                MyBP.UserFields.Fields.Item("U_VS_AVALNOM").Value = "NOMBRE AVAL CREACIÓN";
                MyBP.UserFields.Fields.Item("U_VS_AVALDIR").Value = "DIRECCIÓN AVAL CREACIÓN";

                if (MyBP.Add() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        //ELIMINACIÓN DE REGISTRO (OBTENCIÓN DE REGISTRO + DELETE)
        public bool Eliminar(string codigoSocio)
        {
            try
            {
                MyBP = MyCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (!MyBP.GetByKey(codigoSocio))
                    throw new Exception($"El socio {codigoSocio} no está registrado en la BD");

                if (MyBP.Remove() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public void MostrarListaSociosReglaNegocio()
        {
            try
            {
                string nombre, codigo;
                string query = $"CALL SP_XPS_GETSOCIOS()";
                Recordset recordset = MyCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                recordset.DoQuery(query);

                if (recordset.RecordCount == 0)
                    throw new Exception("No se han encontrado socios");

                //for (int i = 0; i < recordset.RecordCount - 1; i++)
                //{
                //    nombre = recordset.Fields.Item("Nombre").Value;
                //    codigo = recordset.Fields.Item("Codigo").Value;


                //    Console.WriteLine(string.Join("-", codigo, nombre));
                //    recordset.MoveNext();
                //}

                while (!recordset.EoF)
                {
                    //articulo.colleccion.SetCurrentLine(i)
                    nombre = recordset.Fields.Item("Nombre").Value;
                    codigo = recordset.Fields.Item("Codigo").Value;


                    Console.WriteLine(string.Join("-", codigo, nombre));
                    recordset.MoveNext();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}