using System;
using NetEti.Globals;
using System.Windows;
using Vishnu.Interchange;

namespace WPFDialogChecker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            WPFDialogChecker wpfDialogChecker = new WPFDialogChecker();
            wpfDialogChecker.NodeProgressChanged += SubNodeProgressChanged;
            bool? logicalResult;
            try
            {
                // logicalResult = wpfDialogChecker.Run("Exception", new TreeParameters("MainTree", null), new TreeEvent("Testknoten", "Testknoten", "Testknoten",
                //     "Testknoten", "Testknoten", null, NodeLogicalState.None, null, null));
                //logicalResult = wpfDialogChecker.Run("null", new TreeParameters("MainTree", null), new TreeEvent("Testknoten", "Testknoten", "Testknoten",
                //     "Testknoten", "Testknoten", null, NodeLogicalState.None, null, null));
                //logicalResult = wpfDialogChecker.Run("true", new TreeParameters("MainTree", null), new TreeEvent("Testknoten", "Testknoten", "Testknoten",
                //     "Testknoten","Testknoten", null, NodeLogicalState.None, null, null));
                logicalResult = wpfDialogChecker.Run("---", new TreeParameters("MainTree", null), new TreeEvent("Testknoten", "Testknoten", "Testknoten",
                     "Testknoten", "Testknoten", null, NodeLogicalState.None, null, null));
                MessageBox.Show(String.Format("Result: {0}, ReturnObject: {1}", logicalResult.ToString(),
                    ((wpfDialogChecker.ReturnObject) ?? "null").ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Testknoten-Exception: {0}", ex.Message));
            }
        }

        static void SubNodeProgressChanged(object sender, CommonProgressChangedEventArgs args)
        {
            Console.WriteLine("{0}: {1} von {2}", args.ItemName, args.CountSucceeded, args.CountAll);
        }
    }
}
