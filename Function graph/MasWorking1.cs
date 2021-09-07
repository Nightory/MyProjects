using System;
using System.Windows.Forms;

namespace Graphics2
{
    public partial class Form1 : Form
    {
        //Запись данных в структуру
        public void WriteMas()
        {
            minX = 9999;
            maxX = -9999;
            minY = 9999;
            maxY = -9999;
            size = dataGridView1.Rows.Count;
            for (int i = 0; i < size - 1; i++)
            {
                if (Convert.ToDouble(dataGridView1[0, i].Value) < minX && Convert.ToDouble(dataGridView1[0, i].Value)!=0) minX = Convert.ToDouble(dataGridView1[0, i].Value);
                if (Convert.ToDouble(dataGridView1[0, i].Value) > maxX && Convert.ToDouble(dataGridView1[0, i].Value) != 0) maxX = Convert.ToDouble(dataGridView1[0, i].Value);
                if (Convert.ToDouble(dataGridView1[1, i].Value) < minY && Convert.ToDouble(dataGridView1[1, i].Value) != 0) minY = Convert.ToDouble(dataGridView1[0, i].Value);
                if (Convert.ToDouble(dataGridView1[1, i].Value) > maxY && Convert.ToDouble(dataGridView1[1, i].Value) != 0) maxY = Convert.ToDouble(dataGridView1[0, i].Value);
                if (dataGridView1[0, 1].Value != null || dataGridView1[1, i].Value != null)
                {
                    points.masX[i] = Convert.ToDouble(dataGridView1[0, i].Value);
                    points.masY[i] = Convert.ToDouble(dataGridView1[1, i].Value);
                    points.size = this.size;

                }
            }
        }

    }
}
