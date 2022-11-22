namespace OptionA.Tuner.Decoder.FastFourierTransform
{
    public class TransformService : ITransformService
    {
        public IList<FrequencyPoint> GetFrequencyDomain(IList<double> times, IList<double> amplitudes)
        {
            var samples = amplitudes
                .Select(a => new ComplexNumber(a, 0))
                .ToList();

            var count = samples.Count;

            while (!IsPowerOfTwo(count)) 
            {
                samples.Add(ComplexNumber.Zero);
                count++;
            }

            var transformed = PerformFastFourierTransform(samples);

            var output = transformed
                .Where((t, i) => i >= transformed.Count / 2)
                .Concat(transformed
                    .Where((t, i) => i < transformed.Count / 2))
                .ToList();

            var time = times.Last() - times.First();
            var resolution = amplitudes.Count / (time * output.Count);

            var frequencies = output
                .Select((_, i) => (i - (output.Count / 2)) * resolution)
                .ToList();

            return frequencies
                .Select((f, i) =>
                {
                    var t = output[i];
                    return new FrequencyPoint(f, t.GetMagnitude(), t.GetPhase(), t);
                })
                .ToList();
        }

        private static List<ComplexNumber> PerformFastFourierTransform(List<ComplexNumber> samples)
        {
            int N = samples.Count;
            if (N == 1)
            {
                return samples.ToList();
            }
            var evenList = PerformFastFourierTransform(samples
                .Where((s, i) => i % 2 == 0)
                .ToList());
            var oddList = PerformFastFourierTransform(samples
                .Where((s, i) => i % 2 != 0)
                .ToList());
            var pairedList = GetPairedList(evenList, oddList);

            var result = new ComplexNumber[N];
            foreach (var (even, odd, index) in pairedList)
            {
                double w = (-2.0 * index * Math.PI) / N;
                var wk = new ComplexNumber(Math.Cos(w), Math.Sin(w));

                result[index] = even + (wk * odd);
                result[index + (N / 2)] = even - (wk * odd);
            }

            return result
                .ToList();
        }

        private static IEnumerable<(ComplexNumber Even, ComplexNumber Odd, int Index)> GetPairedList(List<ComplexNumber> even, List<ComplexNumber> odd)
        {
            return even
                .Select((e, i) => (e, odd[i], i));
        }

        private static bool IsPowerOfTwo(int n)
        {
            return (n != 0) 
                && (n & (n - 1)) == 0;
        }
    }
}
