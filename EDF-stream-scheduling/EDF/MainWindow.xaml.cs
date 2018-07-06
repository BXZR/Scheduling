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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EDF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Windows.Threading.DispatcherTimer tm = new System.Windows.Threading.DispatcherTimer();
        List<controller> theControlers = new List<controller>();
        private bool isAValuableCharge = false;
        private Picture thePictureWindow;
        private float timerScale = 1f;
        private bool isScalling = false;
        int indexForCPUNow = -1;
        public string textForInformation = "";

        public MainWindow()
        {
            InitializeComponent();
            tm.Tick += new EventHandler(tm_Tick);
            tm.Interval = TimeSpan.FromSeconds(0.01 * timerScale);
        }



        public void getStream(stream theStream)
        {
            //int CPUIndex = getCPUIndexCanUse();

            //选择冲突最小的信道来处理
            int CPUIndex = getCPUIndexWithCanculate(theStream);

            for (int i = theControlers[CPUIndex].allTimer; i < theStream.allTimeUse; i++)
            {
                for (int j = 0; j < theControlers.Count; j++)
                    theControlers[j].forTimer();
                flashWindow();
            }
            //
            int timerWait = getWaitTimeForTheStream( theStream);

            //Console.WriteLine(this.Title + "get "+ theStream .theName+ " and send to " + CPUIndex);
            //在这里是一个模拟方案
            //一个新的任务需要完成本身的时间和等待的时间才可以将这个流转移到下一个节点

            charge newChargeForBook = new charge(theStream.timeSlot + timerWait, theStream.circleTime, theStream.deadLine, 1, theControlers[CPUIndex].allTimer, theStream.theName + "（第" + theStream.indexNow + "跳）", theControlers[CPUIndex] , false);
            newChargeForBook.theStreamforCharge = theStream;
            newChargeForBook.theMainWindow = this;

            //theControlers[CPUIndex].chargesBook.Add(newChargeForBook);
            theControlers[CPUIndex].charges.Add(newChargeForBook);
            theControlers[CPUIndex].schedule();
            //ListBoxItem f = new ListBoxItem();
            // f.Content = newChargeForBook.getShortInformation();
            //AllChargeBook.Items.Add(f);

        }


        //获得这个任务的等待时间
        int getWaitTimeForTheStream( stream theStreamUsing)
        {

            //如果是终点就没有必要了
            if (theStreamUsing.indexNow == theStreamUsing.thePlanRoute.Count - 1)
                return 0;


            int timeWait = 0;

            MainWindow thePointGet = theStreamUsing.thePlanRoute[theStreamUsing.indexNow];
            MainWindow thePointSend = theStreamUsing.thePlanRoute[theStreamUsing.indexNow + 1];


            int maxCount = -1000;
            for(int i = 0; i < theControlers.Count; i++)
            {
                int count = theControlers[i].charges.FindAll(X => X.theStreamforCharge!= null).Count;
                maxCount = maxCount < count ? count : maxCount;

             }

            for (int k = 0; k < maxCount ; k++)
            {
                int timeAdd = 0;
                for (int i = 0; i < theControlers.Count; i++)
                {  
                    List<charge> theCharges = theControlers[i].charges.FindAll(X => X.theStreamforCharge != null);

                    if (theCharges.Count >= k)
                        continue;

                    stream theStream = theCharges[k].theStreamforCharge;
                    if (thePointGet == thePointGet || thePointSend == thePointSend)
                    {
                        timeAdd = timeAdd < theStream.timeSlot ? theStream.timeSlot : timeAdd;
                    }
                }

                timeWait += timeAdd;

            }
            return timeWait;
        }

        //选择一条冲突最小
        int getCPUIndexWithCanculate(stream theStreamIn)
        {
            //如果是终点就没有必要了
            if (theStreamIn.indexNow == theStreamIn.thePlanRoute.Count - 1)
            {
                return getCPUIndexCanUse();
            }

            MainWindow thePointGet =  theStreamIn.thePlanRoute[theStreamIn.indexNow];
            MainWindow thePointSend = theStreamIn.thePlanRoute[theStreamIn.indexNow+1];

            int minCrash = 999;
            int minCrashControllerIndex = 0;
            for (int i = 0; i < theControlers.Count; i++)
            {
                int crashCountNow = 0;
                for (int j = 0; j < theControlers[i].charges.Count; j++)
                {
                    stream theStream = theControlers[i].charges[j].theStreamforCharge;
                    if (theStream != null && theStream.indexNow <= theStream.thePlanRoute.Count - 1)
                    {
                        MainWindow theStart = theStream.thePlanRoute[theStream.indexNow];
                        MainWindow theEnd = theStream.thePlanRoute[theStream.indexNow + 1];
                        if (theStart == thePointGet && theEnd == thePointSend)
                        {
                            crashCountNow++;
                        }
                    }
                }
                if (crashCountNow < minCrash)
                {
                    minCrash = crashCountNow;
                    minCrashControllerIndex = i;
                }
            }

            return minCrashControllerIndex;
        }

        int getCPUIndexCanUse()
        {
            indexForCPUNow++;
            if (indexForCPUNow >= theControlers.Count)
                indexForCPUNow = 0;
            return indexForCPUNow;
        }

        void flashWindow()
        {
            showTextForEDF.Text = this.textForInformation;
            //AllCharge.Items.Clear();

            for (int k = 0; k < theControlers.Count; k++)
            {
                for (int i = 0; i < theControlers[k].charges.Count; i++)
                {
                    ListBoxItem f = new ListBoxItem();
                    f.Content = theControlers[k].charges[i].getShortInformation();
                    //AllCharge.Items.Add(f);
                }

                showTextForEDF.Select(showTextForEDF.Text.Length, 0);
                if (thePictureWindow == null)
                {
                    // Keyboard.Focus(showTextForEDF);
                }
                else
                    thePictureWindow.thePictureString = theControlers[k].getShowerText();
            }

        }
        void tm_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < theControlers.Count; i++)
            {
                theControlers[i].forTimer();
            }
            flashWindow();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void makeExample()
        {
            try
            {
                int CPUIndex = getCPUIndexCanUse();
                charge newChargeForBook = new charge(5, 30, 29, 2, 1, "示例进程1", theControlers[CPUIndex], false);
                newChargeForBook.theMainWindow = this;
                theControlers[CPUIndex].chargesBook.Add(newChargeForBook);
                ListBoxItem f = new ListBoxItem();
                f.Content = newChargeForBook.getShortInformation();
                //AllChargeBook.Items.Add(f);

                CPUIndex = getCPUIndexCanUse();
                newChargeForBook = new charge(8, 20, 18, 2, 2, "示例进程2", theControlers[CPUIndex], false);
                newChargeForBook.theMainWindow = this;
                theControlers[CPUIndex].chargesBook.Add(newChargeForBook);
                ListBoxItem f2 = new ListBoxItem();
                f2.Content = newChargeForBook.getShortInformation();
                //AllChargeBook.Items.Add(f2);

                CPUIndex = getCPUIndexCanUse();
                newChargeForBook = new charge(6, 25, 25, 2, 3, "示例进程3", theControlers[CPUIndex], false);
                newChargeForBook.theMainWindow = this;
                theControlers[CPUIndex].chargesBook.Add(newChargeForBook);
                ListBoxItem f3 = new ListBoxItem();
                f3.Content = newChargeForBook.getShortInformation();
                //AllChargeBook.Items.Add(f3);

                for (int i = 0; i < theControlers.Count; i++)
                {
                    theControlers[i].makeChargeShower();
                    if (theControlers[i].schedulingMode == 2)//RM的特殊性
                    {
                        theControlers[i].theSort();//排序
                        if (!this.checkForRM(theControlers[i].chargesBook))
                            MessageBox.Show("RM算法不可调度，有超时现象发生。");
                    }
                }
            }
            catch
            {
                Console.WriteLine("index error");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string chargeNamer = "";
                string nameUse = "示例任务1";
                if (chargeNamer.Length > nameUse.Length)
                {
                    chargeNamer = chargeNamer.Substring(0, nameUse.Length);
                }
                else if (chargeNamer.Length < nameUse.Length)
                {
                    chargeNamer += ("XXXXXXXXXXXXXX".Substring(0, nameUse.Length - chargeNamer.Length));
                }
                Console.Write(chargeNamer);
                int timeLonger = 10;
                int deadLiner = 12;
                int circleTimer = 12;
                int timesr = 2;
                int startTimer = 2;
                ////
                int CPUindex = getCPUIndexCanUse();
                charge newChargeForBook = new charge(timeLonger, circleTimer, deadLiner, timesr, startTimer, chargeNamer, theControlers[CPUindex], isAValuableCharge);
                newChargeForBook.theMainWindow = this;
                theControlers[CPUindex].chargesBook.Add(newChargeForBook);
                ListBoxItem f = new ListBoxItem();
                f.Content = newChargeForBook.getShortInformation();
                //AllChargeBook.Items.Add(f);
                ////

                for (int k = 0; k < theControlers.Count; k++)
                {
                    float checker = 0;
                    for (int i = 0; i < theControlers[k].chargesBook.Count; i++)
                        checker += theControlers[k].chargesBook[i].getCheckValue();
                    if (checker > 1)//检查出错了
                    {
                        MessageBox.Show("checker:" + checker + " 其实，这是不可调度的。\n请点击容错来排除这个问题。");
                        flashWindow();
                    }
                    if (theControlers[k].schedulingMode == 2)//RM的特殊性
                    {
                        theControlers[k].theSort();//排序
                        if (!this.checkForRM(theControlers[k].chargesBook))
                            MessageBox.Show("RM算法不可调度，有超时现象发生。");
                    }

                    theControlers[k].makeChargeShower();
                }
            }
            catch
            {
                MessageBox.Show("除了名字麻烦其他项都输入整型数字。");
            }
        }


        public void makeStart()
        {
            tm.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            tm.Start();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            tm.Stop();
        }

        private void AllChargeBook_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released)
            {
                string information = "";
                for (int i = 0; i < theControlers.Count; i++)
                    if (theControlers[i].chargesBook.Count > 0)
                        information += theControlers[i].chargesBook[0].geFullInformation() + "\n";
                MessageBox.Show(information);
            }
        }

        private void AllCharge_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released)
                {
                    string informaion = "";
                    for (int i = 0; i < theControlers.Count; i++)
                        informaion += theControlers[i].charges[0].geFullInformation() + "\n";
                    MessageBox.Show(informaion);
                }
            }
            catch
            {
                MessageBox.Show("信息获取失败");
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < theControlers.Count; i++)
            {
                theControlers[i].forTimer();
            }
            flashWindow();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            makeExample();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {

            flashWindow();

            showTextForEDF.Text = "";
            theControlers = new List<EDF.controller>();
            textForInformation = "";
            for (int i = 0; i < theControlers.Count; i++)
                theControlers[i].allTimer = 1;
            isAValuableCharge = false;
            //checkIsValuable.IsChecked = false;
        }

        private void checkIsValuable_Checked(object sender, RoutedEventArgs e)
        {
            isAValuableCharge = !isAValuableCharge;
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < theControlers.Count; i++)
            {
                thePictureWindow = new Picture();
                thePictureWindow.thePictureString = theControlers[i].getShowerText();
                thePictureWindow.theMainWindow = this;
                thePictureWindow.Show();
                thePictureWindow.makeControllerInformation(theControlers[i]);
            }
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            for (int k = 0; k < theControlers.Count; k++)
            {
                //容错
                //定理3条件1
                float checker = 0;
                for (int i = 0; i < theControlers[k].chargesBook.Count; i++)
                    checker += theControlers[k].chargesBook[i].getCheckValue();

                //定理3条件2
                int min = theControlers[k].minChargeLength();//最小运行时间
                int T = theControlers[k].getAllT();//T所有任务周期的最小公倍数
                                                   //MessageBox.Show("周期最小公倍数："+T+"\n最小运行时间："+min);//额外的输出显示信息（消息框）
                if (checker <= 1 && (T * (1 - checker) < min))
                {
                    MessageBox.Show("这个算法可以直接被调度");
                    return;//这个是可以调度的，所以直接返回
                }
                //////////////////////////////////////使用backward-EDF(第二步)
                try
                {
                    theControlers[k].schedulingMode = 3;//进入容错方式（backward -EDF进行调度）
                    MessageBox.Show("使用backward-EDF进行调度");
                    tm.Stop();
                    theControlers[k].allTimer = 0;
                    while (theControlers[k].allTimer < T)
                    {
                        for (int i = 0; i < theControlers.Count; i++)
                        {
                            theControlers[k].forTimer();
                        }
                        flashWindow();
                    }
                }

                catch
                {
                    theControlers[k].allTimer = 0;
                    theControlers[k].schedulingMode = 1;//进入容错方式（EDF进行调度）
                    while (theControlers[k].allTimer < T)
                    {
                        for (int i = 0; i < theControlers.Count; i++)
                        {
                            theControlers[k].forTimer();
                        }//强行调度EDF，如果任务截止期错失，这个任务就彻底删除
                        flashWindow();
                    }
                }
                //    for (int i = 0; i < theControler.chargesBook.Count; i++)
                //{
                //     if (theControler.chargesBook[i].StartTime < controller.allTimer)
                //         checker += theControler.chargesBook[i].getCheckValue();
                // }
                // MessageBox .Show("使用backwards-EDF进行调度");


                // else//backward -EDF也无法调度，那么只有抛弃一些任务了
                // {
                //    this.theControler.schedulingMode = 1;//默认为EDF调度
                //     theControler.chargesBook = BCEVW.reMoveUnFeasible(theControler.chargesBook);
                //  }

            }
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            if (isScalling == false)
            {
                isScalling = true;
                timerScale = 0.2f;
                //button9.Content = "高速运行";
                tm.Interval = TimeSpan.FromSeconds(0.5 * timerScale);
            }
            else
            {
                isScalling = false;
                //button9.Content = "一般速度";
                timerScale = 1;
                tm.Interval = TimeSpan.FromSeconds(0.5 * timerScale);
            }

        }


        private bool checkForRM(List<charge> charges)//传入的是已经排好序的任务队列
        {

            for (int i = 0; i < charges.Count; i++)
            {
                int I = 0;
                int R = 0;
                do
                {
                    R = I + charges[i].ChargeLength;
                    if (R > charges[i].DeadLine) return false;
                    for (int j = 0; j < i; j++)
                    {
                        I += (R / charges[j].CircleTimer) * charges[j].ChargeLength;
                    }
                    Console.WriteLine("\nR " + R + "   I " + I);
                }
                while ((I + charges[i].ChargeLength) > R);

            }
            return true;
        }


        public void setCanRob(bool can = true)
        {
            for (int i = 0; i < theControlers.Count; i++)
            {
                theControlers[i].isCanRob = can;
            }
        }

        public void setSchedulingMode(int index)
        { 
             for (int i = 0; i<theControlers.Count; i++)
            {
                theControlers[i].schedulingMode = index ;
            }

        }


        public void ChangeCPUCount(int  newCount)
        {
            //Console.WriteLine("change -->"+newCount);
            theControlers = new List<controller>();
            for (int i = 0; i < newCount; i++)
            {
                controller theCPU = new controller();
                theCPU.theCPUName = "CPU" + i;
                theCPU.makeChargeShower();
                theCPU.theMainWindow = this;
                theControlers.Add(theCPU);

            }

            //flash window
            try
            {
                flashWindow();
                //AllChargeBook.Items.Clear();
                showTextForEDF.Text = "";
                textForInformation = "";
                for (int i = 0; i < theControlers.Count; i++)
                    theControlers[i].allTimer = 0;
                isAValuableCharge = false;
                //checkIsValuable.IsChecked = false;
            }
            catch
            {
                Console.WriteLine("初始化未完全，所以不用清理");
            }
        }

    }
}
