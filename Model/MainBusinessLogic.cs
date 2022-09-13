using System;
using NetEti.ApplicationControl;

namespace WPFDialogChecker.Model
{
    /// <summary>
    /// Verarbeitungszustände einer Applikation.
    /// </summary>
    [Flags]
    public enum State
    {
        /// <summary>Startbereit, Zustand nach Initialisierung.</summary>
        None = 1,
        /// <summary>Beschäftigt, wartet auf Starterlaubnis.</summary>
        Waiting = 2,
        /// <summary>Beschäftigt, arbeitet.</summary>
        Working = 4,
        /// <summary>Startbereit, ist beendet.</summary>
        Done = 8,
        /// <summary>Startbereit, ist abgebrochen.</summary>
        Breaked = 16,
        /// <summary>Kann gestartet werden (None, Done oder Breaked).</summary>
        CanStart = None | Done | Breaked,
        /// <summary>Nicht startbereit, wartet oder arbeitet gerade (Waiting oder Working).</summary>
        Busy = Waiting | Working
    }

    /// <summary>
    /// Wird aufgerufen, wenn sich der Verarbeitungszustand eines Knotens geändert hat.
    /// </summary>
    /// <param name="sender">Die Ereignis-Quelle.</param>
    /// <param name="state">None, Waiting, Working, Finished, Busy (= Waiting | Working) oder CanStart (= None|Finished).</param>
    public delegate void StateChangedEventHandler(MainBusinessLogic sender, State state);

    /// <summary>
    /// Der Haupt-Einstiegspunkt für die Geschäftslogik.
    /// </summary>
    public class MainBusinessLogic
    {
        /// <summary>
        /// Wird aufgerufen, wenn sich der Verarbeitungszustand eines Knotens geändert hat.
        /// </summary>
        public event StateChangedEventHandler StateChanged;

        /// <summary>
        /// True, false oder null.
        /// </summary>
        public bool? LogicalState { get; set; }

        /// <summary>
        /// Der aktuelle Verarbeitungszustand der MainBusinessLogic. 
        /// </summary>
        public State ModelState
        {
            get
            {
                return this._modelState;
            }
            set
            {
                if (this._modelState != value)
                {
                    this._modelState = value;
                    this.OnStateChanged();
                }
            }
        }

        /// <summary>
        /// Id des aufrufenden Knoten aus Vishnu.
        /// Wird vom Checker gesetzt und vom ViewModel übernommen.
        /// </summary>
        public string CallingNodeId { get; set; }

        /// <summary>
        /// Standard Konstruktor.
        /// </summary>
        public MainBusinessLogic(string callingNodeId)
        {
            this.ModelState = State.None;
            this.CallingNodeId = callingNodeId;
            this.LogicalState = null;
        }

        /// <summary>
        /// Abbrechen der Task.
        /// Wenn der Knoten selber beschäftigt ist, dann diesen zum Abbruch veranlassen,
        /// ansonsten die Abbruch-Anforderung an alle Kinder weitergeben.
        /// </summary>
        public void Break()
        {
            this.LogicalState = null;
            this.ModelState = State.Breaked;
        }

        /// <summary>
        /// Setzt den logischen Zustand der MainBusinessLogic auf true.
        /// </summary>
        public void SetLogicalStateToTrue()
        {
            this.ModelState = State.Busy;
            this.LogicalState = true;
            this.ModelState = State.Done;
        }

        /// <summary>
        /// Setzt den logischen Zustand der MainBusinessLogic auf false.
        /// </summary>
        public void SetLogicalStateToFalse()
        {
            this.ModelState = State.Busy;
            this.LogicalState = false;
            this.ModelState = State.Done;
        }

        /// <summary>
        /// Setzt den logischen Zustand der MainBusinessLogic auf null.
        /// </summary>
        public void SetLogicalStateToNull()
        {
            this.ModelState = State.Busy;
            this.LogicalState = null;
            this.ModelState = State.Done;
        }

        /// <summary>
        /// Löst das NodeStateChanged-Ereignis aus.
        /// </summary>
        internal virtual void OnStateChanged()
        {
            if (StateChanged != null)
            {
                Statistics.Inc("LogicalNode.State.Get from LogicalNote.OnStateChanged");
                StateChanged(this, this.ModelState);
            }
        }

        private State _modelState;

    }
}
