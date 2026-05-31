# TSMP Codec RGB20

RGB20 TSMP codec, Udon handler, shaders, materials, and prefab.

## Requirements

- `com.kibalab.tsmp.core` 0.0.1 or newer
- VRChat Worlds SDK 3.9.0 or newer

## Usage

Import this package with TSMP Core. `TSMPSetup` discovers `Runtime/TSMPCodecCatalog.asset` in the editor and adds the RGB20 codec prefab to the codec list automatically.

RGB20 is a high-density codec intended for payload-heavy TSMP streams where the extra decode cost is acceptable.
