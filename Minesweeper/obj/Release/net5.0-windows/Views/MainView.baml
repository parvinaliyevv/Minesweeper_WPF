   M S B A M L   `   `   `  ����   Minesweeper     Minesweeper.Views.MainView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  _ [MaterialDesignThemes.Wpf, Version=4.4.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  
�������$��Regular"�$��16(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�   MaterialDesignFont#���    � Title$ Minesweeper��$	��650��$
��1200�� � WindowStyle$
 None=� � AllowsTransparency$
 True��
������ .���� -    �� CornerRadius$ 7�����	#FF8ACA5D._�� $��5q� � RowDefinitions �� $��* �$��75���� $��8* �.+�]� .+��� -   $��
Difficulty�� " MaterialDesignOutlinedComboBox#��[ $
��Left=� �� SelectedIndex �� *Game.Difficulty+ �� Mode$ OneWayToSource=�$��Center=�
�������
���   �$��20 0 0 0q�$	��200��.@��� $
��Easy���� 	 �� 
IsSelected$
	 True��$��Medium���� $
��Hard���� $��Custom��*  $Microsoft.Xaml.Behaviors.Interaction
   Triggers
 +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ SelectionChanged��*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command �� *DifficultyChangedCommand+   CommandParameter ��  �� ElementName$ 
Difficulty���� $��Right=� �� Orientation$ 
Horizontal=�$��
0, 0, 5, 0q�.+��� $
��auto��$
��auto��$��5q�  MaterialDesignIconButton#��[ ���� *SoundOffCommand+����  �� RelativeSource �� *Self+.��'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Music=�
�������$��30��$��60���� $
��auto��$
��auto��$��5q�#��[ ���� *ResetGameCommand+����  �� *Self+.��  $ Refresh=�
�������$��30��$��60���� -   $
��auto��$
��auto��$��5q�#��[ .��  $ Close=�
�������$��30��$��60���� $��Center=�$��Center=�$ 
Horizontal=�.+��� $��/Views/Flag.png��$��Center=�$��40��$��50���� $��Center=�$��24(�$��5, 0, 30, 0q����� *Matrix.FlagCount+$ TwoWay=��� $��/Views/Clock.png��$��Center=�$��40��$��50���� $��Center=�$��24(�$��
5, 0, 0, 0q����� *Game.Time.ViewTime+$ TwoWay=��� $��1������ *Matrix.Cells+$ TwoWay=�$��5q�$	��2.5q����#574841���� .R�A�  A� Rows �� *Matrix.Rows+$ TwoWay=� A� Columns �� *Matrix.Columns+$ TwoWay=����� .R���  # MaterialDesignRaisedLightButton#��[  J� IsHitTestVisible �� *
IsEnable+���� *	Content+���� *
Background+���� *
Background+���� *Height+���� *
FontSize+���� *Row+���� *Column+$��2q� J� InputBindings d�  d� Gesture$ 	LeftClicka� �� Command ��  ��  �� AncestorType
 Window �� Mode$ FindAncestor=� �� Path$" DataContext.LeftClickCommand� �� CommandParameter ��  �� 
 Button$ FindAncestor=�d� $ 
RightClicka� ��  �� 
 Window$ FindAncestor=�$# DataContext.RightClickCommand� ��  �� 
 Button$ FindAncestor=�