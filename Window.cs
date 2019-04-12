using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Consoler;

using esc = Consoler.EscapeSequence;
using window = Consoler.Window;
//using root = Consoler.WindowGroup;

namespace Consoler {

  public class KeyBehaviour {

    public void OnSendKey (ConsoleKeyInfo k) { }
  }

  public class SimpleWindow : Window {
    public SimpleWindow (Rect r) : base (r) { }
  }

  public abstract class Window : KeyBehaviour {

    public Rect bound = Rect.From (left: 1, top: 1, width: 13, height: 5);
    public char background = ' ';
    public string title = "タイトル";
    public string lastContents = "";
    public string latestContents = "";

    public Window (Rect r) {
      this.bound = r;
    }
    /*
    後でabstractに変更
    */
    public void onSendKey (ConsoleKeyInfo k) {

      this.SetContents ("Key");
    }

    public static Window operator + (Window w, string c) {
      w.SetContents (c);
      return w;
    }

    public void SetContents (string s) {
      this.latestContents = s;
      this.lastContents = s;
      this.WriteContents ();
    }

    public void RenderBody () {
      //latest -> last
      this.Clear ();
      this.WriteContents ();

    }

    public void RenderHeader () {
      this.WriteBoarder ();
    }

    public void WriteContents () {
      //一文字ずつ
      string contents = this.lastContents;
      Console.SetCursorPosition (this.bound.left, this.bound.top);
      esc.WriteMB (contents, this.bound.width * this.bound.height, this.bound.width);
    }

    public void Clear () {
      esc.Clear (bound);
    }

    public void WriteBoarder () {
      Console.SetCursorPosition (
        this.bound.left - 1, this.bound.top - 1);
      //縦軸
      for (int y = this.bound.top - 1; y < this.bound.top + this.bound.height + 1; y++) {

        Console.SetCursorPosition (this.bound.left - 1, y);
        Console.Write ("|");
        Console.SetCursorPosition (this.bound.width + this.bound.left, y);
        Console.Write ("|");
      }
      //横軸
      for (int x = this.bound.left - 1; x < this.bound.left + this.bound.width + 1; x++) {
        string dot = (x == this.bound.left - 1 || x == this.bound.left + this.bound.width) ?
          "+" : "-";
        Console.SetCursorPosition (x, this.bound.top - 1);
        Console.Write (dot);
        Console.SetCursorPosition (x, this.bound.top + this.bound.height);
        Console.Write (dot);
      }
      //タイトルを書く
      int m = esc.GetLength (title);
      int offset = (this.bound.width - m) / 2;

      Console.SetCursorPosition (this.bound.left + offset, this.bound.top - 1);
      esc.WriteMB (title);
      //Console.Write(title);
    }
  }
}