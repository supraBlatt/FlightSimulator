using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using FlightSimulator.ViewModels;

namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for MazeBoard.xaml
    /// </summary>
    public partial class FlightBoard : UserControl
    {
        ObservableDataSource<Point> planeLocations = null;
        FlightBoardViewModel vm;
        public FlightBoard()
        {
            InitializeComponent();
            vm = new FlightBoardViewModel();
            vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            planeLocations = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            planeLocations.SetXYMapping(p => p);

            plotter.AddLineGraph(planeLocations, 2, "Route");
        }

        /*adds a new point that needs to be added to the graph*/
        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //only run if a new point needs to be added
            if (e.PropertyName.Equals("new point"))
            {
                FlightBoardViewModel data = sender as FlightBoardViewModel;
                //get the point
                Point toAdd = new Point(data.Lat, data.Lon);
                //add the point
                planeLocations.AppendAsync(Dispatcher, toAdd);
                //System.Diagnostics.Debug.WriteLine("new point added!");
            }
        }

    }

}