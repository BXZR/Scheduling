﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDF
{
   public  class stream
    {
        //数据流
        public string theName = "";
        public List<MainWindow> thePlanRoute = new List<MainWindow>();
        public int timeSlot = 0;
        //这个流的时间长度
        public int allTimeUse = 0;
        public int indexNow = 0;
        public int circleTime = 10;//周期
        public int deadLine = 10;//截止期 
        //是不是主路径流
        private bool isMainRoute = true;
        public bool isOver = false;

        public void setMainRoute(bool isMain)
        {
            isMainRoute = isMain;
            timeSlot = isMain ? 2 : 1;
        }

        public stream(int circleTimeIn, int deadLineIn, string theNameIn)
        {
            circleTime = circleTimeIn;
            deadLine = deadLineIn;
            theName = theNameIn;
            thePlanRoute = new List<MainWindow>();
        }
        
    }
}
