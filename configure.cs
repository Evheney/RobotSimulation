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

    public class robot_configure : configure_ini
    {
        public robot_configure(string ini_filepath)
        {
        }

        private int _robot_Type = 0;

        private int InSMD_Reset;

        private int tvReady = 12;
        private int tvInspection = 1;
        private int tvBarcodeOk = 14;
        private int tvBarcodeNG = 15;
        private int tvReelIsNotRegistered = 13;

        private string _robot_name = "";
        private bool UseRobot = false;
        
        public string ROBOT_NAME
        {
            get
            {
                return load("ROBOT_CONFIG", "ROBOT_NAME", _robot_name);
            }

            set
            {
                save("ROBOT_CONFIG", "ROBOT_NAME", value); _robot_name = value;
            }
        }

        public int ROBOT_TYPE
        {
            get
            {
                return load("ROBOT_CONFIG", "ROBOT_TYPE", _robot_Type);
            }

            set
            {
                save("ROBOT_CONFIG", "ROBOT_TYPE", value); _robot_Type = value;
            }
        }

        public int TvReady
        {
            get
            {
                return load("ROBOT_CONFIG", "TVREADY", tvReady);
            }

            set
            {
                save("ROBOT_CONFIG", "TVREADY", value); tvReady = value;
            }
        }
        public int TvInspection
        {
            get
            {
                return load("ROBOT_CONFIG", "TVINSPECTION", tvInspection);
            }

            set
            {
                save("ROBOT_CONFIG", "TVINSPECTION", value); tvInspection = value;
            }
        }
        public int TvBarcodeOk
        {
            get
            {
                return load("ROBOT_CONFIG", "TVBARCODEOK", tvBarcodeOk);
            }

            set
            {
                save("ROBOT_CONFIG", "TVBARCODEOK", value); tvBarcodeOk = value;
            }
        }
        public int TvBarcodeNG
        {
            get
            {
                return load("ROBOT_CONFIG", "TVBARCODENG", tvBarcodeNG);
            }

            set
            {
                save("ROBOT_CONFIG", "TVBARCODENG", value); tvBarcodeNG = value;
            }
        }
        public int TvReelIsNotRegistered
        {
            get
            {
                return load("ROBOT_CONFIG", "TVREELISNOTREGISTERED", tvReelIsNotRegistered);
            }

            set
            {
                save("ROBOT_CONFIG", "TVREELISNOTREGISTERED", value); tvReelIsNotRegistered = value;
            }
        }

    }

    public class configure : configure_ini
    {
        public string APP_VER = "Ver 0.0.1 (2020-01-13 14:00:00)";

        private bool UseRobot = false;

        public bool USEROBOT
        {
            get
            {
                string userob = load("WORK_CONFIG", "USE_ROBOT", "OFF");
                if (String.Compare(userob, "ON", true) == 0 ||
                    String.Compare(userob, "USE", true) == 0 ||
                    String.Compare(userob, "1", true) == 0 ||
                    String.Compare(userob, "TRUE", true) == 0)
                    return true;

                return false;
            }

            set
            {
                save("WORK_CONFIG", "USE_ROBOT", value); UseRobot = value;
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
