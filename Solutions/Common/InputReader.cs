using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LanguageExt.Parsec;

namespace Common
{
    public static class InputReader
    {
        public static IEnumerable<string> EnumerateLines(Type typeInContainingAssembly, string inputName = "Idg10Input.txt")
        {
            Assembly asm = typeInContainingAssembly.Assembly;
            Console.WriteLine(asm);
            Console.WriteLine(asm.CodeBase);
            Console.WriteLine(asm.Location);

            foreach (var n in asm.GetManifestResourceNames())
            {
                Console.WriteLine(n);
            }
            using (Stream s = asm.GetManifestResourceStream(inputName) ?? asm.GetManifestResourceStream(typeInContainingAssembly.Namespace + "." + inputName))
            using (StreamReader r = new StreamReader(s))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        yield return line;
                    }
                }
            }
        }

        public static IEnumerable<T> ParseLines<T>(
            Type typeInContainingAssembly,
            Parser<T> p,
            string inputName = "Idg10Input.txt")
        {
            return ParseLines(EnumerateLines(typeInContainingAssembly, inputName), p);
        }

        public static IEnumerable<T> ParseLines<T>(
            IEnumerable<string> lines,
            Parser<T> p)
        {
            int lineNumber = 1;
            foreach (string line in lines)
            {
                yield return Parsers.ProcessLine(p, line, lineNumber);

                lineNumber++;
            }
        }
    }
}
