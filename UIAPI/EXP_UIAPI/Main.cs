using EXP_UIAPI.ItemEvent;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EXP_UIAPI
{
    internal class Main
    {
       
        public Application sboApplication { get; set; }
        public SAPbobsCOM.Company sboCompany { get; set; }


        public Main()
        {
            //CONECTAR CON UI API - SAP BO
            ConectarUIAPI();
            //OBTENER CONEXIÓN DI API
            ConectarDIAPI();
            //CONFIGURAR FILTROS
            ConfigurarFiltros();
            //IMPLEMENTAR LOS EVENTOS PRINCIPALES (ITEMEVENT, FORMDATAEVENT, APPEVENT)
            ConfigurarEventosPrincipales();

            CrearMenues();
        }

        private void CrearMenues()
        {
            XmlDocument xmlDocument = new XmlDocument();
            string rutaMenuXML = string.Empty;
            try
            {
                sboApplication.Forms.GetFormByTypeAndCount(169, 1).Freeze(true);
                rutaMenuXML = Path.Combine(System.Windows.Forms.Application.StartupPath, "Menues", "Menu.xml");
                xmlDocument.Load(rutaMenuXML);
                sboApplication.LoadBatchActions(xmlDocument.InnerXml);

                SAPbouiCOM.MenuItem menu = sboApplication.Menus.Item("MNUID_CRUC");
                menu.Image = Path.Combine(System.Windows.Forms.Application.StartupPath, "invoice_15.jpg");
            }
            catch (FileNotFoundException)
            {
                //sboApplication.StatusBarErrorMsg("El recurso menu.xml, no fue encontrado");
            }
            catch { throw; }
            finally
            {
                sboApplication.Forms.GetFormByTypeAndCount(169, 1).Freeze(false);
                sboApplication.Forms.GetFormByTypeAndCount(169, 1).Update();
            }
        }

        private void ConfigurarEventosPrincipales()
        {
            //sboApplication.AppEvent += SboApplication_AppEvent;
            sboApplication.ItemEvent += SboApplication_ItemEvent;
            //sboApplication.FormDataEvent += SboApplication_FormDataEvent;
            //sboApplication.MenuEvent += SboApplication_MenuEvent;
        }

        private void SboApplication_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            try
            {
                BubbleEvent = true;

                switch (pVal.FormTypeEx)
                {
                    case "133": FacturaDeudores_ItemEvent.HandleItemEvent(FormUID, pVal, out BubbleEvent); break;
                    case "139": OrdenesVenta_ItemEvent.HandleItemEvent(FormUID, pVal, out BubbleEvent); break;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        //private void SboApplication_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        //{

        //}

        //private void SboApplication_FormDataEvent(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        //{

        //}



        //private void SboApplication_AppEvent(BoAppEventTypes EventType)
        //{

        //}


        private void ConfigurarFiltros()
        {
            SAPbouiCOM.EventFilters oFilters = new SAPbouiCOM.EventFilters();
            SAPbouiCOM.EventFilter oFilter;

            ////EJERCICIO 1: AL INICIAR EL FORMULARIO, QUE SAP ENVIE UN SALUDO AL USUARIO CON SU NOMBRE
            ////EJERCICIO 2: CUANDO DE CLICK AL BOTON CANCELAR, SE PIDA UNA CONFIRMACIÓN.


            oFilter = oFilters.Add(BoEventTypes.et_FORM_LOAD);
            oFilter.AddEx("133");
            //oFilter.AddEx("BOLETA");

            oFilter = oFilters.Add(BoEventTypes.et_ITEM_PRESSED);
            oFilter.AddEx("133"); //FACTURAS
            oFilter.AddEx("139"); //ORDENES DE VENTA
            //oFilter.AddEx("140"); //???
            //oFilter.AddEx("141"); //???
            //oFilter.AddEx("142"); //???
            ////....

            sboApplication.SetFilter(oFilters);
        }

        private void ConectarDIAPI()
        {
            //METODO 1 (MÁS SENCILLO A NIVEL DE PROGRAMACIÓN)
            //sboCompany = sboApplication.Company.GetDICompany();

            //METODO 2 (RECOMENDADO POR SAP)
            sboCompany = new SAPbobsCOM.Company();
            string cookie = sboCompany.GetContextCookie();
            string connStr = sboApplication.Company.GetConnectionContext(cookie);

            if (sboCompany.Connected)
                sboCompany.Disconnect();

            long ret;
            ret = sboCompany.SetSboLoginContext(connStr);

            if (ret != 0)
                throw new Exception("Login context failed");

            ret = sboCompany.Connect();


            Globales.oCompány = sboCompany;
            Globales.oAplication = sboApplication;
            //METODO 3 (SIN INCLUIR CONECCION CON EL APPLICATION)
            //sboCompany.Server = "";
            //sboCompany.UserName = "";
            //sboCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
        }

        private void ConectarUIAPI()
        {
            string connectionString = string.Empty;
            SboGuiApi sboGuiApi = null;
            try
            {
                sboGuiApi = new SboGuiApi();
                connectionString = Environment.GetCommandLineArgs().GetValue(Environment.GetCommandLineArgs().Length > 0 ? 1 : 0).ToString();
                sboGuiApi.Connect(connectionString);
                sboApplication = sboGuiApi.GetApplication(-1);
                if (sboApplication is null) throw new NullReferenceException();

                sboApplication.StatusBar.SetText("Iniciando add-on Xplora SAP...", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            }
            catch { throw; }
        }

    }
}
