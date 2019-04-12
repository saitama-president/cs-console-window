using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


using Consoler;


public class main {
  public static void Main(string[] args){
    WindowManager w=new WindowManager();

    WindowGroup app=new testWindowGroup();


    w.LoadWindow(app);

    w.Run();
  }
}
