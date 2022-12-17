using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace MemoryEditor
{
    public partial class MemoryEditorGUI : Form
    {
        static void Main()
{
    Process[] processes = Process.GetProcessesByName("Minecraft.Windows");
    if (processes.Length > 0)
    {
        Process MinecraftProcess = processes[0];
        Application.Run(new MemoryEditorGUI(MinecraftProcess));
    }
    else
    {
        MessageBox.Show("Minecraft is not running.");
    }
}
        private Process MinecraftProcess;

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);
        

        public MemoryEditorGUI(Process process)
        {
            InitializeComponent();
            MinecraftProcess = process;
        }
        [DllImport("kernel32.dll")]
private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, IntPtr dwSize, out IntPtr lpNumberOfBytesRead);
        

private void btnReadMemory_Click(object sender, EventArgs e)
{
    // get the address and size of the memory to be read
    int addressInt;
    if (!Int32.TryParse(txtAddress.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out addressInt))
    {
        // txtAddress does not contain a valid hexadecimal string
        // You can show an error message or do something else here
        return;
    }

    IntPtr address = new IntPtr(addressInt);
    int size = Convert.ToInt32(txtSize.Text);

    // read the memory from the process and display it in the text box
    byte[] buffer = new byte[size];
    IntPtr bytesRead;
    if (ReadProcessMemory(MinecraftProcess.Handle, address, buffer, size, out bytesRead))
    {
        // Display the memory addresses and values in the GUI
        for (int i = 0; i < size; i++)
        {
            txtMemory.AppendText(string.Format("{0:X8}: {1}\r\n", address + i, buffer[i]));
        }
    }
}

        private void btnWriteMemory_Click(object sender, EventArgs e)
        {
            IntPtr address = new IntPtr(Convert.ToInt64(txtAddress.Text));
            if (!Int64.TryParse(txtAddress.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long addressInt))
{
    // txtAddress does not contain a valid hexadecimal string
    // You can show an error message or do something else here
    return;
}

address = new IntPtr(addressInt);
            int size = Convert.ToInt32(txtSize.Text);
            byte[] buffer = new byte[size];
            // Convert the hex string in the text box to bytes
            for (int i = 0; i < txtMemory.Text.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(txtMemory.Text.Substring(i, 2), 16);
            }

            // Write the bytes to the Minecraft process
            IntPtr bytesWritten;
            WriteProcessMemory(MinecraftProcess.Handle, address, buffer, size, out bytesWritten);
        }

        // form controls
private void InitializeComponent()
        {
            // create the controls
            this.txtAddress = new TextBox();
            this.txtSize = new TextBox();
            this.txtMemory = new TextBox();
            this.btnReadMemory = new Button();
            this.btnWriteMemory = new Button();

            // set the properties of the controls
            this.txtAddress.Location = new Point(12, 12);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new Size(100, 22);

            this.txtSize.Location = new Point(118, 12);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new Size(100, 22);

            this.txtMemory.Location = new Point(12, 40);
            this.txtMemory.Name = "txtMemory";
            this.txtMemory.Size = new Size(206, 22);
            this.txtMemory.Multiline = true;
            this.txtMemory.ScrollBars = ScrollBars.Vertical;

            this.btnReadMemory.Location = new Point(224, 12);
            this.btnReadMemory.Name = "btnReadMemory";
            this.btnReadMemory.Size = new Size(75, 23);
            this.btnReadMemory.Text = "Read Memory";
            this.btnReadMemory.Click += new EventHandler(this.btnReadMemory_Click);

            this.btnWriteMemory.Location = new Point(224, 41);
            this.btnWriteMemory.Name = "btnWriteMemory";
            this.btnWriteMemory.Size = new Size(75, 23);
            this.btnWriteMemory.Text = "Write Memory";
            this.btnWriteMemory.Click += new EventHandler(this.btnWriteMemory_Click);

            // add the controls to the form
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.txtMemory);
            this.Controls.Add(this.btnReadMemory);
            this.Controls.Add(this.btnWriteMemory);
        }

    // form controls
    public TextBox txtAddress;
    public TextBox txtSize;
    public TextBox txtMemory;
    public Button btnReadMemory;
    public Button btnWriteMemory;

    // event handlers for the controls
    // ...
}
        }