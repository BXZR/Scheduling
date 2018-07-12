using System;
using System.Collections.Generic;
using System.Data;
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
    /// StreamShower.xaml 的交互逻辑
    /// </summary>
    public partial class StreamShower : Window
    {
        public StreamShower()
        {
            InitializeComponent();
        }

        public void setData(List<streamShowing> AA)
        {
            streamDataShow.ItemsSource = AA;
        }

        public void setData(List<List<int>> BB , int length = 0)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i <= length; i++)
                dt.Columns.Add(i.ToString(), typeof(int));

            for (int i = 0; i < BB.Count ; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j <= length; j++)
                    dr[j] = BB[i][j];
                dt.Rows.Add(dr);
            }
            streamDataShow.ItemsSource = dt.DefaultView;
        }
    }
}
