using System.Collections.Immutable;

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
    }
}
