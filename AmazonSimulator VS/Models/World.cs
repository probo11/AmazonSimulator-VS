using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : IObservable<Command>, IUpdatable
    {
        private List<BaseClass> worldObjects = new List<BaseClass>();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private List<Fridge> opgeslagenFridgesList = new List<Fridge>();
        private List<Fridge> magazijnFridgesList = new List<Fridge>();
        private List<Node> nodeList = new List<Node>();
        private List<Robot> robots = new List<Robot>();
        private List<Fridge> tussenlijst = new List<Fridge>();

        Spaceship s;
        Fridge f1, f2, f3, fr;
        Robot r1, r2, r3;

        Node nodePointOne = new Node(10, 0, 10, "node1", 1000000);
        Node nodePointTwo = new Node(14, 0, 10, "node2", 1000000);
        Node nodePointThree = new Node(18, 0, 10, "node3", 1000000);
        Node nodePointFour = new Node(10, 0, 15, "node4", 1000000);
        Node nodePointFive = new Node(14, 0, 15, "node5", 1000000);
        Node nodePointSix = new Node(18, 0, 15, "node6", 1000000);
        Node nodePointSeven = new Node(10, 0, 20, "node7", 1000000);
        Node nodePointEight = new Node(14, 0, 20, "node8", 1000000);
        Node nodePointNine = new Node(18, 0, 20, "node9", 1000000);
        Node nodePointA = new Node(15, 0, 3, "nodeA", 1000000);
        Node nodePointB = new Node(22.5, 0, 3, "nodeB", 1000000);
        Node nodePointC = new Node(22.5, 0, 12, "nodeC", 1000000);
        Node nodePointD = new Node(22.5, 0, 16.5, "nodeD", 1000000);
        Node nodePointE = new Node(22.5, 0, 21.5, "nodeE", 1000000);
        Node nodePointF = new Node(6.7, 0, 21.5, "nodeF", 1000000);
        Node nodePointG = new Node(6.7, 0, 16.5, "nodeG", 1000000);
        Node nodePointH = new Node(6.7, 0, 12, "nodeH", 1000000);
        Node nodePointI = new Node(6.7, 0, 3, "nodeI", 1000000);
        Node nodePointJ = new Node(15, 0, 3, "nodeJ", 1000000);

        public World()
        {
            nodePointA.addNextNode(nodePointB);
            nodePointB.addNextNode(nodePointC);
            nodePointC.addNextNode(nodePointB);
            nodePointC.addNextNode(nodePointD); nodePointC.addNextNode(nodePointThree);
            nodePointD.addNextNode(nodePointC);
            nodePointD.addNextNode(nodePointE); nodePointD.addNextNode(nodePointSix);
            nodePointE.addNextNode(nodePointD); nodePointE.addNextNode(nodePointNine);
            nodePointF.addNextNode(nodePointG); nodePointF.addNextNode(nodePointSeven);
            nodePointG.addNextNode(nodePointF); nodePointG.addNextNode(nodePointH); nodePointG.addNextNode(nodePointFour);
            nodePointH.addNextNode(nodePointG); nodePointH.addNextNode(nodePointI); nodePointH.addNextNode(nodePointOne);
            nodePointI.addNextNode(nodePointH); nodePointI.addNextNode(nodePointJ);
            nodePointOne.addNextNode(nodePointH); nodePointOne.addNextNode(nodePointTwo);
            nodePointTwo.addNextNode(nodePointOne); nodePointTwo.addNextNode(nodePointThree);
            nodePointThree.addNextNode(nodePointC); nodePointThree.addNextNode(nodePointTwo);
            nodePointFour.addNextNode(nodePointG); nodePointFour.addNextNode(nodePointFive);
            nodePointFive.addNextNode(nodePointFour); nodePointFive.addNextNode(nodePointSix);
            nodePointSix.addNextNode(nodePointD); nodePointSix.addNextNode(nodePointFive);
            nodePointSeven.addNextNode(nodePointF); nodePointSeven.addNextNode(nodePointEight);
            nodePointEight.addNextNode(nodePointSeven); nodePointEight.addNextNode(nodePointNine);
            nodePointNine.addNextNode(nodePointE); nodePointNine.addNextNode(nodePointEight);

            r1 = CreateRobot(15, 0, 3);
            r2 = CreateRobot(15, 0, 3);
            r3 = CreateRobot(15, 0, 3);
            robots.Add(r1);
            robots.Add(r2);
            robots.Add(r3);

            s = CreateSpaceship(0, 0, 0);
            s.Move(-550, 0, -30);

            f1 = CreateFridge(nodePointSeven, 15, 0, 3);
            f1.Rotate(0, 90, 0);
            opgeslagenFridgesList.Add(f1);

            f2 = CreateFridge(nodePointThree, 15, 0, 3);
            f2.Rotate(0, 90, 0);
            opgeslagenFridgesList.Add(f2);

            f3 = CreateFridge(nodePointFive, 15, 0, 3);
            f3.Rotate(0, 90, 0);
            opgeslagenFridgesList.Add(f3);
        }

        //beep boop robot
        private Robot CreateRobot(double x, double y, double z)
        {
            Robot r = new Robot(x, y, z, 0, 0, 0);
            worldObjects.Add(r);
            return r;
        }

        //brrr
        private Fridge CreateFridge(Node n, double x, double y, double z)
        {
            Fridge f = new Fridge(n, x, y, z, 0, 0, 0);
            worldObjects.Add(f);
            return f;
        }

        //ufo
        private Spaceship CreateSpaceship(double x, double y, double z)
        {
            Spaceship f = new Spaceship(x, y, x, 0, 0, 0);
            worldObjects.Add(f);
            return f;
        }

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

                SendCreationCommandsToObserver(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        private void SendCommandToObservers(Command c)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(c);
            }
        }

        private void SendCreationCommandsToObserver(IObserver<Command> obs)
        {
            foreach (BaseClass m3d in worldObjects)
            {
                obs.OnNext(new UpdateModel3DCommand(m3d));
            }
        }

        private Fridge GimmeDatFridgeLoc(List<Fridge> list)
        {
            Fridge tempNode = list[0];

            list.RemoveAt(0); //verwijdert de fridge ook meteen uit de lijst
            return tempNode;
        }

        private void FillList()
        {
            nodeList.Add(nodePointOne);
            nodeList.Add(nodePointTwo);
            nodeList.Add(nodePointThree);
            nodeList.Add(nodePointFour);
            nodeList.Add(nodePointFive);
            nodeList.Add(nodePointSix);
            nodeList.Add(nodePointSeven);
            nodeList.Add(nodePointEight);
            nodeList.Add(nodePointNine);
            nodeList.Add(nodePointA);
            nodeList.Add(nodePointB);
            nodeList.Add(nodePointC);
            nodeList.Add(nodePointD);
            nodeList.Add(nodePointE);
            nodeList.Add(nodePointF);
            nodeList.Add(nodePointG);
            nodeList.Add(nodePointH);
            nodeList.Add(nodePointI);
            nodeList.Add(nodePointJ);
        }

        public bool Update(int tick)
        {
            for (int i = 0; i < worldObjects.Count; i++)
            {
                BaseClass u = worldObjects[i];

                if (u is IUpdatable)
                {
                    bool needsCommand = ((IUpdatable)u).Update(tick);

                    if (needsCommand)
                    {
                        SendCommandToObservers(new UpdateModel3DCommand(u));
                    }
                }
            }

            if (s.GetUnload()) // als de ufo aan is
            {
                for (int i = 0; i < robots.Count; i++) // voor elke robot
                {
                    if (robots[i].GetRobotDone()) // als ie klaar is met heen gaan
                    {
                        if (robots[i].GetDoner()) // als ie echt klaar is
                        {
                            robots[i].SetRobotStart(false);
                            robots[i].SetRobotDone(false);
                            robots[i].SetDoner(false);
                            robots[i].SetDone(true);
                        }
                        else
                        {
                            if (robots[i].GetRHeen())
                            {
                                robots[i].SetRHeen(false);
                                if (robots[i].HasFridge()) // als ie een fridge heeft
                                {
                                    // hier dropt hij de fridge
                                    if (!magazijnFridgesList.Contains(robots[i].GetFridge())) // kijkt op de lijst de fridge niet heeft
                                    {
                                        magazijnFridgesList.Add(robots[i].GetFridge());
                                        FillList();
                                        robots[i].InputGraaf(robots[i].GetFridge().GetLoc(), nodePointJ, nodeList);
                                        robots[i].SetFridge(null);
                                    }
                                }
                                else
                                {
                                    // hier pak hij een fridge op
                                    //if (magazijnFridgesList.Contains(fr)) // kijkt of de lijst de fridge heeft
                                    //{
                                        
                                        robots[i].SetFridge(robots[i].GetTempFridge());
                                        opgeslagenFridgesList.Add(robots[i].GetTempFridge());
                                        magazijnFridgesList.Remove(robots[i].GetTempFridge());
                                        FillList();
                                        robots[i].InputGraaf(robots[i].GetFridge().GetLoc(), nodePointJ, nodeList);
                                    //}
                                }
                            }

                            //if (magazijnFridgesList.Count >= 1) //fridges die in het magazijn staan
                            //{
                                robots[i].SetRobotStart(true);
                            //}
                        }
                    }
                    else //heen
                    {
                        if (s.GetSuper() % 2 != 0) // als er geen fridges achter staan dan do je dit
                        {
                            if (magazijnFridgesList.Count >= 1) 
                            {
                                fr = GimmeDatFridgeLoc(magazijnFridgesList);
                                robots[i].SetTempFridge(fr);
                                FillList();
                                robots[i].InputGraaf(nodePointA, fr.GetLoc(), nodeList);
                                robots[i].SetRobotStart(true);
                            }
                        }
                        else if (s.GetSuper() % 2 == 0)// fridges die achter staan
                        {
                            if (opgeslagenFridgesList.Count >= 1) 
                            {
                                fr = GimmeDatFridgeLoc(opgeslagenFridgesList);
                                robots[i].SetFridge(fr);
                                FillList();
                                robots[i].InputGraaf(nodePointA, robots[i].GetFridge().GetLoc(), nodeList);
                                robots[i].SetRobotStart(true);

                            }
                        }
                    }

                }

            }
            if (robots[0].GetDone() && robots[1].GetDone() && robots[2].GetDone()) // als alle 3 robots klaar zijn
            {
                robots[0].SetRHeen(true);
                robots[1].SetRHeen(true);
                robots[2].SetRHeen(true);

                s.Unload();
            }

            if (s.GetDepart())
            {
                robots[0].SetDone(false);
                robots[1].SetDone(false);
                robots[2].SetDone(false);
                s.SetSuper();
                s.SetDepart(false);
            }
            return true;
        }
    }

    internal class Unsubscriber<Command> : IDisposable
    {
        private List<IObserver<Command>> _observers;
        private IObserver<Command> _observer;

        internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }


}