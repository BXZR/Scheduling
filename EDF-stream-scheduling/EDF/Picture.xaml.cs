using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EDF
{
    /// <summary>
    /// Picture.xaml 的交互逻辑
    /// </summary>
    public partial class Picture : Window
    {
        public System.Windows.Threading.DispatcherTimer tm = new System.Windows.Threading.DispatcherTimer();
        public string thePictureString="";
        private  string theTaskString = "";//任务的所有的信息显示
        public MainWindow theMainWindow;

        public void makeControllerInformation(controller theController)
        {
            for (int i = 0; i < theController.chargesBook.Count; i++)
            {
                theTaskString += theController.chargesBook[i].geFullInformationWithOutReturn() + "\n";
            }
            theTaskString += "\n";
            switch (theController.schedulingMode)
            {
                case 0: { theTaskString += "调度算法： EDF"; } break;
                case 1: { theTaskString += "调度算法： LLF"; } break;
                case 2: { theTaskString += "调度算法： RM"; } break;
            }
            if (theController.isCanRob)
                theTaskString += "   （可以抢占）";
            else
                theTaskString += "   （不可抢占）";
            this.taskInformation.Content = theTaskString;
        }

        public Picture()
        {
            InitializeComponent();
            
            tm.Tick += new EventHandler(PIC);
            tm.Interval = TimeSpan.FromSeconds(0.5);
            tm.Start();
        }


        void PIC(object sender, EventArgs e)
        {
          TXT.Text = thePictureString;
          //TXT.Select( TXT.Text.Length , 0);
         // TXT.AppendText("");
          //TXT.ScrollToEnd();
          TextForInformation.Text = theMainWindow.textForInformation;
         // Keyboard.Focus(TextForInformation);
          TextForInformation.Select(TextForInformation.Text.Length, 0);
        }
 

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(thePictureString);
        }
    }
}
