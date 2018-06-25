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
    /// Interaction logic for Operate.xaml
    /// </summary>
    public partial class Operate : Window
    {

        public static string theStringForOperate = "";
        public static List<stream> theStreams = new List<stream>();
        public static List<MainWindow> thePoints = new List<MainWindow>();

        public Operate()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

           
            if (thePoints.Count > 0)
            {
                thePoints[1].Show();
                thePoints[2].Show();
                thePoints[3].Show();
            }

            buttonForDemo.IsEnabled = false;
            buttonForRun.IsEnabled = true;
        }


        private void button_Click_1(object sender, RoutedEventArgs e)
        {

            setStreams();
            startPoints();

            buttonForRun.IsEnabled = false;
            reSetButton.IsEnabled = true;

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            destroyPoints();
        }


        private void reSetButton_Click(object sender, RoutedEventArgs e)
        {
            theStringForOperate = "";
            destroyPoints();
            theStreams = new List<stream>();
            thePoints = new List<MainWindow>();

            Operate A = new EDF.Operate();
            A.Show();
            this.Close();
        }


        private void CPUCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeCPUCount();
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            setSchedulingMode(0);
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            setSchedulingMode(1);
        }

        private void radioButton5_Checked(object sender, RoutedEventArgs e)
        {
            setSchedulingMode(2);
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            setCanRob(false);
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            setCanRob(true);
        }

        //----------------------------------------------------------------------------
        private void makePoints(int pointCount = 10)
        {
            thePoints = new List<MainWindow>();
            for (int i = 0; i < pointCount; i++)
            {
                //创建节点
                MainWindow theM = new MainWindow();
                theM.Title = "No." + (i + 1);
                thePoints.Add(theM);
            }
        }

        private void startPoints()
        {
            for (int i = 0; i < thePoints.Count; i++)
            {
                thePoints[i].makeStart();
            }
        }


        private void destroyPoints()
        {
            for (int i = 0; i < thePoints.Count; i++)
            {
                thePoints[i].Close();
            }
        }

        private void setSchedulingMode(int mode = 0)
        {
            for (int i = 0; i < thePoints.Count; i++)
            {
                thePoints[i].setSchedulingMode(mode);
            }
        }
        private void setCanRob(bool can = true)
        {
            for (int i = 0; i < thePoints.Count; i++)
            {
                thePoints[i].setCanRob(can);
            }
        }

        private void  changeCPUCount()
        {
            Console.WriteLine("-->"+( CPUCount.SelectedIndex + 1 )+" CPUS");
            for (int i = 0; i < thePoints.Count; i++)
                thePoints[i].ChangeCPUCount(CPUCount.SelectedIndex + 1);

            if (radioButton1.IsChecked == true)
                setSchedulingMode(0);
            if (radioButton2.IsChecked == true)
                setSchedulingMode(1);
            if (radioButton5.IsChecked == true)
                setSchedulingMode(2);

            if (radioButton3.IsChecked == true)
                setCanRob(false);
            if (radioButton4.IsChecked == true)
                setCanRob(true);
        }


        //在这个方法构建图
        //thePoints[0]代表第一个节点
        private void setStreams()
        {

            stream Stream1 = new EDF.stream(20, 25, "数据流1");
            Stream1.thePlanRoute.Add(thePoints[0]);
            Stream1.thePlanRoute.Add(thePoints[2]);
            Stream1.thePlanRoute.Add(thePoints[6]);
            Stream1.thePlanRoute.Add(thePoints[9]);
            thePoints[0].getStream(Stream1);

            stream Stream2 = new EDF.stream(15, 30, "数据流2");
            Stream2.thePlanRoute.Add(thePoints[0]);
            Stream2.thePlanRoute.Add(thePoints[3]);
            Stream2.thePlanRoute.Add(thePoints[6]);
            Stream2.thePlanRoute.Add(thePoints[9]);
            thePoints[0].getStream(Stream2);

            stream Stream3 = new EDF.stream(20, 31, "数据流3");
            Stream3.thePlanRoute.Add(thePoints[0]);
            Stream3.thePlanRoute.Add(thePoints[2]);
            Stream3.thePlanRoute.Add(thePoints[5]);
            Stream3.thePlanRoute.Add(thePoints[8]);
            Stream3.thePlanRoute.Add(thePoints[9]);
            thePoints[0].getStream(Stream3);

            stream Stream4 = new EDF.stream(17, 33, "数据流4");
            Stream4.thePlanRoute.Add(thePoints[0]);
            Stream4.thePlanRoute.Add(thePoints[1]);
            Stream4.thePlanRoute.Add(thePoints[5]);
            Stream4.thePlanRoute.Add(thePoints[8]);
            Stream4.thePlanRoute.Add(thePoints[9]);
            thePoints[0].getStream(Stream4);

            stream Stream5 = new EDF.stream(18, 33, "数据流5");
            Stream5.thePlanRoute.Add(thePoints[0]);
            Stream5.thePlanRoute.Add(thePoints[1]);
            Stream5.thePlanRoute.Add(thePoints[5]);
            Stream5.thePlanRoute.Add(thePoints[8]);
            Stream5.thePlanRoute.Add(thePoints[9]);
            thePoints[0].getStream(Stream5);


            theStreams.Add(Stream1);
            theStreams.Add(Stream2);
            theStreams.Add(Stream3);
            theStreams.Add(Stream4);
            theStreams.Add(Stream5);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            makePoints(10);//传入10就是说有10个节点，请注意数组下标不可以越界的 10对应0——9
            changeCPUCount();
        }
    }
}
