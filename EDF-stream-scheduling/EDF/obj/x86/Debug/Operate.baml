   M S B A M L   `   `   `  ����>  :EDF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:EDFEDF  5   	       EDF.Operate   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:EDF     5   	   -   5          � Title$  Operate��5   	   $��273.633��6   $��238.491��6*    � 
ResizeMode$ NoResize=�6h   .��5	      � ���	#FFE6E6E6.+�5
   
   �� -   5         ��Name$ buttonForDemo��5
      $��Build Map Demo��6(   $
��Left=�6A   $��
57,122,0,0q�6\   $	��Top=�6p   $	��130��6�    J� RenderTransformOrigin$ 1.295,4.847&�6�   �� 5   
   -   5        $ buttonForRun��5      $��Schedule��6'   $
��Left=�6:   $��
57,157,0,0q�6U   $	��Top=�6i   $	��130��6�   {�.  6�   �� 5   
   -   5        $ reSetButton��5      $��reSet��6&   $
��Left=�66   $��
57,196,0,0q�6Q   $	��Top=�6e   $	��130��6}   6�   	� 5   
   -   5         	�Name$ radioButton1��5   ^   $	��EDF��6   $��16��6$   $
��Left=�60   $��	57,68,0,0q�6K   $	��Top=�6t    d� 	IsChecked$
 TrueY�6�    	� 	GroupName$ method��6�   6�   	� 5   
   -   5        $ radioButton2��5   _   $	��LLF��6   $��16��6$   $
��Left=�60   $��
103,68,0,0q�6K   $	��Top=�6u   $ method��6�   6�   	� 5   
   -   5        $ radioButton3��5   ^   $��	非抢占��6   $��16��6$   $
��Left=�60   $��	68,93,0,0q�6K   $	��Top=�6t   $
 TrueY�6�   6�   	� 5   
   -   5        $ radioButton4��5   ^   $��抢占��6   $��16��6#   $
��Left=�6/   $��
128,93,0,0q�6J   $	��Top=�6t   6�   	� 5   
   -	   5        $ radioButton5��5   q   $��RM��6   $ method��6#   $��16��66   $
��Left=�6B   $��
146,68,0,0q�6]   $	��Top=�6�   6�   �� 5   
   -
   5         ��Name$ CPUCount��5      $
��Left=�6%   $��
128,32,0,0q�6@   $	��Top=�6T   $��73��6l    �� SelectedIndex$ 0��6w   .@�5      �� $��1��6   �� 5      $��2��6   �� 5      $��3��6   �� 5      $��4��6   �� 5      $��5��6   �� 5      $��6��6   �� 5      $��7��6   �� 5      $��8��6   �� 5      $��9��6   �� 5      $��10��6   �� 5      $��11��6   �� 5      $��12��6   �� 5      $��13��6   �� 5       $��14��6   �� 5!      $��15��6   �� 5"      $��16��6   5#      �� 5$   
   -   5        	 ��Name$	 label��5$      $��信道数量��6   $
��Left=�6.   $��	51,28,0,0q�6I   $��69��6\   $��14(�6g   ���	#FF0023FF$ 1.638,0.793&�6�   $��35��6�   $	��Top=�6�   5&      5'      