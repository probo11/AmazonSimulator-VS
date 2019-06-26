using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    public class Graaf
    {
        private List<Node> nodeList;

        public Graaf(List<Node> nodes)
        {
            nodeList = nodes;
        }

        public List<Node> Dijkstra(Node startNode, Node eindNode)
        {
            List<Node> unvisitedList = nodeList;

            foreach (Node node in nodeList)
            {
                node.updateAfstand(1000000);
                node.updateBool(false);
            }
            startNode.updateAfstand(0);

            while (unvisitedList.Count() > 0)
            {
                Node nextNode = startNode;
                double min = 10000000;
                foreach (Node n in nodeList)
                {
                    if (n.getAfstand() < min && !n.getVisited())
                    {
                        min = n.getAfstand();
                        nextNode = n;
                    }
                }

                List<Node> lijst = nextNode.getNextNodes();

                foreach (Node ding in lijst)
                {
                    double a = Math.Abs(ding.getX() - nextNode.getX()) + Math.Abs(ding.getZ() - nextNode.getZ()) + nextNode.getAfstand();
                    if (a < ding.getAfstand())
                    {
                        ding.updateAfstand(a);
                        ding.updatePrevNode(nextNode);
                    }
                }

                nextNode.updateBool(true);
                unvisitedList.Remove(nextNode);
            }

            List<Node> routeList = new List<Node>();
            Node no = eindNode;

            routeList.Add(eindNode);
            while (no.getPrevNode() != startNode)
            {
                routeList.Add(no.getPrevNode());
                no = no.getPrevNode();
            }
            routeList.Add(startNode);
            routeList.Reverse();

            return routeList;

        }
    }
}