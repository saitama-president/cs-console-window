using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Consoler;

namespace Consoler {

  public class Screen : KeyBehaviour, Renderer {
    public Rect bound => Rect.From (0, 0, Console.WindowWidth, Console.WindowHeight);

    public string title = "タイトル";
    public string lastContents = "";
    public string latestContents = "";

    public ConsoleColor background = ConsoleColor.Black;
    public char fill = '#';

    public ConsoleColor foreGround = ConsoleColor.White;

    public Screen () {
      new Thread (() => {
        while (true) {
          ConsoleKeyInfo k = Console.ReadKey ();
          Console.Write (k.ToString ());
        }

      }).Start ();
    }

    public void Clear () {

      Console.ForegroundColor = ConsoleColor.Yellow;
      EscapeSequence.Clear (bound, '#');

    }

    public void Render () {
      RenderHeader ();

      RenderFooter ();
    }

    public void RenderHeader () {
      //必ず先頭行に書く
      Console.SetCursorPosition (0, 0);
      Console.Write ($"{0,30:D10}");
    }

    public void RenderBody () {
      //throw new NotImplementedException ();
    }

    public void RenderFooter () {
      Console.SetCursorPosition (0, Console.WindowHeight - 1);
      Console.Write ("----Footer----");
    }

    public void Refresh () {
      this.Clear ();
      Console.BackgroundColor = ConsoleColor.DarkRed;
      this.Render ();
    }
  }
}