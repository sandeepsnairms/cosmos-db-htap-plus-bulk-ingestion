namespace BulkCallIngestWinApp
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
            this.ingRate = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.Button();
            this.update = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.singleInsert = new System.Windows.Forms.Button();
            this.prepSubscriber = new System.Windows.Forms.Button();
            this.upSubscriber = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ingRate
            // 
            this.ingRate.Location = new System.Drawing.Point(184, 46);
            this.ingRate.Margin = new System.Windows.Forms.Padding(1);
            this.ingRate.Name = "ingRate";
            this.ingRate.Size = new System.Drawing.Size(61, 23);
            this.ingRate.TabIndex = 0;
            this.ingRate.Text = "50";
            this.ingRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ingRate_KeyPress);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(214, 82);
            this.start.Margin = new System.Windows.Forms.Padding(1);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(77, 22);
            this.start.TabIndex = 1;
            this.start.Text = "Start Timer";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // update
            // 
            this.update.Location = new System.Drawing.Point(280, 45);
            this.update.Margin = new System.Windows.Forms.Padding(1);
            this.update.Name = "update";
            this.update.Size = new System.Drawing.Size(106, 22);
            this.update.TabIndex = 2;
            this.update.Text = "Update rate/sec";
            this.update.UseVisualStyleBackColor = true;
            this.update.Click += new System.EventHandler(this.update_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(307, 82);
            this.stop.Margin = new System.Windows.Forms.Padding(1);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(79, 22);
            this.stop.TabIndex = 3;
            this.stop.Text = "Stop Timer";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Document Ingestion Rate";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "/sec";
            // 
            // singleInsert
            // 
            this.singleInsert.Location = new System.Drawing.Point(16, 82);
            this.singleInsert.Margin = new System.Windows.Forms.Padding(1);
            this.singleInsert.Name = "singleInsert";
            this.singleInsert.Size = new System.Drawing.Size(104, 22);
            this.singleInsert.TabIndex = 6;
            this.singleInsert.Text = "Run Once";
            this.singleInsert.UseVisualStyleBackColor = true;
            this.singleInsert.Click += new System.EventHandler(this.singleInsert_Click);
            // 
            //prepSubscriber
            // 
            this.prepSubscriber.Location = new System.Drawing.Point(89, 42);
            this.prepSubscriber.Margin = new System.Windows.Forms.Padding(1);
            this.prepSubscriber.Name = "prepSubscriber";
            this.prepSubscriber.Size = new System.Drawing.Size(140, 22);
            this.prepSubscriber.TabIndex = 7;
            this.prepSubscriber.Text = "Prepare Subscribers";
            this.prepSubscriber.UseVisualStyleBackColor = true;
            this.prepSubscriber.Click += new System.EventHandler(this.prepSubscriber_Click);
            // 
            // upSubscriber
            // 
            this.upSubscriber.Location = new System.Drawing.Point(249, 42);
            this.upSubscriber.Margin = new System.Windows.Forms.Padding(1);
            this.upSubscriber.Name = "upSubscriber";
            this.upSubscriber.Size = new System.Drawing.Size(140, 22);
            this.upSubscriber.TabIndex = 8;
            this.upSubscriber.Text = "Upload Subscribers";
            this.upSubscriber.UseVisualStyleBackColor = true;
            this.upSubscriber.Click += new System.EventHandler(this.upSubscriber_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ingRate);
            this.groupBox1.Controls.Add(this.start);
            this.groupBox1.Controls.Add(this.update);
            this.groupBox1.Controls.Add(this.singleInsert);
            this.groupBox1.Controls.Add(this.stop);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 118);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ingest Call Records";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.upSubscriber);
            this.groupBox2.Controls.Add(this.prepSubscriber);
            this.groupBox2.Location = new System.Drawing.Point(9, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 79);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ingest Subscribers";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 228);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Cosmos DB : Contoso Mobile Data Ingestion";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox ingRate;
        private Button start;
        private Button update;
        private Button stop;
        private Label label1;
        private Label label2;
        private extTimer timer1;
        private Button singleInsert;
        private Button prepSubscriber;
        private Button upSubscriber;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
    }
}