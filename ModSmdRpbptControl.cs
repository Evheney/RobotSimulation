using chip_counter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;


namespace RobotSimulation
{
    public partial class ModSmdRpbptControl : Form
    {
        public ModSmdRpbptControl()
        {
            InitializeComponent();
        }
        private int m_doReady;
        private int m_doInspectionDone;
        private int m_doBarcodeOK;
        private int m_doBarcodeNG;
        private int m_doReelIsNotRegistred;
        private string m_barcode;
        private bool m_barcodeIsNG;
        private string who = "SMDrobot: ";

        private int m_barcodeRead;
        private int m_placeReady;
        private int m_pickUpReady;
        private int m_Reset;

        private static SmdRobotControl _instance = null;
        //private Timer m_timer = new Timer();
        private System.Timers.Timer m_timer;

        GPIOProc gpio = GPIOProc.Instance;
        private ChipCounterInfo info = ChipCounterInfo.Instance;

        private const int SMD_BARCODE_READY = 1;
        private const int SMD_PLACE_READY = 2;
        private const int SMD_PICKUP_READY = 3;
        private const int SMD_RESET = 4;


        //Low level
        public event Action<bool, bool> TvReadyEvent;
        public event Action<bool> TvInspectionDoneEvent;
        public event Action<bool> TvBarcodeOKEvent;
        public event Action<bool> TvBarcodeNGEvent;
        public event Action<bool> TvReelIsNotRegisteredEvent;

        //High level

        public event Action SmdRobotInitEvent;

        public event Action<bool, bool> SmdSendInspectionDoneEvent;
        public event Action<bool> SmdSendTvReadyEvent;
        public event Action<bool, bool, bool> SmdSendPickupOrReadyEvent;
        public event Action<string> SmdBarcodeOKEvent;
        public event Action SmdBarcodeNGEvent;
        public event Action SmdSetTvReelIsNotRegisteredEvent;
        public event Action<bool> SmdBarcodeReadyEvent;

        private bool IsEnable()
        {
            return true; // info.config.ROBOT.SMD_ENABLE;
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

        public void SubscribeToAllEvents()
        {
            // Low level events
            TvReadyEvent += (param1, param2) => SetTvReady(param1, param2);
            TvInspectionDoneEvent += param => SetTvInspectionDone();
            TvBarcodeOKEvent += param => SetTvBarcodeOK();
            TvBarcodeNGEvent += param => SetTvBarcodeNG(param);
            TvReelIsNotRegisteredEvent += param => SetTvReelIsNotRegistered(param,true);

            // High level events
            SmdRobotInitEvent += () => SmdRobotInit();
            SmdSendInspectionDoneEvent += (param1, param2) => SmdSendInspectionDone(param1,param2);
            SmdSendTvReadyEvent += param => SmdSendTvReady(param);
            SmdSendPickupOrReadyEvent += (param1, param2, param3) => SmdSendPickupOrReady(param1,param2,param3);
            SmdBarcodeOKEvent += param => SmdBarcodeOK(param);
            SmdBarcodeNGEvent += () => SmdBarcodeNG();
            SmdSetTvReelIsNotRegisteredEvent += () => SmdSetTvReelIsNotRegistered();
            SmdBarcodeReadyEvent += param => SmdBarcodeReady(param);
        }

        #endregion
        ////////////////////////////////////////////////////////////////////////////////
        #region Low Level{
        public void SetTvReady(bool val, bool forced = false)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (gpio.Init(info.config))
            {
                bool prevState = gpio.IO_IN.Get(m_doReady);
                if (forced || prevState != val)
                {
                    // Instead of changing GPIO state, raise an event
                    TvReadyEvent?.Invoke(!prevState, false);
                }
            }
            else
            {
                UserMessage(who + "OUT_TV_READY is not specified in McDataAlphaAap.xml", EVS_WARN);
            }
        }
        
        public void SetTvInspectionDone()
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (gpio.Init(info.config))
            {
                bool prevState = gpio.IO_IN.Get(m_doInspectionDone);
                if (prevState)
                {
                    // Instead of changing GPIO state, raise the event
                    TvInspectionDoneEvent?.Invoke(true);
                    ResetBarcode();
                }
                else
                {
                    // Log the warning
                    UserMessage(who + "OUT_TV_INSPECTION_DONE is not specified in McDataAlphaAap.xml", EVS_WARN);
                }
            }
            else
            {
                // Log the error
                UserMessage(who + "--> TV INSPECTION DONE - Error", EVS_DEBUG);
            }
        }

        public void SetTvBarcodeOK()
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (gpio.Init(info.config))
            {
                if (m_barcodeIsNG)
                {
                    // Log a warning and ignore the event
                    UserMessage(who + "--> TV BARCODE OK - Event ignored (already NG)", EVS_WARN);
                }
                else
                {
                    bool prevState = gpio.IO_IN.Get(m_doBarcodeOK);
                    if (prevState)
                    {
                        // Raise the event based on the previous state
                        TvBarcodeOKEvent?.Invoke(true);
                    }
                    else
                    {
                        // Log the warning
                        UserMessage(who + "--> TV BARCODE OK - Error", EVS_DEBUG);
                    }
                }
            }
            else
            {
                // Log the error
                UserMessage(who + "OUT_TV_BARCODE_OK is not specified in McDataAlphaAap.xml", EVS_WARN);
            }
        }

        public void SetTvBarcodeNG(bool val)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (gpio.Init(info.config))
            {
                if (val)
                {
                    m_barcodeIsNG = true;
                    UserMessage(who + "Set BARCODE NG " + val, EVS_DEBUG);
                }

                bool prevState = gpio.IO_IN.Get(m_doBarcodeNG);
                if (prevState != val)
                {
                    // Raise the event based on the previous state
                    TvBarcodeNGEvent?.Invoke(val);
                }
            }
            else
            {
                // Log the error
                UserMessage(who + "OUT_TV_BARCODE_NG is not specified in McDataAlphaAap.xml", EVS_WARN);
            }
        }

        public void SetTvReelIsNotRegistered(bool val, bool forced)
        {
            // Place any initialization or hardware-specific code here
            // Ensure you have 'info' and 'config' available

            if (gpio.Init(info.config))
            {
                bool prevState = gpio.IO_IN.Get(m_doReelIsNotRegistred);
                if (prevState != val || forced)
                {
                    // Raise the event based on the previous state
                    TvReelIsNotRegisteredEvent?.Invoke(val);
                }
            }
            else
            {
                // Log the error
                UserMessage(who + "OUT_TV_REEL_IS_NOT_REGISTERED is not specified in McDataAlphaAap.xml", EVS_WARN);

                // Additional actions (set other IO states)
                SetTvBarcodeOK();
                SetTvBarcodeNG(val);
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

                // Raise an event to indicate initialization
                SmdRobotInitEvent?.Invoke();
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

                // Check if stage is unloaded
                if (inspectionIsFinished && stageIsUnloaded)
                {
                    const bool forcibly = false;
                    SetTvInspectionDone();
                }
                else if (!inspectionIsFinished)
                {
                    UserMessage(who + "Inspection is not finished yet", EVS_WARN);
                }
                else if (!stageIsUnloaded)
                {
                    UserMessage(who + "Stage is not unloaded", EVS_WARN);
                }

                // Raise an event to indicate inspection status
                SmdSendInspectionDoneEvent?.Invoke(inspectionIsFinished, stageIsUnloaded);
            }
        }

        public void SmdSendTvReady(bool ready)
        {
            if (IsEnable())
            {
                UserMessage(who + "SmdSendTvReady: " + ready, EVS_DEBUG);

                if (ready == false)
                {
                    SetTvReady(false);
                }
                else
                {
                    SetTvReelIsNotRegistered(false, false);
                    SetTvInspectionDone();
                    SetTvBarcodeNG(false);
                    SetTvBarcodeOK();
                    SetTvReady(true);
                }

                // Raise an event to indicate TV readiness
                SmdSendTvReadyEvent?.Invoke(ready);

                // If you want to include a delay with a message, you can do so here
                //if (theApp.IsCurrentUserAdmin())
                //    DelayWithMsg(1500, true, "LOGIN as user to operate with robot");
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
                    const bool forcibly = false;
                    SetTvInspectionDone();

                    // Raise an event to indicate TV inspection is done
                    SmdSendPickupOrReadyEvent?.Invoke(afterInspection, unloadResult, inspectionIsFinished);
                }
                else if (!afterInspection)
                {
                    SetTvReelIsNotRegistered(false, false);
                    SetTvInspectionDone();
                    SetTvBarcodeNG(false);
                    SetTvBarcodeOK();
                    SetTvReady(true);

                    // Raise an event to indicate TV is ready
                    SmdSendPickupOrReadyEvent?.Invoke(afterInspection, unloadResult, inspectionIsFinished);
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
                    SetTvReady(false);
                    SetBarcode(bc);

                    const bool forcibly = false;

                    SetTvBarcodeNG(false);
                    SetTvBarcodeOK();

                    // Raise an event to indicate that TV BARCODE OK is set
                    SmdBarcodeOKEvent?.Invoke(bc);
                }
            }
        }

        public void SmdBarcodeNG()
        {
            if (IsEnable())
            {
                const bool forcibly = false;
                SetTvBarcodeOK();
                SetTvBarcodeNG(true);

                // Raise an event to indicate that TV BARCODE NG is set
                SmdBarcodeNGEvent?.Invoke();
            }
        }

        public void SmdSetTvReelIsNotRegistered()
        {
            if (IsEnable())
            {
                const bool registered = false;
                const bool forcibly = false;

                SetTvReelIsNotRegistered(!registered, forcibly);
                SetTvReady(false);

                // Raise an event to indicate that TV Reel is not registered
                SmdSetTvReelIsNotRegisteredEvent?.Invoke();
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
                SetTvReady(false);

                // Check if Barcode is empty
                if (string.IsNullOrEmpty(Barcode()))
                {
                    // Call SmdBarcodeNG
                    SmdBarcodeNG();
                }

                // Raise the SmdBarcodeReadyEvent with the param value
                SmdBarcodeReadyEvent?.Invoke(param);
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
                UserMessage($"{who}UmSmdRobotControl: {paramStr}, wp: {wp}, lp: {lp}", EVS_DEBUG);
            }
            else
            {
                UserMessage($"{who}UmSmdRobotControl: SMD_UNKNOWN_{wp}, wp: {wp}, lp: {lp}", EVS_DEBUG);
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

                    if (i == m_barcodeRead)
                        cmd = SMD_BARCODE_READY;
                    else if (i == m_placeReady)
                        cmd = SMD_PLACE_READY;
                    else if (i == m_pickUpReady)
                        cmd = SMD_PICKUP_READY;
                    else if (i == m_Reset)
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
            m_doBarcodeOK = outBit; //14
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

        #endregion

    }
}
