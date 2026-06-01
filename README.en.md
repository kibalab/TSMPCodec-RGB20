[한국어](README.ko.md) | **English** | [日本語](README.md)

# TSMP Codec RGB20

RGB20 is a high-density TSMP codec that uses RGB channels more tightly to carry more payload in each frame. Use it when the capture and receive path preserves color values reliably.

## Characteristics

- RGB-based 20-bit TSMP symbols
- Higher payload density than RGB16
- Best for broadcast and receive paths with stable color fidelity
- Useful for TSMP scenes with larger state packets or more network components
- Automatically discovered in the `TSMPSetup` Codec tab

## Requirements

- TSMP Core: https://github.com/kibalab/TSMP-Core
- `com.kibalab.tsmp.core` 0.0.1 or newer
- VRChat Worlds SDK 3.9.0 or newer

## Installation

Add the VPM repository in VRChat Creator Companion.

```text
https://vpm.kiba.red/
```

Then install `TSMP Core` and `TSMP Codec RGB20`.

## Usage

1. Add `Packages/com.kibalab.tsmp.core/Samples/TSMPController.prefab` from the Core package to your scene.
2. Open the Codec tab in `TSMPSetup` and click `Refresh Codecs`.
3. Select `RGB20`.
4. Click `Apply Setup`.

## Release Status

This package is currently beta and uses `v0.0.x-beta.x` tags.

## License

MIT License. Copyright (c) 2026 KIBA_Labs.
