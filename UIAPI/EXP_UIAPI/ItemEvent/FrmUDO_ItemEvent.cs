﻿using EXP_UIAPI.APIService;
using EXP_UIAPI.DTO;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXP_UIAPI.ItemEvent
{
    internal static class FrmUDO_ItemEvent
    {
        internal static void HandleItemEvent(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                switch (pVal.EventType)
                {
                    case BoEventTypes.et_ITEM_PRESSED:
                        
                        ItemPressed_ItemEvent(formUID, pVal, out bubbleEvent); //USUARIO CLICK
                        break;

                    case BoEventTypes.et_CHOOSE_FROM_LIST:
                        ChooseFromList_Event(formUID, pVal, out bubbleEvent);
                        break;

                    case BoEventTypes.et_COMBO_SELECT: ComboSelect_Event(formUID, pVal, out bubbleEvent); break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ComboSelect_Event(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                switch (pVal.ItemUID)
                {
                    case "Item_2": SeleccionarSerie(formUID, pVal, out bubbleEvent); break;
                    //case "Item_3": SeleccionarNuevoCombo(formUID, pVal, out bubbleEvent); break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void SeleccionarSerie(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            try
            {
                bubbleEvent = true;
                Form form = Globales.oAplication.Forms.Item(formUID);

                if (!pVal.BeforeAction)
                {
                    ComboBox cb = form.Items.Item(pVal.ItemUID).Specific;
                    string serie = cb.Selected.Value;
                    string correlativo = form.BusinessObject.GetNextSerialNumber(serie, "VS_OUSR").ToString();
                    form.DataSources.DBDataSources.Item("@VS_OUSR").SetValue("DocNum", 0, correlativo);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ChooseFromList_Event(string formUID, SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                switch (pVal.ItemUID)
                {
                    case "Item_4": GetCFLUsuarios(formUID, pVal); break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetCFLUsuarios(string formUID, SAPbouiCOM.ItemEvent pVal)
        {
            try
            {
                Form myForm = Globales.oAplication.Forms.Item(formUID);

                if (pVal.BeforeAction)
                {
                    //FILTRAR

                    ChooseFromList cflUsers = myForm.ChooseFromLists.Item("CFL_OUSR");
                    cflUsers.SetConditions(null);
                    Conditions conditions = cflUsers.GetConditions();
                    Condition cnd = conditions.Add();

                    cnd.Alias = "Department";
                    cnd.Operation = BoConditionOperation.co_EQUAL;
                    cnd.CondVal = "22";

                    cflUsers.SetConditions(conditions);
                }

                if (!pVal.BeforeAction)
                {
                    //COLOCAR EL DATO DEL CFL AL FORMULARIO
                    var cflEvnt = (ChooseFromListEvent)pVal;

                    if (cflEvnt.SelectedObjects is DataTable dtbl)
                    {
                        try { myForm.Items.Item("Item_4").Specific.Value = dtbl.GetValue("USERID", 0).ToString(); } catch (Exception) { }
                    }
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
                    case "Item_6": GetList(formUID, pVal).Wait(); break;
                    default: break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static async Task GetById(string formUID, SAPbouiCOM.ItemEvent pVal)
        {
            if (!pVal.BeforeAction)
            {
                string idUsuario;
                Form myForm = Globales.oAplication.Forms.Item(formUID);

                idUsuario = myForm.Items.Item("Item_4").Specific.Value;
                if (string.IsNullOrEmpty(idUsuario))
                    throw new Exception("Debe indicar un id de usuario");

                User user = await APIUsuarios.GetUserById(idUsuario);

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
                //LLENAR LA GRILLA

                DBDataSource ds = myForm.DataSources.DBDataSources.Item("@VS_USR1");
                int i = 0;

                foreach (User usuario in listUsers)
                {
                    try
                    {
                        ds.InsertRecord(i);
                        ds.SetValue("U_VS_IDUS", i, usuario.id.ToString());
                        ds.SetValue("U_VS_NOMB", i, usuario.name.ToString());
                        ds.SetValue("U_VS_USUA", i, usuario.username.ToString());
                        ds.SetValue("U_VS_CORR", i, usuario.email.ToString());
                        ds.SetValue("U_VS_DIREC", i, usuario.address.street.ToString());
                        ds.SetValue("U_VS_TELF", i, usuario.phone.ToString());
                        ds.SetValue("U_VS_COMP", i, usuario.company.name.ToString());

                        //dt.Rows.Add(i + 1);
                        //dt.SetValue("ID", i, usuario.id.ToString());
                        //dt.SetValue("Nombre", i, usuario.name);
                        //dt.SetValue("Usuario", i, usuario.username);
                        //dt.SetValue("Correo", i, usuario.email);
                        //dt.SetValue("Direccion", i, usuario.address.street);
                        //dt.SetValue("Telefono", i, usuario.phone);
                        //dt.SetValue("Compania", i, usuario.company.name);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    i++;
                }

                Matrix matrixUsuarios = myForm.Items.Item("Item_7").Specific;
                matrixUsuarios.LoadFromDataSource();
                matrixUsuarios.AutoResizeColumns();
            }
        }
    }
}