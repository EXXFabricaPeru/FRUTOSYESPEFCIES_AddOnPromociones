using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddOnPromociones.Entities;
using SAPbouiCOM.Framework;

namespace AddOnPromociones.App
{
    public class Globals
    {
        public static string ShortName = "(EXX)";
        public static string AddOnName = "AddOn de Promociones";
        public static string AddOnVersion = "1.0.0.6";
        public static SAPbouiCOM.Application SBO_Application = Application.SBO_Application;
        public static SAPbouiCOM.EventFilters oFilters;
        public static SAPbouiCOM.EventFilter oFilter;
        public static SAPbobsCOM.Company oCompany;
        public static SAPbobsCOM.Recordset oRec = default(SAPbobsCOM.Recordset);

        public static Application oApp;
        public static string Query;
        public static long lRetCode;
        public static string Path;
        public static List<Form> ListForm = new List<Form>();
        public static bool ActiveFormPago = false;
        public static bool Calculando = false;
        public static List<string> ListaColumnas = new List<string> { "11", "14", "15", "16", "21", "22", "23", "24", "25", "26", "27", "60", "61", "160", "212", "2007" };
        public static SAPbobsCOM.Recordset RunQuery(string query)
        {
            try
            {
                oRec = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRec.DoQuery(query);

                return oRec;
            }
            catch (Exception ex)
            {
                MessageBox(ex.Message);
                return null;
            }
        }

        public static object Release(object objeto)
        {
            if (objeto != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objeto);
                Query = null;
                GC.Collect();
            }
            return null;
        }

        public static void ErrorMessage(String msg)
        {
            SBO_Application.SetStatusBarMessage(ShortName + ": " + msg, SAPbouiCOM.BoMessageTime.bmt_Short);
        }

        public static void InformationMessage(String msg)
        {
            SBO_Application.SetStatusBarMessage(ShortName + ": " + msg, SAPbouiCOM.BoMessageTime.bmt_Short, false);
        }

        public static void SuccessMessage(String msg)
        {
            SBO_Application.SetStatusBarMessage(ShortName + ": " + msg, SAPbouiCOM.BoMessageTime.bmt_Short, false);
        }
        public static void SuccessMessage2(String msg)
        {
            SBO_Application.StatusBar.SetText(ShortName + ": " + msg, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        public static void MessageBox(String msg)
        {
            SBO_Application.MessageBox(msg);
        }

        public static void ListarFormularios()
        {
            try
            {
                Globals.Query = AddOnPromociones.Properties.Resources.ListarFormularios;
                Globals.RunQuery(Globals.Query);
                Globals.oRec.MoveFirst();

                while (!(Globals.oRec.EoF))
                {
                    Entities.Form oForm = new Entities.Form();
                    oForm.Code = Convert.ToInt32(Globals.oRec.Fields.Item("Code").Value.ToString());
                    oForm.Name = Globals.oRec.Fields.Item("Name").Value.ToString();
                    ListForm.Add(oForm);
                    Globals.oRec.MoveNext();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Globals.Release(Globals.oRec);
            }
        }
    }
}
