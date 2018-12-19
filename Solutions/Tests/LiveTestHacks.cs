using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    internal static class LiveTestHacks
    {
        /// <summary>
        /// Gets a value indicating whether we are running inside Visual Studio Live Test.
        /// </summary>
        /// <remarks>
        /// Irritatingly, the instrumentation process used by live testing appears to strip
        /// out resources. Assembly manifest resources are all lost, and any Content files configured
        /// to copy their output to the target folder are ignored for live test purposes. This means
        /// tests that rely on that will fail in live testing. So we need to be able to detect it and
        /// skip things guaranteed to fail in that context.
        /// </remarks>
        public static bool RunningInVsLiveTest { get; } =
            AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.GetName().Name == "Microsoft.CodeAnalysis.LiveUnitTesting.Runtime");
    }
}
