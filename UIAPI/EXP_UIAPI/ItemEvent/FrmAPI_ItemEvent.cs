using EXP_UIAPI.APIService;
using EXP_UIAPI.DTO;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXP_UIAPI.ItemEvent
{
    internal static class FrmAPI_ItemEvent
    {
        internal static void HandleItemEvent(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                switch (pVal.EventType)
                {
                    //case SAPbouiCOM.BoEventTypes.et_FORM_LOAD: SaludarUsuario(formUID, pVal, out bubbleEvent); break;
                    case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED: ItemPressed_ItemEvent(formUID, pVal, out bubbleEvent); //USUARIO CLICK
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

        private static void ItemPressed_ItemEvent(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                switch (pVal.ItemUID)
                {
                    case "Item_5": GetById(formUID, pVal).Wait(); break;
                    case "Item_6": GetList(formUID, pVal).Wait(); break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        private static async Task GetById(string formUID, SAPbouiCOM.ItemEvent pVal)
        {
            

            if(!pVal.BeforeAction)
            {
                string idUsuario;
                Form myForm = Globales.oAplication.Forms.Item(formUID);

                idUsuario = myForm.Items.Item("Item_4").Specific.Value;
                if (string.IsNullOrEmpty(idUsuario))
                    throw new Exception("Debe indicar un id de usuario");

                 User user =  await APIUsuarios.GetUserById(idUsuario);

                Globales.oAplication.MessageBox($"Nombre:  { user.name} . Usuario: {user.username} . Correo: {user.email} ");            

                //cargar en grilla (
            }
        }

        private static async Task GetList(string formUID, SAPbouiCOM.ItemEvent pVal)
        {


            if (!pVal.BeforeAction)
            {
                Form myForm = Globales.oAplication.Forms.Item(formUID);
                

                //lista de usuarios
                List<User> listUsers = await APIUsuarios.GetUserList();
                //Globales.oAplication.MessageBox($"Nombre:  {user.name} . Usuario: {user.username} . Correo: {user.email} ");

            }
        }
    }
}
