using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ascensor
{
    public partial class Form1 : Form
    {
        public enum Piso
        {
            Uno = 332,
            Dos = 178,
            Tres = 31
        }

        const int Speed = 1;
        const int DelayTime = 50;
        const int StopTime = 2000;

        public string QueueProgress = "";

        private readonly List<Piso> LiftQueue;

        private readonly BackgroundWorker LiftEngine;

        public Form1()
        {
            InitializeComponent();
            LiftQueue = new List<Piso>();
            LiftEngine = new BackgroundWorker();
        }

        private void MoveElevator(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (LiftQueue.Count != 0)
                {
                    Thread.Sleep(StopTime);
                    int Floor = (int)LiftQueue.FirstOrDefault();
                    LiftQueue.RemoveAt(0);

                    if (this.pictureBox1.Location.Y < Floor)
                        for (int CurrentHeight = this.pictureBox1.Location.Y; CurrentHeight <= Floor; CurrentHeight += Speed)
                            ChangePosition(CurrentHeight);

                    else if (this.pictureBox1.Location.Y > Floor)
                        for (int CurrentHeight = this.pictureBox1.Location.Y; CurrentHeight >= Floor; CurrentHeight -= Speed)
                            ChangePosition(CurrentHeight);
                    ChangePosition(0);
                    Thread.Sleep(StopTime);
                }
            }

            void ChangePosition(int newHeight)
            {
                Thread.Sleep(DelayTime);
                LiftEngine.ReportProgress(0, newHeight);
            }
        }

        private void UpdateQueueLabel()
        {
            QueueProgress = "";
            foreach (var item in LiftQueue)
            {
                QueueProgress += item;
                if (item != LiftQueue.LastOrDefault())
                {
                    QueueProgress += " - ";
                }
            }
            label1.Text = QueueProgress;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            LiftQueue.Add(Piso.Uno);
            UpdateQueueLabel();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            LiftQueue.Add(Piso.Dos);
            UpdateQueueLabel();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            LiftQueue.Add(Piso.Tres);
            UpdateQueueLabel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LiftEngine.DoWork += MoveElevator;
            LiftEngine.ProgressChanged += ElevatorMoved;
            LiftEngine.WorkerReportsProgress = true;
            LiftEngine.RunWorkerAsync();
        }

        private void ElevatorMoved(object sender, ProgressChangedEventArgs e)
        {
            int state = Convert.ToInt32(e.UserState);
            if (state != 0)
                pictureBox1.Location = new Point(12, state);
            else
                UpdateQueueLabel();
        }

    }
}
