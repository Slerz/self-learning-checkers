
namespace sas
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.board = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.PanelMenu = new System.Windows.Forms.Panel();
			this.Comp2 = new System.Windows.Forms.Button();
			this.Comp1 = new System.Windows.Forms.Button();
			this.Comp0 = new System.Windows.Forms.Button();
			this.ManVSMan = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.PanelMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// board
			// 
			this.board.AllowDrop = true;
			this.board.BackColor = System.Drawing.Color.White;
			this.board.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("board.BackgroundImage")));
			this.board.Location = new System.Drawing.Point(53, 67);
			this.board.Name = "board";
			this.board.Size = new System.Drawing.Size(660, 660);
			this.board.TabIndex = 0;
			this.board.Paint += new System.Windows.Forms.PaintEventHandler(this.board_Paint);
			this.board.MouseUp += new System.Windows.Forms.MouseEventHandler(this.board_MouseUp);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(887, 111);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(887, 150);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "label2";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(874, 195);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(198, 36);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "label3";
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(1044, 67);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(194, 342);
			this.listBox1.TabIndex = 5;
			//this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// listBox2
			// 
			this.listBox2.FormattingEnabled = true;
			this.listBox2.Location = new System.Drawing.Point(1293, 67);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(120, 95);
			this.listBox2.TabIndex = 6;
			// 
			// PanelMenu
			// 
			this.PanelMenu.Controls.Add(this.Comp2);
			this.PanelMenu.Controls.Add(this.Comp1);
			this.PanelMenu.Controls.Add(this.Comp0);
			this.PanelMenu.Controls.Add(this.ManVSMan);
			this.PanelMenu.Location = new System.Drawing.Point(795, 251);
			this.PanelMenu.Name = "PanelMenu";
			this.PanelMenu.Size = new System.Drawing.Size(217, 153);
			this.PanelMenu.TabIndex = 7;
			// 
			// Comp2
			// 
			this.Comp2.Location = new System.Drawing.Point(21, 110);
			this.Comp2.Name = "Comp2";
			this.Comp2.Size = new System.Drawing.Size(90, 23);
			this.Comp2.TabIndex = 3;
			this.Comp2.Text = "Comp Hard";
			this.Comp2.UseVisualStyleBackColor = true;
			this.Comp2.Click += new System.EventHandler(this.Comp2_Click);
			// 
			// Comp1
			// 
			this.Comp1.Location = new System.Drawing.Point(21, 81);
			this.Comp1.Name = "Comp1";
			this.Comp1.Size = new System.Drawing.Size(90, 23);
			this.Comp1.TabIndex = 2;
			this.Comp1.Text = "Comp Medium";
			this.Comp1.UseVisualStyleBackColor = true;
			this.Comp1.Click += new System.EventHandler(this.Comp1_Click);
			// 
			// Comp0
			// 
			this.Comp0.Location = new System.Drawing.Point(21, 49);
			this.Comp0.Name = "Comp0";
			this.Comp0.Size = new System.Drawing.Size(90, 23);
			this.Comp0.TabIndex = 1;
			this.Comp0.Text = "Comp Easy";
			this.Comp0.UseVisualStyleBackColor = true;
			this.Comp0.Click += new System.EventHandler(this.Comp0_Click);
			// 
			// ManVSMan
			// 
			this.ManVSMan.Location = new System.Drawing.Point(21, 20);
			this.ManVSMan.Name = "ManVSMan";
			this.ManVSMan.Size = new System.Drawing.Size(90, 23);
			this.ManVSMan.TabIndex = 0;
			this.ManVSMan.Text = "Man vs Man";
			this.ManVSMan.UseVisualStyleBackColor = true;
			this.ManVSMan.Click += new System.EventHandler(this.ManVSMan_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(1051, 21);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(35, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "label4";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(1164, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(35, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "label5";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(1293, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(35, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "label6";
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(53, 22);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(25, 28);
			this.panel1.TabIndex = 11;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(813, 515);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(35, 13);
			this.label7.TabIndex = 0;
			this.label7.Text = "label7";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1441, 850);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.PanelMenu);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.board);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
			this.PanelMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel board;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Panel PanelMenu;
		private System.Windows.Forms.Button Comp2;
		private System.Windows.Forms.Button Comp1;
		private System.Windows.Forms.Button Comp0;
		private System.Windows.Forms.Button ManVSMan;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label7;
	}
}

