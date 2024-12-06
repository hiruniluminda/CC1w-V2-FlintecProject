using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using Microsoft.Data.SqlClient;



namespace CC1w_V2_wifiboardTest
{
   


    public partial class Form1 : Form
    {
        private static SqlConnection GetDatabaseConnection()
        {
            string connectionString = "Server=HIRUSHAN\\SQLEXPRESS;Database=CCIWDatabase;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
            return new SqlConnection(connectionString);
        }

        // Declare SerialPort objects for Arduino and Multimeter
        SerialPort serialPort = new SerialPort("COM11", 9600);  // Adjust COM port as needed
        SerialPort multimeterPort = new SerialPort("COM3", 9600);  // Adjust COM port as needed

        public Form1()
        {
            InitializeComponent();
        }

        // Open serial ports once and keep them open
        private void OpenSerialPorts()
        {
            if (!serialPort.IsOpen)
                serialPort.Open();

            if (!multimeterPort.IsOpen)
                multimeterPort.Open();
        }

        // Close serial ports
        private void CloseSerialPorts()
        {
            if (serialPort.IsOpen)
                serialPort.Close();

            if (multimeterPort.IsOpen)
                multimeterPort.Close();
        }

        // Timer1 Tick: Send command to Arduino and read response
        private async void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("A");

                // Wait for response asynchronously
                await Task.Delay(50);  // Short delay for Arduino to process
                string arduinoResponse = serialPort.ReadLine();

                // Display Arduino response in TextBox
                textBox1.Invoke(new Action(() => textBox1.Text = arduinoResponse));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

                timer1.Stop();  // Stop the timer after the task completes
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        // Button1 Click: Start Timer1 to send command to Arduino
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        // Button2 Click: Send command "Z" to Arduino and read response
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                label1.Text = "Reading 1: ";
                label3.Text = "Reading 3: ";
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("Z");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadLine();

                // Display Arduino response in TextBox
                textBox2.Invoke(new Action(() => textBox2.Text = response));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        // Button3 Click: Send command "B" to Arduino and read response
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("B");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadLine();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        // Timer2 Tick: Fetch data from Multimeter and display reading
        private async void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Clear input and output buffers
                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                }

                timer2.Stop();  // Stop the timer after fetching data
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Multimeter: {ex.Message}");
            }
        }


        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("C");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("D");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("L");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button14_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("M");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button15_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("N");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button16_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("O");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button17_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("P");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button6_Click_2(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("E");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("F");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button8_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("G");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button9_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("H");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button10_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("I");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button11_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("J");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button12_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts();

                // Send command to Arduino
                serialPort.Write("K");

                // Wait for response asynchronously
                await Task.Delay(50);
                string response = serialPort.ReadExisting();

                // Display Arduino response in TextBox
                textBox3.Invoke(new Action(() => textBox3.Text = response));

                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Send request to Multimeter
                multimeterPort.Write(":FETC?" + ((char)13));

                // Wait for data asynchronously
                // await Task.Delay(100);

                string reading = multimeterPort.ReadLine();

                // If there is a valid reading, process it
                if (!string.IsNullOrEmpty(reading) && reading != "\n")
                {
                    string[] readParts = reading.Split('\n');
                    double readingValue = Convert.ToDouble(readParts[0]);

                    // Display the reading in labels
                    label1.Invoke(new Action(() => label1.Text = $"Reading 1: {readingValue}"));
                    label3.Invoke(new Action(() => label3.Text = $"Reading 3: {readingValue}"));

                    if (readingValue > 10000)
                    {
                        label2.Invoke(new Action(() => label2.Text = "Pass"));
                    }
                    else
                    {
                        label2.Invoke(new Action(() => label2.Text = "Fail"));

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private async void button18_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts(); // Open and configure ports

                // Clear buffers before sending commands
                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();


                multimeterPort.WriteLine(":FUNC RES" + ((char)13));
                await Task.Delay(500);

                MessageBox.Show("Multimeter set to Ohm Mode.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Multimeter: {ex.Message}");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button19_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts(); // Open and configure ports

                // Clear buffers before sending commands
                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();


                multimeterPort.WriteLine(":FUNC VOLT:DC" + ((char)13));
                await Task.Delay(500);

                MessageBox.Show("Multimeter set to DC Mode.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Multimeter: {ex.Message}");
            }
        }

        private async void button20_Click(object sender, EventArgs e)
        {
            try
            {
                OpenSerialPorts(); // Open and configure ports

                // Clear buffers before sending commands
                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();


                multimeterPort.WriteLine(":FUNC CURR:DC" + ((char)13));
                await Task.Delay(500);

                MessageBox.Show("Multimeter set to AMP Mode.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Multimeter: {ex.Message}");
            }
        }

        private async Task ChangeToAmp(object sender, EventArgs e)
        {
            try
            {
                // Clear buffers before sending commands
                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Change multimeter to AMP mode
                multimeterPort.WriteLine(":FUNC CURR:DC" + ((char)13));
                await Task.Delay(500);

                MessageBox.Show("Multimeter set to AMP Mode.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting multimeter to AMP mode: {ex.Message}");
            }
        }

        private async Task ChangeToOhm(object sender, EventArgs e)
        {
            try
            {
                // Clear buffers before sending commands
                multimeterPort.DiscardInBuffer();
                multimeterPort.DiscardOutBuffer();

                // Change multimeter to AMP mode
                multimeterPort.WriteLine(":FUNC RES" + ((char)13));
                await Task.Delay(500);

                MessageBox.Show("Multimeter set to Ohm Mode.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting multimeter to Ohm mode: {ex.Message}");
            }
        }




        private async void button21_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "Reading 1: ";
                label3.Text = "Reading 3: ";
                listBox1.Items.Clear(); // Clear the list before starting
                OpenSerialPorts();
                await ChangeToOhm(sender, e);

                string serialNumber = GenerateSerialNumber(); // Generate a unique serial number

                for (char command = 'A'; command <= 'P'; command++) // Loop through 'A' to 'P'
                {
                    if (command.ToString() == "P")
                    {
                        SerialPort serialPort = new SerialPort("COM11", 9600);  // Adjust COM port as needed
                        SerialPort multimeterPort = new SerialPort("COM3", 9600);  // Adjust COM port as needed

                        await ChangeToAmp(sender, e);
                    }

                    string test = GetTestName(command); // Fetch the test name

                    OpenSerialPorts();

                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();

                    serialPort.Write(command.ToString()); // Send command to Arduino
                    await Task.Delay(50); // Allow Arduino to process the command

                    string response = serialPort.ReadLine(); // Read response from Arduino

                    // Display Arduino response in TextBox and ListBox
                    textBox3.Invoke(new Action(() => textBox3.Text = response));

                    multimeterPort.DiscardInBuffer();
                    multimeterPort.DiscardOutBuffer();

                    // Send request to Multimeter
                    multimeterPort.Write(":FETC?" + ((char)13));

                    string reading = multimeterPort.ReadLine();

                    if (!string.IsNullOrEmpty(reading) && reading != "\n")
                    {
                        string[] readParts = reading.Split('\n');
                        double readingValue = Convert.ToDouble(readParts[0]);

                        string result = string.Empty;

                        if (readingValue > 10000)
                        {
                            result = "Pass";
                        }
                        else
                        {
                            result = "Fail";
                        }

                        // Display the reading in labels and ListBox
                        listBox1.Invoke(new Action(() => listBox1.Items.Add($"{test} : {readingValue} || {result}")));

                        await SaveToDatabase(command.ToString(), test, readingValue, result, serialNumber);

                    }

                    OpenSerialPorts();

                    // Send command to Arduino
                    serialPort.Write("Z");

                    await Task.Delay(50); // Wait for response asynchronously
                    string response2 = serialPort.ReadLine();
                }

                MessageBox.Show("All commands sent and responses received.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error communicating with Arduino: {ex.Message}");
            }
        }

        private static int lastSerialNumber = 0;

        private string GenerateSerialNumber()
        {
            lastSerialNumber++;
            return lastSerialNumber.ToString();
        }


        private string GetTestName(char command)
        {
            return command switch
            {
                'A' => "jumper 2, PIN 1 & PIN 6 (SUPPLY AND GND)",
                'B' => "jumper 2, PIN 3 & PIN 6 (POS AND GND)",
                'C' => "jumper 2, PIN 4 & PIN 6 (RST AND GND)",
                'D' => "jumper 3, PIN 2 & PIN 1 (HOSTD+ AND GND)",
                'E' => "jumper 3, PIN 3 & PIN 1 (HOSTD- AND GND)",
                'F' => "jumper 3, PIN 4 & PIN 1 (HOSTDET AND GND)",
                'G' => "jumper 3, PIN 5 & PIN 1 (STROKETOP AND GND)",
                'H' => "jumper 3, PIN 6 & PIN 1 (STROKEBOT AND GND)",
                'I' => "jumper 1, PIN 1 & PIN 8 (SUPPLY AND GND)",
                'J' => "jumper 1, PIN 2 & PIN 8 (EM_PIN8 AND GND)",
                'K' => "jumper 1, PIN 3 & PIN 8 (GPIOO AND GND)",
                'L' => "jumper 1, PIN 4 & PIN 8 (TXD_RF AND GND)",
                'M' => "jumper 1, PIN 5 & PIN 8 (RXD_RF AND GND)",
                'N' => "jumper 1, PIN 6 & PIN 8 (UART2RX AND GND)",
                'O' => "jumper 1, PIN 7 & PIN 8 (UART2TX AND GND)",
                'P' => "jumper 1, PIN 6 & PIN 8 (SUPPLY AND GND)",
                _ => "Unknown Test"
            };
        }

        private async Task SaveToDatabase(string command, string testName, double readingValue, string result, string serialNumber)
        {
            using (var connection = GetDatabaseConnection())
            {
                await connection.OpenAsync();
                string query = "INSERT INTO TestResults (Command, TestName, ReadingValue, Result, SerialNumber) VALUES (@Command, @TestName, @ReadingValue, @Result, @SerialNumber)";
                using (var commandObj = new SqlCommand(query, connection))
                {
                    commandObj.Parameters.AddWithValue("@Command", command);
                    commandObj.Parameters.AddWithValue("@TestName", testName);
                    commandObj.Parameters.AddWithValue("@ReadingValue", readingValue);
                    commandObj.Parameters.AddWithValue("@Result", result);
                    commandObj.Parameters.AddWithValue("@SerialNumber", serialNumber);

                    await commandObj.ExecuteNonQueryAsync();
                }
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
