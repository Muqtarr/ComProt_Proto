using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace ComProt_Proto
{
    public partial class Form1 : Form 
    {
        string dataOUT;
        string dataIN;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);

                serialPort1.Open();
                progressBar1.Value = 100;

            }
            catch (Exception err) 
            {
                MessageBox.Show(err.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxComPort.Items.AddRange(ports);

        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                dataOUT = tBoxDataOut.Text;
                serialPort1.Write(dataOUT);
            }

        }
        
        private void ShowData(object sender, EventArgs e)
        {
            tBoxDataOut.Text += dataIN;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
                dataIN = serialPort1.ReadExisting();
                this.Invoke(new EventHandler(ShowData));
               
            
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // Create and show a SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files|*.csv";
            saveFileDialog.Title = "Save Data to CSV File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    using (StreamWriter fileWriter = new StreamWriter(filePath))
                    {
                        // Read data from tBoxDataOut and write it to the CSV file
                        string data = tBoxDataOut.Text;
                        fileWriter.WriteLine(data);
                    }

                    MessageBox.Show("Data saved to " + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            tBoxDataOut.Text = string.Empty; // Clear the text in tBoxDataOut
        }

    }
}
