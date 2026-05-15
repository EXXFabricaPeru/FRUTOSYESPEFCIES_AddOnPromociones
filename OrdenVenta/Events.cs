using AddOnPromociones.App;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOnPromociones.OrdenVenta
{
    public class Events
    {
        public static void ItemPressed(ref ItemEvent pVal, Form oForm, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.ActionSuccess && !Globals.Calculando)
                {
                    switch (pVal.ItemUID)
                    {
                        case "38":
                            string Columna = pVal.ColUID;
                            if(Globals.ListaColumnas.Any(x => x == Columna))
                            {
                                Globals.Calculando = true;
                                Main.CalcularMontos(pVal, oForm, out BubbleEvent);
                            }
                            break;
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
            }
        }
    }
}
