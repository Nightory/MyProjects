using System;
using System.Windows.Forms;

namespace Graphics2
{
    public partial class Form1 : Form
    {
        //Обработка нажатия на справку
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1.Редактирование графика не обязательные поля, если их не заполнять будут проставлены стандартные значения\n" +
                "2. В поля можно вводить только числа или числа с плавающей точкой, в противном случае вы не сможете переместиться на другое поле\n" +
                "3. При наведении на график, его можно увеличить колесиком мыши\n" +
                "4. Надо задавать обе границы иначе ничего не произойдет, для возвращения обычных настроек графика очистите поля\n" +
                "5. Точки можно двигать при помощи зажатой левой кнопки мыши, если быстро двигать то точка не будет двигаться\n\n" +
                "Работа выполнена студентом группы 91ПГ Волковым Вадимом");
        }

        //Обработка нажатия на сохранить
        private void сохранитьТаблицуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "") points.xleft = Convert.ToDouble(textBox1.Text);
            if (textBox2.Text != "") points.xright = Convert.ToDouble(textBox2.Text);
            if (textBox5.Text != "") points.ydown = Convert.ToDouble(textBox5.Text);
            if (textBox4.Text != "") points.yup = Convert.ToDouble(textBox4.Text);
            saveFileDialog1.DefaultExt = "Volkov";
            saveFileDialog1.Filter = "Volkov files(.Volkov)|.Volkov";
            saveFileDialog1.ShowDialog();
            string path = saveFileDialog1.FileName;
            if (path != "") SaveData(path);
            else MessageBox.Show("Ошибка неверно указан путь");
        }
        //обработка нажатия на печать
        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Printing.PrintPreview();
        }
        //Обработка нажатия на открыть
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            LoadHandleSave(path);
            for(int i = 0; i < points.size-1; i++)
            {
                if (i == dataGridView1.RowCount - 1)
                {
                    dataGridView1.Rows.Add();
                }
                dataGridView1[0, i].Value = points.masX[i];
                dataGridView1[1, i].Value = points.masY[i];
            }
            if(points.xleft.ToString()!="0")textBox1.Text = points.xleft.ToString();
            if (points.xright.ToString() != "0") textBox2.Text = points.xright.ToString();
            if (points.ydown.ToString() != "0") textBox5.Text = points.ydown.ToString();
            if (points.yup.ToString() != "0") textBox4.Text = points.yup.ToString();
            button1.PerformClick();
        }
    }
}
