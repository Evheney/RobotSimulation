using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace chip_counter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeButtons();
            InitializeGpioINPanels();
            InitializeGpioOUTPanels();
            InitWatchdog();
            string ini_filepath = System.IO.Directory.GetCurrentDirectory() + "\\CHIP_COUNTER.INI";

            info.config = new configure(ini_filepath);
            gpio.Init(info.config);

            IO_Update_Timer.Interval = 500;
            IO_Update_Timer.Start();
        }

        private double minCurrent;
        private double maxCurrent;
        private int minVoltage;
        private int maxVoltage;
        private string name;

        private bool m_forcibly = false;
        ModSmdRobotControl control = ModSmdRobotControl.Instance;

        ChipCounterInfo info = ChipCounterInfo.Instance;
        GPIOProc gpio = GPIOProc.Instance;

        private Dictionary<Button, int> buttonIdIns = new Dictionary<Button, int>();
        private Dictionary<Button, int> buttonIdOuts = new Dictionary<Button, int>();
        private Dictionary<Button, Dictionary<string, Color>> buttonColors = new Dictionary<Button, Dictionary<string, Color>>();
        private Dictionary<Button, Dictionary<string, Color>> buttonColors2 = new Dictionary<Button, Dictionary<string, Color>>();
        private Dictionary<Button, Action<object, EventArgs>> buttonActions = new Dictionary<Button, Action<object, EventArgs>>();


        private List<Panel> gpioPanelsIN;  
        private List<Panel> gpioPanelsOUT;

        int test = 0;
        int idOut = 0;
        int idIn = 0;

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
        private void ButtonShow() 
        {
            if (info.config.USE_ROBOT == false)
            {
                panel1.Visible = false;
            }
            else if (info.config.USE_ROBOT == true) 
            {
                panel1.Visible = true;
            }
            IO_Control.Visible = true;
        }
        private void UIUpdate() 
        {
            if (info.config.USE_ROBOT == true) { ButtonShow(); }
            else if (info.config.USE_ROBOT == false) { ButtonShow(); }
            else { ButtonShow(); }
        }
        #region RobotControl        
        private void SetTvReady_Click(object sender, EventArgs e)
        {
            control.SetTvReady(true, m_forcibly);
        }

        private void SetTvInspectionDone_Click(object sender, EventArgs e)
        {
            control.SetTvInspectionDone(true);
        }

        private void SetTvBarcodeOK_Click(object sender, EventArgs e)
        {
            control.SetTvBarcodeOK(true);
        }

        private void SetTvBarcodeNG_Click(object sender, EventArgs e)
        {
            control.SetTvBarcodeNG(true);
        }

        private void SetTvReelIsNotRegistered_Click(object sender, EventArgs e)
        {
            control.SetTvReelIsNotRegistered(true, false);
        }

        private void ResetBarcode_Click(object sender, EventArgs e)
        {
            control.ResetBarcode();
        }

        private void SetIOSettings_Click(object sender, EventArgs e)
        {
            Log.Debug("SetIOSettings_Click ");
            if (test % 2 == 0)
            {
                control.SetupTvReadyDO(12);
                control.SetupTvInspectionDoneDO(1);
                control.SetupTvBarcodeOkDO(14);
                control.SetupTvBarcodeNgDO(15);
                control.SetupTvReelIsNotRegisteredDO(13);
                control.SetBarcode("Barcode1");
                test++;
            }
            else
            {
                control.SetupTvReadyDO(12);
                control.SetupTvInspectionDoneDO(1);
                control.SetupTvBarcodeOkDO(14);
                control.SetupTvBarcodeNgDO(15);
                control.SetupTvReelIsNotRegisteredDO(13);
                control.SetBarcode("Barcode2");
                test++;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void StageInSend_Click(object sender, EventArgs e)
        {
            gpio.SetOut(GPIO_DEF.OUT_STAGE_OUT, false);
            Log.Debug("--> Stage OUT " + 0);
            gpio.SetOut(GPIO_DEF.IN_STATGE_IN_SENSOR, true);
            Log.Debug("--> Stage IN " + 1);

        }

        private void StageOutSend_Click(object sender, EventArgs e)
        {
            gpio.SetOut(GPIO_DEF.IN_STATGE_IN_SENSOR, false);
            Log.Debug("--> Stage IN " + 0);
            gpio.SetOut(GPIO_DEF.OUT_STAGE_OUT, true);
            Log.Debug("--> Stage OUT " + 1);

        }
        private void SmdBarcodeReadyFunc_Click(object sender, EventArgs e)
        {
            //control.SmdBarcodeReadyFunc(1);
        }

        private void SmdPlaceReadyFunc_Click(object sender, EventArgs e)
        {
            //control.SmdPlaceReadyFunc(2);
        }

        private void SmdPickupReadyFunc_Click(object sender, EventArgs e)
        {
            //control.SmdPickupReadyFunc(3);
        }

        private void SmdResetFunc_Click(object sender, EventArgs e)
        {
            // control.SmdResetFunc(4);
        }

        private void SmdRobotInit_Click(object sender, EventArgs e)
        {
            control.SmdRobotInit();
        }

        private void SmdSendInspectionDone_Click(object sender, EventArgs e)
        {
            control.SmdSendInspectionDone(false, true);
        }

        private void SmdSendTvReady_Click(object sender, EventArgs e)
        {
            control.SmdSendTvReady(true);
        }

        private void SmdSendPickupOrReady_Click(object sender, EventArgs e)
        {
            control.SmdSendPickupOrReady(true, true, true);
        }

        private void SmdBarcodeOK_Click(object sender, EventArgs e)
        {
            control.SmdBarcodeOK("OK");
        }
        #endregion
        private void InitWatchdog() 
        {
            // Specify the path to the .ini file
            string iniFilePath = Directory.GetCurrentDirectory() + "\\CHIP_COUNTER.INI";

            // Create a new instance of FileSystemWatcher
            FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(iniFilePath), Path.GetFileName(iniFilePath));

            // Set the event handlers
            watcher.Changed += OnFileChanged;
            watcher.Created += OnFileCreated;
            watcher.Deleted += OnFileDeleted;
            watcher.Renamed += OnFileRenamed;

            // Enable the watcher
            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Watching {iniFilePath}. Press Enter to exit.");
            Console.ReadLine();

            // Stop watching when Enter is pressed
            //watcher.EnableRaisingEvents = false;
        }
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            // Handle file changes
            Log.Info($"File {e.FullPath} has been {e.ChangeType.ToString().ToLower()}d.");
            // You can implement further processing here, such as reading the .ini file and reacting to changes.

            // Reload values from the .ini file
            LoadIniFileValues(e.FullPath);

            // Perform actions based on the loaded values
            Log.Info($"minCurrent: {minCurrent}");
            Log.Info($"maxCurrent: {maxCurrent}");
            Log.Info($"minVoltage: {minVoltage}");
            Log.Info($"maxVoltage: {maxVoltage}");
            Log.Info($"name: {name}");

        }
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // Handle file created
            Log.Info($"File {e.FullPath} has been {e.ChangeType.ToString().ToLower()}d.");
            // You can implement further processing here, such as reading the .ini file and reacting to changes.
        }
        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            // Handle file changes
            Log.Info($"File {e.FullPath} has been {e.ChangeType.ToString().ToLower()}d.");
            // You can implement further processing here, such as reading the .ini file and reacting to changes.
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            // Handle file renaming
            Log.Info($"File {e.OldFullPath} has been renamed to {e.FullPath}.");
            // You can implement further processing here.
        }

        private void LoadIniFileValues(string filePath)
        {
            // Read values from the .ini file and update the variables
            Dictionary<string, string> iniValues = new Dictionary<string, string>();

            try
            {
                // Read all lines from the .ini file
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Split each line into key and value
                    string[] parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim().ToLower();
                        string value = parts[1].Trim();

                        iniValues[key] = value;
                    }
                }

                // Update variables with the parsed values
                if (iniValues.ContainsKey("USE_ROBOT ") && bool.TryParse(iniValues["USE_ROBOT "], out bool userobotValue))
                
                    info.config.USE_ROBOT = userobotValue;
                    UIUpdate();
                

                if (iniValues.ContainsKey("GPIO_MODE") && int.TryParse(iniValues["GPIO_MODE"], out int gpiomodeValue))
                    info.config.GPIO_MODE = gpiomodeValue;

                if (iniValues.ContainsKey("mincurrent") && double.TryParse(iniValues["mincurrent"], out double minCurrentValue))
                    minCurrent = minCurrentValue;

                if (iniValues.ContainsKey("maxcurrent") && double.TryParse(iniValues["maxcurrent"], out double maxCurrentValue))
                    maxCurrent = maxCurrentValue;

                if (iniValues.ContainsKey("minvoltage") && int.TryParse(iniValues["minvoltage"], out int minVoltageValue))
                    minVoltage = minVoltageValue;

                if (iniValues.ContainsKey("maxvoltage") && int.TryParse(iniValues["maxvoltage"], out int maxVoltageValue))
                    maxVoltage = maxVoltageValue;

                if (iniValues.ContainsKey("name"))
                    name = iniValues["name"];
            }
            catch (Exception ex)
            {
                Log.Error($"Error loading values from the .ini file: {ex.Message}");
            }
        }

        private void InitializeButtons()
        {
            // Associate buttons with their respective actions and images
            // AddButton(buttonName, PerformButtonNameAction, Resources.ButtonNameNormalImage, Resources.ButtonNameHoveredImage, Resources.ButtonNameClickedImage);
            // Associate buttons with their respective actions and images

            //AddButton(buttonName, (sender, e) => PerformButtonNameAction(sender, e), Color.On, Color.OFF);

            AddButton(btn_GPIO_OUT_0, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_1, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_2, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_3, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_4, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_5, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_6, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_7, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_8, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_9, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_10, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_11, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_12, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_13, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_14, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);
            AddButton(btn_GPIO_OUT_15, (sender, e) => Gpio_OUT_Clicked(sender, e), Color.Green, Color.Red);

            /*
            AddButtons(btn_GPIO_IN_0, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_1, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_2, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_3, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_4, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_5, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_6, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_7, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_8, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_9, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_10, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_11, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_12, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_13, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_14, Color.Green, Color.Red);
            AddButtons(btn_GPIO_IN_15, Color.Green, Color.Red);
            */


            // Add more buttons and images as needed
        }

        private void AddButton(Button button, Action<object, EventArgs> action, Color onColor, Color offColor)
        {
            Action<object, EventArgs> p = (sender, e) => action(sender, e);
            buttonActions.Add(button, p);
            buttonIdOuts.Add(button, idOut);
            idOut++;
            buttonColors.Add(button, new Dictionary<string, Color>
            {
                { "On", onColor },
                { "Off", offColor }
            });

            button.Tag = "Off"; // Initial state

            button.Click += btn_Click;
        }
        private void AddButtons(Button button, Color onColor, Color offColor)
        {

            //Action<object, EventArgs> p = (sender, e) => action(sender, e);
            //buttonActions.Add(button, p);
            buttonIdIns.Add(button, idIn);
            idIn++;
            buttonColors2.Add(button, new Dictionary<string, Color>
            {
                { "On", onColor },
                { "Off", offColor }
            });

            button.Tag = "Off"; // Initial state

            button.Click += btn_Clicks;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button buttonOut = (Button)sender;
            int buttonId = buttonIdOuts[buttonOut];

            if (buttonOut.Tag.ToString() == "Off")
            {
                buttonOut.Tag = "On";
                buttonOut.BackColor = buttonColors[buttonOut]["On"];
                // Add any other actions or logic for Click when the button is in the "On" state

                // Set GPIO output for the right buttons (assuming button IDs from 0 to 15 are for the right buttons)
                if (buttonId >= 0 && buttonId <= 15)
                {
                    // Replace this with your actual GPIO set logic using the buttonId
                    gpio.SetOut(buttonId, true);
                }
            }
            else
            {
                buttonOut.Tag = "Off";
                buttonOut.BackColor = buttonColors[buttonOut]["Off"];
                // Add any other actions or logic for Click when the button is in the "Off" state

                // Set GPIO output for the right buttons (assuming button IDs from 0 to 15 are for the right buttons)
                if (buttonId >= 0 && buttonId <= 15)
                {
                    // Replace this with your actual GPIO set logic using the buttonId
                    gpio.SetOut(buttonId, false);
                }
            }
        }
        private void btn_Clicks(object sender, EventArgs e)
        {
            Button buttonsIn = (Button)sender;
            int buttonIdIn = buttonIdIns[buttonsIn];

            if (buttonsIn.Tag.ToString() == "Off")
            {
                buttonsIn.Tag = "On";
                buttonsIn.BackColor = buttonColors2[buttonsIn]["On"];
                Log.Info($"IO_IN {buttonIdIn} Value: {gpio.GetIn(buttonIdIn)}");
                // Add any other actions or logic for Click when the button is in the "On" state
            }
            else
            {
                buttonsIn.Tag = "Off";
                buttonsIn.BackColor = buttonColors2[buttonsIn]["Off"];
                Log.Info($"IO_IN {buttonIdIn} Value: {gpio.GetIn(buttonIdIn)}");
                // Add any other actions or logic for Click when the button is in the "Off" state
            }
        }

        // Initialize the gpioPanels in your form's constructor or Load event
        private void InitializeGpioINPanels()
        {
            gpioPanelsIN = new List<Panel>
    {
        GPIO_IN_0, GPIO_IN_1, GPIO_IN_2, GPIO_IN_3, GPIO_IN_4,
        GPIO_IN_5, GPIO_IN_6, GPIO_IN_7, GPIO_IN_8, GPIO_IN_9,
        GPIO_IN_10, GPIO_IN_11, GPIO_IN_12, GPIO_IN_13, GPIO_IN_14, GPIO_IN_15
    };
        }
        private void InitializeGpioOUTPanels()
        {
            gpioPanelsOUT = new List<Panel>
    {
        GPIO_OUT_0, GPIO_OUT_1, GPIO_OUT_2, GPIO_OUT_3, GPIO_OUT_4,
        GPIO_OUT_5, GPIO_OUT_6, GPIO_OUT_7, GPIO_OUT_8, GPIO_OUT_9,
        GPIO_OUT_10, GPIO_OUT_11, GPIO_OUT_12, GPIO_OUT_13, GPIO_OUT_14, GPIO_OUT_15
    };
        }

        private void UpdateIOIN(int id, Color color)
        {
            gpioPanelsIN[id].BackColor = color;
        }
        private void UpdateIOOUT(int id, Color color)
        {
            gpioPanelsOUT[id].BackColor = color;
        }

        private void UpdateIO_State()
        {
            for (int id = 0; id <= 15; id++)
            {
                Color color = gpio.IO_IN.Get(id) ? Color.Green : Color.Red;
                UpdateIOIN(id, color);
            }
            for (int id = 0; id <= 15; id++)
            {
                Color color = gpio.IO_OUT.Get(id) ? Color.Green : Color.Red;
                UpdateIOOUT(id, color);
            }
        }


        private void Gpio_OUT_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int port = -1;
            if (sender == btn_GPIO_OUT_0)
                // 아무것도 하지 않는다.
                ;
            else if (sender == btn_GPIO_OUT_1)
                port = 1;
            else if (sender == btn_GPIO_OUT_2)
                port = 2;
            else if (sender == btn_GPIO_OUT_3)
                port = 3;
            else if (sender == btn_GPIO_OUT_4)
                port = 4;
            else if (sender == btn_GPIO_OUT_5)
                port = 5;
            else if (sender == btn_GPIO_OUT_6)
                port = 6;
            else if (sender == btn_GPIO_OUT_7)
                port = 7;
            else if (sender == btn_GPIO_OUT_8)
                port = 8;
            else if (sender == btn_GPIO_OUT_8)
                port = 9;
            else if (sender == btn_GPIO_OUT_10)
                port = 10;
            else if (sender == btn_GPIO_OUT_11)
                port = 11;
            else if (sender == btn_GPIO_OUT_12)
                port = 12;
            else if (sender == btn_GPIO_OUT_13)
                port = 13;
            else if (sender == btn_GPIO_OUT_14)
                port = 14;
            else if (sender == btn_GPIO_OUT_15)
                port = 15;
            if (port > -1)
                gpio.SetOut(port, TagToBool(sender,port));
            Log.Debug($"SetOut port: {port} sender : {sender} State : {TagToBool(sender, port)}");
        }
        private bool TagToBool(object sender, int port) 
        {
            Button button = (Button)sender;
            if (button.Tag.ToString() == "ON") { return true; }
            if (button.Tag.ToString() == "OFF") { return false; }
            else { return false; }
        }
        private void gpio_status_timer_Tick(object sender, EventArgs e)
        {
            IO_Update_Timer_Tick();
        }
        private void IO_Update_Timer_Tick() 
        {
            UpdateIO_State();
        }

        private void btn_UpdateUI_Click(object sender, EventArgs e)
        {
            UIUpdate();
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            UIUpdate();
        }
        private void Form1_Close(object sender, FormClosingEventArgs e) 
        {
            IO_Update_Timer.Stop();
            gpio.DeInit();


        }
    }
}





