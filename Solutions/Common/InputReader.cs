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
            using (var r = new StreamReader(s))
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

        public static string ReadAll(Type typeInContainingAssembly, string inputName = "Idg10Input.txt")
        {
            Assembly asm = typeInContainingAssembly.Assembly;

            using (Stream s = asm.GetManifestResourceStream(inputName) ?? asm.GetManifestResourceStream(typeInContainingAssembly.Namespace + "." + inputName))
            using (var r = new StreamReader(s))
            {
                return r.ReadToEnd();
            }
        }

        public static T ParseAll<T>(
            Type typeInContainingAssembly,
            Parser<T> p,
            string inputName = "Idg10Input.txt")
        {
            return Parsers.ProcessLine(p, ReadAll(typeInContainingAssembly, inputName));
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
