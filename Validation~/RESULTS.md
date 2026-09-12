# RGB20 SDK-Optional Validation

Date: 2026-09-12. Unity 2022.3.22f1, Core 0.2.0, Luma4 0.0.3, Worlds SDK 3.10.4-beta.2 with bundled UdonSharp, RTX 4090 / Direct3D11.

The tested implementation is unchanged in 0.0.3-beta.3. Release artifact review found a validation-project-specific program-cache GUID in beta.2, which beta.3 restores to the original reference. Unlike RGB16, RGB20 has no bundled compiled program cache; UdonSharp generates it in the consuming project.

Evidence directory: F:/Unity/TSMP/Validation-Results/s07.

| Check | Result | Log / result prefix |
| --- | --- | --- |
| SDK-free import, Setup, shaders and native GPU loopback | Pass | 20260912-194654-RGB20-Play |
| Windows x64 Development Mono build, stripping disabled | Succeeded, zero errors, nine shader warnings | 20260912-194800-RGB20-Build |
| Actual Player, nine frames of Transform/Humanoid/Unicode/RPC loopback | Pass | 20260912-194836-RGB20-Player |
| Full Udon client compile, automatic backing, real Udon VM block/full-resolution output parity and GPU decoding | Pass | 20260912-194902-RGB20-Udon |

Player: F:/Unity/TSMP/Validation-Codecs-NoSDK/Build/RGB20/CodecValidation.exe. The adjacent .build-report.txt records the build result. All 1,027 test payload bytes matched the GPU output. RGB16 remained installed, also checking coexistence and automatic codec selection.

Shader warnings concern existing sample/classifier initialization analysis and signed integer divisions, including other installed codecs. Codec algorithms were not changed. No SDK world upload, IL2CPP, Quest or lossy transport test was performed. The Udon VM test is not a claim of an uploaded VRChat client test.
