﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#pragma warning disable CS3003 // I need ulong
namespace BenchmarkDotNet.Disassemblers
{
    public class Code
    {
        /// <summary>
        /// The native start offset
        /// </summary>
        public ulong StartAddress { get; set; }

        public string TextRepresentation { get; set; }
        public string Comment { get; set; }
    }

    public class Sharp : Code
    {
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
    }

    public class Asm : Code
    {
        /// <summary>
        /// The native end offset of this ASM representation
        /// </summary>
        public ulong EndAddress { get; set; }
        
        public uint SizeInBytes { get; set; }
    }

    public class Map
    {
        [XmlArray("Instructions")]
        [XmlArrayItem(nameof(Code), typeof(Code))]
        [XmlArrayItem(nameof(Sharp), typeof(Sharp))]
        [XmlArrayItem(nameof(Asm), typeof(Asm))]
        public List<Code> Instructions { get; set; }
    }

    public class DisassembledMethod
    {
        public string Name { get; set; }

        public ulong NativeCode { get; set; }

        public string Problem { get; set; }

        public Map[] Maps { get; set; }
        
        public string CommandLine { get; set; }

        public static DisassembledMethod Empty(string fullSignature, ulong nativeCode, string problem)
            => new DisassembledMethod
            {
                Name = fullSignature,
                NativeCode = nativeCode,
                Maps = Array.Empty<Map>(),
                Problem = problem
            };
    }

    public class DisassemblyResult
    {
        public DisassembledMethod[] Methods { get; set; }
        public string[] Errors { get; set; }

        public DisassemblyResult()
        {
            Methods = new DisassembledMethod[0];
            Errors = new string[0];
        }
    }

    public static class DisassemblerConstants
    {
        public const string NotManagedMethod = "not managed method";

        public const string DisassemblerEntryMethodName = "__ForDisassemblyDiagnoser__";
    }
}
#pragma warning restore CS3003 // Type is not CLS-compliant