   M S B A M L   `   `   `  ����  OrderEntrySystem$    OrderEntrySystem.OrderLineView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5       �� Source�� 5	   
   $ SharedResources.xaml?�6   %    .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��4q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��4 �6#   �� 5      $	��200 �6#    5      �� 5      $
��Auto �6    �� 5      $��4 �6    �� 5      $
��Auto �6    �� 5      $��4 �6    �� 5      $
��Auto �6    �� 5      $��4 �6    �� 5      $
��Auto �6    �� 5      $��4 �6    �� 5      $
��Auto �6    .+�5!      �� $��0��6   $��0��6!   $��Product:��61   $��Right=�6D    �� Target 6`   ��  �� ElementName$ productComboBox���� 5"      -   5         ��Name$ productComboBox��5"      $��0��60   $��2��6=   ��6M   ��  �� Path$ Products� �� SelectedItem 6s   �� $ Product��� 5#      $��2��6   $��0��6!   $��	Quantity:��61   $��Right=�6E    6a   �� $ quantityTextBox���� 5$      -   5        	 ��Name$	 quantityTextBox��5$      $��2��6/   $��2��6<      ValidationStyleTextBox#��[  6�   ��6L   �� $ Quantity�
 �� UpdateSourceTrigger$
 PropertyChanged=� �� ValidatesOnDataErrors . 6�   5%      �� 5&   
   $��1��6     NestedCommandTemplate#��[ 6H   $��Bottom=�6�   $��0,0,4,4q�6�   ��6&   �� $ Commands�6�   5'      5(      