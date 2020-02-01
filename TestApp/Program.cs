using DevSdkByCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Timers;
using System.Configuration;
using System.IO;

namespace TestApp
{
    class Program
    {
        static int userId = 0;
        static NET_SDK_DEVICEINFO oNET_SDK_DEVICEINFO = new NET_SDK_DEVICEINFO();

        static int Main(string[] args)
        {
           if (args.Length < 8)
            {
                Console.WriteLine("Bienvenido al configurador TVT, la ejecucion requiere 8 parametros, ingrese:\n");
                Console.WriteLine("TestApp.exe Param1 Param2 Param3 Param4 Param5 Param6 Param7 Param8\n");
                Console.WriteLine("Donde\n");
                Console.WriteLine("Param 1 = Usuario de login al DVR\n");
                Console.WriteLine("Param 2 = Contrasena de login al DVR\n");
                Console.WriteLine("Param 3 = URL o IP del equipo\n");
                Console.WriteLine("Param 4 = Puerto SDK\n");
                Console.WriteLine("Param 5 = Nueva URL\n");
                Console.WriteLine("Param 6 = Nuevo usuario\n");
                Console.WriteLine("Param 7 = Nueva contrasena\n");
                Console.WriteLine("Param 8 = DYNDNS=4 / NO-IP=5\n");
                
                return -1;
            }
         

            bool bResult;   

            bResult = DevSdkHelper.NET_SDK_Init();

            /*Cargar parámetros variables*/
            
             string loginUserName = args[0];
             string loginPwd = args[1];
             string loginIp = args[2];
             UInt16 loginPort = Convert.ToUInt16(args[3]);
             string nuevoUrlDns= args[4];
             string nuevoUsuarioDns = args[5];
             string nuevoPasswordDns = args[6];
             uint indexServerDns = Convert.ToUInt16(args[7]); 
             
            //Test cambio a NO-IP
            /*
            string loginUserName = "admin";
            string loginPwd = "12345678";
            string loginIp = "pruebas.es.camaras.proseguralarmas.com";
            UInt16 loginPort = Convert.ToUInt16(6039);            
            string nuevoUrlDns = "pruebatvt.ddns.net";
            string nuevoUsuarioDns = "bainileonardo@gmail.com";
            string nuevoPasswordDns = "Patagon1an";
            uint indexServerDns = Convert.ToUInt16(5);
            */
            //Test cambio a DynDns
            /*
            string loginUserName = "admin";
            string loginPwd = "12345678";
            string loginIp = "pruebatvt.ddns.net";
            UInt16 loginPort = Convert.ToUInt16(6039);
            string nuevoUrlDns = "pruebas.es.camaras.proseguralarmas.com";
            string nuevoUsuarioDns = "155gSC-prosegur";
            string nuevoPasswordDns = "prosegur";
            uint indexServerDns = Convert.ToUInt16(4);
            */

            userId = DevSdkHelper.NET_SDK_LoginEx(loginIp, loginPort, loginUserName, loginPwd, ref oNET_SDK_DEVICEINFO, NET_SDK_CONNECT_TYPE.NET_SDK_CONNECT_TCP, "");


            if (userId == -1)
            {
                Console.WriteLine("Login in failed! End ...");
                
                return Convert.ToUInt16(DevSdkHelper.NET_SDK_GetLastError());
            }

            /*Mostrar algo de info*/
            StringBuilder sbNET_SDK_DEVICEINFO_format = new StringBuilder("[ref] oNET_SDK_DEVICEINFO:\r\n");
            sbNET_SDK_DEVICEINFO_format.AppendLine("firmwareVersion: {0}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("kernelVersion: {1}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("hardwareVersion: {2}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("MCUVersion: {3}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("firmwareVersionEx: {4}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("deviceProduct: {5}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("deviceName: {6}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("deviceIP: {7}");
            sbNET_SDK_DEVICEINFO_format.AppendLine("devicePort: {9}");
            Console.WriteLine(string.Format(sbNET_SDK_DEVICEINFO_format.ToString()
                , oNET_SDK_DEVICEINFO.GetFirmwareVersion()
                , oNET_SDK_DEVICEINFO.GetKernelVersion()
                , oNET_SDK_DEVICEINFO.GetHardwareVersion()
                , oNET_SDK_DEVICEINFO.GetMCUVersion()
                , oNET_SDK_DEVICEINFO.GetFirmwareVersionEx()
                , oNET_SDK_DEVICEINFO.GetDeviceProduct()
                , oNET_SDK_DEVICEINFO.GetDeviceName()
                , oNET_SDK_DEVICEINFO.GetDeviceIP().ToString()
                , oNET_SDK_DEVICEINFO.GetDeviceMAC()
                , oNET_SDK_DEVICEINFO.devicePort));

            
         
           
            return cambiarDnsData(nuevoUrlDns, nuevoUsuarioDns, nuevoPasswordDns, indexServerDns);

            
        }


        static int cambiarDnsData(string nuevoUrlDns,string nuevoUsuarioDns,string nuevoPasswordDns,uint indexServerDns)
        {
            int errorCode = -1;
            int lpBytesReturned = 0;
            DD_DDNS_CONFIG dns = new DD_DDNS_CONFIG();
            int tamanioDns = Marshal.SizeOf(dns);
            IntPtr intptrDtc = Marshal.AllocHGlobal(tamanioDns);
            bool ret = DevSdkHelper.NET_SDK_GetDVRConfig(userId, (uint)DD_CONFIG_ITEM_ID.DD_CONFIG_ITEM_NETWORK_DDNS, -1, intptrDtc, tamanioDns, ref lpBytesReturned, true);
            int enterConfig=0;
            DD_DDNS_CONFIG tmp_dns = (DD_DDNS_CONFIG)Marshal.PtrToStructure(intptrDtc, typeof(DD_DDNS_CONFIG));
          
            Marshal.FreeHGlobal(intptrDtc);

            tmp_dns.enable = 1;
            tmp_dns.hostDomain=inicializarArray(tmp_dns.hostDomain);
            tmp_dns.hostDomain = Encoding.ASCII.GetBytes(llenarConCero(nuevoUrlDns, tmp_dns.hostDomain));
            tmp_dns.userName = inicializarArray(tmp_dns.userName);
            tmp_dns.userName= Encoding.ASCII.GetBytes(llenarConCero(nuevoUsuarioDns, tmp_dns.userName));
            tmp_dns.password = inicializarArray(tmp_dns.password);
            tmp_dns.password = Encoding.ASCII.GetBytes(llenarConCero(nuevoPasswordDns, tmp_dns.password));

            tmp_dns.useDDNSServer = indexServerDns;// Es para que use el dns correcto en el desplegable, es el index de dns servers 5 No-Ip 4 DynDns.

             
            tamanioDns = Marshal.SizeOf(tmp_dns);
            intptrDtc = Marshal.AllocHGlobal(tamanioDns);            
            Marshal.StructureToPtr(tmp_dns, intptrDtc, true);

            DevSdkHelper.NET_SDK_ExitDVRConfig(userId);// Se limpia por las dudas que haya salido mal

            enterConfig =DevSdkHelper.NET_SDK_EnterDVRConfig(userId);
            Console.WriteLine("Ingreso a la configuracion, error code: {0}", DevSdkHelper.NET_SDK_GetLastError());
            
            if (enterConfig != -1)
            {
                Console.WriteLine("Ingreso a la configuración ok");
            }
            else
            {
                Console.WriteLine("ERROR, NO Ingresa a la configuración");
                //  Console.WriteLine("Fallo al ingresar a la configuracion, El error es : {0}", DevSdkHelper.NET_SDK_GetLastError());
                errorCode = Convert.ToUInt16(DevSdkHelper.NET_SDK_GetLastError());
                return errorCode;
            }
            ret = DevSdkHelper.NET_SDK_SetDVRConfig(userId, (uint)DD_CONFIG_ITEM_ID.DD_CONFIG_ITEM_NETWORK_DDNS, -1, intptrDtc, tamanioDns);
            Marshal.FreeHGlobal(intptrDtc);
           


            if (ret)
            {
                Console.WriteLine("Guardado correcto");
                DevSdkHelper.NET_SDK_ExitDVRConfig(userId);
                if (DevSdkHelper.NET_SDK_RebootDVR(userId))
                {
                    Console.WriteLine("Reiniciando equipo");
                    errorCode = Convert.ToUInt16(DevSdkHelper.NET_SDK_GetLastError());
                    return errorCode;
                }
                else
                {
                    errorCode = Convert.ToUInt16(DevSdkHelper.NET_SDK_GetLastError());
                    
                    Console.WriteLine("Fallo al reiniciar El error es : {0}", errorCode);
                    return errorCode;
                }

            }
            else
            {
                errorCode = Convert.ToUInt16(DevSdkHelper.NET_SDK_GetLastError());
                Console.WriteLine("No se pudo guardar...");              
                Console.WriteLine("El error es : {0}", errorCode);
                
            }


           
            return errorCode;

        }

      

     
        private static String llenarConCero(String literal, Byte[]datoByte)
        {
            while (literal.Length < datoByte.Length)
                literal += "\0";
            return literal;
        }

        private static Byte[] inicializarArray(Byte[] datoByte)
        {
            int i = 0;
            while (i < datoByte.Length) { 
            datoByte[i] = 0;
            i++;
        }
            return datoByte;
        }
       




    }
}
