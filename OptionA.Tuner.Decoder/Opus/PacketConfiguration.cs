using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionA.Tuner.Decoder.Opus
{
    public record PacketConfiguration
    {
        public int ConfigurationNumber { get; init; }
        public DecoderMode Mode { get; init; }
        public Bandwidth Bandwidth { get; init; }
        public decimal FrameSize { get; init; }

        private PacketConfiguration(int configurationNumber, DecoderMode mode, Bandwidth bandwidth, decimal frameSize)
        {
            ConfigurationNumber = configurationNumber;
            Mode = mode;
            Bandwidth = bandwidth;
            FrameSize = frameSize;
        }

        public static readonly Dictionary<int, PacketConfiguration> Configurations = new()
        {
            { 0, new PacketConfiguration(0, DecoderMode.Silk, Bandwidth.Bandwidths["NB"], 10) },
            { 1, new PacketConfiguration(1, DecoderMode.Silk, Bandwidth.Bandwidths["NB"], 20) },
            { 2, new PacketConfiguration(2, DecoderMode.Silk, Bandwidth.Bandwidths["NB"], 40) },
            { 3, new PacketConfiguration(3, DecoderMode.Silk, Bandwidth.Bandwidths["NB"], 60) },
            { 4, new PacketConfiguration(4, DecoderMode.Silk, Bandwidth.Bandwidths["MB"], 10) },
            { 5, new PacketConfiguration(5, DecoderMode.Silk, Bandwidth.Bandwidths["MB"], 20) },
            { 6, new PacketConfiguration(6, DecoderMode.Silk, Bandwidth.Bandwidths["MB"], 40) },
            { 7, new PacketConfiguration(7, DecoderMode.Silk, Bandwidth.Bandwidths["MB"], 60) },
            { 8, new PacketConfiguration(8, DecoderMode.Silk, Bandwidth.Bandwidths["WB"], 10) },
            { 9, new PacketConfiguration(9, DecoderMode.Silk, Bandwidth.Bandwidths["WB"], 20) },
            { 10, new PacketConfiguration(10, DecoderMode.Silk, Bandwidth.Bandwidths["WB"], 40) },
            { 11, new PacketConfiguration(11, DecoderMode.Silk, Bandwidth.Bandwidths["WB"], 60) },
            { 12, new PacketConfiguration(12, DecoderMode.Hybrid, Bandwidth.Bandwidths["SWB"], 10) },
            { 13, new PacketConfiguration(13, DecoderMode.Hybrid, Bandwidth.Bandwidths["SWB"], 20) },
            { 14, new PacketConfiguration(14, DecoderMode.Hybrid, Bandwidth.Bandwidths["FB"], 10) },
            { 15, new PacketConfiguration(15, DecoderMode.Hybrid, Bandwidth.Bandwidths["FB"], 20) },
            { 16, new PacketConfiguration(16, DecoderMode.Celt, Bandwidth.Bandwidths["NB"], 2.5M) },
            { 17, new PacketConfiguration(17, DecoderMode.Celt, Bandwidth.Bandwidths["NB"], 5) },
            { 18, new PacketConfiguration(18, DecoderMode.Celt, Bandwidth.Bandwidths["NB"], 10) },
            { 19, new PacketConfiguration(19, DecoderMode.Celt, Bandwidth.Bandwidths["NB"], 20) },
            { 20, new PacketConfiguration(20, DecoderMode.Celt, Bandwidth.Bandwidths["WB"], 2.5M) },
            { 21, new PacketConfiguration(21, DecoderMode.Celt, Bandwidth.Bandwidths["WB"], 5) },
            { 22, new PacketConfiguration(22, DecoderMode.Celt, Bandwidth.Bandwidths["WB"], 10) },
            { 23, new PacketConfiguration(23, DecoderMode.Celt, Bandwidth.Bandwidths["WB"], 20) },
            { 24, new PacketConfiguration(24, DecoderMode.Celt, Bandwidth.Bandwidths["SWB"], 2.5M) },
            { 25, new PacketConfiguration(25, DecoderMode.Celt, Bandwidth.Bandwidths["SWB"], 5) },
            { 26, new PacketConfiguration(26, DecoderMode.Celt, Bandwidth.Bandwidths["SWB"], 10) },
            { 27, new PacketConfiguration(27, DecoderMode.Celt, Bandwidth.Bandwidths["SWB"], 20) },
            { 28, new PacketConfiguration(28, DecoderMode.Celt, Bandwidth.Bandwidths["FB"], 2.5M) },
            { 29, new PacketConfiguration(29, DecoderMode.Celt, Bandwidth.Bandwidths["FB"], 5) },
            { 30, new PacketConfiguration(30, DecoderMode.Celt, Bandwidth.Bandwidths["FB"], 10) },
            { 31, new PacketConfiguration(31, DecoderMode.Celt, Bandwidth.Bandwidths["FB"], 20) },
        };

    }
}
