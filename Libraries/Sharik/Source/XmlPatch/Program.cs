using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Security;
using Sharik.Xml;
using Sharik.Text;

namespace Sharik.Xml
{

    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                try
                {
                    XmlUtilities.PatchXmlFile(args[0], args[1], args[2]);
                    Console.WriteLine("XML patching succeeded.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    var inner = e.InnerException;
                    while (inner != null)
                    {
                        Console.WriteLine(inner.Message);
                        inner = inner.InnerException;
                    }
                }
            }
            Console.WriteLine("XmlPatch is a utility for patching XML files using XPath-based script.");
            Console.WriteLine("Usage: XmlPatch <input-xml-file-path> <patch-file-path> <output-xml-file-path>");
        }

    }

}
