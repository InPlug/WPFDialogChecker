using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDialogChecker.ViewModel;

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
            InitializeComponent();
            this._contentRendered = false;
        }

        /// <summary>
        /// Setzt die Fenstergröße unter Berücksichtigung von Maximalgrenzen auf die
        /// Höhe und Breite des Inhalts und die Property SizeToContent auf WidthAndHeight.
        /// </summary>
        public void InitWindowSize(object parameter)
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
