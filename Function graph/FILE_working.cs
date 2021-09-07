using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Graphics2
{
//Функция сохранения в файл
    public partial class Form1 : Form
    {
        public void SaveData(string path)
        {
            WriteMas();
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, points);
            fs.Close();
        }
        //Функция чтения данных из файла
        public void LoadHandleSave(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    points = (Point)formatter.Deserialize(fs);
                    
                }
                catch (System.Exception Error)
                {
                    MessageBox.Show(Error.Message);
                    
                }
                finally
                {
                    fs.Close();
                }
            }
        }
    }
}
