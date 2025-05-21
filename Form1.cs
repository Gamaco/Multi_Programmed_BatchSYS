using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projecto_2_19_2025
{

    public partial class GUI_OS : Form
    {
        OS os;
        PCB pcbGUI;
        Boolean autoTrigger;

        private System.Windows.Forms.NumericUpDown numericUpDown_BlockTime;
        private System.Windows.Forms.Label labelBlockTime;

        public GUI_OS()
        {
            os = new OS();
            autoTrigger = false;
            InitializeComponent();
           
        }

       
       

        private void UpdateREADY()
        {

            lstReady.DataSource = null;
            lstReady.DataSource = os.getlistReady();
            lblReadyCount.Text = String.Format("Count: {0}", os.getlistReady().Count.ToString()); // Actualiza el contador de procesos en READY
        }

        private void updateNewProcesses()
        {
            RAM.DataSource = null;
            RAM.DataSource = os.getnewProcesses();
            lblNewCount.Text = String.Format("Count: {0}", os.getnewProcesses().Count.ToString()); // Actualiza el contador de procesos nuevos


        }

        private void UpdateFinished()
        {
            List_Finish.DataSource = null;
            List_Finish.DataSource = os.getfinished();
            lblFinishedCount.Text = String.Format("Count: {0}", os.getfinished().Count.ToString()); // Actualiza el contador de procesos terminados
        }

        private void UpdateRunning()
        {
            CPU_List.Items.Clear();
            int runningCount = 0;

            foreach (CPU cpu in os.getCPUs())
            {
                if (cpu.isExecutingAProcess())
                {
                    CPU_List.Items.Add(String.Format("CPU [{0}]: {1}", cpu.getId(), cpu.getExecutingProcess().ToString()));
                    runningCount++;
                }
            }

            int totalCPUs = os.getCPUs().Count;
            float usage = (totalCPUs > 0) ? ((float)runningCount / totalCPUs) * 100 : 0;

            lbl_cpuUsage.Text = $"CPU Usage: {usage:0}%"; // El 0 es para que lo redondee

        }


        private async void btnClock_Click(object sender, EventArgs e)
        {
            // La mitad de un segundo en MS
            int velocidadClick = 300;

            // Chequea si esta en modo automatico
            if (autoTrigger == false && checkBox1.Checked == true)
            {
                autoTrigger = true;
                btnClock.Text = "Stop";

                await Task.Run(() =>
                {
                    while (autoTrigger)
                    {
                        Console.WriteLine("\n-----New Tick-----");
                        os.ClockTick();

                        Invoke(new Action(() =>
                        {
                            UpdateRunning();
                            UpdateFinished();
                            updateNewProcesses();
                            UpdateREADY();
                            UpdateRunning();
                        }));

                        Thread.Sleep(velocidadClick);
                    }
                });

                btnClock.Text = "Clock tick";
            }
            else
            {
                autoTrigger = false;
                btnClock.Text = "Clock tick";
            }

            // Si no esta en modo automatico, hace un solo tick y ya
            if (!checkBox1.Checked)
            {
                Console.WriteLine("\n-----New Tick-----");
                os.ClockTick();
                UpdateRunning();
                UpdateFinished();
                updateNewProcesses();
                UpdateREADY();
                UpdateRunning();
            }
        }


        private void btnPCB_Click(object sender, EventArgs e)
        {
            pcbGUI = new PCB();                  // Crea una nueva ventana PCB
            pcbGUI.CargarProcesos(os);          // Le pasa el sistema operativo actual
            pcbGUI.Show();                      // Muestra la ventana

        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;

            // Cambia la cantidad de CPUs disponibles
            os.SetAvailableCPUs((int)numericUpDown.Value);
        }

        private void numericUpDown_QuantumTime_ValueChanged(object sender, EventArgs e)
        {
            os.updateQuantumTime((int)numericUpDown_QuantumTime.Value); // Cambia el quantum de cada CPU
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (os.getMinProcessesSize() > (int)numericUpDown1.Value)
            {
                MessageBox.Show("El tamaño máximo no puede ser menor que el tamaño mínimo.");
                numericUpDown1.Value = os.getMaxProcessesSize();
            }
            else
            {
                os.updateMaxProcessesSize((int)numericUpDown1.Value); // Cambia el tamaño minimo de los procesos
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (os.getMaxProcessesSize() < (int)numericUpDown2.Value)
            {
                MessageBox.Show("El tamaño mínimo no puede ser mayor que el tamaño máximo.");
                numericUpDown2.Value = os.getMinProcessesSize();
            }
            else
            {
                os.updateMinProcessesSize((int)numericUpDown2.Value); // Cambia el tamaño minimo de los procesos
            }
        }

        private void numericUpDown_BlockTime_ValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}

