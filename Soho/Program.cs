namespace Soho
{
    internal class Program
    {
        static void Main()
        {
            // See https://sungrazer.nrl.navy.mil/tutorial

            // General algo idea rev 1...

            // Download sequential images.
            // Load as bitmaps so individual pixels can be calculated. (how many? 3-5?)
            // Divide image into square blocks. (how small?). ignore certain regions that have static values which will create false positives (e.g. the center and corners? not sure if this is necessary)
            // Compare "whiteness" of neighboring blocks. Is it significantly higher? (how significant?)
            // Does a similar block of whiteness appear in the subsequent image, but in a different location? (the match has to be somewhat loose to account for noise)
            // Render images with detection results to help visualize/analyze algo output?
        }
    }
}
