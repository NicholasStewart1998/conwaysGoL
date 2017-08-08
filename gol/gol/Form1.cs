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

namespace gameOfLife {
    public partial class Form1 : Form {
        private const int cellsX = 30, cellsY = 30, cellH = 15, cellW = 15, cellSp = 3, outerSpacing = 14;
        public Form1() {
            InitializeComponent();
            //this.SetBounds(0 - cellSpacing, 0 - cellSpacing, (cellWidth + cellSpacing) * cellsX, (cellHeight + cellSpacing) * cellsY);
            this.Height = SystemInformation.CaptionHeight + outerSpacing + 2 * cellSp + (cellH + cellSp) * cellsY;
            this.Width = outerSpacing + 2 * cellSp + (cellW + cellSp) * cellsX;
            setupCells(cellsX, cellsY, cellW, cellH, cellSp);
            
        }

        private void setupCells(int numCellsX, int numCellsY, int cellWidth, int cellHeight, int spacing) {
            List<List<Cell>> cellX = new List<List<Cell>>();
            for (int i = 0; i < numCellsX; i++) {
                List<Cell> cellY = new List<Cell>();
                for (int j = 0; j < numCellsY; j++) {
                    Cell cell = new Cell(i, j, spacing + (spacing + cellWidth) * i, spacing + (spacing + cellHeight) * j, cellWidth, cellHeight);
                    cellY.Add(cell);
                    cell.MouseDown += new MouseEventHandler(cellClickHander);
                    this.Controls.Add(cell);
                    Console.WriteLine(spacing + (spacing + cellHeight) * j);
                }
                cellX.Add(cellY);
            }
        }

        private void setupForm() {

        }

        private void cellClickHander(object sender, MouseEventArgs e) {
            Cell cell = (Cell)sender;

            if(cell.alive) {
                cell.alive = false;
            } else {
                cell.alive = true;
            }
        }

    }
}
