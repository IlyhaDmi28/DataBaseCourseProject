   M S B A M L   `   `   `  ����N  JFinancial assistant, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Financial_assistantFinancial_assistant  5   	   '    !Financial_assistant.AddUserWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:Financial_assistant     5   	     � Title$  AddUserWindow��5   	   $	��650��6   $	��470��6,   $	��470��5	   	   $	��600��6    � Icon$ Picture/icons/icon.png��5
   	    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $" Resources/RoundElements.xaml?�6%   %    5      .��5      �� .+�5   
   �� $	��160��6   $	��160��6   $��0 20 0 0q�6*   ��6<   �� *SelectPictureCommand+.��5      �� $	��160��6/   $	��160��6;   ��6   �� *	Picture+6;   5      (   "Financial_assistant.PlaceholdField  5   
   -   5          Name$
 Name��5         Placeholder$ Имя��6-   $��0 40 0 0q�6@     5   
   -   5        $ Login��5      $ 
Логин��6.   $��0 40 0 0q�6C     5   
   -   5        $ Password��5      $ Пароль��61   $��0 40 0 0q�6G   	� 5   
   -   5         	�Name$ UserRole��5      $��Пользователь��6(   $��18(�6?   $��	40 40 0 0q�6M    d� 	IsChecked$
 TrueY�6`   	 	� 	GroupName$	 1��6q   	� 5   
   -   5        $ 	AdminRole��5      $ ��Администратор��6)   $��18(�6A   $��	40 10 0 0q�6O   $	 1��6b   �� 5   
   $	��170��6   $��40��6   $��0 50 0 0q�6)   
��� � �$��Добавить��6N   $��18(�6a   
�������   ButtonRound#��[  6�   ��6�   �� *AddUserCommand+6�   5      5      