using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer.Test.Helpers
{
    public static class MetaDataReferenceHelper
    {
        private static readonly ImmutableDictionary<string, string> s_AssemplyMap = GetTrustedPlatformAssemblies();

        public static PortableExecutableReference GetAssemblyReference(string assemblyName)
        {
            var assemblyLocation = GetAssemblyLocation(assemblyName);
            return MetadataReference.CreateFromFile(assemblyLocation);
        }

        public static string GetAssemblyLocation(string assemblyName)
        {
            return s_AssemplyMap[assemblyName];
        }

        private static ImmutableDictionary<string, string> GetTrustedPlatformAssemblies()
        {
            // https://docs.microsoft.com/en-us/dotnet/core/tutorials/netcore-hosting
            // TRUSTED_PLATFORM_ASSEMBLIES: This is a list of assembly paths (delimited by ';' on Windows and ':' on Unix) which the AppDomain should prioritize loading and give full trust to 
            return AppContext
                .GetData("TRUSTED_PLATFORM_ASSEMBLIES")
                .ToString()
                .Split(';')
                .ToImmutableDictionary(Path.GetFileName);
        }
    }
}
