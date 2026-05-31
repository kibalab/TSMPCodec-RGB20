using UnityEngine;

namespace K13A.TSMP
{
    [AddComponentMenu("TSMP/Codecs/RGB20 Codec")]
    public sealed class TSMPCodecRGB20 : TSMPCodec
    {
        private const int SymbolModeRgb20 = 4;
        private const int BitsPerSymbol = 20;
        private const int RedSymbolCount = 128;
        private const int GreenSymbolCount = 128;
        private const int BlueSymbolCount = 64;
        private const int CalibrationSymbolCount = RedSymbolCount + GreenSymbolCount + BlueSymbolCount;

        public Material byteDecodeMaterial;
        public Material[] debugMaterials;

#if !COMPILER_UDONSHARP
        public override int SymbolMode => SymbolModeRgb20;
        public override int GetPayloadStartRow(int width, int blockSize)
        {
            int activeWidthBlocks = Mathf.Max(1, FrameCapacity.GetActiveWidthBlocks(width, blockSize));
            return Luma4Raster.PayloadStartRow + ((CalibrationSymbolCount + activeWidthBlocks - 1) / activeWidthBlocks);
        }

        public override int GetPayloadCapacityBytes(int width, int height, int blockSize)
        {
            int activeWidthBlocks = FrameCapacity.GetActiveWidthBlocks(width, blockSize);
            int activeHeightBlocks = FrameCapacity.GetActiveHeightBlocks(height, blockSize);
            int payloadRows = Mathf.Max(0, activeHeightBlocks - GetPayloadStartRow(width, blockSize) - Luma4Raster.ReservedEndRows);
            return activeWidthBlocks * payloadRows * BitsPerSymbol / 8;
        }

        public override int GetPayloadBlocksForBytes(int byteCount) => (byteCount * 8 + BitsPerSymbol - 1) / BitsPerSymbol;

        public override bool TryWriteFrame(Texture2D texture, int blockSize, byte[] headerBytes, byte[] payloadBytes, out string error)
        {
            if (!ValidateFrame(texture, blockSize, headerBytes, payloadBytes, out error))
                return false;

            Color32[] pixels = FrameRaster.CreateClearedPixels(texture.width, texture.height);
            Luma4Raster.WriteBaseRegions(pixels, texture.width, texture.height, blockSize, headerBytes);
            WriteCalibration(pixels, texture.width, texture.height, blockSize);
            WritePayload(pixels, texture.width, texture.height, blockSize, payloadBytes, payloadBytes.Length);
            FrameRaster.WriteEndMarker(pixels, texture.width, texture.height, blockSize);
            texture.SetPixels32(pixels);
            texture.Apply(false, false);
            return true;
        }

        public override byte[] GetCodecOptionBytes() => null;
        public override int DecodeMaterialCount => byteDecodeMaterial != null ? 1 : 0;
        public override int DebugMaterialCount => debugMaterials != null ? debugMaterials.Length : 0;
        public override Material GetDebugMaterial(int index) => debugMaterials != null && index >= 0 && index < debugMaterials.Length ? debugMaterials[index] : null;
        public override Material GetDecodeMaterial(int index) => index == 0 ? byteDecodeMaterial : null;

        public override void ConfigureMaterials(CodecMaterialContext context)
        {
            base.ConfigureMaterials(context);
            ConfigureRgb20Material(byteDecodeMaterial, context);
            ConfigureMaterialGroup(debugMaterials, context, false);
            if (debugMaterials != null)
            {
                for (int i = 0; i < debugMaterials.Length; i++)
                    ConfigureRgb20Material(debugMaterials[i], context);
            }
        }

        private static void ConfigureRgb20Material(Material material, CodecMaterialContext context)
        {
            if (material == null)
                return;

            SetFloatIfPresent(material, "_Rgb20CalibrationStartBlock", Luma4Raster.PayloadStartRow * context.FrameLayout.ActiveWidthBlocks);
        }

        private bool ValidateFrame(Texture2D texture, int blockSize, byte[] headerBytes, byte[] payloadBytes, out string error)
        {
            return ValidateRasterFrame(texture, blockSize, headerBytes, payloadBytes, out error);
        }

        private static void WriteCalibration(Color32[] pixels, int width, int height, int blockSize)
        {
            int activeWidthBlocks = FrameCapacity.GetActiveWidthBlocks(width, blockSize);
            int blockCursor = Luma4Raster.PayloadStartRow * activeWidthBlocks;

            for (int i = 0; i < RedSymbolCount; i++, blockCursor++)
                FrameRaster.WriteColorBlockAtIndex(pixels, width, height, blockSize, blockCursor, RgbRedCalibrationColor(i, 7));
            for (int i = 0; i < GreenSymbolCount; i++, blockCursor++)
                FrameRaster.WriteColorBlockAtIndex(pixels, width, height, blockSize, blockCursor, RgbGreenCalibrationColor(i, 7));
            for (int i = 0; i < BlueSymbolCount; i++, blockCursor++)
                FrameRaster.WriteColorBlockAtIndex(pixels, width, height, blockSize, blockCursor, RgbBlueCalibrationColor(i, 6));
        }

        private void WritePayload(Color32[] pixels, int width, int height, int blockSize, byte[] payloadBytes, int payloadByteCount)
        {
            int activeWidthBlocks = FrameCapacity.GetActiveWidthBlocks(width, blockSize);
            int payloadStartBlock = GetPayloadStartRow(width, blockSize) * activeWidthBlocks;
            int maxBlocks = GetPayloadBlocksForBytes(GetPayloadCapacityBytes(width, height, blockSize));
            int symbolCount = GetPayloadBlocksForBytes(payloadByteCount);
            for (int i = 0; i < symbolCount && i < maxBlocks; i++)
            {
                uint symbol = FrameRaster.ReadBits(payloadBytes, payloadByteCount, i * BitsPerSymbol, BitsPerSymbol);
                FrameRaster.WriteColorBlockAtIndex(pixels, width, height, blockSize, payloadStartBlock + i, SymbolToRgb20Color(symbol));
            }
        }
#endif

        public override void ApplyDecodeOptions()
        {
            selectedDecodeMaterial = byteDecodeMaterial;
            payloadStartRow = activeWidthBlocks > 0 ? 5 + ((320 + activeWidthBlocks - 1) / activeWidthBlocks) : 5;
            payloadBlockCount = (byteCount * 8 + 19) / 20;

            if (selectedDecodeMaterial != null)
                selectedDecodeMaterial.SetFloat("_Rgb20CalibrationStartBlock", calibrationStartBlock);
        }

#if UDONSHARP
        public override int GetEncoderSymbolMode()
        {
            return SymbolModeRgb20;
        }

        public override int GetEncoderPayloadStartRow(int width, int blockSize)
        {
            int activeWidthBlocks = Mathf.Max(1, GetEncoderActiveWidthBlocks(width, blockSize));
            return 5 + ((320 + activeWidthBlocks - 1) / activeWidthBlocks);
        }

        public override int GetEncoderPayloadCapacityBytes(int width, int height, int blockSize)
        {
            int activeWidthBlocks = GetEncoderActiveWidthBlocks(width, blockSize);
            int activeHeightBlocks = GetEncoderActiveHeightBlocks(height, blockSize);
            int payloadRows = Mathf.Max(0, activeHeightBlocks - GetEncoderPayloadStartRow(width, blockSize) - 1);
            return activeWidthBlocks * payloadRows * 20 / 8;
        }

        public override bool WriteEncoderPayload(Color32[] pixels, int width, int height, int blockSize, byte[] payloadBytes, int payloadByteCount)
        {
            int activeWidthBlocks = GetEncoderActiveWidthBlocks(width, blockSize);
            int activeHeightBlocks = GetEncoderActiveHeightBlocks(height, blockSize);
            if (pixels == null || payloadBytes == null || activeWidthBlocks <= 0 || activeHeightBlocks <= 0)
                return false;

            int blockCursor = 5 * activeWidthBlocks;
            for (int i = 0; i < 128; i++, blockCursor++)
                WriteEncoderColorBlockAtIndex(pixels, width, height, blockSize, blockCursor, RgbRedCalibrationColor(i, 7));
            for (int i = 0; i < 128; i++, blockCursor++)
                WriteEncoderColorBlockAtIndex(pixels, width, height, blockSize, blockCursor, RgbGreenCalibrationColor(i, 7));
            for (int i = 0; i < 64; i++, blockCursor++)
                WriteEncoderColorBlockAtIndex(pixels, width, height, blockSize, blockCursor, RgbBlueCalibrationColor(i, 6));

            int payloadStartRow = GetEncoderPayloadStartRow(width, blockSize);
            int payloadStartBlock = payloadStartRow * activeWidthBlocks;
            int maxBlocks = activeWidthBlocks * Mathf.Max(0, activeHeightBlocks - payloadStartRow - 1);
            int symbolCount = (payloadByteCount * 8 + 19) / 20;
            for (int i = 0; i < symbolCount && i < maxBlocks; i++)
                WriteEncoderColorBlockAtIndex(pixels, width, height, blockSize, payloadStartBlock + i, SymbolToRgb20Color(ReadEncoderBits(payloadBytes, payloadByteCount, i * 20, 20)));

            return true;
        }
#endif

        private static Color32 SymbolToRgb20Color(uint symbol)
        {
            int rIndex = (int)(symbol & 0x7Fu);
            int gIndex = (int)((symbol >> 7) & 0x7Fu);
            int bIndex = (int)((symbol >> 14) & 0x3Fu);
            return new Color32(QuantizeColorLevel(rIndex, 127), QuantizeColorLevel(gIndex, 127), QuantizeColorLevel(bIndex, 63), 255);
        }

        private static Color32 RgbRedCalibrationColor(int index, int bits)
        {
            return new Color32(QuantizeColorLevel(index, (1 << bits) - 1), 128, 128, 255);
        }

        private static Color32 RgbGreenCalibrationColor(int index, int bits)
        {
            return new Color32(128, QuantizeColorLevel(index, (1 << bits) - 1), 128, 255);
        }

        private static Color32 RgbBlueCalibrationColor(int index, int bits)
        {
            return new Color32(128, 128, QuantizeColorLevel(index, (1 << bits) - 1), 255);
        }

        private static byte QuantizeColorLevel(int index, int maxIndex)
        {
            if (maxIndex <= 0)
                return 128;

            return (byte)Mathf.RoundToInt(24f + index * (208f / maxIndex));
        }
    }
}
