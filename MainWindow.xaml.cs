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
            log4net.Config.XmlConfigurator.Configure();

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
            _controlPanel.UpdateUI = UpdateAll;
            _engineObjectsList.OnDeleteObject = UpdateAllAfterChangeProperties;
            _engineObjectsList.OnZoomToObject = ZoomToObject;
            _controlPanel.EngineModeChanged = EngineModeChanged;
        }

        private void ZoomToObject(EngineObject obj) {
            _engineCanva.ZoomToObject(obj);
        }

        private void EngineModeChanged() {
            if(Engine.Engine.Mode != Engine.Engine.EngineMode.Stopped) {
                _engineObjectsList.Enable(false);
                _engineTabProperties.Enable(false);
            } else {
                _engineObjectsList.Enable(true);
                _engineTabProperties.Enable(true);
            }
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

        private void UpdateAllAfterChangeProperties() {
            _engineTabProperties.SetObject(null);
            UpdateAll();
        }

        private void UpdateAll() {
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

            if(Engine.Engine.Mode != Engine.Engine.EngineMode.Running)
                _engineObjectsList.Update(Engine.Engine.GetObjects());
            _controlPanel.Update();
            _engineCanva.Update();
            _engineTabProperties.Update();

        }

    }
}