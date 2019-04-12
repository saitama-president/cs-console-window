using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Consoler;

using esc = Consoler.EscapeSequence;
using window = Consoler.Window;
using root = Consoler.WindowGroup;


namespace Consoler{

  public class AA{
    public static string Mona()=>@"
 ∧_∧
( '∀`)
|     |
";
  }

  public class WindowManager{

    public WindowManager(){

    }

    public WindowGroup currentWindow=null;


    public void LoadWindow(WindowGroup g){
      esc.Clear();
      this.currentWindow=g;
       
    }

    public void unLoadWindow(){
      //何も表示しない状態
    }

    public void Run(){

      new Thread(()=>{
        while(true){
          ConsoleKeyInfo k = Console.ReadKey(true);
//          Console.Write($"KEY= {k.Key.ToString()}");

          if(this.currentWindow!=null){
            this.currentWindow.SendKey(k);
          }
        }        
      }).Start(); 
    }
  }

  public class KeyBehaviour{

    public void OnSendKey(ConsoleKeyInfo k){

    }
    
  }

  public interface Renderer{
    void Render();
  }
  ///テスト用　ここから
  public class testWindowGroup:WindowGroup{
    public testWindowGroup(){
      this.Add(new Window());
      
    }

  }

  ///テスト用　ここまで

  public class WindowGroup : List<window>{

    public Window ActiveWindow => this[0];
    public static WindowGroup operator +(WindowGroup group,Window w){
      group.Add(w);
      return group;
    }
    

    public void SendKey(ConsoleKeyInfo k){
      
      this.ActiveWindow.onSendKey(k);

      //本質的にはrenderは別のタイミングで呼ぶべき

      this.ActiveWindow.PreRender();
      this.ActiveWindow.Render();
      
    }

    public void Render(){
      /*foreach(Window w in this.Reverse()){
        w.PreRender();
        w.Render();
      }
      */
    }
    
    public static WindowGroup Create(){
      WindowGroup w=new WindowGroup();
      return w;
    }
  }

  public class Window:KeyBehaviour{
    public int left = 2;
    public int top = 2;
    public char background = ' ';
    public int width = 16;
    public int height = 4;

    public string title = "タイトル" ;

    public string lastContents="";
    public string latestContents="";

    public int num =0;

    public Window(){

    }
    /*
    後でabstractに変更
    */
    public void onSendKey(ConsoleKeyInfo k){
 //       this.SetContents($"W={k.Key.ToString()}:{num}");
        this.SetContents(AA.Mona());
        num++;
    }

    public static Window operator +(Window w,string c){
      w.SetContents(c);

      return w;
    }

    public void SetContents(string s){
      this.latestContents=s;
    }


    public void Render(){
      //latest -> last
      if(latestContents != lastContents){
        lastContents = latestContents;
        this.Clear();
        this.WriteContents();
      }
    }

    public void PreRender(){
      this.WriteBoarder();
    }


    public void WriteContents(){
      //一文字ずつ
      string contents=this.lastContents;
      Console.SetCursorPosition(left,top);
      esc.WriteMB(contents,this.width*this.height,this.width);
    }

    public void Clear(){
      esc.Blit(left,top,width,height,
          background);
    }
    public void WriteBoarder(){
      Console.SetCursorPosition(left-1,top-1);
      //縦軸
      for(int y=top-1; y < top+height+1 ;y++){

        Console.SetCursorPosition(left-1,y);
        Console.Write("|");   
        Console.SetCursorPosition(width+left,y);
        Console.Write("|");   
      }
      //横軸
      for(int x=left-1; x < left+width+1 ;x++){
        string dot=(x==left-1 || x == left+width)
          ?"+":"-"
          ;
        Console.SetCursorPosition(x,top-1);
        Console.Write(dot);
        Console.SetCursorPosition(x,top+height);
        Console.Write(dot);
      }
      //タイトルを書く
      int m = esc.GetLength(title);
      int offset = (width - m) / 2;

      Console.SetCursorPosition(left+offset,top-1);
      esc.WriteMB(title);
      //Console.Write(title);
    }
  }
}
