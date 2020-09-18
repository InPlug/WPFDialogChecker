using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using NetEti.Globals;
using NetEti.ApplicationControl;
using System.Threading.Tasks;
using Vishnu.Interchange;
using System.Threading;
using WPFDialogChecker.ViewModel;

namespace WPFDialogChecker
{
    /// <summary>
    /// Vishnu-Dialog-Checker zur manuellen Eingabe von Checker-Ergebnissen: true, false, null oder Exception.
    /// </summary>
    public class WPFDialogChecker : INodeChecker
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        public WPFDialogChecker()
        {
            //// Die Haupt-Klasse der Geschäftslogik
            //this._mainBusinessLogic = new Model.MainBusinessLogic();

            //// Das Main-Window
            //this._mainWindow = new View.MainWindow();

            //// Das MainBusinessLogic-ViewModel
            //this._mainBusinessLogicViewModel = new MainBusinessLogicViewModel(this._mainBusinessLogic, this._mainWindow);

            //// Das Main-ViewModel
            //this._mainWindowViewModel = new MainWindowViewModel(this._mainBusinessLogicViewModel, this._mainWindow.InitWindowSize);

            //// Verbinden von Main-Window mit Main-ViewModel
            //this._mainWindow.DataContext = this._mainWindowViewModel; //mainViewModel;
        }

        /// <summary>
        /// Kann aufgerufen werden, wenn sich der Verarbeitungs-Fortschritt
        /// des Checkers geändert hat, muss aber zumindest aber einmal zum
        /// Schluss der Verarbeitung aufgerufen werden.
        /// </summary>
        public event CommonProgressChangedEventHandler NodeProgressChanged;

        /// <summary>
        /// Rückgabe-Objekt des Checkers (zusätzlich zum Check-Result (bool?)).
        /// </summary>
        public object ReturnObject { get; set; }

        /// <summary>
        /// Startet den Checker - wird von einem Knoten im LogicalTaskTree aufgerufen.
        /// Checker liefern grundsätzlich true oder false zurück. Darüber hinaus können
        /// weiter gehende Informationen über das ReturnObject transportiert werden;
        /// Hier wird DateTime.Now über das ReturnObject zurückgegeben.
        /// </summary>
        /// <param name="checkerParameters">Spezifische Aufrufparameter oder null.</param>
        /// <param name="treeParameters">Für den gesamten Tree gültige Parameter oder null.</param>
        /// <param name="source">Auslösendes TreeEvent oder null.</param>
        /// <returns>True, False oder null</returns>
        public bool? Run(object checkerParameters, TreeParameters treeParameters, TreeEvent source)
        {
            // Parameterübernahme
            string callingNodeId = source.NodeName; ;
            if (source.Results != null && source.Results.Count > 0)
            {
                Result lastResult = source.Results.First().Value;
            }

            // Die Haupt-Klasse der Geschäftslogik
            this._mainBusinessLogic = new Model.MainBusinessLogic(callingNodeId);

            // Das Main-Window
            this._mainWindow = new View.MainWindow();

            // Das MainBusinessLogic-ViewModel
            this._mainBusinessLogicViewModel = new MainBusinessLogicViewModel(this._mainBusinessLogic, this._mainWindow);

            // Das Main-ViewModel
            this._mainWindowViewModel = new MainWindowViewModel(this._mainBusinessLogicViewModel, this._mainWindow.InitWindowSize);

            // Verbinden von Main-Window mit Main-ViewModel
            this._mainWindow.DataContext = this._mainWindowViewModel; //mainViewModel;
            this.ReturnObject = null;
            this.OnNodeProgressChanged(String.Format("{0}", this.GetType().Name), 100, 50, ItemsTypes.items);

            this._mainWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            Point parentViewAbsoluteScreenPosition = treeParameters.GetParentViewAbsoluteScreenPosition();
            this._mainWindow.Left = parentViewAbsoluteScreenPosition.X - this._mainWindow.ActualWidth / 2;
            this._mainWindow.Top = parentViewAbsoluteScreenPosition.Y - this._mainWindow.ActualHeight / 2;

            this._mainWindow.ShowDialog();
            this.OnNodeProgressChanged(String.Format("{0}", this.GetType().Name), 100, 100, ItemsTypes.items);
            if (this._mainBusinessLogicViewModel.LastException != null)
            {
                InfoController.Say("User clicked Exp");
                throw this._mainBusinessLogicViewModel.LastException;
            }
            if (this._mainWindow.DialogResult.HasValue && this._mainWindow.DialogResult.Value == true)
            {
                InfoController.Say("User clicked True");
            }
            else
            {
                InfoController.Say("User clicked False");
            }
            this.ReturnObject = checkerParameters.ToString();
            bool? rtn = this._mainBusinessLogic.LogicalState;

            this._mainWindow = null;
            this._mainWindowViewModel = null;
            this._mainBusinessLogicViewModel = null;
            this._mainBusinessLogic = null;

            return rtn;
        }

        private View.MainWindow _mainWindow;
        private Model.MainBusinessLogic _mainBusinessLogic;
        private ViewModel.MainBusinessLogicViewModel _mainBusinessLogicViewModel;
        private ViewModel.MainWindowViewModel _mainWindowViewModel;

        private void OnNodeProgressChanged(string itemsName, int countAll, int countSucceeded, ItemsTypes itemsType)
        {
            if (NodeProgressChanged != null)
            {
                NodeProgressChanged(null, new CommonProgressChangedEventArgs(itemsName, countAll, countSucceeded, itemsType, null));
            }
        }

    }
}
