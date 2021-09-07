using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Graphics2
{
    public partial class Form1 : Form
    {
        double minX, maxX, minY,maxY;
        Point points = new Point();
        int size;
        private bool isLeftButtonPressed = false;
        private System.Drawing.Point mouseDown = System.Drawing.Point.Empty;
        double oldXValue;
        double oldYValue;
        public Form1()
        {
            InitializeComponent();
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            chart1.MouseWheel += Chart1_MouseWheel;
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 30;
            chart1.Series[1].ToolTip = "X = #VALX, Y = #VALY";
            points.masX = new double[100];
            points.masY = new double[100];
        }
        
        //Проверка на коректность данных в ячейках таблицы
        private void dataGridView1_CellValidated_1(object sender, DataGridViewCellEventArgs e)
        {
            float dummy;
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
            {
                if (!float.TryParse(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
                {
                    MessageBox.Show("Неверный формат данных!");
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = null;
                    return;
                }
            }
            for (int i = 0; i < e.RowIndex; i++)
            {
                if (Convert.ToDouble(dataGridView1[0, e.RowIndex].Value) == Convert.ToDouble(dataGridView1[0, i].Value) && e.ColumnIndex == 0)
                {
                    MessageBox.Show("Не может быть 2 одинаковые точки X");
                    dataGridView1[0, e.RowIndex].Value = null;
                    return;
                }
            }
        }
        //Проверка на коректность данных в текстовых полях
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text != "")
            {
                float dummy;
                e.Cancel = !float.TryParse(textBox1.Text, out dummy);
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.Text != "")
            {
                float dummy;
                e.Cancel = !float.TryParse(textBox2.Text, out dummy);
            }
        }

        //Отображение координат относительно курсора мышки
        private void chart1_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.PlottingArea:
                case ChartElementType.Gridlines:
                case ChartElementType.DataPoint:
                    var x = e.HitTestResult.ChartArea.AxisX.PixelPositionToValue(e.X);
                    var y = e.HitTestResult.ChartArea.AxisY.PixelPositionToValue(e.Y);
                    e.Text = string.Format("X:{0:f1}; Y:{1:f1}", x, y);
                    break;
            }
        }
        //Перемещение точек на графике при помощи зажатой мышки
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftButtonPressed)
            {
                var result = chart1.HitTest(e.X, e.Y);
                if (result.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.PlottingArea)
                {
                    
                    var newXValue = result.ChartArea.AxisX.PixelPositionToValue(e.X);
                    var newYValue = result.ChartArea.AxisY.PixelPositionToValue(e.Y);
                    for (int i = 0; i < size; i++) {
                        if (Math.Abs(oldXValue-points.masX[i])<=0.7 && Math.Abs(oldYValue -points.masY[i])<=0.7)
                        {
                            dataGridView1[0, i].Value = newXValue;
                            dataGridView1[1, i].Value = newYValue;
                            button1.PerformClick();
                        }
                    }
                }
            }
        }
        //Проверка нажатия мышки
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isLeftButtonPressed = true;
                mouseDown = e.Location;
                var result = chart1.HitTest(e.X, e.Y);
                oldXValue = result.ChartArea.AxisX.PixelPositionToValue(mouseDown.X);
                oldYValue = result.ChartArea.AxisY.PixelPositionToValue(mouseDown.Y);

            }
        }
        //Проверка отжатия мышки
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isLeftButtonPressed = false;
            }
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (textBox5.Text != "")
            {
                float dummy;
                e.Cancel = !float.TryParse(textBox5.Text, out dummy);
            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (textBox4.Text != "")
            {
                float dummy;
                e.Cancel = !float.TryParse(textBox4.Text, out dummy);
            }
        }
        //Обработка кнопки построения графика
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            WriteMas();
            for (int i = 0; i < size - 1; i++)
            {
                this.chart1.Series[0].Points.AddXY(points.masX[i], points.masY[i]);
                this.chart1.Series[1].Points.AddXY(points.masX[i], points.masY[i]);
            }
            changesGraph();
        }
    }
}
