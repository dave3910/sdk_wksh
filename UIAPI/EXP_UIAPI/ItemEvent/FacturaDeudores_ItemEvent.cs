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
                    
                    case SAPbouiCOM.BoEventTypes.et_CLICK: //USUARIO CLICK;


                        //
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
