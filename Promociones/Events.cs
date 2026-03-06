using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOnPromociones.Promociones
{
    class Events
    {
        public static void ItemPressed(ref ItemEvent pVal, Form oForm, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.BeforeAction)
                {
                    switch (pVal.ItemUID)
                    {
                        case "1":
                            Main.PagarBonificacion(pVal, oForm, out BubbleEvent); break;
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
