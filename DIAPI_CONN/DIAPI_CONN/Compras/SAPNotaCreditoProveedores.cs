using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN.Compras
{
    internal class SAPNotaCreditoProveedores
    {
        public Company MyCompany { get; set; }
        public Documents MyDocument { get; set; }

        public SAPNotaCreditoProveedores(Company company)
        {
            MyCompany = company;
        }

        public int Crear()
        {
            try
            {
                MyDocument = MyCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);
                MyDocument.DocDate = DateTime.Now;
                MyDocument.DocDueDate = DateTime.Now;
                MyDocument.TaxDate = DateTime.Now;

                MyDocument.Lines.BaseEntry = 1817;
                MyDocument.Lines.BaseLine = 0;
                MyDocument.Lines.BaseType = 18;

                MyDocument.Lines.BatchNumbers.BatchNumber = "20230301601";
                MyDocument.Lines.BatchNumbers.Quantity = 15;
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
    }
}
