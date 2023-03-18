using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN.Compras
{
    internal class SAPFacturaProveedores
    {
        public Company MyCompany { get; set; }
        public Documents MyDocument { get; set; }

        public SAPFacturaProveedores(Company company)
        {
            MyCompany = company;
        }

        public int Crear()
        {
            try
            {
                MyDocument = MyCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                MyDocument.DocDate = DateTime.Now;
                MyDocument.DocDueDate = DateTime.Now;
                MyDocument.TaxDate = DateTime.Now;

                MyDocument.Lines.BaseEntry = 774;
                MyDocument.Lines.BaseLine = 0;
                MyDocument.Lines.BaseType = 20;
                MyDocument.Lines.Add();

                MyDocument.Lines.BaseEntry = 774;
                MyDocument.Lines.BaseLine = 1;
                MyDocument.Lines.BaseType = 20;
                MyDocument.Lines.Add();

                MyDocument.Comments = "CREADO POR SDK";

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
                if(!MyDocument.GetByKey(docEntry))
                    throw new Exception(MyCompany.GetLastErrorDescription());

                DateTime fechaActual = DateTime.Now;

                MyDocument.DocDueDate= new DateTime(fechaActual.Year, fechaActual.Month, DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month));
                MyDocument.NumAtCard = "01-F001-257845";
                MyDocument.UserFields.Fields.Item("U_BPV_SERI").Value = "F001";
                MyDocument.UserFields.Fields.Item("U_BPP_MDTD").Value = "01";
                MyDocument.UserFields.Fields.Item("U_BPV_NCON2").Value = "257845";

                MyDocument.Comments = "COMENTARIO ACTUALIZADO";

                if(MyDocument.Update() != 0)
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
