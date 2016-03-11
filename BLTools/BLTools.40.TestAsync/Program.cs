using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools;
using BLTools.Async;
using System.Threading;

namespace TestAsync {
  class Program {
    static void Main(string[] args) {

      //Console.WriteLine("test.");

      //int Sum1 = Async.ExecuteAsync<int>(() => {
      //  int i;
      //  int LocalSum = 0;
      //  for (i = 1; i < 1000; i++) {
      //    Console.WriteLine("       {0}", i);
      //    LocalSum += i;
      //  }
      //  return LocalSum;
      //}, null).Result;

      //int Sum2 = Async.ExecuteAsync<int>(() => {
      //  int i;
      //  int LocalSum = 0;
      //  for (i = 1; i < 1000; i++) {
      //    Console.WriteLine("-------{0}", i);
      //    LocalSum += i;
      //  }
      //  return LocalSum;
      //}, null).Result;
      ////}, BackgroundLoopDone);

      //for (int i = 1; i < 1000; i++) {
      //  Console.WriteLine("{0}", i);
      //}

      //Console.WriteLine("Sum={0}", Sum1);
      //Console.WriteLine("Sum={0}", Sum2);
      //ConsoleExtension.Pause("completed");
    }

    static void BackgroundLoopDone(object sender, EventArgs e) {
      Console.WriteLine("************************************************");

    }
  }
}
