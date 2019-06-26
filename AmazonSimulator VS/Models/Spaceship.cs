using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Spaceship : BaseClass
    {
        private bool unload = false, depart = false;
        private int super = 0;
        public Spaceship(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("vrachtwagen", x, y, z, rotationX, rotationY, rotationZ)
        {

        }

        public override bool Update(int tick)
        {
            Arrive();

            return base.Update(tick);
        }

        private void Arrive()
        {
            //ufo komt aan op (15, 0, -30) van (-550, o, -30)
            //dan moet ie stoppen en een seintje geven aan unload

            if (this.x >= -550 && this.x < 15)
            {
                this.Move(this.x + 5, this.y, this.z);
            }
            if (this.x >= 15)
            {
                unload = true;
            }
        }



        public void Unload()
        {
            // seintje robot koelkast moet in of uitladen
            //daarna moet ie een seintje geven aan depart
            Depart();
        }

        private void Depart()
        {
            //ufo gaat weg naar (550, 0, -30)  en teleport terug naar (-550, 0, -30)
            unload = false;
            this.Move(this.x + 5, this.y, this.z);
            if (this.x >= 550)
            {
                this.Move(-550, 0, -30);
                depart = true;
            }
        }

        public bool GetUnload()
        {
            return unload;
        }

        public void SetUnload(bool a)
        {
            unload = a;
        }

        public bool GetDepart()
        {
            return depart;
        }

        public void SetDepart(bool a)
        {
            depart = a;
        }

        public int GetSuper()
        {
            return super;
        }

        public void SetSuper()
        {
            super++;
        }
    }

}
