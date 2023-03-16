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

                //MyItem.WhsInfo.WarehouseCode = "01";
                //MyItem.WhsInfo.
                //...

                if (MyItem.Add() != 0)
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
