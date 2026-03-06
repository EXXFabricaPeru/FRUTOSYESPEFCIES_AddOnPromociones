using AddOnPromociones.App;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AddOnPromociones.Promociones
{
    class Main
    {
        public static void PagarBonificacion(ItemEvent pVal, Form oForm, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (oForm.Mode == BoFormMode.fm_ADD_MODE)
                {
                    string CuentaIGV = ConfigurationManager.AppSettings["CuentaIGV"];
                    double IGVGratuito = 0, MontoPago = 0;
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
                                if (ocheck.Checked)
                                {
                                    string MontoIGTV = string.Empty;
                                    if (oMoneda == "PEN" || oMoneda == "SOL")
                                        MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("82").Cells.Item(i).Specific).Value.ToString();
                                    else
                                        MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("83").Cells.Item(i).Specific).Value.ToString();
                                    MontoIGTV = MontoIGTV.Replace(oMoneda, "").Trim();
                                    IGVGratuito += Convert.ToDouble(MontoIGTV);
                                }
                            }
                        }
                        else
                        {
                            oItem = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("94").Cells.Item(i).Specific).Value;

                            if (!string.IsNullOrEmpty(oItem))
                            {
                                SAPbouiCOM.CheckBox ocheck = (SAPbouiCOM.CheckBox)oDetalles.Columns.Item("2007").Cells.Item(i).Specific;
                                if (ocheck.Checked)
                                {
                                    string MontoIGTV = string.Empty;
                                    if (oMoneda == "PEN" || oMoneda == "SOL")
                                        MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("32").Cells.Item(i).Specific).Value.ToString();
                                    else
                                        MontoIGTV = ((SAPbouiCOM.EditText)oDetalles.Columns.Item("33").Cells.Item(i).Specific).Value.ToString();
                                    MontoIGTV = MontoIGTV.Replace(oMoneda, "").Trim();
                                    IGVGratuito += Convert.ToDouble(MontoIGTV);
                                }
                            }
                        }
                    }

                    IGVGratuito = Math.Round(IGVGratuito, 2, MidpointRounding.AwayFromZero);
                    if (IGVGratuito > 0)
                    {
                        if (string.IsNullOrEmpty(oPago))
                        {
                            BubbleEvent = false;
                            Globals.ActiveFormPago = true;
                            oForm.Items.Item("256000001").Click(BoCellClickType.ct_Regular);
                            Globals.SBO_Application.ActivateMenuItem("5892");
                        }
                        else
                        {
                            oPago = oPago.Replace(oMoneda, "");
                            MontoPago = Math.Round(Convert.ToDouble(oPago), 2, MidpointRounding.AwayFromZero);
                            if (IGVGratuito != MontoPago)
                            {
                                Globals.InformationMessage($"El monto pagado debe ser el total de IGV de las líneas gratuitas: {oMoneda} {IGVGratuito.ToString("N2")}");
                                BubbleEvent = false;
                                Globals.ActiveFormPago = true;
                                oForm.Items.Item("256000001").Click(BoCellClickType.ct_Regular);
                                Globals.SBO_Application.ActivateMenuItem("5892");
                            }
                        }
                    }

                    if (Globals.ActiveFormPago)
                    {
                        SAPbouiCOM.Form oFormMP = Globals.SBO_Application.Forms.ActiveForm;
                        if (oFormMP.TypeEx == "146")
                        {

                            try
                            {
                                ((SAPbouiCOM.EditText)oFormMP.Items.Item("32").Specific).Value = CuentaIGV;
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                ((SAPbouiCOM.ComboBox)oFormMP.Items.Item("8").Specific).Select(oMoneda, BoSearchKey.psk_ByValue);
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                ((SAPbouiCOM.EditText)oFormMP.Items.Item("38").Specific).Value = $"{oMoneda} {IGVGratuito.ToString("N2")}";
                            }
                            catch (Exception)
                            {
                            }

                            //try
                            //{
                            //    oFormMP.Items.Item("26").Click(BoCellClickType.ct_Regular);
                            //}
                            //catch (Exception)
                            //{
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BubbleEvent = false;
                throw ex;
            }
            finally
            {
                GC.Collect();
                oForm.Freeze(false);
                Globals.ActiveFormPago = false;
            }
        }
    }
}
