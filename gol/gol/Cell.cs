using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace gameOfLife {

    class Cell : PictureBox {

        private int cellX, cellY;
        private bool state;

        public Cell(int x, int y, int xPos, int yPos, int width, int height) {
            this.SetBounds(xPos, yPos, width, height);
            cellX = x;
            cellY = y;
            this.BackColor = Color.Aquamarine;
        }

        public bool alive {
            get { return state; }
            set {
                state = value;
                if (state) {
                    this.BackColor = Color.Blue;
                } else {
                    this.BackColor = Color.Aquamarine;
                }
            }
        }

        public int row {
            get { return cellX; }
            set { cellX = value; }
        }

        public int col {
            get { return cellY; }
            set { cellY = value; }
        }

    }
}
