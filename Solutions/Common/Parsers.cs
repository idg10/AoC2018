using System;
using LanguageExt.Parsec;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Common
{
    public static class Parsers
    {
        /// <summary>
        /// Provides various low-level parsers.
        /// </summary>
        public static readonly GenTokenParser lexer = Token.makeTokenParser(GenLanguageDef.Empty);

        /// <summary>
        /// Parse a decimal into a 32-bit integer.
        /// </summary>
        public static readonly Parser<int> pInt32 = lexer.Integer;

        /// <summary>
        /// Parse a comma-separated pair of integers.
        /// </summary>
        public static readonly Parser<(int x, int y)> pInt32CommaInt32 = Sep2By(pInt32, Trim(ch(',')));

        /// <summary>
        /// Parse a date/time of the form [1518-11-01 00:00].
        /// </summary>
        public static readonly Parser<DateTime> pDateTime =
            from year in pInt32
            from d1 in ch('-')
            from month in pInt32
            from d2 in ch('-')
            from day in pInt32
            from s1 in spaces
            from hours in pInt32
            from c1 in ch(':')
            from mins in pInt32
            select new DateTime(year, month, day, hours, mins, 0);

        /// <summary>
        /// Parse content optionally surrounded by other ignorable content (e.g. spaces).
        /// </summary>
        /// <typeparam name="T">The type returned by the parser whose results are required.</typeparam>
        /// <typeparam name="TTrim">The type returned by the parser whose results are to be trimmed.</typeparam>
        /// <param name="p">The parser of interest.</param>
        /// <param name="pTrim">Parser for the content to be trimmed.</param>
        /// <returns>The combined parser.</returns>
        public static Parser<T> Trim<T, TTrim>(Parser<T> p, Parser<TTrim> pTrim) =>
            from _ in pTrim
            from v in p
            from __ in pTrim
            select v;

        /// <summary>
        /// Parse content optionally surrounded by spaces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Parser<T> Trim<T>(Parser<T> p) => Trim(p, spaces);

        /// <summary>
        /// Parse an 'x'-separater pair of integers.
        /// </summary>
        public static readonly Parser<(int w, int h)> pInt32ByInt32 = Sep2By(pInt32, ch('x'));

        /// <summary>
        /// Creates a parser that parses two values of the same type, with a separator between them.
        /// </summary>
        /// <typeparam name="S">The separator parsed type.</typeparam>
        /// <typeparam name="T">The value parsers' type.</typeparam>
        /// <param name="p">The value parser.</param>
        /// <param name="sep">The separator parser.</param>
        /// <returns>The pair of parsed values.</returns>
        public static Parser<(T, T)> Sep2By<S, T>(Parser<T> p, Parser<S> sep) =>
            from x in p
            from _ in sep
            from y in p
            select (x, y);

        /// <summary>
        /// Produce a function that parses a single line of text.
        /// </summary>
        /// <typeparam name="T">The parsed output type.</typeparam>
        /// <param name="p">The parser.</param>
        /// <returns>
        /// A function that takes a string and produces the parsed result. If parsing fails,
        /// this prints out an error message and terminates the process.
        /// </returns>
        public static Func<string, T> LineProcessor<T>(Parser<T> p) => text => ProcessLine(p, text);

        /// <summary>
        /// Parse a single line of text.
        /// </summary>
        /// <typeparam name="T">The parsed output type.</typeparam>
        /// <param name="p">The parser.</param>
        /// <param name="line">The line of text to parse.</param>
        /// <param name="lineNumber">
        /// The line number to report if any errors are encountered.
        /// </param>
        /// <returns>
        /// Takes a string and produces the parsed result. If parsing fails,
        /// this prints out an error message and terminates the process.
        /// </returns>
        public static T ProcessLine<T>(Parser<T> p, string line, int lineNumber = 1)
        {
            ParserResult<T> result = parse(p, line);
            if (result.IsFaulted)
            {
                Console.WriteLine($"Error on line {lineNumber}: {result.Reply.Error}");
                Environment.Exit(1);
            }
            else if (result.Reply.State.Index != result.Reply.State.EndIndex)
            {
                if (!string.IsNullOrWhiteSpace(result.Reply.Error.Msg))
                {
                    Console.WriteLine($"{result.Reply.Error} {result.Reply.Error.Pos}");
                }
                string remaining = line.Substring(result.Reply.State.Index);
                Console.WriteLine($"Unexpected text at end of line {lineNumber}: {remaining}");
                Environment.Exit(1);
            }
            return result.Reply.Result;
        }
    }
}
