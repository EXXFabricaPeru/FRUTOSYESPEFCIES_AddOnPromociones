using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOnPromociones.App
{
    class Connect
    {
        public static void SetApplication()
        {
            try
            {
                string args = (string)Environment.GetCommandLineArgs().GetValue(1);
                if (args.Length < 1)
                    Globals.oApp = new SAPbouiCOM.Framework.Application();
                else
                    Globals.oApp = new SAPbouiCOM.Framework.Application(args);

                Globals.SBO_Application = SAPbouiCOM.Framework.Application.SBO_Application;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ConnectToCompany()
        {
            try
            {
                SAPbobsCOM.Company oCompany;
                oCompany = (SAPbobsCOM.Company)Globals.SBO_Application.Company.GetDICompany();
                Globals.oCompany = oCompany;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AgregarFiltros()
        {
            try
            {
                Globals.oFilters = new SAPbouiCOM.EventFilters();
                Globals.oFilter = Globals.oFilters.Add(SAPbouiCOM.BoEventTypes.et_VALIDATE);
                Globals.oFilter.Add(139);

                Globals.oFilter = Globals.oFilters.Add(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
                foreach (var item in Globals.ListForm)
                    Globals.oFilter.Add(item.Code);

                Globals.SBO_Application.SetFilter(Globals.oFilters);
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
    }
}
