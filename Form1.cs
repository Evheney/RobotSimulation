using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace chip_counter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeButtons();
            string ini_filepath = System.IO.Directory.GetCurrentDirectory() + "\\CHIP_COUNTER.INI";
            info.config = new configure(ini_filepath);

            gpio.Init(info.config);

        }
        //SmdRobotControl control = SmdRobotControl.Instance;
        private bool m_forcibly = false;
        ModSmdRobotControl control = ModSmdRobotControl.Instance;

        ChipCounterInfo info = ChipCounterInfo.Instance;
        GPIOProc gpio = GPIOProc.Instance;

        private Dictionary<Button, int> buttonIds = new Dictionary<Button, int>();
        private Dictionary<Button, Dictionary<string, Color>> buttonColors = new Dictionary<Button, Dictionary<string, Color>>();
        private Dictionary<Button, Dictionary<string, Color>> buttonColors2 = new Dictionary<Button, Dictionary<string, Color>>();
        private Dictionary<Button, Action<object, EventArgs>> buttonActions = new Dictionary<Button, Action<object, EventArgs>>();

        int test = 0;
        int id = 0;

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

        private void InitializeButtons()
        {
            // Associate buttons with their respective actions and images
            // AddButton(buttonName, PerformButtonNameAction, Resources.ButtonNameNormalImage, Resources.ButtonNameHoveredImage, Resources.ButtonNameClickedImage);
            // Associate buttons with their respective actions and images

            //AddButton(buttonName, (sender, e) => PerformButtonNameAction(sender, e), Color.On, Color.OFF);

            AddButton(Btn_Xray, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Start_Lamp, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Return_Lamp, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Input_Stage, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Output_Stage, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Stage_Initialize, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_7th_IO_OUt, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Green_Lamp, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Yellow_Lamp, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Red_Lamp, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(Btn_Tower_Buzz, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(button20, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(button19, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(button18, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(button17, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);
            AddButton(button33, (sender, e) => btn_Click(sender, e), Color.Green, Color.Red);




            AddButtons(btn_System_Power, Color.Green, Color.Red, 0);
            AddButtons(btn_Door_Close, Color.Green, Color.Red, 1);
            AddButtons(btn_Start_Button, Color.Green, Color.Red, 2);
            AddButtons(btn_Return_Button, Color.Green, Color.Red, 3);
            AddButtons(Btn_Stage_In, Color.Green, Color.Red, 4);
            AddButtons(Btn_Stage_Out, Color.Green, Color.Red, 5);
            AddButtons(Btn_Stage_in_MCU, Color.Green, Color.Red, 6);
            AddButtons(Btn_Fan_Sensor, Color.Green, Color.Red, 7);
            AddButtons(Btn_Front_Area, Color.Green, Color.Red, 8);
            AddButtons(Btn_Second_Fan, Color.Green, Color.Red, 9);
            AddButtons(Btn_Emergency, Color.Green, Color.Red, 10);
            AddButtons(Btn_Motor, Color.Green, Color.Red, 11);
            AddButtons(Btn_Reel1, Color.Green, Color.Red, 12);
            AddButtons(Btn_Reel2, Color.Green, Color.Red, 13);
            AddButtons(Btn_Reel3, Color.Green, Color.Red, 14);
            AddButtons(Btn_Reel4, Color.Green, Color.Red, 15);


            // Add more buttons and images as needed
        }

        private void AddButton(Button button, Action<object, EventArgs> action, Color onColor, Color offColor)
        {
            Action<object, EventArgs> p = (sender, e) => action(sender, e);
            buttonActions.Add(button, p);
            buttonIds.Add(button, id);
            id++;
            buttonColors.Add(button, new Dictionary<string, Color>
    {
        { "On", onColor },
        { "Off", offColor }
    });

            button.Tag = "Off"; // Initial state

            button.Click += btn_Click;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int buttonId = buttonIds[button];

            if (button.Tag.ToString() == "Off")
            {
                button.Tag = "On";
                button.BackColor = buttonColors[button]["On"];
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
                button.Tag = "Off";
                button.BackColor = buttonColors[button]["Off"];
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
            Button buttons = (Button)sender;
            int buttonId = buttonIds[buttons];

            if (buttons.Tag.ToString() == "Off")
            {
                buttons.Tag = "On";
                buttons.BackColor = buttonColors2[buttons]["On"];
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
                buttons.Tag = "Off";
                buttons.BackColor = buttonColors2[buttons]["Off"];
                // Add any other actions or logic for Click when the button is in the "Off" state

                // Set GPIO output for the right buttons (assuming button IDs from 0 to 15 are for the right buttons)
                if (buttonId >= 0 && buttonId <= 15)
                {
                    // Replace this with your actual GPIO set logic using the buttonId
                    gpio.SetOut(buttonId, false);
                }
            }
        }
        private void AddButtons(Button button, Color onColor, Color offColor, int buttonId)
        {
            buttonColors2.Add(button, new Dictionary<string, Color>
    {
        { "On", onColor },
        { "Off", offColor }
    });
            buttonIds.Add(button, buttonId);

            button.Tag = "Off"; // Initial state

            button.Click += btn_Clicks;
        }
    }
}





