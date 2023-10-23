using System;
using System.Drawing;
using System.IO;

namespace chip_counter
{
    public class configure_ini
    {
        protected IniFile ini;
        private string _ini_filepath = "";

        protected configure_ini()
        {

        }

        protected configure_ini(string ini_filepath)
        {
            _ini_filepath = ini_filepath;
            ini = new IniFile(ini_filepath);
        }

        protected void Ini_Init(string ini_filepath)
        {
            _ini_filepath = ini_filepath;
            ini = new IniFile(ini_filepath);
        }

        protected void save(string section, string key, string value)
        {
            try
            {
                ini.Write(key, value, section);
            }
            catch (Exception ex) { }
        }
        protected void save(string section, string key, int value)
        {
            try
            {
                ini.Write(key, value, section);
            }
            catch (Exception ex) { }
        }
        protected void save(string section, string key, uint value)
        {
            try
            {
                ini.Write(key, value, section);
            }
            catch (Exception ex) { }
        }

        protected void save(string section, string key, double value)
        {
            try
            {
                ini.Write(key, value, section);
            }
            catch (Exception ex) { }
        }

        protected void save(string section, string key, bool value)
        {
            try
            {
                ini.Write(key, value, section);
            }
            catch (Exception ex) { }
        }
        /// maybe for future
        //protected void save(string section, string key, char value)
        //{
        //    try
        //    {
        //        ini.Write(key, value, section);
        //    }
        //    catch (Exception ex) { }
        //}

        protected bool load(string section, string key, bool value = false)
        {
            try
            {
                value = ini.Read(key, value, section);
            }
            catch (Exception ex) { }

            return value;
        }

        protected int load(string section, string key, int value = 0)
        {
            try
            {
                value = ini.Read(key, value, section);
            }
            catch (Exception ex) { }

            return value;
        }

        protected uint load(string section, string key, uint value = 0)
        {
            try
            {
                value = ini.Read(key, value, section);
            }
            catch (Exception ex) { }

            return value;
        }

        protected double load(string section, string key, double value = 0)
        {
            try
            {
                value = ini.Read(key, value, section);
            }
            catch (Exception ex) { }

            return value;
        }

        protected string load(string section, string key, string value = "")
        {
            //"WORK_CONFIG", "USE_SYNCHRONOUS_INSPECT", "OFF"
            try
            {
                value = ini.Read(key, value, section);
            }
            catch (Exception ex) { }

            return value;
        }
        ///maybe for future
        //protected char load(string section, string key, char value = ' ')
        //{
        //    try
        //    {
        //        value = ini.Read(key, value, section);
        //    }
        //    catch (Exception ex) { }

        //    return value;
        //}
    }

    public class dosimeter_configure : configure_ini
    {
        private string _comport = "COM4";
        private int _baurdrate = 9600;
        private int _databits = 8;
        private int _stopbits = 1;
        private int _parity = 0;
        private int _handshake = 0;
        private double _alarm_max_value = 2.0;

        public dosimeter_configure(string ini_filepath) : base(ini_filepath)
        {

        }

        public string COMPORT
        {
            get
            {
                _comport = load("DOSIMETER_CONFIG", "COMPORT", _comport);
                return _comport;
            }
            set
            {
                _comport = value;
                save("DOSIMETER_CONFIG", "COMPORT", _comport);
            }
        }

        public int BAUDRATE
        {
            get
            {
                _baurdrate = load("DOSIMETER_CONFIG", "BAUDRATE", _baurdrate);
                return _baurdrate;
            }
            set
            {
                _baurdrate = value;
                save("DOSIMETER_CONFIG", "BAUDRATE", _baurdrate);
            }

        }

        public int DATABITS
        {
            get
            {
                _databits = load("DOSIMETER_CONFIG", "DATABITS", _databits);
                return _databits;
            }
            set
            {
                _databits = value;
                save("DOSIMETER_CONFIG", "DATABITS", _databits);
            }
        }

        public int STOPBITS
        {
            get
            {
                _stopbits = load("DOSIMETER_CONFIG", "STOPBITS", _stopbits);
                return _stopbits;
            }
            set
            {
                _stopbits = value;
                save("DOSIMETER_CONFIG", "STOPBITS", _stopbits);
            }
        }

        public int PARITY
        {
            get
            {
                _parity = load("DOSIMETER_CONFIG", "PARITY", _parity);
                return _parity;
            }
            set
            {
                _parity = value;
                save("DOSIMETER_CONFIG", "PARITY", _parity);
            }
        }

        public int HANDSHAKE
        {
            get
            {
                _handshake = load("DOSIMETER_CONFIG", "HANDSHAKE", _handshake);
                return _handshake;
            }
            set
            {
                _handshake = value;
                save("DOSIMETER_CONFIG", "HANDSHAKE", _handshake);
            }
        }

        public bool USE
        {
            get
            {
                string temp = load("DOSIMETER_CONFIG", "USE", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("DOSIMETER_CONFIG", "USE", "ON");
                else
                    save("DOSIMETER_CONFIG", "USE", "OFF");
            }
        }

        public int SENSOR_COUNT
        {
            get { return load("DOSIMETER_CONFIG", "SENSOR_COUNT", 1); }
            set { save("DOSIMETER_CONFIG", "SENSOR_COUNT", value); }
        }

        public double ALARM_VALUE
        {
            get { return load("DOSIMETER_CONFIG", "ALARM_MAX_VALUE", _alarm_max_value); }
            set { _alarm_max_value = value; save("DOSIMETER_CONFIG", "ALARM_MAX_VALUE", value); }
        }
    }

    public class tube_configure : configure_ini
    {
        private string _comport = "COM1";
        private int _baurdrate = 9600;
        private int _databits = 8;
        private int _stopbits = 1;
        private int _parity = 0;
        private int _handshake = 0;
        private DateTime _tube_start_datetime = DateTime.Now - new TimeSpan(10, 0, 0, 0);
        private DateTime _tube_last_datetime = DateTime.Now - new TimeSpan(10, 0, 0, 0);
        private double _voltage = 50.0f;
        private double _current = 50.0f;
        private int _tube_type = 1;
        private int tubeonTime = 5;
        private int tubeoffTime = 5;
        private int tubestep = 5;

        public int TUBE_TYPE
        {
            get
            {
                _tube_type = load("TUBE_CONFIG", "TUBE_TYPE", _tube_type);
                return _tube_type;
            }
            set
            {
                _tube_type = value;
                save("TUBE_CONFIG", "TUBE_TYPE", value);
            }
        }

        public tube_configure(string ini_filepath) : base(ini_filepath)
        {
            // 초기에만 읽는다.
            _tube_type = load("TUBE_CONFIG", "TUBE_TYPE", _tube_type);
        }

        public string COMPORT
        {
            get
            {

                _comport = load("TUBE_CONFIG", "COMPORT", _comport);
                return _comport;
            }
            set
            {
                _comport = value;
                save("TUBE_CONFIG", "COMPORT", _comport);
            }
        }

        public int BAUDRATE
        {
            get
            {
                _baurdrate = load("TUBE_CONFIG", "BAUDRATE", _baurdrate);
                return _baurdrate;
            }
            set
            {
                _baurdrate = value;
                save("TUBE_CONFIG", "BAUDRATE", _baurdrate);
            }

        }

        public int DATABITS
        {
            get
            {
                _databits = load("TUBE_CONFIG", "DATABITS", _databits);
                return _databits;
            }
            set
            {
                _databits = value;
                save("TUBE_CONFIG", "DATABITS", _databits);
            }
        }

        public int STOPBITS
        {
            get
            {
                _stopbits = load("TUBE_CONFIG", "STOPBITS", _stopbits);
                return _stopbits;
            }
            set
            {
                _stopbits = value;
                save("TUBE_CONFIG", "STOPBITS", _stopbits);
            }
        }

        public int PARITY
        {
            get
            {
                _parity = load("TUBE_CONFIG", "PARITY", _parity);
                return _parity;
            }
            set
            {
                _parity = value;
                save("TUBE_CONFIG", "PARITY", _parity);
            }
        }

        public int HANDSHAKE
        {
            get
            {
                _handshake = load("TUBE_CONFIG", "HANDSHAKE", _handshake);
                return _handshake;
            }
            set
            {
                _handshake = value;
                save("TUBE_CONFIG", "HANDSHAKE", _handshake);
            }
        }

        public double VOLTAGE
        {
            get
            {
                _voltage = load("TUBE_CONFIG", "VOLTAGE", _voltage);
                return _voltage;
            }
            set
            {
                _voltage = value;
                save("TUBE_CONFIG", "VOLTAGE", _voltage);
            }
        }

        public double CURRENT
        {
            get
            {
                _current = load("TUBE_CONFIG", "CURRENT", _current);
                return _current;
            }
            set
            {
                _current = value;
                save("TUBE_CONFIG", "CURRENT", _current);
            }
        }

        // Start time tube using.
        public DateTime TUBE_START_DATETIME
        {
            get
            {
                string start_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string chk_datetime = start_datetime;
                start_datetime = load("TUBE_CONFIG", "START_DATETIME", start_datetime);
                if (start_datetime == chk_datetime)
                    save("TUBE_CONFIG", "START_DATETIME", start_datetime);
                _tube_start_datetime = DateTime.Parse(start_datetime);

                return _tube_start_datetime;
            }
            set
            {
                _tube_start_datetime = value;

                string start_datetime = _tube_start_datetime.ToString("yyyy/MM/dd HH:mm:ss");

                save("TUBE_CONFIG", "START_DATETIME", start_datetime);
            }
        }

        public int USED_TIME
        {
            get
            {
                return load("TUBE_CONFIG", "USED_TIME", 0);
            }

            set
            {
                save("TUBE_CONFIG", "USED_TIME", value);
            }
        }

        public DateTime TUBE_LAST_DATE
        {
            get
            {
                string last_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                last_datetime = load("TUBE_CONFIG", "LAST_DATETIME", last_datetime);

                _tube_last_datetime = DateTime.Parse(last_datetime);

                return _tube_last_datetime;
            }
            set
            {
                _tube_last_datetime = value;

                string last_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                save("TUBE_CONFIG", "LAST_DATETIME", last_datetime);
            }
        }
        public int tubeONTime
        {
            get
            {
                tubeonTime = load("TUBE_CONFIG", "TUBEONTIME", tubeonTime);
                if (tubeonTime == 0) { tubeonTime = 10; }
                return tubeonTime;
            }
            set
            {
                tubeonTime = value;
                save("TUBE_CONFIG", "TUBEONTIME", value);
            }
        }
        public int tubeOFFTime
        {
            get
            {
                tubeoffTime = load("TUBE_CONFIG", "TUBEOFFTIME", tubeoffTime);
                if (tubeoffTime == 0) { tubeoffTime = 10; }
                return tubeoffTime;
            }
            set
            {
                tubeoffTime = value;
                save("TUBE_CONFIG", "TUBEOFFTIME", value);
            }
        }
        public int tubeStep
        {
            get
            {
                tubestep = load("TUBE_CONFIG", "TUBESTEP", tubestep);
                if (tubestep == 0) { tubestep = 5; }
                return tubestep;
            }
            set
            {
                tubestep = value;
                save("TUBE_CONFIG", "TUBESTEP", value);
            }
        }
    }

    public class detector_configure : configure_ini
    {
        protected IniFile advanced_ini;
        private string _detector_ip = "127.0.0.1";
        private string _my_ip = "127.0.0.1";
        private string _mode = "DARK";
        private string _data_filepath = Directory.GetCurrentDirectory() + "\\A_Data";
        private string _zoom_data_filepath = Directory.GetCurrentDirectory() + "\\A_Data";
        private string _ref_filepath = Directory.GetCurrentDirectory() + "\\A_Cal";
        private string _imageMargin = "0,0,0,0";
        private string _use_correction_afterimage = "OFF";
        private int _detector_delay_time; // millisecond
        //2022-07-21 Add Detector Type
        private int _detector_type = 0; //0 is bontech Detector
        //2022-01-28 SMH
        private DateTime _detector_last_datetime;
        public int _mingreylevel = 19000;
        public int _maxgreylevel = 19800;


        public detector_configure(string ini_path, string ini_filename)
        {
            _data_filepath = ini_path + @"\A_Data";
            _zoom_data_filepath = ini_path + @"\A_Data\Zoom";
            _ref_filepath = ini_path + @"\A_Cal";

            Ini_Init(_data_filepath + @"\" + "ImageCapture.ini");

            advanced_ini = new IniFile(ini_path + "\\" + ini_filename);
        }


        public string DETECTOR_IP
        {
            get
            {
                _detector_ip = load("PI Setting", "SensorIP", _detector_ip);
                return _detector_ip;

            }
            set
            {
                _detector_ip = value;
                save("PI Setting", "SensorIP", _detector_ip);
            }
        }

        public string MY_IP
        {
            get
            {
                _my_ip = load("PI Setting", "HostIP", _my_ip);
                return _my_ip;

            }
            set
            {
                _my_ip = value;
                save("PI Setting", "HostIP", _my_ip);
            }
        }

        public int DELAY_TIME
        {
            get
            {
                _detector_delay_time = advanced_ini.Read("DELAY_TIME", 2000, "DETECTOR_CONFIG");
                return _detector_delay_time;
            }

            set
            {
                _detector_delay_time = value;
                advanced_ini.Write("DELAY_TIME", _mode, "DETECTOR_CONFIG");
            }
        }

        public int CAP_MODE
        {
            get
            {
                return advanced_ini.Read("CAP_MODE", 2, "DETECTOR_CONFIG");

            }
            set
            {
                advanced_ini.Write("CAP_MODE", value, "DETECTOR_CONFIG");
            }
        }

        public int AVG_COUNT
        {
            get
            {
                return advanced_ini.Read("AVG_COUNT", 4, "DETECTOR_CONFIG");

            }
            set
            {
                advanced_ini.Write("AVG_COUNT", value, "DETECTOR_CONFIG");
            }
        }

        public string MODE
        {
            get
            {
                _mode = advanced_ini.Read("MODE", _mode, "DETECTOR_CONFIG");
                return _mode;

            }
            set
            {
                _mode = value;
                advanced_ini.Write("MODE", _mode, "DETECTOR_CONFIG");
            }
        }

        public string IMAGE_MAGIN
        {
            get
            {
                _imageMargin = advanced_ini.Read("MARGIN", _imageMargin, "DETECTOR_CONFIG");
                return _imageMargin;
            }

            set
            {
                _imageMargin = value;
                advanced_ini.Write("MARGIN", _imageMargin, "DETECTOR_CONFIG");
            }
        }

        public string ZOOM_DATA_FILEPATH
        {
            get
            {
                _zoom_data_filepath = advanced_ini.Read("ZOOM_DATA_FILEPATH", _zoom_data_filepath, "DETECTOR_CONFIG");
                return _zoom_data_filepath;

            }
            set
            {
                _zoom_data_filepath = value;
                advanced_ini.Write("ZOOM_DATA_FILEPATH", _zoom_data_filepath, "DETECTOR_CONFIG");
            }
        }

        public string DATA_FILEPATH
        {
            get
            {
                _data_filepath = advanced_ini.Read("DATA_FILEPATH", _data_filepath, "DETECTOR_CONFIG");
                return _data_filepath;

            }
            set
            {
                _data_filepath = value;
                advanced_ini.Write("DATA_FILEPATH", _data_filepath, "DETECTOR_CONFIG");
            }
        }

        public string REF_FILEPATH
        {
            get
            {
                _ref_filepath = advanced_ini.Read("REF_FILEPATH", _ref_filepath, "DETECTOR_CONFIG");
                return _ref_filepath;

            }
            set
            {
                _ref_filepath = value;
                advanced_ini.Write("REF_FILEPATH", _ref_filepath, "DETECTOR_CONFIG");
            }
        }

        public DateTime DETECTOR_LAST_DATE
        {
            get
            {
                try
                {
                    string last_datetime = advanced_ini.Read("LAST_DATETIME", DateTime.Now.ToString(), "DETECTOR_CONFIG");
                    _detector_last_datetime = DateTime.Parse(last_datetime);
                }
                catch
                {
                    _detector_last_datetime = DateTime.Now;
                }

                return _detector_last_datetime;
            }
            set
            {
                _detector_last_datetime = value;

                string last_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                advanced_ini.Write("LAST_DATETIME", last_datetime, "DETECTOR_CONFIG");
            }
        }

        public bool USE_CORRECTION_AFTERIMAGE
        {
            get
            {
                try
                {
                    _use_correction_afterimage = advanced_ini.Read("USE_CORRECTION_AFTERIMAGE", "OFF", "DETECTOR_CONFIG");

                    if (String.Compare(_use_correction_afterimage, "ON", true) == 0 ||
                        String.Compare(_use_correction_afterimage, "USE", true) == 0 ||
                        String.Compare(_use_correction_afterimage, "1", true) == 0 ||
                        String.Compare(_use_correction_afterimage, "TRUE", true) == 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                catch { return true; }

            }
            set
            {
                try
                {
                    if (value)
                    {
                        _use_correction_afterimage = "ON";
                    }
                    else
                    {
                        _use_correction_afterimage = "OFF";
                    }
                    advanced_ini.Write("USE_REMOVE_AFTERIMAGE", _use_correction_afterimage, "DETECTOR_CONFIG");

                }
                catch { }

            }
        }

        //
        public int DETECTOR_TYPE
        {
            get
            {

                int value = advanced_ini.Read("DETECTOR_TYPE", 0, "DETECTOR_CONFIG");

                if (value == 0)
                    _detector_type = DETECTOR_B;
                else if (value == 1)
                    _detector_type = DETECTOR_V;

                return _detector_type;

            }
            set
            {
                _detector_type = value;
                if (_detector_type == DETECTOR_B)
                    advanced_ini.Write("DETECTOR_TYPE", "0", "DETECTOR_CONFIG");
                else if (_detector_type == DETECTOR_V)
                    advanced_ini.Write("DETECTOR_TYPE", "1", "DETECTOR_CONFIG");

            }
        }
        public int DETECTOR_B
        {
            get { return 0; }
        }

        public int DETECTOR_V
        {
            get { return 1; }
        }

        public string VENU_DETECTOR_MODE
        {
            get
            {
                if (DETECTOR_TYPE == DETECTOR_B)
                    return "";

                string value = advanced_ini.Read("VENU_DETECTOR_MODE", "Mode1", "DETECTOR_CONFIG");
                if (value == string.Empty)
                    value = "Mode1";

                return value;
            }
        }
        public int MINGREYLEVEL
        {
            get
            {
                int mingreylevel = advanced_ini.Read("DETECTOR_CONFIG", _mingreylevel, "MINGREYLEVEL");
                if (mingreylevel == 0) { mingreylevel = 19000; }
                return mingreylevel;
            }
            set
            {
                _mingreylevel = value;
                advanced_ini.Write("DETECTOR_CONFIG", value, "MINGREYLEVEL");
            }
        }
        public int MAXGREYLEVEL
        {
            get
            {
                int maxgreylevel = advanced_ini.Read("DETECTOR_CONFIG", _maxgreylevel, "MAXGREYLEVEL");
                if (maxgreylevel == 0) { maxgreylevel = 19800; }
                return maxgreylevel;
            }
            set
            {
                _maxgreylevel = value;
                advanced_ini.Write("DETECTOR_CONFIG", value, "MAXGREYLEVEL");
            }
        }

    }



    public class scanner_configure : configure_ini
    {
        private string _comport = "COM1";
        private int _baurdrate = 9600;
        private int _databits = 8;
        private int _stopbits = 1;
        private int _parity = 0;
        private int _handshake = 0;
        private string _scanner_model = "DS2208";
        private bool _use_hid_keyboad = false;
        private bool _use_regex = false;
        private bool useHoneywell = false;
        private string regex_format = "[^\u0020-\u007E]";
        private string regex_replace_char = "@";

        public scanner_configure(string ini_filepath) : base(ini_filepath)
        {

        }

        public bool HID_KEYBOARD
        {
            get
            {
                _use_hid_keyboad = load("BARCODE_CONFIG", "USE_HID_KEYBOARD", _use_hid_keyboad);
                return _use_hid_keyboad;
            }
            set
            {
                _use_hid_keyboad = value;
                save("BARCODE_CONFIG", "USE_HID_KEYBOARD", _use_hid_keyboad);
            }
        }

        public string REGEX_REPLACE_CHAR
        {
            get
            {
                regex_replace_char = load("BARCODE_CONFIG", "REGEX_REPLACE_CHAR", regex_replace_char);
                return regex_replace_char;
            }
            set
            {
                regex_replace_char = value;
                save("BARCODE_CONFIG", "REGEX_REPLACE_CHAR", regex_replace_char);
            }
        }

        public bool USE_REGEX
        {
            get
            {
                string temp = load("BARCODE_CONFIG", "USE_REGEX", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                {
                    return true;
                }
                return false;
            }

            set
            {
                if (value)
                    save("BARCODE_CONFIG", "USE_REGEX", "ON");
                else
                    save("BARCODE_CONFIG", "USE_REGEX", "OFF");
            }
        }

        public string REGEX_FORMAT
        {
            get
            {
                regex_format = load("BARCODE_CONFIG", "REGEX_FORMAT", regex_format);
                return regex_format;
            }
            set
            {
                regex_format = value;
                save("BARCODE_CONFIG", "REGEX_FORMAT", regex_format);
            }
        }

        public string SCANNER_MODEL
        {
            get
            {
                _scanner_model = load("BARCODE_CONFIG", "MODEL_NAME", _scanner_model);
                return _scanner_model;
            }
            set
            {
                _scanner_model = value;
                save("BARCODE_CONFIG", "MODEL_NAME", _scanner_model);
            }
        }
        public string COMPORT
        {
            get
            {

                _comport = load("BARCODE_CONFIG", "COMPORT", _comport);
                return _comport;
            }
            set
            {
                _comport = value;
                save("BARCODE_CONFIG", "COMPORT", _comport);
            }
        }

        public int BAUDRATE
        {
            get
            {
                _baurdrate = load("BARCODE_CONFIG", "BAUDRATE", _baurdrate);
                return _baurdrate;
            }
            set
            {
                _baurdrate = value;
                save("BARCODE_CONFIG", "BAUDRATE", _baurdrate);
            }

        }

        public int DATABITS
        {
            get
            {
                _databits = load("BARCODE_CONFIG", "DATABITS", _databits);
                return _databits;
            }
            set
            {
                _databits = value;
                save("BARCODE_CONFIG", "DATABITS", _databits);
            }
        }

        public int STOPBITS
        {
            get
            {
                _stopbits = load("BARCODE_CONFIG", "STOPBITS", _stopbits);
                return _stopbits;
            }
            set
            {
                _stopbits = value;
                save("BARCODE_CONFIG", "STOPBITS", _stopbits);
            }
        }

        public int PARITY
        {
            get
            {
                _parity = load("BARCODE_CONFIG", "PARITY", _parity);
                return _parity;
            }
            set
            {
                _parity = value;
                save("BARCODE_CONFIG", "PARITY", _parity);
            }
        }

        public int HANDSHAKE
        {
            get
            {
                _handshake = load("BARCODE_CONFIG", "HANDSHAKE", _handshake);
                return _handshake;
            }
            set
            {
                _handshake = value;
                save("BARCODE_CONFIG", "HANDSHAKE", _handshake);
            }
        }

        public bool INSTALL_INNER_SCANNER
        {
            get
            {
                return load("BARCODE_CONFIG", "INSTALL_INNER_SCANNER", false);
            }

            set
            {
                save("BARCODE_CONFIG", "INSTALL_INNER_SCANNER", value);
            }
        }

        public bool USE_INNER_SCANNER
        {
            get
            {
                return load("BARCODE_CONFIG", "USE_INNER_SCANNER", false);
            }

            set
            {
                save("BARCODE_CONFIG", "USE_INNER_SCANNER", value);
            }
        }

        public string INNER_SCANNER_IP
        {
            get
            {
                return load("BARCODE_CONFIG", "INNER_SCANNER_IP", "192.168.1.10");
            }

            set
            {
                save("BARCODE_CONFIG", "INNER_SCANNER_IP", value);
            }
        }

        public int INNER_SCANNER_PORT
        {
            get
            {
                return load("BARCODE_CONFIG", "INNER_SCANNER_PORT", 2002);
            }

            set
            {
                save("BARCODE_CONFIG", "INNER_SCANNER_PORT", value);
            }
        }

        public int BARCODE_READING_TIME
        {
            get
            {
                return load("BARCODE_CONFIG", "BARCODE_READING_TIME", 2500);
            }

            set
            {
                save("BARCODE_CONFIG", "BARCODE_READING_TIME", value);
            }
        }

        public bool USE_SELECT_BARCODE_FOR_AUTOSMART
        {
            get
            {
                string temp = load("BARCODE_CONFIG", "USE_SELECT_BARCODE_FOR_AUTOSMART", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                {
                    return true;
                }
                return false;
            }

            set
            {
                if (value)
                    save("BARCODE_CONFIG", "USE_SELECT_BARCODE_FOR_AUTOSMART", "ON");
                else
                    save("BARCODE_CONFIG", "USE_SELECT_BARCODE_FOR_AUTOSMART", "OFF");
            }
        }

        public string AUTOSMART_RELATION_ROI
        {
            get
            {
                return load("BARCODE_CONFIG", "AUTOSMART_RELATION_ROI", "1,2,3,4");
            }

        }

        public string HONEYWELL_SCANNER
        {
            get
            {
                return load("BARCODE_CONFIG", "HONEYWELL_SCANNER", "OFF");
            }
            set
            {
                save("BARCODE_CONFIG", "HONEYWELL_SCANNER", value);
            }
        }


    }

    public class printer_configure : configure_ini
    {
        private string _printer_name = "ZDesigner GT800 (ZPL)";
        private bool _use_printer = true;
        private string _format_file1 = "PRINTER_FMT1.LBL";
        private string _format_file2 = "PRINTER_FMT2.LBL";
        private int _print_fmt = 0;
        private string _print_custom_name = "";
        private Size _paper_size = new Size(120, 50);

        public printer_configure(string ini_filepath) : base(ini_filepath)
        {

        }

        public string NAME
        {
            get
            {
                _printer_name = load("PRINTER_CONFIG", "NAME", _printer_name);
                return _printer_name;
            }
            set
            {
                _printer_name = value;
                save("PRINTER_CONFIG", "NAME", _printer_name);
            }
        }

        public Size PAPER_SIZE
        {
            get
            {
                _paper_size.Width = load("PRINTER_CONFIG", "PAPER_WIDTH", _paper_size.Width);
                _paper_size.Height = load("PRINTER_CONFIG", "PAPER_HEIGHT", _paper_size.Height);

                return _paper_size;
            }

            set
            {
                _paper_size = value;

                save("PRINTER_CONFIG", "PAPER_WIDTH", _paper_size.Width);
                save("PRINTER_CONFIG", "PAPER_HEIGHT", _paper_size.Height);
            }
        }

        public string PRINT_CUSTOM_NAME
        {
            get
            {
                return load("PRINTER_CONFIG", "CUSTOM_NAME", _print_custom_name);
            }

            set
            {
                save("PRINTER_CONFIG", "CUSTOM_NAME", value);
                _print_custom_name = value;
            }
        }

        public int PRINT_FMT
        {
            get
            {
                return load("PRINTER_CONFIG", "FMT", _print_fmt);
            }

            set
            {
                save("PRINTER_CONFIG", "FMT", value);
                _print_fmt = value;
            }
        }

        public bool USE
        {
            get
            {
                string temp = load("PRINTER_CONFIG", "USE", "ON");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("PRINTER_CONFIG", "USE", "ON");
                else
                    save("PRINTER_CONFIG", "USE", "OFF");
            }
        }

        public string FMT1
        {
            get
            {
                _format_file1 = load("PRINTER_CONFIG", "FMT1", _format_file1);
                return _format_file1;
            }
            set
            {
                _format_file1 = value;
                save("PRINTER_CONFIG", "FMT1", _format_file1);
            }
        }

        public string FMT2
        {
            get
            {
                _format_file2 = load("PRINTER_CONFIG", "FMT2", _format_file2);
                return _format_file2;
            }
            set
            {
                _format_file2 = value;
                save("PRINTER_CONFIG", "FMT2", _format_file2);
            }
        }

        public bool USE_VERIFICATION_BARCODE_FOR_PRINT
        {
            get
            {
                string temp = load("PRINTER_CONFIG", "USE_VERIFICATION_BARCODE_FOR_PRINT", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("PRINTER_CONFIG", "USE_VERIFICATION_BARCODE_FOR_PRINT", "ON");
                else
                    save("PRINTER_CONFIG", "USE_VERIFICATION_BARCODE_FOR_PRINT", "OFF");
            }
        }
    }

    public class barcode_reader_configure : configure_ini
    {
        public barcode_reader_configure(string ini_filepath) : base(ini_filepath)
        {

        }

        public string IP
        {
            get
            {
                return load("BARCODE_READER_CONFIG", "IP", "192.168.0.1");
            }
            set
            {
                save("BARCODE_READER_CONFIG", "IP", value);
            }
        }

        public char GROUP_SEPERATOR
        {
            get
            {
                int seperator = load("BARCODE_READER_CONFIG", "GROUP_SEPERATOR", 0x1C);
                return (char)seperator;
            }

            set
            {
                int seperator = value;
                save("BARCODE_READER_CONFIG", "GROUP_SEPERATOR", seperator);
            }
        }

        public string DATA_SEPERATOR
        {
            get
            {
                return load("BARCODE_READER_CONFIG", "DATA_SEPERATOR", @"\,\,");
            }

            set
            {
                save("BARCODE_READER_CONFIG", "DATA_SEPERATOR", value);
            }
        }

    }

    public class configure : configure_ini
    {
        public string APP_VER = "Ver 0.0.1 (2020-01-13 14:00:00)";

        private int print_font_size = 10;

        private tube_configure _tube_config;
        private detector_configure _detector_config;
        private scanner_configure _scanner_config;
        private printer_configure _printer_config;
        private dosimeter_configure _dosimeter_config;
        private barcode_reader_configure _barcode_reader_config;

        private int _stage_in_timeout = 5000; //5000
        private int _stage_out_timeout = 5000; //5000
        private int _detector_up_timeout = 5000; //5000
        private int _detector_down_timeout = 5000; //5000
        private int _xray_timeout = 5000; //5000
        private int _detector_timeout = 5000; //5000
        private int _inspection_timeout = 5000; //5000
        private string _customer = "none";
        private string _model = "none";
        private string _barcode_pattern_type = "DEFAULT";
        private string _barcode_pattern = "@BARCODE1";
        private string _barcode_scanner_code_encoder = "UTF8";
        private int _barcode_type = 0;
        private int _print_type = 0;
        private int _detector_updown = 0;
        private int _detector_rotate = 0;
        private int _auto_inspect_after_scan = 0;
        private int _use_duplicate_check = 1;
        private int _is_detector_updown_invert = 0;
        private string administrator_password = "0317300825"; //"0317300825"
        private WATCrypt crypt = new WATCrypt("28902417");
        private string _language = "GERMAN";
        private bool _record_run_time = false;
        private string font = "none";

        public configure(string ini_filepath) : base(ini_filepath)
        {
            string directory = Path.GetDirectoryName(ini_filepath);

            _tube_config = new tube_configure(directory + @"\TUBE.INI");
            _detector_config = new detector_configure(directory, @"\DETECTOR.INI");
            _scanner_config = new scanner_configure(directory + @"\BARCODE_SCANNER.INI");
            _printer_config = new printer_configure(directory + @"\PRINTER.INI");
            _dosimeter_config = new dosimeter_configure(directory + @"\DOSIMETER.INI");
            _barcode_reader_config = new barcode_reader_configure(directory + @"\BARCODE_READER.INI");
        }

        public tube_configure TUBE
        {
            get { return _tube_config; }
        }

        public detector_configure DETECTOR
        {
            get { return _detector_config; }
        }

        public scanner_configure SCANNER
        {
            get { return _scanner_config; }
        }

        public printer_configure PRINT
        {
            get { return _printer_config; }
        }

        public dosimeter_configure DOSIMETER
        {
            get { return _dosimeter_config; }
        }

        public barcode_reader_configure BARCODE_READER
        {
            get { return _barcode_reader_config; }
        }

        public bool USE_SYNCHRONOUS_INSPECT
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_SYNCHRONOUS_INSPECT", "OFF");
                if (String.CompareOrdinal(temp, "ON") == 0 ||
                    String.CompareOrdinal(temp, "USE") == 0 ||
                    String.CompareOrdinal(temp, "1") == 0)
                    return true;

                return false;
            }
        }

        public int BOTTLE_INSPECT_LUT_MIN
        {
            get { return load("WORK_CONFIG", "BOTTLE_INSPECT_LUT_MIN", 10000); }
            set { save("WORK_CONFIG", "BOTTLE_INSPECT_LUT_MIN", value); }
        }

        public int BOTTLE_INSPECT_LUT_MAX
        {
            get { return load("WORK_CONFIG", "BOTTLE_INSPECT_LUT_MAX", 32767); }
            set { save("WORK_CONFIG", "BOTTLE_INSPECT_LUT_MAX", value); }
        }

        public bool USE_BOTTLE_INSPECT
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_BOTTLE_INSPECT", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_BOTTLE_INSPECT", "ON");
                else
                    save("WORK_CONFIG", "USE_BOTTLE_INSPECT", "OFF");
            }
        }

        public int VIEWER_LUT_MIN
        {
            get { return load("WORK_CONFIG", "VIEWER_LUT_MIN", 10000); }
            set { save("WORK_CONFIG", "VIEWER_LUT_MIN", value); }
        }

        public int VIEWER_LUT_MAX
        {
            get { return load("WORK_CONFIG", "VIEWER_LUT_MAX", 32767); }
            set { save("WORK_CONFIG", "VIEWER_LUT_MAX", value); }
        }

        public bool USE_SAVE_IMAGE
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_SAVE_IMAGE", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_SAVE_IMAGE", "ON");
                else
                    save("WORK_CONFIG", "USE_SAVE_IMAGE", "OFF");
            }
        }


        public int SAVE_IMAGE_INDEX
        {
            get
            {
                return load("WORK_CONFIG", "SAVE_IMAGE_INDEX", 1);
            }

            set
            {
                save("WORK_CONFIG", "SAVE_IMAGE_INDEX", value);
            }
        }

        public bool USE_DENIED_INSPECT_LIST
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_DENIED_INSPECT_LIST", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_DENIED_INSPECT_LIST", "ON");
                else
                    save("WORK_CONFIG", "USE_DENIED_INSPECT_LIST", "OFF");
            }
        }

        public bool USE_REEL_SENSOR
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_REEL_SENSOR", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_REEL_SENSOR", "ON");
                else
                    save("WORK_CONFIG", "USE_REEL_SENSOR", "OFF");
            }
        }

        public bool USE_READING_BARCODE_FILTER
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_READING_BARCODE_FILTER", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_READING_BARCODE_FILTER", "ON");
                else
                    save("WORK_CONFIG", "USE_READING_BARCODE_FILTER", "OFF");
            }
        }

        public int AUTO_START_AFTER_SCAN
        {
            get
            {
                return _auto_inspect_after_scan = load("WORK_CONFIG", "AUTO_START_AFTER_SCAN", _auto_inspect_after_scan);
            }

            set
            {
                save("WORK_CONFIG", "AUTO_START_AFTER_SCAN", value); _auto_inspect_after_scan = value;
            }

        }
        public int DETECTOR_ROTATE
        {
            get
            {
                _detector_rotate = load("WORK_CONFIG", "DETECTOR_ROTATE", _detector_rotate);
                return _detector_rotate;
            }

            set
            {
                save("WORK_CONFIG", "DETECTOR_ROTATE", value); _detector_rotate = value;
            }

        }

        public bool USE_DETECTOR_UPDOWN
        {
            get
            {
                return load("WORK_CONFIG", "USE_DETECTOR_UPDOWN", false);
            }
            set
            {
                save("WORK_CONFIG", "USE_DETECTOR_UPDOWN", value);
            }
        }

        public int DETECTOR_UPDOWN_INVERT
        {
            get
            {
                return _barcode_type = load("WORK_CONFIG", "DETECTOR_UPDOWN_INVERT", _is_detector_updown_invert);
            }

            set
            {
                save("WORK_CONFIG", "DETECTOR_UPDOWN_INVERT", value); _is_detector_updown_invert = value;
            }
        }

        public int DETECTOR_UPDOWN
        {
            get
            {
                return _barcode_type = load("WORK_CONFIG", "DETECTOR_UPDOWN", _detector_updown);
            }

            set
            {
                save("WORK_CONFIG", "DETECTOR_UPDOWN", value); _detector_updown = value;
            }
        }

        public int PRINT_FONT_SIZE
        {
            get
            {
                int value = load("WORK_CONFIG", "PRINT_FONT_SIZE", 10);
                if (value <= 0)
                {
                    value = 10;
                }
                print_font_size = value;
                return value;
            }
            set { if (value > 0) { save("WORK_CONFIG", "PRINT_FONT_SIZE", value); print_font_size = value; } }
        }
        public string FONT_NAME
        {
            //if(FONT_NAME!=null)
            get { return load("WORK_CONFIG", "FONT_NAME", font); }
            set { save("WORK_CONFIG", "FONT_NAME", value); }
            //get 
            //{
            //    string value = load("WORK_CONFIG", "FONT_NAME", font);
            //    if (font == null)
            //    {
            //        return "Microsoft Sans Serif";
            //    }
            //    else { return font; }
            //}            
            //set 
            //{
            //    if (font != null)
            //    {
            //        save("WORK_CONFIG", "FONT_NAME", font);
            //    }
            //    else { font = "Microsoft Sans Serif"; }
            //}
        }
        public string BARCODE_SCAN_CODE_ENCODING
        {
            get
            {
                return load("WORK_CONFIG", "BARCODE_CODE_ENCODING", _barcode_scanner_code_encoder);
            }

            set
            {
                save("WORK_CONFIG", "BARCODE_CODE_ENCODING", value);
            }
        }

        public int BARCODE_PATTERN_TYPE
        {
            get
            {
                _barcode_pattern_type = load("WORK_CONFIG", "BARCODE_PATTERN_TYPE", _barcode_pattern_type);
                return _barcode_pattern_type.Equals("DEFAULT") ? 0 : 1;
            }

            set
            {
                if (value == 0)
                    _barcode_pattern_type = "DEFAULT";
                else
                    _barcode_pattern_type = "DYNAMIC_EXTRACT";
                save("WORK_CONFIG", "BARCODE_PATTERN_TYPE", _barcode_pattern_type);
            }
        }

        public int ALGORITHM_RESULT_LEVEL
        {
            get { return load("WORK_CONFIG", "ALGORITHM_RESULT_LEVEL", 0); }
            set { save("WORK_CONFIG", "ALGORITHM_RESULT_LEVEL", 0); }
        }
        public bool USE_ALGORITHM_WARNING_RESULT
        {
            get { return load("WORK_CONFIG", "USE_ALGORITHM_WARNING_RESULT", false); }
            set { save("WORK_CONFIG", "USE_ALGORITHM_WARNING_RESULT", value); }
        }

        public string BARCODE_PATTERN
        {
            get { return _barcode_pattern = load("WORK_CONFIG", "BARCODE_PATTERN", _barcode_pattern); }
            set { save("WORK_CONFIG", "BARCODE_PATTERN", value); _barcode_pattern = value; }
        }

        public bool USE_SOCKET_MES
        {
            get { return load("WORK_CONFIG", "USE_SOCKET_MES", false); }


            set { save("WORK_CONFIG", "USE_SOCKET_MES", value); }
        }

        public int BARCODE_TYPE
        {
            get
            {
                return _barcode_type = load("WORK_CONFIG", "BARCODE_TYPE", _barcode_type);
            }

            set
            {
                save("WORK_CONFIG", "BARCODE_TYPE", value); _barcode_type = value;
            }
        }

        public int PRINT_TYPE
        {
            get
            {
                return _barcode_type = load("WORK_CONFIG", "PRINT_TYPE", _print_type);
            }

            set
            {
                save("WORK_CONFIG", "PRINT_TYPE", value); _print_type = value;
            }
        }

        public string LANGUAGE
        {
            get
            {
                return load("WORK_CONFIG", "LANGUAGE", _language);
            }

            set
            {
                save("WORK_CONFIG", "LANGUAGE", value);
            }
        }
        public string PASSWORD
        {
            get
            {
                string defaultValue = crypt.Encrypt("0317300825");
                string EncodeValue = load("WORK_CONFIG", "PASSWORD", defaultValue);
                string ret;

                ret = crypt.Decrypt(EncodeValue);

                ret = ret.Trim('\0');
                return ret;
            }

            set
            {
                string EncodeValue = crypt.Encrypt(value);

                save("WORK_CONFIG", "PASSWORD", EncodeValue);
            }
        }

        public string CUSTOMER
        {
            get { return load("WORK_CONFIG", "CUSTOMER", _customer); }
            set { save("WORK_CONFIG", "CUSTOMER", value); }
        }
        public string MODEL
        {
            get { return load("WORK_CONFIG", "MODEL", _model); }
            set { save("WORK_CONFIG", "MODEL", value); }
        }

        public int FRONT_AREA_TIMEOUT
        {
            get { return load("WORK_CONFIG", "FRONT_AREA_TIMEOUT", 1000); }
            set { save("WORK_CONFIG", "FRONT_AREA_TIMEOUT", value); }
        }

        public int STAGE_IN_TIMEOUT
        {
            get { return load("WORK_CONFIG", "STAGE_IN_TIMEOUT", 5000); }
            set { save("WORK_CONFIG", "STAGE_IN_TIMEOUT", value); }
        }

        public int STAGE_OUT_TIMEOUT
        {
            get { return load("WORK_CONFIG", "STAGE_OUT_TIMEOUT", 5000); }
            set { save("WORK_CONFIG", "STAGE_OUT_TIMEOUT", value); }

        }

        public int DETECTOR_UP_TIMEOUT
        {
            get { return load("WORK_CONFIG", "DETECTOR_UP_TIMEOUT", 40000); }
            set { save("WORK_CONFIG", "DETECTOR_UP_TIMEOUT", value); }

        }
        public int DETECTOR_DOWN_TIMEOUT
        {
            get { return load("WORK_CONFIG", "DETECTOR_DOWN_TIMEOUT", 40000); }
            set { save("WORK_CONFIG", "DETECTOR_DOWN_TIMEOUT", value); }

        }

        public int XRAY_TIMEOUT
        {
            get { return load("WORK_CONFIG", "XRAY_TIMEOUT", 5000); }
            set { save("WORK_CONFIG", "XRAY_TIMEOUT", value); }

        }
        public int DETECTOR_TIMEOUT
        {
            get { return load("WORK_CONFIG", "DETECTOR_TIMEOUT", 5000); }
            set { save("WORK_CONFIG", "DETECTOR_TIMEOUT", value); }

        }
        public int INSPECTION_TIMEOUT
        {
            get { return load("WORK_CONFIG", "INSPECTION_TIMEOUT", 5000); }
            set { save("WORK_CONFIG", "INSPECTION_TIMEOUT", value); }

        }

        public int IS_DUPLICATE_CHECK
        {
            get { return load("WORK_CONFIG", "USE_DUPLICATE_CHECK", _use_duplicate_check); }
            set { save("WORK_CONFIG", "USE_DUPLICATE_CHECK", value); }
        }

        public bool IS_RECORD_RUN_TIME
        {
            get { return load("WORK_CONFIG", "RECORD_RUN_TIME", _record_run_time); }
            set
            {
                _record_run_time = value;
                save("WORK_CONFIG", "RECORD_RUN_TIME", value);
            }
        }

        public string RESULT_FILE_PATH
        {
            get { return load("WORK_CONFIG", "RESULT_FILE_PATH", "RESULT"); }
            set { save("WORK_CONFIG", "RESULT_PATH", value); }
        }

        public string ONLY_RESULT_VIEW
        {
            get { return load("WORK_CONFIG", "ONLY_RESULT_VIEW", "OFF"); }
            set { save("WORK_CONFIG", "ONLY_RESULT_VIEW", value); }
        }

        public bool IS_USE_FAN
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_FAN", "OFF");

                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }
            set { save("WORK_CONFIG", "USE_FAN", value); }
        }

        //2022-01-19 SMG 로그 관리 기능
        public int DELETE_LOG_PERIOD
        {
            get
            {
                int value = load("WORK_CONFIG", "DEL_LOG_PERIOD", 14);
                if (value < 0)
                    value = 14;
                return value;
            }
            set { if (value >= 0) save("WORK_CONFIG", "DEL_LOG_PERIOD", value); }
        }

        //2022-01-27 SMH 특정 일수 단위로 캘리브레이션 실행시키는 기능
        public int PASSED_CALIBRATION_PERIOD
        {
            get
            {
                int value = load("WORK_CONFIG", "PASSED_CALIBRATION_PERIOD", 6);
                if (value < 0)
                    value = 6;
                return value;
            }
            set { if (value >= 0) save("WORK_CONFIG", "PASSED_CALIBRATION_PERIOD", value); }
        }

        public int PASSED_WARMUP_PERIOD
        {
            get
            {
                int value = load("WORK_CONFIG", "PASSED_WARMUP_PERIOD", 30);
                if (value < 0)
                    value = 30;
                return value;
            }
            set { if (value >= 0) save("WORK_CONFIG", "PASSED_CALIBRATION_PERIOD", value); }
        }

        public bool CREATE_TRIGGER_DARK
        {
            get { return load("WORK_CONFIG", "CREATE_TRIGGER_DARK", true); }
            set { save("WORK_CONFIG", "CREATE_TRIGGER_DARK", value); }
        }

        public bool USE_RESULT_COUNT_EDIT
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_RESULT_COUNT_EDIT", "OFF");

                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_RESULT_COUNT_EDIT", "ON");
                else
                    save("WORK_CONFIG", "USE_RESULT_COUNT_EDIT", "OFF");
            }
        }
        public bool USE_BARCODE2
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_BARCODE2", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_BARCODE2", "ON");
                else
                    save("WORK_CONFIG", "USE_BARCODE2", "OFF");
            }

        }
        public bool USE_SMARTSCANNER
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_SMARTSCANNER", "OFF");
                if (String.Compare(temp, "ON", true) == 0 ||
                    String.Compare(temp, "USE", true) == 0 ||
                    String.Compare(temp, "1", true) == 0 ||
                    String.Compare(temp, "TRUE", true) == 0)
                    return true;

                return false;
            }
            set
            {
                if (value)
                    save("WORK_CONFIG", "USE_SMARTSCANNER", "ON");
                else
                    save("WORK_CONFIG", "USE_SMARTSCANNER", "OFF");
            }
        }
        public bool USE_STAGE_INITIALIZING
        {
            get
            {
                string temp = load("WORK_CONFIG", "USE_STAGE_INITIALIZING", "OFF");
                if (String.Compare(temp, "ON", true) == 0) { return true; }

                return false;
            }
            set
            {
                save("WORK_CONFIG", "USE_STAGE_INITIALIZING", value ? "ON" : "OFF");
            }
        }
        public int THRESHOLD
        {
            get
            {
                return load("WORK_CONFIG", "THRESHOLD", 50); ;
            }

            set
            {
                save("WORK_CONFIG", "THRESHOLD", value);
            }
        }

        //value 0 == tmc_gpio, value 1 == MCUBoard
        public int GPIO_MODE
        {
            get
            {
                return load("WORK_CONFIG", "GPIO_MODE", 0); ;
            }

            set
            {
                save("WORK_CONFIG", "GPIO_MODE", value);
            }
        }
    }
}
