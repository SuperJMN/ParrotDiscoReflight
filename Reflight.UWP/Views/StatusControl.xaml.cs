namespace ParrotDiscoReflight.Views
{
    public sealed partial class StatusControl
    {
        //private MapIcon spaceNeedleIcon;


        public StatusControl()
        {
            InitializeComponent();

            //MessageBus.Current.Listen<BasicGeoposition>("First")
            //    .Do(TrySetLocation)
            //    .Do(_ => MapControl.ZoomLevel = 20)
            //    .Subscribe(p => MapControl.Center = new Geopoint(p));

            //MessageBus.Current.Listen<BasicGeoposition>().Subscribe(TrySetLocation);

            //MapControl.MapServiceToken = "2ywvE5Sf0Oc0N1S53Jj2~fbOf4w-CQmaDPXG11-Lx-A~Au6RnaxhBKpKQc79NkfwbrdFrrSr1NHNolZKDcpVFKk1eIWqYubIC3eTZsezamPm";

            //AddPin();
        }

        //private void TrySetLocation(BasicGeoposition x)
        //{
        //    try
        //    {
        //        spaceNeedleIcon.Location = new Geopoint(x);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

        //private void AddPin()
        //{
        //    var myLandmarks = new List<MapElement>();

        //    spaceNeedleIcon = new MapIcon
        //    {
        //        NormalizedAnchorPoint = new Point(0.5, 1.0),
        //        ZIndex = 0,
        //    };

        //    myLandmarks.Add(spaceNeedleIcon);

        //    var landmarksLayer = new MapElementsLayer
        //    {
        //        ZIndex = 1,
        //        MapElements = myLandmarks
        //    };

        //    MapControl.Layers.Add(landmarksLayer);
        //}
    }
}
