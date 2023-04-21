using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EXP_UIAPI
{
    internal static class Globales
    {
        public static Application oAplication { get; set; }
        public static SAPbobsCOM.Company oCompány { get; set; }

        internal static void CrearFormulario(string path, string tipo, string id)
        {
            XmlDocument xmlDocument = new XmlDocument();

            FormCreationParams CreationPackage = oAplication.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
            xmlDocument.Load(path);
            CreationPackage.XmlData = xmlDocument.InnerXml;
            CreationPackage.FormType = tipo;
            CreationPackage.UniqueID = id;
            oAplication.Forms.AddEx(CreationPackage);
        }
    }
}
