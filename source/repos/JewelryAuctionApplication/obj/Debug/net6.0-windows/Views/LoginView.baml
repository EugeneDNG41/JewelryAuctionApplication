   M S B A M L   `   `   `  ����O  KJewelryAuctionSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:JewelryAuctionSystem.ViewsJewelryAuctionSystem.Views  5      ;"clr-namespace:JewelryAuctionSystemJewelryAuctionSystem  6E   Q-clr-namespace:JewelryAuctionSystem.ViewModelsJewelryAuctionSystem.ViewModels  6�   *    $JewelryAuctionSystem.Views.LoginView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:JewelryAuctionSystem.Views     5      Cjewelryauctionsystem"clr-namespace:JewelryAuctionSystem     6E   D
viewmodels-clr-namespace:JewelryAuctionSystem.ViewModels     6�   
�������.��5      �   � RowDefinitions  5   
   �� 5      $��30 �6   �� 5      .+�5   
   �� $	��320��6   $��1��5       �� Orientation$ Vertical=�5      $��0,35,0,0q�5      .+�5      �� $��
GEM GAMBIT��6   $��25(�5      $��Medium"�5      $��
Montserrat)�5      $��Center=�5      �� 5      $"��Take a gamble for your gems.��6   $��12(�5       $��Medium"�5!      $��
Montserrat)�5"      $
��Wrap=�5#       �� TextAlignment$ Center=�5$      $��0,5,0,0q�5%      �� 5(      $��Username��6   $��12(�5*      $��Medium"�5+      $��
Montserrat)�5,      $��0,35,0,0q�5-      �� 5/      -   5         ��Name$ txtUser��5/      $��13(�51      $��Medium"�52      $��
Montserrat)�53      $��28��54      $��Center=�55      $��0,5,0,0q�56      $��20,0,0,0q�57      ��50      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�.��57   %   58      �� 5:      $��Password��6   $��12(�5<      $��Medium"�5=      $��
Montserrat)�5>      $��0,15,0,0q�5?      .   (JewelryAuctionSystem.BindablePasswordBox  5A      $��28��5B   %   $��0,5,0,0q�5C   %      Password 5A   7   �� *
Password+ �� Mode$ TwoWay=�$ PropertyChanged=�5D      �� 5F      ���#D7596D$��12(�5H      $��Medium"�5I      $��
Montserrat)�5J      $��0,10,0,0q�5K      $
��Wrap=�5L      ��5F      �� *ErrorMessage+5L      �� 5N      -   5         ��Name$ btnLogin��5N      $��0q�5P      $��LOG IN��5Q      
�������$��12(�5S      $��
Montserrat)�5T       � Cursor$
 Hand��5U      $��0,30,0,0q�5V      ��5O      �� *LoginCommand+��5X      �� 5Y      	 �� 
TargetType"	 ��6   .�5Z      �� 
 �� Property
 � �� �� Value
 �   �67    �� Triggers 5[      P� 5\       P� Property �@J�IsMouseOver P� Value$
 True��6=   . �5]   "   �� 
 � �� �#28AEED6?   5^      5`      ��5c      �� 5d       �� 
TargetType" ��6&   .R�5e      �� $	��150��6!   $��40��6-    �� CornerRadius$ 20��5f      #��z��5g      ._�5h      �� $��Center=�6/   $��Center=�5i   +   5j      5k      5m      �� 5o      $ 
Horizontal=�6   $��Center=�5p      $��0,15,0,0q�5q      .+�5s      �� $��Forgot Password?��6   $��12(�5u      $��Medium"�5v      $��
Montserrat)�5w      �� 5y      $��Reset��6   
���OO/�$��12(�5{      $��Medium"�5|      $��
Montserrat)�5}      $
 Hand��5~      $��8,0,0,0q�5      5�      5�      5�      5�      