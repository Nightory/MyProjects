using System;
using System.Drawing;
using System.Windows.Forms;

namespace Graphics2
{
    public partial class Form1: Form
    {
        //Увеличение графика при наведении на него при помощи колесика мышки
        private void Chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                chart1.Width += 1;
                chart1.Height += 1;
            }
            else
            {
                chart1.Width -= 1;
                chart1.Height -= 1;
            }
        }
        //Изменение границ графика и цвета
        public void changesGraph()
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox1.Text);
                chart1.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox2.Text);
            }
            else
            {
                chart1.ChartAreas[0].AxisX.Minimum = minX-5;
                chart1.ChartAreas[0].AxisX.Maximum = maxX+5;
            }
            if (textBox4.Text != "" && textBox5.Text != "")
            {
                chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text);
                chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox4.Text);
            }
            else
            {
                chart1.ChartAreas[0].AxisY.Minimum = minY-5;
                chart1.ChartAreas[0].AxisY.Maximum = maxY+5;
            }
            if (comboBox1.SelectedItem == null) return;
            if (comboBox1.SelectedItem.ToString() == "Синий") chart1.Series[0].Color = Color.Blue;
            else if (comboBox1.SelectedItem.ToString() == "Красный") chart1.Series[0].Color = Color.Red;
            else if (comboBox1.SelectedItem.ToString() == "Зеленый") chart1.Series[0].Color = Color.Green;
            else chart1.Series[0].Color = Color.Blue;
        }
    }
}
