using AddOnPromociones.App;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AddOnPromociones.OrdenVenta
{
    public class Main
    {
        public static void CalcularMontos(ItemEvent pVal, Form oForm, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                double IGVGratuito = 0, TotalCliente = 0;
                string oType = ((SAPbouiCOM.ComboBox)oForm.Items.Item("3").Specific).Selected.Value;
                SAPbouiCOM.Matrix oDetalles = (SAPbouiCOM.Matrix)oForm.Items.Item(oType == "I" ? "38" : "39").Specific;

                string oMoneda = ((SAPbouiCOM.ComboBox)oForm.Items.Item("63").Specific).Selected.Value;
                string oPago = ((SAPbouiCOM.EditText)oForm.Items.Item("31").Specific).Value;

                for (int i = 1; i <= oDetalles.RowCount; i++)
                {
                    string oItem = string.Empty;
                    if (oType == "I")
                    {
                        oItem = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("1").Cells.Item(i).Specific).Value;

                        if (!string.IsNullOrEmpty(oItem))
                        {
                            SAPbouiCOM.CheckBox ocheck = (SAPbouiCOM.CheckBox)oDetalles.Columns.Item("2007").Cells.Item(i).Specific;
                            string Total = string.Empty, MontoIGTV = string.Empty;
                            if (oMoneda == "PEN" || oMoneda == "SOL")
                            {
                                Total = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("21").Cells.Item(i).Specific).Value.ToString();
                                MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("82").Cells.Item(i).Specific).Value.ToString();
                            }
                            else
                            {
                                Total = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("23").Cells.Item(i).Specific).Value.ToString();
                                MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("83").Cells.Item(i).Specific).Value.ToString();
                            }
                            Total = Total.Replace(oMoneda, "").Trim();
                            MontoIGTV = MontoIGTV.Replace(oMoneda, "").Trim();

                            Total = Total == string.Empty ? "0.00" : Total;
                            MontoIGTV = MontoIGTV == string.Empty ? "0.00" : MontoIGTV;

                            if (ocheck.Checked)
                                IGVGratuito += Convert.ToDouble(MontoIGTV);
                            else
                                TotalCliente += (Convert.ToDouble(Total) + Convert.ToDouble(MontoIGTV));
                        }
                    }
                    else
                    {
                        oItem = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("94").Cells.Item(i).Specific).Value;

                        if (!string.IsNullOrEmpty(oItem))
                        {
                            SAPbouiCOM.CheckBox ocheck = (SAPbouiCOM.CheckBox)oDetalles.Columns.Item("2007").Cells.Item(i).Specific;
                            string Total = string.Empty;
                            string MontoIGTV = string.Empty;
                            if (oMoneda == "PEN" || oMoneda == "SOL")
                            {
                                Total = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("12").Cells.Item(i).Specific).Value.ToString();
                                MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("32").Cells.Item(i).Specific).Value.ToString();
                            }
                            else
                            {
                                Total = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("14").Cells.Item(i).Specific).Value.ToString();
                                MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("33").Cells.Item(i).Specific).Value.ToString();
                            }
                            Total = Total.Replace(oMoneda, "").Trim();
                            MontoIGTV = MontoIGTV.Replace(oMoneda, "").Trim();

                            Total = Total == string.Empty ? "0.00" : Total;
                            MontoIGTV = MontoIGTV == string.Empty ? "0.00" : MontoIGTV;

                            if (ocheck.Checked)
                                IGVGratuito += Convert.ToDouble(MontoIGTV);
                            else
                                TotalCliente += (Convert.ToDouble(Total) + Convert.ToDouble(MontoIGTV));
                        }
                    }
                }

                SAPbouiCOM.Form oFormUserFields;
                try
                {
                    oFormUserFields = Globals.SBO_Application.Forms.Item(oForm.UDFFormUID);
                    if (!oFormUserFields.Visible)
                        throw new Exception();

                    ((SAPbouiCOM.EditText)oFormUserFields.Items.Item("U_EXX_TOTCLI").Specific).Value = TotalCliente.ToString("N2");
                    ((SAPbouiCOM.EditText)oFormUserFields.Items.Item("U_EXX_IGVGRA").Specific).Value = IGVGratuito.ToString("N2");
                }
                catch (Exception ex)
                {
                    throw new Exception("Por favor habilite los campos de usuario.");
                }

                //SAPbouiCOM.DBDataSource oDBDocument = oForm.DataSources.DBDataSources.Item("ORDR");
                //oDBDocument.SetValue("U_EXX_TOTCLI", 0, TotalCliente.ToString("N2"));
                //oDBDocument.SetValue("U_EXX_IGVGRA", 0, IGVGratuito.ToString("N2"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oForm.Freeze(false);
                Globals.Calculando = false;

            }
        }
    }
}
