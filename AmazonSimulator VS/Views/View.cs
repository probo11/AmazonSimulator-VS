using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Views
{
    public abstract class View 
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Command value) { }
    }
}
