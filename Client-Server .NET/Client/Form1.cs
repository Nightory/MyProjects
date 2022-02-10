using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LR3
{
    public partial class Form1 : Form
    {
        const int PORT = 8005;
        string LOCALHOST;
        TcpClient client;
        public Form1()
        {
            InitializeComponent();
            Connect.Enabled = true;
            Send.Enabled = false;
            Message.ReadOnly = true;
            chat.ReadOnly = true;
            Exit.Enabled = false;
        }

        private void Registration_Click(object sender, EventArgs e)
        {
            if(IP.Text!="" && User.Text!="" && Password.Text!="")
            {
                LOCALHOST = IP.Text;
                client = new TcpClient();
                client.Connect(LOCALHOST, PORT);
                string message = "43792 " + User.Text + " " + Password.Text.GetHashCode();
                Task receive = new Task(Receive);
                receive.Start();
                byte[] data = new byte[256];
                NetworkStream stream = client.GetStream();
                try
                {
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Message.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Заполните поля IP, UserName и Password");
        }
        private void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                if (IP.Text != "" && User.Text != "" && Password.Text != "")
                {
                    User.ReadOnly = true;
                    LOCALHOST = IP.Text;
                    IP.ReadOnly = true;
                    Exit.Enabled = true;
                    Registration.Enabled = false;
                    Password.ReadOnly = true;
                    Connect.Enabled = false;
                    client = new TcpClient();
                    client.Connect(LOCALHOST, PORT);
                    Task receive = new Task(Receive);
                    Send.Enabled = true;
                    Message.ReadOnly = false;
                    receive.Start();
                    string message = "63582 " + User.Text + " " + Password.Text.GetHashCode();
                    byte[] data = new byte[256];
                    NetworkStream stream = client.GetStream();
                    try
                    {
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                        Message.Clear();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else MessageBox.Show("Заполните поля IP и UserName");
            }
            catch (SocketException ex)
            {
                User.ReadOnly = false;
                IP.ReadOnly = false;
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Send_Click(object sender, EventArgs e)
        {
            string message = String.Format("{0}: {1}", User.Text, Message.Text);
            byte [] data = new byte[256];
            NetworkStream stream = client.GetStream();
            try
            {
                data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Message.Clear();                
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message);
                stream.Close();
                client.Close();
                Close();
            }
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[256];
                    NetworkStream stream = client.GetStream();
                    stream.Read(data, 0, data.Length);
                    string message = Encoding.Unicode.GetString(data);
                    string time = DateTime.Now.ToShortTimeString();
                    string[] words = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    this.Invoke(new MethodInvoker(() =>
                    {
                        if (words[0]== "97564")
                        {
                            message = "";
                            Send.Enabled = false;
                            Message.ReadOnly = true;
                            Connect.Enabled = true;
                            Exit.Enabled = false;
                            Registration.Enabled = true;
                            User.ReadOnly = false;
                            Password.ReadOnly = false;
                            IP.ReadOnly = false;
                            for (int i = 1; i < words.Length; i++) message = message + " " + words[i];
                            chat.ReadOnly = false;
                            chat.Text = chat.Text + time + " " + message + "\r\n";
                            chat.ReadOnly = true;
                        }
                        else
                        {
                            chat.ReadOnly = false;
                            chat.Text = chat.Text + "\r\n" + time + " " + message;
                            chat.ReadOnly = true;
                        }
                    }));
                }
            }
            catch (Exception ex) 
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    User.ReadOnly = false;
                    IP.ReadOnly = false;
                }));
                MessageBox.Show(ex.Message);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            string message = "85345 Exit";
            byte[] data = new byte[256];
            NetworkStream stream = client.GetStream();
            try
            {
                data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Message.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                stream.Close();
                client.Close();
            }
            client.Close();
            stream.Close();
            Close();
        }
    }
}
