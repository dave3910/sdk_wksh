using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN
{
    public class SAPItems
    {
        public Company MyCompany { get; set; }
        public Items MyItem { get; set; }

        public SAPItems(Company company)
        {
            MyCompany = company;
        }

        public bool Crear(string codigoArticulo, string nombre)
        {
            try
            {
                MyItem = MyCompany.GetBusinessObject(BoObjectTypes.oItems);

                MyItem.ItemCode = codigoArticulo;
                MyItem.ItemName = nombre;
                MyItem.ItemsGroupCode = 124;

                MyItem.InventoryItem = BoYesNoEnum.tYES;
                MyItem.SalesItem = BoYesNoEnum.tYES;
                MyItem.PurchaseItem = BoYesNoEnum.tYES;

                MyItem.ManageBatchNumbers = BoYesNoEnum.tYES;

                if (MyItem.Add() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public bool ActualizarArticulo(string codigoArticulo)
        {
            try
            {
                MyItem = MyCompany.GetBusinessObject(BoObjectTypes.oItems);
                if (!MyItem.GetByKey(codigoArticulo))
                    throw new Exception($"El artículo {codigoArticulo} no está registrado en la BD");

                MyItem.ItemName = "ARTICULO MODIFICADO";
                MyItem.UserFields.Fields.Item("U_VS_PDARNC").Value = "UPDT";

                for (int i = 0; i < MyItem.WhsInfo.Count; i++)
                {
                    MyItem.WhsInfo.SetCurrentLine(i);
                    if(MyItem.WhsInfo.WarehouseCode == "RSZAGO08")
                    {
                        MyItem.WhsInfo.Locked = BoYesNoEnum.tYES;
                        break;
                    }
                }

                if(MyItem.Update() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public bool Eliminar(string codigo)
        {
            try
            {
                MyItem = MyCompany.GetBusinessObject(BoObjectTypes.oItems);

                if (!MyItem.GetByKey(codigo))
                    throw new Exception($"El articulo {codigo} no está registrado en la BD");

                if (MyItem.Remove() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
    }
}
