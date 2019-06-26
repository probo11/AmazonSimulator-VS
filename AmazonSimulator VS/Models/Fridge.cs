using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Controllers;

namespace Models
{
    public class Fridge : BaseClass
    {
        private Node locNode;
        public Fridge(Node locNode, double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("fridge", x, y, z, rotationX, rotationY, rotationZ)
        {
            this.locNode = locNode;
        }

        public void UpdateLocNode(Node locatie)
        {
            locNode = locatie;
        }

        public Node GetLoc()
        {
            return locNode;
        }

        public override bool Update(int tick)
        {
            return base.Update(tick);
        }
    }
}