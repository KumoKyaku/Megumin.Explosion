# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!--
## [Unreleased] - YYYY-MM-NN

### Added   
### Changed  
### Deprecated  
### Removed  
### Fixed  
### Security  
-->

---
## [Unreleased] - YYYY-MM-NN

### Added
- 新增 `MEGUMIN_UGUI` version define（`com.unity.ugui` >= 1.0.0）。
  UGUI 事件系统、UGUI 编辑器扩展与 TMP 相关代码按宏自动裁剪，
  未安装 UGUI/TMP 的工程可直接编译；安装后功能与原来一致。

### Fixed
- Unity 6.6：`Selection.instanceIDs` / `Object.GetInstanceID()` 改为
  `Selection.entityIds` / `Object.GetEntityId()`，旧版本仍走原 API。
- Unity 6.5+：`Graphics.DrawMesh` 去掉已过时的 `LightProbeProxyVolume` 重载参数。
- Unity 2021.3/2022：`Awaitable` 相关用例按 `UNITY_2023_1_OR_NEWER` 隔离，避免低版本编译失败。
- 3 个历史 GBK 编码源码文件（`DebugMeshData` / `TagMask` / `OtherCustomTest`）
  统一转为 UTF-8（无 BOM），避免不同编辑器/地区设置下注释与字符串乱码。

### Changed
- 清理 `NewButtonAttribute` 旧特性注册产生的 CS0618 警告，移除未使用字段。

## [4.1.0] - 2022-11-09
### Changed  
- EditorButton重命名为Button

## [4.0.0] - 2022-02-24
修复大量命名空间

## [3.0.0] - 2021-8-28
升级目标框架到netstandard2.1

## [1.0.0] - 2019-1-1
### Added
