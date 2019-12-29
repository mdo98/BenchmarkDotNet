using BenchmarkDotNet.Diagnosers;

namespace BenchmarkDotNet.Disassemblers
{
    internal class LinuxDisassembler
    {
        private readonly DisassemblyDiagnoserConfig config;

        internal LinuxDisassembler(DisassemblyDiagnoserConfig config) => this.config = config;

        internal DisassemblyResult Disassemble(DiagnoserActionParameters parameters)
        {
            var settings = BuildDisassemblerSettings(parameters);

            var disassembledMethods = ClrMdDisassembler.AttachAndDisassemble(settings);

            return new DisassemblyResult { Methods = disassembledMethods };
        }

        private Settings BuildDisassemblerSettings(DiagnoserActionParameters parameters)
            => new Settings(
                processId: parameters.Process.Id,
                typeName: $"BenchmarkDotNet.Autogenerated.Runnable_{parameters.BenchmarkId.Value}",
                methodName: DisassemblerConstants.DisassemblerEntryMethodName,
                printAsm: config.PrintAsm,
                printSource: config.PrintSource,
                recursiveDepth: config.RecursiveDepth,
                resultsPath: default
            );
    }
}