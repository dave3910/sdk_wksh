using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN.Compras
{
    public class SAPOrdenDeCompra
    {
        public Company MyCompany { get; set; }
        public Documents MyDocument { get; set; }

        public SAPOrdenDeCompra(Company company)
        {
            MyCompany = company;
        }

        public int Crear()
        {
            try
            {
                MyDocument = MyCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders);

                MyDocument.CardCode = "PL10082747082";
                MyDocument.TaxDate = DateTime.Now;
                MyDocument.DocDate = DateTime.Now;
                MyDocument.DocDueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) );

                MyDocument.DocType = BoDocumentTypes.dDocument_Items;

                MyDocument.UserFields.Fields.Item("U_VS_AFEDET").Value = "Y";
                MyDocument.UserFields.Fields.Item("U_VS_PORDET").Value = 10.5;

                MyDocument.Lines.ItemCode = "1301033000000001";
                MyDocument.Lines.Quantity = 20;
                MyDocument.Lines.Price = 25;
                MyDocument.Lines.TaxCode = "I18";
                MyDocument.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "AFECTA";
                MyDocument.Lines.Add();

                MyDocument.Lines.ItemCode = "1001002000000008";
                MyDocument.Lines.Quantity = 15;
                MyDocument.Lines.Price = 10;
                MyDocument.Lines.TaxCode = "I18";
                MyDocument.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "AFECTA 2";
                MyDocument.Lines.Add();


                if (MyDocument.Add() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());

                return Convert.ToInt32(MyCompany.GetNewObjectKey());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
