Shader "Hidden/TSMP/Decode RGB20 Bytes"
{
    Properties
    {
        [HideInInspector] _TSMPHeaderTex ("Decoded Header", 2D) = "black" {}
        [HideInInspector] _TSMPHeaderPixels ("Header Pixels", Float) = 0
        [HideInInspector] _CalibrationLut ("Calibration LUT", 2D) = "black" {}
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
            #pragma multi_compile_local _ TSMP_CALIBRATION_LUT
            #include "Packages/com.kibalab.tsmp.core/Runtime/Codecs/Common/Shaders/cgincs/TSMPDecodeCommon.cginc"

#if defined(TSMP_CALIBRATION_LUT)
            Texture2D<float4> _CalibrationLut;
#endif

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
#if defined(TSMP_CALIBRATION_LUT)
                        float3 c = _CalibrationLut.Load(int3(offset + i, 0, 0)).rgb;
#else
                        float3 c = SampleBlockByIndex(_Rgb20CalibrationStartBlock + offset + i);
#endif
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

            float4 frag(v2f i) : SV_Target
            {
#if defined(TSMP_COMBINED_BYTE_OUTPUT)
                int baseByte;
                float4 prefix;
                if (TSMPReadOutputPrefix(i.uv, baseByte, prefix))
                    return prefix;
#else
                float2 pixel = floor(i.uv * float2(_OutputWidth, _OutputHeight));
                pixel = clamp(pixel, 0.0, float2(_OutputWidth - 1.0, _OutputHeight - 1.0));
                int baseByte = ((int)pixel.y * (int)_OutputWidth + (int)pixel.x) * 4;
#endif
                if (baseByte >= (int)_ByteCount)
                    return 0.0;

                int totalBits = 20;
                int bitIndex = baseByte * 8;
                int symbolIndex = FloorDivNonNegative(bitIndex, (float)totalBits);
                int bitShift = bitIndex - symbolIndex * totalBits;
                int requiredBits = min(4, (int)_ByteCount - baseByte) * 8;
                uint packed = (uint)DecodeRgb20Symbol(symbolIndex) >> bitShift;
                int nextBit = totalBits - bitShift;
                if (nextBit < requiredBits)
                {
                    packed |= (uint)DecodeRgb20Symbol(symbolIndex + 1) << nextBit;
                    nextBit += totalBits;
                }
                if (nextBit < requiredBits)
                {
                    packed |= (uint)DecodeRgb20Symbol(symbolIndex + 2) << nextBit;
                    nextBit += totalBits;
                }

                if (requiredBits < 32)
                    packed &= (1u << requiredBits) - 1u;

                return float4(packed & 0xFFu, (packed >> 8) & 0xFFu,
                    (packed >> 16) & 0xFFu, (packed >> 24) & 0xFFu) / 255.0;
            }
            ENDCG
        }
    }

    Fallback Off
}
