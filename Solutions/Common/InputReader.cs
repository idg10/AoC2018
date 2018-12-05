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
            int lineNumber = 1;
            foreach (string line in EnumerateLines(typeInContainingAssembly, inputName))
            {
                yield return Parsers.ProcessLine(p, line, lineNumber);

                lineNumber++;
            }
        }
    }
}
