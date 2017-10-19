using System.Drawing;
using System.IO;
using CEDD_Descriptor;

namespace retrieveImageFeatures.UtilitiesClasses
{
    public class UtilityDescriptor
    {
        public double[] GetCeddDescriptorForImage(Stream file)
        {
            double[] ceddDiscriptor = null;
            CEDD cedd = new CEDD();
            using (Bitmap bmp = new Bitmap(file))
            {
                ceddDiscriptor = cedd.Apply(bmp);
            }
            return ceddDiscriptor;
        }

        public double[] GetCeddDescriptorForImage(System.Drawing.Image file)
        {
            double[] ceddDiscriptor = null;
            CEDD cedd = new CEDD();
            using (Bitmap bmp = new Bitmap(file))
            {
                ceddDiscriptor = cedd.Apply(bmp);
            }
            return ceddDiscriptor;
        }
    }
}