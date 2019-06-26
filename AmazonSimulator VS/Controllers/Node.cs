using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    public class Node
    {
        private string naam;
        private double coordX;
        private double coordY;
        private double coordZ;
        private List<Node> nextNodes = new List<Node>();
        private double afstandVanStartNode;
        private Node prevNode;
        private bool visited;

        public Node(double x, double y, double z, string naam, double afstand)
        {
            coordX = x;
            coordY = y;
            coordZ = z;
            this.naam = naam;
            afstandVanStartNode = afstand;
        }

        public void addNextNode(Node nxt)
        {
            nextNodes.Add(nxt);
        }

        public void updatePrevNode(Node prvNod)
        {
            prevNode = prvNod;
        }

        public void updateBool(bool visited)
        {
            this.visited = visited;
        }

        public void updateAfstand(double a)
        {
            afstandVanStartNode = a;
        }

        public bool getVisited()
        {
            return visited;
        }

        public Node getPrevNode()
        {
            return prevNode;
        }

        public double getAfstand()
        {
            return afstandVanStartNode;
        }

        public List<Node> getNextNodes()
        {
            return nextNodes;
        }

        public string getNaam()
        {
            return naam;
        }

        public double getX()
        {
            return coordX;
        }

        public double getY()
        {
            return coordY;
        }

        public double getZ()
        {
            return coordZ;
        }
    }
}