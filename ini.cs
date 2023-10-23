using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace chip_counter
{
    public class IniFile
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileInt(string Section, string Key, int nDefault, string FilePath);

        public struct IniKey
        {
            public IniFile parent;
            public string section;
            public string v;


            //public static implicit operator string(IniKey keyValue)
            //{
            //    return keyValue.v;
            //}

            public static implicit operator IniKey(string value)
            {
                IniKey key_info = new IniKey();

                key_info.v = value;
                key_info.parent = null;
                key_info.section = "";

                return key_info;
            }

            public IniKey this[string key]
            {
                get
                {
                    v = parent.Read(key, "", section);
                    return this;
                }

                set
                {
                    parent.Write(key, value.v, section);
                }
            }
        }

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public IniKey this[string section]
        {
            get
            {
                IniKey key_info = new IniKey();

                key_info.parent = this;
                key_info.section = section;

                return key_info;
            }
            set
            {
                IniKey key_info = value;

                //key_info.parent = this;
                //key_info.section = value;

            }
        }


        public uint Read(string Key, uint defaultValue = 0, string Section = null)
        {
            return GetPrivateProfileInt(Section ?? EXE, Key, (int)defaultValue, Path);

        }

        public int Read(string Key, int defaultValue = 0, string Section = null)
        {
            return (int)GetPrivateProfileInt(Section ?? EXE, Key, defaultValue, Path);

        }

        public bool Read(string Key, bool defaultValue = false, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            string DefaultString = defaultValue ? "TRUE" : "FALSE";
            bool returnVal = defaultValue;

            GetPrivateProfileString(Section ?? EXE, Key, DefaultString, RetVal, 255, Path);


            if (String.Equals(RetVal.ToString(), "TRUE", StringComparison.OrdinalIgnoreCase))
                returnVal = true;
            else
                returnVal = false;

            return returnVal;
        }

        public double Read(string Key, double defaultValue = .0f, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            string DefaultString = Convert.ToString(defaultValue);
            double retturnVal = .0f;

            GetPrivateProfileString(Section ?? EXE, Key, DefaultString, RetVal, 255, Path);

            retturnVal = Convert.ToDouble(RetVal.ToString());

            return retturnVal;
        }

        public string Read(string Key, string defaultValue = "", string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, defaultValue, RetVal, 255, Path);
            return RetVal.ToString();
        }

        /// maybe for future

        //public char Read(string Key, char defaultValue = ' ', string Section = null)
        //{
        //    return (char)GetPrivateProfileInt(Section ?? EXE, Key, defaultValue, Path);

        //}

        public void Write(string Key, bool Value, string Section = null)
        {
            string BooleanString = Value ? "TRUE" : "FALSE";

            WritePrivateProfileString(Section ?? EXE, Key, BooleanString, Path);
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void Write(string Key, int Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value.ToString(), Path);
        }

        public void Write(string Key, uint Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value.ToString(), Path);
        }

        public void Write(string Key, double Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value.ToString(), Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}