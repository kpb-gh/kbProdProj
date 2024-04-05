using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace kbProdProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer dTimer = new DispatcherTimer();
        private List<Vehicle> vehicles = new List<Vehicle>();
        private List<Node> nodes = MapLoader.GetNodesFromFile();
        private List<Driver> drivers = new List<Driver>();
        private Key keyDown = Key.None;
        
        public MainWindow()
        {
            InitializeComponent();
            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            AddEntities();
            dTimer.Start();
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            List<Driver> doneDrivers = new List<Driver>();
            foreach (Driver d in drivers)
            {
                if (d.DriveAI())
                {
                    doneDrivers.Add(d);
                }
            }
            foreach (Vehicle v in vehicles)
            {
                v.Update();
            }
            foreach (Driver item in doneDrivers)
            {
                drivers.Remove(item);
            }
        }

        private void AddEntities()
        {
            // region 1 - add nodes
            foreach (var obj in nodes)
            {
                CarGrid.Children.Add(obj.self);
            }
            // region 2 - add vehicles
            Vehicle v = new Vehicle(new int[] { (int)nodes[0].self.Margin.Left, (int)nodes[0].self.Margin.Top }, new SolidColorBrush(Colors.Black), 0);
            v.Angle = (int)DriverMath.Angle_ToNode(nodes[1],v); // spawn facing a valid route
            v.CurrentLocation = nodes[0];
            vehicles.Add(v);
            CarGrid.Children.Add(v.self);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            keyDown = e.Key;
            if (vehicles[0].ovrrd)
            {
                if (keyDown == Key.Up) { vehicles[0].Accel(); }
                else if (keyDown == Key.Down) { vehicles[0].Brake(); }
                else if (keyDown == Key.Left) { vehicles[0].TurnLeft(); }
                else if (keyDown == Key.Right) { vehicles[0].TurnRight(); }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            keyDown = Key.None;
            if (vehicles[0].ovrrd) { vehicles[0].Neutral(); }
        }

        private void tNodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tNodeBox.Text == "")
            {
                tNodeLab.Visibility = Visibility.Visible;
            } else
            {
                tNodeLab.Visibility = Visibility.Hidden;
            }
        }

        private void tVehicleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tVehicleBox.Text == "")
            {
                tVehicleLab.Visibility = Visibility.Visible;
            } else
            {
                tVehicleLab.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errLab.Visibility = Visibility.Hidden;
            bool flag = false;
            int v, n = 0;
            if (!(int.TryParse(tVehicleBox.Text, out v) && int.TryParse(tNodeBox.Text, out n)))
            {
                errLab.Visibility = Visibility.Visible;
                errLab.Content = "Ensure both boxes only have numerical IDs";
                return;
            }
            foreach (var i in nodes)
            {
                if (i.id == n) { flag = true; break; }
            }
            if (!flag)
            {
                errLab.Visibility = Visibility.Visible;
                errLab.Content = "Node ID does not correspond to real node";
                return;
            }
            else if (vehicles.Count <= v)
            {
                errLab.Visibility = Visibility.Visible;
                errLab.Content = "Vehicle ID does not correspond to real vehicle";
                return;
            }
            else
            {
                Driver d = new Driver(vehicles[v], vehicles[v].CurrentLocation, nodes.First(a => a.id == n), nodes);
                drivers.Add(d);
            }
        }

        private void btn_killAllDrivers_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in drivers)
            {
                i.DieSafely();
            }
            drivers = new List<Driver>();
        }
    }
}
