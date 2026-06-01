[한국어](README.ko.md) | [English](README.en.md) | **日本語**

# TSMP Codec RGB20

RGB20 は RGB チャンネルをより高密度に使い、1 フレームにより多くの payload を格納する TSMP codec です。キャプチャと受信経路が色を安定して保持できる場合に使用します。

## 特徴

- RGB ベースの 20-bit TSMP シンボル
- RGB16 より高い payload 密度
- 色再現性が安定した配信/受信経路に適合
- 大きな状態 packet や多数の network component を使う TSMP 構成に有用
- `TSMPSetup` の Codec タブで自動検出

## 要件

- TSMP Core: https://github.com/kibalab/TSMP-Core
- `com.kibalab.tsmp.core` 0.0.1 以降
- VRChat Worlds SDK 3.9.0 以降

## インストール

VRChat Creator Companion で VPM リポジトリを追加します。

```text
https://vpm.kiba.red/
```

その後、`TSMP Core` と `TSMP Codec RGB20` をインストールします。

## 使い方

1. Core パッケージの `Packages/com.kibalab.tsmp.core/Samples/TSMPController.prefab` をシーンに配置します。
2. `TSMPSetup` の Codec タブで `Refresh Codecs` を押します。
3. `RGB20` を選択します。
4. `Apply Setup` を実行します。

## リリース状態

このパッケージは beta 段階で、`v0.0.x-beta.x` 形式のタグを使用します。

## ライセンス

MIT License. Copyright (c) 2026 KIBA_Labs.
