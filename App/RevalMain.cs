using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOnPromociones.App
{
    class RevalMain
    {
        public RevalMain()
        {
            try
            {
                Connect.SetApplication();
                Connect.ConnectToCompany();

                Globals.SBO_Application.AppEvent += new _IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                Metadata.CreateMetadata();
                Globals.ListarFormularios();
                Connect.AgregarFiltros();

                if (Globals.ListForm.Count > 0)
                {
                    Globals.SBO_Application.ItemEvent += new _IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
                    Globals.SBO_Application.StatusBar.SetText(Globals.AddOnName + " v." + Globals.AddOnVersion + " Conectada con éxito.", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Success);//Cambiar mensaje de conexion
                    Globals.oApp.Run();
                }
                else
                    Globals.SBO_Application.StatusBar.SetText(Globals.AddOnName + " v." + Globals.AddOnVersion + " No se encontró formularios habilitados para el addon de promociones.", SAPbouiCOM.BoMessageTime.bmt_Short, (SAPbouiCOM.BoStatusBarMessageType)SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            }
            catch (Exception ex)
            {
                Globals.ErrorMessage(ex.Message);
            }
        }

        private void SBO_Application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.FormTypeEx != "0")
            {
                try
                {
                    SAPbouiCOM.Form oForm = Globals.SBO_Application.Forms.Item(pVal.FormUID);
                    if (Globals.ListForm.Any(x => x.Code.ToString() == oForm.TypeEx))
                    {
                        if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                            Promociones.Events.ItemPressed(ref pVal, oForm, out BubbleEvent);
                    }

                    if (oForm.TypeEx == "139")
                    {
                        if (pVal.EventType == BoEventTypes.et_VALIDATE)
                            OrdenVenta.Events.ItemPressed(ref pVal, oForm, out BubbleEvent);
                    }
                }
                catch (Exception ex)
                {
                    BubbleEvent = false;
                    if (ex.Message != "Form - Invalid Form" && ex.Message != "Invalid Choose From List  [66000-104]")
                        Globals.ErrorMessage(ex.Message);
                }
            }
        }

        private void SBO_Application_AppEvent(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                    Globals.SBO_Application.MessageBox(Globals.AddOnName + " finalizará");
                    Environment.Exit(Environment.ExitCode);
                    System.Windows.Forms.Application.Exit();
                    break;
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    Environment.Exit(Environment.ExitCode);
                    System.Windows.Forms.Application.Exit();
                    break;
                default:
                    break;
            }
        }
    }
}
