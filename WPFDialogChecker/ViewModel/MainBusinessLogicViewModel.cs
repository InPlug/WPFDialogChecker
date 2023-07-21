using System;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using NetEti.MVVMini;
using WPFDialogChecker.Model;
using System.Threading;
using System.Threading.Tasks;

namespace WPFDialogChecker.ViewModel
{
    /// <summary>
    /// ViewModel für die TreeView in LogicalTaskTreeControl im ersten Tab des MainWindow.
    /// </summary>
    /// <remarks>
    /// File: LogicalTaskTreeViewModel
    /// Autor: Erik Nagel
    ///
    /// 05.01.2013 Erik Nagel: erstellt
    /// </remarks>
    public class MainBusinessLogicViewModel : ObservableObject
    {
        #region public members

        #region published members

        /// <summary>
        /// Id des Aufrufenden Knotens im LogicalTaskTree plus Start-Frage.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return this.CallingNodeId + " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            }
            set
            {
                this.RaisePropertyChanged("WindowTitle");
            }
        }

        /// <summary>
        /// Id des Aufrufenden Knotens im LogicalTaskTree.
        /// </summary>
        public string CallingNodeId
        {
            get
            {
                return this._root.CallingNodeId;
            }
            set
            {
                this.RaisePropertyChanged("CallingNodeId");
            }
        }

        /// <summary>
        /// True, false oder null..
        /// </summary>
        public bool? LogicalState
        {
            get
            {
                return this._logicalState;
            }
            set
            {
                if (this._logicalState != value)
                {
                    this._logicalState = value;
                    this.RaisePropertyChanged("LogicalState");
                }
            }
        }

        /// <summary>
        /// Gesetzte Test-Exception oder null.
        /// </summary>
        public ApplicationException? LastException
        {
            get
            {
                return this._lastException;
            }
            set
            {
                if (this._lastException != value)
                {
                    this._lastException = value;
                    this.RaisePropertyChanged("LastException");
                }
            }
        }

        /// <summary>
        /// Command für den CmdTrue-Button in der MainBusinessLogic.
        /// </summary>
        public ICommand CmdTrue { get { return this._cmdTrueMainBusinessLogicRelayCommand; } }

        /// <summary>
        /// Command für den CmdFalse-Button in der MainBusinessLogic.
        /// </summary>
        public ICommand CmdFalse { get { return this._cmdFalseMainBusinessLogicRelayCommand; } }

        /// <summary>
        /// Command für den CmdNull-Button in der MainBusinessLogic.
        /// </summary>
        public ICommand CmdNull { get { return this._cmdNullMainBusinessLogicRelayCommand; } }

        /// <summary>
        /// Command für den CmdExp-Button in der MainBusinessLogic.
        /// </summary>
        public ICommand CmdExp { get { return this._cmdExpMainBusinessLogicRelayCommand; } }

        /// <summary>
        /// Command für den Break-Button im der MainBusinessLogic.
        /// </summary>
        public ICommand Break { get { return this._btnBreakMainBusinessLogicRelayCommand; } }

        #endregion published members

        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainBusinessLogicViewModel(MainBusinessLogic root, FrameworkElement uiMain)
        {
            this._root = root;
            this._uIMain = uiMain;
            this.LogicalState = null;
            this._lastException = null;
            //this._nodeId = "WPF"

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                this._uIDispatcher = this._uIMain.Dispatcher;
            }
            this._cmdTrueMainBusinessLogicRelayCommand = new RelayCommand(cmdTrueMainBusinessLogicExecute, canCmdTrueMainBusinessLogicExecute);
            this._cmdFalseMainBusinessLogicRelayCommand = new RelayCommand(cmdFalseMainBusinessLogicExecute, canCmdFalseMainBusinessLogicExecute);
            this._cmdNullMainBusinessLogicRelayCommand = new RelayCommand(cmdNullMainBusinessLogicExecute, canCmdNullMainBusinessLogicExecute);
            this._cmdExpMainBusinessLogicRelayCommand = new RelayCommand(cmdExpMainBusinessLogicExecute, canCmdExpMainBusinessLogicExecute);
            this._btnBreakMainBusinessLogicRelayCommand = new RelayCommand(breakMainBusinessLogicExecute, canBreakMainBusinessLogicExecute);

            this._root.StateChanged -= this.mainBusinessLogicStateChanged;
            this._root.StateChanged += this.mainBusinessLogicStateChanged;

        }

        #endregion public members

        #region private members

        private MainBusinessLogic _root;
        private RelayCommand _cmdTrueMainBusinessLogicRelayCommand;
        private RelayCommand _cmdFalseMainBusinessLogicRelayCommand;
        private RelayCommand _cmdNullMainBusinessLogicRelayCommand;
        private RelayCommand _cmdExpMainBusinessLogicRelayCommand;
        private RelayCommand _btnBreakMainBusinessLogicRelayCommand;
        private FrameworkElement _uIMain { get; set; }
        private System.Windows.Threading.Dispatcher? _uIDispatcher { get; set; }
        private bool? _logicalState;
        private ApplicationException? _lastException;

        // private MainBusinessLogicViewModel() { }

        private void mainBusinessLogicStateChanged(object sender, State state)
        {
            this.RaisePropertyChanged("CallingNodeId");
            this.LogicalState = this._root.LogicalState;
            // Die Buttons müssen zum Update gezwungen werden, da die Verarbeitung in einem
            // anderen Thread läuft:
            this._cmdTrueMainBusinessLogicRelayCommand.UpdateCanExecuteState(this.Dispatcher);
            this._btnBreakMainBusinessLogicRelayCommand.UpdateCanExecuteState(this.Dispatcher);
        }

        private void _waitAndClose()
        {
            CommandManager.InvalidateRequerySuggested();
            new TaskFactory().StartNew(new Action(() =>
            {
                Thread.Sleep(1000);
                if (this.Dispatcher.CheckAccess())
                    ((Window)this._uIMain).DialogResult = true;
                else
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(new Action(() => { ((Window)this._uIMain).DialogResult = true; })));
            }));
        }

        private void cmdTrueMainBusinessLogicExecute(object? parameter)
        {
            this._lastException = null;
            this._root.SetLogicalStateToTrue();
            this._waitAndClose();
        }

        private bool canCmdTrueMainBusinessLogicExecute()
        {
            return (this._root.ModelState & State.CanStart) > 0;
        }

        private void cmdFalseMainBusinessLogicExecute(object? parameter)
        {
            this._lastException = null;
            this._root.SetLogicalStateToFalse();
            this._waitAndClose();
        }

        private bool canCmdFalseMainBusinessLogicExecute()
        {
            return (this._root.ModelState & State.CanStart) > 0;
        }

        private void cmdNullMainBusinessLogicExecute(object? parameter)
        {
            this.LastException = null;
            this._root.SetLogicalStateToNull();
            this._waitAndClose();
        }

        private bool canCmdNullMainBusinessLogicExecute()
        {
            return (this._root.ModelState & State.CanStart) > 0;
        }

        private void cmdExpMainBusinessLogicExecute(object? parameter)
        {
            this._root.SetLogicalStateToNull();
            this.LastException = new ApplicationException("Test-Exception");
            this._waitAndClose();
        }

        private bool canCmdExpMainBusinessLogicExecute()
        {
            return (this._root.ModelState & State.CanStart) > 0;
        }

        private void breakMainBusinessLogicExecute(object? parameter)
        {
            this._root.Break();
        }

        private bool canBreakMainBusinessLogicExecute()
        {
            return (this._root.ModelState & State.CanStart) == 0;
        }

        #endregion private members

    }
}
