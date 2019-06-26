using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Controllers;
namespace Models
{
    public class Robot : BaseClass
    {
        Graaf g;
        List<Node> noot = new List<Node>();
        List<Node> nodess = new List<Node>();
        Fridge fridge, tempFridge;

        private bool robotDone = false, robotDoner = false, robotStart = false, OKx = false, OKz = false, rHeen = true, done = false;

        public Robot(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("robot", x, y, z, rotationX, rotationY, rotationZ)
        {

        }

        public void InputGraaf(Node startNode, Node eindNode, List<Node> list)
        {
            g = new Graaf(list);
            nodess = g.Dijkstra(startNode, eindNode);
            g = null;
        }

        public override bool Update(int tick)
        {
            if (robotStart)
            {
                
                if (nodess.Count > 1)
                {

                    double sane1X = nodess[0].getX();
                    double sane1Z = nodess[0].getZ();
                    double sane2X = nodess[1].getX();
                    double sane2Z = nodess[1].getZ();

                    if (this.x < sane2X && OKx == false)
                    {
                        this.Move(this.x + 0.1, 3.8, this.z);
                        if (this.fridge != null) { fridge.Move(this.x - 0.5, 0.5, this.z + 1); }
                    }
                    else if (this.x > sane2X && OKx == false)
                    {
                        this.Move(this.x - 0.1, 3.8, this.z);
                        if (this.fridge != null) { fridge.Move(this.x - 0.5, 0.5, this.z + 1); }
                    }
                    if (this.x >= sane2X - 0.1 && this.x <= sane2X + 0.1)
                    {
                        OKx = true;
                    }
                    if (this.z < sane2Z && OKz == false)
                    {
                        this.Move(this.x, 3.8, this.z + 0.1);
                        if (this.fridge != null) { fridge.Move(this.x - 0.5, 0.5, this.z + 1); }
                    }
                    else if (this.z > sane2Z && OKz == false)
                    {
                        this.Move(this.x, 3.8, this.z - 0.1);
                        if (this.fridge != null) { fridge.Move(this.x - 0.5, 0.5, this.z + 1); }
                    }
                    if (this.z >= sane2Z - 0.1 && this.z <= sane2Z + 0.1)
                    {
                        OKz = true;
                    }

                    if (OKx && OKz)
                    {
                        nodess.RemoveAt(0);
                        OKz = false;
                        OKx = false;
                    }
                }
                else
                {
                    if (robotDone)
                    {
                        robotDoner = true;
                    }
                    robotDone = true;
                }
            }
            return base.Update(tick);
        }

        public bool GetRobotDone()
        {
            return robotDone;
        }

        public void SetRobotDone(bool a)
        {
            robotDone = a;
        }

        public void SetRobotStart(bool a)
        {
            robotStart = a;
        }

        public bool GetRobotStart()
        {
            return robotStart;
        }

        public void SetFridge(Fridge f)
        {
            fridge = f;
        }

        public Fridge GetFridge()
        {
            return fridge;
        }

        public bool HasFridge()
        {
            if (fridge != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetDoner()
        {
            return robotDoner; // kabab
        }

        public void SetDoner(bool a)
        {
            robotDoner = a;
        }

        public bool GetRHeen()
        {
            return rHeen;
        }

        public void SetRHeen(bool a)
        {
            rHeen = a;
        }

        public bool GetDone()
        {
            return done;
        }

        public void SetDone(bool a)
        {
            done = a;
        }

        public Fridge GetTempFridge()
        {
            return tempFridge;
        }

        public void SetTempFridge(Fridge f)
        {
            tempFridge = f;
        }
    }

}