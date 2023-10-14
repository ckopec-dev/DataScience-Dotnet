
namespace Core
{
    public static class PhysicsHelper
    {
        public static double Decay(double initialQuantity, double halfLife, double elapsedTime)
        {
            return initialQuantity * Math.Pow(0.5d, (elapsedTime / halfLife));
        }
    }
}
