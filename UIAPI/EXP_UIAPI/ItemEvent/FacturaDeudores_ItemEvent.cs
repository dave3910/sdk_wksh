using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXP_UIAPI.ItemEvent
{
    internal static class FacturaDeudores_ItemEvent
    {
        internal static void HandleItemEvent(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                switch (pVal.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:SaludarUsuario(formUID, pVal, out bubbleEvent); break;
                    case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED: Click_ItemEvent(formUID, pVal, out bubbleEvent); //USUARIO CLICK
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void Click_ItemEvent(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            try
            {
                string type = pVal.FormTypeEx;
                string formuid = pVal.FormUID;


                bubbleEvent = true;

                switch (pVal.ItemUID)
                {
                    case "2": PedirConfirmacionCerrar(formUID,  pVal, out  bubbleEvent); break;

                    default:
                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void PedirConfirmacionCerrar(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            try
            {
                bubbleEvent = true;

                if (pVal.BeforeAction)
                {

                    bubbleEvent = !TieneDatos(formUID);
                    if (!bubbleEvent)
                        Globales.oAplication.MessageBox("No puede cerrar el formulario, porque este tiene un comentario");

                   //int rpta = Globales.oAplication.MessageBox("Está a punto de cerrar el formulario. ¿Desea cerrarlo?", 2, "Sí", "No");
                   
                   // if(rpta != 1)
                   //     bubbleEvent = false;
                }

                //if (!pVal.BeforeAction)
                //{
                //    Globales.oAplication.MessageBox("Formulario cerrado");
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool TieneDatos(string formUID)
        {
            try
            {
                Form myForm = Globales.oAplication.Forms.Item(formUID);

                Item cajaComentario = myForm.Items.Item("16");
                string texto = cajaComentario.Specific.Value;

                return !string.IsNullOrEmpty(texto);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void SaludarUsuario(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent =true;

            try
            {
                if(!pVal.BeforeAction)
                {
                    Globales.oAplication.MessageBox($"Hola {Globales.oCompány.UserName}");
                }

                //switch (pVal.ItemUID)
                //{
                //    //"BOTON1" : PULSOBOTON1();
                //    //"BOTON2" : PULSOBOTON2();
                //    //"BOTON3" : PULSOBOTON3();
                        //"BOTON4" : PULSOBOTON4();
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
