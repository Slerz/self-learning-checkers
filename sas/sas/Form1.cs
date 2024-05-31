using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;


namespace sas
{
	public partial class Form1 : Form
	{
		GameHist GH = new GameHist();
		int[] Cells = new int[32];
		int [,] CellsVar = new int[32, 1000];
		//0 - пустая ячейка
		//1 - белая шашка
		//2 - черная шашка
		//3 - белая дамка
		//4 - черная дамка
		Graphics g1;
		Pen bl = new Pen(Color.Black, 3);
		Pen lime = new Pen(Color.Lime, 3);
		SolidBrush whiteB = new SolidBrush(Color.White);
		SolidBrush blackB = new SolidBrush(Color.Black);
		SolidBrush GrayB = new SolidBrush(Color.Gray);
		int CellsWidth = 70;
		int x0 = 50;
		int y0 = 50;
		int t;
		int sost = 0;
		//0 ждем хода белых
		//1 ждем хода черных
		//2 ждем цели белых
		//3 ждем цели черных
		int selectedcell;
		int[] MC0 = new int[200];
		int[] MC1 = new int[200];
		string[] TC = new string[200];
		int MCCount = 0;
		string[] letter = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
		string[] number = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
		Font drawFont = new Font("Arial", 16);
		SolidBrush drawBrush = new SolidBrush(Color.Black);
		string[] snum = new string[32] {"b8", "d8", "f8", "h8", "a7", "c7", "e7", "g7", "b6", "d6", "f6", "h6", "a5", "c5", "e5", "g5", "b4", "d4", "f4", "h4", "a3", "c3", "e3","g3", "b2", "d2", "f2", "h2", "a1", "c1", "e1", "g1" };
		int GameStatus = 0;
		/*
		0 - 2 человека
		1 - комп 0
		2 - комп 1
		3- комп 2 с обучением
		*/

		SoundPlayer EatSound;
		SoundPlayer StepSound;
		SoundPlayer ErrorSound;

		int[] NextStep0 = new int[100];
		int[] NextStep1 = new int[100];
		int[] NextStepValue = new int[100];
		int NextStepCount;
		// NextStep0 - позиция, откуда можно сделать ход пешкой
		// NextStep1 - позиция, куда можно сделать ход пешкой
		//NextStepCount  - количество возможных ходов пешкой

		int[] NextTake0 = new int[100];
		int[] NextTake1 = new int[100];
		int[] NextTakeValue = new int[100];
		string[] NextTakeList = new string[100];
		int NextTakeCount;
		// NextTake0 - позиция, откуда можно сделать взятие пешкой
		// NextTake1 - позиция, куда можно сделать взятие пешкой
		//NextTakeCount  - количество возможных взятий пешкой

		int[] NextDStep0 = new int[100];
		int[] NextDStep1 = new int[100];
		int[] NextDStepValue = new int[100];
		int NextDStepCount;
		// NextDStep0 - позиция, откуда можно сделать ход дамкой
		// NextDStep1 - позиция, куда можно сделать ход дамкой
		//NextDStepCount  - количество возможных ходов дамкой

		int[] NextDTake0 = new int[100];
		int[] NextDTake1 = new int[100];
		int[] NextDTakeValue = new int[100];
		int NextDTakeCount;
		string[] NextTakeDList = new string[100];
		// NextDTake0 - позиция, откуда можно сделать взятие дамкой
		// NextDTake1 - позиция, куда можно сделать взятие дамкой
		//NextDTakeCount  - количество 	
		//int[] MasStep = new int[100];

		int debug = 0;
		//0 - нет отладки
		//1 - отладка записи в файл
		
		bool learnstatus;

		bool TakeDoes;
		int AutoTakeCell;
		int[] DiagDTake = new int[8];
		int DiagDTakeCount;
		public Form1()

		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			t = 0;
			sost = 0;
			selectedcell = -1;
			MCCount = 0;
			for (int i = 0; i < 200; i++) TC[i] = "";
			listBox1.Items.Clear();
			GameStatus = 0;
			string pt = Directory.GetCurrentDirectory();

			//звуки
			EatSound = new System.Media.SoundPlayer (pt + "\\eat.wav");
			StepSound = new System.Media.SoundPlayer(pt + "\\step.wav");
			ErrorSound = new System.Media.SoundPlayer(pt + "\\error.wav");

			GH.HistFN = pt + "\\Hist.sas";

			TakeDoes = false;

			string br = pt + "\\board.jpg";
			Image board = Image.FromFile(br);

			panel1.BackColor = Color.Gray;
		}
		void OutBoard()
		{
			
			//оценка на табло
			int p1 = PostEvaluation(1);
			label5.Text = Convert.ToString(p1);
			int p0 = PostEvaluation(0);
			label4.Text = Convert.ToString(p0);

			string pt = Directory.GetCurrentDirectory(); // текущ папка
			string fnb = pt + "\\black.png";
			string fnw = pt + "\\white.png";
			string fnbd = pt + "\\blackD.png";
			string fnwd = pt + "\\whiteD.png";

			Image sb = Image.FromFile(fnb);
			Image sw = Image.FromFile(fnw);
			Image sbd = Image.FromFile(fnbd);
			Image swd = Image.FromFile(fnwd);

			g1 = board.CreateGraphics();
			
			/*рисуем поле
			g1 = board.CreateGraphics();
			
			g1.DrawLine(bl, x0, y0, x0 + w, y0);
			g1.DrawLine(bl, x0, y0, x0, y0 + w);
			g1.DrawLine(bl, x0, y0 + w, x0 + w, y0 + w);
			g1.DrawLine(bl, x0 + w, y0, x0 + w, y0 + w);
			*/


			string cellw = pt + "\\cellw.png";
			string cellb = pt + "\\cellb.png";

			
			Image cellw1 = Image.FromFile(cellw);
			Image cellb1 = Image.FromFile(cellb);

			
			label3.Text = "";


			if (sost == 0)
			{
				label3.Text = "Ход белых";
			}

			if (sost == 1)
			{
				label3.Text = "Ход черных";

			}

			/*for (int i = 1; i < 8; i++)
			{
				int x1 = x0 + CellsWidth * i;
				g1.DrawLine(bl, x1, y0, x1, y0 + w);
			}
			*/
			
			for (int i = 0; i < 8; i++)
			{
				int w = 8 * CellsWidth;
				int x1 = x0 + 70 * i;
				int y1 = y0 + 70 * i;

				g1.DrawString(letter[i], drawFont, drawBrush, x1 + 25, y0 + w + 5);
				g1.DrawString(number[7 - i], drawFont, drawBrush, x0 - 30, y1 + 20);
			}

			/*
			for (int i = 1; i < 8; i++)
			{
				int y1 = y0 + CellsWidth * i;
				g1.DrawLine(bl, x0, y1, x0 + w, y1);

			}
			*/
			for (int i = 0; i < 8; i++)
			{
				int k = i / 2;
				int m = i - 2 * k;
				int x1 = 1 - m;
				int x2 = x0 * x1;
				int y2 = y0 + i * CellsWidth;
				for (int j = m; j < 8; j = j + 2)
				{
					int x3 = x0 + CellsWidth * (x1 + j - m);
					g1.DrawImage(cellb1, x3, y2, 70, 70);

				}

			}


			for (int i = 0; i < 4; i++)
			{
				int y1 = y0 + 140 * i;
				g1.DrawImage(cellw1, x0, y1, 70, 70);
				for (int k = 0; k < 4; k++)
				{
					int x1 = x0 + 140 * k;
					g1.DrawImage(cellw1, x1, y1, 70, 70);
				}
			}

			for (int i = 0; i < 4; i++)
			{
				int y1 = y0 + 140 * i;
				g1.DrawImage(cellw1, x0 + 70, y1 + 70, 70, 70);
				for (int k = 0; k < 4; k++)
				{
					int x1 = x0 + 140 * k;
					g1.DrawImage(cellw1, x1 + 70, y1 + 70, 70, 70);
				}
			}

			for (int i = 0; i < 32; i++)
			{
				int row = i / 4;
				int col = i - row * 4;
				int m = row / 2;
				int n = row - 2 * m; //тип ряда
				int yc = y0 + row * CellsWidth;
				int xc = 0;

				int cc = CellsWidth / 5;
				cc = 4 * cc;
				int cc1 = (CellsWidth - cc) / 2;
				yc = yc + cc1;
				if (n == 0)
				{

					xc = x0 + CellsWidth + 2 * col * CellsWidth + cc1;
				}
				else
				{

					xc = x0 + 2 * col * CellsWidth + cc1;
				}

				if (Cells[i] == 1)
				{

					if (row == 0)
					{
						
						g1.DrawImage(swd, xc, yc, cc, cc);
						Cells[i] = 3;
						p1 = PostEvaluation(1);
						label5.Text = Convert.ToString(p1);
						p0 = PostEvaluation(0);
						label4.Text = Convert.ToString(p0);
					}
					else
					{
						g1.DrawImage(sw, xc, yc, cc, cc);
					}
				}

				if (Cells[i] == 2)
				{
					if (row == 7)
					{
						g1.DrawImage(sbd, xc, yc, cc, cc);
						Cells[i] = 4;
						p1 = PostEvaluation(1);
						label5.Text = Convert.ToString(p1);
						p0 = PostEvaluation(0);
						label4.Text = Convert.ToString(p0);
					}
					else
					{
						g1.DrawImage(sb, xc, yc, cc, cc);
					}
				}

				if (Cells[i] == 3)
				{
					g1.DrawImage(swd, xc, yc, cc, cc);
				}

				if (Cells[i] == 4)
				{
					g1.DrawImage(sbd, xc, yc, cc, cc);
				}


			}
			if (selectedcell > -1)
			{
				int r = selectedcell / 4;
				int c = selectedcell - 4 * r;
				if (r == 0 || r == 2 || r == 4 || r == 6) c = 1 + 2 * c;
				if (r == 1 || r == 3 || r == 5 || r == 7) c = 2 * c;
				int xc = x0 + CellsWidth * c;
				int yc = y0 + CellsWidth * r;
				g1.DrawLine(lime, xc, yc, xc + CellsWidth, yc);
				g1.DrawLine(lime, xc, yc, xc, yc + CellsWidth);
				g1.DrawLine(lime, xc, yc + CellsWidth, xc + CellsWidth, yc + CellsWidth);
				g1.DrawLine(lime, xc + CellsWidth, yc + CellsWidth, xc + CellsWidth, yc);
			}


		}

		private void board_Paint(object sender, PaintEventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			OutBoard();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{

			timer1.Enabled = false;
			InitStand();
			OutBoard();
		}
		void InitStand()
		{
			if (debug == 0)
			{
				for (int i = 0; i < 12; i++) Cells[i] = 2;
				for (int i = 12; i < 20; i++) Cells[i] = 0;
				for (int i = 20; i < 32; i++) Cells[i] = 1;
			}
			if (debug == 1)
			{
				for (int i = 0; i < 32; i++) Cells[i] = 0;

				Cells[4] = 2;
				Cells[14] = 2;
				Cells[18] = 1;
				Cells[16] = 1;
			}
			if (debug == 2)
			{
				for (int i = 0; i < 32; i++) Cells[i] = 0;

				Cells[16] = 2;
				Cells[17] = 2;
				Cells[18] = 2;
				Cells[6] = 1;
				Cells[7] = 1;
				Cells[31] = 1;
			}

		}

		private void board_MouseUp(object sender, MouseEventArgs e)
		{
			int p0, p1;
			int x = e.X;
			int y = e.Y;
			int nc = GiveCell(x, y);
			if (nc == -1) return;
			label1.Text = Convert.ToString(nc);
			label6.Text = Convert.ToString(GameStatus);
			bool IsD = false;

			if (sost == 0)
			{

				if (Cells[nc] == 1 || Cells[nc] == 3)
				{
					selectedcell = nc;
					sost = 2;
					OutBoard();
					TakeDoes = false;
					return;
				}
			}
			if (sost == 2)
			{
				if (Cells[nc] == 2) return;
				if (Cells[nc] == 1 || Cells[nc] == 3)
				{
					selectedcell = nc;
					OutBoard();
					return;
				}
				if (Cells[selectedcell] == 3) IsD = true;
				if (checkstep(0, selectedcell, nc))
				{
					if (IsD)
					{
						Cells[nc] = 3;
						p1 = PostEvaluation(1);
						label5.Text = Convert.ToString(p1);
						p0 = PostEvaluation(0);
						label4.Text = Convert.ToString(p0);
					}
					else
					{
						Cells[nc] = 1;
					}

					Cells[selectedcell] = 0;
					WriteToList(selectedcell, nc);
					p0 = PostEvaluation(0);
					label4.Text = Convert.ToString(p0);
					p1 = PostEvaluation(1);
					label5.Text = Convert.ToString(p1);
					//    Добавлен контроль на продолжение взятия
					if (IsTakeAll(1) && TakeDoes)
					{
						selectedcell = nc;
						OutBoard();
					}
					else
					{
						selectedcell = -1;
						if (GameStatus == 0)
						{
							sost = 1;
						}
						else
						{
							DoStepComp();
						}
						OutBoard();
						CheckGameEnd();
					}



				}
			}

			if (sost == 1)
			{
				if (Cells[nc] == 2 || Cells[nc] == 4)
				{
					selectedcell = nc;
					sost = 3;
					TakeDoes = false;
					OutBoard();
					return;
				}
			}
			if (sost == 3)
			{
				if (Cells[nc] == 1) return;
				if (Cells[nc] == 2 || Cells[nc] == 4)
				{
					selectedcell = nc;
					OutBoard();
					return;
				}
				if (Cells[selectedcell] == 4) IsD = true;
				if (checkstep(1, selectedcell, nc))
				{
					if (IsD)
					{
						Cells[nc] = 4;
						p1 = PostEvaluation(1);
						label5.Text = Convert.ToString(p1);
						p0 = PostEvaluation(0);
						label4.Text = Convert.ToString(p0);
					}
					else
					{
						Cells[nc] = 2;
					}
					Cells[selectedcell] = 0;
					WriteToList(selectedcell, nc);
					p1 = PostEvaluation(1);
					label5.Text = Convert.ToString(p1);
					p0 = PostEvaluation(0);
					label4.Text = Convert.ToString(p0);


					//    Добавлен контроль на продолжение взятия
					if (IsTakeAll(2) && TakeDoes)
					{
						selectedcell = nc;
						OutBoard();
					}
					else
					{
						selectedcell = -1;
						sost = 0;
						OutBoard();
						CheckGameEnd();
					}

				}
			}

		}

		void WriteToList(int nc0, int nc1)
		{
			//если MCCount = 0 или четный, то записываем как белый ход, иначе как черные :2 *2
			int ns = MCCount / 2 + 1;

			string del = ":";
			if (TC[MCCount] == "")
			{
				del = "-";
				StepSound.Play();
			}
			if (MCCount == 0 || MCCount % 2 == 0)
			{
				
				string sm = Convert.ToString(ns) + ". " + CellToString(nc0) + del + CellToString(nc1) + "  " + TC[MCCount];
				listBox1.Items.Add(sm);
				
			}
			else 
			{
			
				string sm = "     " + CellToString(nc0) + del + CellToString(nc1) + "  " + TC[MCCount];
				listBox1.Items.Add(sm);
				
			}
			
			MC0[MCCount] = nc0;
			MC1[MCCount] = nc1;
			

			MCCount++;
		}

		void DoStepComp()
		{
			if (GameStatus == 1)
			{
				DoStepComp0();
			}
			if (GameStatus == 2) DoStepComp1();

			if (GameStatus == 3) DoStepComp2();
		}

		void DoStepComp0()
		{
			DoStepComp1();
		}

		void DoStepComp1()
		{
			int nc = GiveBestStep(1);
			if (nc > -1)
			{
				if (NextStepCount > 0)
				{
					int sof = Cells[NextStep0[nc]];
					Cells[NextStep0[nc]] = 0;
					Cells[NextStep1[nc]] = sof;
					WriteToList(NextStep0[nc], NextStep1[nc]);
					OutBoard();
					sost = 0;
				}

				if (NextTakeCount > 0)
				{
					int sof = Cells[NextTake0[nc]];
					Cells[NextTake0[nc]] = 0;
					Cells[NextTake1[nc]] = sof;
					WriteToList(NextTake0[nc], NextTake1[nc]);
					int nc1 = Convert.ToInt32(NextTakeList[nc]);
					Cells[nc1] = 0;
					if (sof==2)
                    {
						bool bt = true;
						int tk = 0;
						nc = NextTake1[nc];
						while (bt)
                        {
                           if (IsTake(nc))
                           {
								
								int ncbt = GiveBestTake(nc);
								if (ncbt>-1)
                                {
									Cells[nc] = 0;
									Cells[AutoTakeCell] = 0;
									Cells[ncbt] = 2;
									nc = ncbt;
                                }
								else
                                {
									bt = false;
                                }
								tk++;
								if (tk > 8) bt = false;
                           }
						   else
                            {
								bt = false;
                            }
                        }

                    }
					if (sof==4)
                    {
						bool bt = true;
						int tk = 0;
						nc = NextTake1[nc];
						while (bt)
						{
							if (IsTakeD(nc))
							{

								int ncbt = GiveBestTakeD(nc);
								if (ncbt > -1)
								{
									Cells[nc] = 0;
									Cells[AutoTakeCell] = 0;
									Cells[ncbt] = 4;
									nc = ncbt;
								}
								else
								{
									bt = false;
								}
								tk++;
								if (tk > 8) bt = false;
							}
							else
							{
								bt = false;
							}
						}

					}
					OutBoard();
					sost = 0;
				}
			}
		}

		void DoStepComp2()
		{
			
			if (learnstatus)
			{
				panel1.BackColor = Color.Lime;
				int st = GH.GiveBestStepFromBase(MC0, MC1, TC, MCCount);
				if (st > -1)
				{
					int maxr = -1000;
					for (int i = 0; i < GH.HStepCount; i++)
					{
						if (GH.HResult[i] > maxr)
						{
							maxr = GH.HResult[i];
						}
					}
					label7.Text = Convert.ToString(GH.HStepCount) + " $ " + Convert.ToString(maxr) + " $ " + Convert.ToString(GH.HResult[st]);
					
					int s0 = GH.HStep[st];
					int s1 = GH.HStep1[st];
					string s01t = GH.HStepT[st];
					if (s01t == "")
					{
						Cells[s0] = 0;
						Cells[s1] = 2; //пока не учитываются ходы дамкой
					}
					else
					{
						int[] cellsr = new int[32];
						int[] cellsT = new int[12];
						int cellsTcount = 0;
						for (int i = 0; i < 32; i++)
						{
							cellsr[i] = Cells[i];
						}
						if (Cells[s0] == 2)
						{
							if (IsTake(s0))
							{
								int sd = s0;
								for (int dir = 0; dir < 4; dir++)
								{
									if (GiveNextCell(sd, dir) == 1 || GiveNextCell(st, dir) == 3)
									{
										int sa = GiveNextCellN(sd, dir);
										int sb = GiveNextCell(sa, dir);
										if (sb == 0)
										{
											cellsT[cellsTcount] = sa;
											cellsTcount++;
											if (sa != s1)
											{
												sd = GiveNextCellN(sa,dir)  ;//sd = sa;
												for (int dir1 = 0; dir1 < 4; dir1++)
												{
													if (GiveNextCell(sd, dir1) == 1 || GiveNextCell(sd, dir1) == 3)
													{
														sa = GiveNextCellN(sd, dir1);
														sb = GiveNextCell(sa, dir1);
														if (sb == 0)
														{
															bool b = true;
															for (int it=0;it<cellsTcount;it++)
                                                            {
																if (cellsT[it] == sa) b = false;

                                                            }
															if (b)
															{
																cellsT[cellsTcount] = sa;
																cellsTcount++;
															}
															//if (sa != s1) поставить еще дз


														}
													}
												}
											}
										}
									}
								}
							}
							Cells[s0] = 0;
							Cells[s1] = 2;
							for (int i1 = 0; i1 < cellsTcount; i1++)
							{
								Cells[cellsT[i1]] = 0;
							}
						}
					}
					OutBoard();
					WriteToList(s0, s1);
					return;
				}
				else 
				{
					learnstatus = false;
					panel1.BackColor = Color.Red;
				}
			}
			label7.Text = "";
			DoStepComp1();
		/*	int nc = GiveBestStep(1);
			if (nc > -1)
			{
				if (NextStepCount > 0)
				{
					Cells[NextStep0[nc]] = 0;
					Cells[NextStep1[nc]] = 2;
					WriteToList(NextStep0[nc], NextStep1[nc]);
					OutBoard();
					sost = 0;
				}

				if (NextTakeCount > 0)
				{
					Cells[NextTake0[nc]] = 0;
					Cells[NextTake1[nc]] = 2;
					WriteToList(NextTake0[nc], NextTake1[nc]);
					int nc1 = Convert.ToInt32(NextTakeList[nc]);
					Cells[nc1] = 0;
					OutBoard();
					sost = 0;
				}
			}*/
		}

		int GiveBestStep(int deep)
		{
			int best = -1;
			int[] Cells_c = new int[32];
			int sost_c;
			for (int i = 0; i < 32; i++)
			{
				Cells_c[i] = Cells[i];
				
			}
				sost_c = sost;
			int col = 1;
			//if (sost == 0 || sost == 2) col = 0;

			BuildStepList(col);
			int oc = -1000;
			for (int i = 0; i < NextStepCount; i++)
			{
				//  Пока не работают ходы дамок   25_11_21
				//  Сделано  25_11_21
				int sof = Cells[NextStep0[i]];
				Cells[NextStep0[i]] = 0;
				Cells[NextStep1[i]] = sof;
				NextStepValue[i] = PostEvaluation();

				if (NextStepValue[i] > oc)
				{
					best = i;
					oc = NextStepValue[i];           
				}
				Cells[NextStep0[i]] = sof;
				Cells[NextStep1[i]] = 0;
			}


			for (int i = 0; i < NextTakeCount; i++)
			{
				//  Пока не работают взятия  дамками   25_11_21
				int sof = Cells[NextStep0[i]];
				Cells[NextTake0[i]] = 0;
				Cells[NextTake1[i]] = sof;

				int nc = Convert.ToInt32(NextTakeList[i]);
				Cells[nc] = 0;
				
				NextStepValue[i] = PostEvaluation();

				if (NextStepValue[i] > oc)
				{
					best = i;
					oc = NextStepValue[i];
				}
				//Cells[NextStep0[i]] = 2;
				//Cells[NextStep1[i]] = 0;

				for (i = 0; i < 32; i++)
				{
					Cells[i] = Cells_c[i];

				}
			}

			return (best);
		}
		int GiveBestTake(int nc)
		{
			int[] CellBT = new int[4];
			int[] CellBTR = new int[4];
			int[] CellBTT = new int[4];
			int best = -1;
			if (Cells[nc] != 2) return (best);
			int[] Cells_c = new int[32];
			int sost_c;
			for (int i = 0; i < 32; i++)
			{
				Cells_c[i] = Cells[i];

			}

			sost_c = sost;
			for (int i = 0; i < 4; i++) CellBTR[i] = -100;
			for (int dir = 0; dir < 4; dir++)
			{
				int vn = GiveNextCell(nc, dir);
				if (vn == 1 || vn == 3)
				{
					int nc1 = GiveNextCellN(nc, dir);
					int vn2 = GiveNextCell(nc1, dir);
					if (vn2 == 0)
					{
						int nc2 = GiveNextCellN(nc1, dir);
						CellBT[dir] = nc2;
						CellBTR[dir] = PostEvaluation(1);
						CellBTT[dir] = nc1;
						for (int i = 0; i < 32; i++)
						{
							Cells[i] = Cells_c[i];

						}
					}
				}
			}
			int p = -100;
			int bt = -1;
			for (int i = 0; i < 4; i++)
			{
				if (CellBTR[i] > p)
				{
					p = CellBTR[i];
					bt = CellBT[i];
					AutoTakeCell = CellBTT[i];
				}
			}
			return (bt);
		}
		int GiveBestTakeD(int nc)
		{
			int[] CellBT = new int[4];
			int[] CellBTR = new int[4];
			int[] CellBTT = new int[4];
			int best = -1;
			if (Cells[nc] != 4) return (best);
			int[] Cells_c = new int[32];
			int sost_c;
			for (int i = 0; i < 32; i++)
			{
				Cells_c[i] = Cells[i];

			}

			sost_c = sost;
			for (int i = 0; i < 4; i++) CellBTR[i] = -100;
			for (int dir = 0; dir < 4; dir++)
			{
				int vn = GiveNextCell(nc, dir);
				if (vn == 1 || vn == 3)
				{
					int nc1 = GiveNextCellN(nc, dir);
					int vn2 = GiveNextCell(nc1, dir);
					if (vn2 == 0)
					{
						int nc2 = GiveNextCellN(nc1, dir);
						CellBT[dir] = nc2;
						CellBTR[dir] = PostEvaluation(1);
						CellBTT[dir] = nc1;
						for (int i = 0; i < 32; i++)
						{
							Cells[i] = Cells_c[i];

						}
					}
				}
			}
			int p = -100;
			int bt = -1;
			for (int i = 0; i < 4; i++)
			{
				if (CellBTR[i] > p)
				{
					p = CellBTR[i];
					bt = CellBT[i];
					AutoTakeCell = CellBTT[i];
				}
			}
			return (bt);
		}
		void BuildStepList(int col)
		{
			NextStepCount = 0;
			NextTakeCount = 0;
			NextDStepCount = 0;
			NextDTakeCount = 0;
			DiagDTakeCount = 0;
			if (col == 0)
			{
				for (int i = 0; i < 32; i++)
				{
					if (Cells[i] == 1)
					{
						if (GiveNextCell(i, 3) == 0)
						{
							int nc = GiveNextCellN(i, 3);
							NextStep0[NextStepCount] = i;
							NextStep1[NextStepCount] = nc;
							NextStepCount++;
						}

						if (GiveNextCell(i, 0) == 0)
						{
							int nc = GiveNextCellN(i, 0);
							NextStep0[NextStepCount] = i;
							NextStep1[NextStepCount] = nc;
							NextStepCount++;
						}
					}

				}
			}


			if (col == 1)
				
			{
				if (!IsTakeAll(2))
				{
					for (int i = 0; i < 32; i++)
					{
						if (Cells[i] == 2)
						{
							if (GiveNextCell(i, 1) == 0)
							{
								int nc = GiveNextCellN(i, 1);
								NextStep0[NextStepCount] = i;
								NextStep1[NextStepCount] = nc;
								NextStepCount++;
							}

							if (GiveNextCell(i, 2) == 0)
							{
								int nc = GiveNextCellN(i, 2);
								NextStep0[NextStepCount] = i;
								NextStep1[NextStepCount] = nc;
								NextStepCount++;
							}
						}
						if (Cells[i]==4)
                        {
							int j = i;
							bool b = true;
							while (b)
                            {
								if (GiveNextCell(j, 1) == 0)
                                {
									int nc = GiveNextCellN(j, 1);
									NextStep0[NextStepCount] = i;
									NextStep1[NextStepCount] = nc;
									NextStepCount++;
									j = nc;
								}
								else
                                {
									b = false;
                                }

							}
							b = true;
							j = i;
							while (b)
							{
								if (GiveNextCell(j, 0) == 0)
								{
									int nc = GiveNextCellN(j, 0);
									NextStep0[NextStepCount] = i;
									NextStep1[NextStepCount] = nc;
									NextStepCount++;
									j = nc;
								}
								else
								{
									b = false;
								}

							}
							b = true;
							j = i;
							while (b)
							{
								if (GiveNextCell(j, 2) == 0)
								{
									int nc = GiveNextCellN(j, 2);
									NextStep0[NextStepCount] = i;
									NextStep1[NextStepCount] = nc;
									NextStepCount++;
									j = nc;
								}
								else
								{
									b = false;
								}

							}
							b = true;
							j = i;
							while (b)
							{
								if (GiveNextCell(j, 3) == 0)
								{
									int nc = GiveNextCellN(j, 3);
									NextStep0[NextStepCount] = i;
									NextStep1[NextStepCount] = nc;
									NextStepCount++;
									j = nc;
								}
								else
								{
									b = false;
								}

							}
						}

					}
				}
				else   //  Блок для взятия.
				{
					for (int i = 0; i < 32; i++)
					{
						if (Cells[i] == 2)
						{
							int ncc = GiveNextCell(i, 1);

							if (ncc == 1 || ncc == 3)
							{
								int nc = GiveNextCellN(i, 1);
								if (GiveNextCell(nc, 1) == 0)
								{
									int nc1 = GiveNextCellN(nc, 1);
									NextTake0[NextTakeCount] = i;
									NextTake1[NextTakeCount] = nc1;
									NextTakeList[NextTakeCount] = Convert.ToString(nc);
									NextTakeCount++;
								}
							}

							ncc = GiveNextCell(i, 2);
							if (ncc == 1 || ncc == 3)
							{
								int nc = GiveNextCellN(i, 2);
								if (GiveNextCell(nc, 2) == 0)
								{
									int nc1 = GiveNextCellN(nc, 2);
									NextTake0[NextTakeCount] = i;
									NextTake1[NextTakeCount] = nc1;
									NextTakeList[NextTakeCount] = Convert.ToString(nc);
									NextTakeCount++;
								}
							}

							ncc = GiveNextCell(i, 0);
							if (ncc == 1 || ncc == 3)
							{
								int nc = GiveNextCellN(i, 0);
								if (GiveNextCell(nc, 0) == 0)
								{
									int nc1 = GiveNextCellN(nc, 0);
									NextTake0[NextTakeCount] = i;
									NextTake1[NextTakeCount] = nc1;
									NextTakeList[NextTakeCount] = Convert.ToString(nc);
									NextTakeCount++;
								}
							}


							ncc = GiveNextCell(i, 3);
							if (ncc == 1 || ncc == 3)
							{
								int nc = GiveNextCellN(i, 3);
								if (GiveNextCell(nc, 3) == 0)
								{
									int nc1 = GiveNextCellN(nc, 3);
									NextTake0[NextTakeCount] = i;
									NextTake1[NextTakeCount] = nc1;
									NextTakeList[NextTakeCount] = Convert.ToString(nc);
									NextTakeCount++;
								}
							}
						}
						if (Cells[i]==4)
                        {   //  Взятие дамкой

							for (int d=0;d<4;d++)
                            {
								bool b = true;
								bool bs = false;
								int nct = -1;
								int j = i;
								while (b)
                                {
									int nc = GiveNextCell(j, d);
									if (nc==0)
                                    {
										int ncn = GiveNextCellN(j, d);
										if (bs)
                                        {
											bool bt = false;
											if (DiagDTakeCount>0)
                                            {
												for (int ii=0;ii<DiagDTakeCount;ii++)
                                                {
													if (DiagDTake[ii]==ncn) bt = true;
                                                }
                                            }
											else
                                            {
												bt = true;
                                            }
											if (bt)
											{
												NextTake0[NextTakeCount] = i;
												NextTake1[NextTakeCount] = ncn;
												NextTakeList[NextTakeCount] = Convert.ToString(nct);
												NextTakeCount++;
											
											}
											j = ncn;
										}
										else
                                        {
											j = ncn;
                                        }

                                    }
									else
                                    {
										if ((nc==1)||(nc==3))
                                        {
											if (!bs)
											{
												int ncn = GiveNextCellN(j, d);
												nct = ncn;
												bs = true;
												j = ncn;
												BuildListTakeDDir(ncn, d);
											}
											else
                                            {
												b = false;
                                            }
                                        }
										else
                                        {
											b = false;
                                        }
                                    }
                                }

                            }
                        }// Конец блока взятия дамкой.

					}
				}
			}
		}
		void BuildListTakeDDir(int nc, int dir)
        {
			//   функция строит список ячеек на диагонали после ячейки nc в направлении dir, где есть взятие дамкой.
			//   Информация заносится в массив DiagDTake[8], количество таких полей - DiagDTakeCount
			//   При наличии такого списка недопустимы другие поля на диагонали для хода. Используется для компьютера, играющего черными
			bool b = true;
			int k = 0;
			DiagDTakeCount = 0;
			int na = nc;
			int nf = nc;
			int sof = Cells[nc];
			Cells[nf] = 0;
			while (b)
            {
				int dc1 = GiveNextCell(na, dir);
				if (dc1==0)
                {
					int nc1 = GiveNextCellN(na, dir);
					Cells[nc1] = 4;
					if (IsTakeD(nc1))
                    {
						DiagDTake[DiagDTakeCount] = nc1;
						DiagDTakeCount++;
                    }
					Cells[nc1] = 0;
					na = nc1;
                }
				else
                {
					b = false;
                }
				k++;
				if (k > 8) b = false;
            }
			Cells[nf] = sof;
		}
		void CheckGameEnd()
		{
			int b = 0;
			int w = 0;
			for (int i = 0; i < 32; i++)
			{
				if (Cells[i] == 1) w++;
				if (Cells[i] == 2) b++;
				if (Cells[i] == 3) w++;
				if (Cells[i] == 4) b++;
			}
			if (w == 0)
			{
				label3.Text = "Игра окончена. Черные победили!";
				GH.AddGameList(MC0, MC1, TC, MCCount, 1, GameStatus);
				SaveGame(1);
			}

			if (b == 0)
			{
				label3.Text = "Игра окончена. Белые победили!";
				GH.AddGameList(MC0, MC1, TC, MCCount, 0, GameStatus);
				SaveGame(0);
			}
		}

		void SaveGame(int c)
		{
			string path = Directory.GetCurrentDirectory() + "//Games//";
			string f = "G" + Convert.ToString(DateTime.Now);
			string fn = path + f + ".gam";
		}

		int GiveCell(int x, int y)
		{
			int n = -1;
			if (x > 10 && y > 10)
			{
				int r = (y - y0) / CellsWidth;
				int c = (x - x0) / CellsWidth;
				if (r > 7 || c > 7) return (-1);
				int nc = c + 8 * r;
				if (r == 0 || r == 2 || r == 4 || r == 6)
				{
					if (c == 0 || c == 2 || c == 4 || c == 6) return (-1);
					n = nc / 2;
				}
				else
				{
					if (c == 1 || c == 3 || c == 5 || c == 7) return (-1);
					n = nc / 2;
				}
			}

			return (n);
		}

		bool Take(int col, int cell0, int cell1) //взятие
		{
			if (col == 0)
			{
				if (Cells[cell0] == 1)
				{
					int gnc = GiveNextCell(cell0, 0);
					if (gnc == 2 || gnc == 4)
					{
						int c1 = GiveNextCellN(cell0, 0);
						if (GiveNextCell(c1, 0) == 0)
						{
							int c2 = GiveNextCellN(c1, 0);
							if (GiveNextCellN(c1, 0) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 1;
								Cells[c1] = 0;
								EatSound.Play();
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								return (true);
							}
						}
					}


					gnc = GiveNextCell(cell0, 1);
					if (gnc == 2 || gnc == 4)
					{
						int c1 = GiveNextCellN(cell0, 1);
						if (GiveNextCell(c1, 1) == 0)
						{
							int c2 = GiveNextCellN(c1, 1);
							if (GiveNextCellN(c1, 1) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 1;
								Cells[c1] = 0;
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								EatSound.Play();
								return (true);
							}
						}
					}

					gnc = GiveNextCell(cell0, 2);
					if (gnc == 2 || gnc == 4)
					{
						int c1 = GiveNextCellN(cell0, 2);
						if (GiveNextCell(c1, 2) == 0)
						{
							int c2 = GiveNextCellN(c1, 2);
							if (GiveNextCellN(c1, 2) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 1;
								Cells[c1] = 0;
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								EatSound.Play();
								return (true);
							}
						}
					}

					gnc = GiveNextCell(cell0, 3);
					if (gnc == 2 || gnc == 4)
					{
						int c1 = GiveNextCellN(cell0, 3);
						if (GiveNextCell(c1, 3) == 0)
						{
							int c2 = GiveNextCellN(c1, 3);
							if (GiveNextCellN(c1, 3) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 1;
								Cells[c1] = 0;
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								EatSound.Play();
								return (true);
							}
						}
					}

				}
				else
				{
					if (Cells[cell0] == 3)
					{
						if (IsTakeD(cell0))
						{
							if (TakeD(col, cell0, cell1))
							{
								OutBoard();
								EatSound.Play();
								return (true);
							}
							else
							{
								return (false);
							}
						}
						else
						{
							return (false);
						}

					}
					else
					{
						return (false);
					}
				}
			}


			if (col == 1)
			{
				if (Cells[cell0] == 2)
				{
					int gnc = GiveNextCell(cell0, 0);
					if (gnc == 1 || gnc == 3)
					{
						int c1 = GiveNextCellN(cell0, 0);
						if (GiveNextCell(c1, 0) == 0)
						{
							int c2 = GiveNextCellN(c1, 0);
							if (GiveNextCellN(c1, 0) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 2;
								Cells[c1] = 0;
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								EatSound.Play();
								return (true);
							}
						}
					}

					gnc = GiveNextCell(cell0, 1);
					if (gnc == 1 || gnc == 3)
					{
						int c1 = GiveNextCellN(cell0, 1);
						if (GiveNextCell(c1, 1) == 0)
						{
							int c2 = GiveNextCellN(c1, 1);
							if (GiveNextCellN(c1, 1) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 2;
								Cells[c1] = 0;
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								EatSound.Play();
								return (true);
							}
						}
					}

					gnc = GiveNextCell(cell0, 2);
					if (gnc == 1 || gnc == 3)
					{
						int c1 = GiveNextCellN(cell0, 2);
						if (GiveNextCell(c1, 2) == 0)
						{
							int c2 = GiveNextCellN(c1, 2);
							if (GiveNextCellN(c1, 2) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 2;
								Cells[c1] = 0;
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								EatSound.Play();
								return (true);
							}
						}
					}

					gnc = GiveNextCell(cell0, 3);
					if (gnc == 1 || gnc == 3)
					{
						int c1 = GiveNextCellN(cell0, 3);
						if (GiveNextCell(c1, 3) == 0)
						{
							int c2 = GiveNextCellN(c1, 3);
							if (GiveNextCellN(c1, 3) == cell1)
							{
								Cells[cell0] = 0;
								Cells[cell1] = 2;
								Cells[c1] = 0;
								TC[MCCount] = Convert.ToString(c1) + ",";
								OutBoard();
								EatSound.Play();
								return (true);
							}
						}
					}

					return (false);
				}
				else
				{
					if (Cells[cell0] == 4)
					{
						if (IsTakeD(cell0))
						{
							if (TakeD(col, cell0, cell1))
							{
								OutBoard();
								EatSound.Play();
								return (true);
							}
							else
							{
								return (false);
							}
						}
						else
						{
							return (false);
						}

					}
					else
					{
						return (false);
					}
				}
			}
			return (false);
		}


		bool TakeD(int col, int cell0, int cell1) //взятие у дамок
		{
			int ncell = cell0;
			int[] CellD = new int[32];
			int ncTake = -1;
			int[] TakeList = new int[32];
			int TakeCount = 0;
			if (Cells[ncell] == 3)
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;
					bool bt = false;
					int nc = ncell;

					while (bc)
					{
						if (!bt)
						{
							int nextc = GiveNextCell(nc, dir);
							if (nextc == 2 || nextc == 4)
							{
								bt = true;
								nc = GiveNextCellN(nc, dir);
								ncTake = nc;
								TakeCount = 0;
								int jn = nc;
								bool b2 = true;
								while (b2)
								{
									if (GiveNextCell(jn, dir) == 0)
									{
										int ncj = GiveNextCellN(jn, dir);
										for (int ii = 0; ii < 32; ii++) CellD[ii] = Cells[ii];
										Cells[ncj] = 3;
										Cells[cell0] = 0;
										Cells[ncTake] = 0;
										if (IsTakeD(ncj))
										{
											TakeList[TakeCount] = ncj;
											TakeCount++;
										}

										for (int ii = 0; ii < 32; ii++) Cells[ii] = CellD[ii];
										jn = ncj;

									}
									else
									{
										b2 = false;
									}

								}

							}
							else
							{
								if (nextc == 0)
								{
									nc = GiveNextCellN(nc, dir);
								}
								else
								{
									bc = false;
								}
							}
						}
						else
						{
							if (GiveNextCell(nc, dir) == 0)
							{
								if (GiveNextCellN(nc, dir) == cell1)
								{

									if (TakeCount > 0)
									{
										for (int ii = 0; ii < TakeCount; ii++)
										{
											if (cell1 == TakeList[ii])
											{
												Cells[cell1] = Cells[cell0];
												Cells[cell0] = 0;
												Cells[ncTake] = 0;
												return (true);

											}
										}
										return (false);

									}
									Cells[cell1] = Cells[cell0];
									Cells[cell0] = 0;
									Cells[ncTake] = 0;
									return (true);
								}
								nc = GiveNextCellN(nc, dir);
							}

							else
							{
								bc = false;
							}
						}
					}
				}
			}
			if (Cells[ncell] == 4)
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;
					bool bt = false;
					int nc = ncell;
					while (bc)
					{
						if (!bt)
						{
							int nextc = GiveNextCell(nc, dir);
							if (nextc == 1 || nextc == 3)
							{
								bt = true;
								nc = GiveNextCellN(nc, dir);
								ncTake = nc;
								TakeCount = 0;
								int jn = nc;
								bool b2 = true;
								while (b2)
								{
									if (GiveNextCell(jn, dir) == 0)
									{
										int ncj = GiveNextCellN(jn, dir);
										for (int ii = 0; ii < 32; ii++) CellD[ii] = Cells[ii];
										Cells[ncj] = 4;
										Cells[cell0] = 0;
										Cells[ncTake] = 0;
										if (IsTakeD(ncj))
										{
											TakeList[TakeCount] = ncj;
											TakeCount++;
										}

										for (int ii = 0; ii < 32; ii++) Cells[ii] = CellD[ii];
										jn = ncj;

									}
									else
									{
										b2 = false;
									}

								}
							}
							else
							{
								if (nextc == 0)
								{
									nc = GiveNextCellN(nc, dir);
								}
								else
								{
									bc = false;
								}
							}
						}
						else
						{
							if (GiveNextCell(nc, dir) == 0)
							{
								if (GiveNextCellN(nc, dir) == cell1)
								{
									if (TakeCount > 0)
									{
										for (int ii = 0; ii < TakeCount; ii++)
										{
											if (cell1 == TakeList[ii])
											{
												Cells[cell1] = Cells[cell0];
												Cells[cell0] = 0;
												Cells[ncTake] = 0;
												return (true);

											}
										}
										return (false);

									}
									Cells[cell1] = Cells[cell0];
									Cells[cell0] = 0;
									Cells[ncTake] = 0;
									return (true);
								}
								nc = GiveNextCellN(nc, dir);
							}

							else
							{
								bc = false;
							}
						}
					}
				}
			}
			return (false);
		}

		bool checkstep(int col, int cell0, int cell1)
		{
			int p1, p0;
			label3.Text = "";

			string cl = "белые";
			if (col == 1) cl = "черные";
			int r = cell0 / 4;

			if (IsTakeAll(col + 1))

			{
				if (!Take(col, cell0, cell1))
				{
					sost = sost - 2;
					selectedcell = -1;
					OutBoard();
					if (col == 0)
					{
						label3.Text = "Ход белых: взятие обязательно";
						ErrorSound.Play();
					}
					if (col == 1)
					{
						label3.Text = "Ход черных: взятие обязательно";
						ErrorSound.Play();
					}

					return (false);
				}
				else
				{
					p1 = PostEvaluation(1);
					label5.Text = Convert.ToString(p1);
					p0 = PostEvaluation(0);
					label4.Text = Convert.ToString(p0);
					TakeDoes = true;
					return (true);
				}
			}


			label1.Text = cl + "   " + Convert.ToString(cell0) + "   " + Convert.ToString(cell1) + "   " + Convert.ToString(r);
			if (col == 0)
			{

				if (Cells[cell0] == 1)
				{
					if (cell0 < 4) return (false);
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						if (cell1 == cell0 - 3) return (true);
						if (cell1 == cell0 - 4) return (true);
					}
					else
					{
						if (cell1 == cell0 - 5) return (true);
						if (cell1 == cell0 - 4) return (true);
					}
					label3.Text = "Ход белых: недопустимый ход";
					ErrorSound.Play();
					selectedcell = -1;
					sost = 0;
					return (false);
				}
				if (Cells[cell0] == 3)
				{
					if (StepD(col, cell0, cell1))
					{
						p1 = PostEvaluation(1);
						label5.Text = Convert.ToString(p1);
						p0 = PostEvaluation(0);
						label4.Text = Convert.ToString(p0);
						return (true);
					}
					else
					{
						label3.Text = "Ход белых: недопустимый ход";
						ErrorSound.Play();
						selectedcell = -1;
						sost = 0;
						return (false);
					}
				}

			}


			if (col == 1)
			{
				if (Cells[cell0] == 2)
				{
					if (cell0 > 27) return (false);
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						if (cell1 == cell0 + 4) return (true);
						if (cell1 == cell0 + 5) return (true);
					}
					else
					{
						if (cell1 == cell0 + 3) return (true);
						if (cell1 == cell0 + 4) return (true);
					}
					label3.Text = "Ход черных: недопустимый ход";
					ErrorSound.Play();
					selectedcell = -1;
					sost = 1;
					return (false);
				}
				if (Cells[cell0] == 3)
				{
					if (StepD(col, cell0, cell1))
					{
						p1 = PostEvaluation(1);
						label5.Text = Convert.ToString(p1);
						p0 = PostEvaluation(0);
						label4.Text = Convert.ToString(p0);
						return (true);
					}
					else
					{
						label3.Text = "Ход черных: недопустимый ход";
						ErrorSound.Play();
						selectedcell = -1;
						sost = 0;
						return (false);
					}
				}
			}
			p1 = PostEvaluation(1);
			label5.Text = Convert.ToString(p1);
			p0 = PostEvaluation(0);
			label4.Text = Convert.ToString(p0);
			return (true);
		}

		bool IsTakeAll(int col)
		{
			for (int i = 0; i < 32; i++)
			{
				if (Cells[i] == col)
				{
					if (IsTake(i))
					{
						int nc2 = i;
						return (true);
					}

				}

				if (Cells[i] == col + 2)
				{
					if (IsTake(i))
					{
						int nc2 = i;
						return (true);
					}

				}

			}
			return (false);
		}
		int GiveNextCell(int ncell, int ndir)
		{
			//0 - пустая ячейка
			//1 - белая шашка
			//2 - черная шашка
			//3 - белая дамка
			//4 - черная дамка
			//-1 - нет соседа в этом направлении
			int r = ncell / 4;
			int c = ncell - 4 * r;
			if (ndir == 0)
			{
				if (r == 0) return (-1);
				if (r == 2 || r == 4 || r == 6)
				{
					if (c == 3) return (-1);
					return (Cells[ncell - 3]);
				}
				if (r == 1 || r == 3 || r == 5 || r == 7)
				{
					return (Cells[ncell - 4]);

				}
			}
			if (ndir == 1)
			{
				if (r == 7) return (-1);
				if (r == 0 || r == 2 || r == 4 || r == 6)
				{
					if (c == 3) return (-1);
					return (Cells[ncell + 5]);
				}
				if (r == 1 || r == 3 || r == 5)
				{
					return (Cells[ncell + 4]);
				}
			}

			if (ndir == 2)
			{
				if (r == 7) return (-1);
				if (r == 0 || r == 2 || r == 4 || r == 6)
				{
					return (Cells[ncell + 4]);
				}
				if (c == 0) return (-1);
				if (r == 1 || r == 3 || r == 5)
				{
					return (Cells[ncell + 3]);
				}
			}

			if (ndir == 3)
			{
				if (r == 0) return (-1);
				if (r == 2 || r == 4 || r == 6)
				{
					return (Cells[ncell - 4]);
				}
				if (c == 0) return (-1);

				if (r == 1 || r == 3 || r == 5 || r == 7)
				{
					return (Cells[ncell - 5]);
				}
			}


			return (-1);
		}

		int GiveNextCellN(int ncell, int ndir)
		{
			//0 - пустая ячейка
			//1 - белая шашка
			//2 - черная шашка
			//3 - белая дамка
			//4 - черная дамка
			//-1 - нет соседа в этом направлении
			int r = ncell / 4;
			int c = ncell - 4 * r;
			if (ndir == 0)
			{
				if (r == 0) return (-1);
				if (r == 2 || r == 4 || r == 6)
				{
					if (c == 3) return (-1);
					//return (Cells[ncell - 3]);
					return (ncell - 3);
				}
				if (r == 1 || r == 3 || r == 5 || r == 7)
				{
					//return (Cells[ncell - 4]);
					return (ncell - 4);

				}
			}
			if (ndir == 1)
			{
				if (r == 7) return (-1);
				if (r == 0 || r == 2 || r == 4 || r == 6)
				{
					if (c == 3) return (-1);
					//return (Cells[ncell + 5]);
					return (ncell + 5);
				}
				if (r == 1 || r == 3 || r == 5)
				{
					//return (Cells[ncell + 4]);
					return (ncell + 4);
				}
			}

			if (ndir == 2)
			{
				if (r == 7) return (-1);
				if (r == 0 || r == 2 || r == 4 || r == 6)
				{
					//return (Cells[ncell + 4]);
					return (ncell + 4);
				}
				if (c == 0) return (-1);
				if (r == 1 || r == 3 || r == 5)
				{
					//return (Cells[ncell + 3]);
					return (ncell + 3);
				}
			}

			if (ndir == 3)
			{
				if (r == 0) return (-1);

				if (r == 2 || r == 4 || r == 6)
				{
					//return (Cells[ncell - 4]);
					return (ncell - 4);
				}
				if (c == 0) return (-1);
				if (r == 1 || r == 3 || r == 5 || r == 7)
				{

					//return (Cells[ncell - 5]);
					return (ncell - 5);
				}
			}


			return (-1);
		}
		bool IsTake(int ncell)
		{
			int r = ncell / 4;
			int col = ncell - 4 * r;
			if (Cells[ncell] == 0) return (false);
			if (Cells[ncell] == 1)
			{
				int gnc = GiveNextCell(ncell, 0);
				if (gnc == 2 || gnc == 4)

				{

					int ncell2 = ncell - 4;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell - 3;
					}
					if (GiveNextCell(ncell2, 0) == 0) return (true);
					
				}

				gnc = GiveNextCell(ncell, 1);

				if (gnc == 2 || gnc == 4)
				{
					int ncell2 = ncell + 4;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell + 5;
					}
					if (GiveNextCell(ncell2, 1) == 0) return (true);

				}
				gnc = GiveNextCell(ncell, 2);

				if (gnc == 2 || gnc == 4)
				{
					int ncell2 = ncell + 3;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell + 4;
					}
					if (GiveNextCell(ncell2, 2) == 0) return (true);
				}
				gnc = GiveNextCell(ncell, 3);

				if (gnc == 2 || gnc == 4)
				{
					int ncell2 = ncell - 5;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell - 4;
					}
					if (GiveNextCell(ncell2, 3) == 0)
					{
						int nc2 = ncell;
						return (true);
					}

				}
			}


			if (Cells[ncell] == 2)
			{
				int gnc = GiveNextCell(ncell, 0);
				if (gnc == 1 || gnc == 3)

				{

					int ncell2 = ncell - 4;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell - 3;
					}
					if (GiveNextCell(ncell2, 0) == 0) return (true);

				}

				gnc = GiveNextCell(ncell, 1);

				if (gnc == 1 || gnc == 3)
				{
					int ncell2 = ncell + 4;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell + 5;
					}
					if (GiveNextCell(ncell2, 1) == 0) return (true);

				}
				gnc = GiveNextCell(ncell, 2);

				if (gnc == 1 || gnc == 3)
				{
					int ncell2 = ncell + 3;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell + 4;
					}
					if (GiveNextCell(ncell2, 2) == 0) return (true);
				}
				gnc = GiveNextCell(ncell, 3);

				if (gnc == 1 || gnc == 3)
				{
					int ncell2 = ncell - 5;
					if (r == 0 || r == 2 || r == 4 || r == 6)
					{
						ncell2 = ncell - 4;
					}
					if (GiveNextCell(ncell2, 3) == 0) return (true);
				}
			}
			if (Cells[ncell] == 3)
			{
				return (IsTakeD(ncell));
			}

			if (Cells[ncell] == 4)
			{
				return (IsTakeD(ncell));
			}


			return (false);
		}

		bool IsTakeD(int ncell)
		{
			if (Cells[ncell] == 3)
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;
					bool bt = false;
					int nc = ncell;
					while (bc)
					{
						if (!bt)
						{
							int nextc = GiveNextCell(nc, dir);
							if (nextc == 2 || nextc == 4)
							{
								bt = true;
								nc = GiveNextCellN(nc, dir);
							}
							else
							{
								if (nextc == 0)
								{
									nc = GiveNextCellN(nc, dir);
								}
								else
								{
									bc = false;
								}
							}
						}
						else
						{
							if (GiveNextCell(nc, dir) == 0)
							{
								return (true);
							}
							else
							{
								bc = false;
							}
						}
					}
				}
			}
			if (Cells[ncell] == 4)
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;
					bool bt = false;
					int nc = ncell;
					while (bc)
					{
						if (!bt)
						{
							int nextc = GiveNextCell(nc, dir);
							if (nextc == 1 || nextc == 3)
							{
								bt = true;
								nc = GiveNextCellN(nc, dir);
							}
							else
							{
								if (nextc == 0)
								{
									nc = GiveNextCellN(nc, dir);
								}
								else
								{
									bc = false;
								}
							}
						}
						else
						{
							if (GiveNextCell(nc, dir) == 0)
							{
								return (true);
							}
							else
							{
								bc = false;
							}
						}
					}
				}
			}
			return (false);
		}

		bool StepTakeD(int col, int ncell, int nc2)
		{
			if ((Cells[ncell] == 3) && (col == 0))
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;
					bool bt = false;
					int nc = ncell;
					while (bc)
					{
						if (!bt)
						{
							int nextc = GiveNextCell(nc, dir);
							if (nextc == 2 || nextc == 4)
							{
								bt = true;
								nc = GiveNextCellN(nc, dir);
							}
							else
							{
								if (nextc == 0)
								{
									nc = GiveNextCellN(nc, dir);
								}
								else
								{
									bc = false;
								}
							}
						}
						else
						{
							if (GiveNextCell(nc, dir) == 0)
							{
								if (GiveNextCellN(nc, dir) == nc2)
								{
									return (true);
								}
								else
								{
									nc = GiveNextCellN(nc, dir);
								}
							}
							else
							{
								bc = false;
							}
						}
					}
				}
			}
			if (Cells[ncell] == 4)
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;
					bool bt = false;
					int nc = ncell;
					while (bc)
					{
						if (!bt)
						{
							int nextc = GiveNextCell(nc, dir);
							if (nextc == 1 || nextc == 3)
							{
								bt = true;
								nc = GiveNextCellN(nc, dir);
							}
							else
							{
								if (nextc == 0)
								{
									nc = GiveNextCellN(nc, dir);
								}
								else
								{
									bc = false;
								}
							}
						}
						else
						{
							if (GiveNextCell(nc, dir) == 0)
							{
								return (true);
							}
							else
							{
								bc = false;
							}
						}
					}
				}
			}
			return (false);
		}
		bool StepD(int col, int ncell, int nc2)
		{
			if ((Cells[ncell] == 3) && (col == 0))
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;

					int nc = ncell;
					while (bc)
					{

						int nextc = GiveNextCell(nc, dir);

						if (nextc == 0)
						{
							if (GiveNextCellN(nc, dir) == nc2)
							{
								return (true);

							}
							else
							{
								nc = GiveNextCellN(nc, dir);
							}
						}
						else
						{
							bc = false;
						}


					}
				}
			}
			if (Cells[ncell] == 4)
			{
				for (int dir = 0; dir < 4; dir++)
				{
					bool bc = true;

					int nc = ncell;
					while (bc)
					{

						int nextc = GiveNextCell(nc, dir);

						if (nextc == 0)
						{
							if (GiveNextCellN(nc, dir) == nc2)
							{
								return (true);

							}
							else
							{
								nc = GiveNextCellN(nc, dir);
							}
						}
						else
						{
							bc = false;
						}


					}

				}
			}
			return (false);
		}



		string CellToString(int ncell)
		{
			return (snum[ncell]);
			
			//listBox1.Items.Add(snum[ncell]);
			
		}

		private void ManVSMan_Click(object sender, EventArgs e)
		{
			GameStatus = 0;
			PanelMenu.Visible = false;
		}

		private void Comp0_Click(object sender, EventArgs e)
		{
			GameStatus = 1;
			PanelMenu.Visible = false;
		}

		int PostEvaluation(int col = 1)
		{
			int pos = 0;
			if (col == 0)
			{
				for (int i = 0; i < 32; i++)
				{
					if (Cells[i] == 1)
					{
						pos = pos + 10;
					}
					
					if (Cells[i] == 2)
					{
						pos = pos - 10;
					}

					if (Cells[i] == 3)
					{
						pos = pos + 100;
					}

					if (Cells[i] == 4)
					{
						pos = pos - 100;
					}
				}
				if (IsTakeAll(2))
				{
					pos = pos - 10;
				}
			}


			if (col == 1)
			{
				for (int i = 0; i < 32; i++)
				{
					if (Cells[i] == 1)
					{
						pos = pos - 10;
					}

					if (Cells[i] == 2)
					{
						pos = pos + 10;
					}

					if (Cells[i] == 3)
					{
						pos = pos - 100;
					}

					if (Cells[i] == 4)
					{
						pos = pos + 100;
					}
				}
				if (IsTakeAll(1))
				{
					pos = pos - 10;
				}
			}
			return (pos);
		}

		private void Form1_LocationChanged(object sender, EventArgs e)
		{
			OutBoard();
		}

		private void Comp1_Click(object sender, EventArgs e)
		{
			GameStatus = 2;
		}

		private void Comp2_Click(object sender, EventArgs e)
		{
			GameStatus = 3;
			learnstatus = true;
			panel1.BackColor = Color.Lime;
		}
	}

	class GameHist
	{
		public int[] HStep = new int[100];
		public int[] HStep1 = new int[100];
		public int[] HResult = new int[100];
		public string[] HStepT = new string[100];
		public int HStepCount;
		public string HistFN;
		public void AddGameList(int[] MC0, int[] MC1, string[] TC, int MCCount, int win, int GameStatus)
		{
			string d = Directory.GetCurrentDirectory();
			string a;
			string fn = d + "\\temp.sas";
			try
			{
				StreamReader rd = new StreamReader (HistFN);
				StreamWriter wr = new StreamWriter(fn);
				int bv = 0;
				while (!rd.EndOfStream)
				{
					a = rd.ReadLine();
					wr.WriteLine(a);
					bv++;
					
				}
				rd.Close();
				DateTime dt; //= new DateTime(DateTime);
				dt = DateTime.Now;
				a = Convert.ToString(dt) + "/" + Convert.ToString(win) + "/" + Convert.ToString(GameStatus) + "/" + Convert.ToString(bv) + "/";
				for (int i = 0; i < MCCount; i++)
				{
					a = a + Convert.ToString(MC0[i]) + "-" + Convert.ToString(MC1[i]) + "-" + TC[i] + ";";
				}
				wr.WriteLine(a);
				//Directory.Delete(HistFN);
				File.Delete(HistFN);
				wr.Close();
				File.Move(fn, HistFN);
				File.Delete(fn);
			}

			catch (Exception e)
			{
				//string b = "не нашел файл истории";
				StreamWriter wr = new StreamWriter(HistFN);
				DateTime dt; //= new DateTime(DateTime);
				dt = DateTime.Now;
				a = Convert.ToString(dt) + "/" + Convert.ToString(win) + "/" + Convert.ToString(GameStatus) + "/";
				for (int i = 0; i < MCCount; i++)
				{
					a = a + Convert.ToString(MC0[i]) + "-" + Convert.ToString(MC1[i]) + "-" + TC[i] + ";";
				}
				wr.WriteLine(a);
				wr.Close();

			}
			
			
			
		}
		public int BaseVolume()
		{
			StreamReader rd = new StreamReader(HistFN);
			int n = 0;
			while (!rd.EndOfStream)
			{
				string s = rd.ReadLine();
				n++;
			}
			rd.Close();
			return (n);
		}

		public int GiveBestStepFromBase(int[] MC0, int[] MC1, string[] TC, int MCCount)
		{
			string sh0 = "";
			string sh1 = "";
			string sh2 = "";
			char d = Convert.ToChar(";");
			char d2 = Convert.ToChar("/");
			char d3 = Convert.ToChar(",");
			char d1 = Convert.ToChar("-");
			HStepCount = 0;
			int rcount = 0;
			StreamReader rd = new StreamReader(HistFN);
			while (!rd.EndOfStream)
			{
				string s = rd.ReadLine();
				rcount++;
				string s1 = GiveEl(s, 4, d2);
				string sr = GiveEl(s, 1, d2);
				int isr = Convert.ToInt32(sr);
				bool b = true;
				int ns = 0;
				bool pis = false;
				while (b)
				{
					

					string s2 = GiveEl(s1, ns, d);

					if (s2 != "")
					{
						sh0 = GiveEl(s2, 0, d1);
						sh1 = GiveEl(s2, 1, d1);
						sh2 = GiveEl(s2, 2, d1);
						int ish0 = Convert.ToInt32(sh0);
						int ish1 = Convert.ToInt32(sh1);

						if (ns == MCCount)
						{
							bool ba = true;
							for (int i = 0; i < HStepCount; i++)
							{
								if (ish0 == HStep[i] && ish1 == HStep1[i])
								{
									if (isr == 0)
									{
										HResult[i] --;
									}
									else 
									{
										HResult[i]++;
									}
									ba = false;
								}
							}
							if (ba)
							{ 
							//в дальнейшем проверить взятие
							HStep[HStepCount] = Convert.ToInt32(sh0);
							HStep1[HStepCount] = Convert.ToInt32(sh1);
								HStepT[HStepCount] = sh2;
							//HResult[HStepCount] = Convert.ToInt32(sr);

								if (isr == 0)
								{
									HResult[HStepCount] = -1;
								}
								else 
								{
									HResult[HStepCount] = 1;
								}

							HStepCount++;
							}
							b = false;
						}

						if (Convert.ToInt32(sh0) == MC0[ns])
						{
							if (Convert.ToInt32(sh1) == MC1[ns])
							{
								ns++;
							}
							else
							{
								b = false;
							}

						}
						else
						{
							b = false;
						}

						if (ns > 200)
						{
							b = false;
						}
						
					}
					else
					{
						b = false;
					}
				}
			}
			int rrcount = rcount + 1;
			rd.Close();
			if (HStepCount == 0)
			{
				return (-1);
			}
			else
			{
				int ib = -1;
				int ir = -100;
				
				for (int i = 0; i < HStepCount; i++)
				{
					if (HResult[i] > ir)
					{
						ib = i;
						ir = HResult[i];
					}
				}
				if (ir > -100)
				{
					return (ib);
				}
			}
			return (-1);
		}

		string GiveEl(string s, int n, char d)
		{

			int l = 0;
			string os = "";
			int m = s.Length;
			for (int i = 0; i < m; i++)
			{
				if (s[i] == d)
				{
					l++;
					if (l > n) return (os);
				}
				else
				{
					if (l == n) os = os + s[i];
				}
			}
			return (os);

		}
	}


	

}


