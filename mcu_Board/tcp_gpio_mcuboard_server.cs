using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace chip_counter.mcu_Board
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
    //    public const int IN_DETECTOR_UP_SENSOR = 6;// < = STAGE IN DETECTOR 센서 PCB BOARD사용 으로 변경
    //    public const int IN_DETECTOR_DOWN_SENSOR = 7;//사용 안함
    //    public const int IN_FRONT_AREA_SENSOR = 8;
    //    public const int IN_AIR_SENSOR = 9; // FAN IO 로 변경 예정 (2021.06.21) // 사용안함
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
    //    //밑은 사용 안함
    //    public const int OUT_DETECTOR_UP = 6;
    //    public const int OUT_DETECTOR_DOWN = 7;
    //    public const int OUT_TOWER_GREEN = 8;
    //    public const int OUT_TOWER_YELLOW = 9;
    //    public const int OUT_TOWER_RED = 10;
    //    public const int OUT_TOWER_BUZZ = 11;
    //};
    public static class DEBUGMODE
    {
        static public bool GetMode()
        {
            return false;
            //#if DEBUG
            //            return true;
            //#else
            //            return false;
            //#endif
        }
    }
    public sealed class GPIO_STATE
    {
        public const int OPEN = 0;
        public const int MCU_WAIT = 1;
        public const int INIT = 2;
        public const int RUNNING = 3;
        public const int STAGE_INITIALIZE = 4;
        public const int STAGE_INITIALIZING = 5;
        public const int ERROR = 10;
        public const int ERROR_CODE4 = 11;
        public const int ERROR_CODE5 = 12;
        public const int ERROR_CODE6 = 13;
        public const int ERROR_CODE7 = 14;
        public const int ERROR_CODE8 = 15;
    }

    class TcpMCUBoardDevice
    {
        struct DATAS
        {
            public int packet_num;
            public int length;
            public int retry_count;
            public string stx;
            public string op_code;
            public string data;
            public string etx;
            public string cm_state;
            public string input_state;
            public string output_state;
        }

        private sealed class GPIO_OP_CODE
        {
            public const byte PACK_ACK = 0x00; // packet ack success
            public const byte PACK_NACK = 0x01; // fail
            public const byte PACK_INIT_SERVER = 0x02; // initialize server status
            public const byte PACK_INIT_CM = 0x20; // inicitialize client request
            public const byte PACK_STOC_STATUS = 0x30; // server to client request
            public const byte PACK_CTOS_STATUS = 0x03; // client to server answer
            public const byte PACK_STOC_OUPUT = 0x40; // server to client output
            public const byte PACK_CTOS_OUPUT = 0x04; // client to server answer
            public const byte PACK_STOC_AUTO_SEND_STATUS = 0x50; // auto send mode change state request
            public const byte PACK_CTOS_AUTO_SEND_STATUS = 0x05; // auto send mode change state answer  \brief same as ctos status
            public const byte PACK_STOC_STOP_AUTO_SEND_STATUS = 0x60; // auto send mode stop \brief send only when requested
        }

        //WORK STATE
        /**
         * \brief the order of requests Initialize read -> initialize write-> read mode
         *  default -> write mode
         */
        private const byte WRITE_MODE = 0x00;
        private const byte READ_MODE = 0x01;
        private const byte SET_GPIO_MODE = 0x02;
        private const byte ERROR_MODE = 0x03;
        private const byte INITIALIZE_READ_MODE = 0x04;
        private const byte INITIALIZE_WRITE_MODE = 0x05;

        //SET OUTPUT MODE
        public const byte NORMAL_MODE = 0x00; // 30 OP code
        //private const byte SET_OUTPU_INDEX_INIT_MODE = 0x01;
        public const byte SET_OUTPU_INDEX_PREPARE_MODE = 0x01; //OP code 40
        public const byte SET_OUTPU_INDEX_READY_MODE = 0x02; //OP Code 40 success

        //private const byte SET_OUTPUT_CLEAR_INIT_MODE = 0x04;
        public const byte SET_OUTPUT_CLEAR_PREPARE_MODE = 0x03; //OP code 40
        public const byte SET_OUTPUT_CLEAR_READY_MODE = 0x04; //OP code 40  -> output signal: false

        public const byte SET_AUTO_RECV_STATUS_MODE = 0x05; //OP code 50 
        public const byte SET_AUTO_RECV_STATUS_STOP_MODE = 0x06; //OP code 60


        private const int IO_OUT_CNT = 16;
        private const int IO_IN_CNT = 16;
        private const int PORT = 9001;
        private BitArray _io_out = new BitArray(IO_OUT_CNT, false);
        private BitArray _io_in = new BitArray(IO_IN_CNT, false);

        private Queue<byte> _queueOP = new Queue<byte>();
        private int lastIDAck = -1;
        private DateTime lastExecutionTime = DateTime.MinValue; // Initialize to the earliest possible time
        private DateTime lastExecutionTime2 = DateTime.MinValue; // Initialize to the earliest possible time
        private const int DELAY_MCU = 35;
        private bool is_send = false;
        private bool disconnect = false;
        public bool mcudisconnect = true;
        private bool waitNeededPacket = false;
        private object cyj_network_lock = new object();
        private byte _lastOpCodeToSend = 0;
        private bool _errorState = false;
        private bool isInitializingFinished = false;
        private bool failedinitialize = false;


        //private ushort _card_no = 0;
        //private ushort _max_board_cnt = 1;
        //private uint _io_in_cnt = 0;
        //private uint _io_out_cnt = 0;

        public string LastError = "";
        public string MODEL = "";
        public string FW_VER = "";
        public string HW_VER = "";
        public string PROTOCAL_VER = "";
        public int MACHINE_STATE = GPIO_STATE.OPEN;

        private static TcpMCUBoardDevice instance = null;
        private static readonly object gpio_device_lock = new object();
        private static readonly object send_lock = new object();
        private static readonly object setout_lock = new object();


        private bool _is_open = false;
        private Thread gpio_server_thread = null;
        private Thread gpio_thread = null;
        private AutoResetEvent gpio_exit_event = new AutoResetEvent(false);
        //private AutoResetEvent write_complete_event = new AutoResetEvent(false);

        private TcpListener _server = null;
        private TcpClient _client = null;

        private DATAS _data = new DATAS(); // all else exept 00 01
        private DATAS _send_data = new DATAS();
        private DATAS _response_data = new DATAS(); // ack nack when in use only 00 and 01 in use
        private byte _set_gpio_mode = NORMAL_MODE; // default mode OP Code 30

        private string state_str = "";
        private string state_old = "";
        private string lastsendpacket = "";
        private byte last_send_opcode = 0;

        private int init_mode = 0;

        //public bool initializing_finished = false;
        //private bool initializing_started = false;
        MessageForm initstage = new MessageForm();


        private byte _proc_state = INITIALIZE_READ_MODE;

        private BitArray _edit_io_out = new BitArray(IO_OUT_CNT, false); // if we have some changes in OP COde 40 we use this instead of io_out

        public delegate void MessageFormShowFunc(string msg, MessageForm initstage); //message form for showing message to operator or user.
        public event MessageFormShowFunc MessageFormShowEvent;


        public delegate void OKFunc(); //message form for showing message to operator or user.
        public event OKFunc OKEvent;


        private System.Windows.Forms.Timer read_timeout_timer = new System.Windows.Forms.Timer(); // if no respond in 3 sec send nack
        //private System.Windows.Forms.Timer repeat_timeout_timer = new System.Windows.Forms.Timer(); // if no respond in 500ms read data has to be received
        private bool auto_recv_data_mode = false; //if OP COde 50 in use then auto recieve data mode enable 
        public bool EDIT_MODE = false; //if OP 40 code is in esponse then in use // when get OP COde 40 then -> true response data OP Code 04 -> false

        /* 2023.05.11 */
        private Logger _log = null;

        public Logger Log
        {
            get
            {
                if (_log == null)
                    _log = new Logger("GPIO");

                return _log;
            }
        }



        /*   cyj */
        public byte GPIO_SEND_MODE
        {
            get
            {
                return _set_gpio_mode;
            }
        }
        public bool OPEN
        {
            get { return _is_open; }
        }

        public BitArray IO_IN
        {
            get
            {
                return _io_in;
            }
        }

        public BitArray IO_OUT
        {
            get { return _io_out; }
        }

        public static TcpMCUBoardDevice Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TcpMCUBoardDevice();
                }
                return instance;
            }
        }

        public bool IS_CONNECTED
        {
            get
            {
                if (_client != null && _client.Client != null && _client.Client.Connected)
                {
                    return true;
                }
                return false;
            }
        }

        async void AysncServer()
        {
            MACHINE_STATE = GPIO_STATE.OPEN;
            _server.Start();
            MACHINE_STATE = GPIO_STATE.MCU_WAIT;

            while (true)
            {
                // 비동기 Accept
                try
                {

                    Log.Info("[GPIO] AysncServer() : Waitting client");
                    TcpClient tc = await _server.AcceptTcpClientAsync().ConfigureAwait(false);
                    //Log.Info(tc.Connected + "connected?");
                    if (tc.Connected == true && _client == null)
                    {

                        Log.Info("[GPIO] AysncServer() : client connect!!");

                        mcudisconnect = false;

                        _client = tc;
                        _client.NoDelay = true;
                        _proc_state = INITIALIZE_READ_MODE;
                        Log.Info($"[GPIO] : AysncServer() _proc_state {proc_state_str()}");
                        //gpio_thread = new Thread(new ThreadStart(gpio_proc));
                        gpio_thread = new Thread(new ThreadStart(gpio_new_proc));
                        gpio_thread.Start();


                    }
                }
                catch
                {

                    Log.Info("[GPIO] AysncServer() : Server Close");

                    if (gpio_exit_event.WaitOne(100))
                    {
                        break;
                    }

                    break;
                }

                if (gpio_exit_event.WaitOne(100))
                {
                    break;
                }
                // 새 쓰레드에서 처리
                //Task.Factory.StartNew(AsyncTcpProcess, tc);
            }
        }

        public bool Init()
        {

            try
            {

                Log.Info("GPIO mcu device init");
                initstage.TYPE = MessageForm.MESSAGE_FORM_TYPE.MODELESS;
                if (_server == null)
                {
                    _server = new TcpListener(IPAddress.Any, PORT);
                    _server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    set_data_format();
                    read_timeout_timer.Interval = 5000; // can set timer for response 3000-12000 1000=1sec
                    read_timeout_timer.Tick += new EventHandler(read_timeout_timer_Tick);
                    // repeat_timeout_timer.Interval = 500;
                    // repeat_timeout_timer.Tick += new EventHandler(repeat_timeout_timer_Tick);
                    gpio_server_thread = new Thread(new ThreadStart(AysncServer));
                    gpio_server_thread.Start();
                    _is_open = true;
                }
                return true;

            }


            catch
            {
                return false;
            }
        }

        void read_timeout_timer_Tick(Object sender, System.EventArgs e)
        {
            if (OPEN)
            {
                respose_ack_nack(GPIO_OP_CODE.PACK_NACK);
                Log.Info($"Nack " + GPIO_OP_CODE.PACK_NACK);
            }
        }
        void repeat_timeout_timer_Tick(Object sender, System.EventArgs e)
        {
            if (OPEN)
            {
                retry_last_send();
            }
        }

        public void DeInit()
        {
            try
            {
                mcudisconnect = true;
                _is_open = false;

                if (_server != null)
                {
                    _server.Stop();
                    _server.Server.Close();
                    _server = null;
                }

                if (gpio_server_thread != null)
                {
                    //if (gpio_server_thread.Join(500) == false) //1000
                    //{
                    gpio_server_thread.Abort();
                    //}
                }
                //if (OPEN)
                //{
                if (IS_CONNECTED)
                {
                    _client.Dispose();
                    _client.Close();
                    gpio_exit_event.Set();
                    //write_complete_event.Set();
                }
                if (gpio_thread != null)
                {
                    //if (gpio_thread.Join(500) == false) //1000
                    //{
                    gpio_thread.Abort();
                    //}
                }
                //}
                SetOutClear();
                ForceSetOutToOff();
                if (instance != null)
                {
                    instance = null;
                }
            }
            catch (Exception ex)
            {
                Log.Info("[GPIO] DeInit() error : " + ex.Message);
            }
        }

        public bool IsPowerOff()
        {
            try
            {
                if (mcudisconnect)
                {
                    return true;
                }

                if (OPEN)
                {
                    GetIn();
                    //for (int i = 0; i < 12; i++)
                    {
                        if (IO_IN.Get(0))
                            return false;
                    }
                }
            }
            catch { }

            return true;
        }

        private bool _prev_errorState = false;
        //private MessageForm instance;

        public void SetErrorState(bool bState)
        {
            _errorState = bState;
            //Log.Info($"_errorState is : => {_errorState}");
            if (_prev_errorState != bState)
            {
                Log.Info("_errorState changed");
                _prev_errorState = _errorState;
                send_data_w(GPIO_OP_CODE.PACK_STOC_STATUS);
            }

        }

        public void SetOutClear()
        {

            try
            {
                if (OPEN && mcudisconnect == false)
                {
                    init_mode = 0;
                    _set_gpio_mode = SET_OUTPUT_CLEAR_PREPARE_MODE;
                    Log.Info($"[GPIO] : set_gpio_mode {set_gpio_mode_str()}");
                    //ClearIO();
                    //ForceSetOut();
                    //ForceSetOutToOff();
                }
                else { ClearOutIO(); }
            }
            catch { }
        }

        private int read_and_response(int size, string checkOpcode = "00")
        {
            size = read_data();
            if (size == 0)
            {
                Log.Info("[ERROR] Client Connected break");
                if (_client != null)
                {
                    _client.Close();
                    _client = null;
                }
                return size;
            }
            else if (size < 0)
            {
                //respose_ack_nack(GPIO_OP_CODE.PACK_NACK);
                return size;
            }
            if (checkOpcode == "") return size;
            if (!String.IsNullOrEmpty(checkOpcode) && _data.op_code == checkOpcode) // ||  _data.op_code == GPIO_OP_CODE.PACK_ACK.ToString("X2")
            {
                _data.retry_count = 0;
                if (checkOpcode == GPIO_OP_CODE.PACK_ACK.ToString("X2"))
                    return size;
            }
            else if (_data.op_code == GPIO_OP_CODE.PACK_NACK.ToString("X2"))
            {
                _data.retry_count++;
                return size;
            }
            respose_ack_nack(GPIO_OP_CODE.PACK_ACK);

            return size;
        }

        private void gpio_new_proc()
        {
            try
            {
                int size = 0;
                init_mode = 0;
                SetOutClear();
                MACHINE_STATE = GPIO_STATE.INIT;
                Log.Info("[GPIO] AyscServer() : gpio_proc Open - connected Client");

                while (true)
                {
                    //if (disconnect)
                    //{
                    //    send_data(GPIO_OP_CODE.PACK_STOC_AUTO_SEND_STATUS);
                    //    disconnect = false;
                    //}
                    //else
                    //{
                    if (init_mode == 0)
                    {
                        //send_data(GPIO_OP_CODE.PACK_INIT_CM);
                        //size = read_and_response(size, GPIO_OP_CODE.PACK_ACK.ToString("X2"));
                        //if (size == 0)
                        //{
                        //    return;
                        //}
                        //else if (size < 0)
                        //{
                        //    continue;
                        //}
                        //size = read_and_response(size, GPIO_OP_CODE.PACK_INIT_SERVER.ToString("X2"));
                        //if (size == 0)
                        //{
                        //    return;
                        //}
                        //else if (size < 0)
                        //{
                        //    continue;
                        //}
                        MACHINE_STATE = GPIO_STATE.INIT;
                        size = read_and_response(size, GPIO_OP_CODE.PACK_INIT_CM.ToString("X2"));
                        if (size == 0)
                        {
                            return;
                        }
                        else if (size < 0)
                        {
                            continue;
                        }
                        init_mode++;
                        //MACHINE_STATE = GPIO_STATE.SURVO_INITIALIZE_COMPLETE;
                        //auto_recv_data_mode = true;
                    }
                    if (init_mode == 1)
                    {
                        send_data(GPIO_OP_CODE.PACK_INIT_SERVER); //OP Code 02 -> sending response
                                                                  //repeat_timeout_timer.Start();

                        size = read_and_response(size, GPIO_OP_CODE.PACK_ACK.ToString("X2")); //OP Code 00
                        if (size == 0)
                        {
                            return;
                        }
                        else if (size < 0)
                        {
                            continue;
                        }
                        init_mode++;

                    }
                    if (init_mode == 2) //2023_06_01 changed
                    {

                        Log.Info($"init_mode = {init_mode}");
                        send_data(GPIO_OP_CODE.PACK_STOC_AUTO_SEND_STATUS);//OP Code 50 -> sending for checking mcu's autosend status

                        auto_recv_data_mode = true;
                        init_mode++;
                    }

                    //if (init_mode == 2) //2023_04_30
                    //{
                    //    GetIn();
                    //    GetOut();
                    //    send_data(GPIO_OP_CODE.PACK_STOC_OUPUT);//OP Code 40 -> sending response

                    //    size = read_and_response(size, GPIO_OP_CODE.PACK_ACK.ToString("X2")); //OP Code 00
                    //    if (size == 0)
                    //    {
                    //        return;
                    //    }
                    //    else if (size < 0)
                    //    {
                    //        continue;
                    //    }
                    //    size = read_and_response(size, GPIO_OP_CODE.PACK_CTOS_OUPUT.ToString("X2")); //OP Code 04
                    //    if (size == 0)
                    //    {
                    //        return;
                    //    }
                    //    else if (size < 0)
                    //    {
                    //        continue;
                    //    }
                    //    init_mode++;
                    //}
                    Log.Info($"init_mode = {init_mode}");
                    if (init_mode == 3) //2
                    {
                        // GetIn();
                        // GetOut();
                        send_data(GPIO_OP_CODE.PACK_STOC_STATUS); //OP Code 30 -> sending response
                                                                  //repeat_timeout_timer.Start();

                        //size = read_and_response(size, GPIO_OP_CODE.PACK_ACK.ToString("X2")); //OP Code 00
                        //if (size == 0)
                        //{
                        //    return;
                        //}
                        //else if (size < 0)
                        //{
                        //    continue;
                        //}
                        size = read_and_response(size, GPIO_OP_CODE.PACK_CTOS_STATUS.ToString("X2")); //OP Code 03
                        if (size == 0)
                        {
                            return;
                        }
                        else if (size < 0)
                        {
                            continue;
                        }
                        GetIn();
                        GetOut();
                        //checking for the 2nd bit in CM_state to be true so we are in error state
                        // if yes, we send commands to reset error state.
                        string cm_state = _data.data.Substring(0, 4);
                        Log.Info(cm_state);
                        bool error2 = (Convert.ToInt32(cm_state, 16) & (1 << 2)) != 0;
                        if (error2)
                        {
                            Log.Info("{MCU} Error state has detected");
                            _errorState = true;
                            send_data(GPIO_OP_CODE.PACK_STOC_STATUS);
                            //size = read_and_response(size, GPIO_OP_CODE.PACK_ACK.ToString("X2")); //OP Code 00
                            //if (size == 0)
                            //{
                            //    return;
                            //}
                            //else if (size < 0)
                            //{
                            //    continue;
                            //}
                            size = read_and_response(size, GPIO_OP_CODE.PACK_CTOS_STATUS.ToString("X2")); //OP Code 03
                            if (size == 0)
                            {
                                return;
                            }
                            else if (size < 0)
                            {
                                continue;
                            }
                            Thread.Sleep(100);
                            _errorState = false;
                            send_data(GPIO_OP_CODE.PACK_STOC_STATUS);
                            //size = read_and_response(size, GPIO_OP_CODE.PACK_ACK.ToString("X2")); //OP Code 00
                            //if (size == 0)
                            //{
                            //    return;
                            //}
                            //else if (size < 0)
                            //{
                            //    continue;
                            //}
                            size = read_and_response(size, GPIO_OP_CODE.PACK_CTOS_STATUS.ToString("X2")); //OP Code 03
                            if (size == 0)
                            {
                                return;
                            }
                            else if (size < 0)
                            {
                                continue;
                            }
                            Thread.Sleep(100);
                        }

                        MACHINE_STATE = GPIO_STATE.RUNNING;
                        ////checking for 0th bit in CM_state to be true to finish initializing.
                        //string cm_state = _data.data.Substring(0, 4);
                        //if ((Convert.ToInt32(cm_state, 16) & 1) == 1) // & 1=> 1st bit is true(==1) if finished.
                        //{
                        //    initializing_finished = true;
                        //    Log.Info("initializing has already been finished");
                        //}
                        init_mode++;

                    }
                    Log.Info($"init_mode = {init_mode}");
                    MACHINE_STATE = GPIO_STATE.RUNNING;

                    //if (init_mode == 3)
                    //{
                    //    send_data(GPIO_OP_CODE.PACK_STOC_AUTO_SEND_STATUS); //OP Code 50 -> sending response
                    //                                                        //repeat_timeout_timer.Start();

                    //    //size = read_and_response(size, GPIO_OP_CODE.PACK_ACK.ToString("X2")); //OP Code 00
                    //    //if (size == 0)
                    //    //{
                    //    //    return;
                    //    //}
                    //    //else if (size < 0)
                    //    //{
                    //    //    continue;
                    //    //}
                    //    init_mode++;
                    //    MACHINE_STATE = GPIO_STATE.RUNNING;
                    //    auto_recv_data_mode = true;
                    //}
                    break;
                    //}
                }

                //while (true)
                //{
                //    //TO DO need for rework


                //}
                // Log.Info($"initializing_finished in initstage = {initializing_finished}");
                //if (!initializing_finished)
                //{
                //    MessageFormShowEvent("Stage is initializing... please wait", initstage);
                //}

                while (true)
                {
                    //if (_errorState)
                    //{
                    //    send_data(30);
                    //    _client.Close();
                    //    ForceSetOutToOff();
                    //      //ChipCounterForm.work_state.ERROR;
                    //}
                    //if (mcudisconnect == true && initializing_finished == true)
                    //{
                    //    initializing_finished = false;
                    //}
                    //if (mcudisconnect == false && initializing_finished == false && IO_IN.Get(GPIO_DEF.IN_SYSTEM_START) == true)
                    //{
                    //    //Log.Info($"initializing_finished in initstage = {initializing_finished}");
                    //    //MessageFormShowEvent("Stage is initializing... please wait");
                    //    //if (!initstage.Visible)
                    //    //{
                    //    //    initstage.ButtonDisable();
                    //    //    initstage.MessageBody.Text = "Stage is initializing... please wait";
                    //    //    initstage.TopMost = true;
                    //    //    initstage.BringToFront();
                    //    //    initstage.Show();
                    //    //}
                    //    //if (!initstage.Visible)
                    //    //{
                    //    //    initstage.ButtonDisable();
                    //    //    initstage.MessageBody.Text = "Stage is initializing... please wait";
                    //    //    initstage.ShowDialog();
                    //    //    //initstage.Show((System.Windows.Forms.IWin32Window)this);
                    //    //    //initstage.TopMost = true;
                    //    //}

                    //    //send_data_w(30);
                    //    if (IsInitializingFinished())
                    //    {
                    //        Log.Info($"initializing_finished after finished = {initializing_finished}");
                    //        initializing_finished = true;
                    //        //initstage.Hide(); // Close();
                    //        MessageFormShowEvent("");
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        send_data_w(GPIO_OP_CODE.PACK_STOC_STATUS);
                    //    }
                    //}
                    if (_client == null || _client.Connected == false)
                        break;
                    Log.Info($"[DEBUG] wait  : {MACHINE_STATE}");
                    size = read_data();
                    lock (cyj_network_lock)
                    {
                        if (size == 0)
                        {
                            Log.Info("[ERROR] Client Connected break");
                            if (_client != null)
                            {
                                _client.Close();
                                _client = null;
                            }
                            break;
                        }
                        else if (size < 0)
                        {
                            //respose_ack_nack(GPIO_OP_CODE.PACK_NACK);
                            _data.retry_count++;
                            continue;
                        }

                        Log.Info($"[DEBUG] received : {MACHINE_STATE}");
                        if (_data.op_code == GPIO_OP_CODE.PACK_ACK.ToString("X2"))
                        {
                            _data.retry_count = 0;
                            continue;
                        }
                        else if (_data.op_code == GPIO_OP_CODE.PACK_NACK.ToString("X2"))
                        {
                            _data.retry_count++;
                            if (last_send_opcode != 0)
                            {
                                send_data_w(last_send_opcode);
                            }
                            continue;
                        }
                        else if (_data.op_code == GPIO_OP_CODE.PACK_CTOS_STATUS.ToString("X2"))
                        {
                            //Log.Info("Get 03x, Set waitNeededPacket to true"); // false => true
                            waitNeededPacket = true; // false => true
                        }
                        else if (_data.op_code == GPIO_OP_CODE.PACK_CTOS_OUPUT.ToString("X2"))
                        {
                            EDIT_MODE = false;
                            //Log.Info("Get 04x, Set waitNeededPacket to true");
                            if (OPEN)
                            {
                                GetIn();
                                GetOut();
                            }
                            waitNeededPacket = true; // false => true
                        }
                        else if (_data.op_code == GPIO_OP_CODE.PACK_CTOS_AUTO_SEND_STATUS.ToString("X2"))
                        {
                            //Log.Info("Get 05x, Set waitNeededPacket to true");
                            waitNeededPacket = true;
                            if (OPEN)
                            {
                                GetIn();
                                GetOut();
                            }
                            //auto_recv_data_mode = false; not needed
                            //System power button is off
                            string in_state = _data.input_state.Substring(0, 4);
                            string cm_state = _data.data.Substring(0, 4);
                            if ((Convert.ToInt32(in_state, 16) & 1) == 0 && (Convert.ToInt32(cm_state, 16) == 0x04))
                            {
                                //initializing_finished = false;
                                is_send = false;
                                initstage.ButtonEnable();
                                failedinitialize = true;
                                Log.Info($"Emergency pressed. initializing_finished ");

                            }
                        }
                        // dont send ack , nack 
                        //respose_ack_nack(GPIO_OP_CODE.PACK_ACK);
                        if (_data.op_code == GPIO_OP_CODE.PACK_CTOS_AUTO_SEND_STATUS.ToString("X2")
                            || _data.op_code == GPIO_OP_CODE.PACK_CTOS_STATUS.ToString("X2")
                            || _data.op_code == GPIO_OP_CODE.PACK_CTOS_OUPUT.ToString("X2")
                            )
                        {
                            //Log.Info($"Get {_data.op_code}, Set waitNeededPacket to false");
                            waitNeededPacket = false;



                            //if (IO_IN.Get(GPIO_DEF.IN_EMERGENCY) == false) 
                            //{
                            //    initializing_finished = false;
                            //    initstage.ButtonEnable();
                            //    is_send = false;
                            //    failedinitialize = true;
                            //    Log.Info($"Emergency pressed. initializing_finished = {initializing_finished}");
                            //}
                            //if (mcudisconnect == true) 
                            //{
                            //    initializing_finished = false;
                            //    is_send = false;
                            //    initstage.ButtonEnable();
                            //    failedinitialize = true;
                            //    Log.Info($"MCU Disconnected. initializing_finished = {initializing_finished}");
                            //}

                            string in_state = _data.input_state.Substring(0, 4);
                            string cm_state = _data.data.Substring(0, 4);
                            //int a = Convert.ToInt32(in_state, 16);
                            // User pressed [Start System] button
                            if ((MACHINE_STATE != GPIO_STATE.STAGE_INITIALIZE && MACHINE_STATE != GPIO_STATE.STAGE_INITIALIZING) &&
                                ((Convert.ToInt32(in_state, 16) & 1) == 1 && Convert.ToInt32(cm_state, 16) == 0))
                            {
                                //TcpMCUBoardDevice tcpMCUBoardDevice = this;
                                //MessageFormShowEvent("Pressed [Start System] button. Stage is initializing...", initstage);


                                //MessageFormShowEvent("Press confirm button to start initializing", initstage);

                                MACHINE_STATE = GPIO_STATE.STAGE_INITIALIZE;

                                //initstage.okFunc = ()=> {
                                //    Log.Info("Confirm Clicked");
                                //    initstage.okFunc = null;
                                //    initstage.ButtonDisable();
                                //    if (!initializing_started)
                                //    {
                                //        initializing_started = true;
                                //        //is_send = true;
                                //        send_data(GPIO_OP_CODE.PACK_STOC_OUPUT, true);
                                //    }
                                //    if (_errorState)
                                //    {
                                //        send_data_w(GPIO_OP_CODE.PACK_STOC_STATUS);
                                //    }
                                //    else
                                //    {

                                //    }
                                //} ;

                            }

                            if (MACHINE_STATE == GPIO_STATE.STAGE_INITIALIZING)
                            {
                                if ((Convert.ToInt32(cm_state, 16) & 1) == 1) // & 1=> 1st bit is true(==1) if finished.
                                {
                                    MACHINE_STATE = GPIO_STATE.RUNNING;
                                    //initializing_finished = true;
                                    //initializing_started = false;
                                    //MessageFormShowEvent("", initstage);
                                    //is_send = false;
                                }
                                //else
                                //{

                                //    _lastOpCodeToSend = GPIO_OP_CODE.PACK_STOC_STATUS;
                                //    Thread.Sleep(1000);
                                //}
                            }
                            if (_data.op_code == GPIO_OP_CODE.PACK_CTOS_STATUS.ToString("X2") && ((Convert.ToInt32(cm_state, 16) & 4) == 4))
                            {
                                // CheckStatus();
                            }


                            if (_lastOpCodeToSend != 0)
                            {
                                Log.Info($"after respose_ack_nack, send _lastOpCodeToSend {_lastOpCodeToSend:X2}");
                                send_data(_lastOpCodeToSend);
                                _lastOpCodeToSend = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("[GPIO] AysncServer() : client Disconnect!!");
                //Log.Info($"[GPIO] AysncServer() : client has errors!!{e}");

                //disconnect = true;
                mcudisconnect = true;
                ForceSetOutToOff();
                is_send = false;

                set_data_format();
                if (_client != null)
                {
                    _client.Close();
                    _client = null;
                }
                auto_recv_data_mode = false;
                MACHINE_STATE = GPIO_STATE.MCU_WAIT;
            }
        }
       
        private string set_gpio_mode_str()
        {
            switch (_set_gpio_mode)
            {
                case NORMAL_MODE: return $"NORMAL_MODE {_set_gpio_mode}"; // 30 OP code
                //private const byte SET_OUTPU_INDEX_INIT_MODE = 0x01;
                case SET_OUTPU_INDEX_PREPARE_MODE: return $"SET_OUTPU_INDEX_PREPARE_MODE {_set_gpio_mode}"; //OP code 40

                case SET_OUTPU_INDEX_READY_MODE: return $"SET_OUTPU_INDEX_READY_MODE {_set_gpio_mode}"; //OP Code 40 success

                //private const byte SET_OUTPUT_CLEAR_INIT_MODE = 0x04;
                case SET_OUTPUT_CLEAR_PREPARE_MODE: return $"SET_OUTPUT_CLEAR_PREPARE_MODE {_set_gpio_mode}"; //OP code 40
                case SET_OUTPUT_CLEAR_READY_MODE: return $"SET_OUTPUT_CLEAR_READY_MODE {_set_gpio_mode}"; //OP code 40  -> output signal: false

                case SET_AUTO_RECV_STATUS_MODE: return $"SET_AUTO_RECV_STATUS_MODE { _set_gpio_mode}"; //OP code 50 
                case SET_AUTO_RECV_STATUS_STOP_MODE: return $"SET_AUTO_RECV_STATUS_STOP_MODE {_set_gpio_mode}"; //OP code 60
                default:
                    return $"{_set_gpio_mode}";
            }
        }

        private string proc_state_str()
        {
            switch (_proc_state)
            {
                case WRITE_MODE: return "WRITE_MODE";
                case READ_MODE: return "READ_MODE";
                case SET_GPIO_MODE: return "SET_GPIO_MODE";
                case ERROR_MODE: return "ERROR_MODE";
                case INITIALIZE_READ_MODE: return "INITIALIZE_READ_MODE";
                case INITIALIZE_WRITE_MODE: return "INITIALIZE_WRITE_MODE";
            }

            return $"UNKNOWN_{_proc_state}";
        }
        private string output_index_str(int index)
        {
            switch (index)
            {
                case 0: return "X-Ray Lamp";
                case 2: return "Start lamp";
                case 3: return "Return lamp";
                case 4: return "Motion Stage In";
                case 5: return "Motion Stage Out";
                default: return $"Unknown {index}";
            }

            return $"UNKNOWN_{index}";
        }
        #region unused code maybe useful in future
        private void TMC_OutputStatus()
        {
            try
            {
                //lock (gpio_device_lock)
                //{
                //    if (IO_OUT_CNT <= 16)
                //    {
                //        ushort out_status = 0;
                //        if (TMCAEDLL.AIO_GetDOWord(_card_no, 1, ref out_status) == 0)
                //        {
                //            for (int i = 0; i < IO_OUT_CNT; i++)
                //            {
                //                if (((out_status >> i) & 0x00001) == 0x0001)
                //                    _io_out.Set(i, true);
                //                else
                //                    _io_out.Set(i, false);
                //            }
                //        }
                //    }
                //    else if (IO_OUT_CNT <= 32)
                //    {
                //        uint out_status = 0;
                //        if (TMCAEDLL.AIO_GetDODWord(_card_no, 1, ref out_status) == 0)
                //        {
                //            for (int i = 0; i < IO_OUT_CNT; i++)
                //            {
                //                if (((out_status >> i) & 0x00001) == 0x0001)
                //                    _io_out.Set(i, true);
                //                else
                //                    _io_out.Set(i, false);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex) { }
        }

        private void TMC_InputStatus()
        {
            try
            {
                //lock (gpio_device_lock)
                //{
                //    if (IO_IN_CNT <= 16)
                //    {
                //        ushort in_status = 0;
                //        if (TMCAEDLL.AIO_GetDIBit(_card_no, 1, ref in_status) == 0)
                //        {
                //            for (int i = 0; i < IO_OUT_CNT; i++)
                //            {
                //                if (((in_status >> i) & 0x00001) == 0x0001)
                //                    _io_in.Set(i, true);
                //                else
                //                    _io_in.Set(i, false);
                //            }
                //        }
                //    }
                //    else if (IO_IN_CNT <= 32)
                //    {
                //        uint in_status = 0;
                //        if (TMCAEDLL.AIO_GetDIDWord(_card_no, 1, ref in_status) == 0)
                //        {
                //            for (int i = 0; i < IO_OUT_CNT; i++)
                //            {
                //                if (((in_status >> i) & 0x00001) == 0x0001)
                //                    _io_in.Set(i, true);
                //                else
                //                    _io_in.Set(i, false);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex) { }
        }
        #endregion
        public void SetOutToggle(int index, bool on_off = true)
        {
            try
            {
                //lock (gpio_device_lock)
                //{
                //    if (on_off)
                //    {
                //        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_ON);
                //        Thread.Sleep(200);
                //        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_OFF);
                //    }
                //    else
                //    {
                //        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_OFF);
                //        Thread.Sleep(200);
                //        TMCAEDLL.AIO_PutDOBit(_card_no, (ushort)index, tmcDef.CMD_ON);
                //    }
                //}
            }
            catch (Exception ex)
            {
                LastError = "[GPIO] [ERROR] SetOutToggle : " + ex.Message;
            }
        }

        private void ForceSetOutToOff()
        {
            try
            {
                Log.Info("[GPIO] ForceSetOutToOff");

                lock (gpio_device_lock)
                {
                    _data.input_state = "0000";
                    _data.cm_state = "0000";
                    _data.output_state = "0000";
                    for (int i = 0; i < _io_in.Length; i++)
                    {
                        _io_in.Set(i, false);
                    }

                    for (int i = 0; i < _edit_io_out.Length; i++)
                    {
                        _edit_io_out.Set(i, false);
                    }

                    for (int i = 0; i < _io_out.Length; i++)
                    {
                        _io_out.Set(i, false);
                    }

                    //required for MCU to set this to true because MCU has this IO reversed compared with us.
                    _io_in.Set(10, true);
                }

                init_mode = 0;
                MACHINE_STATE = -1;

                if (gpio_thread != null)
                    gpio_thread.Abort();
            }
            catch (Exception ex)
            {
                //LastError = "TMC() : SetOut : " + ex.Message;
            }
        }
        private void ClearOutIO()
        {
            if (OPEN)
            {
                for (int i = 0; i < _edit_io_out.Length; i++)
                {
                    if (i == 1)
                        continue;
                    _edit_io_out.Set(i, false);
                    _io_out.Set(i, false);
                }
            }
        }

        private void clear_edit_io()
        {
            for (int i = 0; i < _edit_io_out.Length; i++)
            {
                if (i == 1)
                    continue;
                _edit_io_out.Set(i, false);
            }
        }

        private void ForceSetOut()
        {
            try
            {
                //lock (gpio_device_lock)
                {

                    if (OPEN)
                    {
                        for (int i = 0; i < _edit_io_out.Length; i++)
                        {
                            if (i == 1)
                                continue;
                            _edit_io_out.Set(i, false);
                            _io_out.Set(i, false);
                        }
                    }
                }

                if (auto_recv_data_mode == false)
                {
                    _set_gpio_mode = SET_OUTPUT_CLEAR_PREPARE_MODE;
                    Log.Info($"[GPIO] : _set_gpio_mode {set_gpio_mode_str()}");
                }
                else
                {
                    //int timeout = 1000;
                    _set_gpio_mode = SET_OUTPUT_CLEAR_PREPARE_MODE;
                    Log.Info($"[GPIO] : _set_gpio_mode {set_gpio_mode_str()}");
                    ///Log.Info($"[GPIO] : ForceSetOut");
                    send_data_w(GPIO_OP_CODE.PACK_STOC_OUPUT);

                    //if (_proc_state != READ_MODE)
                    //    timeout = 3000;

                    //if (write_complete_event.WaitOne(timeout) == false)
                    //{
                    //    Log.Info("[FAILED][GPIO]Set Out status Failed");
                    //}
                    //else
                    //    write_complete_event.Reset();
                }

            }
            catch (Exception ex)
            {
                LastError = "TMC() : SetOut : " + ex.Message;
            }
        }

        public void SetInit()
        {
            send_data_w(GPIO_OP_CODE.PACK_STOC_OUPUT, true);
            MACHINE_STATE = GPIO_STATE.STAGE_INITIALIZING;
        }

        public void SetOut(List<int> indexs, bool on_off)
        {
            try
            {
                if (!DEBUGMODE.GetMode())
                {
                    bool is_need_send = false;
                    foreach (int index in indexs)
                    {
                        if (index == GPIO_DEF.OUT_RETURN_LAMP ||
                                    index == GPIO_DEF.OUT_START_LAMP ||
                                    index == GPIO_DEF.OUT_TOWER_YELLOW)
                        {
                            if (_io_out.Get(index) != on_off)
                            {
                                is_need_send = true;

                                _io_out.Set(index, on_off);
                                Log.Info($"START or RETURN lamp, Yellow ctl. :{on_off.ToString()} .. : {index}");
                            }
                        }
                    }

                    if (is_need_send)
                        send_data_w(GPIO_OP_CODE.PACK_STOC_STATUS);
                }
            }
            catch (Exception ex) { }

        }

        public void SetOut(int index, bool on_off)
        {
            // lock (setout_lock)
            {
                if (!DEBUGMODE.GetMode())
                {
                    try
                    {

                        //lock (gpio_device_lock)
                        {
                            //ignoring lamp on-off command send
                            if (index == GPIO_DEF.OUT_RETURN_LAMP ||
                                index == GPIO_DEF.OUT_START_LAMP ||
                                index == GPIO_DEF.OUT_TOWER_YELLOW)
                            {
                                // 리턴과 스타트 버튼 램프, 황색 램프 _io_out 에 바로 제어
                                if (_io_out.Get(index) != on_off)
                                {
                                    _io_out.Set(index, on_off);
                                    Log.Info($"START or RETURN lamp, Yellow ctl. :{on_off.ToString()} .. : {index}");
                                    send_data_w(GPIO_OP_CODE.PACK_STOC_STATUS);
                                }
                                return;
                                //if (_io_out.Get(GPIO_DEF.OUT_RETURN_LAMP) == false || _io_out.Get(GPIO_DEF.OUT_START_LAMP) == false)
                                //{
                                //    Log.Info("Ignore lamps to ON");
                                //    return;
                                //}
                                //else if (_io_out.Get(GPIO_DEF.OUT_RETURN_LAMP) == true || _io_out.Get(GPIO_DEF.OUT_START_LAMP) == true)
                                //{
                                //    Log.Info("Ignore lamps to OFF");
                                //    return;
                                //}

                            }

                            // 타워등 모두 제어 무시.
                            if (index == GPIO_DEF.OUT_TOWER_YELLOW || index == GPIO_DEF.OUT_TOWER_GREEN ||
                                index == GPIO_DEF.OUT_TOWER_RED)
                            {
                                if (index == GPIO_DEF.OUT_TOWER_YELLOW)
                                {
                                    bool old_on_off = _io_out.Get(index);

                                    _io_out.Set(index, on_off);

                                    if (old_on_off != on_off)
                                    {
                                        Log.Info("YELLOW lamp " + on_off.ToString());
                                        send_data_w(GPIO_OP_CODE.PACK_STOC_STATUS);
                                    }
                                }
                            }

                            // 녹색, 노랑과 빨강 일때 off 일 경우에는 무시한다.
                            if (index == GPIO_DEF.OUT_TOWER_YELLOW || index == GPIO_DEF.OUT_TOWER_GREEN ||
                                index == GPIO_DEF.OUT_TOWER_RED)
                            {
                                //Log.Info("[GPIO] Ignore tower lamp command.");
                                return;
                            }

                            // XRAY GPIO 명령 무시.
                            if (index == GPIO_DEF.OUT_XRAY_ON)
                            {
                                string state = on_off ? "ON" : "OFF";
                                //Log.Info("[GPIO] Ignore xray command.");
                                return;
                            }

                            // STAGE IN, STAGE OUT 의 off는 무시.
                            //if ((index == GPIO_DEF.OUT_STAGE_IN || index == GPIO_DEF.OUT_STAGE_OUT) && on_off == false )
                            //{
                            //    Log.Info("[GPIO] Ignore Stage In,Out off command.");
                            //    return;
                            //}

                            //Log.Info($"[GPIO] SetOut : index = {index} on_off = {on_off} ");

                            if (_edit_io_out.Get(index) != on_off)
                                _edit_io_out.Set(index, on_off);

                            //Log.Info($"[GPIO] SetOut value setted(editor) : index = {index} on_off = {on_off} ");


                            if (_io_out.Get(index) == on_off || index > 5)
                            {
                                //Log.Info($"[GPIO] Setout : Ignore command when value is same. : index = {index} on_off = {on_off} ");
                                return;
                            }

                        }

                        // 곧 변경 예정..
                        if (auto_recv_data_mode == false)
                        {
                            _set_gpio_mode = SET_OUTPU_INDEX_PREPARE_MODE;
                            Log.Info($"[GPIO] : set_gpio_mode {set_gpio_mode_str()}");
                            //auto_recv_data_mode = true;
                        }
                        else
                        {
                            //int timeout = 1000;
                            //_set_gpio_mode = SET_OUTPU_INDEX_PREPARE_MODE;

                            //Log.Info($"[GPIO] SetOut value used: index = {index} on_off = {on_off} ");

                            // Instead of direct send_data, put OP in queue
                            //_queueOP.Enqueue(GPIO_OP_CODE.PACK_STOC_OUPUT); //2023/03/27 not working
                            string state = on_off ? "ON" : "OFF";
                            Log.Info($"[GPIO] : SetOut {output_index_str(index)} {state}, index: {index} Qsz: {_queueOP.Count()}");

                            send_data_w(GPIO_OP_CODE.PACK_STOC_OUPUT);

                            EDIT_MODE = true;

                        }
                    }
                    catch (Exception ex)
                    {
                        LastError = "[GPIO] [ERROR] SetOut : " + ex.Message;
                    }

                }
            }
        }

        // Bit checking: State string to output string of 1
        public string Describe(string state_str, string description_str)
        {
            string output = description_str; //"IN: "
            try
            {
                //lock (gpio_device_lock)
                //{
                //ushort in_status = (ushort)(buff[0] | (buff[1] << 8)); ;
                ushort in_status = 0;
                if (state_str != null)
                    in_status = ushort.Parse(state_str, System.Globalization.NumberStyles.HexNumber);

                for (int i = 0; i < 16; i++)
                {
                    if (((in_status >> i) & 0x00001) == 0x0001)
                        output += " " + i.ToString();
                    //else
                    //    _io_in.Set(i, false);
                }
                //}
                //Log.Info("GetIn");
            }
            catch (Exception ex)
            {
                output += "Exception: " + ex.Message;
            }

            return output;
        }


        public bool GetIn()
        {
            LastError = "GetOut() : unknown error()";

            try
            {
                //byte[] buff = Encoding.ASCII.GetBytes(_data.input_state);

                lock (gpio_device_lock)
                {
                    //ushort in_status = (ushort)(buff[0] | (buff[1] << 8)); ;
                    ushort in_status = 0;

                    if (_data.input_state != null)
                        in_status = ushort.Parse(_data.input_state, System.Globalization.NumberStyles.HexNumber);
                    for (int i = 0; i < 16; i++)
                    {
                        if (((in_status >> i) & 0x00001) == 0x0001)
                            _io_in.Set(i, true);
                        else
                            _io_in.Set(i, false);
                    }
                }
                //Log.Info("GetIn");

                return true;
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
                //byte[] buff = Encoding.ASCII.GetBytes(_data.input_state);

                lock (gpio_device_lock)
                {
                    //ushort in_status = (ushort)(buff[0] | (buff[1] << 8)); ;
                    ushort in_status = 0;

                    if (_data.input_state != null)
                        in_status = ushort.Parse(_data.input_state, System.Globalization.NumberStyles.HexNumber);

                    for (int i = 0; i < 16; i++)
                    {
                        if (((in_status >> i) & 0x00001) == 0x0001)
                            _io_in.Set(i, true);
                        else
                            _io_in.Set(i, false);
                    }

                }
                return _io_in.Get(index);
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
            if (!DEBUGMODE.GetMode())
            {
                try
                {
                    //byte[] buff = Encoding.ASCII.GetBytes(_data.output_state);


                    lock (gpio_device_lock)
                    {
                        //ushort out_status = (ushort)(buff[0] | (buff[1] << 8)); ;
                        ushort out_status = 0;

                        if (_data.output_state != null)
                            out_status = ushort.Parse(_data.output_state, System.Globalization.NumberStyles.HexNumber);

                        // MCU에서 수신되는 OUT 쪽 플래그는 데이터는 모두 무시한다.
                        /*
                        for (int i = 0; i < 16; i++)
                        {
                            // Return 버튼과 Start 버튼 OUT 쪽은 mcu 에서 수신되는 데이터는 무시하자.
                            if (i != GPIO_DEF.OUT_RETURN_LAMP && i != GPIO_DEF.OUT_START_LAMP)
                            {
                                if (((out_status >> i) & 0x00001) == 0x0001)
                                    _io_out.Set(i, true);
                                else
                                    _io_out.Set(i, false);
                            }
                        }
                        */
                    }
                    return _io_out.Get(index);

                }
                catch (Exception ex)
                {
                    LastError = "TMC() : GetOut : " + ex.Message;
                }
            }

            return false;
        }

        public bool GetOut()
        {
            if (!DEBUGMODE.GetMode())
            {
                LastError = "GetOut() : unknown error()";
                try
                {
                    //byte[] buff = Encoding.ASCII.GetBytes(_data.output_state);

                    lock (gpio_device_lock)
                    {
                        //ushort out_status = (ushort)(buff[0] | (buff[1] << 8)); ;
                        ushort out_status = 0;

                        if (_data.output_state != null)
                            out_status = ushort.Parse(_data.output_state, System.Globalization.NumberStyles.HexNumber);

                        // MCU에서 수신되는 OUT 쪽 플래그는 데이터는 모두 무시한다.
                        /*
                        for (int i = 0; i < 16; i++)
                        {
                            // Return 버튼과 Start 버튼 OUT 쪽은 mcu 에서 수신되는 데이터는 무시하자.
                            if (i != GPIO_DEF.OUT_RETURN_LAMP && i != GPIO_DEF.OUT_START_LAMP)
                            {
                                if (((out_status >> i) & 0x00001) == 0x0001)
                                    _io_out.Set(i, true);
                                else
                                    _io_out.Set(i, false);
                            }
                        }
                        */
                    }
                    //Log.Info("GetOut");
                    return true;

                }
                catch (Exception ex)
                {
                    LastError = "TMC() : GetOut : " + ex.Message;
                }

            }
            return false;
        }


        public int CalcCheckSum(byte[] command, int length)
        {
            int checksum = 0;
            for (int i = 0; i < length; ++i)
                checksum += command[i];

            return checksum;
        }


        #region Packit Processing
        private bool split_packet(byte[] buff, int size, bool initialize = false)
        {
            int pos = 0;
            int check_sum = 0;
            string data = Encoding.ASCII.GetString(buff);
            data = data.Substring(0, size);

            if (data.Length <= 0)
            {
                Log.Info("[MCU] read_data: [ERROR] Received 0 bytes");
                return false;
            }

            if (data.Length % 2 != 0)
            {
                Log.Info($"[MCU] read_data: [ERROR] Received {data.Length} bytes");
                return false;
            }



            if (data.IndexOf(_data.stx) == 0 && data.IndexOf(_data.etx) == (data.Length - 1))
            {
                //for (int i = 0; i < size - 5; i++)
                //{
                //    check_sum += buff[i];
                //}
                check_sum = CalcCheckSum(buff, size - 5);


                char[] del_word = new char[] { '[', ']' };
                string split_data = data.Trim(del_word);
                _data.packet_num = int.Parse(split_data.Substring(pos, 2), System.Globalization.NumberStyles.HexNumber);
                pos += 2;

                _data.retry_count = int.Parse(split_data.Substring(pos, 2), System.Globalization.NumberStyles.HexNumber); ;
                pos += 2;

                _data.op_code = split_data.Substring(pos, 2);
                pos += 2;

                _data.length = int.Parse(split_data.Substring(pos, 2), System.Globalization.NumberStyles.HexNumber); ;
                pos += 2;

                _data.data = split_data.Substring(pos, _data.length);
                pos += _data.length;

                string last_4_check = split_data.Substring(split_data.Length - 4/*pos*/, 4);
                string calc_check = check_sum.ToString("X4");
                //if (split_data.Substring(pos, 4).Equals(check_sum.ToString("X4")) == false)
                //    return false;

                // TODO: Comment later
                if (string.Equals(last_4_check, calc_check, StringComparison.OrdinalIgnoreCase) == false)
                    Log.Info($"[MCU] read_data: Last4: {last_4_check}, Calc: {calc_check} NOT MATCHED");
                //else
                //Log.Info($"[MCU] read_data: Last4: {last_4_check}, Calc: {calc_check} ");

                if (initialize)
                {
                    pos = 0;
                    for (int cnt = 0; cnt < _data.length / 2; cnt++)
                    {
                        switch (cnt)
                        {
                            case 0:
                                HW_VER = _data.data.Substring(pos, 2);
                                pos += 2;
                                break;
                            case 1:
                                FW_VER = _data.data.Substring(pos, 2);
                                pos += 2;
                                break;
                            case 2:
                                PROTOCAL_VER = _data.data.Substring(pos, 2);
                                pos += 2;
                                break;
                        }
                    }
                }
                else
                {
                    if (!_data.op_code.Equals(GPIO_OP_CODE.PACK_ACK.ToString("X2"))
                        && !_data.op_code.Equals(GPIO_OP_CODE.PACK_NACK.ToString("X2")))
                    {
                        pos = 0;
                        string state_cm = "";
                        string state_input = "";
                        string state_output = "";

                        for (int cnt = 0; cnt < _data.length / 4; cnt++)
                        {
                            switch (cnt)
                            {
                                case 0:
                                    _data.cm_state = _data.data.Substring(pos, 4);
                                    state_cm = Describe(_data.cm_state, "CM:");
                                    pos += 4;
                                    break;
                                case 1:
                                    _data.input_state = _data.data.Substring(pos, 4);
                                    state_input = Describe(_data.input_state, "IN:");
                                    pos += 4;
                                    break;
                                case 2:
                                    _data.output_state = _data.data.Substring(pos, 4);
                                    state_output = Describe(_data.output_state, "OUT:");
                                    pos += 4;
                                    break;
                            }
                            state_str = state_cm + ", " + state_input + ", " + state_output;
                        }
                    }
                    else
                    {
                        pos = 0;
                        string state_cm = "";
                        string state_input = "";
                        string state_output = "";

                        for (int cnt = 0; cnt < _data.length / 4; cnt++)
                        {
                            switch (cnt)
                            {
                                case 0:
                                    _data.cm_state = _data.data.Substring(pos, 4);
                                    state_cm = Describe(_data.cm_state, "CM:");
                                    pos += 4;
                                    break;
                                case 1:
                                    _data.input_state = _data.data.Substring(pos, 4);
                                    state_input = Describe(_data.input_state, "IN:");
                                    pos += 4;
                                    break;
                                case 2:
                                    _data.output_state = _data.data.Substring(pos, 4);
                                    state_output = Describe(_data.output_state, "OUT:");
                                    pos += 4;
                                    break;
                            }
                            state_str = state_cm + ", " + state_input + ", " + state_output;
                        }
                    }
                }
                // For testing could be useful
                //if (state_str == state_old)
                //{
                //    Log.Info($"'{state_str}' is equal to '{state_old}' state is the same");
                //}
                //else
                //{
                //    Log.Info($"[MCU] read_data: {data} #:{_data.packet_num} retry:{_data.retry_count} op:{_data.op_code} len:{_data.length} {state_str}");
                //    state_old = state_str;
                //}

                // TODO: Comment later
                Log.Info($"[MCU] read_dataall: {data} #:{_data.packet_num} retry:{_data.retry_count} op:{_data.op_code} len:{_data.length} {state_str}");

            }
            else
            {
                Log.Info($"[MCU] read_data: [ERROR] STX: {data.IndexOf(_data.stx)}, ETX: {data.IndexOf(_data.etx)}");
                return false;
            }

            _response_data.packet_num = _data.packet_num;
            _response_data.retry_count = _data.retry_count;
            _response_data.data = _data.op_code + _data.packet_num.ToString("X2");

            return true;
        }

        private string make_packet(byte op_code, bool init = false)
        {
            string packet = _send_data.stx;
            if (_data.packet_num == 255)
            {
                _data.packet_num = 0;
            }
            else
            {
                _data.packet_num = _data.packet_num + 1;
            }

            _send_data.op_code = op_code.ToString("X2");
            _send_data.data = set_data(ref _send_data, op_code, init);

            packet += _data.packet_num.ToString("X2");
            _send_data.packet_num = _data.packet_num;

            // Log.Info(packet+"packet number");
            packet += _data.retry_count.ToString("X2");
            _send_data.retry_count = _data.retry_count;
            // Log.Info(packet+"retry_count");
            packet += _send_data.op_code;
            // Log.Info(packet+"opcode");
            packet += _send_data.length.ToString("X2");
            // Log.Info(packet+"length");
            packet += _send_data.data;
            //  Log.Info(packet+$"data=> {_data.data} cm:{_data.cm_state} in:{_data.input_state} out:{_data.output_state}");
            packet += sum_checksum(_send_data);
            //  Log.Info(packet+"checksum");
            packet += _send_data.etx;
            //  Log.Info(packet+"etx");
            last_send_opcode = op_code;

            //  Log.Info($"packet is ready and is: {packet}");
            return packet;
        }

        //사용할 필요가 없지만 일단 남겨 둠
        //private byte set_op_code(string op_code)
        //{
        //    byte res = 0x00;
        //    byte byte_op_code = Convert.ToByte(op_code);

        //    switch (byte_op_code)
        //    {
        //        case GPIO_OP_CODE.PACK_ACK:
        //            res = GPIO_OP_CODE.PACK_ACK;
        //            break;
        //        case GPIO_OP_CODE.PACK_NACK:
        //            res = GPIO_OP_CODE.PACK_NACK;
        //            break;
        //        case GPIO_OP_CODE.PACK_INIT_SEVER:
        //            res = GPIO_OP_CODE.PACK_INIT_CM;
        //            break;
        //        case GPIO_OP_CODE.PACK_CTOS_STATUS:
        //            res = GPIO_OP_CODE.PACK_STOC_STATUS;
        //            break;
        //        case GPIO_OP_CODE.PACK_CTOS_OUPUT:
        //            res = GPIO_OP_CODE.PACK_STOC_OUPUT;
        //            break;
        //        case GPIO_OP_CODE.PACK_CTOS_AUTO_SEND_STATUS:
        //            res = GPIO_OP_CODE.PACK_STOC_AUTO_SEND_STATUS;
        //            break;
        //        default:
        //            break;
        //    }

        //    return res;
        //}
        private string stage_init_send(int machine_state)
        {
            string out_state = _data.output_state;

            string cur_state_cm = Describe(_data.cm_state, " ");
            string cur_state_input = Describe(_data.input_state, " ");
            string cur_state_output = Describe(_data.output_state, " ");

            string currentstate = "CM: " + cur_state_cm + "IN: " + cur_state_input + "OUT: " + cur_state_output;
            Log.Info($"Currentstate is: {currentstate}");

            out_state = "0040";

            cur_state_output = Describe(out_state, "OUT:");
            string new_state = "CM: " + cur_state_cm + "IN: " + cur_state_input + "OUT: " + cur_state_output;
            Log.Info($"New state is: {new_state}");

            return out_state;

        }
        private string error_state_send(int machine_state)
        {
            string cm_state = _data.cm_state;

            string cur_state_cm = Describe(_data.cm_state, "CM:");
            string cur_state_input = Describe(_data.input_state, "IN:");
            string cur_state_output = Describe(_data.output_state, "OUT:");
            if (lastCheckStatus)
                _errorState = true;
            if (_errorState)
            {
                string currentstate = cur_state_cm + cur_state_input + cur_state_output;
                Log.Info($"Currentstate is: {currentstate}");

                cm_state = "0002";

                cur_state_cm = Describe(cm_state, "CM:");
                string new_state = cur_state_cm + cur_state_input + cur_state_output;
                Log.Info($"New state is: {new_state}");
                _errorState = false;
            }
            else
            {
                string currentstate = cur_state_cm + cur_state_input + cur_state_output;
                Log.Info($"Currentstate is: {currentstate}");

                cm_state = "0000";

                cur_state_cm = Describe(cm_state, "CM:");
                string new_state = cur_state_cm + cur_state_input + cur_state_output;
                Log.Info($"New state is: {new_state}");
            }
            Log.Info(cm_state);

            return cm_state;
        }
        private string set_data(ref DATAS setData, byte op_code, bool init = false)
        {
            string res = "";
            short high_bit = 0;

            switch (op_code)
            {
                case GPIO_OP_CODE.PACK_ACK:
                    setData.length = 4;
                    res = setData.op_code + _data.packet_num;
                    break;
                case GPIO_OP_CODE.PACK_NACK:
                    setData.length = 4;
                    res = setData.op_code + setData.packet_num;
                    break;
                case GPIO_OP_CODE.PACK_INIT_SERVER:
                    setData.length = 4;
                    res = "10" + "10";//앞의 것 프로그램 버전이므로 수정해야함 임의로 10 넣은 것
                    break;
                case GPIO_OP_CODE.PACK_STOC_STATUS:
                    setData.length = 4;

                    high_bit = 0;

                    if (_io_out.Get(GPIO_DEF.OUT_TOWER_YELLOW) == true)
                        high_bit |= 1 << 1;
                    if (_io_out.Get(GPIO_DEF.OUT_RETURN_LAMP) == true)
                        high_bit |= 1 << 3;
                    if (_io_out.Get(GPIO_DEF.OUT_START_LAMP) == true)
                        high_bit |= 1 << 4;

                    res = high_bit.ToString("X4");//0002
                    setData.cm_state = error_state_send(MACHINE_STATE);
                    //res = setData.cm_state;
                    Log.Info($"[GPIO] after packing cm_state is {setData.cm_state}");
                    Log.Info($"[GPIO] after packing res is {res}");
                    break;
                case GPIO_OP_CODE.PACK_STOC_OUPUT:
                    setData.length = 8;

                    high_bit = 0;

                    if (_io_out.Get(GPIO_DEF.OUT_TOWER_YELLOW) == true)
                        high_bit |= 1 << 1;
                    if (_io_out.Get(GPIO_DEF.OUT_RETURN_LAMP) == true)
                        high_bit |= 1 << 3;
                    if (_io_out.Get(GPIO_DEF.OUT_START_LAMP) == true)
                        high_bit |= 1 << 4;

                    //if (_io_out.Get(GPIO_DEF.OUT_TOWER_YELLOW) == true)
                    //    res = "0002";
                    //else
                    //    res = "0000";
                    res = high_bit.ToString("X4");

                    if (init)
                    {
                        Log.Info("[GPIO] Stage initializing send");
                        res += stage_init_send(MACHINE_STATE);
                        is_send = true;
                    }
                    else
                    {
                        //Log.Info("[GPIO] Normal send");
                        // sending 4,5 -> 8,9 together if you use the code below
                        byte[] buff = new byte[_edit_io_out.Length / 8];
                        _edit_io_out.CopyTo(buff, 0);

                        clear_edit_io();

                        ushort io_out = (ushort)(buff[0] | (buff[1] << 8));
                        res += io_out.ToString("X4");

                        Log.Info($"[GPIO] making data : {io_out.ToString("X4")} .. ");
                    }
                    // <--
                    //if (!initializing_finished && !is_send)
                    //{
                    //    Log.Info("Stage initializing send");
                    //    res += stage_init_send(MACHINE_STATE);
                    //    is_send = true;
                    //}
                    //else 
                    //{
                    //    Log.Info("Normal send");
                    //    // sending 4,5 -> 8,9 together if you use the code below
                    //    byte[] buff = new byte[_edit_io_out.Length / 8];
                    //    _edit_io_out.CopyTo(buff, 0);
                    //    ushort io_out = (ushort)(buff[0] | (buff[1] << 8));
                    //    res += io_out.ToString("X4");
                    //}
                    /////////////////// -->2023.05.04 

                    // unused code?
                    //for (int i = 0; i < buff.Length; i++)
                    //    res += Convert.ToChar(buff[i]).ToString();
                    //res += io_out.ToString("X4");
                    break;
                case GPIO_OP_CODE.PACK_STOC_AUTO_SEND_STATUS:
                    setData.length = 4;
                    res = MACHINE_STATE.ToString("X4");
                    break;
                case GPIO_OP_CODE.PACK_STOC_STOP_AUTO_SEND_STATUS:
                    setData.length = 4;
                    res = MACHINE_STATE.ToString("X4");
                    break;
                default:
                    break;
            }


            return res;
        }

        private void set_data_format()
        {
            _data.stx = "[";
            _data.etx = "]";
            _data.op_code = "";
            _data.length = 0;
            _data.packet_num = 0;
            _data.retry_count = 1;

            _send_data.stx = "[";
            _send_data.etx = "]";
            _send_data.op_code = "";
            _send_data.length = 0;
            //_send_data.packet_num = 0;
            _send_data.retry_count = 1;

            _response_data.stx = "[";
            _response_data.etx = "]";
            _response_data.op_code = GPIO_OP_CODE.PACK_ACK.ToString("X2");
            _response_data.length = 4;
            _response_data.packet_num = 0;
            _response_data.retry_count = 1;
        }

        private string sum_checksum(DATAS chk_data)
        {
            int res = 0;
            string origin_data = null;
            //if (ack_mode == false)
            //    origin_data = _data.stx + _data.packet_num.ToString("X2") + _data.retry_count.ToString("X2") + _data.op_code + _data.length.ToString("X2") + _data.data;
            //else
            //    origin_data = _response_data.stx + _response_data.packet_num.ToString("X2") + _response_data.retry_count.ToString("X2") + _response_data.op_code + _response_data.length.ToString("X2") + _response_data.data;
            origin_data = chk_data.stx + chk_data.packet_num.ToString("X2") + chk_data.retry_count.ToString("X2") + chk_data.op_code + chk_data.length.ToString("X2") + chk_data.data;

            byte[] buff = Encoding.ASCII.GetBytes(origin_data);

            foreach (byte item in buff)
                res += item;

            return res.ToString("X4");
        }

        private int read_data(bool Initialize = false)
        {
            //DateTime.Now();
            //lock (gpio_device_lock)
            {
                int size = -1;
                int cnt = 0;
                if (_client == null || _client.Connected == false)
                    return 0;

                byte[] item = new byte[1];
                byte[] buff = new byte[32];

                while (true)
                {
                    if (_client == null || _client.Connected == false)
                        return 0;
                    if ((size = _client.GetStream().Read(item, 0, item.Length)) <= 0)
                        return size;
                    buff[cnt] = item[0];
                    cnt++;

                    if (Encoding.ASCII.GetString(item) == _data.etx)
                        break;
                }

                size = cnt;


                string hex = BitConverter.ToString(buff, 0, size);
                //Log.Info($"[MCU] read_hex : {hex}   {size} bytes");


                if (split_packet(buff, size, Initialize) == false)
                {
                    size = -1;
                    return size;
                }

                // Update the last execution time
                // lastExecutionTime = DateTime.Now;

                return size;
            }
        }

        public bool lastCheckStatus;
        private int send_data_w(byte op_code, bool init = false)
        {
            try
            {
                //Log.Info("try locking send_data_w");
                lock (cyj_network_lock)
                {

                    int try_cnt = 0;

                    //while (try_cnt < 3)
                    //{
                    //    if (op_code == GPIO_OP_CODE.PACK_STOC_OUPUT || op_code == GPIO_OP_CODE.PACK_STOC_STATUS)
                    //    {
                    //        if (!waitNeededPacket)
                    //        {
                    //            break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //    // otherwise
                    //    Log.Info("[GPIO] waiting send_data_w when waitNeededPacket set to false : " + try_cnt.ToString());
                    //    Thread.Sleep(200);
                    //    try_cnt++;
                    //}

                    //if (op_code == GPIO_OP_CODE.PACK_STOC_OUPUT || op_code == GPIO_OP_CODE.PACK_STOC_STATUS)
                    //{
                    //    if (waitNeededPacket)
                    //    {
                    //        // if (op_code != GPIO_OP_CODE.PACK_STOC_STATUS)
                    //        {
                    //            Log.Info($"[GPIO] send_data_w keep opcode {op_code:X2}, _lastOpCodeToSend {_lastOpCodeToSend:X2}");
                    //            _lastOpCodeToSend = op_code;
                    //            return 0;
                    //        }
                    //    }
                    //}

                    // otherwise
                    Log.Info("[GPIO] send_data_w waitNeededPacket set to true and send_data");

                    waitNeededPacket = true;
                    return send_data(op_code, init);
                }
            }
            catch (Exception ex) { }

            return -1;
        }

        public void CheckStatus(bool val)
        {
            lastCheckStatus = val;
            send_data_w(GPIO_OP_CODE.PACK_STOC_STATUS);
        }

        private int send_data(byte op_code, bool init = false)
        {
            int res = -1;
            string msg = "";

            lock (gpio_device_lock)
            {
                // 우선 주석처리 2023.05.11
                /*
                // Code that should not run more frequently than every 50 Millisecconds
                if (DateTime.Now.Subtract(lastExecutionTime).TotalMilliseconds < DELAY_MCU * 10)
                {
                    Thread.Sleep(DELAY_MCU);
                }
                */
                ////////////////////////////////////

                msg = make_packet(op_code, init);

                if (msg == string.Empty)
                    return -1;

                if (_client == null || _client.Connected == false)
                {
                    Log.Info("[GPIO] send_data : [ERROR] client is not connected");
                    return 0;
                }
                //if (_queueOP.Count() > 0) 
                //{
                //    var en = msg;
                //    Log.Info("QUEUE_MODE");
                //    Log.Info($"_queueOP.Count() : {_queueOP}");
                //    _queueOP.Enqueue(en);
                //    //extra bool for recieved or not
                //}
                //foreach(var id in _queueOP);{ }

                lastsendpacket = msg;


                Log.Info($"[GPIO] send_data : {msg} #:{_data.packet_num} retry:{_send_data.retry_count} op:{_send_data.op_code} len:{_send_data.length} cm_state: {_send_data.cm_state}");

                byte[] buff = Encoding.ASCII.GetBytes(msg);
                _client.GetStream().Write(buff, 0, buff.Length);

                string hex = BitConverter.ToString(buff, 0, buff.Length);
                //Log.Info($"[TCV] send_hex : {hex}   {buff.Length} bytes");

                // Extra delay to prevent simultaneous sending commands

                // Update the last execution time
                lastExecutionTime = DateTime.Now;

            }
            return res;
        }
        private void retry_last_send()
        {
            lock (gpio_device_lock)
            {
                Log.Info($"[GPIO] repeat_last_data : {lastsendpacket} #:{_data.packet_num} retry:{_send_data.retry_count} op:{_send_data.op_code} len:{_send_data.length}");

                byte[] buff = Encoding.ASCII.GetBytes(lastsendpacket);
                _client.GetStream().Write(buff, 0, buff.Length);

                string hex = BitConverter.ToString(buff, 0, buff.Length);
                //Log.Info($"[TCV] repeat_last_hex : {hex}   {buff.Length} bytes");

                // Extra delay to prevent simultaneous sending commands

                // Update the last execution time
                lastExecutionTime = DateTime.Now;
            }
        }
        private void respose_ack_nack(byte ack_neck)
        {
            lock (send_lock)
            {
                string msg = _data.stx;
                string ack = "???";
                //Code that should not run more frequently than every 50 Millisecconds
                if (DateTime.Now.Subtract(lastExecutionTime).TotalMilliseconds < DELAY_MCU)
                {
                    Thread.Sleep(DELAY_MCU);

                    //Log.Info($"[GPIO] send_data : [IGNORE] op_code:{op_code}");
                    //return 1;
                }
                switch (ack_neck)
                {
                    case GPIO_OP_CODE.PACK_ACK:
                        _response_data.op_code = GPIO_OP_CODE.PACK_ACK.ToString("X2");
                        Log.Info("ACK");
                        ack = "ACK";
                        // for blocking continuous sends of ack with same ids
                        //if (lastIDAck == _data.packet_num)
                        //{
                        //    Log.Info($"[GPIO] lastIDAck : {lastIDAck}, ignore send ack");
                        //    return;
                        //}
                        lastIDAck = _data.packet_num;
                        break;
                    case GPIO_OP_CODE.PACK_NACK:
                        _response_data.op_code = GPIO_OP_CODE.PACK_NACK.ToString("X2");
                        Log.Info("NACK");
                        ack = "NACK";
                        break;
                }

                msg += _response_data.packet_num.ToString("X2");
                msg += _response_data.retry_count.ToString("X2");
                msg += _response_data.op_code;
                msg += _response_data.length.ToString("X2");
                msg += _response_data.data;
                msg += sum_checksum(_response_data);
                msg += _response_data.etx;

                Log.Info($"[GPIO] response {ack} : {msg} #:{_response_data.packet_num} retry:{_response_data.retry_count} op:{_response_data.op_code} len:{_response_data.length}");

                byte[] buff = Encoding.ASCII.GetBytes(msg);
                _client.GetStream().Write(buff, 0, buff.Length);

                // Extra delay to prevent simultaneous sending commands
                //Thread.Sleep(50);
                lastExecutionTime = DateTime.Now;
            }
        }
        #endregion

    }
}
