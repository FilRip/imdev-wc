using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imdev_wc
{
    class Program
    {
        static bool _ignoreBlank;
        static string _ignoreStartWith="";
        static bool subfolder;
        static string _path;
        static bool _details,_detailsrow;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Parameter missing");
                Console.WriteLine("imdev-wc <options> extension");
                Console.WriteLine("/ignoreblank to ignore trimed blank lines");
                Console.WriteLine("/ignore:<set> to ignore line that start with <set>");
                Console.WriteLine("/path:<Path> to start at the specified path");
                Console.WriteLine("/subfolder to browse the subfolders too");
                Console.WriteLine("/details display details of each folder");
                Console.WriteLine("/details_row display details of each row");
                Console.WriteLine("extension must be the last parameter");
                return;
            }
            _path = System.Environment.CurrentDirectory;
            foreach (String arg in args)
                if (arg.Trim().StartsWith("/"))
                {
                    if (arg.Trim().ToLower().StartsWith("/ignoreblank"))
                        _ignoreBlank = true;
                    else if (arg.Trim().ToLower().StartsWith("/ignore:"))
                        _ignoreStartWith = arg.Trim().Substring(8);
                    else if (arg.Trim().ToLower().StartsWith("/subfolder"))
                        subfolder = true;
                    else if (arg.Trim().StartsWith("/path:"))
                        _path = arg.Trim().Substring(6);
                    else if (arg.Trim().StartsWith("/details_row"))
                        _detailsrow = true;
                    else if (arg.Trim().StartsWith("/details"))
                        _details = true;
                    else
                        Console.WriteLine("Unknown parameter : " + arg.Trim());
                }

            int total;
            total=boucle(_path,args[args.Length-1]);
            Console.WriteLine("\r\nNb de lignes : " + total.ToString());
        }

        private static int boucle(string rep,string masque)
        {
            int sousTotal = 0;
            string[] contenu;
            foreach (string fichier in System.IO.Directory.GetFiles(rep,masque))
            {
                contenu = System.IO.File.ReadAllLines(fichier);
                if (_ignoreBlank)
                    contenu = contenu.Where(s => s.Trim() != "").ToArray();
                if (_ignoreStartWith != "")
                    contenu = contenu.Where(s => !s.Trim().StartsWith(_ignoreStartWith)).ToArray();
                if (contenu.Length > 0)
                {
                    if (_detailsrow) Console.WriteLine(fichier + " = " + contenu.Length);
                    sousTotal += contenu.Length;
                }
            }

            if (subfolder)
                foreach (string sousrep in System.IO.Directory.GetDirectories(rep))
                {
                    sousTotal += boucle(sousrep, masque);
                }
            if ((_details) && (sousTotal>0)) Console.WriteLine("Sous total pour " + rep + " = " + sousTotal.ToString());
            return sousTotal;
        }
    }
}
