# GPU Decode Regression

Run in a dedicated Unity 2022.3 project referencing this checkout and the actual Core and Luma4 packages. The runner installs a script under `Assets/GpuShaderValidation` and replaces the test scene. Close that project before invoking Unity; do not use a working scene project.

```powershell
./Validation~/Gpu/Run-Validation.ps1 -UnityPath 'C:/Program Files/Unity/Hub/Editor/2022.3.22f1/Editor/Unity.exe' -ProjectPath F:/Unity/TSMP/Validation-Codecs-NoSDK -ResultsDirectory F:/Unity/TSMP/Validation-Results/gpu
```

The test requires a graphics device and uses D3D11, not `-nographics`. `SymbolBaselines` contains the decoder from commit `3d8b1bc`, stored as text outside Unity import. Both baseline and current shaders are compiled and exercised on the GPU.

## Symbol Reuse (G03)

The fixture uses the native 7/7/6 writer at 1280 x 720 with 8-pixel blocks, a linear source, and a linear 4096 x 1 RGBA8 output. It compares the complete output against the baseline for sample sizes 1/4/8, exact raster, deterministic channel distortion/noise, flat calibration/tie input, and byte counts 0/1/2/3/4/5/7/16/31/56/257/1027. Exact input must also round-trip to its payload, and bytes after the requested count must remain zero.

Timings wrap one blit in each command-buffer GPU marker, alternate baseline/candidate order, warm up for 20 frames, and collect 45 frames with one GPU sample block per marker. Readbacks are outside the timed region.

### Verified Result

Unity 2022.3.22f1, NVIDIA GeForce RTX 4090, Direct3D11: all 108 output cases passed, including exact round-trip and trailing-zero assertions.

| Payload bytes | Sample size | Before (us) | After (us) |
| ---: | ---: | ---: | ---: |
| 4 | 1 | 149.504 | 124.928 |
| 56 | 1 | 275.456 | 188.416 |
| 1027 | 1 | 276.480 | 189.440 |
| 4 | 4 | 1324.032 | 899.072 |
| 56 | 4 | 2214.912 | 1388.544 |
| 1027 | 4 | 2242.560 | 1405.952 |

The shader now decodes at most three symbols per output pixel and packs them into one 32-bit value. Channel classification and calibration are unchanged. These are GPU microbenchmarks, not whole-frame CPU or VRChat timings. GPU clocks were not locked and another editor was open; gains on other GPUs/APIs or actual compressed streams remain unmeasured.
