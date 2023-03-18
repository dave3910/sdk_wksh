using SAPbobsCOM;
using System;

namespace DIAPI_CONN.Compras
{
    internal class SAPNotaDebitoProveedores
    {
        public Company MyCompany { get; set; }
        public Documents MyDocument { get; set; }

        public SAPNotaDebitoProveedores(Company company)
        {
            MyCompany = company;
        }

        public int Crear()
        {
            try
            {
                MyDocument = MyCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                MyDocument.DocumentSubType = BoDocumentSubType.bod_PurchaseDebitMemo;

                MyDocument.CardCode = "PL10082747082";
                MyDocument.TaxDate = DateTime.Now;
                MyDocument.DocDate = DateTime.Now;
                MyDocument.DocDueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                MyDocument.DocType = BoDocumentTypes.dDocument_Items;

                MyDocument.UserFields.Fields.Item("U_VS_AFEDET").Value = "Y";
                MyDocument.UserFields.Fields.Item("U_VS_PORDET").Value = 10.5;

                MyDocument.Lines.ItemCode = "1301033000000001";
                MyDocument.Lines.Quantity = 20;
                MyDocument.Lines.Price = 25;
                MyDocument.Lines.TaxCode = "I18";
                MyDocument.Lines.UserFields.Fields.Item("U_VS_TIPAFE").Value = "AFECTA";

                MyDocument.Lines.BatchNumbers.BatchNumber = "20230301601";
                MyDocument.Lines.BatchNumbers.Quantity = 20;
                MyDocument.Lines.BatchNumbers.Add();

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

        public bool Actualizar(int docEntry)
        {
            try
            {
                MyDocument = MyCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                if (!MyDocument.GetByKey(docEntry))
                    throw new Exception(MyCompany.GetLastErrorDescription());

                DateTime fechaActual = DateTime.Now;

                MyDocument.DocDueDate = new DateTime(fechaActual.Year, fechaActual.Month, DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month));
                MyDocument.NumAtCard = "01-F001-257845";
                MyDocument.UserFields.Fields.Item("U_BPV_SERI").Value = "F001";
                MyDocument.UserFields.Fields.Item("U_BPP_MDTD").Value = "01";
                MyDocument.UserFields.Fields.Item("U_BPV_NCON2").Value = "257845";

                MyDocument.Comments = "COMENTARIO ACTUALIZADO";

                if (MyDocument.Update() != 0)
                    throw new Exception(MyCompany.GetLastErrorDescription());

                return true;
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
    }
}