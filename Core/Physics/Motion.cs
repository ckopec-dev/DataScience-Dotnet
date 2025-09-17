namespace Core.Physics
{
    /// <summary>
    /// Usual metrics:
    /// Distance = meters
    /// Time = seconds
    /// Acceleration is meters/second/second.
    /// Speed is how fast. Velocity is how fast in a particular direction.
    /// Speed is a scalar. Velocity is a vector.
    /// </summary>
    public class Motion
    {
        #region Public methods

        public static decimal Speed(decimal Distance, decimal Time)
        {
            return Distance / Time;
        }

        public static decimal Distance(decimal Speed, decimal Time)
        {
            return Speed * Time;
        }

        public static decimal Time(decimal Speed, decimal Distance)
        {
            return Distance / Speed;
        }

        public static decimal Acceleration(decimal SpeedFinish, decimal SpeedStart, decimal Time)
        {
            // SpeedFinish is the speed after Time has elapsed.
            return (SpeedFinish - SpeedStart) / Time;
        }

        public static decimal SpeedOfGravity(decimal Seconds)
        {
            // Returns the speed of an object (in meters/second) dropped in a vacuum from an arbitrarily high height (but 
            // near Earth's surface) after Time has elapsed.
            return 9.8m * Seconds;
        }

        public static decimal DistanceOfGravity(decimal SpeedStart, decimal Time)
        {
            // Returns the distance an object has fallen after Time has elapsed.
            return SpeedStart * Time + 0.5m * 9.8m * Time * Time;
        }

        #endregion
    }
}
