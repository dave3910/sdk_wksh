using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIAPI_CONN.Bancos
{
    internal class SAPPagosEfectuados
    {
        public Company MyCompany { get; set; }
        public Payments MyPayment { get; set; }

        public SAPPagosEfectuados(Company company)
        {
            MyCompany = company;
        }

        public int CrearPago()
        {
            try
            {
                MyPayment = MyCompany.GetBusinessObject(BoObjectTypes.oVendorPayments);

                MyPayment.CardCode = "PL10082747082";
                MyPayment.TaxDate = DateTime.Now;
                MyPayment.DueDate = DateTime.Now;
                MyPayment.DocCurrency = "SOL";

                MyPayment.JournalRemarks = "DATO DE ASIENTO";
                MyPayment.Remarks = "COMENTARIO DE PAGO";

                //FACTURA PROVEEDORES
                MyPayment.Invoices.DocEntry = 1817;
                MyPayment.Invoices.InvoiceType = BoRcptInvTypes.it_PurchaseInvoice;
                MyPayment.Invoices.SumApplied = 50;
                MyPayment.Invoices.Add();

                //nota debito proveedores
                MyPayment.Invoices.DocEntry = 1821;
                MyPayment.Invoices.InvoiceType = BoRcptInvTypes.it_PurchaseInvoice;
                MyPayment.Invoices.SumApplied = 400;
                MyPayment.Invoices.Add();

                //ASIENTOS
                MyPayment.Invoices.DocEntry = 5366;
                //MyPayment.Invoices.DocLine = 1;
                MyPayment.Invoices.InvoiceType = BoRcptInvTypes.it_JournalEntry;
                
                //MyPayment.Invoices.SumApplied = 1050;
                MyPayment.Invoices.AppliedFC = 1050;
                MyPayment.Invoices.Add();

                //CHECKES-- COLECCIÓN(500)

                //MyPayment.Checks.DueDate = DateTime.Now;
                //MyPayment.Checks.CheckSum = 500;
                //MyPayment.Checks.CountryCode = "PE";
                
                //MyPayment.Checks.BankCode = "IBK";
                //MyPayment.Checks.CheckAccount = "200-3003984824";
                //MyPayment.Checks.AccounttNum = "104108";
                //MyPayment.Checks.Add();


                //TRANSFERENCIA (200)
                MyPayment.TransferAccount = "104103";
                MyPayment.TransferDate = DateTime.Now;
                MyPayment.TransferReference = "1540254";
                MyPayment.TransferSum = 200;


                //TARJETAS DE CREDITO -- COLECCIÓN (500)
                MyPayment.CreditCards.CreditCard = 1;
                MyPayment.CreditCards.CreditAcct = "104108";
                MyPayment.CreditCards.CreditSum = 500;
                MyPayment.CreditCards.NumOfCreditPayments = 1;
                MyPayment.CreditCards.Add();

                //EFECTIVO (300)
                MyPayment.CashAccount = "104108";
                MyPayment.CashSum = 300;


                if (MyPayment.Add() != 0)
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
