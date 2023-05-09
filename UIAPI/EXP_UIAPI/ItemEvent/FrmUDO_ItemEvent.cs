using EXP_UIAPI.APIService;
using EXP_UIAPI.DTO;
using SAPbobsCOM;
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

                    SAPbouiCOM.ChooseFromList cflUsers = myForm.ChooseFromLists.Item("CFL_OUSR");
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
                    case "1": CrearUsuarios(out bubbleEvent, formUID, pVal); break;
                    default: break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CrearUsuarios(out bool bubbleEvent, string formUID, SAPbouiCOM.ItemEvent pVal)
        {
            bubbleEvent = true;
            Form myForm = Globales.oAplication.Forms.Item(formUID);

            try
            {
                if(pVal.BeforeAction && myForm.Mode == BoFormMode.fm_ADD_MODE)
                {
                    int rpta = Globales.oAplication.MessageBox("Esta seguro de crear los usuarios ?", 2, "Sí", "No");

                    if(rpta != 1)
                    {
                        bubbleEvent = false;
                        return;
                    }

                    Matrix matrix = myForm.Items.Item("Item_7").Specific;
                    DBDataSource ds = myForm.DataSources.DBDataSources.Item("@VS_USR1");

                    if (matrix.RowCount == 0)
                        throw new Exception("No puede crear porque no hay usuarios");

                    bubbleEvent = UsuariosCreados(matrix, ds);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static bool UsuariosCreados(Matrix matrix, DBDataSource ds)
        {
            bool usuariosCreados = true;


            try
            {
                if (Globales.oCompány.InTransaction)
                    Globales.oCompány.EndTransaction(BoWfTransOpt.wf_RollBack);

                Globales.oCompány.StartTransaction();

                for (int i = 1; i <= 2; i++)
                {
                    try
                    {
                        string id = CrearUsuario(matrix, i);
                        ds.SetValue("U_VS_IDSAP", i -1, id);
                    }
                    catch (Exception ex)
                    {
                        Globales.oAplication.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);

                        //if (Globales.oCompány.InTransaction)
                        Globales.oCompány.EndTransaction(BoWfTransOpt.wf_RollBack);

                        usuariosCreados = false;
                        break;
                    }
                }

                if(usuariosCreados)
                {
                    Globales.oCompány.EndTransaction(BoWfTransOpt.wf_Commit);
                    matrix.LoadFromDataSource();
                }
                    


            }
            catch (Exception)
            {

                throw;
            }

            return usuariosCreados;
        }

        private static string CrearUsuario(Matrix matrix, int i)
        {
            try
            {
                Users oUsuario = Globales.oCompány.GetBusinessObject(BoObjectTypes.oUsers);

                oUsuario.UserCode = matrix.Columns.Item("Col_2").Cells.Item(i).Specific.Value + "_2";
                oUsuario.UserPassword = "1234";
                oUsuario.eMail = matrix.Columns.Item("Col_3").Cells.Item(i).Specific.Value;
                oUsuario.UserName = matrix.Columns.Item("Col_1").Cells.Item(i).Specific.Value;
                oUsuario.MobilePhoneNumber = matrix.Columns.Item("Col_5").Cells.Item(i).Specific.Value;
                oUsuario.UserFields.Fields.Item("U_VS_DIRE").Value = matrix.Columns.Item("Col_4").Cells.Item(i).Specific.Value;

                if (oUsuario.Add() != 0)
                    throw new Exception(Globales.oCompány.GetLastErrorDescription());

                return Globales.oCompány.GetNewObjectKey();
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
            Form myForm = null;
            if (!pVal.BeforeAction)
            {
                try
                {
                    myForm = Globales.oAplication.Forms.Item(formUID);
                    myForm.Freeze(true);

                    //lista de usuarios
                    List<User> listUsers = await APIUsuarios.GetUserList();
                    //LLENAR LA GRILLA

                    DBDataSource ds = myForm.DataSources.DBDataSources.Item("@VS_USR1");
                    int i = 0;

                    ds.Clear();

                    foreach (User usuario in listUsers)
                    {
                        try
                        {
                            ds.InsertRecord(i);
                            ds.SetValue("LineId", i, (i + 1).ToString());
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

                    //int filaSeleccionada = matrixUsuarios.GetNextSelectedRow(0, BoOrderType.ot_RowOrder);
                    //int filas[];
                    
                    //while (filaSeleccionada != -1)
                    //{
                    //    //fila[0] = matrixUsuarios.GetNextSelectedRow(0, BoOrderType.ot_RowOrder);
                    //}


                    matrixUsuarios.LoadFromDataSource();
                    matrixUsuarios.AutoResizeColumns();
                }
                catch (Exception)
                {
                    throw;
                }
                finally { myForm.Freeze(false); }
            }
        }
    }
}