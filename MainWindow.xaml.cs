using System.Windows;
using System.Windows.Threading;
using TempoEngine.Engine;

namespace TempoEngine {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{

        private readonly DispatcherTimer updateTimer;
        private bool _objectsChanged = false;
        private readonly int _windowRefreshRate = 60;

        public MainWindow(){
            InitializeComponent();

            // init engine(proccessor of the app with entity of main window)
            Engine.Engine.Init(this);

            _windowRefreshRate = Util.SystemInfo.GetRefreshRate();
            updateTimer = new DispatcherTimer();
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = TimeSpan.FromSeconds(1.0 / _windowRefreshRate);
            ObjectsChanged();
            updateTimer.Start();

            _engineObjectsList.OnSelectedObjectChanged = SelectedObjectChanged;
            _controlPanel.DeleteSelected = SelectedObjectChanged;
            _controlPanel.UpdateUI = UpdateAfterRead;
            _engineObjectsList.OnZoomToObject = ZoomToObject;
        }

        private void ZoomToObject(EngineObject obj) {
            _engineCanva.ZoomToObject(obj);
        }

        private void SelectedObjectChanged(EngineObject obj) {
            // Implement the logic to handle the selection change
            _engineCanva.Update();
            _engineTabProperties.SetObject(obj);
            _engineTabProperties.Update();

            if(obj == null) {  _engineObjectsList.Update(Engine.Engine.GetObjects()); return; }

            obj.PropertyChanged += (sender, args) => {
                _engineObjectsList.Update(Engine.Engine.GetObjects());
                _engineTabProperties.Update();
                _engineCanva.Update();
            };
        }

        private void UpdateAfterRead() {
            _engineObjectsList.Update(Engine.Engine.GetObjects());
            _engineTabProperties.Update();
            _engineCanva.Update();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e) {
            Update();
        }

        private void ObjectsChanged() {
            _objectsChanged = true;
        }

        public void Update() {
            if (!Engine.Engine.IsRunning() && !_objectsChanged) return;
            if(_objectsChanged) _objectsChanged = false;

            _engineObjectsList.Update(Engine.Engine.GetObjects());

        }

    }
}