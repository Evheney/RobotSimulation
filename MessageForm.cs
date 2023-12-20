using System;
using System.Drawing;
using System.Windows.Forms;

namespace chip_counter
{
    public partial class MessageForm : Form
    {
        public enum MESSAGE_FORM_TYPE { CONFIRM, RESET, NONE, EXIT, MODELESS };
        public MESSAGE_FORM_TYPE TYPE = MESSAGE_FORM_TYPE.CONFIRM;
        public bool RESET = false;
        public bool SHOW_RESET_BTN = false;
        public delegate void OKFunc(); //message form for showing message to operator or user.
        public OKFunc okFunc;
        //public event OKFunc OKEvent;

        public MessageForm(MESSAGE_FORM_TYPE typeMsg = MESSAGE_FORM_TYPE.CONFIRM)
        {
            TYPE = typeMsg;
            InitializeComponent();

            if (TYPE == MESSAGE_FORM_TYPE.MODELESS)
            {
                btnOk.Enabled = false;
                btnOk.Visible = false;
                btnConfirm.Enabled = false;
                btnConfirm.Visible = false;
                btnReset.Enabled = false;
                btnReset.Visible = false;
                ButtonOK.Enabled = false;
                ButtonOK.Visible = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            RESET = true;
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.CenterToScreen();

            btnOk.Enabled = false;
            btnOk.Visible = false;
            btnConfirm.Enabled = false;
            btnConfirm.Visible = false;
            btnReset.Enabled = false;
            btnReset.Visible = false;
            ButtonOK.Enabled = false;
            ButtonOK.Visible = false;
            if (TYPE == MESSAGE_FORM_TYPE.RESET)
            {
                btnReset.Enabled = true;
                btnReset.Visible = true;
                this.CenterToScreen();
                this.BringToFront();
            }
            if (TYPE == MESSAGE_FORM_TYPE.EXIT || TYPE == MESSAGE_FORM_TYPE.CONFIRM || TYPE == MESSAGE_FORM_TYPE.MODELESS)
            {
                btnConfirm.Enabled = true;
                btnConfirm.Visible = true;
                this.CenterToScreen();
                this.BringToFront();
            }
            //if (TYPE == MESSAGE_FORM_TYPE.CONFIRM) 
            //{
            //    btnConfirm.Enabled = true;
            //    btnConfirm.Visible = true;
            //    btnReset.Enabled = false;
            //    btnReset.Visible = false;
            //    ButtonOK.Enabled = true;
            //    ButtonOK.Visible = true;
            //    this.CenterToScreen();
            //}

            if (MessageBody.Text.Length > 100)
            {
                MessageBody.Font = new Font("굴림체", 10, FontStyle.Bold);
            }

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (TYPE == MESSAGE_FORM_TYPE.MODELESS)
            {
                if (okFunc != null)
                {
                    okFunc();
                }
                Hide();
            }
            else
            {
                Close();
            }
            //Hide();
        }

        //private void ButtonOk_Click(object sender, EventArgs e) 
        //{
        //    DialogResult = DialogResult.OK;

        //    if (okFunc != null)
        //    {
        //        okFunc();
        //    }

        //    Close();
        //}
        public void ButtonEnable()
        {
            //this.Location = new Point(this.Location.X, this.Location.Y - 20);
            btnOk.Visible = false;
            btnConfirm.Visible = true;
            //ButtonOK.Visible = false;
        }
        public void ButtonDisable()
        {
            //this.Location = new Point(this.Location.X, this.Location.Y - 20);
            btnOk.Visible = false;
            btnConfirm.Visible = false;
            ButtonOK.Visible = false;
        }
        public void PlaceFormInCenter()
        {
            CenterToParent();
        }
        public void PlaceFormToCenter()
        {
            StartPosition = FormStartPosition.Manual;
            Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                                 (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
        }

        //public void PlaceFormInCenter()
        //{
        //    Rectangle rcScreen = Screen.WorkingArea;
        //    Rectangle rcForm = new Rectangle(0, 0, this.Width, this.Height);
        //    this.Location = new Point((rcScreen.Left + rcScreen.Right) / 2 - (rcForm.Width / 2), (rcScreen.Top + rcScreen.Bottom) / 2 - (rcForm.Height / 2));
        //}


        //public void ExitBtn_Click(object sender, EventArgs e)
        //{
        //    Close();
        //}
    }
}
