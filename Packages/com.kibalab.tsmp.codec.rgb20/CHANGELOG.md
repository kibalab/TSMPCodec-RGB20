# Changelog

## 0.0.3-beta.4

- Require Core 0.3.0-beta.2 in UPM and >=0.3.0-beta.2 in VPM because the codec now calls the preparation API. Core 0.2.0 and 0.3.0-beta.1 do not provide that API.
- Use Float32 calibration preparation for effective sample sizes greater than one; retain the original single-sample path.
- Decode at most three symbols per output pixel and pack their bits once instead of repeating symbol decoding for each output byte.
- Include the preparation shader/material on the codec prefab. Missing preparation resources retain ordinary decoding with a compatible Core.
- Preserve codec IDs, packet layout and existing script/material/prefab GUIDs. Keep VRChat SDK requirements in VPM only.
- Update Core before installing this codec. Enable prerelease packages in VCC to select the matching beta versions.

## 0.0.3-beta.3

- Preserve the original SDK-generated program-cache reference instead of publishing a validation-project-specific GUID. UdonSharp creates the compiled cache locally; this package ships the program source and field metadata, not a precompiled program cache.
- Keep the SDK-optional workflow, dependencies, shader fixes and codec behavior from 0.0.3-beta.2 unchanged.

## 0.0.3-beta.2

- Support ordinary Unity without a VRChat SDK dependency through UPM; retain Worlds SDK requirements for VPM.
- Require Core 0.2.0 and use its shared automatic Controller workflow with an SDK-neutral codec prefab.
- Detect installed Worlds packages with an assembly version define so the Udon encoder path is available without manual scripting defines.
- Resolve shared shader includes through Packages/com.kibalab.tsmp.core for local and installed packages.
- Preserve codec IDs, settings, script/material/prefab GUIDs and the encoded wire format.
- Regenerate Udon field metadata against Core 0.2.0 without profiler instrumentation.

## 0.0.3-beta.1

- Beta release metadata for VPM distribution.
- Includes RGB20 codec runtime, shaders, materials, prefab, and sample.
