using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TempoEngine.Engine;
using TempoEngine.UIControls;

namespace TempoEngine{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{

        private DispatcherTimer updateTimer;
        private bool _objectsChanged = false;
        private int windowRefreshRate = 60;

        public MainWindow(){
            InitializeComponent();

            // init engine(proccessor of the app with entity of main window)
            Engine.Engine.Init(this);

            windowRefreshRate = Util.SystemInfo.GetRefreshRate();
            updateTimer = new DispatcherTimer();
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = TimeSpan.FromSeconds(1.0 / windowRefreshRate);
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e) {
            Update();
        }

        private void ObjectsChanged() {
            _objectsChanged = true;
        }

        public void Update() {
            if (!Engine.Engine.isRunning() || _objectsChanged) return;

            _engineObjectsList.Update(Engine.Engine.GetObjects());

        }
    }
}