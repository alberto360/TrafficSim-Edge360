using System;
using System.Security.Cryptography;
using System.Text;

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

            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(intersection.Position.X + "_" + intersection.Position.Y));//ewww nasty nasty, but consistant
                Id = new Guid(hash);
            }


            Intersection = intersection;
            Partner = partner;

            Status = ETrafficLightStatus.Red;

            GreenDuration = 15;//Util.GetRandomNumber(10, 30); set to a default value, will be overwritten by ML
            YellowDuration = 12;//Util.GetRandomNumber(10, 15); initially set the warning light to a set value

            if (Partner != null)
            {
                Partner.Partner = this;
            }
        }
        public Guid Id { get; private set; }
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

        public void Initialize()
        {
        }

        public void Update(float delta)
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