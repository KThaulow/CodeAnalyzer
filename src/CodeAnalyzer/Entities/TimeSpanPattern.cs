using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer.Entities
{
    public class TimeSpanPattern
    {
        public TimeSpanPattern(Regex pattern, DiagnosticDescriptor rule)
        {
            Pattern = pattern;
            Rule = rule;
        }

        public Regex Pattern { get; private set; }

        public DiagnosticDescriptor Rule { get; private set; }

    }
}
