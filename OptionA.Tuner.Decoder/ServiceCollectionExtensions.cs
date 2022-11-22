using Microsoft.Extensions.DependencyInjection;
using OptionA.Tuner.Decoder.EBML;
using OptionA.Tuner.Decoder.FastFourierTransform;
using OptionA.Tuner.Decoder.Opus;
using OptionA.Tuner.Decoder.WebM;

namespace OptionA.Tuner.Decoder
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpusDecoder(this IServiceCollection services)
        {
            services
                .AddSingleton<ITransformService, TransformService>()
                .AddSingleton<IOpusDecoder, OpusDecoder>()
                .AddSingleton<IEbmlParser, EbmlParser>()
                .AddSingleton<IWebMDecoder, WebMDecoder>();

            return services;
        }
    }
}
