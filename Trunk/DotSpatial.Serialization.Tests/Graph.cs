namespace DotSpatial.Serialization.Tests
{
    public class Graph
    {
        [Serialize("Root")]
        public Node Root { get; private set; }

        public Graph()
        {
        }

        public Graph(Node root)
        {
            Root = root;
        }
    }
}