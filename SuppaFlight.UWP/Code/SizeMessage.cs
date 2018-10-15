namespace SuppaFlight.UWP.Code
{
    public class SizeMessage
    {
        public SizeMessage(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        public uint Width { get; }
        public uint Height { get; }
    }
}