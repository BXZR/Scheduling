using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDF
{

   public class showCharge
    {
        public charge theCharge;
        public int rowIndex;
        public showCharge(charge theCharge, int rowIndex)
        {
            this.theCharge = theCharge;
            this.rowIndex = rowIndex;
        }
    }

    //这个Controller就是CPU
   public  class controller
    {
        public string theCPUName = "CPU";
        public  List<controller> theAllController = new List<controller> ();//全局静态唯一控制器
        public List<string> controllerPictures;//用字符串拼接成的图
        public List<charge> charges;
        public charge selectedCharge = null;
        public List<charge> chargesBook;//任务的示例集合
        public  int allTimer = 0;//一共运行了多少时间单位
        public  int schedulingMode = 0;//默认的调度策略，默认调度为EDF，1 EDF 2 LLF 3 RM
        public  bool isCanRob = false;//可以抢占
        private List<showCharge> chargeShower;//用于显示图的字符串 
        public MainWindow theMainWindow;


        public controller()
        {
            charges = new List<charge>();
            chargesBook = new List<charge>();
            //theAllController.Add(this);
            chargeShower = new List<showCharge>();
            controllerPictures = new List<string>();
        }


       //求最小公约数（两个数）
        int gcd(int a, int b)
        {
            if (a < b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            if (b == 0)
                return a;
            else
                return gcd(b,a%b);
        }
       //求最小公倍数
        int lcm(int a, int b)
        {
            return a * b / gcd(a,b);
        }
       //求n个数的最小公倍数
        int nlcm(List<charge> book,int index)
        {
            if (index == 1 )
                return book[0].CircleTimer;
            else
            {
                return lcm( book[index -1].CircleTimer , nlcm(book ,index-1));
            }
        }

        public int getAllT()//获取所有的任务的周期的最小公倍数
        {
            return nlcm(chargesBook, chargesBook.Count);
        }

        public int minChargeLength()
        {
            int min = 9999999;
            for (int i = 0; i < chargesBook.Count; i++)
            {
                min = min < chargesBook[i].ChargeLength ? min : chargesBook[i].ChargeLength;
            }
            return min;
        }

       public void makeChargeShower()
        {
            chargeShower.Clear();
            controllerPictures.Clear();
            int rowsCount = chargesBook.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                chargeShower.Add(new showCharge(chargesBook[i], i));
                controllerPictures.Add("");
            }
            
        }

        public void makePictures(charge selected)
        {
            if (selected != null)
            {

                int index = -1;
                for (int i = 0; i < chargeShower.Count; i++)
                {
                    if (chargeShower[i].theCharge.Name == selected.Name.Split('(')[0])
                    {
                        //Console.WriteLine(chargeShower[i].theCharge.Name + "[[");
                        index = i;
                    }
                }

                if (index >= 0)
                {
                    int i = 0;
                    for (; i < chargeShower.Count; i++)
                    {
                        if (chargeShower[i].rowIndex == (chargeShower.Count - 1 - index))
                        {
                            controllerPictures[i] += "|" + selected.Name.Split('(')[0].PadRight(5) + "|";
                        }
                        else
                        {
                            string namer = chargeShower[i].theCharge.Name.Split('(')[0];
                            string s = "|";
                            s = s.PadLeft(namer.Length, '　');
                            controllerPictures[i] += "|  " + s;
                        }
                    }

                }
                else
                {
                    for (int i=0; i < chargeShower.Count; i++)
                    {
        
                            string namer = chargeShower[i].theCharge.Name.Split('(')[0];
                            string s = "|";
                            s = s.PadLeft(namer.Length, '　');
                            controllerPictures[i] += "|  " + s;
                    }
                }
            }
            else
            {
                for (int i = 0; i < chargeShower.Count; i++)
                {

                    string namer = chargeShower[i].theCharge.Name.Split('(')[0];
                    string s = "|";
                    s = s.PadLeft(namer.Length, '　');
                    controllerPictures[i] += "|  " + s;
                }
            }

        }

        public string getShowerText()
        {
            string shower = "";
            for (int i = 0; i < controllerPictures.Count; i++)
            {
                shower += controllerPictures[i];
                shower += "\n";
            }
            return shower;
        }
        public void addCharge(charge newCharge)
        {
            charges.Add(newCharge);
        }
        public void removeCharge(charge theCharge)
        {
            if (theCharge == null )
                return;
            if (charges.Contains(theCharge))
            {
                if (theCharge.theStreamforCharge != null && theCharge.theStreamforCharge.thePlanRoute.Count > 0 && theCharge.theStreamforCharge.isOver == false)
                {
                    int timeUseForThisStream = this.allTimer - theCharge.StartTime;
                    theCharge.theStreamforCharge.allTimeUse += timeUseForThisStream;
                    theCharge.theStreamforCharge.indexNow++;
                    if (theCharge.theStreamforCharge.indexNow < theCharge.theStreamforCharge.thePlanRoute.Count)
                    {
                        theMainWindow.textForInformation += theCharge.Name + "转到下一个节点\n";
                        theCharge.theStreamforCharge.thePlanRoute[theCharge.theStreamforCharge.indexNow].getStream(theCharge.theStreamforCharge);
                    }
                    else if (theCharge.theStreamforCharge.indexNow == theCharge.theStreamforCharge.thePlanRoute.Count)
                    {
                        //if(theCharge.theStreamforCharge.thePlanRoute[theCharge.theStreamforCharge.thePlanRoute.Count - 1] == this.theMainWindow )
                            theCharge.theStreamforCharge.isOver = true;
                        Operate.theStringForOperate += theCharge.theStreamforCharge.theName + ":\n跳数：" + theCharge.theStreamforCharge.thePlanRoute.Count + "\ntimeUse = " + theCharge.theStreamforCharge.allTimeUse + "\n--------------------------\n";
                        //if (Operate.isAllOver())
                        //    System.Windows.MessageBox.Show(Operate.theStringForOperate);
                        Operate.thestreamShows.Add(new streamShowing(theCharge.theStreamforCharge.theName + "   ", theCharge.theStreamforCharge.thePlanRoute.Count.ToString("f0") + "   ", theCharge.theStreamforCharge.allTimeUse.ToString("f0")+"   "));
                        if (Operate.isAllOver())
                        {
                            StreamShower A = new StreamShower();
                            A.streamDataShow.ItemsSource = Operate.thestreamShows;
                            A.Show();
                            Console.WriteLine("OVER");
                        }
                    }
                }
                charges.Remove(theCharge);
                schedule();
                theMainWindow.textForInformation += theCharge.Name + "完成，调度一次\n";
                theCharge = null;
            }
        }

        void quickSort( List<charge> a ,int low,int high)
        {
	        if(low>= high)
	        return;
	        int first = low;
	        int last = high;
	        charge keyValue = a[low];
	        while(low<high)
	        {
		        while(low<high && a[high].LooseTimer>= keyValue.LooseTimer)
		        high--;
		        a[low] = a[high];
		        while(low<high && a[low].LooseTimer<= keyValue.LooseTimer)
		        low++;
		        a[high] = a[low];
	        }
	        a[low] = keyValue;
            quickSort(a, first, low - 1);
            quickSort(a, low + 1, last); 
        }

        public void schedule(int indexFrom =0)
        {
            if (charges.Count >= 1)
            {
                quickSort(charges, indexFrom, charges.Count - 1);
            }
            if (charges.Count > 0)
                selectedCharge = charges[0];
            else
                selectedCharge = null;
        }

        public void theSort()//只是排序不调度
        {
            quickSort(charges, 0, charges.Count - 1);
        }

        private void flashItems()
        {
            for (int i = 0; i < charges.Count; i++)
            {
                charges[i].valueForCharge();
            }
        }

        private void makeItem()
        {
            for (int i = 0; i < charges.Count; i++)
            {
                charges[i].timer();
            }
        }
        public void forTimer( bool canMissCrash =false)//每一个时间单位都做些什么，算是总的调用方法
        {

                theMainWindow.textForInformation += "\n"+theCPUName +"  第" + allTimer + "时钟——————————————\n";
                flashItems();
                for (int i = 0; i < chargesBook.Count; i++)
                {
                    if ((((allTimer) % chargesBook[i].CircleTimer) == chargesBook[i].StartTime )&& chargesBook[i].canAdd())
                    {
                    charge aCharge = new charge(chargesBook[i], chargesBook[i].TimerCount, this);
                    aCharge.theMainWindow = chargesBook[i].theMainWindow;
                    aCharge.theStreamforCharge = chargesBook[i].theStreamforCharge;

                    charges.Add(aCharge);
                        flashItems();
                    theMainWindow.textForInformation += chargesBook[i].Name + "新的实例到达,调度一次\n";
                        if (charges.Count == 0)
                            schedule();
                        else
                            schedule(1);
                    }
                }
                if (isCanRob )
                {
                    if (allTimer > 1)
                        schedule(0);
                }
                makePictures(selectedCharge);
                allTimer++;
                makeItem();

                if (canMissCrash)
                {
                    bool isCrashed =selectedCharge .isCrashed();
                    if (isCrashed)
                    {
                        List<charge> toDelete = new List<charge>();
                        for (int i = 0; i < charges.Count; i++)
                        {
                            if (charges[i].Name == selectedCharge.Name)
                                toDelete.Add(charges [i]);
                        }
                        for (int i = 0; i < toDelete.Count; i++)
                        {
                            charges.Remove(toDelete [i]);
                        }
                        toDelete.Clear();
                        for (int i = 0; i < chargesBook.Count; i++)
                        {
                            if (theAllController[i].chargesBook[i].Name == selectedCharge.Name)
                                toDelete.Add(theAllController[i].chargesBook[i]);
                        }
                        for (int i = 0; i < toDelete.Count; i++)
                        {
                            theAllController[i].chargesBook.Remove(toDelete[i]);
                        }
                    }
                }
                if (selectedCharge != null)
                {
                    selectedCharge.work();
                }
                else
                {
                   theMainWindow.textForInformation += "当前没有任何任务\n";
                }
        }   
    }
}
