namespace retrieveImageFeatures.UtilitiesClasses.Database
{
    public class DtRecord
    {
        public static double[] ConvertFloatToDouble(float[] inputArray)
        {
            if (inputArray == null)
                return null;

            double[] output = new double[inputArray.Length];
            for (int i = 0; i < inputArray.Length; i++)
                output[i] = inputArray[i];

            return output;
        }

        public string name;
        public double[] descriptor;

    }
}