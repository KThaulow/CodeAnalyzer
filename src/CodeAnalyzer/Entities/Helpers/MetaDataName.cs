using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer.Entities.Helpers
{
    public class MetaDataName : IEquatable<MetaDataName>
    {
        public MetaDataName(ImmutableArray<string> containingNamespaces, string name)
        {
            Name = name;
            ContainingNamespaces = containingNamespaces;
        }

        public ImmutableArray<string> ContainingNamespaces { get; }

        public string Name { get; }


        public override bool Equals(object obj)
        {
            var name = obj as MetaDataName;
            return name != null &&
                   Equals(name);
        }

        public bool Equals(MetaDataName name)
        {
            return ContainingNamespaces.Equals(name.ContainingNamespaces) &&
                   Name == name.Name;
        }

        public bool Equals(ISymbol symbol)
        {
            if (symbol == null)
                return false;

            if (!string.Equals(Name, symbol.MetadataName, StringComparison.Ordinal))
                return false;

            INamespaceSymbol symbolContainingNamespace = symbol.ContainingNamespace;

            foreach (var containingNameSpace in ContainingNamespaces)
            {
                if (symbolContainingNamespace?.IsGlobalNamespace != false)
                    return false;

                if (!string.Equals(symbolContainingNamespace.Name, containingNameSpace, StringComparison.Ordinal))
                    return false;

                symbolContainingNamespace = symbolContainingNamespace.ContainingNamespace;
            }

            return symbolContainingNamespace?.IsGlobalNamespace == true;
        }



        public override int GetHashCode()
        {
            var hashCode = 1511997551;
            hashCode = hashCode * -1521134295 + EqualityComparer<ImmutableArray<string>>.Default.GetHashCode(ContainingNamespaces);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
