using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CHIP8Manager.Core.Emulation
{
    /// <summary>
    /// Provides an implementation for displaying drawn pixels on a virtual display.
    /// </summary>
    public class VirtualDisplay
    {
        private Color foregroundColor;
        private Color backgroundColor;
        private Byte emulatorResolutionScale;
        private Byte chip8PixelWidth;
        private Byte chip8PixelHeight;
        private DrawingVisual drawingVisual;
        private DrawingContext drawingContext;

        public VirtualDisplay(Color foregroundColor, Color backgroundColor, Byte emulatorResolutionScale, Byte chip8PixelWidth, Byte chip8PixelHeight)
        {
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
            this.emulatorResolutionScale = emulatorResolutionScale;
            this.chip8PixelWidth = chip8PixelWidth;
            this.chip8PixelHeight = chip8PixelHeight;
            this.drawingVisual = new DrawingVisual();
            this.drawingContext = this.drawingVisual.RenderOpen();           
        }

        public void DrawPixel(Byte x, Byte y)
        {
            Rect rect = new Rect(new System.Windows.Point(x * this.emulatorResolutionScale, y * this.emulatorResolutionScale), new System.Windows.Size(this.emulatorResolutionScale, this.emulatorResolutionScale));
            this.drawingContext.DrawRectangle(new SolidColorBrush(this.foregroundColor), (System.Windows.Media.Pen)null, rect);
        }

        public RenderTargetBitmap Render()
        {
            this.drawingContext.Close();
            RenderTargetBitmap bitmap = new RenderTargetBitmap(this.chip8PixelWidth * this.emulatorResolutionScale, this.chip8PixelHeight * this.emulatorResolutionScale, 0, 0, PixelFormats.Default);
            bitmap.Render(drawingVisual);
            this.drawingContext = this.drawingVisual.RenderOpen();
            return bitmap;
        }

        public void Clear()
        {
            Rect rect = new Rect(new System.Windows.Point(0, 0), new System.Windows.Size(this.chip8PixelWidth * this.emulatorResolutionScale, this.chip8PixelHeight * this.emulatorResolutionScale));
            this.drawingContext.DrawRectangle(new SolidColorBrush(this.backgroundColor), (System.Windows.Media.Pen)null, rect);
        }
    }
}
