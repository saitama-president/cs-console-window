using System;
using Consoler;

class test {
  public static void Main (string[] args) {
    Console.WriteLine ("OK!");
    Screen s = new Screen ();
    s.Refresh ();

    /*
        Window w = new SimpleWindow (Rect.From (
          left: 2, top: 2, width: 19, height: 3
        ));

        Console.Clear ();
        w.SetContents ("ぬるぽ");
        w.WriteContents ();
    */
  }
}