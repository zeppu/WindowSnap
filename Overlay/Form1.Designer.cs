using System.Windows.Forms;

namespace Overlay
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HalfCenter = new System.Windows.Forms.Button();
            this.QuarterLeft = new System.Windows.Forms.Button();
            this.QuarterRight = new System.Windows.Forms.Button();
            this.ThirdRight = new System.Windows.Forms.Button();
            this.ThirdLeft = new System.Windows.Forms.Button();
            this.TwoThirdLeft = new System.Windows.Forms.Button();
            this.TwoThirdRight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // HalfCenter
            // 
            this.HalfCenter.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.HalfCenter.Location = new System.Drawing.Point(509, 1);
            this.HalfCenter.Name = "HalfCenter";
            this.HalfCenter.Size = new System.Drawing.Size(108, 103);
            this.HalfCenter.TabIndex = 0;
            this.HalfCenter.Text = "1/2 Center";
            this.HalfCenter.UseVisualStyleBackColor = true;
            // 
            // QuarterLeft
            // 
            this.QuarterLeft.Location = new System.Drawing.Point(395, 1);
            this.QuarterLeft.Name = "QuarterLeft";
            this.QuarterLeft.Size = new System.Drawing.Size(108, 103);
            this.QuarterLeft.TabIndex = 1;
            this.QuarterLeft.Text = "1/4 Left";
            this.QuarterLeft.UseVisualStyleBackColor = true;
            // 
            // QuarterRight
            // 
            this.QuarterRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.QuarterRight.Location = new System.Drawing.Point(623, 1);
            this.QuarterRight.Name = "QuarterRight";
            this.QuarterRight.Size = new System.Drawing.Size(108, 103);
            this.QuarterRight.TabIndex = 2;
            this.QuarterRight.Text = "1/4 Right";
            this.QuarterRight.UseVisualStyleBackColor = true;
            // 
            // ThirdRight
            // 
            this.ThirdRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ThirdRight.Location = new System.Drawing.Point(381, 1);
            this.ThirdRight.Name = "ThirdRight";
            this.ThirdRight.Size = new System.Drawing.Size(108, 103);
            this.ThirdRight.TabIndex = 3;
            this.ThirdRight.Text = "1/3 Right";
            this.ThirdRight.UseVisualStyleBackColor = true;
            // 
            // ThirdLeft
            // 
            this.ThirdLeft.Location = new System.Drawing.Point(637, 1);
            this.ThirdLeft.Name = "ThirdLeft";
            this.ThirdLeft.Size = new System.Drawing.Size(108, 103);
            this.ThirdLeft.TabIndex = 4;
            this.ThirdLeft.Text = "1/3 Left";
            this.ThirdLeft.UseVisualStyleBackColor = true;
            // 
            // TwoThirdLeft
            // 
            this.TwoThirdLeft.Location = new System.Drawing.Point(937, 1);
            this.TwoThirdLeft.Name = "TwoThirdLeft";
            this.TwoThirdLeft.Size = new System.Drawing.Size(108, 103);
            this.TwoThirdLeft.TabIndex = 4;
            this.TwoThirdLeft.Text = "2/3 Left";
            this.TwoThirdLeft.UseVisualStyleBackColor = true;
            // 
            // TwoThirdRight
            // 
            this.TwoThirdRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TwoThirdRight.Location = new System.Drawing.Point(81, 1);
            this.TwoThirdRight.Name = "TwoThirdRight";
            this.TwoThirdRight.Size = new System.Drawing.Size(108, 103);
            this.TwoThirdRight.TabIndex = 3;
            this.TwoThirdRight.Text = "2/3 Right";
            this.TwoThirdRight.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LimeGreen;
            this.ClientSize = new System.Drawing.Size(1152, 108);
            this.Controls.Add(this.ThirdLeft);
            this.Controls.Add(this.ThirdRight);
            this.Controls.Add(this.QuarterRight);
            this.Controls.Add(this.QuarterLeft);
            this.Controls.Add(this.HalfCenter);
            this.Controls.Add(this.TwoThirdRight);
            this.Controls.Add(this.TwoThirdLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.LimeGreen;
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button HalfCenter;
        public System.Windows.Forms.Button QuarterLeft;
        public System.Windows.Forms.Button QuarterRight;
        public System.Windows.Forms.Button ThirdRight;
        public System.Windows.Forms.Button TwoThirdRight;
        public System.Windows.Forms.Button ThirdLeft;
        public System.Windows.Forms.Button TwoThirdLeft;
    }
}

