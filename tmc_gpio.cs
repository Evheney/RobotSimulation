using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace chip_counter.tmc
{
    //public sealed class GPIO_DEF
    //{
    //    // GPIO in list
    //    public const int IN_SYSTEM_START = 0;
    //    public const int IN_DOOR_CLOSE = 1;
    //    public const int IN_START = 2;
    //    public const int IN_RETURN = 3;
    //    public const int IN_STATGE_IN_SENSOR = 4;
    //    public const int IN_STATGE_OUT_SENSOR = 5;
    //    public const int IN_DETECTOR_UP_SENSOR = 6;
    //    public const int IN_DETECTOR_DOWN_SENSOR = 7;
    //    public const int IN_FRONT_AREA_SENSOR = 8;
    //    public const int IN_AIR_SENSOR = 9; // FAN IO 로 변경 예정 (2021.06.21)
    //    public const int IN_EMERGENCY = 10;
    //    public const int IN_MOTOR_ERROR = 11;
    //    public const int IN_REEL_SENSOR1 = 12;
    //    public const int IN_REEL_SENSOR2 = 13;
    //    public const int IN_REEL_SENSOR3 = 14;
    //    public const int IN_REEL_SENSOR4 = 15;

    //    // GPIO out list
    //    public const int OUT_XRAY_ON = 0;
    //    public const int OUT_START_LAMP = 2;
    //    public const int OUT_RETURN_LAMP = 3;
    //    public const int OUT_STAGE_IN = 4;
    //    public const int OUT_STAGE_OUT = 5;
    //    public const int OUT_DETECTOR_UP = 6;
    //    public const int OUT_DETECTOR_DOWN = 7;
    //    public const int OUT_TOWER_GREEN = 8;
    //    public const int OUT_TOWER_YELLOW = 9;
    //    public const int OUT_TOWER_RED = 10;
    //    public const int OUT_TOWER_BUZZ = 11;
    //};

    class GPIO_device
    {
        private const int IO_OUT_CNT = 16;
        private const int IO_IN_CNT = 16;
        private BitArray _io_out = new BitArray(IO_OUT_CNT, false);
        private BitArray _io_in = new BitArray(IO_IN_CNT, false);

        private ushort _card_no = 0;
        private ushort _max_board_cnt = 1;
        private uint _io_in_cnt = 0;
        private uint _io_out_cnt = 0;

        public string LastError = "";
        public string MODEL = "";
        private static GPIO_device instance = null;
        private static readonly object gpio_device_lock = new object();
        private bool _is_open = false;
        private Thread gpio_thread = null;
        private AutoResetEvent gpio_exit_event = new AutoResetEvent(false);

        public bool OPEN
        {
            get { return _is_open; }
        }

        public BitArray IO_IN
        {
            get { return _io_in; }
        }

        public BitArray IO_OUT
        {
            get { return _io_out; }
        }

        public static GPIO_device Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GPIO_device();
                }
                return instance;
            }
        }

        public bool Init()
        {
            try
            {
                int IO_Cnt = TMCAEDLL.AIO_LoadDevice();
                uint model = 0, comm = 0, di_num = 0, do_num = 0;

                if (IO_Cnt < 0)
                {
                    LastError = "TMC() : IO Device Not found!";
                    return false;
                }

                TMCAEDLL.AIO_BoardInfo(_card_no, ref model, ref comm, ref _io_in_cnt, ref _io_out_cnt);
                switch (model)
                {
                    case tmcDef.TMC_AE:

                        //모델명
                        MODEL = String.Format("TMC-AE") + String.Format("{0:00}", _io_in_cnt) + "DIO";
                        break;

                    case tmcDef.TMC_AFDI:

                        //모델명
                        MODEL = String.Format("TMC-AF") + String.Format("{0:00}", _io_in_cnt) + "DI";
                        break;

                    case tmcDef.TMC_AFDO:

                        //모델명
                        MODEL = String.Format("TMC-AF") + String.Format("{0:00}", _io_out_cnt) + "DO";
                        break;

                    case tmcDef.TMC_AFDIO:

                        //모델명
                        MODEL = String.Format("TMC-AF") + String.Format("{0:00}", _io_in_cnt) + "DIO";
                        break;
                    default:
                        break;
                }

                gpio_thread = new Thread(new ThreadStart(gpio_proc));
                gpio_thread.Start();

                _is_open = true;

                IO_IN.Set(GPIO_DEF.IN_EMERGENCY, true);
                return true;
            }
            catch (Exception ex)
            {
                LastError = "TMC() : Init : " + ex.Message;
            }

            return false;
        }

        public void DeInit()
        {
            try
            {
                if (OPEN)
                {
                    gpio_exit_event.Set();

                    if (gpio_thread != null)
                        gpio_thread.Join(1000);

                    TMCAEDLL.AIO_UnloadDevice();
                }
            }
            catch (Exception)
            { }
        }

        public bool IsPowerOff()
        {
            try
            {
                if (OPEN)
                {
                    GetIn();
                    for (int i = 0; i < 12; i++)
                    {
                        if (IO_IN.Get(i))
                            return false;
                    }
                }

            }
            catch { }

            return true;
        }

        public void SetOutClear()
        {
            try
            {
                if (OPEN)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (i == 1)
                            continue;
                        ForceSetOut(i, false);
                    }
                }
            }
            catch { }
        }

        private void gpio_proc()
        {
            try
            {
                SetOutClear();

                while (true)
                {
                    if (gpio_exit_event.WaitOne(100))
                        break;

                    if (OPEN)
                    {
                        GetIn();
                        GetOut();
                    }
                }

            }
            catch (Exception)
            { }
        }
        private void TMC_OutputStatus()
        {
            try
            {
                lock (gpio_device_lock)
                {
                    if (IO_OUT_CNT <= 16)
                    {
                        ushort out_status = 0;
                        if (TMCAEDLL.AIO_GetDOWord(_card_no, 1, ref out_status) == 0)
                        {
                            for (int i = 0; i < IO_OUT_CNT; i++)
                            {
                                if (((out_status >> i) & 0x00001) == 0x0001)
                                    _io_out.Set(i, true);
                                else
                                    _io_out.Set(i, false);
                            }
                        }
                    }
                    else if (IO_OUT_CNT <= 32)
                    {
                        uint out_status = 0;
                        if (TMCAEDLL.AIO_GetDODWord(_card_no, 1, ref out_status) == 0)
                        {
                            for (int i = 0; i < IO_OUT_CNT; i++)
                            {
                                if (((out_status >> i) & 0x00001) == 0x0001)
                                    _io_out.Set(i, true);
                                else
                                    _io_out.Set(i, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void TMC_InputStatus()
        {
            try
            {
                lock (gpio_device_lock)
                {
                    if (IO_IN_CNT <= 16)
                    {
                        ushort in_status = 0;
                        if (TMCAEDLL.AIO_GetDIBit(_card_no, 1, ref in_status) == 0)
                        {
                            for (int i = 0; i < IO_OUT_CNT; i++)
                            {
                                if (((in_status >> i) & 0x00001) == 0x0001)
                                    _io_in.Set(i, true);
                                else
                                    _io_in.Set(i, false);
                            }
                        }
                    }
                    else if (IO_IN_CNT <= 32)
                    {
                        uint in_status = 0;
                        if (TMCAEDLL.AIO_GetDIDWord(_card_no, 1, ref in_status) == 0)
                        {
                            for (int i = 0; i < IO_OUT_CNT; i++)
                            {
                                if (((in_status >> i) & 0x00001) == 0x0001)
                                    _io_in.Set(i, true);
                                else
                                    _io_in.Set(i, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void SetOutToggle(int index, bool on_off = true)
        {
            try
            {
                lock (gpio_device_lock)
                {
                    if (on_off)
                    {
                        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_ON);
                        Thread.Sleep(200);
                        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_OFF);
                    }
                    else
                    {
                        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_OFF);
                        Thread.Sleep(200);
                        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_ON);
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOutToggle : " + ex.Message;
            }
        }

        private void ForceSetOut(int index, bool on_off)
        {
            try
            {
                lock (gpio_device_lock)
                {
                    if (on_off)
                        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_ON);
                    else
                        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_OFF);

                    _io_out.Set(index, on_off);
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOut : " + ex.Message;
            }
        }
        public void SetOut(List<int> indexs, bool on_off)
        {
#if DEBUG == false
            try
            {
                lock (gpio_device_lock)
                {
                    foreach (int index in indexs)
                    {
                        if (_io_out.Get(index) != on_off)
                        {
                            if (on_off)
                                TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_ON);
                            else
                                TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_OFF);

                            _io_out.Set(index, on_off);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOut : " + ex.Message;
            }
#endif
        }
        public void SetOut(int index, bool on_off)
        {
#if DEBUG == false
            try
            {
                lock (gpio_device_lock)
                {
                    if (_io_out.Get(index) != on_off)
                    {
                        if (on_off)
                            TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_ON);
                        else
                            TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_OFF);

                        _io_out.Set(index, on_off);
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOut : " + ex.Message;
            }
#endif
        }

        public bool GetIn()
        {
            LastError = "GetOut() : unknown error()";

            try
            {
                lock (gpio_device_lock)
                {
                    uint in_status = 0;

                    if (TMCAEDLL.AIO_GetDIDWord(_card_no, 0, ref in_status) >= 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            if (((in_status >> i) & 0x00001) == 0x0001)
                                _io_in.Set(i, true);
                            else
                                _io_in.Set(i, false);
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetIn : " + ex.Message;
            }

            return false;
        }

        public bool GetIn(int index)
        {
            LastError = "GetOut() : unknown error()";

            try
            {
                lock (gpio_device_lock)
                {
                    uint in_status = 0;

                    if (TMCAEDLL.AIO_GetDIDWord(_card_no, 0, ref in_status) < 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            if (((in_status >> i) & 0x00001) == 0x0001)
                                _io_in.Set(i, true);
                            else
                                _io_in.Set(i, false);
                        }

                        return _io_in.Get(index);
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetIn : " + ex.Message;
            }

            return false;
        }

        public bool GetOut(int index)
        {
            LastError = "GetOut() : unknown error()";
#if DEBUG == false
            try
            {
                lock (gpio_device_lock)
                {
                    ushort out_status = 0;

                    if (TMCAEDLL.AIO_GetDOWord(_card_no, 0, ref out_status) >= 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            if (((out_status >> i) & 0x00001) == 0x0001)
                                _io_out.Set(i, true);
                            else
                                _io_out.Set(i, false);
                        }

                        return _io_out.Get(index);
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetOut : " + ex.Message;
            }
#endif
            return false;
        }

        public bool GetOut()
        {
#if DEBUG == false
            LastError = "GetOut() : unknown error()";

            try
            {
                lock (gpio_device_lock)
                {
                    ushort out_status = 0;

                    if (TMCAEDLL.AIO_GetDOWord(_card_no, 0, ref out_status) >= 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            if (((out_status >> i) & 0x00001) == 0x0001)
                                _io_out.Set(i, true);
                            else
                                _io_out.Set(i, false);
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = "TMC() : GetOut : " + ex.Message;
            }
#endif
            return false;
        }
    }
}
