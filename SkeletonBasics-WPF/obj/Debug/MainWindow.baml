   M S B A M L   `   `   `  ����  SkeletonBasics-WPF8    2Microsoft.Samples.Kinect.SkeletonBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$,  &Práctica 2 NPI Oliver Sánchez Marín��5   	   $	��856��65   $	��770��6B    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SquareCheckBox �� 
TargetType �� Orientation �Name	 ��Name
 ��Name �� TextAlignment �� Triggers P� Property P� Value �� 
TargetName��%1  &	        &	 #     &	 F     '�� i     &	 �     7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5	   
   $ 	#ff00BCF2��62   �� 5
   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
Horizontal=�6)   
������ .+�5   "   � $ SquareCheckBoxChecked��6'   .+�5   &   �� $	 CheckedNormal��6,   $��Images\CheckedNormal.png��6C   $
��None=�6e   $��Center=�6t   �� 5   &   $	 CheckedHover��6,   $��Images\CheckedHover.png��6B   $
��None=�6c   $��Center=�6r   $y�	Collapsed=�6�   5   #   � 5   "   $ SquareCheckBoxUnchecked��6'   $y�	Collapsed=�6H   .+�5   &   �� $	 UncheckedNormal��6,   $ ��Images\UncheckedNormal.png��6E   $
��None=�6i   $��Center=�6x   �� 5   &   $	 UncheckedHover��6,   $��Images\UncheckedHover.png��6D   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   5   #   �� 5   "   $
 SquareCheckBoxText��6,   #��z��6H   $
 Left=�6i   $��Center=�6   8��  $��15(�6�   $��9,0,0,0q�6�   5      5       5      P� 5       �@d�	IsChecked$ falseY�6;   . �5    "   �� $ SquareCheckBoxChecked��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5!   "   $ SquareCheckBoxUnchecked��6O    � y�$ Visible=�6?   6O   5"      P� 5#       �@J�IsMouseOver . . �5$   "   �� $ CheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5%   "   $ UncheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5&   "   $ CheckedHover��6O    � y�$ Visible=�6?   6O   �� 5'   "   $ UncheckedHover��6O    � y�$ Visible=�6?   6O   �� 5(   "   8  $ SquareCheckBoxText��6h    � ��5)      5+      5-      5.      5/      .��51      � -   5        $��
layoutGrid��51      $��	10 0 10 0q�6    � RowDefinitions 52   
   �� 53      $
��Auto �6   �� 54      $��* �6   �� 55      $
��Auto �6   �� 56      $
��Auto �6   �� 57      $
��Auto �6   .+�59   
   ]� $��0��6   $��0 0 0 20q�6!   .+�5:      �� -   5        $��txtAyuda��5:   �   $��WrapWithOverflow=�6   $��Comic Sans MS)�69   $��Stretch=�6T   $��Stretch=�6r   $��0��6�   $��20(�6�   $
��Bold"�6�   $ Center=�6�   $��80��6�   5;      (� 5<   
   $��1��6    (� Stretch$ Uniform=�6   $��Center=�61   .��5=      �� -   5        $��ImageC��5=      $	��640��6"   $	��480��6.   5>      (� 5?   
   $��1��6   $ Uniform=�6   $��Center=�61   .��5@      �� -   5        $��ImageE��5@      $	��640��6"   $	��480��6.   5A      (� 5B   
   $��1��6   $ Uniform=�6   $��Center=�61   .��5C      �� -   5        $��figura1��5C      $��Images\figura1.png��6#   $	��640��6?   $	��480��6K   $y�Hidden=�6X   5D      (� 5E   
   $��1��6   $ Uniform=�6   $��Center=�61   .��5F      �� -   5        $��figura2��5F      $��Images\figura2.png��6#   $	��640��6?   $	��480��6K   $y�Hidden=�6X   5G      (� 5H   
   $��1��6   $ Uniform=�6   $��Center=�61   .��5I      �� -   5        $��figura3��5I      $��Images\figura3.png��6#   $	��640��6?   $	��480��6K   $y�Hidden=�6X   5J      (� 5K   
   $��1��6   $ Uniform=�6   $��Center=�61   .��5L      �� -	   5        $��figura4��5L      $��Images\figura4.png��6#   $	��640��6?   $	��480��6K   $y�Hidden=�6X   5M      (� 5N   
   $��1��6   $ Uniform=�6   $��Center=�61   .��5O      �� -
   5        $��figura5��5O      $��Images\figura5.png��6#   $	��640��6?   $	��480��6K   $y�Hidden=�6X   5P      �� 5Q   
   -   5        $��lblFin��5Q   �   $��1��6   $��EJERCICIO FINALIZADO��6   $��48(�6=   $
��Bold"�6K   ���	#FFC8FFFF$��79��6t   $��Center=�6�   $��Center=�6�   $y�Hidden=�6�   $	��543��6�   ���	#FF52318F$��92,261,92,277q�6�   �� 5S   
   -   5        $��
btnIniciar��5S   i   $��2��6   $��15(�6   $��Normal"�6,   $��34��6@   $��Center=�6L   $��Center=�6{   $	��136��6�   $��Iniciar ejercicio��6�   $��0,5,0,0q�6�   �� 5T   
   -   5        $��	btnSaltar��5T   �   $��2��6   $��Saltar tutorial��6   $��15(�68   $��Normal"�6F   $��34��6Z   $��Right=�6f   $��Center=�6�   $	��105��6�   $��0,5,0,0q�6�   �� 5U   
   -   5        $��checkBoxSeatedMode��5U   �   $��3��6   #��[ 6    $��Modo sentado��6H   $
��Auto��6_   $
��Left=�6m   $��Center=�6�   6�   �� 5V   
   -   5        $��	checkBox1��5V   e   $��3��6   $��Mostrar esqueleto��6    $
��Auto��6<   $
��Left=�6J   #��[ 6v   $��Center=�6�   $��	180,0,0,0q�6�   �� 5W   
   -   5        $��lblRep��5W   �   $��3��6   $��Segoe UI)�6   $��18(�63   #��[  6A   $��35��6o   $��Right=�6{   $��Center=�6�   $��43��6�   $��0,0,10,0q�6�   �� 5X   
   -   5        $��sldRepe��5X   _   $��3��6   $��23��6   $��Right=�6*    � Maximum$ 50Q�6F    � Minimum$ 1Q�6S   $��Center=�6n   $	��175��6�   $��0,0,60,0q�6�   �� 5Y   
   -   5        $��label3��5Y   �   $��3��6   $��Repeticiones: ��6   $��Segoe UI)�66   $��18(�6L   #��[  6Z   $��35��6�   $��Right=�6�   $��Center=�6�   $	��116��6�   $��	0,0,250,0q�6�   �� 5Z   
   -   5        $��	statusBar��5Z   <   $��4��6   $
��Left=�6!   $��Center=�6M   
�������#��[  6{   $��26��6�   .@�5[      �� $��0 0 0 10q�6   .��5\      �� -   5        $��statusBarText��5\      $��-1 0 0 0q�61   20Click 'Seated' to change skeletal pipeline type!6u   5]      5^      5_      5`      