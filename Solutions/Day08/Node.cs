using System.Collections.Immutable;
using System.Linq;

namespace Day08
{
    public class Node
    {
        public Node(
            IImmutableList<Node> children,
            IImmutableList<int> metadata)
        {
            Children = children;
            Metadata = metadata;
        }

        public IImmutableList<Node> Children { get; }

        public IImmutableList<int> Metadata { get; }

        public int CalculateValue() => Children.IsEmpty()
            ? Metadata.Sum()
            : Metadata
                .Where(i => i <= Children.Count)
                .Sum(i => Children[i - 1].CalculateValue());
    }
}
