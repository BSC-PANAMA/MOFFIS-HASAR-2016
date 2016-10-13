using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace MOFFIS
{
    public class HASAR
    {
        [DllImport("winfis32.dll", CharSet = CharSet.Unicode)]
        public static extern int OpenComFiscal(int Com, int Mode);

        [DllImport("winfis32.dll", CharSet = CharSet.Unicode)]
        public static extern int CloseComFiscal(int Handler);

        [DllImport("winfis32.dll", CharSet = CharSet.Unicode)]
        public static extern int SetModoEpson();

        [DllImport("winfis32.dll", CharSet = CharSet.Unicode)]
        public static extern int InitFiscal(int Handler);

        [DllImport("winfis32.dll", CharSet = CharSet.Ansi)]
        public static extern int MandaPaqueteFiscal(int Handler, string Buff);

        [DllImport("winfis32.dll", CharSet = CharSet.Ansi)]
        public static extern int UltimaRespuesta(int Handler, [MarshalAs(UnmanagedType.VBByRefStr, SizeConst = 500)] ref string Buff);

        //[DllImport("BemaFi32.dll")]
        //public static extern int Bematech_FI_NumeroCupon([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCupon);

        [DllImport("winfis32.dll")]
        public static extern void Abort(int PortNumber);

        //        'void Abort (int PortNumber)
        //<DllImport("winfis32.dll", CharSet:=CharSet.Ansi)> _
        //        Public Shared Sub Abort(ByVal PortNumber As Integer)
        //End Sub

        //        'int UltimaRespuesta (int Handler, char *Buffer)
        //<DllImport("winfis32.dll", CharSet:=CharSet.Ansi)> _
        //        Public Shared Function UltimaRespuesta(ByVal Handler As Integer, ByVal Buffer As String) As Integer
        //End Function

        public static void LimpiarDoc()
        {
            string PathVariables = "";
            PathVariables = ConfigurationSettings.AppSettings["RutaVariables"];
            StreamWriter writer = new StreamWriter(PathVariables);
            writer.WriteLine("");
            writer.Close();
        }

        public static string LeerDoc()
        {
            string PathVariables = "";
            PathVariables = ConfigurationSettings.AppSettings["RutaVariables"];
            StreamReader reader = new StreamReader(PathVariables);
            string Status = reader.ReadToEnd();
            reader.Close();

            string[] stringSeparator = { "respuesta" };

            string[] Palabras;
            Palabras = Status.Split(stringSeparator, StringSplitOptions.RemoveEmptyEntries);
            return Palabras[1];
            //return Palabras[2];
        }

        public static void Analisa_iRetorno(int IRetorno)
        {
            string MSG = "";
            string MSGCaption = "Atención";
            MessageBoxIcon MSGIco = MessageBoxIcon.Information;

            switch (IRetorno)
            {
                case -1:
                    MSG = "Error: Error general!";
                    MSGCaption = "Error";
                    MSGIco = MessageBoxIcon.Error;
                    break;
                case -2:
                    MSG = "Error: Handler inválido!";
                    MSGCaption = "Error";
                    MSGIco = MessageBoxIcon.Error;
                    break;
                case -3:
                    MSG = "Error: Intento de enviar un comando cuando se estaba procesando!";
                    break;
                case -4:
                    MSG = "Error: Error de comunicaciones!";
                    MSGCaption = "Error de Comunicacion con la Impresora";
                    break;
                case -5:
                    MSG = "Error: Puerto ya abierto!";
                    MSGIco = MessageBoxIcon.Error;
                    break;
                case -6:
                    MSG = "Error: No hay memoria";
                    break;
                case -7:
                    MSG = "Error: El puerto ya estaba abierto";
                    break;
                case -8:
                    MSG = "Error: La dirección del buffer de respuesta es inválida";
                    MSGCaption = "Error";
                    MSGIco = MessageBoxIcon.Error;
                    break;
                case -9:
                    MSG = "Error: El comando no finalizó, sino que volvió una respuesta tipo STAT_PRN";
                    MSGCaption = "Error";
                    MSGIco = MessageBoxIcon.Error;
                    break;
                case -10:
                    MSG = "Error: El proceso en curso fue abortado por el usuario!";
                    break;
                case -11:
                    MSG = "Error: No hay más puertos disponibles!";
                    break;
                case -12:
                    MSG = "Error TCP/IP: Error estableciendo comunicación TCP/IP!";
                    break;
                case -13:
                    MSG = "Error TCP/IP: No se encontró el host!";
                    break;
                case -14:
                    MSG = "Error TCP/IP: Error de conexión con el host!";
                    break;
                case -15: MSG = "Error: Se recibió NAK al comando enviado!";
                    break;
            }
            if (MSG.Length != 0)
                System.Windows.Forms.MessageBox.Show(MSG, MSGCaption, MessageBoxButtons.OK, MSGIco);
        }



        public static string error_SF(string status, int mode)
        {
            string resp = "";
            string Errores = "";

            string s = Convert.ToString(Convert.ToInt32(status, 16), 2);
            int n1 = s.Length - 1;
            int[] ar = new int[16];

            switch (mode)
            {
                case 1:
                    foreach (char n in s)
                    {
                        ar[n1] = Convert.ToInt32(n.ToString());
                        if (Convert.ToInt32(n.ToString()) == 1)
                        {
                            switch (n1)
                            {
                                case 2:
                                    {
                                        Errores += "Se ha interrumpido la conexion entre el controlador fiscal y la impresora." + '\x0D';
                                        break;
                                    }
                                case 3:
                                    {
                                        Errores += "La impresora no ha logrado comunicarse dentro del periodo establecido." + '\x0D';
                                        break;
                                    }
                                case 14:
                                    {
                                        Errores += "Impresora sin papel para ser impreso." + '\x0D';
                                        break;
                                    }
                                case 15:
                                    {
                                        Errores += "OR logico de los bits 0-6,14." + '\x0D';
                                        break;
                                    }
                            }
                        }
                        n1 = n1 - 1;

                    }

                    break;
                case 2:
                    {
                        foreach (char n in s)
                        {
                            ar[n1] = Convert.ToInt32(n.ToString());
                            if (Convert.ToInt32(n.ToString()) == 1)
                            {
                                switch (n1)
                                {
                                    case 0:
                                        {
                                            Errores += "Error de comprobacion de memoria fiscal." + '\x0D';
                                            break;
                                        }
                                    case 1:
                                        {
                                            Errores += "Error de comprobacion de memoria de trabajo" + '\x0D';
                                            break;
                                        }
                                    case 3:
                                        {
                                            Errores += "Comando desconocido" + '\x0D';
                                            break;
                                        }
                                    case 4:
                                        {
                                            Errores += "Datos no validos en un campo" + '\x0D';
                                            break;
                                        }
                                    case 5:
                                        {
                                            Errores += "Comando no valido en el estado fiscal actual" + '\x0D';
                                            break;
                                        }
                                    case 6:
                                        {
                                            Errores += "Desborde del total" + '\x0D';
                                            break;
                                        }
                                    case 7:
                                        {
                                            Errores += "Memoria fiscal llena" + '\x0D';
                                            break;
                                        }
                                    case 8:
                                        {
                                            Errores += "Memoria fiscal a punto de llenarse" + '\x0D';
                                            break;
                                        }
                                    case 11:
                                        {
                                            Errores += "Es necesario hacer un cierre de jornada fiscal" + '\x0D';
                                            break;
                                        }
                                    case 15:
                                        {
                                            Errores += "OR logico de los bits 0 a 8 y 11" + '\x0D';
                                            break;
                                        }
                                }
                            }
                            n1 = n1 - 1;

                        }
                        break;
                    }
            }
            if (Errores.Trim() == "")
            {
                return "0";
            }

            return Errores;
        }



        //        Public Shared Function error_SF(ByVal status As String, ByVal mode As Integer) As String
        //    Dim resp = "", n As String
        //    Dim s As String = Convert.ToString(Convert.ToInt32(status, 16), 2)
        //    Dim n1 As Integer = s.Length - 1
        //    Dim ar(15) As Integer

        //    Select Case mode
        //        Case 1

        //            For Each n In s
        //                ar(n1) = n

        //                If n = 1 Then
        //                    Select Case n1

        //                        'Case 0
        //                        '    resp = resp
        //                        'Case 1
        //                        '    resp = resp
        //                        Case 2
        //                            resp = resp + "Se ha interrumpido la conexion entre el controlador fiscal y la impresora" + vbCrLf
        //                        Case 3
        //                            resp = resp + "La impresora no ha logrado comunicarse dentro del periodo establecido" + vbCrLf
        //                            'Case 4
        //                            '    resp = resp
        //                            'Case 5
        //                            '    resp = resp
        //                            'Case 6
        //                            '    resp = resp
        //                            'Case 7
        //                            '    resp = resp
        //                            'Case 8
        //                            '    resp = resp
        //                            'Case 9
        //                            '    resp = resp
        //                            'Case 10
        //                            '    resp = resp
        //                            'Case 11
        //                            '    resp = resp
        //                            'Case 12
        //                            '    resp = resp
        //                            'Case 13
        //                            '    resp = resp
        //                        Case 14
        //                            resp = resp + "Impresora sin papel para ser impreso" + vbCrLf
        //                        Case 15
        //                            resp = resp + "OR logico de los bits 0-6,14" + vbCrLf

        //                    End Select
        //                End If
        //                n1 -= 1
        //            Next
        //        Case 2

        //                For Each n In s
        //                ar(n1) = n


        //                If n = 1 Then
        //                    Select Case n1
        //                        Case 0
        //                            resp = resp + "Error de comprobacion de memoria fiscal" + vbCrLf
        //                        Case 1
        //                            resp = resp + "Error de comprobacion de memoria de trabajo" + vbCrLf
        //                            'Case 2
        //                            '    resp = resp
        //                        Case 3
        //                            resp = resp + "Comando desconocido" + vbCrLf
        //                        Case 4
        //                            resp = resp + "Datos no validos en un campo" + vbCrLf
        //                        Case 5
        //                            resp = resp + "Comando no valido en el estado fiscal actual" + vbCrLf
        //                        Case 6
        //                            resp = resp + "Desborde del total" + vbCrLf
        //                        Case 7
        //                            resp = resp + "Memoria fiscal llena" + vbCrLf
        //                        Case 8
        //                            resp = resp + "Memoria fiscal a punto de llenarse" + vbCrLf
        //                            'Case 9
        //                            '    resp = resp
        //                            'Case 10
        //                            '    resp = resp
        //                        Case 11
        //                            resp = resp + "Es necesario hacer un cierre de jornada fiscal" + vbCrLf
        //                            'Case 12
        //                            '    resp = resp + "Documento fiscal abierto" + vbCrLf
        //                            'Case 13
        //                            '    resp = resp + "Documento abierto" + vbCrLf
        //                            '    'Case 14
        //                            '    resp = resp
        //                        Case 15
        //                            resp = resp + "OR logico de los bits 0 a 8 y 11" + vbCrLf

        //                    End Select
        //                End If
        //                n1 -= 1
        //            Next

        //    End Select
        //    If resp = "" Then
        //        resp = "0"
        //    End If
        //    Return resp
        //End Function
    }
}
