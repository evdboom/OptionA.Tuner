namespace OptionA.Tuner.Decoder.FastFourierTransform
{
    public interface ITransformService
    {
        IList<FrequencyPoint> GetFrequencyDomain(IList<double> times, IList<double> amplitudes);
    }
}
