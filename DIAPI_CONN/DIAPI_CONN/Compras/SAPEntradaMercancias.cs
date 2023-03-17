using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN.Compras
{
    public class SAPEntradaMercancias
    {
        public Company MyCompany { get; set; }
        public Documents MyDocument { get; set; }

        public SAPEntradaMercancias(Company company)
        {
            MyCompany = company;
        }

        public int Crear()
        {
            try
            {
                MyDocument = MyCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

                MyDocument.CardCode = "PL10082747082";
                MyDocument.TaxDate = DateTime.Now;
                MyDocument.DocDate = DateTime.Now;
                MyDocument.DocDueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                MyDocument.Lines.BaseEntry = 1467; //DOCENTRY DEL DOCUMENTO BASE
                MyDocument.Lines.BaseLine = 0; //NÚMERO DE LINEA DEL DOCUMENTO BASE
                MyDocument.Lines.BaseType = 22; //TIPO DE DOCUMENTO BASE
                MyDocument.Lines.Quantity = 15;

                MyDocument.Lines.BatchNumbers.BatchNumber = "20230301601";
                MyDocument.Lines.BatchNumbers.Quantity = 15;
                MyDocument.Lines.BatchNumbers.Add();

                MyDocument.Lines.Add();

                MyDocument.Lines.BaseEntry = 1467; //DOCENTRY DEL DOCUMENTO BASE
                MyDocument.Lines.BaseLine = 1; //NÚMERO DE LINEA DEL DOCUMENTO BASE
                MyDocument.Lines.BaseType = 22; //TIPO DE DOCUMENTO BASE
                MyDocument.Lines.Quantity = 5;

                MyDocument.Lines.BatchNumbers.BatchNumber = "20230301602";
                MyDocument.Lines.BatchNumbers.Quantity = 5;
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
