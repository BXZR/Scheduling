using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDF
{
   public class charge
    {
        private string name;//任务名称
        private int chargeLength;//任务长度
        private int circleTime;//周期
        private int deadLine;//截止时间
        private int timeFromBirth = 0;//从开始到此时的时间
        private int looseTimer;//松弛时间
        private int timerCount = 0;
        private int timeMax=99;
        private bool isValueAble = false;//重要度
        private int startTime;//任务初始到来时间
        public int LooseTimer { get { return looseTimer; } }//looseTimer松弛时间可以外部访问，其他都不可以
        public int CircleTimer { get { return circleTime; } }
        public string Name { get { return name; } }
        public int TimerCount { get { return timerCount; } }
        public int StartTime { get { return startTime; } }
        public bool IsValueAble { get { return isValueAble; } }
        public int ChargeLength { get { return chargeLength; } }
        public int DeadLine { get { return deadLine; } }
        private controller theController;
        public stream theStreamforCharge = null;//这个任务用来调度的流
        public MainWindow theMainWindow;//这个任务所属的窗口


        public bool isCrashed()
        {
            int loose = deadLine - chargeLength - theController.allTimer;
            if (loose < 0)
                return true;//超时
            return false;
        }

        public float getCheckValue()
        {
            return chargeLength / circleTime;
        }

        public bool canAdd()
        {
            timerCount++;
            if(timerCount <= timeMax)
                return true;
            return false;
        }
        public string geFullInformation()
        {
            return "名称：" + name + "\n任务长度：" + chargeLength + "\n任务周期：" + circleTime + "\n截止时间：" + deadLine + "\n执行次数：" + timeMax+"\n到来时间： "+startTime;

        }
        public string geFullInformationWithOutReturn()
        {
            return "名称：" + name + "   任务长度：" + chargeLength + "   任务周期：" + circleTime + "   截止时间：" + deadLine + "   执行次数：" + timeMax + "   到来时间：" + startTime;

        }
        public string getShortInformation()
        {
            string information = name + " / 时长" + chargeLength + "(+" + timeFromBirth + ") / 截止期" + deadLine;
            if (!basicCharge())
                information += "（不可调度）";
            if(!basicChargeForFlow())
                information += "（必定超期）";
            return information;
        }

        public charge(int chargeLength, int circleTime, int deadLine, int timerMax , int startTime ,string name, controller theControllerIn , bool isValueAble = false)
        {
            this.chargeLength = chargeLength;
            this.circleTime = circleTime;
            this.deadLine = deadLine;
            this.timeMax = timerMax;
            this.name = name;
            this.startTime = startTime;
            this.isValueAble = isValueAble;
            theController = theControllerIn;
        }
        public charge(charge theCharge, controller theControllerIn)
        {
            this.chargeLength = theCharge.chargeLength;
            this.circleTime = theCharge.circleTime;
            this.deadLine = theCharge.deadLine;
            this.timeMax = theCharge.timeMax;
            this.name = theCharge.name;
            this.startTime = theCharge .startTime;
            this.isValueAble = theCharge.isValueAble;
            theController = theControllerIn;
        }
        public charge(charge theCharge, int indexForThisKind, controller theControllerIn)//附加，这个是某一类进程的第几个实例
        {
            this.chargeLength = theCharge.chargeLength;
            this.circleTime = theCharge.circleTime;
            this.deadLine = theCharge.deadLine + (indexForThisKind-1)*CircleTimer;//相对截止时间变成绝对截止时间了
            this.timeMax = theCharge.timeMax;
            this.startTime = theCharge .startTime;
            this.isValueAble = theCharge.isValueAble;
            this.name = theCharge.name+"(第"+indexForThisKind+"实例)";
            theController = theControllerIn;
        }
        public bool basicCharge()
        {
            if (deadLine < chargeLength)
            {
                timeFromBirth = -999999;
                return false;
            }
            return true;
        }

        public bool basicChargeForFlow()
        {
            if (deadLine < (chargeLength+ timeFromBirth))
            {
                timeFromBirth = -999999;
                return false;
            }
            return true;
        }



        public void timer()
        {
            timeFromBirth++;
        }
        public void valueForCharge()
        {
            if (theController.schedulingMode == 0)//截止时间（EDF）
                looseTimer = deadLine;
            else if (theController.schedulingMode == 1)
                looseTimer = deadLine - chargeLength - theController.allTimer;//剩余的松弛时间（LLF）
            else if (theController.schedulingMode == 2)
                looseTimer = circleTime;//周期（RM）
            else if (theController.schedulingMode == 3)
                looseTimer = startTime;//backwards-EDF (截止时间和开始时间互换，使用EDF的方法，所以返回开始时间就行)
            if (this.isValueAble)
                looseTimer = -99999;

        }
       
        public void work()
        {
                chargeLength--;
                if (chargeLength > 0 && looseTimer < 10000)
                {
                    theMainWindow.textForInformation += name + "运行一个时间单位,还需要" + chargeLength + "时间\n";
                }
                if (looseTimer >= 10000)
                {
                theMainWindow.textForInformation += name + "必定超期，所以不运行了\n";
                }
           
                if (chargeLength<= 0)
                {
                    theController.removeCharge(this);
                }
        }
    }

}