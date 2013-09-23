using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace PaintRT
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		RotateTransform colorsRotation = new RotateTransform();
		TranslateTransform shapeSlider = new TranslateTransform();
		RTColors chosenColor = RTColors.Red;
		RTShapes chosenShape = RTShapes.Circle;
		Dictionary<Color, Color> nextColor = new Dictionary<Color, Color>()
		{
			{RTColorsToColor(RTColors.Red), RTColorsToColor(RTColors.Black)},
			{RTColorsToColor(RTColors.Black), RTColorsToColor(RTColors.Blue)},
			{RTColorsToColor(RTColors.Blue), RTColorsToColor(RTColors.Green)},
			{RTColorsToColor(RTColors.Green), RTColorsToColor(RTColors.Red)},
		};

		public MainPage()
		{
			this.InitializeComponent();
			this.RotateColors.RenderTransform = this.colorsRotation;
			this.MoveShapes.RenderTransform = this.shapeSlider;
		}

		public void DrawingField_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var drawingField = sender as Canvas;
			if (drawingField != null)
			{
				double pointedX = e.GetPosition(this).X;
				double pointedY = e.GetPosition(this).Y;
				UIElement createElement = CreateSelectedElement();
				Canvas.SetTop(createElement, pointedY - 100);
				Canvas.SetLeft(createElement, pointedX - 100);
				drawingField.Children.Add(createElement);
			}
		}

		public void Button_Save(object sender, RoutedEventArgs e)
		{
			//var a = DrawingField.Children[0];
		}

		private RTColors GetClosestColor(double currentAngleDouble)
		{
			int currentAngle = (int)currentAngleDouble;
			if (currentAngle < 0) currentAngle += 360;
			if (currentAngle > 45 && currentAngle < 135)
			{
				return RTColors.Green;
			}
			else if (currentAngle > 135 && currentAngle < 225)
			{
				return RTColors.Blue;
			}
			else if (currentAngle > 225 && currentAngle < 315)
			{
				return RTColors.Black;
			}
			else
			{
				return RTColors.Red;
			}
		}

		private UIElement CreateSelectedElement()
		{
			Brush fillColor = new SolidColorBrush(RTColorsToColor(this.chosenColor));
			UIElement newElement = new Ellipse();
			debug.Text = chosenShape.ToString();
			switch (chosenShape)
			{
				case RTShapes.Line:
					newElement = new Rectangle()
					{
						Fill = fillColor,
						Width = 10,
						Height = 200,
					};
					break;
				case RTShapes.Circle:
					newElement = new Ellipse()
					{
						Fill = fillColor,
						Width = 200,
						Height = 200,
						StrokeThickness = 200,
					};
					break;
				case RTShapes.Square:
					newElement = new Rectangle()
					{
						Fill = fillColor,
						Width = 200,
						Height = 200,
					};
					break;
			}
			newElement.Holding += NewElementHolding;
			return newElement;
		}

		void NewElementHolding(object sender, HoldingRoutedEventArgs e)
		{
			var shapeElement = sender as Shape;
			if (e.HoldingState == Windows.UI.Input.HoldingState.Started)
			{
				if (shapeElement != null)
				{
					SolidColorBrush brush = shapeElement.Fill as SolidColorBrush;
					SolidColorBrush newColor = new SolidColorBrush(nextColor[brush.Color]);
					shapeElement.Fill = newColor;
				}
			}
			e.Handled = true;
		}

		private static Color RTColorsToColor(RTColors colorType)
		{
			Color newColor = new Color();

			switch (colorType)
			{
				case RTColors.Red:
					newColor = Colors.Red;
					break;
				case RTColors.Green:
					newColor = Colors.Green;
					break;
				case RTColors.Blue:
					newColor = Colors.Blue;
					break;
				case RTColors.Black:
					newColor = Colors.Black;
					break;
			}
			return newColor;
		}

		private void RotateColors_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			var border = sender as Border;
			if (border != null)
			{
				this.colorsRotation.Angle += e.Delta.Rotation;
			}
		}

		private void RotateColors_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			var border = sender as Border;
			if (border != null)
			{
				this.chosenColor = this.GetClosestColor(colorsRotation.Angle);
				this.colorsRotation.Angle = (int)chosenColor;
			}
		}

		private void MoveShapes_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			var shapes = sender as StackPanel;
			if (shapes != null)
			{
				var xOffset = e.Delta.Translation.X;

				this.shapeSlider.X += xOffset;
				if (this.shapeSlider.X < -55)
				{
					this.shapeSlider.X = -55;
				}
				if (this.shapeSlider.X > 52)
				{
					this.shapeSlider.X = 52;
				}
			}
		}

		private void MoveShapes_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			var shapes = sender as StackPanel;
			if (shapes != null)
			{
				var shapeSliderPos = this.shapeSlider.X;
				if (shapeSliderPos < -30)
				{
					this.chosenShape = RTShapes.Square;
				}
				else if (shapeSliderPos > 28)
				{
					this.chosenShape = RTShapes.Line;
				}
				else
				{
					this.chosenShape = RTShapes.Circle;
				}
				this.shapeSlider.X = (int)this.chosenShape;
			}
		}
	}
}