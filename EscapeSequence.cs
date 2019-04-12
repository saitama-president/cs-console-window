using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Consoler {

  public class EscapeSequence {

    //    public static void MoveTo(int x,int y)=>Console.Write($"\u001b[{y+1};{x+1}H");

    public static void Clear () => Console.Write ($"\u001b[2J");

    public static void Blit (int left, int top, int width = 100, int height = 1, char c = ' ') {
      for (int y = top; y < top + height; y++) {
        for (int x = left; x < left + width; x++) {
          Console.SetCursorPosition (x, y);
          Console.Write (c);
        }
      }
    }
    public static void Clear (Rect bound, char fill = ' ') {
      for (int y = bound.top; y < bound.top + bound.height; y++) {
        for (int x = bound.left; x < bound.left + bound.width; x++) {
          Console.SetCursorPosition (x, y);
          Console.Write (fill);
        }
      }
    }

    public static void Blit (string source, Rect r) {
      int x = 0;
      int y = 0;
      int c = 0;

      for (int i = 0; i < source.Length &&
        i < r.width * r.height &&
        y < r.height; i++, c++) {
        int size = 1 == Encoding.UTF8.GetByteCount (source[i].ToString ()) ? 1 : 2;
        Console.Write (source[i]);
        if (1 < size) {
          x++;
        }

        x++;
        //折り返すか？
        if (r.width <= x) {
          MoveLeft (x);
          x = 0;
          y++;
        }
        c++;
      }
    }

    public static void WriteMB (string s, int maxLength = 999, int overflow = 80) {
      int x = 0;
      int y = 0;
      int c = 0;
      //BREAK
      //\e[1B ↓ \e[xD　←
      for (int i = 0; i < s.Length && c < maxLength; i++, c++) {
        int size = 1 == Encoding.UTF8.GetByteCount (s[i].ToString ()) ? 1 : 2;
        Console.Write (s[i]);
        if (1 < size) {
          x++;
        }
        x++;
        //折り返すか？
        if (overflow <= x) {
          MoveDown ();
          MoveLeft (x);
          x = 0;
          y++;
        }
        c++;
      }
    }

    public static int GetLength (string src) {
      int c = src.Sum (v => {
        return 1 == Encoding.UTF8.GetByteCount (v.ToString ()) ? 1 : 2;
      });
      return c;
    }

    public static void MoveLeft (int step = 1) {
      Console.Write ($"\u001b[{step}D");
    }
    public static void MoveRight (int step = 1) {
      Console.Write ($"\u001b[{step}C");
    }
    public static void MoveUp (int step = 1) {
      Console.Write ($"\u001b[{step}A");
    }
    public static void MoveDown (int step = 1) {
      Console.Write ($"\u001b[{step}B");
    }

    public static void MoveBottom () {
      Console.CursorLeft = 10;
      Console.Write ($"\u001b[{Console.WindowTop}B");
    }
  }

}