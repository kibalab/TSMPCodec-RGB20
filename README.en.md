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
- `com.kibalab.tsmp.core` 0.3.0-beta.2 or newer
- Unity 2022.3
- VRChat Worlds SDK 3.9.0 or newer only for VRChat; not required in ordinary Unity

## Installation

Add the VPM repository in VRChat Creator Companion.

```text
https://vpm.kiba.red/
```

Then install `TSMP Core` and `TSMP Codec RGB20`.

For ordinary Unity, install Core 0.3.0-beta.2, the default Luma4 codec and this codec through Unity Package Manager. For a local checkout, use Add package from disk on each package.json; VRCSDK is not required. UPM uses an exact Core 0.3.0-beta.2 dependency; VPM accepts Core 0.3.0-beta.2 or newer.

## Usage

1. Add `Packages/com.kibalab.tsmp.core/Samples/TSMPController.prefab` from the Core package to your scene.
2. Open the Codec tab in `TSMPSetup` and select `RGB20` from the automatically discovered codecs.
3. Setup prepares the codec and its materials automatically in both ordinary Unity and VRChat. No conversion menu is needed.

## Release Status

This package is currently beta and uses `v0.0.x-beta.x` tags.

## License

MIT License. Copyright (c) 2026 KIBA_Labs.

## Preparation API compatibility

This release requires the preparation API introduced in Core 0.3.0-beta.2. Core 0.2.0 and 0.3.0-beta.1 lack `PrepareDecode` and cannot compile this codec, even when the calibration material is unassigned. Update Core before installing this codec. A missing preparation material only selects the original shader path after compilation.

UPM uses a version string, while VPM uses a version range. Local/disk or Git installs must supply a compatible Core directly in the project's dependencies; package metadata does not tell UPM to fetch Core from GitHub. For VPM betas, enable pre-release packages and select the matching versions after publication.
