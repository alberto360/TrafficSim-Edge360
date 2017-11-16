namespace TrafficSim
{
    public class TrafficLight : ASimBase
    {
        public enum ETrafficLightStatus
        {
            Green,
            Yellow,
            Red,
            Yield
        }

        private float _lastUpdateTime;

        private ETrafficLightStatus _status;

        public TrafficLight(Intersection intersection, TrafficLight partner)
        {
            Intersection = intersection;
            Partner = partner;

            Status = ETrafficLightStatus.Red;

            GreenDuration = Util.GetRandomNumber(10, 30);
            YellowDuration = Util.GetRandomNumber(10, 15);

            if (Partner != null)
            {
                Partner.Partner = this;
            }
        }

        public float GreenDuration { get; set; }
        public Intersection Intersection { get; set; }

        public TrafficLight Partner { get; set; }

        public Road Road { get; set; }
        public Line Segment { get; set; }

        public ETrafficLightStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                _lastUpdateTime = 0;
            }
        }

        public float YellowDuration { get; set; }

        public override void Initialize()
        {
        }

        public override void Update(float delta)
        {
            _lastUpdateTime += delta;

            switch (Status)
            {
                case ETrafficLightStatus.Green:
                    if (_lastUpdateTime > GreenDuration)
                    {
                        Status = ETrafficLightStatus.Yellow;
                    }
                    break;
                case ETrafficLightStatus.Yellow:
                    if (_lastUpdateTime > YellowDuration)
                    {
                        Status = ETrafficLightStatus.Red;
                    }
                    break;
            }
        }
    }
}