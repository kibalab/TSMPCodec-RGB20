Shader "Hidden/TSMP/Decode RGB20 Bytes"
{
    Properties
    {
        _MainTex ("TSMP Source", 2D) = "black" {}
        _BlockSize ("Block Size", Float) = 8
        _SampleSize ("Sample Size", Float) = 0
        _StartBlock ("Start Block", Float) = 0
        _ByteCount ("Byte Count", Float) = 0
        _ActiveWidthBlocks ("Active Width Blocks", Float) = 80
        _SourceWidth ("Source Width", Float) = 640
        _SourceHeight ("Source Height", Float) = 360
        _OutputWidth ("Output Width", Float) = 14
        _OutputHeight ("Output Height", Float) = 1
        _FlipY ("Flip Y", Float) = 1
        _Rgb20CalibrationStartBlock ("RGB20 Calibration Start Block", Float) = 640
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Overlay" }

        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma target 3.5
            #pragma vertex vert
            #pragma fragment frag
            #include "../../../com.kibalab.tsmp.core/Runtime/Codecs/Common/Shaders/cgincs/TSMPDecodeCommon.cginc"

            float _Rgb20CalibrationStartBlock;

            float3 CalibrationLowRgb()
            {
                float3 r = SampleBlockByIndex(_Rgb20CalibrationStartBlock);
                float3 g = SampleBlockByIndex(_Rgb20CalibrationStartBlock + 128.0);
                float3 b = SampleBlockByIndex(_Rgb20CalibrationStartBlock + 256.0);
                return float3(r.r, g.g, b.b);
            }

            float3 CalibrationHighRgb()
            {
                float3 r = SampleBlockByIndex(_Rgb20CalibrationStartBlock + 127.0);
                float3 g = SampleBlockByIndex(_Rgb20CalibrationStartBlock + 255.0);
                float3 b = SampleBlockByIndex(_Rgb20CalibrationStartBlock + 319.0);
                return float3(r.r, g.g, b.b);
            }

            int ClassifyChannel(float value, int count, int offset, int channel)
            {
                int bestIndex = 0;
                float bestDistance = 999.0;

                [loop]
                for (int i = 0; i < 128; i++)
                {
                    if (i < count)
                    {
                        float3 c = SampleBlockByIndex(_Rgb20CalibrationStartBlock + offset + i);
                        float candidate = channel == 0 ? c.r : channel == 1 ? c.g : c.b;
                        float d = abs(value - candidate);
                        if (d < bestDistance)
                        {
                            bestDistance = d;
                            bestIndex = i;
                        }
                    }
                }

                return bestIndex;
            }

            int DecodeRgb20Symbol(int symbolIndex)
            {
                float blockIndex = PayloadBlockIndex(symbolIndex);
                float3 rgb = SampleBlockByIndex(blockIndex);
                int r = ClassifyChannel(rgb.r, 128, 0, 0);
                int g = ClassifyChannel(rgb.g, 128, 128, 1);
                int b = ClassifyChannel(rgb.b, 64, 256, 2);
                return r | (g << 7) | (b << 14);
            }

            int DecodeByte(int byteIndex)
            {
                if (byteIndex < 0 || byteIndex >= (int)_ByteCount)
                    return 0;

                int bitIndex = byteIndex * 8;
                int symbolIndex = FloorDivNonNegative(bitIndex, 20.0);
                int bitShift = bitIndex - symbolIndex * 20;
                int symbol = DecodeRgb20Symbol(symbolIndex);
                int value = (symbol >> bitShift) & 0xFF;

                if (bitShift > 12)
                {
                    int nextSymbol = DecodeRgb20Symbol(symbolIndex + 1);
                    int remaining = 20 - bitShift;
                    int mask = (1 << remaining) - 1;
                    int low = (symbol >> bitShift) & mask;
                    int high = nextSymbol & ((1 << (8 - remaining)) - 1);
                    value = low | (high << remaining);
                }

                return value;
            }

            #include "../../../com.kibalab.tsmp.core/Runtime/Codecs/Common/Shaders/cgincs/TSMPDecodeByteOutput.cginc"
            ENDCG
        }
    }

    Fallback Off
}
