using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace moeckly_hw2_JuliaSets
{
    /// <summary>
    /// Isaac Moeckly
    /// CS 480 - Assignment 2
    /// Generate Julia Sets depending on user input in WPF-C#
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void gen_Click(object sender, RoutedEventArgs e)
        {
            // Initialize values
            double c1, c2 = 0.0;
            bool wrong = false;

            // Check if c1 input is double
            if (!double.TryParse(c1txt.Text, out double vc1)) { wrong = true; }

            // Check if c2 input is double
            if (!double.TryParse(c2txt.Text, out double vc2)) { wrong = true; }

            // If any inputs are wrong, dont generate
            if (wrong)
            {
                inputC.Content = "Incorrect Input.";
                inputC.Foreground = System.Windows.Media.Brushes.Red;
                inputC.FontWeight = FontWeights.Bold;
                return;
            }

            // Set values equal to inputted values accordingly
            c1 = vc1;
            c2 = vc2;
            inputC.Content = "Successfully Generated!";
            inputC.Foreground = System.Windows.Media.Brushes.Green;
            inputC.FontWeight = FontWeights.Bold;
            currentc1.Content = c1txt.Text;
            currentc2.Content = c2txt.Text;

            // xn+1 = xn^2 + yn^2 + c1;
            // yn+1 = 2xn*yn + c2;

            // Call Generate() function
            Generate(c1, c2);
        }

        private void Generate(double c1, double c2)
        {
            // Defining bitmap
            int height = Convert.ToInt32(image1.Height);
            int width = Convert.ToInt32(image1.Width);

            Bitmap drawingSurface = new Bitmap(width, height);

            // two for loops to iterate through entire bitmap
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int color = 0;
                    // 0 = blue
                    // 1 = red
                    // 2 = orange
                    // 3 = yellow
                    // 4 = green

                    // Transforming into user values from pixel values, pulled from for loop iteratives
                    double xit = 4.0 * ((Convert.ToDouble(x) - 0) / (Convert.ToDouble(width) - 0.0)) - 2.0;
                    double yit = 4.0 * ((Convert.ToDouble(y) - Convert.ToDouble(height)) / (0.0 - Convert.ToDouble(height))) - 2.0;
                    double xold, yold;

                    // 100 iterations to check for divergence
                    for (int i = 0; i < 100; i++)
                    {
                        // Set xold and yold, used for setting xn+1 and yn+1
                        xold = xit;
                        yold = yit;

                        // If values diverge, check for amount of divergence if 'iterative coloring' is checked,
                        // then break out of iterative loop
                        if (((xold * xold) + (yold * yold)) > 30)
                        {
                            if (itercolor.IsChecked == true)
                            {
                                if (i > 75) { color = 4; }
                                else if (i > 50) { color = 3; }
                                else if (i > 25) { color = 2; }
                                else if (i >= 0) { color = 1; }
                            }
                            else { color = 1; }

                            break;
                        }

                        // Set xn+1 and yn+1
                        xit = xold * xold - yold * yold + c1;
                        yit = 2 * xold * yold + c2;
                    }

                    // Reverse coloring if inverse coloring is checked
                    if (inv.IsChecked == true)
                    {
                        if(color == 0) { color = 1; }
                        else if(color == 1) { color = 0; }
                        else if(color == 2) { color = 4; }
                        else if(color == 4) { color = 2; }
                    }

                    // Draw pixels based on color
                    if (blkwht.IsChecked == true)
                    {
                        if (color == 0) { drawingSurface.SetPixel(x, y, Color.White); }
                        if (color == 1) { drawingSurface.SetPixel(x, y, Color.Black); }
                        if (color == 2) { drawingSurface.SetPixel(x, y, Color.DimGray); }
                        if (color == 3) { drawingSurface.SetPixel(x, y, Color.DarkGray); }
                        if (color == 4) { drawingSurface.SetPixel(x, y, Color.LightGray); }
                    }
                    else
                    {
                        if (color == 0) { drawingSurface.SetPixel(x, y, Color.Blue); }
                        if (color == 1) { drawingSurface.SetPixel(x, y, Color.Red); }
                        if (color == 2) { drawingSurface.SetPixel(x, y, Color.Orange); }
                        if (color == 3) { drawingSurface.SetPixel(x, y, Color.Yellow); }
                        if (color == 4) { drawingSurface.SetPixel(x, y, Color.Green); }
                    }
                }
            }

            // Save bitmap to memoryStream and set imageSource to memoryStream
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();

            MemoryStream stream = new MemoryStream();
            drawingSurface.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;

            bitmap.EndInit();

            // Set window's image source to generated bitmap png
            image1.Source = bitmap;

            return;
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
