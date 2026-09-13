# GPU Decode Regression

Run in a dedicated Unity 2022.3 project referencing this checkout and the actual Core and Luma4 packages. The runner installs a script under `Assets/GpuShaderValidation` and replaces the test scene. Close that project before invoking Unity; do not use a working scene project.

```powershell
./Validation~/Gpu/Run-Validation.ps1 -UnityPath 'C:/Program Files/Unity/Hub/Editor/2022.3.22f1/Editor/Unity.exe' -ProjectPath F:/Unity/TSMP/Validation-Codecs-NoSDK -ResultsDirectory F:/Unity/TSMP/Validation-Results/gpu
```

The test requires a graphics device and uses D3D11, not `-nographics`. `SymbolBaselines` contains the decoder from commit `3d8b1bc`, stored as text outside Unity import. Both baseline and current shaders are compiled and exercised on the GPU.

## Symbol Reuse (G03)

The fixture uses the native 7/7/6 writer at 1280 x 720 with 8-pixel blocks, a linear source, and a linear 4096 x 1 RGBA8 output. It compares the complete output against the baseline for sample sizes 1/4/8, exact raster, deterministic channel distortion/noise, flat calibration/tie input, and byte counts 0/1/2/3/4/5/7/16/31/56/257/1027. Exact input must also round-trip to its payload, and bytes after the requested count must remain zero.

Timings wrap 32 blits in each uniquely named GPU marker and wait for GPU completion between baseline and candidate batches. The runner alternates order, warms up for 10 frames, and collects 15 frames. Medians are divided by 32; readbacks are outside the GPU markers. The additional wall-clock values include amortized submission, completion and readback costs.

This replaces the initial single-draw timing method, which sometimes returned zero or misattributed adjacent work. Only completion-separated batch measurements are reported below.

### Verified Result

Unity 2022.3.22f1, NVIDIA GeForce RTX 4090, Direct3D11: all 108 output cases passed, including exact round-trip and trailing-zero assertions.

| Payload bytes | Sample size | Before (us) | After (us) |
| ---: | ---: | ---: | ---: |
| 4 | 1 | 26.976 | 10.432 |
| 56 | 1 | 44.544 | 17.120 |
| 1027 | 1 | 45.408 | 17.464 |
| 4 | 4 | 398.136 | 167.672 |
| 56 | 4 | 639.904 | 252.992 |
| 1027 | 4 | 665.856 | 254.848 |

The shader now decodes at most three symbols per output pixel and packs them into one 32-bit value. Channel classification and calibration are unchanged. These are GPU microbenchmarks, not whole-frame CPU or VRChat timings. GPU clocks were not locked and another editor was open; gains on other GPUs/APIs or actual compressed streams remain unmeasured.
