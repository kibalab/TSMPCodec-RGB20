# TSMP Codec RGB20

TSMP 用の高密度 RGB codec パッケージです。RGB16 より多くの payload を 1 フレームに格納できます。色再現性が安定したストリーム経路での使用に向いています。

## 要件

- TSMP Core: https://github.com/kibalab/TSMP-Core
- `com.kibalab.tsmp.core` 0.0.1 以降
- VRChat Worlds SDK 3.9.0 以降

## 使い方

TSMP Core と一緒にこのパッケージをインストールし、Core の `Samples/TSMPController.prefab` をシーンに配置します。その後、`TSMPSetup` の Codec タブで `RGB20` を選択し、`Apply Setup` を実行します。

## リリース状態

このパッケージは beta 段階で、`v0.0.x-beta.x` 形式のタグを使用します。
