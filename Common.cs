using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Consoler;

namespace Consoler {

  public struct Point {
    public int x, y;
    public Point (int x, int y) {
      this.x = x;
      this.y = y;
    }

    public static Point From (int x, int y) {
      return new Point (x, y);
    }
  }

  public struct Size {
    public int width, height;
    public Size (int w, int h) {
      this.width = w;
      this.height = h;
    }
    public static Size From (int w, int h) {
      return new Size (w, h);
    }
  }

  public struct Rect {
    public int left, top, width, height;
    public static Rect From (
      int left,
      int top,
      int width,
      int height
    ) {
      Rect r = new Rect ();
      r.left = left;
      r.top = top;
      r.width = width;
      r.height = height;
      return r;
    }

    public override string ToString () {
      return $"[ {left},{top}  -> {width},{height} ]";
    }
  }

  public interface Renderer {
    void RenderHeader ();
    void RenderBody ();
    void RenderFooter ();
    void Clear ();
    void Refresh ();
  }
}