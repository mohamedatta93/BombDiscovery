using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace BombDiscovery
{
    class Control
    {
     
       public RandomArray Simulation;
      public int nvisited = 0;

       NewGame newgame;
     public   bool gamefinish = false;
       public Control(NewGame m)
        {
            newgame = m;
       
           Simulation = new RandomArray(newgame.raws,newgame.columns, newgame.nbomb);

           
        }
    public void expand(int index1,int index2)
{
    if (newgame.buttons[index1][index2].IsEnabled==false||gamefinish==true)
        return;
    newgame.uncoverButton(index1, index2);
    
        if (Simulation.Places[index1][index2] != 0)
        return;
        
    if (Simulation.check(index1 - 1, index2 - 1)) expand(index1 - 1, index2 - 1);
    if (Simulation.check(index1-1, index2)) expand(index1 - 1, index2  );
    if (Simulation.check(index1-1, index2+1)) expand(index1 - 1, index2 + 1);
    if (Simulation.check(index1, index2-1)) expand(index1  , index2 - 1);
    if (Simulation.check(index1, index2+1)) expand(index1 , index2 + 1);
    if (Simulation.check(index1+1, index2-1)) expand(index1 + 1, index2 - 1);
    if (Simulation.check(index1+1, index2)) expand(index1 + 1, index2  );
  if (Simulation.check(index1+1, index2+1)) expand(index1 + 1, index2 + 1);

   
}

    }





}
