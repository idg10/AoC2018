using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
    }
}
