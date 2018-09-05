using System.Collections.Immutable;

namespace CodeAnalyzer.Entities.Helpers
{
    public static class Namespaces
    {
        public static readonly ImmutableArray<string> System_Linq = ImmutableArray.Create("System", "Linq");

        public static readonly ImmutableArray<string> System = ImmutableArray.Create("System");
    }
}
