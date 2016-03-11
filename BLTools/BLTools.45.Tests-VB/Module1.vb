Imports BLTools
Imports BLTools.ConsoleExtension

Module Module1

  Sub Main()
    Dim oArgs As New SplitArgs(My.Application.CommandLineArgs())
    Trace.Listeners.Add(New TimeStampTraceListener("execution.log"))
    Trace.AutoFlush = True

    Trace.WriteLine("Test of trace")
    Trace.WriteLine(String.Format("Number of arguments = {0}", oArgs.Count))

    For Each ArgumentItem As ArgElement In oArgs
      Trace.WriteLine(String.Format("{0} = {1}", ArgumentItem.Name, ArgumentItem.Value))
    Next

    Trace.WriteLine(String.Format("Argument 3 = {0}", oArgs(3).Name))

    If oArgs.IsDefined("help") Then
      Usage()
    End If

    Environment.Exit(0)
    ConsoleExtension.Pause()

  End Sub

  Sub Usage()
    Console.WriteLine("Usage: Test BLTools in VB")
    ConsoleExtension.Pause()
    Environment.Exit(1)
  End Sub

End Module
