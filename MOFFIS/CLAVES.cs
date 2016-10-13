using System;
using System.Collections.Generic;
using System.Text;

namespace MOFFIS
{
    public class CLAVES
    {
        public string[] clave = new string[] { "L12BSCKJ59", "A31BSCKO39", "V24BSCKC32" };



        public bool varificarclave(string consultor)
        {
            //string respuesta = "";
            for (int i = 0; i < clave.Length; i++)
            {
                //respuesta += valores[i];

                if (clave[i] == consultor)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
