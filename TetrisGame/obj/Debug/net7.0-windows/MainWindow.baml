   M S B A M L   `   `   `  ����E  ATetrisGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TetrisGame
TetrisGame  5   	       TetrisGame.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TetrisGame     5   	     � Title$  Tetris Game��5   	   $	��600��6   $	��800��6*    � 	Resources 5	         GameButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%A  &	        �� 5   
   " ��6(   .�5      ��  � ��
 ��  �6+   �� 5       � ��
 �����6+   �� 5       � ��$ 14(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 5q�6'   �� 5       � ��$ 10q�6(   �� 5       � ��$ 2q�60   �� 5       � ��
 �   �6,   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 5��5   !   ._�5      �� $��Center=�6/   $��Center=�5   /   #��z��5   /   5      5       5"      5#      5%      .��5&      �  � ColumnDefinitions 5(   
   �� 5)      $��5* �6   �� 5*      $��2* �6   .+�5.   
   �� ���#333$��0��6#   $��10q�63   $ 10��6?   ._�5/      � -   5        $��GameGrid��5/      50      �� 52   
   $��1��6   $	��Top=�6%   $��Center=�6=   $��10q�6Z   .+�53      �� $��Score��6   $��Center=�6$   $��16(�6A   $
��Bold"�6O   
����  �6a   �� 54      -   5        $��ScoreTextBlock��54      $��Center=�6.   $��14(�6K   �� 56      
�������$	��120��6,   $	��120��68   $��10q�6E   $ 10��6Q   ._�57      �� -   5        $��NextBlockCanvas��57      58      �� 5:      $��Speed��6   $��Center=�6$   $��16(�6A   $
��Bold"�6O   
����  �6a   �� 5;      -   5        $��SpeedTextBlock��5;      $��Center=�6.   $��14(�6K   �� 5<      $��Level��6   $��Center=�6$   $��16(�6A   $
��Bold"�6O   
����  �6a   �� 5=      -   5        $��LevelTextBlock��5=      $��Center=�6.   $��14(�6K   �� 5@      -   5        $��SoundToggleButton��5@      
�������
�������$��Center=�6U   $��50��6r   $��50��6}   $��10q�6�   .��5A      �� -   5         ��Name$ 
SoundImage��5A      $��Images/sound_icon.png��6,   $��35��6K   $��35��6V   5B      �� 5E      -   5        $��StartGameButton��5E      $��
Start Game��60   #��[  6E   6n   �� 5F      -	   5        $��PauseGameButton��5F      $��
Pause Game��6,   #��[  6A   6j   �� 5G      -
   5        $��EndGameButton��5G      $��End Game��6*   #��[  6=   6f   5I      5J      5K      