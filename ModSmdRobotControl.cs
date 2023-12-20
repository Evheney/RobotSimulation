using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace chip_counter
{
    public partial class ModSmdRobotControl : Form
    {
        public ModSmdRobotControl()
        {
            InitializeComponent();

            Set_In_Reset(12);                           //should be read from config
            Set_In_BarcodeReady(13);                    //should be read from config
            Set_In_PlaceReady(14);                      //should be read from config
            Set_In_Pickup_Ready(15);                    //should be read from config

            SetupTvReadyDO(12);                         //should be read from config
            SetupTvInspectionDoneDO(1);                 //should be read from config
            SetupTvBarcodeOkDO(14);                     //should be read from config
            SetupTvBarcodeNgDO(15);                     //should be read from config
            SetupTvReelIsNotRegisteredDO(13);           //should be read from config
        }
        /// <summary>
        /// init required variables
        /// </summary>
        public bool m_forcibly = false;

        private int m_doReady;

        private int m_Out_doBarcodeOK;
        private int m_doBarcodeNG;
        private int m_doReelIsNotRegistred;
        private int m_doInspectionDone;

        private int m_doPickupOrReady;
        bool m_placeWasStarted = false;

        private string m_barcode;
        private bool m_barcodeIsNG;

        public bool registered = false; //TODO//need to include from chipcounter form
        public bool m_bReelIsNotRegistered = true; //need to include from chipcounter form

        private int m_In_Reset = 12;                 //should be read from config
        private int m_In_BarcodeReady = 13;          //should be read from config
        private int m_In_PlaceReady = 14;            //should be read from config
        private int m_In_PickReady = 15;             //should be read from config

        private string who = "SMDrobot: ";

        public static ModSmdRobotControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModSmdRobotControl();
                }

                return _instance;
            }
        }

        private static ModSmdRobotControl _instance = null;
        //private Timer m_timer = new Timer();
        private System.Timers.Timer m_timer;

        GPIOProc gpio = GPIOProc.Instance;
        private ChipCounterInfo info = ChipCounterInfo.Instance;

        private const int SMD_BARCODE_READY = 1;
        private const int SMD_PLACE_READY = 2;
        private const int SMD_PICKUP_READY = 3;
        private const int SMD_RESET = 4;


        public event Action<bool> SmdBarcodeReadyEvent;
        public event Action<bool> SmdPickupReadyEvent;
        public event Action<bool> SmdPlaceReadyEvent;
        public event Action<bool> SmdResetEvent;


        private bool IsEnable()
        {
            return true; // TODO info.config.ROBOT.SMD_ENABLE;
        }

        #region Logger{
        //////////////////////////////////////////////////////////////////////////////
        private int EVS_DEBUG = 1;
        private int EVS_INFO = 2;
        private int EVS_WARN = 3;
        private int EVS_ERROR = 8;

        private Logger _log = null;

        public Logger Log
        {
            get
            {
                if (_log == null)
                    _log = Logger.Instance;

                return _log;
            }
        }

        private void UserMessage(string message)
        {
            Log.Info(message);
        }

        private void UserMessage(string message, int logLevel)
        {
            if (logLevel == EVS_DEBUG) { Log.Debug(message); }
            if (logLevel == EVS_INFO) { Log.Info(message); }
            if (logLevel == EVS_WARN) { Log.Warn(message); }
            if (logLevel == EVS_ERROR) { Log.Error(message); }
        }
        #endregion

        public void SubscribeToAllEvents()
        {

            SmdBarcodeReadyEvent += param => SmdBarcodeReady(param);

            SmdPickupReadyEvent += param => SmdPickupReady(param);
            SmdPlaceReadyEvent += param => SmdPlaceReady(param);

            SmdResetEvent += param => SmdReset(param);

        }

        private bool IsValidIO(int di)
        {
            // Implement the IsValidIO logic
#if DEBUG 
            return di >= 0;
#else
            if (gpio.OPEN && di >= 0) { return true; }

            return false;
#endif
        }

        ////////////////////////////////////////////////////////////////////////////////
        #region Low Level{
        public void SetTvReady(bool val, bool forced)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (IsValidIO(m_doReady))
            {
                bool prevState = gpio.IO_OUT.Get(m_doReady);
                if (forced || prevState != val)
                {
                    // TODO Instead of changing GPIO state, raise an event
                    //TvReadyEvent?.Invoke(!prevState, false);
                    UserMessage(who + "--> TV READY " + val, EVS_DEBUG);
                    gpio.SetOut(m_doReady, val);
                }
            }

        }

        public void SetTvInspectionDone(bool val)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (IsValidIO(m_doInspectionDone))
            {
                bool prevState = gpio.IO_OUT.Get(m_doInspectionDone);
                if (prevState != val)
                {
                    // TODO Instead of changing GPIO state, raise the event
                    //TvInspectionDoneEvent?.Invoke(true);
                    UserMessage(who + "--> TV INSPECTION DONE " + val, EVS_DEBUG);
                    gpio.SetOut(m_doInspectionDone, val);
                    ResetBarcode();
                }

            }

        }

        public void SetTvBarcodeOK(bool val)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (IsValidIO(m_Out_doBarcodeOK))
            {
                if (m_barcodeIsNG)
                {
                    // Log a warning and ignore the event
                    UserMessage(who + "--> TV BARCODE OK - Event ignored (already NG)", EVS_WARN);
                }
                else
                {
                    bool prevState = gpio.IO_OUT.Get(m_Out_doBarcodeOK);
                    if (prevState != val)
                    {
                        // TODO Raise the event based on the previous state
                        //TvBarcodeOKEvent?.Invoke(true);
                        UserMessage(who + "--> TV BARCODE OK " + val, EVS_DEBUG);
                        gpio.SetOut(m_Out_doBarcodeOK, val);
                    }
                }
            }

        }

        public void SetTvBarcodeNG(bool val)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (IsValidIO(m_doBarcodeNG))
            {
                if (val)
                {
                    m_barcodeIsNG = true;
                    UserMessage(who + "Set BARCODE NG " + val, EVS_DEBUG);
                }

                bool prevState = gpio.IO_OUT.Get(m_doBarcodeNG);
                if (prevState != val)
                {
                    // TODO Raise the event based on the previous state
                    //TvBarcodeNGEvent?.Invoke(val);
                    UserMessage(who + "--> TV BARCODE NG " + val, EVS_DEBUG);
                    gpio.SetOut(m_doBarcodeNG, val);
                }
            }

        }

        public void SetTvReelIsNotRegistered(bool val, bool forced)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (IsValidIO(m_doReelIsNotRegistred))
            {
                bool prevState = gpio.IO_OUT.Get(m_doReelIsNotRegistred);
                if (prevState != val || forced)
                {
                    // TODO Raise the event based on the previous state
                    //TvReelIsNotRegisteredEvent?.Invoke(val);
                    UserMessage(who + "--> TV REEL IS NOT REGISTERED " + val, EVS_DEBUG);
                    gpio.SetOut(m_doReelIsNotRegistred, val);
                }
            }
        }

        #region Barcode handling {
        string Barcode() { return m_barcode; }

        public void SetBarcode(string bc)
        {
            m_barcode = bc;
            UserMessage(who + "SET BARCODE TO " + bc, EVS_DEBUG);
        }

        public void ResetBarcode()
        {
            m_barcodeIsNG = false;

            if (!string.IsNullOrEmpty(m_barcode))
            {
                m_barcode = string.Empty;
                UserMessage(who + "RESET BARCODE", EVS_DEBUG);
            }
        }
        bool IsBarcodeNG() { return m_barcodeIsNG; }

        #endregion

        #endregion

        #region High level{

        public void SmdRobotInit()
        {
            // Hook up the Elapsed event
            SubscribeToAllEvents();

            // Create and configure the timer
            m_timer = new System.Timers.Timer(500); // 500 milliseconds
            m_timer.Elapsed += (sender, e) => MonitorIOChanges();
            m_timer.AutoReset = true;
            m_timer.Start();


            if (IsEnable())
            {
                UserMessage(who + "SMD robot control " + (IsEnable() ? "enabled" : "disabled"), EVS_DEBUG);

                // Additional initialization code specific to your hardware

                // TODO Raise an event to indicate initialization
                // SmdRobotInitEvent?.Invoke();
            }
        }

        public void SmdSendInspectionDone(bool inspectionIsFinished, bool stageIsUnloaded)
        {
            if (IsEnable())
            {
                bool isOK = (inspectionIsFinished && stageIsUnloaded);
                UserMessage(who + "Inspection is finished: " + inspectionIsFinished
                                + ", Stage is unloaded: " + stageIsUnloaded,
                    isOK ? EVS_DEBUG : EVS_WARN);

                // TODO Check if stage is unloaded
                if (inspectionIsFinished && stageIsUnloaded)
                {
                    //const bool forcibly = false;
                    SetTvInspectionDone(true);
                }
                else if (!inspectionIsFinished)
                {
                    UserMessage(who + "Inspection is not finished yet", EVS_WARN);
                }
                else if (!stageIsUnloaded)
                {
                    UserMessage(who + "Stage is not unloaded", EVS_WARN);
                }

                // TODO Raise an event to indicate inspection status
                //SmdSendInspectionDoneEvent?.Invoke(inspectionIsFinished, stageIsUnloaded);
            }
        }

        public void SmdSendTvReady(bool ready)
        {
            if (IsEnable())
            {
                UserMessage(who + "SmdSendTvReady: " + ready, EVS_DEBUG);

                if (ready == false)
                {
                    SetTvReady(false, m_forcibly);
                }
                else
                {
                    SetTvReelIsNotRegistered(false, m_forcibly);
                    SetTvInspectionDone(false);
                    SetTvBarcodeNG(false);
                    SetTvBarcodeOK(false);
                    SetTvReady(true, m_forcibly);
                }

                // TODO Raise an event to indicate TV readiness
                //SmdSendTvReadyEvent?.Invoke(ready);
            }
        }

        public void SmdSendPickupOrReady(bool afterInspection, bool unloadResult, bool inspectionIsFinished)
        {
            if (IsEnable() && unloadResult)
            {
                // After counting is finished and stage is unloading: send '<-- TV_INSPECTION_DONE'
                // When the stage is just unloaded without inspection, send "TV_READY_ON"

                UserMessage(who + "SmdSendPickupOrReady: unloadResult: " + unloadResult
                                + ", after inspection: " + afterInspection,
                    EVS_DEBUG);

                if (afterInspection && inspectionIsFinished)
                {
                    //const bool forcibly = false;
                    SetTvInspectionDone(true);

                    // Raise an event to indicate TV inspection is done
                    //SmdSendPickupOrReadyEvent?.Invoke(afterInspection, unloadResult, inspectionIsFinished);
                }
                else if (!afterInspection)
                {
                    SetTvReelIsNotRegistered(false, m_forcibly);
                    SetTvInspectionDone(false);
                    SetTvBarcodeNG(false);
                    SetTvBarcodeOK(false);
                    SetTvReady(true, m_forcibly);

                    // TODO Raise an event to indicate TV is ready
                    //SmdSendPickupOrReadyEvent?.Invoke(afterInspection, unloadResult, inspectionIsFinished);
                }
            }
        }

        public void SmdBarcodeOK(string bc)
        {
            if (IsEnable())
            {
                if (IsBarcodeNG())
                {
                    UserMessage(who + "--> TV BARCODE OK " + true + " ignored (already NG)", EVS_WARN);
                }
                else
                {
                    SetTvReady(false, m_forcibly);
                    SetBarcode(bc);

                    //const bool forcibly = false;

                    SetTvBarcodeNG(false);
                    SetTvBarcodeOK(true);

                    // TODO Raise an event to indicate that TV BARCODE OK is set
                    //SmdBarcodeOKEvent?.Invoke(bc);
                }
            }
        }

        public void SmdBarcodeNG()
        {
            if (IsEnable())
            {
                //const bool forcibly = false;
                SetTvBarcodeOK(false);
                SetTvBarcodeNG(true);

                // TODO Raise an event to indicate that TV BARCODE NG is set
                //SmdBarcodeNGEvent?.Invoke();
            }
        }

        public void SmdSetTvReelIsNotRegistered()
        {
            if (IsEnable())
            {
                const bool registered = false;

                SetTvReelIsNotRegistered(!registered, m_forcibly);
                SetTvReady(false, m_forcibly);

                // TODO Raise an event to indicate that TV Reel is not registered
                //SmdSetTvReelIsNotRegisteredEvent?.Invoke();
            }
        }

        public void SmdBarcodeReady(bool param)
        {
            if (!IsEnable())
                return;

            UserMessage($"{who} <-- SMD BARCODE READY {param}", EVS_DEBUG);

            // Check if param is not equal to 0
            if (param)
            {
                // Call SetTvReady with false
                SetTvReady(false, m_forcibly);

                // Check if Barcode is empty
                if (string.IsNullOrEmpty(Barcode()))
                {
                    // Call SmdBarcodeNG
                    SmdBarcodeNG();
                }

                // TODO Raise the SmdBarcodeReadyEvent with the param value
                //SmdBarcodeReadyEvent?.Invoke(param);
            }
        }
        void SmdPlaceReady(bool param)
        {
            if (!IsEnable())
                return;

            UserMessage(who + "<-- SMD PLACE READY " + param, EVS_DEBUG);

            if (param)
            {
                m_placeWasStarted = true;

                SetTvReady(false, m_forcibly);

                // NOTE: Do not reset BARCODE OK and BARCODE NG signals
            }
            else if (m_placeWasStarted)
            {
                m_placeWasStarted = false;

                string barcode = Barcode(); //TODO need to include from chipcounter form or imageviewer
                if (m_bReelIsNotRegistered)
                {
                    UserMessage(who + "Skip START_SCANNING. Reel is not registered.", EVS_WARN);
                    SmdSetTvReelIsNotRegistered();
                }
                else if (!string.IsNullOrEmpty(barcode))
                {
                    UserMessage(who + "Send START_SCANNING " + barcode, EVS_DEBUG);
                    // TODO Autostart needed here
                    //Autostart();
                }
                else
                {
                    UserMessage(who + "Skip START_SCANNING. Barcode is empty", EVS_WARN);
                    SmdBarcodeNG();
                }
            }
        }

        public void SmdPickupReady(bool param)
        {
            if (IsEnable())
                return;

            UserMessage($"{who} <-- SMD PICKUP READY {param}", EVS_DEBUG);

            if (param)
            {

                SetTvInspectionDone(true);
                SetTvReelIsNotRegistered(false, m_forcibly);
            }
            else
            {
                SetTvReelIsNotRegistered(false, m_forcibly);
                SetTvInspectionDone(false);
                SetTvBarcodeNG(false);
                SetTvBarcodeOK(false);
                SetTvReady(true, m_forcibly);
            }
        }

        public void SmdReset(bool param)
        {
            UserMessage($"{who} <-- SMD RESET {param}", EVS_DEBUG);

            bool smdRobotEnable = false; // TODO Replace this with your logic to get the enable status.

            if (!smdRobotEnable)
                return;

            if (param)
            {
                // Just ignore?
                // ++++> or try to unload the stage if it is not scanning?
                // or initialize the stage?

                // Remove barcode from barcode list in case of received SMD RESET signal
                m_barcode = "";

                //if nessesary
                GoHome();
            }
        }

        private void GoHome()
        {
            UserMessage($"{who} UNLOAD_STAGE", EVS_DEBUG);

            if (gpio.GetIn(GPIO_DEF.IN_STATGE_IN_SENSOR))
            {
                gpio.SetOut(GPIO_DEF.OUT_STAGE_OUT, true);
            }
            else if (!gpio.GetIn(GPIO_DEF.IN_STATGE_OUT_SENSOR))
            {
                gpio.SetOut(GPIO_DEF.OUT_STAGE_OUT, true);
            }

            while (true)
            {
                if (gpio.GetIn(GPIO_DEF.IN_STATGE_OUT_SENSOR))
                {
                    gpio.SetOut(GPIO_DEF.OUT_STAGE_OUT, false);
                    break;
                }
            }
        }
        #endregion

        private Dictionary<int, string> smdParamId = new Dictionary<int, string>
        {
        { 0, "SMD_SPARE(0)" },
        { 1, "SMD_BARCODE_READY(1)" },
        { 2, "SMD_PLACE_READY(2)" },
        { 3, "SMD_PICKUP_READY(3)" },
        { 4, "SMD_RESET(4)" }
        };
        /// Main function for using controls
        public void HandleSmdRobotControlMessage(int wp, bool lp)
        {

            if (smdParamId.ContainsKey(wp))
            {
                string paramStr = smdParamId[wp];
                UserMessage($"{who} SmdRobotControl: {paramStr}, wp: {wp}, lp: {lp}", EVS_DEBUG);
            }
            else
            {
                UserMessage($"{who} SmdRobotControl: SMD_UNKNOWN_{wp}, wp: {wp}, lp: {lp}", EVS_DEBUG);
            }
            switch (wp)
            {
                case SMD_BARCODE_READY:
                    SmdBarcodeReadyEvent?.Invoke(lp);
                    break;
                case SMD_PLACE_READY:
                    SmdPlaceReadyEvent?.Invoke(lp);
                    break;
                case SMD_PICKUP_READY:
                    SmdPickupReadyEvent?.Invoke(lp);
                    break;
                case SMD_RESET:
                    SmdResetEvent?.Invoke(lp);
                    break;
                default:
                    UserMessage($"{who}Unhandled message received: SMD_UNKNOWN_{wp}, wp: {wp}, lp: {lp}", EVS_DEBUG);
                    break;

            }
        }

        private bool[] previousIOState = new bool[16]; // Array to store the previous IO state

        public void MonitorIOChanges()
        {
            bool[] currentIOState = new bool[16];

            for (int i = 0; i < currentIOState.Length; i++)
            {
                currentIOState[i] = gpio.IO_IN.Get(i);
            }

            for (int i = 0; i < currentIOState.Length; i++)
            {
                if (currentIOState[i] != previousIOState[i])
                {
                    int cmd = -1; // Initialize j to an invalid value
                    bool value = currentIOState[i];

                    if (i == m_In_BarcodeReady)
                        cmd = SMD_BARCODE_READY;
                    else if (i == m_In_PlaceReady)
                        cmd = SMD_PLACE_READY;
                    else if (i == m_In_PickReady)
                        cmd = SMD_PICKUP_READY;
                    else if (i == m_In_Reset)
                        cmd = SMD_RESET;

                    if (cmd != -1)
                        HandleSmdRobotControlMessage(cmd, value);

                }
            }

            // Update the previous state after processing
            previousIOState = currentIOState;
        }

        #region Setup I/O signals numbers {

        public void SetupTvReadyDO(int outBit)
        {
            UserMessage(who + "Set OUT_TV_READY : " + outBit.ToString());
            m_doReady = outBit; //12
        }

        public void SetupTvInspectionDoneDO(int outBit)
        {
            UserMessage(who + "Set OUT_TV_INSPECTION_DONE : " + outBit.ToString());
            m_doInspectionDone = outBit; //1
        }

        public void SetupTvBarcodeOkDO(int outBit)
        {
            UserMessage(who + "Set OUT_TV_BARCODE_OK : " + outBit.ToString());
            m_Out_doBarcodeOK = outBit; //14
        }

        public void SetupTvBarcodeNgDO(int outBit)
        {
            UserMessage(who + "Set OUT_TV_BARCODE_NG : " + outBit.ToString());
            m_doBarcodeNG = outBit; //15
        }

        public void SetupTvReelIsNotRegisteredDO(int outBit)
        {
            UserMessage(who + "Set OUT_TV_REEL_IS_NOT_REGISTERED : " + outBit.ToString());
            m_doReelIsNotRegistred = outBit; //13
        }

        public void Set_In_Reset(int inBit)
        {
            UserMessage(who + "Set IN_RESET : " + inBit.ToString());
            m_In_Reset = inBit; //12
        }
        public void Set_In_BarcodeReady(int inBit)
        {
            UserMessage(who + "Set IN_BARCODEREADY : " + inBit.ToString());
            m_In_BarcodeReady = inBit; //13
        }
        public void Set_In_PlaceReady(int inBit)
        {
            UserMessage(who + "Set IN_PLACEREADY : " + inBit.ToString());
            m_In_PlaceReady = inBit; //14
        }
        public void Set_In_Pickup_Ready(int inBit)
        {
            UserMessage(who + "Set IN_PICKUP_READY : " + inBit.ToString());
            m_In_PickReady = inBit; //15
        }

        #endregion

        private void Form_Load(object sender, EventArgs e)
        {
            SmdRobotInit(); //TODO Include this in starting for Init device or else?
        }
    }
}
