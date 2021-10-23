using UnityEngine;

public class MakovChain : MonoBehaviour {
    public class Node 
    {
        public List<Edge> edges;
        public abstract void DefineEdges();
        public virtual List<Edge> AddEdge( List<Edge> currentEdges, Node _destinyNode, int _probability )
        {
            Edge newEdge = new Edge();
            newEdge.originNode  = this;
            newEdge.destinyNode = _destinyNode;
            newEdge.probabily   = _probability;
            
            currentEdges.Add( newEdge );
            return currentEdges;
        }
    }

    public class Edge
    {
        public Node originNode;
        public Node destinyNode;
        public int  probabily;
    }
    
}