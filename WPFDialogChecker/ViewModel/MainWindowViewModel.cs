using System;
using NetEti.MVVMini;
using System.Windows.Input;

namespace WPFDialogChecker.ViewModel
{
    /// <summary>
    /// ViewModel für das MainWindow.
    /// </summary>
    /// <remarks>
    /// File: MainWindowViewModel
    /// Autor: Erik Nagel
    ///
    /// 09.01.2014 Erik Nagel: erstellt
    /// </remarks>
    public class MainWindowViewModel : ObservableObject
    {
        #region public members

        /// <summary>
        /// ViewModel für den LogicalTaskTree.
        /// </summary>
        public MainBusinessLogicViewModel MainBusinessLogicViewModel_
        {
            get
            {
                return this._mainBusinessLogicViewModel_;
            }
            set
            {
                if (this._mainBusinessLogicViewModel_ != value)
                {
                    this._mainBusinessLogicViewModel_ = value;
                    this.RaisePropertyChanged("MainBusinessLogicViewModel_");
                }
            }
        }

        /// <summary>
        /// Setzt die Fenstergröße unter Berücksichtigung von Maximalgrenzen auf die
        /// Höhe und Breite des Inhalts und die Property SizeToContent auf WidthAndHeight.
        /// </summary>
        public ICommand? InitSizeCommand { get { return this._initSizeRelayCommand; } }

        /// <summary>
        /// Konstruktor - übernimmt das mainBusinessLogicViewModel und eine Methode des
        /// MainWindows zum Restaurieren der Fenstergröße abhängig vom Fensterinhalt.
        /// </summary>
        /// <param name="mainBusinessLogicViewModel">ViewModel für den LogicalTaskTree.</param>
        /// <param name="initWindowSize">Restauriert die Fenstergröße abhängig vom Fensterinhalt.</param>
        public MainWindowViewModel(MainBusinessLogicViewModel mainBusinessLogicViewModel, Action<object?> initWindowSize)
        {
            this._mainBusinessLogicViewModel_ = mainBusinessLogicViewModel;
            this._initSizeRelayCommand = new RelayCommand(initWindowSize);
        }

        #endregion public members

        #region private members

        private RelayCommand? _initSizeRelayCommand;
        private MainBusinessLogicViewModel _mainBusinessLogicViewModel_;

        #endregion private members

    }
}
