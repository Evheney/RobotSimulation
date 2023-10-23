//using chip_counter.mcu_Board;
using chip_counter.tmc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace chip_counter
{
    public sealed class GPIO_DEF
    {
        // GPIO in list
        public const int IN_SYSTEM_START = 0;
        public const int IN_DOOR_CLOSE = 1;
        public const int IN_START = 2;
        public const int IN_RETURN = 3;
        public const int IN_STATGE_IN_SENSOR = 4;
        public const int IN_STATGE_OUT_SENSOR = 5;
        public const int IN_DETECTOR_UP_SENSOR = 6; // 
        public const int IN_DETECTOR_DOWN_SENSOR = 7; //
        public const int IN_FRONT_AREA_SENSOR = 8;
        public const int IN_AIR_SENSOR = 9; // FAN IO 로 변경 예정 (2021.06.21)
        public const int IN_EMERGENCY = 10;
        public const int IN_MOTOR_ERROR = 11; //
        public const int IN_REEL_SENSOR1 = 12;
        public const int IN_REEL_SENSOR2 = 13;
        public const int IN_REEL_SENSOR3 = 14;
        public const int IN_REEL_SENSOR4 = 15;

        //commented sensors -> the sensors which might be deleted in future. Not used in current configuration.

        // GPIO out list
        public const int OUT_XRAY_ON = 0;
        public const int OUT_START_LAMP = 2;
        public const int OUT_RETURN_LAMP = 3;
        public const int OUT_STAGE_IN = 4;
        public const int OUT_STAGE_OUT = 5;
        //현재 사용 하지 않는 Out list
        public const int OUT_DETECTOR_UP = 6; //
        public const int OUT_DETECTOR_DOWN = 7; //
        public const int OUT_TOWER_GREEN = 8; //
        public const int OUT_TOWER_YELLOW = 9; //
        public const int OUT_TOWER_RED = 10; //
        public const int OUT_TOWER_BUZZ = 11; //

    };

    class GPIOProc
    {
        const int TMC_GPIO_MODE = 0;
        const int MCU_GPIO_MODE = 1;
        private static GPIOProc _instance = null;

        private GPIO_device _gpio_device = null;
        //private TcpMCUBoardDevice _mcu_device = null;
        private string LastError = "";
        private int _gpio_mode = 0;

        private Logger _log = null;

        //public delegate void GPIOProcLogEventFunc(string msg);
        //public event GPIOProcLogEventFunc GPIOProcLog;

        public bool SET_OUT_IO_READ = true;
        public bool OPEN
        {
            get
            {
                bool res = false;
                if (_gpio_mode == TMC_GPIO_MODE)
                    res = (_gpio_device == null ? false : _gpio_device.OPEN);
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    res = (_mcu_device == null ? false : _mcu_device.OPEN);

                return res;
            }
        }

        public BitArray IO_IN
        {
            get
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    return (_gpio_device == null ? null : _gpio_device.IO_IN);
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    return (_mcu_device == null ? null : _mcu_device.IO_IN);

                return null;
            }
        }

        public BitArray IO_OUT
        {
            get
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    return (_gpio_device == null ? null : _gpio_device.IO_OUT);
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    return (_mcu_device == null ? null : _mcu_device.IO_OUT);

                return null;
            }
        }
        //public int STATE
        //{
        //    get
        //    {
        //        if (_gpio_mode == MCU_GPIO_MODE)
        //            return (_mcu_device == null ? GPIO_STATE.ERROR : _mcu_device.MACHINE_STATE);
        //        else
        //            return GPIO_STATE.RUNNING;
        //    }
        //    set
        //    {
        //        if (_gpio_mode == MCU_GPIO_MODE)
        //        {
        //            if (_mcu_device != null)
        //            {
        //                _mcu_device.MACHINE_STATE = value;
        //            }
        //        }
        //    }
        //}

        public static GPIOProc Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GPIOProc();
                }

                return _instance;
            }
        }

        //public bool EDIT_MODE
        //{
        //    get
        //    {
        //        if (_gpio_mode == MCU_GPIO_MODE)
        //        {
        //            if (_mcu_device.EDIT_MODE)
        //                return true;
        //        }
        //        return false;
        //    }
        //}
        //private void Log.Info(string log)
        //{
        //    try
        //    {
        //        Log.Info(log);
        //    }
        //    catch { }
        //}
        public Logger Log
        {
            get
            {
                if (_log == null)
                    _log = Logger.Instance;

                return _log;
            }
        }
        public bool Init(configure config)
        {
            _gpio_mode = config.GPIO_MODE;

#if DEBUG
            Log.Info("Gpio Mode : " + "In Debug Mode 0 ");
            _gpio_device = GPIO_device.Instance;
#else

            Log.Info("Gpio Mode : " + _gpio_mode.ToString());
            if (_gpio_mode == TMC_GPIO_MODE)
            {
                _gpio_device = GPIO_device.Instance;
                if (_gpio_device.Init() == false)
                {
                    Log.Info("[GPIO] Device Init Failed!");
                    return false;
                }
                Log.Info("[GPIO] Device Init Complete!");
            }
#endif
            //else if (_gpio_mode == MCU_GPIO_MODE)
            //{
            //    _mcu_device = TcpMCUBoardDevice.Instance;
            //    //_mcu_device.WriteMessageEvent += WriteLog.Info;
            //    if (_mcu_device.Init() == false)
            //    {
            //        Log.Info("[MCU] Device Init Failed!");
            //        return false;
            //    }
            //    //_mcu_device.WriteMessageEvent += WriteLog.Info;
            //    Log.Info("[MCU] Device Init Complete!");
            //}
            return true;
        }

        public void DeInit()
        {
            if (_gpio_mode == TMC_GPIO_MODE)
            {
                if (_gpio_device != null)
                {
                    _gpio_device.DeInit();
                    _gpio_device = null;
                }
            }
            //else if (_gpio_mode == MCU_GPIO_MODE)
            //{
            //    if (_mcu_device != null)
            //    {
            //        //_mcu_device.WriteMessageEvent -= WriteLog.Info;
            //        _mcu_device.DeInit();
            //        _mcu_device = null;
            //    }
            //}
            _instance = null;
        }
        public bool IsPowerOff()
        {
            bool res = false;
            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    res = _gpio_device.IsPowerOff();
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    res = _mcu_device.IsPowerOff();

            }
            catch { return false; }

            return res;
        }

        /// <summary>
        /// 삼성에서 타워램프를 에러 시 노랑색으로 출력 요청함.
        /// </summary>
        /// <param name="bState"></param>
        public void SetErrorState(bool bState)
        {
            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                {
                    // _gpio_device.SetErrorState();
                }
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    _mcu_device.SetErrorState(bState);
            }
            catch { }
        }

        public void SetOutClear()
        {
            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    _gpio_device.SetOutClear();
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    _mcu_device.SetOutClear();
            }
            catch { }
        }


        public void SetOutToggle(int index, bool on_off = true)
        {
            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    _gpio_device.SetOutToggle(index, on_off);
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    _mcu_device.SetOutToggle(index, on_off);
            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOutToggle : " + ex.Message;
                Log.Info(LastError);
            }
        }

        //public void SetInit()
        //{
        //    try
        //    {
        //        if (_gpio_mode != TMC_GPIO_MODE)
        //            _mcu_device.SetInit();
        //    }
        //    catch (Exception ex)
        //    {
        //        LastError = "TMC() : SetInit : " + ex.Message;
        //    }
        //}
        //public void CheckStatus(bool val)
        //{
        //    try
        //    {
        //        if (_gpio_mode != TMC_GPIO_MODE)
        //            _mcu_device.CheckStatus(val);
        //    }
        //    catch (Exception ex)
        //    {
        //        LastError = "TMC() : CheckStatus : " + ex.Message;
        //    }
        //}

        public void SetOut(List<int> indexs, bool on_off)
        {
            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                {
                    //_gpio_device.setout(index, on_off);
                    _gpio_device.SetOut(indexs, on_off);
                    Log.Info($"SetOut for MCU index: {indexs} on_off : {on_off}");
                }
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //{
                //    Log.Info($"SetOut for MCU index: {indexs} on_off : {on_off}");
                //    _mcu_device.SetOut(indexs, on_off);
                //}
            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOut (list) : " + ex.Message;
            }
        }

        public void SetOut(int index, bool on_off)
        {
#if DEBUG
            Log.Debug($"SetOut : Index : {index} State : {on_off}");
#endif
#if DEBUG == false
            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    _gpio_device.SetOut(index, on_off);
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    //Log.Info($"SetOut for MCU index: {index} on_off : {on_off}");
                //    _mcu_device.SetOut(index, on_off);
            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOut : " + ex.Message;
            }
#endif
        }

        public bool GetIn()
        {
            bool res = false;
            LastError = "GetOut() : unknown error()";

            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    res = _gpio_device.GetIn();
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    res = _mcu_device.GetIn();
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetIn : " + ex.Message;
                Log.Info(LastError);
                res = false;
            }

            return res;
        }

        public bool GetIn(int index)
        {
            bool res = false;
            LastError = "GetOut() : unknown error()";

            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    res = _gpio_device.GetIn(index);
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    res = _mcu_device.GetIn(index);
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetIn : " + ex.Message;
                Log.Info(LastError);
                res = false;
            }

            return res;
        }

        public bool GetOut(int index)
        {
            bool res = false;
            LastError = "GetOut() : unknown error()";
#if DEBUG == false
            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    res = _gpio_device.GetOut(index);
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    res = _mcu_device.GetOut(index);
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetOut : " + ex.Message;
                Log.Info(LastError);
                res = false;
            }
#endif
            return res;
        }

        public bool GetOut()
        {
            bool res = false;
#if DEBUG == false
            LastError = "GetOut() : unknown error()";

            try
            {
                if (_gpio_mode == TMC_GPIO_MODE)
                    res = _gpio_device.GetOut();
                //else if (_gpio_mode == MCU_GPIO_MODE)
                //    res = _mcu_device.GetOut();
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetOut : " + ex.Message;
                Log.Info(LastError);
                res = false;
            }
#endif
            return res;
        }

        //public void WriteLog.Info(string log)
        //{
        //    Log.Info(log);
        //}
    }
}
