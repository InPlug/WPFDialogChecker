using System.Windows;
using View;

namespace WPFDialogChecker.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region public members

        const double MAXWIDTH = 1200;
        const double MAXHEIGTH = 800;

        /// <summary>
        /// Konstruktor des Haupt-Fensters.
        /// </summary>
        public MainWindow()
        {
            /* Der nachfolgende Lock wurde erforderlich, da ansonsten (bei sehr großen Jobs mit sehr vielen Controls) folgender Fehler auftreten kann:
             * Letztes Ergebnis: Node334: System.NullReferenceException: Der Objektverweis wurde nicht auf eine Objektinstanz festgelegt.
             * bei System.lO.Packaging.PackagePart.CleanUpRequestedStreamsList()
             * bei System.IO.Packaging.PackagePart.GetStream(FileMode mode, FileAccess access)
             * bei System.Windows.Application.LoadComponent(Object component, Uri resourceLocator)
             * bei WPFDialogChecker.View.MainWindow.InitializeComponent()
             * bei WPFDialogChecker.View.MainWindow..ctor()
             * bei WPFDialogChecker.WPFDialogChecker.Run(Object checkerParameters, TreeParameters treeParameters, TreeEvent source)
             * bei LogicalTaskTree.CheckerShell.runlt(Object checkerParameters, TreeParameters treeParameters, TreeEvent source) in C:\Users\micro\Documents\private4\WPF\Vishnu_Root\VishnuHome\Vishnu\LogicalTaskTree\CheckerShell.cs:Zeile 444.
             * bei LogicalTaskTree.CheckerShell.Run(Object checkerParameters, TreeParameters treeParameters, TreeEvent source) in C:\Users\micro\Documents\private4\WPF\Vishnu_Root\VishnuHome\Vishnu\LogicalTaskTree\CheckerShell.cs:Zeile 74.
             * bei LogicalTaskTree.SingleNode.DoRun(TreeEvent source) in C:\Users\micro\Documents\private4\WPF\Vishnu_Root\VishnuHome\Vishnu\LogicalTaskTree\SingleNode.cs:Zeile 538.
             * Weiterer Hinweis: LockHelper muss zwingend eine Klasse dieser Assembly sein, Auslagerung in NetEti.Globals führt bei großen Jobs zu Ladefehlern!
            */
            lock (LockHelper.Instance)
                InitializeComponent();
            this._contentRendered = false;
        }

        /// <summary>
        /// Setzt die Fenstergröße unter Berücksichtigung von Maximalgrenzen auf die
        /// Höhe und Breite des Inhalts und die Property SizeToContent auf WidthAndHeight.
        /// </summary>
        public void InitWindowSize(object? parameter)
        {
            this.MaxWidth = System.Windows.SystemParameters.VirtualScreenWidth; //MAXWIDTH;
            this.MaxHeight = System.Windows.SystemParameters.VirtualScreenHeight; //MAXHEIGTH;
            double additionalHeight = 0;
            double initialWidth = this.Width;
            if (initialWidth > this.MaxWidth)
            {
                initialWidth = this.MaxWidth;
                additionalHeight = 18;
            }
            double initialHeight = this.Height + additionalHeight;
            if (initialHeight > this.MaxHeight)
            {
                initialHeight = this.MaxHeight;
            }
            this.Height = initialHeight;
            this.Width = initialWidth;
            this._contentRendered = true;
            this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
        }

        #endregion public members

        #region private members

        private bool _contentRendered;

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            if (!this._contentRendered)
            {
                this.InitWindowSize(null);
            }
        }

        #endregion private members

    }
}
