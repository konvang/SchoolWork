   M S B A M L   `   `   `  ���� K  GOrderEntrySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:OrderEntrySystemOrderEntrySystem  "    OrderEntrySystem.ProductView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:OrderEntrySystem       � 	Resources   �� Source�� $ SharedResources.xaml?�%    .���  � RowDefinitions �� $
��Auto ��� $��* �.+�� $��4q� � ColumnDefinitions �� $
��Auto ��� $��4 ��� $	��200 � �� $
��Auto ��� $��4 ��� $
��Auto ��� $��4 ��� $
��Auto ��� $��4 ��� $
��Auto ��� $��4 ��� $
��Auto ��� $��4 ��� $
��Auto ��� $��4 ��� $
��Auto �.+��� $��0��$��0��$��Name:��$��Right=� �� Target ��  �� ElementName$ nameTextBox���� -    ��Name$ nameTextBox��$��0��$��2��   ValidationStyleTextBox#��[  ����  �� Path$
 Name� �� ValidatesOnDataErrors . 	 �� UpdateSourceTrigger$	 PropertyChanged=��� $��2��$��0��$��Description:��$��Right=� �� $ descriptionTextBox���� -   $ descriptionTextBox��$��2��$��2��#��[  ���� $ Description� . $	 PropertyChanged=��� $��4��$��0��$��Price:��$��Right=� �� $ priceTextBox���� -   $ priceTextBox��$��4��$��2��#��[  ���� $ Price� . $	 PropertyChanged=��� $��6��$��0��$��	Location:��$��Right=� �� $ locationComboBox���� -   
 ��Name$
 locationComboBox��$��6��$��2������ $ 	Locations� �� SelectedItem �� $ Location��� $��8��$��0��$��
Condition:��$��Right=� �� $ conditionComboBox���� -   $
 conditionComboBox��$��8��$��2������ $ 
Conditions� �� $ 	Condition��� $��10��$��0��$��	Quantity:��$��Right=� �� $ quantityTextBox���� -   $ quantityTextBox��$��10��$��2��#��[  ���� $ Quantity� . $	 PropertyChanged=�(   "OrderEntrySystem.MultiCategoryView  $��12��$��2�� � DataContext �� $ FilteredCategoryViewModel��� $��1��$��2��  NestedCommandTemplate#��[ $��Bottom=�$��0,0,4,4q����� $ Commands�