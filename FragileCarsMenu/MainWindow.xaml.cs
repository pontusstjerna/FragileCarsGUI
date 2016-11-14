using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FragileCarsMenu
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {

        private string trackName = "2";
        private int laps = 5;
        private int countdown = 3000;
        private string botClass = "model.carcontrollers.TapeBot";
        private int resX = 1680;
        private int resY = 1050;
        private GroupBox[] cars;
        private CheckBox[] carCheckboxes;


        public MainMenuWindow()
        {
            InitializeComponent();
            fullscreen.IsChecked = true;
            smokeEnabled.IsChecked = true;
            skidmarksEnabled.IsChecked = true;
           
            friction.Value = 30;
            frictionLabel.Content = "Friction: " + 30;

            lapLabel.Content += laps.ToString();
            lapScroll.Value = 1;

            greenActive.IsChecked = true;
            greenPlayer.IsChecked = true;

            cars = new GroupBox[] {greencarBox, yellowcarBox, redcarBox, bluecarBox};
            carCheckboxes = new CheckBox[] { greenActive, yellowActive, redActive, blueActive };

            setCarBoxes();
        }

        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            WriteToFile();

            string filePath = Directory.GetCurrentDirectory();
            Process proc = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "-jar " + filePath + "\\fragilecars.jar";
            startInfo.FileName = "java";
            proc.StartInfo = startInfo;
            proc.Start();

            
        }

        private void WriteToFile()
        {
            System.IO.File.WriteAllLines(Directory.GetCurrentDirectory() + "\\config.txt", CreateLines());
        }

        private string[] CreateLines()
        {
            string[] lines = new string[14];

            lines[0] = "#trackName=\"" + trackName + "\"";
            lines[1] = "#nCars=\"" + getnCars() + "\"";
            lines[2] = "#nPlayers=\"" + getnPlayers() + "\"";
            lines[3] = "#laps=\"" + laps + "\"";
            lines[4] = "#countdown=\"" + countdown + "\"";
            lines[5] = "#friction=\"" + (int)friction.Value + "\"";
            lines[6] = "#betterGraphics=\"true\"";
            lines[7] = "#smokeEnabled=\"" + smokeEnabled.IsChecked + "\"";
            lines[8] = "#skidmarksEnabled=\"" + skidmarksEnabled.IsChecked + "\"";
            lines[9] = "#botClass=\"" + botClass + "\"";
            lines[10] = "#debugEnabled=\"" + debugEnabled.IsChecked + "\"";
            lines[11] = "#fullscreenEnabled=\"" + fullscreen.IsChecked + "\"";
            lines[12] = "#resX=\"" + resX + "\"";
            lines[13] = "#resY=\"" + resY + "\"";
           

            return lines;
        }

        private int getnCars()
        {
            int n = 0;
            foreach(CheckBox b in carCheckboxes){
                if(b.IsChecked == true)
                {
                    n++;
                }
            }
            return n;
        }

        private int getnPlayers()
        {
            int n = 0;
            foreach (CheckBox b in new CheckBox[] { bluePlayer, redPlayer, yellowPlayer, greenPlayer })
            {
                if (b.IsChecked == true)
                {
                    n++;
                }
            }
            return n;
        }

        private void setCarBoxes()
        {
            for(int i = 0; i < carCheckboxes.Length && cars[i].IsEnabled == true; i++) //Go through all the group boxes
            {
                if(carCheckboxes[i].IsChecked != true) //If the current car is inactive
                {
                    for(int j = i+1; j < cars.Length; j++) //Disable the following
                    {
                        cars[j].IsEnabled = false;
                    }
                }
                else //Else enable the next one
                {
                    if(i != carCheckboxes.Length - 1) //Unless it is the last box
                    {
                        cars[i + 1].IsEnabled = true;
                    }
                }
            }
        }

        private void friction_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            frictionLabel.Content = "Friction: " + (int)friction.Value;
        }

        private void carActive_Clicked(object sender, RoutedEventArgs e)
        {
            setCarBoxes();
        }

        private void lapScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            const int stdValue = 1;

            if (lapScroll.Value < stdValue) laps++;
            else if(lapScroll.Value > stdValue && laps > 1) laps--;

            lapScroll.Value = stdValue;

            lapLabel.Content = "Laps: " + laps;
        }
    }
}
