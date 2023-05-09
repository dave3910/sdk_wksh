using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXP_UIAPI.MenuEvents
{
    internal static class FormAPI_MenuEvents
    {
        const string PATH = "Recursos/Formularios/FormAPI.srf";
        const string TYPE = "FrmAPI";

        internal static void CrearFormulario()
        {
            try
            {
                var formUID = string.Concat(TYPE, new Random().Next(0, 1000));
                Globales.CrearFormulario(PATH, TYPE, formUID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
