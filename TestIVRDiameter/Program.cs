using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace TestIVRDiameter
{
    class Program
    {
        [DllImport(@"D:\Develops\TestIVRDiameter\TestIVRDiameter\bin\IVRDiameter.dll", EntryPoint = "initDmProxy"
            , SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static extern int initDmProxy(string as_conf, StringBuilder rs_errr_mssg);

        [DllImport(@"D:\Develops\TestIVRDiameter\TestIVRDiameter\bin\IVRDiameter.dll", EntryPoint = "connDmProxy"
            , SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static extern int connDmProxy(string as_conf, string as_func_conf, StringBuilder rs_errr_mssg);

        [DllImport(@"D:\Develops\TestIVRDiameter\TestIVRDiameter\bin\IVRDiameter.dll", EntryPoint = "discDmProxy"
            , SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static extern int discDmProxy(StringBuilder rs_errr_mssg);

        [DllImport(@"D:\Develops\TestIVRDiameter\TestIVRDiameter\bin\IVRDiameter.dll", EntryPoint = "postDmCmmd"
            , SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static extern int postDmCmmd(string funcName, int subrType, string postXML, StringBuilder responseXML, StringBuilder rs_errr_mssg);

        static void Main(string[] args)
        {
            Console.WriteLine("This is C# program to test call API IVR Diameter");
            //string rs_errr_mssg = string.Empty;

            //Connect
            StringBuilder rs_errr_mssg = new StringBuilder(4096);
            int ri = Program.connDmProxy("ConfigFile", "ConfigFunc", rs_errr_mssg);
            if ( ri == 0)
            {
                Console.WriteLine(string.Format("Return is {0}", rs_errr_mssg));
            }
            else
            {
                Console.WriteLine("Error:Can't call call API IVR Diameter");
            }

            Console.WriteLine("This is C# program to test call Post API IVR Diameter");

            rs_errr_mssg.Clear();

            //Post XML
            StringBuilder responseXML = new StringBuilder(4096);
            ri = Program.postDmCmmd("ConfigFunc", 1, "postXML", responseXML, rs_errr_mssg);
            if (ri == 0)
            {
                Console.WriteLine("XML Reponse...");
                Console.WriteLine("{0}", responseXML);
                Console.WriteLine(string.Format("Return is {0}", rs_errr_mssg));
            }
            else
            {
                Console.WriteLine("Error:Can't call call API IVR Diameter");
            }

            rs_errr_mssg.Clear();

            //Disconnect
            ri = Program.discDmProxy(rs_errr_mssg);
            if (ri == 0)
            {
                Console.WriteLine(string.Format("Return is {0}", rs_errr_mssg));
            }
            else
            {
                Console.WriteLine("Error:Can't call call API IVR Diameter");
            }
            Console.Read();
            string[] prefixes = new string[3] { "http://10.112.224.14/AbacusWeb/", "Joanne", "Robert" };
            SimpleListenerExample(prefixes);

        }

        public static void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required, 
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes. 
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response. 
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            listener.Stop();
        }
    }
}
