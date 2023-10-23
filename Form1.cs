using System;
using System.IO;
using System.Windows.Forms;

namespace chip_counter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string ini_filepath = System.IO.Directory.GetCurrentDirectory() + "\\CHIP_COUNTER.INI";
            info.config = new configure(ini_filepath);

        }
        SmdRobotControl control = SmdRobotControl.Instance;
        ChipCounterInfo info = ChipCounterInfo.Instance;
        GPIOProc gpio = GPIOProc.Instance;

        int test = 0;

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

        private void SetTvReady_Click(object sender, EventArgs e)
        {
            control.SetTvReady(true);
        }

        private void SetTvInspectionDone_Click(object sender, EventArgs e)
        {
            control.SetTvInspectionDone(true, false);
        }

        private void SetTvBarcodeOK_Click(object sender, EventArgs e)
        {
            control.SetTvBarcodeOK(true, false);
        }

        private void SetTvBarcodeNG_Click(object sender, EventArgs e)
        {
            control.SetTvBarcodeNG(true, false);
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
            control.SmdBarcodeReadyFunc(1);
        }

        private void SmdPlaceReadyFunc_Click(object sender, EventArgs e)
        {
            control.SmdPlaceReadyFunc(2);
        }

        private void SmdPickupReadyFunc_Click(object sender, EventArgs e)
        {
            control.SmdPickupReadyFunc(3);
        }

        private void SmdResetFunc_Click(object sender, EventArgs e)
        {
            control.SmdResetFunc(4);
        }

        private void SmdRobotInit_Click(object sender, EventArgs e)
        {
            control.SmdRobotInit();
        }

        private void SmdSendInspectionDone_Click(object sender, EventArgs e)
        {
            control.SmdSendInspectionDone(false,true);
        }

        private void SmdSendTvReady_Click(object sender, EventArgs e)
        {
            control.SmdSendTvReady(true);
        }

        private void SmdSendPickupOrReady_Click(object sender, EventArgs e)
        {
            control.SmdSendPickupOrReady(true,true,true);
        }

        private void SmdBarcodeOK_Click(object sender, EventArgs e)
        {
            control.SmdBarcodeOK("OK");
        }
    }
}
