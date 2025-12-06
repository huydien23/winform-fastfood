using System;
using System.Drawing;
using System.Windows.Forms;

namespace PM_Ban_Do_An_Nhanh.Helpers
{
    /// <summary>
    /// Form hiển thị loading indicator
    /// </summary>
    public class LoadingForm : Form
    {
        private ProgressBar progressBar;
        private Label lblMessage;

        public LoadingForm(string message = "Đang xử lý...")
        {
            InitializeComponent(message);
        }

        private void InitializeComponent(string message)
        {
            this.progressBar = new ProgressBar();
            this.lblMessage = new Label();

            // Form
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(300, 100);
            this.BackColor = Color.White;
            this.TopMost = true;

            // ProgressBar
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.MarqueeAnimationSpeed = 30;
            this.progressBar.Location = new Point(20, 40);
            this.progressBar.Size = new Size(260, 23);

            // Label
            this.lblMessage.Text = message;
            this.lblMessage.Font = new Font("Arial", 10F, FontStyle.Bold);
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.lblMessage.Location = new Point(0, 10);
            this.lblMessage.Size = new Size(300, 20);

            // Add controls
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblMessage);
        }

        public void UpdateMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => lblMessage.Text = message));
            }
            else
            {
                lblMessage.Text = message;
            }
        }
    }
}
