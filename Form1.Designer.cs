
namespace chip_counter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SetTvReady = new System.Windows.Forms.Button();
            this.SetTvInspectionDone = new System.Windows.Forms.Button();
            this.SetTvBarcodeOK = new System.Windows.Forms.Button();
            this.SetTvBarcodeNG = new System.Windows.Forms.Button();
            this.SetTvReelIsNotRegistered = new System.Windows.Forms.Button();
            this.ResetBarcode = new System.Windows.Forms.Button();
            this.SetIOSettings = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.StageInSend = new System.Windows.Forms.Button();
            this.StageOutSend = new System.Windows.Forms.Button();
            this.SmdBarcodeReadyFunc = new System.Windows.Forms.Button();
            this.SmdPlaceReadyFunc = new System.Windows.Forms.Button();
            this.SmdPickupReadyFunc = new System.Windows.Forms.Button();
            this.SmdResetFunc = new System.Windows.Forms.Button();
            this.SmdRobotInit = new System.Windows.Forms.Button();
            this.SmdSendInspectionDone = new System.Windows.Forms.Button();
            this.SmdSendTvReady = new System.Windows.Forms.Button();
            this.SmdSendPickupOrReady = new System.Windows.Forms.Button();
            this.SmdBarcodeOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.IO_Update_Timer = new System.Windows.Forms.Timer(this.components);
            this.IO_Control = new System.Windows.Forms.TableLayoutPanel();
            this.btn_GPIO_OUT_5 = new System.Windows.Forms.Button();
            this.GPIO_OUT_0 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_1 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_2 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_3 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_4 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_5 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_6 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_7 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_8 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_9 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_10 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_11 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_12 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_13 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_14 = new System.Windows.Forms.Panel();
            this.GPIO_OUT_15 = new System.Windows.Forms.Panel();
            this.GPIO_IN_15 = new System.Windows.Forms.Panel();
            this.GPIO_IN_14 = new System.Windows.Forms.Panel();
            this.GPIO_IN_13 = new System.Windows.Forms.Panel();
            this.GPIO_IN_12 = new System.Windows.Forms.Panel();
            this.GPIO_IN_11 = new System.Windows.Forms.Panel();
            this.GPIO_IN_10 = new System.Windows.Forms.Panel();
            this.GPIO_IN_9 = new System.Windows.Forms.Panel();
            this.GPIO_IN_7 = new System.Windows.Forms.Panel();
            this.GPIO_IN_8 = new System.Windows.Forms.Panel();
            this.GPIO_IN_6 = new System.Windows.Forms.Panel();
            this.GPIO_IN_5 = new System.Windows.Forms.Panel();
            this.GPIO_IN_4 = new System.Windows.Forms.Panel();
            this.GPIO_IN_3 = new System.Windows.Forms.Panel();
            this.GPIO_IN_2 = new System.Windows.Forms.Panel();
            this.GPIO_IN_1 = new System.Windows.Forms.Panel();
            this.GPIO_IN_0 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_GPIO_OUT_0 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_1 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_2 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_3 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_4 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_6 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_7 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_8 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_9 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_10 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_11 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_12 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_13 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_14 = new System.Windows.Forms.Button();
            this.btn_GPIO_OUT_15 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_15 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_14 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_13 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_12 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_11 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_10 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_9 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_8 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_7 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_6 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_4 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_3 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_2 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_1 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_0 = new System.Windows.Forms.Button();
            this.btn_GPIO_IN_5 = new System.Windows.Forms.Button();
            this.btn_UpdateUI = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.IO_Control.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetTvReady
            // 
            this.SetTvReady.Location = new System.Drawing.Point(3, 53);
            this.SetTvReady.Name = "SetTvReady";
            this.SetTvReady.Size = new System.Drawing.Size(164, 36);
            this.SetTvReady.TabIndex = 0;
            this.SetTvReady.Text = "SetTvReady";
            this.SetTvReady.UseVisualStyleBackColor = true;
            this.SetTvReady.Click += new System.EventHandler(this.SetTvReady_Click);
            // 
            // SetTvInspectionDone
            // 
            this.SetTvInspectionDone.Location = new System.Drawing.Point(3, 197);
            this.SetTvInspectionDone.Name = "SetTvInspectionDone";
            this.SetTvInspectionDone.Size = new System.Drawing.Size(164, 36);
            this.SetTvInspectionDone.TabIndex = 1;
            this.SetTvInspectionDone.Text = "SetTvInspectionDone";
            this.SetTvInspectionDone.UseVisualStyleBackColor = true;
            this.SetTvInspectionDone.Click += new System.EventHandler(this.SetTvInspectionDone_Click);
            // 
            // SetTvBarcodeOK
            // 
            this.SetTvBarcodeOK.Location = new System.Drawing.Point(3, 101);
            this.SetTvBarcodeOK.Name = "SetTvBarcodeOK";
            this.SetTvBarcodeOK.Size = new System.Drawing.Size(164, 36);
            this.SetTvBarcodeOK.TabIndex = 2;
            this.SetTvBarcodeOK.Text = "SetTvBarcodeOK";
            this.SetTvBarcodeOK.UseVisualStyleBackColor = true;
            this.SetTvBarcodeOK.Click += new System.EventHandler(this.SetTvBarcodeOK_Click);
            // 
            // SetTvBarcodeNG
            // 
            this.SetTvBarcodeNG.Location = new System.Drawing.Point(3, 149);
            this.SetTvBarcodeNG.Name = "SetTvBarcodeNG";
            this.SetTvBarcodeNG.Size = new System.Drawing.Size(164, 36);
            this.SetTvBarcodeNG.TabIndex = 3;
            this.SetTvBarcodeNG.Text = "SetTvBarcodeNG";
            this.SetTvBarcodeNG.UseVisualStyleBackColor = true;
            this.SetTvBarcodeNG.Click += new System.EventHandler(this.SetTvBarcodeNG_Click);
            // 
            // SetTvReelIsNotRegistered
            // 
            this.SetTvReelIsNotRegistered.Location = new System.Drawing.Point(3, 248);
            this.SetTvReelIsNotRegistered.Name = "SetTvReelIsNotRegistered";
            this.SetTvReelIsNotRegistered.Size = new System.Drawing.Size(164, 36);
            this.SetTvReelIsNotRegistered.TabIndex = 4;
            this.SetTvReelIsNotRegistered.Text = "SetTvReelIsNotRegistered";
            this.SetTvReelIsNotRegistered.UseVisualStyleBackColor = true;
            this.SetTvReelIsNotRegistered.Click += new System.EventHandler(this.SetTvReelIsNotRegistered_Click);
            // 
            // ResetBarcode
            // 
            this.ResetBarcode.Location = new System.Drawing.Point(3, 296);
            this.ResetBarcode.Name = "ResetBarcode";
            this.ResetBarcode.Size = new System.Drawing.Size(164, 36);
            this.ResetBarcode.TabIndex = 5;
            this.ResetBarcode.Text = "ResetBarcode";
            this.ResetBarcode.UseVisualStyleBackColor = true;
            this.ResetBarcode.Click += new System.EventHandler(this.ResetBarcode_Click);
            // 
            // SetIOSettings
            // 
            this.SetIOSettings.Location = new System.Drawing.Point(3, 6);
            this.SetIOSettings.Name = "SetIOSettings";
            this.SetIOSettings.Size = new System.Drawing.Size(164, 36);
            this.SetIOSettings.TabIndex = 6;
            this.SetIOSettings.Text = "SetIOSettings";
            this.SetIOSettings.UseVisualStyleBackColor = true;
            this.SetIOSettings.Click += new System.EventHandler(this.SetIOSettings_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(185, 217);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 7;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // StageInSend
            // 
            this.StageInSend.Location = new System.Drawing.Point(227, 283);
            this.StageInSend.Name = "StageInSend";
            this.StageInSend.Size = new System.Drawing.Size(139, 35);
            this.StageInSend.TabIndex = 8;
            this.StageInSend.Text = "StageInSend";
            this.StageInSend.UseVisualStyleBackColor = true;
            this.StageInSend.Click += new System.EventHandler(this.StageInSend_Click);
            // 
            // StageOutSend
            // 
            this.StageOutSend.Location = new System.Drawing.Point(380, 283);
            this.StageOutSend.Name = "StageOutSend";
            this.StageOutSend.Size = new System.Drawing.Size(139, 35);
            this.StageOutSend.TabIndex = 9;
            this.StageOutSend.Text = "StageOutSend";
            this.StageOutSend.UseVisualStyleBackColor = true;
            this.StageOutSend.Click += new System.EventHandler(this.StageOutSend_Click);
            // 
            // SmdBarcodeReadyFunc
            // 
            this.SmdBarcodeReadyFunc.Location = new System.Drawing.Point(185, 9);
            this.SmdBarcodeReadyFunc.Name = "SmdBarcodeReadyFunc";
            this.SmdBarcodeReadyFunc.Size = new System.Drawing.Size(164, 36);
            this.SmdBarcodeReadyFunc.TabIndex = 10;
            this.SmdBarcodeReadyFunc.Text = "SmdBarcodeReadyFunc";
            this.SmdBarcodeReadyFunc.UseVisualStyleBackColor = true;
            this.SmdBarcodeReadyFunc.Click += new System.EventHandler(this.SmdBarcodeReadyFunc_Click);
            // 
            // SmdPlaceReadyFunc
            // 
            this.SmdPlaceReadyFunc.Location = new System.Drawing.Point(185, 52);
            this.SmdPlaceReadyFunc.Name = "SmdPlaceReadyFunc";
            this.SmdPlaceReadyFunc.Size = new System.Drawing.Size(164, 36);
            this.SmdPlaceReadyFunc.TabIndex = 11;
            this.SmdPlaceReadyFunc.Text = "SmdPlaceReadyFunc";
            this.SmdPlaceReadyFunc.UseVisualStyleBackColor = true;
            this.SmdPlaceReadyFunc.Click += new System.EventHandler(this.SmdPlaceReadyFunc_Click);
            // 
            // SmdPickupReadyFunc
            // 
            this.SmdPickupReadyFunc.Location = new System.Drawing.Point(185, 92);
            this.SmdPickupReadyFunc.Name = "SmdPickupReadyFunc";
            this.SmdPickupReadyFunc.Size = new System.Drawing.Size(164, 36);
            this.SmdPickupReadyFunc.TabIndex = 12;
            this.SmdPickupReadyFunc.Text = "SmdPickupReadyFunc";
            this.SmdPickupReadyFunc.UseVisualStyleBackColor = true;
            this.SmdPickupReadyFunc.Click += new System.EventHandler(this.SmdPickupReadyFunc_Click);
            // 
            // SmdResetFunc
            // 
            this.SmdResetFunc.Location = new System.Drawing.Point(185, 133);
            this.SmdResetFunc.Name = "SmdResetFunc";
            this.SmdResetFunc.Size = new System.Drawing.Size(164, 36);
            this.SmdResetFunc.TabIndex = 13;
            this.SmdResetFunc.Text = "SmdResetFunc";
            this.SmdResetFunc.UseVisualStyleBackColor = true;
            this.SmdResetFunc.Click += new System.EventHandler(this.SmdResetFunc_Click);
            // 
            // SmdRobotInit
            // 
            this.SmdRobotInit.Location = new System.Drawing.Point(360, 9);
            this.SmdRobotInit.Name = "SmdRobotInit";
            this.SmdRobotInit.Size = new System.Drawing.Size(164, 36);
            this.SmdRobotInit.TabIndex = 14;
            this.SmdRobotInit.Text = "SmdRobotInit";
            this.SmdRobotInit.UseVisualStyleBackColor = true;
            this.SmdRobotInit.Click += new System.EventHandler(this.SmdRobotInit_Click);
            // 
            // SmdSendInspectionDone
            // 
            this.SmdSendInspectionDone.Location = new System.Drawing.Point(360, 52);
            this.SmdSendInspectionDone.Name = "SmdSendInspectionDone";
            this.SmdSendInspectionDone.Size = new System.Drawing.Size(164, 36);
            this.SmdSendInspectionDone.TabIndex = 15;
            this.SmdSendInspectionDone.Text = "SmdSendInspectionDone";
            this.SmdSendInspectionDone.UseVisualStyleBackColor = true;
            this.SmdSendInspectionDone.Click += new System.EventHandler(this.SmdSendInspectionDone_Click);
            // 
            // SmdSendTvReady
            // 
            this.SmdSendTvReady.Location = new System.Drawing.Point(360, 93);
            this.SmdSendTvReady.Name = "SmdSendTvReady";
            this.SmdSendTvReady.Size = new System.Drawing.Size(164, 36);
            this.SmdSendTvReady.TabIndex = 16;
            this.SmdSendTvReady.Text = "SmdSendTvReady";
            this.SmdSendTvReady.UseVisualStyleBackColor = true;
            this.SmdSendTvReady.Click += new System.EventHandler(this.SmdSendTvReady_Click);
            // 
            // SmdSendPickupOrReady
            // 
            this.SmdSendPickupOrReady.Location = new System.Drawing.Point(360, 133);
            this.SmdSendPickupOrReady.Name = "SmdSendPickupOrReady";
            this.SmdSendPickupOrReady.Size = new System.Drawing.Size(164, 36);
            this.SmdSendPickupOrReady.TabIndex = 17;
            this.SmdSendPickupOrReady.Text = "SmdSendPickupOrReady";
            this.SmdSendPickupOrReady.UseVisualStyleBackColor = true;
            this.SmdSendPickupOrReady.Click += new System.EventHandler(this.SmdSendPickupOrReady_Click);
            // 
            // SmdBarcodeOK
            // 
            this.SmdBarcodeOK.Location = new System.Drawing.Point(360, 174);
            this.SmdBarcodeOK.Name = "SmdBarcodeOK";
            this.SmdBarcodeOK.Size = new System.Drawing.Size(164, 36);
            this.SmdBarcodeOK.TabIndex = 18;
            this.SmdBarcodeOK.Text = "SmdBarcodeOK";
            this.SmdBarcodeOK.UseVisualStyleBackColor = true;
            this.SmdBarcodeOK.Click += new System.EventHandler(this.SmdBarcodeOK_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label33);
            this.panel1.Controls.Add(this.SmdPickupReadyFunc);
            this.panel1.Controls.Add(this.SetTvReady);
            this.panel1.Controls.Add(this.StageOutSend);
            this.panel1.Controls.Add(this.SmdBarcodeOK);
            this.panel1.Controls.Add(this.StageInSend);
            this.panel1.Controls.Add(this.SetTvInspectionDone);
            this.panel1.Controls.Add(this.SmdSendPickupOrReady);
            this.panel1.Controls.Add(this.SetTvBarcodeOK);
            this.panel1.Controls.Add(this.SmdSendTvReady);
            this.panel1.Controls.Add(this.SetTvBarcodeNG);
            this.panel1.Controls.Add(this.SmdSendInspectionDone);
            this.panel1.Controls.Add(this.SetTvReelIsNotRegistered);
            this.panel1.Controls.Add(this.SmdRobotInit);
            this.panel1.Controls.Add(this.ResetBarcode);
            this.panel1.Controls.Add(this.SmdResetFunc);
            this.panel1.Controls.Add(this.SetIOSettings);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.SmdPlaceReadyFunc);
            this.panel1.Controls.Add(this.SmdBarcodeReadyFunc);
            this.panel1.Location = new System.Drawing.Point(3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(523, 376);
            this.panel1.TabIndex = 21;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(283, 347);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(83, 15);
            this.label33.TabIndex = 0;
            this.label33.Text = "Robot Control";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(438, 394);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(83, 15);
            this.label34.TabIndex = 22;
            this.label34.Text = "IO Control =>";
            // 
            // IO_Update_Timer
            // 
            this.IO_Update_Timer.Enabled = true;
            this.IO_Update_Timer.Interval = 1000;
            this.IO_Update_Timer.Tick += new System.EventHandler(this.gpio_status_timer_Tick);
            // 
            // IO_Control
            // 
            this.IO_Control.ColumnCount = 4;
            this.IO_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.IO_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.IO_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.IO_Control.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_5, 3, 6);
            this.IO_Control.Controls.Add(this.GPIO_OUT_0, 2, 1);
            this.IO_Control.Controls.Add(this.GPIO_OUT_1, 2, 2);
            this.IO_Control.Controls.Add(this.GPIO_OUT_2, 2, 3);
            this.IO_Control.Controls.Add(this.GPIO_OUT_3, 2, 4);
            this.IO_Control.Controls.Add(this.GPIO_OUT_4, 2, 5);
            this.IO_Control.Controls.Add(this.GPIO_OUT_5, 2, 6);
            this.IO_Control.Controls.Add(this.GPIO_OUT_6, 2, 7);
            this.IO_Control.Controls.Add(this.GPIO_OUT_7, 2, 8);
            this.IO_Control.Controls.Add(this.GPIO_OUT_8, 2, 9);
            this.IO_Control.Controls.Add(this.GPIO_OUT_9, 2, 10);
            this.IO_Control.Controls.Add(this.GPIO_OUT_10, 2, 11);
            this.IO_Control.Controls.Add(this.GPIO_OUT_11, 2, 12);
            this.IO_Control.Controls.Add(this.GPIO_OUT_12, 2, 13);
            this.IO_Control.Controls.Add(this.GPIO_OUT_13, 2, 14);
            this.IO_Control.Controls.Add(this.GPIO_OUT_14, 2, 15);
            this.IO_Control.Controls.Add(this.GPIO_OUT_15, 2, 16);
            this.IO_Control.Controls.Add(this.GPIO_IN_15, 0, 16);
            this.IO_Control.Controls.Add(this.GPIO_IN_14, 0, 15);
            this.IO_Control.Controls.Add(this.GPIO_IN_13, 0, 14);
            this.IO_Control.Controls.Add(this.GPIO_IN_12, 0, 13);
            this.IO_Control.Controls.Add(this.GPIO_IN_11, 0, 12);
            this.IO_Control.Controls.Add(this.GPIO_IN_10, 0, 11);
            this.IO_Control.Controls.Add(this.GPIO_IN_9, 0, 10);
            this.IO_Control.Controls.Add(this.GPIO_IN_7, 0, 8);
            this.IO_Control.Controls.Add(this.GPIO_IN_8, 0, 9);
            this.IO_Control.Controls.Add(this.GPIO_IN_6, 0, 7);
            this.IO_Control.Controls.Add(this.GPIO_IN_5, 0, 6);
            this.IO_Control.Controls.Add(this.GPIO_IN_4, 0, 5);
            this.IO_Control.Controls.Add(this.GPIO_IN_3, 0, 4);
            this.IO_Control.Controls.Add(this.GPIO_IN_2, 0, 3);
            this.IO_Control.Controls.Add(this.GPIO_IN_1, 0, 2);
            this.IO_Control.Controls.Add(this.GPIO_IN_0, 0, 1);
            this.IO_Control.Controls.Add(this.label4, 3, 0);
            this.IO_Control.Controls.Add(this.label3, 1, 0);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_0, 3, 1);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_1, 3, 2);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_2, 3, 3);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_3, 3, 4);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_4, 3, 5);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_6, 3, 7);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_7, 3, 8);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_8, 3, 9);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_9, 3, 10);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_10, 3, 11);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_11, 3, 12);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_12, 3, 13);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_13, 3, 14);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_14, 3, 15);
            this.IO_Control.Controls.Add(this.btn_GPIO_OUT_15, 3, 16);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_15, 1, 16);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_14, 1, 15);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_13, 1, 14);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_12, 1, 13);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_11, 1, 12);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_10, 1, 11);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_9, 1, 10);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_8, 1, 9);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_7, 1, 8);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_6, 1, 7);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_4, 1, 5);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_3, 1, 4);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_2, 1, 3);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_1, 1, 2);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_0, 1, 1);
            this.IO_Control.Controls.Add(this.btn_GPIO_IN_5, 1, 6);
            this.IO_Control.Location = new System.Drawing.Point(532, 1);
            this.IO_Control.Name = "IO_Control";
            this.IO_Control.RowCount = 17;
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.IO_Control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.IO_Control.Size = new System.Drawing.Size(297, 442);
            this.IO_Control.TabIndex = 25;
            // 
            // btn_GPIO_OUT_5
            // 
            this.btn_GPIO_OUT_5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_5.Location = new System.Drawing.Point(179, 159);
            this.btn_GPIO_OUT_5.Name = "btn_GPIO_OUT_5";
            this.btn_GPIO_OUT_5.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_5.TabIndex = 46;
            this.btn_GPIO_OUT_5.Text = "Output Stage";
            this.btn_GPIO_OUT_5.UseVisualStyleBackColor = true;
            // 
            // GPIO_OUT_0
            // 
            this.GPIO_OUT_0.Location = new System.Drawing.Point(150, 29);
            this.GPIO_OUT_0.Name = "GPIO_OUT_0";
            this.GPIO_OUT_0.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_0.TabIndex = 44;
            // 
            // GPIO_OUT_1
            // 
            this.GPIO_OUT_1.Location = new System.Drawing.Point(150, 55);
            this.GPIO_OUT_1.Name = "GPIO_OUT_1";
            this.GPIO_OUT_1.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_1.TabIndex = 43;
            // 
            // GPIO_OUT_2
            // 
            this.GPIO_OUT_2.Location = new System.Drawing.Point(150, 81);
            this.GPIO_OUT_2.Name = "GPIO_OUT_2";
            this.GPIO_OUT_2.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_2.TabIndex = 42;
            // 
            // GPIO_OUT_3
            // 
            this.GPIO_OUT_3.Location = new System.Drawing.Point(150, 107);
            this.GPIO_OUT_3.Name = "GPIO_OUT_3";
            this.GPIO_OUT_3.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_3.TabIndex = 41;
            // 
            // GPIO_OUT_4
            // 
            this.GPIO_OUT_4.Location = new System.Drawing.Point(150, 133);
            this.GPIO_OUT_4.Name = "GPIO_OUT_4";
            this.GPIO_OUT_4.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_4.TabIndex = 40;
            // 
            // GPIO_OUT_5
            // 
            this.GPIO_OUT_5.Location = new System.Drawing.Point(150, 159);
            this.GPIO_OUT_5.Name = "GPIO_OUT_5";
            this.GPIO_OUT_5.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_5.TabIndex = 39;
            // 
            // GPIO_OUT_6
            // 
            this.GPIO_OUT_6.Location = new System.Drawing.Point(150, 185);
            this.GPIO_OUT_6.Name = "GPIO_OUT_6";
            this.GPIO_OUT_6.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_6.TabIndex = 38;
            // 
            // GPIO_OUT_7
            // 
            this.GPIO_OUT_7.Location = new System.Drawing.Point(150, 211);
            this.GPIO_OUT_7.Name = "GPIO_OUT_7";
            this.GPIO_OUT_7.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_7.TabIndex = 37;
            // 
            // GPIO_OUT_8
            // 
            this.GPIO_OUT_8.Location = new System.Drawing.Point(150, 237);
            this.GPIO_OUT_8.Name = "GPIO_OUT_8";
            this.GPIO_OUT_8.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_8.TabIndex = 36;
            // 
            // GPIO_OUT_9
            // 
            this.GPIO_OUT_9.Location = new System.Drawing.Point(150, 263);
            this.GPIO_OUT_9.Name = "GPIO_OUT_9";
            this.GPIO_OUT_9.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_9.TabIndex = 35;
            // 
            // GPIO_OUT_10
            // 
            this.GPIO_OUT_10.Location = new System.Drawing.Point(150, 289);
            this.GPIO_OUT_10.Name = "GPIO_OUT_10";
            this.GPIO_OUT_10.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_10.TabIndex = 34;
            // 
            // GPIO_OUT_11
            // 
            this.GPIO_OUT_11.Location = new System.Drawing.Point(150, 315);
            this.GPIO_OUT_11.Name = "GPIO_OUT_11";
            this.GPIO_OUT_11.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_11.TabIndex = 33;
            // 
            // GPIO_OUT_12
            // 
            this.GPIO_OUT_12.Location = new System.Drawing.Point(150, 341);
            this.GPIO_OUT_12.Name = "GPIO_OUT_12";
            this.GPIO_OUT_12.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_12.TabIndex = 32;
            // 
            // GPIO_OUT_13
            // 
            this.GPIO_OUT_13.Location = new System.Drawing.Point(150, 367);
            this.GPIO_OUT_13.Name = "GPIO_OUT_13";
            this.GPIO_OUT_13.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_13.TabIndex = 31;
            // 
            // GPIO_OUT_14
            // 
            this.GPIO_OUT_14.Location = new System.Drawing.Point(150, 393);
            this.GPIO_OUT_14.Name = "GPIO_OUT_14";
            this.GPIO_OUT_14.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_14.TabIndex = 30;
            // 
            // GPIO_OUT_15
            // 
            this.GPIO_OUT_15.Location = new System.Drawing.Point(150, 419);
            this.GPIO_OUT_15.Name = "GPIO_OUT_15";
            this.GPIO_OUT_15.Size = new System.Drawing.Size(23, 20);
            this.GPIO_OUT_15.TabIndex = 29;
            // 
            // GPIO_IN_15
            // 
            this.GPIO_IN_15.Location = new System.Drawing.Point(3, 419);
            this.GPIO_IN_15.Name = "GPIO_IN_15";
            this.GPIO_IN_15.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_15.TabIndex = 28;
            // 
            // GPIO_IN_14
            // 
            this.GPIO_IN_14.Location = new System.Drawing.Point(3, 393);
            this.GPIO_IN_14.Name = "GPIO_IN_14";
            this.GPIO_IN_14.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_14.TabIndex = 28;
            // 
            // GPIO_IN_13
            // 
            this.GPIO_IN_13.Location = new System.Drawing.Point(3, 367);
            this.GPIO_IN_13.Name = "GPIO_IN_13";
            this.GPIO_IN_13.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_13.TabIndex = 28;
            // 
            // GPIO_IN_12
            // 
            this.GPIO_IN_12.Location = new System.Drawing.Point(3, 341);
            this.GPIO_IN_12.Name = "GPIO_IN_12";
            this.GPIO_IN_12.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_12.TabIndex = 28;
            // 
            // GPIO_IN_11
            // 
            this.GPIO_IN_11.Location = new System.Drawing.Point(3, 315);
            this.GPIO_IN_11.Name = "GPIO_IN_11";
            this.GPIO_IN_11.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_11.TabIndex = 28;
            // 
            // GPIO_IN_10
            // 
            this.GPIO_IN_10.Location = new System.Drawing.Point(3, 289);
            this.GPIO_IN_10.Name = "GPIO_IN_10";
            this.GPIO_IN_10.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_10.TabIndex = 28;
            // 
            // GPIO_IN_9
            // 
            this.GPIO_IN_9.Location = new System.Drawing.Point(3, 263);
            this.GPIO_IN_9.Name = "GPIO_IN_9";
            this.GPIO_IN_9.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_9.TabIndex = 28;
            // 
            // GPIO_IN_7
            // 
            this.GPIO_IN_7.Location = new System.Drawing.Point(3, 211);
            this.GPIO_IN_7.Name = "GPIO_IN_7";
            this.GPIO_IN_7.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_7.TabIndex = 28;
            // 
            // GPIO_IN_8
            // 
            this.GPIO_IN_8.Location = new System.Drawing.Point(3, 237);
            this.GPIO_IN_8.Name = "GPIO_IN_8";
            this.GPIO_IN_8.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_8.TabIndex = 28;
            // 
            // GPIO_IN_6
            // 
            this.GPIO_IN_6.Location = new System.Drawing.Point(3, 185);
            this.GPIO_IN_6.Name = "GPIO_IN_6";
            this.GPIO_IN_6.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_6.TabIndex = 28;
            // 
            // GPIO_IN_5
            // 
            this.GPIO_IN_5.Location = new System.Drawing.Point(3, 159);
            this.GPIO_IN_5.Name = "GPIO_IN_5";
            this.GPIO_IN_5.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_5.TabIndex = 28;
            // 
            // GPIO_IN_4
            // 
            this.GPIO_IN_4.Location = new System.Drawing.Point(3, 133);
            this.GPIO_IN_4.Name = "GPIO_IN_4";
            this.GPIO_IN_4.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_4.TabIndex = 28;
            // 
            // GPIO_IN_3
            // 
            this.GPIO_IN_3.Location = new System.Drawing.Point(3, 107);
            this.GPIO_IN_3.Name = "GPIO_IN_3";
            this.GPIO_IN_3.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_3.TabIndex = 28;
            // 
            // GPIO_IN_2
            // 
            this.GPIO_IN_2.Location = new System.Drawing.Point(3, 81);
            this.GPIO_IN_2.Name = "GPIO_IN_2";
            this.GPIO_IN_2.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_2.TabIndex = 28;
            // 
            // GPIO_IN_1
            // 
            this.GPIO_IN_1.Location = new System.Drawing.Point(3, 55);
            this.GPIO_IN_1.Name = "GPIO_IN_1";
            this.GPIO_IN_1.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_1.TabIndex = 28;
            // 
            // GPIO_IN_0
            // 
            this.GPIO_IN_0.Location = new System.Drawing.Point(3, 29);
            this.GPIO_IN_0.Name = "GPIO_IN_0";
            this.GPIO_IN_0.Size = new System.Drawing.Size(23, 20);
            this.GPIO_IN_0.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(179, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 15);
            this.label4.TabIndex = 26;
            this.label4.Text = "OUT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "IN";
            // 
            // btn_GPIO_OUT_0
            // 
            this.btn_GPIO_OUT_0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_0.Location = new System.Drawing.Point(179, 29);
            this.btn_GPIO_OUT_0.Name = "btn_GPIO_OUT_0";
            this.btn_GPIO_OUT_0.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_0.TabIndex = 45;
            this.btn_GPIO_OUT_0.Text = "X-Ray";
            this.btn_GPIO_OUT_0.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_1
            // 
            this.btn_GPIO_OUT_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_1.Location = new System.Drawing.Point(179, 55);
            this.btn_GPIO_OUT_1.Name = "btn_GPIO_OUT_1";
            this.btn_GPIO_OUT_1.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_1.TabIndex = 46;
            this.btn_GPIO_OUT_1.Text = "1_IO_OUT";
            this.btn_GPIO_OUT_1.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_2
            // 
            this.btn_GPIO_OUT_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_2.Location = new System.Drawing.Point(179, 81);
            this.btn_GPIO_OUT_2.Name = "btn_GPIO_OUT_2";
            this.btn_GPIO_OUT_2.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_2.TabIndex = 47;
            this.btn_GPIO_OUT_2.Text = "Start_Lamp";
            this.btn_GPIO_OUT_2.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_3
            // 
            this.btn_GPIO_OUT_3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_3.Location = new System.Drawing.Point(179, 107);
            this.btn_GPIO_OUT_3.Name = "btn_GPIO_OUT_3";
            this.btn_GPIO_OUT_3.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_3.TabIndex = 48;
            this.btn_GPIO_OUT_3.Text = "Return Lamp";
            this.btn_GPIO_OUT_3.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_4
            // 
            this.btn_GPIO_OUT_4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_4.Location = new System.Drawing.Point(179, 133);
            this.btn_GPIO_OUT_4.Name = "btn_GPIO_OUT_4";
            this.btn_GPIO_OUT_4.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_4.TabIndex = 49;
            this.btn_GPIO_OUT_4.Text = "Input Stage";
            this.btn_GPIO_OUT_4.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_6
            // 
            this.btn_GPIO_OUT_6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_6.Location = new System.Drawing.Point(179, 185);
            this.btn_GPIO_OUT_6.Name = "btn_GPIO_OUT_6";
            this.btn_GPIO_OUT_6.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_6.TabIndex = 50;
            this.btn_GPIO_OUT_6.Text = "Stage Initialize";
            this.btn_GPIO_OUT_6.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_7
            // 
            this.btn_GPIO_OUT_7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_7.Location = new System.Drawing.Point(179, 211);
            this.btn_GPIO_OUT_7.Name = "btn_GPIO_OUT_7";
            this.btn_GPIO_OUT_7.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_7.TabIndex = 51;
            this.btn_GPIO_OUT_7.Text = "7TH_IO_OUT";
            this.btn_GPIO_OUT_7.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_8
            // 
            this.btn_GPIO_OUT_8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_8.Location = new System.Drawing.Point(179, 237);
            this.btn_GPIO_OUT_8.Name = "btn_GPIO_OUT_8";
            this.btn_GPIO_OUT_8.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_8.TabIndex = 52;
            this.btn_GPIO_OUT_8.Text = "Green Lamp";
            this.btn_GPIO_OUT_8.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_9
            // 
            this.btn_GPIO_OUT_9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_9.Location = new System.Drawing.Point(179, 263);
            this.btn_GPIO_OUT_9.Name = "btn_GPIO_OUT_9";
            this.btn_GPIO_OUT_9.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_9.TabIndex = 53;
            this.btn_GPIO_OUT_9.Text = "Yellow Lamp";
            this.btn_GPIO_OUT_9.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_10
            // 
            this.btn_GPIO_OUT_10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_10.Location = new System.Drawing.Point(179, 289);
            this.btn_GPIO_OUT_10.Name = "btn_GPIO_OUT_10";
            this.btn_GPIO_OUT_10.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_10.TabIndex = 54;
            this.btn_GPIO_OUT_10.Text = "Red Lamp";
            this.btn_GPIO_OUT_10.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_11
            // 
            this.btn_GPIO_OUT_11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_11.Location = new System.Drawing.Point(179, 315);
            this.btn_GPIO_OUT_11.Name = "btn_GPIO_OUT_11";
            this.btn_GPIO_OUT_11.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_11.TabIndex = 55;
            this.btn_GPIO_OUT_11.Text = "Tower  Buzz";
            this.btn_GPIO_OUT_11.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_12
            // 
            this.btn_GPIO_OUT_12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_12.Location = new System.Drawing.Point(179, 341);
            this.btn_GPIO_OUT_12.Name = "btn_GPIO_OUT_12";
            this.btn_GPIO_OUT_12.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_12.TabIndex = 56;
            this.btn_GPIO_OUT_12.Text = "12TH_IO_OUT";
            this.btn_GPIO_OUT_12.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_13
            // 
            this.btn_GPIO_OUT_13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_13.Location = new System.Drawing.Point(179, 367);
            this.btn_GPIO_OUT_13.Name = "btn_GPIO_OUT_13";
            this.btn_GPIO_OUT_13.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_13.TabIndex = 57;
            this.btn_GPIO_OUT_13.Text = "13TH_IO_OUT";
            this.btn_GPIO_OUT_13.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_14
            // 
            this.btn_GPIO_OUT_14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_14.Location = new System.Drawing.Point(179, 393);
            this.btn_GPIO_OUT_14.Name = "btn_GPIO_OUT_14";
            this.btn_GPIO_OUT_14.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_14.TabIndex = 58;
            this.btn_GPIO_OUT_14.Text = "14TH_IO_OUT";
            this.btn_GPIO_OUT_14.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_OUT_15
            // 
            this.btn_GPIO_OUT_15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_GPIO_OUT_15.Location = new System.Drawing.Point(179, 419);
            this.btn_GPIO_OUT_15.Name = "btn_GPIO_OUT_15";
            this.btn_GPIO_OUT_15.Size = new System.Drawing.Size(115, 20);
            this.btn_GPIO_OUT_15.TabIndex = 59;
            this.btn_GPIO_OUT_15.Text = "15TH_IO_OUT";
            this.btn_GPIO_OUT_15.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_15
            // 
            this.btn_GPIO_IN_15.Location = new System.Drawing.Point(32, 419);
            this.btn_GPIO_IN_15.Name = "btn_GPIO_IN_15";
            this.btn_GPIO_IN_15.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_15.TabIndex = 75;
            this.btn_GPIO_IN_15.Text = "Reel 4 Sensor";
            this.btn_GPIO_IN_15.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_14
            // 
            this.btn_GPIO_IN_14.Location = new System.Drawing.Point(32, 393);
            this.btn_GPIO_IN_14.Name = "btn_GPIO_IN_14";
            this.btn_GPIO_IN_14.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_14.TabIndex = 74;
            this.btn_GPIO_IN_14.Text = "Reel 3 Sensor";
            this.btn_GPIO_IN_14.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_13
            // 
            this.btn_GPIO_IN_13.Location = new System.Drawing.Point(32, 367);
            this.btn_GPIO_IN_13.Name = "btn_GPIO_IN_13";
            this.btn_GPIO_IN_13.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_13.TabIndex = 73;
            this.btn_GPIO_IN_13.Text = "Reel 2 Sensor";
            this.btn_GPIO_IN_13.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_12
            // 
            this.btn_GPIO_IN_12.Location = new System.Drawing.Point(32, 341);
            this.btn_GPIO_IN_12.Name = "btn_GPIO_IN_12";
            this.btn_GPIO_IN_12.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_12.TabIndex = 72;
            this.btn_GPIO_IN_12.Text = "Reel 1 Sensor";
            this.btn_GPIO_IN_12.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_11
            // 
            this.btn_GPIO_IN_11.Location = new System.Drawing.Point(32, 315);
            this.btn_GPIO_IN_11.Name = "btn_GPIO_IN_11";
            this.btn_GPIO_IN_11.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_11.TabIndex = 71;
            this.btn_GPIO_IN_11.Text = "Motor Status";
            this.btn_GPIO_IN_11.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_10
            // 
            this.btn_GPIO_IN_10.Location = new System.Drawing.Point(32, 289);
            this.btn_GPIO_IN_10.Name = "btn_GPIO_IN_10";
            this.btn_GPIO_IN_10.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_10.TabIndex = 70;
            this.btn_GPIO_IN_10.Text = "Emergency";
            this.btn_GPIO_IN_10.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_9
            // 
            this.btn_GPIO_IN_9.Location = new System.Drawing.Point(32, 263);
            this.btn_GPIO_IN_9.Name = "btn_GPIO_IN_9";
            this.btn_GPIO_IN_9.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_9.TabIndex = 69;
            this.btn_GPIO_IN_9.Text = "Second Fan";
            this.btn_GPIO_IN_9.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_8
            // 
            this.btn_GPIO_IN_8.Location = new System.Drawing.Point(32, 237);
            this.btn_GPIO_IN_8.Name = "btn_GPIO_IN_8";
            this.btn_GPIO_IN_8.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_8.TabIndex = 68;
            this.btn_GPIO_IN_8.Text = "Front Area";
            this.btn_GPIO_IN_8.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_7
            // 
            this.btn_GPIO_IN_7.Location = new System.Drawing.Point(32, 211);
            this.btn_GPIO_IN_7.Name = "btn_GPIO_IN_7";
            this.btn_GPIO_IN_7.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_7.TabIndex = 67;
            this.btn_GPIO_IN_7.Text = "Fan Sensor";
            this.btn_GPIO_IN_7.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_6
            // 
            this.btn_GPIO_IN_6.Location = new System.Drawing.Point(32, 185);
            this.btn_GPIO_IN_6.Name = "btn_GPIO_IN_6";
            this.btn_GPIO_IN_6.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_6.TabIndex = 66;
            this.btn_GPIO_IN_6.Text = "StageInMCU";
            this.btn_GPIO_IN_6.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_4
            // 
            this.btn_GPIO_IN_4.Location = new System.Drawing.Point(32, 133);
            this.btn_GPIO_IN_4.Name = "btn_GPIO_IN_4";
            this.btn_GPIO_IN_4.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_4.TabIndex = 65;
            this.btn_GPIO_IN_4.Text = "StageINSensor";
            this.btn_GPIO_IN_4.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_3
            // 
            this.btn_GPIO_IN_3.Location = new System.Drawing.Point(32, 107);
            this.btn_GPIO_IN_3.Name = "btn_GPIO_IN_3";
            this.btn_GPIO_IN_3.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_3.TabIndex = 64;
            this.btn_GPIO_IN_3.Text = "Return Button";
            this.btn_GPIO_IN_3.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_2
            // 
            this.btn_GPIO_IN_2.Location = new System.Drawing.Point(32, 81);
            this.btn_GPIO_IN_2.Name = "btn_GPIO_IN_2";
            this.btn_GPIO_IN_2.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_2.TabIndex = 63;
            this.btn_GPIO_IN_2.Text = "Start_Button";
            this.btn_GPIO_IN_2.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_1
            // 
            this.btn_GPIO_IN_1.Location = new System.Drawing.Point(32, 55);
            this.btn_GPIO_IN_1.Name = "btn_GPIO_IN_1";
            this.btn_GPIO_IN_1.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_1.TabIndex = 62;
            this.btn_GPIO_IN_1.Text = "Door Close";
            this.btn_GPIO_IN_1.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_0
            // 
            this.btn_GPIO_IN_0.Location = new System.Drawing.Point(32, 29);
            this.btn_GPIO_IN_0.Name = "btn_GPIO_IN_0";
            this.btn_GPIO_IN_0.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_0.TabIndex = 60;
            this.btn_GPIO_IN_0.Text = "System Power";
            this.btn_GPIO_IN_0.UseVisualStyleBackColor = true;
            // 
            // btn_GPIO_IN_5
            // 
            this.btn_GPIO_IN_5.Location = new System.Drawing.Point(32, 159);
            this.btn_GPIO_IN_5.Name = "btn_GPIO_IN_5";
            this.btn_GPIO_IN_5.Size = new System.Drawing.Size(112, 20);
            this.btn_GPIO_IN_5.TabIndex = 61;
            this.btn_GPIO_IN_5.Text = "StageOUTSensor";
            this.btn_GPIO_IN_5.UseVisualStyleBackColor = true;
            // 
            // btn_UpdateUI
            // 
            this.btn_UpdateUI.Location = new System.Drawing.Point(378, 408);
            this.btn_UpdateUI.Name = "btn_UpdateUI";
            this.btn_UpdateUI.Size = new System.Drawing.Size(75, 23);
            this.btn_UpdateUI.TabIndex = 26;
            this.btn_UpdateUI.Text = "UpdateUi";
            this.btn_UpdateUI.UseVisualStyleBackColor = true;
            this.btn_UpdateUI.Click += new System.EventHandler(this.btn_UpdateUI_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 455);
            this.Controls.Add(this.btn_UpdateUI);
            this.Controls.Add(this.IO_Control);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Close);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.IO_Control.ResumeLayout(false);
            this.IO_Control.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SetTvReady;
        private System.Windows.Forms.Button SetTvInspectionDone;
        private System.Windows.Forms.Button SetTvBarcodeOK;
        private System.Windows.Forms.Button SetTvBarcodeNG;
        private System.Windows.Forms.Button SetTvReelIsNotRegistered;
        private System.Windows.Forms.Button ResetBarcode;
        private System.Windows.Forms.Button SetIOSettings;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button StageInSend;
        private System.Windows.Forms.Button StageOutSend;
        private System.Windows.Forms.Button SmdBarcodeReadyFunc;
        private System.Windows.Forms.Button SmdPlaceReadyFunc;
        private System.Windows.Forms.Button SmdPickupReadyFunc;
        private System.Windows.Forms.Button SmdResetFunc;
        private System.Windows.Forms.Button SmdRobotInit;
        private System.Windows.Forms.Button SmdSendInspectionDone;
        private System.Windows.Forms.Button SmdSendTvReady;
        private System.Windows.Forms.Button SmdSendPickupOrReady;
        private System.Windows.Forms.Button SmdBarcodeOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Timer IO_Update_Timer;
        private System.Windows.Forms.TableLayoutPanel IO_Control;
        private System.Windows.Forms.Panel GPIO_IN_15;
        private System.Windows.Forms.Panel GPIO_IN_14;
        private System.Windows.Forms.Panel GPIO_IN_13;
        private System.Windows.Forms.Panel GPIO_IN_12;
        private System.Windows.Forms.Panel GPIO_IN_11;
        private System.Windows.Forms.Panel GPIO_IN_10;
        private System.Windows.Forms.Panel GPIO_IN_9;
        private System.Windows.Forms.Panel GPIO_IN_7;
        private System.Windows.Forms.Panel GPIO_IN_8;
        private System.Windows.Forms.Panel GPIO_IN_6;
        private System.Windows.Forms.Panel GPIO_IN_5;
        private System.Windows.Forms.Panel GPIO_IN_4;
        private System.Windows.Forms.Panel GPIO_IN_3;
        private System.Windows.Forms.Panel GPIO_IN_2;
        private System.Windows.Forms.Panel GPIO_IN_1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel GPIO_IN_0;
        private System.Windows.Forms.Panel GPIO_OUT_0;
        private System.Windows.Forms.Panel GPIO_OUT_1;
        private System.Windows.Forms.Panel GPIO_OUT_2;
        private System.Windows.Forms.Panel GPIO_OUT_3;
        private System.Windows.Forms.Panel GPIO_OUT_4;
        private System.Windows.Forms.Panel GPIO_OUT_5;
        private System.Windows.Forms.Panel GPIO_OUT_6;
        private System.Windows.Forms.Panel GPIO_OUT_7;
        private System.Windows.Forms.Panel GPIO_OUT_8;
        private System.Windows.Forms.Panel GPIO_OUT_9;
        private System.Windows.Forms.Panel GPIO_OUT_10;
        private System.Windows.Forms.Panel GPIO_OUT_11;
        private System.Windows.Forms.Panel GPIO_OUT_12;
        private System.Windows.Forms.Panel GPIO_OUT_13;
        private System.Windows.Forms.Panel GPIO_OUT_14;
        private System.Windows.Forms.Panel GPIO_OUT_15;
        private System.Windows.Forms.Button btn_GPIO_OUT_5;
        private System.Windows.Forms.Button btn_GPIO_OUT_0;
        private System.Windows.Forms.Button btn_GPIO_OUT_1;
        private System.Windows.Forms.Button btn_GPIO_OUT_2;
        private System.Windows.Forms.Button btn_GPIO_OUT_3;
        private System.Windows.Forms.Button btn_GPIO_OUT_4;
        private System.Windows.Forms.Button btn_GPIO_OUT_6;
        private System.Windows.Forms.Button btn_GPIO_OUT_7;
        private System.Windows.Forms.Button btn_GPIO_OUT_8;
        private System.Windows.Forms.Button btn_GPIO_OUT_9;
        private System.Windows.Forms.Button btn_GPIO_OUT_10;
        private System.Windows.Forms.Button btn_GPIO_OUT_11;
        private System.Windows.Forms.Button btn_GPIO_OUT_12;
        private System.Windows.Forms.Button btn_GPIO_OUT_13;
        private System.Windows.Forms.Button btn_GPIO_OUT_14;
        private System.Windows.Forms.Button btn_GPIO_OUT_15;
        private System.Windows.Forms.Button btn_GPIO_IN_15;
        private System.Windows.Forms.Button btn_GPIO_IN_14;
        private System.Windows.Forms.Button btn_GPIO_IN_13;
        private System.Windows.Forms.Button btn_GPIO_IN_12;
        private System.Windows.Forms.Button btn_GPIO_IN_11;
        private System.Windows.Forms.Button btn_GPIO_IN_10;
        private System.Windows.Forms.Button btn_GPIO_IN_9;
        private System.Windows.Forms.Button btn_GPIO_IN_8;
        private System.Windows.Forms.Button btn_GPIO_IN_7;
        private System.Windows.Forms.Button btn_GPIO_IN_6;
        private System.Windows.Forms.Button btn_GPIO_IN_4;
        private System.Windows.Forms.Button btn_GPIO_IN_3;
        private System.Windows.Forms.Button btn_GPIO_IN_2;
        private System.Windows.Forms.Button btn_GPIO_IN_1;
        private System.Windows.Forms.Button btn_GPIO_IN_0;
        private System.Windows.Forms.Button btn_GPIO_IN_5;
        private System.Windows.Forms.Button btn_UpdateUI;
    }
}

