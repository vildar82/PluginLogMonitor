namespace LogMonitor
{
   partial class FormLog
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLog));
         this.buttonStart = new System.Windows.Forms.Button();
         this.buttonStop = new System.Windows.Forms.Button();
         this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
         this.label1 = new System.Windows.Forms.Label();
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
         this.SuspendLayout();
         // 
         // buttonStart
         // 
         this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.buttonStart.Location = new System.Drawing.Point(35, 68);
         this.buttonStart.Name = "buttonStart";
         this.buttonStart.Size = new System.Drawing.Size(154, 41);
         this.buttonStart.TabIndex = 0;
         this.buttonStart.Text = "Start";
         this.buttonStart.UseVisualStyleBackColor = true;
         this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
         // 
         // buttonStop
         // 
         this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.buttonStop.Location = new System.Drawing.Point(247, 69);
         this.buttonStop.Name = "buttonStop";
         this.buttonStop.Size = new System.Drawing.Size(154, 40);
         this.buttonStop.TabIndex = 1;
         this.buttonStop.Text = "Stop";
         this.buttonStop.UseVisualStyleBackColor = true;
         this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
         // 
         // notifyIcon1
         // 
         this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
         this.notifyIcon1.Text = "Log Plugin Monitor";
         this.notifyIcon1.Visible = true;
         this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.label1.Location = new System.Drawing.Point(23, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(51, 20);
         this.label1.TabIndex = 2;
         this.label1.Text = "label1";
         // 
         // richTextBox1
         // 
         this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.richTextBox1.Location = new System.Drawing.Point(12, 145);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.ReadOnly = true;
         this.richTextBox1.Size = new System.Drawing.Size(1252, 421);
         this.richTextBox1.TabIndex = 4;
         this.richTextBox1.Text = "";
         // 
         // FormLog
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1276, 578);
         this.Controls.Add(this.richTextBox1);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.buttonStop);
         this.Controls.Add(this.buttonStart);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "FormLog";
         this.ShowInTaskbar = false;
         this.Text = "Plugin Log Monitor";
         this.Deactivate += new System.EventHandler(this.FormLog_Deactivate);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button buttonStart;
      private System.Windows.Forms.Button buttonStop;
      private System.Windows.Forms.NotifyIcon notifyIcon1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.RichTextBox richTextBox1;
      private System.ComponentModel.BackgroundWorker backgroundWorker1;
   }
}