using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConwaysGameOfLife
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            randomGen = new Random();

            black = rgbaToColor(0, 0, 0, 255);
            white = rgbaToColor(255, 255, 255, 255);

            this.Load += FormLoaded;
        }

        public void FormLoaded(object sender, System.EventArgs e)
        {
            InitializeGraphics((Form1)sender);
        }

        CustomBitmap graphics;
        public Random randomGen;
        public Form1 formHolding;
        private int[] newArray;
        private int[] oldArray;
        int black;
        int white;

        Thread frameThread = null;

        /// <summary>
        /// This initializes front and back buffer
        /// The idea here is that one of these can be the old array,
        /// which is the one that you read from when making changes.
        /// These changes then occur by writing to the new array,
        /// then that new array is passed into the CustomBitmap (graphics)
        /// object above to actually update the graphics.
        ///
        /// You can see an example of this in the DrawStuff method below.
        /// </summary>
        public void InitializeGraphics(Form1 formHolding)
        {
            graphics = new CustomBitmap(GraphicsBox, formHolding);

            int graphicsSize = graphics.GetDataSize();
            newArray = new int[graphicsSize];
            oldArray = new int[graphicsSize];

            // Fill buffers initially with random data
            for (int i = 0; i < newArray.Length; i++)
            {
                if ((randomGen.Next() & 1) == 0)
                {
                    newArray[i] = black;
                    oldArray[i] = black;
                }
                else
                {
                    newArray[i] = white;
                    oldArray[i] = white;
                }
            }

            // Assign data to buffers (and flip the buffers to display data)
            graphics.SetBitmapData(newArray);

            // Create a separate thread to do the heavy processing
            // This allows other non-existent GUI functions to run as needed
            frameThread = new Thread(() => EveryFrame());
            frameThread.IsBackground = true;
            frameThread.Start();
        }
        public void OpenEditorMode(Form1 formHolding)
        {
            // Fill buffers initially with white
            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = black;
                oldArray[i] = black;
            }

            // Assign data to buffers (and flip the buffers to display data)
            graphics.SetBitmapData(newArray);
        }

        /// <summary>
        /// This method is intended to be run as a seperate thread,
        /// and updates graphics (and other game logic) repeatedly,
        /// computing the FPS every second as well.
        /// </summary>
        private void EveryFrame()
        {
            long curSecond = DateTime.Now.Second;
            int numPerSecond = 0;
            while (!isPaused)
            {
                if (curSecond != DateTime.Now.Second)
                {
                    // This value can be displayed as needed
                    int FPS = numPerSecond;

                    this.BeginInvoke(new Action(() => { lbl_FPS.Text = "FPS: " + FPS.ToString(); }));

                    numPerSecond = 0;
                    curSecond = DateTime.Now.Second;
                }
                numPerSecond++;

                DrawStuff();
            }
        }

        /// <summary>
        /// Does one cycle of rules for Conway's Game of Life,
        /// and updates the graphics accordingly.
        ///
        /// As explained above, after we swap them (so old array is the
        /// previous new array), we read from old array and never write to it,
        /// and write to new array but never read from it (since it's values
        /// before they are assigned are old old - I mean you could read from it,
        /// it just wouldn't help).
        ///
        /// Then once we have the new generation values, we pass them into
        /// our graphics to be stored in a Bitmap and displayed.
        /// </summary>
        public void DrawStuff()
        {
            // Swap the two byte buffers (so the new is now old and we can modify other)
            int[] tempArray = oldArray;
            oldArray = newArray;
            newArray = tempArray;

            int width = GraphicsBox.Width;
            int height = GraphicsBox.Height;

            // oldArray is intended to be read from for data,
            // and newArray is intended to be written to based on that data.
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int neighbors = 0;
                    int curPos = x + y * width;

                    if (oldArray[curPos - 1 - width] == white) neighbors++;
                    if (oldArray[curPos - width] == white) neighbors++;
                    if (oldArray[curPos + 1 - width] == white) neighbors++;
                    if (oldArray[curPos - 1] == white) neighbors++;
                    if (oldArray[curPos + 1] == white) neighbors++;
                    if (oldArray[curPos - 1 + width] == white) neighbors++;
                    if (oldArray[curPos + width] == white) neighbors++;
                    if (oldArray[curPos + 1 + width] == white) neighbors++;

                    if (oldArray[curPos] == white && (neighbors < 2 || neighbors > 3))
                        newArray[curPos] = black;
                    else if (oldArray[curPos] == black && neighbors == 3)
                        newArray[curPos] = white;
                    else
                        newArray[curPos] = oldArray[curPos];
                }
            }

            graphics.SetBitmapData(newArray);
        }

        /// <summary>
        /// "Packs" the given rgba byte values (0-255) into
        /// the corresponding integer value, for use in the array
        /// above.
        ///
        /// Technically a byte array could be used throughout
        /// this code and then this method wouldn't be needed, but I
        /// think an integer array is more efficient and actually makes
        /// the code cleaner in the long run.
        /// </summary>
        public int rgbaToColor(byte r, byte g, byte b, byte a)
        {
            return r + (g << 8) + (b << 16) + (a << 24);
        }

        bool isPaused = false;
        private void GraphicsBox_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (isPaused)
                    {
                        var pos = GraphicsBox.Width * e.Y + e.X;
                        newArray[pos] = white;
                        oldArray[pos] = white;

                        // Assign data to buffers (and flip the buffers to display data)
                        graphics.SetBitmapData(newArray);
                    }
                    break;
                case MouseButtons.Right:
                    isPaused = !isPaused;

                    if (isPaused)
                    {
                        frameThread.Suspend();

                        OpenEditorMode(formHolding);
                    }
                    else
                    {
                        frameThread.Resume();
                    }
                    break;
                case MouseButtons.Middle:
                    break;
                default:
                    break;
            }
        }

        private void GraphicsBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isPaused)
            {
                if (e.X < GraphicsBox.Width && e.Y < GraphicsBox.Height)
                {
                    var pos = GraphicsBox.Width * e.Y + e.X;
                    newArray[pos] = white;
                    oldArray[pos] = white;

                    // Assign data to buffers (and flip the buffers to display data)
                    graphics.SetBitmapData(newArray); 
                }
            }
        }
    }

    /// <summary>
    /// Contains a Bitmap to be written to as needed.
    /// The idea here is that a PictureBox is provided,
    /// and a Bitmap of that same size is created.
    ///
    /// GetDataSize() will then return the size of the int[]
    /// array that needs to be created, and an int[] array of that
    /// size with colors created by the rgbaToColor method above.
    /// That int[] array can be passed into the SetBitmapData method
    /// which will display the corresponding graphics on the screen.
    /// </summary>
    public class CustomBitmap
    {
        private PictureBox holder;
        private Bitmap frontBitmap;
        public Bitmap backBitmap;
        private Rectangle bitmapRectangle;
        private Form1 formHolding;
        private bool isFormClosed;

        public CustomBitmap(PictureBox holder, Form1 formHolding)
        {
            this.holder = holder;
            this.formHolding = formHolding;

            this.formHolding.FormClosed += FormHoldingCloseCallback;
            isFormClosed = false;

            frontBitmap = new System.Drawing.Bitmap(holder.Width, holder.Height, PixelFormat.Format32bppArgb);
            frontBitmap.SetResolution(1000, 1000);
            backBitmap = new System.Drawing.Bitmap(holder.Width, holder.Height, PixelFormat.Format32bppArgb);
            backBitmap.SetResolution(1000, 1000);
            bitmapRectangle = new Rectangle(0, 0, holder.Width, holder.Height);
        }
        public void FormHoldingCloseCallback(object sender, System.EventArgs e)
        {
            this.isFormClosed = true;
        }
        public int GetDataSize()
        {
            return holder.Width * holder.Height;
        }
        public void SetBitmapData(int[] buffer)
        {
            lock (backBitmap)
            {
                BitmapData graphicsData = null;
                try
                {
                    // Lock the data bits of the back bitmap
                    graphicsData = backBitmap.LockBits(bitmapRectangle, System.Drawing.Imaging.ImageLockMode.ReadWrite, backBitmap.PixelFormat);

                    // Copy the RGBA values of the given buffer to the back bitmap's data
                    System.Runtime.InteropServices.Marshal.Copy(buffer, 0, graphicsData.Scan0, buffer.Length);
                }
                finally
                {
                    // Unlock the data bits of the back bitmap
                    backBitmap.UnlockBits(graphicsData);
                }

                // Swap the front and back bitmaps
                Bitmap tempBitmap = backBitmap;
                backBitmap = frontBitmap;
                frontBitmap = tempBitmap;

                // Display the new front bitmap (the one we just drew on)
                if (!this.isFormClosed)
                {
                    formHolding.BeginInvoke(new Action(() => { holder.Image = (Bitmap)frontBitmap; }));
                }
            }
        }
    }
}