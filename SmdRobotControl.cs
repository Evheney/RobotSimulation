using System;
using System.Collections.Generic;
using System.Timers;

namespace chip_counter
{
    public class SmdRobotControl
    {

        private int m_doReady;
        private int m_doInspectionDone;
        private int m_doBarcodeOK;
        private int m_doBarcodeNG;
        private int m_doReelIsNotRegistred;
        private string m_barcode;
        private bool m_barcodeIsNG;
        private string who = "SMDrobot: ";

        private static SmdRobotControl _instance = null;
        private Timer m_timer = new Timer();

        GPIOProc gpio = GPIOProc.Instance;
        private ChipCounterInfo info = ChipCounterInfo.Instance;

        private const int SMD_BARCODE_READY = 1;
        private const int SMD_PLACE_READY = 2;
        private const int SMD_PICKUP_READY = 3;
        private const int SMD_RESET = 4;

        private event Action<int> SmdBarcodeReadyEvent;
        private event Action<int> SmdPlaceReadyEvent;
        private event Action<int> SmdPickupReadyEvent;
        private event Action<int> SmdResetEvent;



        //private Controller m_pController; // Assuming Controller is a class that represents your controller

        public static SmdRobotControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SmdRobotControl();
                }

                return _instance;
            }
        }
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

        public void SubscribeToEvents()
        {
            SmdBarcodeReadyEvent += SmdBarcodeReadyFunc;
            SmdPlaceReadyEvent += SmdPlaceReadyFunc;
            SmdPickupReadyEvent += SmdPickupReadyFunc;
            SmdResetEvent += SmdResetFunc;
        }
        public void RaiseEvents()
        {
            SmdBarcodeReadyEvent?.Invoke(1);
            SmdPlaceReadyEvent?.Invoke(2);
            SmdPickupReadyEvent?.Invoke(3);
            SmdResetEvent?.Invoke(4);
        }
        #region LowLevel {
        public void SetTvReady(bool val, bool forced = false)
        {
            if (gpio.Init(info.config) != false)// && gpio.IO_IN.Get(m_doReady)))
            {
                bool prevState = gpio.IO_IN.Get(m_doReady);
                if (forced || prevState != val)
                {
                    UserMessage(who + "--> TV READY " + val, EVS_DEBUG);
                    gpio.SetOut(m_doReady, val);
                }
            }
            else
            {
                UserMessage(who + "OUT_TV_READY is not specified in McDataAlphaAap.xml", EVS_WARN);
            }
        }

        public void SetTvInspectionDone(bool val, bool forced)
        {
            if (gpio.Init(info.config) != false)// && gpio.IO_IN.Get(m_doInspectionDone))
            {
                bool prevState = gpio.IO_IN.Get(m_doInspectionDone);
                if (forced || prevState != val)
                {
                    UserMessage(who + "--> TV INSPECTION DONE " + val, EVS_DEBUG);
                    gpio.SetOut(m_doInspectionDone, val);
                }

                ResetBarcode();
            }
            else
            {
                UserMessage(who + "--> TV INSPECTION DONE " + val, EVS_DEBUG);
                UserMessage(who + "OUT_TV_INSPECTION_DONE is not specified in McDataAlphaAap.xml", EVS_WARN);
            }
        }

        public void SetTvBarcodeOK(bool val, bool forced)
        {
            //m_timer.Stop();

            if (gpio.Init(info.config) != false)// && gpio.IO_IN.Get(m_Out_doBarcodeOK))
            {
                if (m_barcodeIsNG && val)
                {
                    UserMessage(who + "--> TV BARCODE OK " + val + " ignored (already NG)", EVS_WARN);
                }
                else
                {
                    bool prevState = gpio.IO_IN.Get(m_doBarcodeOK);
                    if (forced || prevState != val)
                    {
                        UserMessage(who + "--> TV BARCODE OK " + val, EVS_DEBUG);
                        gpio.SetOut(m_doBarcodeOK, val);
                    }
                }
            }
            else
            {
                UserMessage(who + "OUT_TV_BARCODE_OK is not specified in McDataAlphaAap.xml", EVS_WARN);
            }
        }

        public void SetTvBarcodeNG(bool val, bool forced)
        {
            //m_timer.Stop();

            if (gpio.Init(info.config) != false)// && gpio.IO_IN.Get(m_doBarcodeNG))
            {
                if (val)
                {
                    m_barcodeIsNG = true;
                    UserMessage(who + "Set BARCODE NG " + val, EVS_DEBUG);
                }

                bool prevState = gpio.IO_IN.Get(m_doBarcodeNG);
                if (forced || prevState != val)
                {
                    UserMessage(who + "--> TV BARCODE NG " + val, EVS_DEBUG);
                    gpio.SetOut(m_doBarcodeNG, val);
                }
            }
            else
            {
                UserMessage(who + "OUT_TV_BARCODE_NG is not specified in McDataAlphaAap.xml", EVS_WARN);
            }
        }

        public void SetTvReelIsNotRegistered(bool val, bool forced)
        {
            if (gpio.Init(info.config) != false)// && gpio.IO_IN.Get(m_doReelIsNotRegistred))
            {
                bool prevState = gpio.IO_IN.Get(m_doReelIsNotRegistred);
                if (forced || prevState != val)
                {
                    UserMessage(who + "--> TV REEL IS NOT REGISTERED " + val, EVS_DEBUG);
                    gpio.SetOut(m_doReelIsNotRegistred, val);
                }
            }
            else
            {
                UserMessage(who + "OUT_TV_REEL_IS_NOT_REGISTERED is not specified in McDataAlphaAap.xml", EVS_WARN);

                SetTvBarcodeOK(false, false);
                SetTvBarcodeNG(val, false);
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

        private bool IsEnable()
        {
            return true; // info.config.ROBOT.SMD_ENABLE;
        }
        #endregion

        #region High level { 
        public void SmdRobotInit() // SmdRobotConnect
        {
            // Hook up the Elapsed event
            SubscribeToEvents();
            RaiseEvents();

            if (IsEnable())
            {
                UserMessage(who + "SMD robot control " + (IsEnable() ? "enabled" : "disabled"),
                    EVS_DEBUG);

                //const bool timeoutEnable = g_StartupData.m_ccParams.Get<bool>(
                //    CChipCounterTaskParams::kSmdBarcodeReadingTimeoutEnable, false);
                //const UINT timeoutMs = g_StartupData.m_ccParams.Get<UINT>(
                //    CChipCounterTaskParams::kSmdBarcodeReadingTimeoutMs, 1000);

                //SetMessageTargetPtr(this);
                //SetMotionControllerPtr(glpMotionController);
                //SetTimeout(timeoutEnable, timeoutMs, false);

                UserMessage(who + "RESET output I/Os", EVS_DEBUG);
                const bool forcibly = true;

                SetTvReady(false, forcibly);
                SetTvInspectionDone(false, forcibly);
                SetTvBarcodeOK(false, forcibly);
                SetTvBarcodeNG(false, forcibly);
                SetTvReelIsNotRegistered(false, forcibly);
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
                    SetTvInspectionDone(true, forcibly);
                }
                else if (!inspectionIsFinished)
                {
                    UserMessage(who + "Inspection is not finished yet", EVS_WARN);
                }
                else if (!stageIsUnloaded)
                {
                    UserMessage(who + "Stage is not unloaded", EVS_WARN);
                }
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
                    SetTvInspectionDone(false, false);
                    SetTvBarcodeNG(false, false);
                    SetTvBarcodeOK(false, false);
                    SetTvReady(true);
                }

                //if (theApp.IsCurrentUserAdmin())
                //    DelayWithMsg(1500, true, _T("LOGIN as user to operate with robot"));
            }
        }
        public void SmdSendPickupOrReady(bool afterInspection, bool unloadResult, bool inspectionIsFinished)
        {

            if (IsEnable() && unloadResult)
            {
                // After counting is finished and stage is unloading : send '<-- TV_INSPECTION_DONE'
                // When stage is just unloaded without inspection send "TV_READY_ON"

                UserMessage(who + "SmdSendPickupOrReady: unloadResult: " + unloadResult
                                + ", after inspection: " + afterInspection,
                    EVS_DEBUG);

                if (afterInspection && inspectionIsFinished)
                {
                    const bool forcibly = false;

                   SetTvInspectionDone(true, forcibly);
                }
                else if (!afterInspection)
                {
                    SetTvReelIsNotRegistered(false, false);
                    SetTvInspectionDone(false, false);
                    SetTvBarcodeNG(false, false);
                    SetTvBarcodeOK(false, false);
                    SetTvReady(true);
                }
            }
        }
        public void SmdBarcodeOK(string bc)
        {

            if (IsEnable())
            {
                if (IsBarcodeNG())
                {

                    UserMessage(who + "--> TV BARCODE OK " + true + " ignored (already NG)",
                            EVS_WARN);
                }
                else
                {
                    SetTvReady(false);
                    SetBarcode(bc);

                    const bool forcibly = false;

                    SetTvBarcodeNG(false, false);
                    SetTvBarcodeOK(true, forcibly);
                }
            }
        }

        public void SmdBarcodeNG()
        {

            if (IsEnable())
            {
                const bool forcibly = false;
                SetTvBarcodeOK(false, false);
                SetTvBarcodeNG(true, forcibly);
            }
        }

        public void SmdSetTvReelIsNotRegistered(/*const std::string& bc*/)
        {

            if (IsEnable())
            {
                const bool registered = false;
                const bool forcibly = false;

                SetTvReelIsNotRegistered(!registered, forcibly);
                SetTvReady(false);
            }
        }

        public void SmdBarcodeReady(int param)
        {
            if (!IsEnable())
                return;

            UserMessage($"{who} <-- SMD BARCODE READY {param}", EVS_DEBUG);

            // bool forcibly = g_StartupData.m_ccParams.Get<bool>(CChipCounterTaskParams.kSmdForcedSetIO, false);

            if (param != 0)
            {
                SetTvReady(false);

                if (string.IsNullOrEmpty(Barcode()))
                {
                    SmdBarcodeNG();
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


        public void HandleSmdRobotControlMessage(int wp, int lp)
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
                    // Handle unhandled case or log a message
                    break;
            }
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

        #region Testing code {

        public void SmdBarcodeReadyFunc(int lp) 
        {
            gpio.SetOut(GPIO_DEF.IN_REEL_SENSOR2,true);
            UserMessage(who + "Set SmdBarcodeReadyFunc : " + GPIO_DEF.IN_REEL_SENSOR2.ToString());

        }
        public void SmdPlaceReadyFunc(int lp) 
        {
            gpio.SetOut(GPIO_DEF.IN_REEL_SENSOR3, true);
            UserMessage(who + "Set SmdPlaceReadyFunc : " + GPIO_DEF.IN_REEL_SENSOR3.ToString());
        }
        public void SmdPickupReadyFunc(int lp)
        {
            gpio.SetOut(GPIO_DEF.IN_REEL_SENSOR4, true);
            UserMessage(who + "Set SmdPickupReadyFunc : " + GPIO_DEF.IN_REEL_SENSOR4.ToString());
        }
        public void SmdResetFunc(int lp)
        {
            gpio.SetOut(GPIO_DEF.IN_REEL_SENSOR1, true);
            UserMessage(who + "Set SmdResetFunc : " + GPIO_DEF.IN_REEL_SENSOR1.ToString());
        }

        #endregion
    }

}