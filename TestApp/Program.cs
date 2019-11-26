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

        static void Main(string[] args)
        {


            


            bool bResult;
           

            bResult = DevSdkHelper.NET_SDK_Init();

            string loginIp = ConfigurationManager.AppSettings["ip"];
            UInt16 loginPort = Convert.ToUInt16(ConfigurationManager.AppSettings["port"]);
            string loginUserName = ConfigurationManager.AppSettings["username"];
            string loginPwd = ConfigurationManager.AppSettings["password"];


            userId = DevSdkHelper.NET_SDK_LoginEx(loginIp, loginPort, loginUserName, loginPwd, ref oNET_SDK_DEVICEINFO, NET_SDK_CONNECT_TYPE.NET_SDK_CONNECT_TCP, "");


            if (userId == -1)
            {
                Console.WriteLine("Login in failed! End ...");
                Console.ReadLine();
                return;
            }

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


            string cmd = "";
            int contador = 0;
            while (true)
            {

                Console.WriteLine("**************INGRESE OPCION DESEADA********************");
                Console.WriteLine("0: Testing LB LEYENDO PARAMETROS DDNS");
                Console.WriteLine("1: LIMPIAR PANTALLA");
                Console.WriteLine("2: ASIGNAR VALORES NO-IP");
                Console.WriteLine("3: ASIGNAR VALORES DYNDNS");
                Console.WriteLine("4: Reiniciar DVR");
                Console.WriteLine("Q: Exit");
             
                cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "0":
                        testLB();
                        break;
                    case "1":
                        Console.Clear();
                        break;
                    case "2":
                        cambiarXnoIp(contador);
                        break;
                    case "3":
                        cambiarXDyndns();
                        break;
                    case "4":
                        reiniciarDvr(userId);
                        break;
                    case "q":
                        Environment.Exit(0);
                        break;
                    case "Q":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Command not support");
                        break;
                }
            }

            Console.ReadLine();
        }

        static void reiniciarDvr(int userId)
        {
            DevSdkHelper.NET_SDK_EnterDVRConfig(userId);
            if (DevSdkHelper.NET_SDK_RebootDVR(userId))
            {
                Console.WriteLine("Reiniciando....");
            }
            else
            {
                Console.WriteLine("No se puede reiniciar");
            }
            DevSdkHelper.NET_SDK_ExitDVRConfig(userId);
        }

        static void cambiarXnoIp(int contador)
        {
            
            int lpBytesReturned = 0;
            DD_DDNS_CONFIG dns = new DD_DDNS_CONFIG();
            int tamanioDns = Marshal.SizeOf(dns);
            IntPtr intptrDtc = Marshal.AllocHGlobal(tamanioDns);
            bool ret = DevSdkHelper.NET_SDK_GetDVRConfig(userId, (uint)DD_CONFIG_ITEM_ID.DD_CONFIG_ITEM_NETWORK_DDNS, -1, intptrDtc, tamanioDns, ref lpBytesReturned, true);

            DD_DDNS_CONFIG tmp_dns = (DD_DDNS_CONFIG)Marshal.PtrToStructure(intptrDtc, typeof(DD_DDNS_CONFIG));
          
            Marshal.FreeHGlobal(intptrDtc);

            tmp_dns.enable = 1;
            tmp_dns.hostDomain=inicializarArray(tmp_dns.hostDomain);
            tmp_dns.hostDomain = Encoding.ASCII.GetBytes(llenarConCero("pruebatvt.ddns.net", tmp_dns.hostDomain));
            tmp_dns.userName = inicializarArray(tmp_dns.userName);
            tmp_dns.userName= Encoding.ASCII.GetBytes(llenarConCero("bainileonardo@gmail.com", tmp_dns.userName));
            tmp_dns.password = inicializarArray(tmp_dns.password);
            tmp_dns.password = Encoding.ASCII.GetBytes(llenarConCero("Patagon1an", tmp_dns.password));

            tmp_dns.useDDNSServer = 5;

             
            tamanioDns = Marshal.SizeOf(tmp_dns);
            intptrDtc = Marshal.AllocHGlobal(tamanioDns);            
            Marshal.StructureToPtr(tmp_dns, intptrDtc, true);

            DevSdkHelper.NET_SDK_EnterDVRConfig(userId);
            ret = DevSdkHelper.NET_SDK_SetDVRConfig(userId, (uint)DD_CONFIG_ITEM_ID.DD_CONFIG_ITEM_NETWORK_DDNS, -1, intptrDtc, tamanioDns);
            Marshal.FreeHGlobal(intptrDtc);
           


            if (ret)
            {
                Console.WriteLine("Guardado correcto");
               // DevSdkHelper.NET_SDK_RebootDVR(userId);
               contador = 0;
            }
            else
            {
                Console.WriteLine("No se pudo guardar...");
              /*  if (contador < 3)
                {
                    Console.WriteLine("Intentando de nuevo...");
                    contador++;
                    cambiarXnoIp(contador);
                   

                }*/
                Console.WriteLine("El error es : {0}", DevSdkHelper.NET_SDK_GetLastError());
            }

            DevSdkHelper.NET_SDK_ExitDVRConfig(userId);





        }

        static void cambiarXDyndns()
        {
            int lpBytesReturned = 0;
            DD_DDNS_CONFIG dns = new DD_DDNS_CONFIG();
            int tamanioDns = Marshal.SizeOf(dns);
            IntPtr intptrDtc = Marshal.AllocHGlobal(tamanioDns);
            bool ret = DevSdkHelper.NET_SDK_GetDVRConfig(userId, (uint)DD_CONFIG_ITEM_ID.DD_CONFIG_ITEM_NETWORK_DDNS, -1, intptrDtc, tamanioDns, ref lpBytesReturned, true);
            DD_DDNS_CONFIG tmp_dns = (DD_DDNS_CONFIG)Marshal.PtrToStructure(intptrDtc, typeof(DD_DDNS_CONFIG));
            Marshal.FreeHGlobal(intptrDtc);

            tmp_dns.enable = 1;
            tmp_dns.hostDomain = Encoding.ASCII.GetBytes(llenarConCero("pruebas.es.camaras.proseguralarmas.com", tmp_dns.hostDomain));
            tmp_dns.userName = Encoding.ASCII.GetBytes(llenarConCero("155gSC-prosegur", tmp_dns.userName));
            tmp_dns.password = Encoding.ASCII.GetBytes(llenarConCero("prosegur", tmp_dns.password));
            tmp_dns.useDDNSServer = 4;


            tamanioDns = Marshal.SizeOf(tmp_dns);
            intptrDtc = Marshal.AllocHGlobal(tamanioDns);
            Marshal.StructureToPtr(tmp_dns, intptrDtc, true);

            DevSdkHelper.NET_SDK_EnterDVRConfig(userId);
            ret = DevSdkHelper.NET_SDK_SetDVRConfig(userId, (uint)DD_CONFIG_ITEM_ID.DD_CONFIG_ITEM_NETWORK_DDNS, -1, intptrDtc, tamanioDns);
            Marshal.FreeHGlobal(intptrDtc);
            DevSdkHelper.NET_SDK_ExitDVRConfig(userId);

            if (ret)
            {
                Console.WriteLine("Guardado correcto");
            }
            else
            {
                Console.WriteLine("No se pudo guardar...");
                Console.WriteLine("El error es : {0}", DevSdkHelper.NET_SDK_GetLastError());
            }
           
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
        static void testLB()
        {
                                             
            DD_DDNS_CONFIG dns = new DD_DDNS_CONFIG();
            int tamanioDns = Marshal.SizeOf(dns);
            IntPtr intptrDtc = Marshal.AllocHGlobal(tamanioDns);
            int lpBytesReturned = 0;
            bool ret = DevSdkHelper.NET_SDK_GetDVRConfig(userId, (uint)DD_CONFIG_ITEM_ID.DD_CONFIG_ITEM_NETWORK_DDNS, -1, intptrDtc, tamanioDns, ref lpBytesReturned, true);
            DD_DDNS_CONFIG tmp_dns = (DD_DDNS_CONFIG)Marshal.PtrToStructure(intptrDtc, typeof(DD_DDNS_CONFIG));
            Marshal.FreeHGlobal(intptrDtc);

            string usuario = Encoding.ASCII.GetString(tmp_dns.userName);
            string password = Encoding.UTF8.GetString(tmp_dns.password);
            string dominio = Encoding.UTF8.GetString(tmp_dns.hostDomain);
          
            Console.WriteLine("dns.userName: {0}", usuario);
            Console.WriteLine("dns.userName: {0}", password);
            Console.WriteLine("dns.hostDomain: {0}", dominio);
            Console.WriteLine("dns.enable: {0}", tmp_dns.enable);
            Console.WriteLine("dns.iSize: {0}", tmp_dns.iSize);
            Console.WriteLine("dns.interval: {0}", tmp_dns.interval);
            Console.WriteLine("dns.useDDNSServer: {0}", tmp_dns.useDDNSServer);
            Console.WriteLine("dns.userHostDomain: {0}", tmp_dns.userHostDomain);

          

        }
       




    }
}
