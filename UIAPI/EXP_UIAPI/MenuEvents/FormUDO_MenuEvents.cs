using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXP_UIAPI.MenuEvents
{
    internal static class FormUDO_MenuEvents
    {
        const string PATH = "Recursos/Formularios/FormUDO.srf";
        const string TYPE = "FrmUDO";

        internal static void CrearFormulario()
        {
            try
            {
                var formUID = string.Concat(TYPE, new Random().Next(0, 1000));
                Globales.CrearFormulario(PATH, TYPE, formUID);


                CargarDatosPorDefecto(formUID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CargarDatosPorDefecto(string formUID)
        {
            Form form = Globales.oAplication.Forms.Item(formUID);

            try
            {
                //LLENADO DE INFORMACIÓN HACIA CONTROL
                string idUsuario = Globales.oCompány.UserSignature.ToString();
                ComboBox combo = form.Items.Item("Item_1").Specific;

                //3 FORMAS DE SELECCIONAR AL USUARIO CARLOS
                //comboUsuario.Select(2, BoSearchKey.psk_Index);
                //comboUsuario.Select("2", BoSearchKey.psk_ByValue);
                //comboUsuario.Select("Carlos", BoSearchKey.psk_ByDescription);


                combo.Select(idUsuario, BoSearchKey.psk_ByValue);

                //LLENADO DE INFORMACIÓN HACIA DBDATASOURCE
                DBDataSource ds = form.DataSources.DBDataSources.Item("@VS_OUSR");
                ds.SetValue("U_VS_FECH", 0, DateTime.Now.ToString("yyyyMMdd"));

                //CARGAR LAS SERIES AL COMBO
                combo = form.Items.Item("Item_2").Specific;
                combo.ValidValues.LoadSeries("VS_OUSR", BoSeriesMode.sf_Add);
                combo.Select(0, BoSearchKey.psk_Index);

                string codigoSerie = combo.Selected.Value;
                ds.SetValue("DocNum", 0, form.BusinessObject.GetNextSerialNumber(codigoSerie, "VS_OUSR").ToString());

                Folder folder = form.Items.Item("Item_12").Specific;
                folder.Select();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
