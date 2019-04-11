using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using esc = EscapeSequence;
using window = ConsoleWindow;
using root = ConsoleWindowGroup;


public class Consoler {
  public static void Main(string[] args){
    esc.Clear();
    esc.MoveTo(0,0);

    window w=new window();
    w+="ぬるぽだよぬるぽだねぬるぬる";
    w.PreRender();
    w.Render();
    
    esc.MoveTo(0,20);
    new Thread(()=>{
      while(true){
        ConsoleKeyInfo k = Console.ReadKey(true);
        Console.Write($"KEY= {k.Key.ToString()}");
      }
    }).Start();
  }
}

public class EscapeSequence{
  public static string A=>"A";
  public static void MoveTo(int x,int y)=>Console.Write($"\u001b[{y+1};{x+1}H");
  
  public static void Clear()=>Console.Write($"\u001b[2J");

  public static void Blit(int left,int top,int width=100,int height=1,char c=' '){
    for(int y=top;y < top + height ; y++){
      for(int x=left;x < left+width; x++){
        MoveTo(y:y,x:x);
        Console.Write(c);
      }
    }
  }

  public static void WriteMB(string s,int maxLength= 999,int overflow = 80){
    
    int x=0;
    int y=0;
    int c=0;
    //BREAK
    //\e[1B ↓ \e[xD　←
    for(int i = 0;i<s.Length && c < maxLength;i++,c++){
      int size = 1==Encoding.UTF8.GetByteCount(s[i].ToString())?1:2;
      Console.Write(s[i]);
      if(1<size){
        x++;
      }
      x++;
      //折り返すか？
      if(overflow <= x){
        MoveDown();
        MoveLeft(x);
        x=0;
        y++;
      }
      c++;
    }
  }

  public static int GetLength(string src){
    int c = src.Sum(v=>{
      return 1==Encoding.UTF8.GetByteCount(v.ToString())?1:2;
    });
    return c;
  }
  
  public static void MoveLeft(int step=1){
    Console.Write($"\u001b[{step}D");
  }
  public static void MoveRight(int step=1){
    Console.Write($"\u001b[{step}C");
  }
  public static void MoveUp(int step=1){
    Console.Write($"\u001b[{step}A");
  }
  public static void MoveDown(int step=1){
    Console.Write($"\u001b[{step}B");
  }
};

public class ConsoleWindowGroup : List<window>{
  public ConsoleWindow ActiveWindow => this[0];
 
  public void Render(){
    foreach(ConsoleWindow w in this.Reverse()){
      w.PreRender();
      w.Render();
    }
  } 
}

public class ConsoleWindow{
  public int left = 2;
  public int top = 2;
  public char background = '_';
  public int width = 10;
  public int height = 2;

  public string title = "タイトル" ;

  public string lastContents="";
  public string latestContents="";

  public static ConsoleWindow operator +(ConsoleWindow w,string c){
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

  public ConsoleWindow(){
    
  }

  public void WriteContents(){
    //一文字ずつ
    string contents=this.lastContents;
    esc.MoveTo(left,top);
    esc.WriteMB(contents,this.width*this.height,this.width);
  }

  
  
  public void Clear(){
    esc.Blit(left,top,width,height,
    background);
  }
  public void WriteBoarder(){
    esc.MoveTo(left-1,top-1);
    //縦軸
    for(int y=top-1; y < top+height+1 ;y++){
      
      esc.MoveTo(left-1,y);
      Console.Write("|");   
      esc.MoveTo(width+left,y);
      Console.Write("|");   
    }
    //横軸
    for(int x=left-1; x < left+width+1 ;x++){
      string dot=(x==left-1 || x == left+width)
        ?"+":"-"
      ;
      esc.MoveTo(x,top-1);
      Console.Write(dot);
      esc.MoveTo(x,top+height);
      Console.Write(dot);
    }
    //タイトルを書く
    int m = esc.GetLength(title);
    int offset = (width - m) / 2;
    
    esc.MoveTo(left+offset,top-1);
    esc.WriteMB(title);
    //Console.Write(title);
  }
}
