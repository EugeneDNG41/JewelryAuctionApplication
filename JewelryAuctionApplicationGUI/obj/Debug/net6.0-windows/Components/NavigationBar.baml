   M S B A M L   `   `   `  ����W  SJewelryAuctionApplicationGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:JewelryAuctionApplicationGUI.Components'JewelryAuctionApplicationGUI.Components  5      ;    5JewelryAuctionApplicationGUI.Components.NavigationBar   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Glocal5clr-namespace:JewelryAuctionApplicationGUI.Components     5      
����=H�  � 	Resources  5
       �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value    BooleanToVisibilityConverter��%  '��       &	  �    �� 5   
   " ��6   .�5      ��  � ��
 ���� 6+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6$   5      5       5      P� 5       �@J�IsMouseOver$
 True��61   . �5      ��  � �� 5      �� 5      " ��6.   .R�5   "   �� #��z��6,   $��	Underline{�6M   5      5      5       5"      �� 5#   
   6'   5$      .��5%      � $
��1000��6   $��20 10q�6    � ColumnDefinitions 5&   
   �� 5'      $��* �6   �� 5(      $
��auto �6   �� 5)      $
��auto �6   �� 5*      $
��auto �6   �� 5+      $
��auto �6   �� 5,      $
��auto �6   �� 5-      $
��auto �6   .+�5/   
   �� $��0��50      $��14(�51      
�������$��Placeholder name��53      �� 55   
   $��1��56      $��10 0 0 0q�57      $
��Home��59      ��58      �� *NavigateHomeCommand+59      �� 5;   
   $��3��5<      $��10 0 0 0q�5=      $��Login��5?      ��5>      �� *NavigateLoginCommand+y�5@      �� *IsLoggedOut+	 �� 	Converter#	 [  �� 5A   
   $��4��5B      $��10 0 0 0q�5C      $��Sign up��5E      ��5D      �� *NavigateSignupCommand+y�5F      �� *IsLoggedOut+#	 [  �� 5O   
   $��6��5P      $��10 0 0 0q�5Q      $��Logout��5S      ��5R      �� *NavigateLogoutCommand+y�5T      �� *
IsLoggedIn+#	 [  5U      5V      