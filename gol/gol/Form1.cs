using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace gameOfLife {
    public partial class Form1 : Form {
        private const int cellsX = 100,
                          cellsY = 80,
                          cellH = 12,
                          cellW = 12,
                          cellSp = 2,
                          constSpacing = 14,
                          btnHeight = 30,
                          btnWidth = 100;
        Timer gameTickTimer;
        List<List<Cell>> cellList;
        Button btnFileDialogueRead = new Button();
        Button btnFileDialogueWrite = new Button();
        Button btnClear = new Button();

        public Form1() {
            InitializeComponent();
            //this.SetBounds(0 - cellSpacing, 0 - cellSpacing, (cellWidth + cellSpacing) * cellsX, (cellHeight + cellSpacing) * cellsY);
            setupForm();
            cellList = setupCells(cellsX, cellsY, cellW, cellH, cellSp);
        }

        private List<List<Cell>> setupCells(int numCellsX, int numCellsY, int cellWidth, int cellHeight, int spacing) {
            List<List<Cell>> cellX = new List<List<Cell>>();
            for (int i = 0; i < numCellsX; i++) {
                List<Cell> cellY = new List<Cell>();
                for (int j = 0; j < numCellsY; j++) {
                    Cell cell = new Cell(i, j, spacing + (spacing + cellWidth) * i, spacing + (spacing + cellHeight) * j, cellWidth, cellHeight);
                    cellY.Add(cell);
                    cell.MouseDown += new MouseEventHandler(cellClickHander);
                    this.Controls.Add(cell);
                    //Console.WriteLine(spacing + (spacing + cellHeight) * j);
                }
                cellX.Add(cellY);
            }
            return cellX;
        }

        private void setupForm() {
            Button btnTimer = new Button();
            btnTimer.Text = "Start";
            btnTimer.Click += new EventHandler(btnTimerClickHandler);
            btnTimer.SetBounds(cellSp, SystemInformation.CaptionHeight + (3 * cellSp) + (cellH + cellSp) * cellsY - btnHeight, btnWidth, btnHeight);
            this.Controls.Add(btnTimer);

            btnFileDialogueRead.Text = "Load Config";
            btnFileDialogueRead.Click += new EventHandler(btnFileClickReadHandler);
            btnFileDialogueRead.SetBounds((cellSp * 2) + btnWidth, SystemInformation.CaptionHeight + (3 * cellSp) + (cellH + cellSp) * cellsY - btnHeight, btnWidth, btnHeight);
            this.Controls.Add(btnFileDialogueRead);

            btnFileDialogueWrite.Text = "Write Config";
            btnFileDialogueWrite.Click += new EventHandler(btnFileClickWriteHandler);
            btnFileDialogueWrite.SetBounds((cellSp * 3) + 2 * btnWidth, SystemInformation.CaptionHeight + (3 * cellSp) + (cellH + cellSp) * cellsY - btnHeight, btnWidth, btnHeight);
            this.Controls.Add(btnFileDialogueWrite);

            btnClear.Text = "Clear Canvas";
            btnClear.Click += new EventHandler(btnClearHandler);
            btnClear.SetBounds((cellSp * 4) + 3 * btnWidth, SystemInformation.CaptionHeight + (3 * cellSp) + (cellH + cellSp) * cellsY - btnHeight, btnWidth, btnHeight);
            this.Controls.Add(btnClear);

            gameTickTimer = new Timer();
            gameTickTimer.Tick += new EventHandler(gameTickHandler);
            gameTickTimer.Interval = 50;


            
            this.Height = SystemInformation.CaptionHeight + constSpacing + (2 * cellSp) + (cellH + cellSp) * cellsY + btnHeight;
            this.Width = constSpacing + (2 * cellSp) + (cellW + cellSp) * cellsX;
            
            
        }

        private void cellClickHander(object sender, MouseEventArgs e) {
            Cell cell = (Cell)sender;
            //Console.WriteLine(cell.row + ", " + cell.col);
            if (!gameTickTimer.Enabled) {
                if (cell.alive) {
                    cell.alive = false;
                } else {
                    cell.alive = true;
                }
            }
        }

        private void btnTimerClickHandler(object sender, EventArgs e) {
            Button btn = (Button)sender;

            if (btn.Text == "Start") {
                btn.Text = "Stop";
                gameTickTimer.Start();
                btnFileDialogueRead.Enabled = false;
                btnFileDialogueWrite.Enabled = false;
                btnClear.Enabled = false;
            } else {
                btn.Text = "Start";
                gameTickTimer.Stop();
                btnFileDialogueRead.Enabled = true;
                btnFileDialogueWrite.Enabled = true;
                btnClear.Enabled = true;
            }
            
        }

        private void btnFileClickReadHandler(object sender, EventArgs e) {
            Button btn = (Button)sender;
            string fn;
            List<string> data = new List<string>();

            OpenFileDialog diag = new OpenFileDialog();
            diag.InitialDirectory = "C:\\";
            diag.Filter = "txt files (*.txt)|*.txt";
            if (diag.ShowDialog() == DialogResult.OK) {
                fn = diag.FileName;
                
                using (StreamReader reader = new StreamReader(fn)) {
                    while (reader.Peek() >= 0) {
                        data.Add(reader.ReadLine());
                        //Console.WriteLine(data[data.Count - 1]);
                    }
                }

                //if ((str = diag.OpenFile()) != null) {
                //    byte[] bytes = new byte[str.Length];
                //    //Console.WriteLine(bytes.Length);

                //    using(str) {
                        
                //    }
                //}
            }

            int x = 0;
            int y = 0;
            foreach(string str in data) {
                char[] chars = str.ToCharArray();
                foreach (char c in chars) {
                    if (c.ToString() == "1") {
                        cellList[y][x].nextState = true;
                    } else {
                        cellList[y][x].nextState = false;
                    }
                    cellList[y][x].update();
                    x++;
                }
                x = 0;
                y++;
            }
        }

        private void btnFileClickWriteHandler(object sender, EventArgs e) {
            Button btn = (Button)sender;
            string fn = "", temp = "";
            SaveFileDialog diag = new SaveFileDialog();
            diag.InitialDirectory = "C:\\";
            diag.Filter = "txt files (*.txt)|*.txt";
            if(diag.ShowDialog() == DialogResult.OK) {
                fn = diag.FileName;
                using (StreamWriter str = new StreamWriter(fn)) {
                    foreach (List<Cell> cells in cellList) {
                        foreach (Cell cell in cells) {
                            if (cell.alive) {
                                temp += "1";
                            } else {
                                temp += "0";
                            }
                        }
                        str.WriteLine(temp);
                        temp = "";
                    }

                }
            }
        }

        private void btnClearHandler(object sender, EventArgs e) {
            foreach (List<Cell> cells in cellList) {
                foreach (Cell cell in cells) {
                    cell.alive = false;
                }
            }
        }

        private void gameTickHandler(object sender, EventArgs e) {
            processFrame();
        }

        private void processFrame() {
            //List<int[]> alive = new List<int[]>();
            //List<List<bool>> alive = new List<List<bool>>();
            //bool living = false;
            //alive.Clear();

            int numAlive;

            foreach (List<Cell> list in cellList) {
                //List<bool> aliveRow = new List<bool>();
                foreach (Cell cell in list) {

                    numAlive = 0;
                    for (int i = cell.row - 1; i < cell.row + 2; i++) {
                        for (int j = cell.col - 1; j < cell.col + 2; j++) {

                            if (i >= 0 && j >= 0 && i < cellsX && j < cellsY) {
                                if (cellList[i][j].alive) {
                                    numAlive++;
                                }
                            }

                        }
                    }
                    if (cell.alive) {
                        numAlive--;
                    }

                    if (numAlive < 2) {
                        //cell.alive = false;
                        //aliveRow.Add(false);
                        cell.nextState = false;
                    } else if (numAlive == 2 && cell.alive) {
                        //cell.alive = true;
                        //aliveRow.Add(true);
                        cell.nextState = true;
                    } else if (numAlive == 3) {
                        //cell.alive = true;
                        //aliveRow.Add(true);
                        cell.nextState = true;
                    } else if (numAlive > 3) {
                        //cell.alive = false;
                        //aliveRow.Add(false);
                        cell.nextState = false;
                    }

                }
                //alive.Add(aliveRow);
            }

            foreach (List<Cell> list in cellList) {
                foreach (Cell cell in list) {
                    cell.update();
                }
            }




                }



    }
}
