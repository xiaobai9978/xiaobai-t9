# 小白T9 / 小狼毫 fork —— ARM 架构支持可行性分析

> 分析日期：2026-10-10
> 约束：**不修改任何源码**（本报告只做静态分析与路线设计，未改动任何文件）
> 参考对象：上游 rime 小狼毫（Weasel）的 ARM 实现
> 结论定位：**可行，但工作量不在"写代码"，而在"工具链 + 依赖重建 + 安装器分发"这三件事上**

---

## 〇、结论速览

| 维度 | 结论 |
|---|---|
| 整体可行性 | ✅ **可行**。C++ 内核在**源码层已经支持 ARM64**（继承自上游），不是从零适配 |
| 主要障碍 | ① 本机**没有 ARM64 编译器**；② t9s2t 的 ASR 引擎是 x64 绑定；③ 安装器**完全没有架构分发能力** |
| 需要改源码吗 | **核心场景不需要**。C++ 工程的 ARM64 配置已存在；C# 侧只需改 `.csproj` 工程配置（非源码）；安装器是 `.nsi` 脚本 |
| 最大风险 | **没有真实的 ARM64 设备可用于验证**——这是整件事里最难绕过的一条 |
| 工作量量级 | 工具链就绪后，核心链路约 **1–2 人天**跑通；做到"4 架构单安装包 + 全组件原生"约 **5–10 人天** |
| 建议 | 按 **P0（工具链验证）→ P1（内核 ARM64 分发）→ P2（C# 原生化）→ P3（t9s2t 引擎）** 四步走；在 P0 拿到真实 ARM 设备之前不要投入 P2 之后的开发 |

---

## 一、当前 ARM 支持的现状

### 1.1 C++ 内核：**源码级已支持**（这块最大，且已就绪）

| 工程 | Win32 | x64 | ARM | ARM64 | ARM64 配置数 |
|---|---|---|---|---|---|
| `WeaselServer` | ✔ | ✔ | ✔ | **✔** | 13 |
| `WeaselDeployer` | ✔ | ✔ | ✘ | **✘** | 0 |
| `WeaselSetup` | ✔ | ✘ | ✘ | **✘** | 0 |
| `WeaselIPC` | ✔ | ✔ | ✔ | **✔** | 14 |
| `WeaselIPCServer` | ✔ | ✔ | ✔ | **✔** | 12 |
| `WeaselUI` | ✔ | ✔ | ✔ | **✔** | 14 |
| `WeaselIME` | ✔ | ✔ | ✔ | **✔** | 18 |
| `WeaselTSF` | ✔ | ✔ | ✔ | **✔** | 20 |
| `RimeWithWeasel` | ✔ | ✔ | ✔ | **✔** | 12 |

`weasel.sln` 里已经有 **60 处 ARM64** 引用；`build.bat` / `xbuild.bat` 都有 `arm64` 构建开关：

```bat
build.bat arm64 release     →  b2 (Boost ARM32+ARM64) → msbuild weasel.sln /p:Platform=ARM
                                                       → msbuild weasel.sln /p:Platform=ARM64
                                                       → arm64x_wrapper/build.bat
                                                       → copy weaselARM64X.dll / .ime
```

**源码里的 ARM 特化处理也已经写好**：

| 位置 | 作用 |
|---|---|
| `WeaselTSF/Register.cpp:215` `#ifdef _M_ARM64` | ARM64 下注册表 `InprocServer32` 要重写为指向 **ARM64X 重定向 DLL**（因为 `GetModuleFileNameA` 会返回实际加载的 `weaselARM64.dll`） |
| `WeaselSetup/imesetup.cpp:70` `is_arm64_machine()` | 用 `IsWow64Process2` 判断本机是否 ARM64 |
| `WeaselSetup/imesetup.cpp:163` | ARM64 上把 **ARM32 版本**装进 WOW ARM32 系统目录（注释写明"适用于低于 Windows 11 24H2"） |
| `arm64x_wrapper/` | 4 个 `.def` + `build.bat`：用 `cl /arm64EC` + `link /machine:arm64x /defArm64Native:` 生成 **ARM64X** 的 `weaselARM64X.dll` / `.ime`（同一文件在 ARM64 与 x64 进程里都能用） |

`CHANGELOG.md:206-207` 也直接写着上游能力：
```
* 支持 arm/arm64 架構
* 單安裝包支持 win32/x64/arm/arm64 架構系統的自動釋放文件
```

### 1.2 产物侧：**完全没有 ARM64**（这是真正的缺口）

实测 `output/` 下所有 PE 文件的机器类型：

| 文件 | 当前架构 |
|---|---|
| `WeaselServer.exe` / `WeaselDeployer.exe` | x64 |
| `WeaselSetup.exe` | x86 |
| `Win32\WeaselServer.exe` / `Win32\WeaselDeployer.exe` / `Win32\rime.dll` | x86 |
| `rime.dll` / `WinSparkle.dll` / `weaselx64.ime` / `weaselx64.dll` | x64 |
| `weasel.ime` / `weasel.dll` | x86 |
| `output\ARM64\`、`weaselARM64X.*` | **不存在** |
| 7 个 C# 工具 + 4 个安装包 | **全部 x86(32)**（AnyCPU 未设 `Prefer32Bit=false`） |
| `t9s2t\t9s2t.exe` + `onnxruntime.dll` + `sherpa-onnx-c-api.dll` | **x64** |

### 1.3 安装器：**只处理注册表视图，不做架构分发**

`output/install.nsi` 里一共 7 处 `${If} ${IsNativeARM64}`，**全部只是 `SetRegView 64`**，没有一处是架构相关的文件投递。`File` 语句是无条件的：

```
File "weasel.dll"      File "weaselx64.dll"
File "weasel.ime"      File "weaselx64.ime"
File "Win32\WeaselServer.exe"  File "Win32\rime.dll" ...
```

**上游那套"单安装包按架构自动释放文件"的逻辑，在这个 fork 里是缺失的。**

### 1.4 与上游的做法对比

| 维度 | 上游小狼毫 | 本 fork 现状 |
|---|---|---|
| 内核 ARM64 构建配置 | 有 | **有**（继承） |
| ARM64X 重定向 DLL | 用（`weasel.dll` 单文件重定向 → `weaselARM64.dll` / `weaselx64.dll`） | 有 `arm64x_wrapper` 脚本，但**产物从未生成**，安装器仍在投递 `weasel.dll`+`weaselx64.dll` 双文件老方案 |
| 单安装包多架构释放 | 有 | **没有** |
| ARM32 WOW 目录处理 | 有（`is_arm64_machine()` + `get_wow_arm32_system_dir`） | 源码里有，产物与安装器未启用 |
| 实际 ARM64 产物 | 有 | **无** |

**一句话总结现状：源码（含 fork 自己的脚本）已经把 ARM64 的路铺好了，但从来没走上去过——从未构建、从未打包、从未分发。**

---

## 二、主要障碍

### 障碍 1（硬阻塞）：本机没有 ARM64 工具链

| 检查项 | 实测结果 |
|---|---|
| VS2019 MSVC 14.16 / 14.29 的编译器目录 | 只有 `Hostx64\{x64,x86}` 与 `Hostx86\{x64,x86}`，**没有 `arm64`** ❌ |
| `C:\Program Files\Microsoft Visual Studio\2022\` | **是个空目录**（2023-06-09 创建的残留），VS2022 **并未真正安装** ❌ |
| `arm64x_wrapper/build.bat` 的硬性依赖 | `vswhere -requires Microsoft.VisualStudio.Component.VC.Tools.ARM64EC` ❌ 未满足 |
| Windows SDK | 已装 `10.0.20348.0`，`bin\...\arm64\` 有 ARM64 工具、`Lib\...\um\arm64` 有 ARM64 库 ✔（但 ARM64X 需要更新的 SDK） |

**结论**：**当前机器连一个 ARM64 目标都编不出来**，更谈不上 ARM64EC / ARM64X。
必须先装 **VS2022（17.4+）+「MSVC v143 - VS 2022 C++ ARM64/ARM64EC 生成工具」+ 较新的 Windows 11 SDK**。

> ⚠️ 注意 `env.vs2022.bat` 里写的是 `CMAKE_GENERATOR="Visual Studio 17 2022"` / `PLATFORM_TOOLSET=v143` / `BJAM_TOOLSET=msvc-14.3`，
> 而当前生效的 `env.bat` 用的是 VS2019 时代配置 —— 工具链升级本身就是一次不小的环境变更。

### 障碍 2（硬阻塞）：t9s2t 的语音引擎是 x64 绑定

`t9s2t` 运行时加载两个原生库（源码里写死文件名）：

```csharp
private static readonly string[] RequiredEngineDlls = { "onnxruntime.dll", "sherpa-onnx-c-api.dll" };
```

- 现有产物都是 **x64**，且 `t9s2t.exe` 自身也是 **x64** 进程；
- ARM64 上只有两条路：

| 方案 | 前提 | 代价 |
|---|---|---|
| **(a) 靠 x64 模拟运行**（零改动） | 仅 **Windows 11 ARM**（Win10 ARM 无 x64 模拟） | ASR 推理跑在模拟层，性能显著下降 |
| **(b) 换 ARM64 原生引擎** | 需要 ARM64 的 `onnxruntime.dll` + `sherpa-onnx-c-api.dll`，并让 t9s2t 以 64 位/ARM64 进程运行 | 需要改**.csproj 位数**（非源码）+ **安装器按架构分发同名 DLL** |

**好消息（推翻了旧结论）**：这两个依赖的 ARM64 原生版本**上游都有官方发布**——

| 依赖 | ARM64 Windows 可用性 | 证据 |
|---|---|---|
| **sherpa-onnx** | ✅ **有官方预编译** | `sherpa-onnx-v1.13.8-win-arm64-shared-MT-Release-lib.tar.bz2`；NuGet 包 `org.k2fsa.sherpa.onnx.runtime.win-arm64`（1.13.8，2025 年发布） |
| **ONNX Runtime** | ✅ 官方支持 `win-arm64` RID | `Microsoft.ML.OnnxRuntime` 含全平台原生库；各 EP 包矩阵明确列出 `win-arm64` |

> 📌 需要更正一条旧认知：此前记录"上游只发布 win-x64、没有其它架构"——**实测 sherpa-onnx 从 v1.13.x 起已提供 win-arm64 预编译包与对应 NuGet**。
> 另外 `engine.json` 目前是**写死两个 DLL URL、不含架构字段**的结构，要在不改 C# 源码的前提下支持 ARM64，
> 只能靠 **安装器按架构投递不同文件**（把 ARM64 的 DLL 放到 ARM64 安装包的同一路径），而不是让程序自己选。

### 障碍 3（中）：librime 及其全部第三方依赖需要 ARM64 重建

`librime` 与 `plum` 都是 git submodule，链接用的是**预编译产物**：

```
lib/rime.lib (x86)      lib64/rime.lib (x64)
librime/lib/{glog,gtest,leveldb,marisa,opencc,yaml-cpp}.lib
```

- librime 本身是跨平台的（上游有 Linux aarch64 / Android 构建），`librime/build.bat` 支持 `-A%ARCH%`，**源码层面没障碍**；
- 但**这一整套（boost + librime + glog/leveldb/marisa/opencc/yaml-cpp + WinSparkle）都要为 ARM64 重新构建**，是本次工作中**最重的一块体力活**；
- `WinSparkle` 只有 `.lib/.dll` 预编译产物（`lib/`=x86、`lib64/`=x64）+ 头文件，**没有源码也没有 ARM64 版** → 需要单独从上游取源码构建，或确认其 ARM64 支持情况。

### 障碍 4（中）：安装器完全没有架构分发能力

需要新增的不是"改几行"，而是**一整套分发逻辑**：
- 按 `$IsNativeARM64 / $IsNativeAMD64 / 其它` 分派文件；
- 处理 `weasel.dll` / `weasel.ime` 在 ARM64 上的**重定向式（ARM64X）**投递；
- 处理 **ARM32 WOW 目录**（Win11 24H2 之前才有该目录）；
- 卸载段要能正确清理所有架构的文件与注册表项。

### 障碍 5（低-中）：C# 工具与 .NET Framework 版本门槛

- 7 个 C# 工程都是 `AnyCPU` 且**没有** `<Prefer32Bit>false</Prefer32Bit>` → 编译结果是 **`anycpu32bitpreferred`（32 位进程）**；
- 在 ARM64 Windows 上以 **WOW x86 模拟**运行 —— **功能可用，性能一般**；
- 若想原生 ARM64 执行：需把工程改成 `Prefer32Bit=false`（**这是 `csproj` 工程配置，不是源码**），前提是目标机装有 **.NET Framework 4.8.1+ 的 ARM64 CLR**（Windows 11 内置；Windows 10 ARM 上不存在，只能继续模拟）；
- 好消息：键盘钩子（`SetWindowsHookEx(WH_KEYBOARD_LL)`）、`keybd_event`、`RegisterHotKey`、托盘、WinForms 在 ARM64 上语义完全一致，**没有需要改的逻辑**；
- `numkeyboard` 依赖 `System.Management`（WMI）→ 在 ARM64 上由 WMI 服务提供，纯托管调用，无架构问题。

### 障碍 6（低）：签名流程要扩展到新产物

现有 Certum 证书 + `Set-AuthenticodeSignature` 流程**对 ARM64/ARM64X 产物同样适用**，只需把新产物纳入批量签名清单。无额外门槛。

---

## 三、需要调整或新增的模块

| # | 模块 | 现状 | 需要做什么 | 是否触及源码 |
|---|---|---|---|---|
| 1 | **VS 工具链** | 无 ARM64 编译器 | 安装 VS2022 17.4+ / ARM64+ARM64EC 组件 / 新 SDK | ✘ 环境 |
| 2 | `env.bat` | VS2019 配置 | 切到 v143 / VS2022（参考现成的 `env.vs2022.bat`） | ✘ 构建配置 |
| 3 | **Boost** | 有 x86/x64 | `build.bat boost arm64`（脚本已支持） | ✘ 无 |
| 4 | **librime + 第三方** | 只有 x86/x64 `.lib` | 全部 ARM64 重建（glog/gtest/leveldb/marisa/opencc/yaml-cpp） | ✘ 无 |
| 5 | **WinSparkle** | 只有 x86/x64 预编译 | 取源码为 ARM64 构建，或评估替换/去掉更新检查 | ✘ 无 |
| 6 | **Weasel C++ 组件** | 7/9 有 ARM64 配置 | 直接构建即可（`msbuild /p:Platform=ARM64`） | ✘ 无 |
| 7 | **WeaselDeployer / WeaselSetup** | 无 ARM64 配置 | 保持"Win32 版靠 x86 模拟"（当前 sln 已这么映射）**或**补 ARM64 配置 | **需要改 `.vcxproj`**（若要原生） |
| 8 | **ARM64X 重定向 DLL** | 脚本在，产物无 | 运行 `arm64x_wrapper/build.bat` 产出 `weaselARM64X.dll/.ime` | ✘ 无 |
| 9 | **C# 工具位数** | 全 32 位 | 7 个 `.csproj` 加 `<Prefer32Bit>false</Prefer32Bit>`（或加 ARM64 平台） | ✘ 工程配置 |
| 10 | **t9s2t 引擎** | x64 绑定 | 服务端补 ARM64 版 DLL；安装器按架构投递 | ✘ 无（走打包） |
| 11 | **`install.nsi`** | 只做 SetRegView | 增加架构判断 + 文件分派 + 卸载清理 | ✘ 打包脚本 |
| 12 | **签名/校验脚本** | 覆盖现有产物 | 清单里加 ARM64 产物 | ✘ 脚本 |

> **只有第 7 项在"要原生 ARM64 的部署/设置程序"这一前提下才需要动 `.vcxproj`**；其余全部是构建、打包、环境层面的事。
> 也就是说：**在不改任何 C++/C# 源码的前提下，ARM64 支持是可以做出来的。**

---

## 四、依赖项与第三方库兼容性总表

| 依赖 | 类型 | ARM64 可用性 | 说明 |
|---|---|---|---|
| **Boost 1.84** | 源码 | ✅ | `build.bat` 已有 `architecture=arm address-model=64` 配置 |
| **librime** | 源码（submodule） | ✅ | 跨平台，`build.bat` 支持 `-A%ARCH%` |
| **glog / gtest / leveldb / marisa / yaml-cpp** | 源码（librime thirdparty） | ✅ | 随 librime 一起重建 |
| **OpenCC** | 源码 | ✅ | 同上 |
| **WinSparkle** | **预编译** | ⚠️ **待确认** | 仓库只有 `.lib/.dll` + 头文件，无源码、无 ARM64 版；需单独获取源码构建 |
| **ONNX Runtime** | 预编译 | ✅ | 官方 `win-arm64` RID |
| **sherpa-onnx** | 预编译 | ✅ | 官方 `win-arm64` 预编译包 + NuGet |
| **NAudio / Newtonsoft.Json / SharpCompress / ZstdSharp 等** | 托管或 IL | ✅ | 架构无关，在 ARM64 CLR 上直接可用 |
| **7z.exe / curl.exe** | 第三方 x86 二进制 | ✅（模拟） | 仅安装/更新期用，x86 模拟即可 |
| **CMake / NSIS / Git** | 构建工具 | ✅ | 均可在 ARM64 或 x64 模拟下运行 |
| **.NET Framework 4.8 / 4.6.1** | 运行时 | ⚠️ 有条件 | 原生 ARM64 CLR 需 **4.8.1+**（Win11 内置）；Win10 ARM 只能跑模拟的 32 位 CLR |

**结论：真正的第三方不确定性只有 WinSparkle 一个**，其余要么源码可控，要么上游已提供 ARM64。

---

## 五、构建与打包流程的改动点

```
现在：  env.bat(VS2019) → build.bat → boost(x86/x64) + librime(x86/x64)
                                        → msbuild sln /p:Platform=x64
                                        → msbuild sln /p:Platform=Win32
                                        → makensis install.nsi（无条件投递 x86/x64 文件）

目标：  env.bat(VS2022, v143) → build.bat arm64 → boost(含 arm64) + librime(含 arm64)
                                                  → msbuild sln /p:Platform=ARM   ← 已有
                                                  → msbuild sln /p:Platform=ARM64 ← 已有
                                                  → msbuild sln /p:Platform=x64   ← 已有
                                                  → msbuild sln /p:Platform=Win32 ← 已有
                                                  → arm64x_wrapper/build.bat      ← 已有
                                                  → makensis install.nsi（★ 需新增架构分派）
```

**具体改动点**

| 环节 | 改动 |
|---|---|
| 环境 | `env.bat`：`PLATFORM_TOOLSET=v143`、`BJAM_TOOLSET=msvc-14.3`、`CMAKE_GENERATOR="Visual Studio 17 2022"`（可直接参考 `env.vs2022.bat`） |
| 产物布局 | 建议新增 `output\ARM64\`（与现有 `Win32\` 对称），或采用上游的 ARM64X 单文件名方案 |
| 安装器 | ① 用 `${IsNativeARM64}` 分派主程序/rime.dll/WinSparkle.dll/IME；② `weasel.dll`+`weaselx64.dll` 改为投递 ARM64X 版本（或补 `weaselARM64.dll`）；③ ARM32 WOW 目录处理；④ 卸载段清理全架构 |
| C# 工程 | 7 个 `.csproj` 增加 `<Prefer32Bit>false</Prefer32Bit>`；`t9s2t` 需要按架构出包（x64 包 / ARM64 包） |
| 引擎分发 | 服务端 `models/` 目录补 ARM64 版 `onnxruntime.dll` + `sherpa-onnx-c-api.dll`；安装器按架构投递 |
| 签名 | 批量签名清单加入 ARM64 产物 |
| 体积 | 4 架构全塞进单包 → 预计从当前 **45 MB** 增至 **60–75 MB**（取决于是否用 ARM64X 合并文件） |

---

## 六、潜在风险与工作量评估

### 6.1 风险清单

| # | 风险 | 等级 | 说明 / 缓解 |
|---|---|---|---|
| R1 | **没有真实 ARM64 设备可供验证** | 🔴 高 | 全部产物即使编出来也无法确认可用性。**这是整个项目最大的风险**，建议在动手前先解决设备（或确认目标用户是否真有 ARM 设备） |
| R2 | 工具链与 ARM64X 的版本要求高 | 🟠 中高 | ARM64EC 需 VS2022 17.3+、ARM64X 需 17.4+，且 SDK 要够新；现有 SDK 10.0.20348 偏旧 |
| R3 | WinSparkle 无 ARM64 预编译 | 🟠 中 | 需从源码构建；最坏情况可先禁用更新检查（但要改源码，与约束冲突）→ 建议优先取源码构建 |
| R4 | ARM32 WOW 在 Win11 24H2 后被移除 | 🟡 中 | 32 位回退路径适用面收窄；安装器需要条件化处理（上游已这么做） |
| R5 | Win10 ARM 无 x64 模拟 | 🟡 中 | 若目标包含 Win10 ARM，t9s2t **必须**原生 ARM64，没有"零改动"退路 |
| R6 | .NET Framework ARM64 CLR 门槛 | 🟡 中 | 原生 ARM64 执行需 4.8.1+（Win11 内置 + 可能需随安装包分发） |
| R7 | 安装包体积膨胀 | 🟢 低 | 4 架构并存不可避免；可用 ARM64X 合并 dll/ime 减少两处 |
| R8 | 签名流程扩展 | 🟢 低 | 现成流程可直接复用 |
| R9 | 用户群 ARM 占比未知 | 🟠 中 | **投入产出比问题**：如果实际用户里 ARM 设备占比很低，完整支持的优先级应下调 |

### 6.2 工作量评估（工具链就绪后）

| 阶段 | 内容 | 预估 |
|---|---|---|
| P0 | 装 VS2022 + ARM64/ARM64EC 工具链，`build.bat arm64` 跑通 C++ 组件 | **0.5–1 人天**（下载安装占大头） |
| P1 | Boost + librime + 第三方 ARM64 重建 | **1–2 人天**（编译时间长，踩坑主要在第三方） |
| P2 | WinSparkle ARM64 | **0.5–1 人天**（或记为风险） |
| P3 | 安装器架构分派改造 | **1–2 人天** |
| P4 | C# 工具去 Prefer32Bit + 验证 | **0.5 人天** |
| P5 | t9s2t ARM64 引擎 + 服务端补文件 + 按架构分包 | **0.5–1 人天** |
| P6 | 全流程联调 + 签名 + 出包 | **1 人天** |
| **合计** | 核心链路（P0+P1+P3，仅 Weasel 内核） | **≈ 2.5–5 人天** |
| **合计** | 完整 4 架构 + 全组件原生 | **≈ 5–9 人天** |

> 注意：以上**不含调试时间**。没有真机的情况下，R1 会让"调试"变成"盲调"，实际工期可能翻倍。

---

## 七、可行路线与优先级建议

### 路线 A（推荐）：渐进式原生支持

| 优先级 | 目标 | 动作 | 产出 |
|---|---|---|---|
| **P0** | 打通工具链 | 装 VS2022 17.4+ + ARM64/ARM64EC 组件 + 新 SDK；`build.bat arm64 release` 跑通 | 能产出 ARM64 版 Weasel 组件与 `weaselARM64X.dll/.ime` |
| **P1** | 内核可用 | 安装器增加架构分派：ARM64 上投递 ARM64 的 `WeaselServer/WeaselDeployer/rime.dll` + ARM64X 的 `weasel.dll/.ime`；`WeaselDeployer/WeaselSetup` 继续用 Win32 版走 x86 模拟 | **ARM64 设备上输入法内核可用** |
| **P2** | 周边工具原生化 | 7 个 `.csproj` 加 `<Prefer32Bit>false</Prefer32Bit>`；验证 Win11 ARM 上原生运行 | C# 工具性能提升 |
| **P3** | 语音引擎原生化 | 服务端补 ARM64 版引擎 DLL；安装器按架构投递；t9s2t 出 ARM64 包 | t9s2t 在 ARM64 上原生可用（也才可能在 Win10 ARM 上可用） |
| **P4** | 收尾 | 4 架构合一安装包、签名、体积优化（ARM64X 合并） | 单一安装包多架构 |

**为什么这个顺序**：P0 是唯一能证伪整件事的环节（工具链不通则一切休谈），P1 用最小代价拿到"能用的输入法"，
P2/P3 都是"锦上添花 + 性能"，P4 才是体验统一。

### 路线 B（最小代价）：全模拟，零构建改动

- 不做任何 ARM64 构建，依赖 Windows 自身的 x86/x64 模拟运行现有产物；
- **适用面**：仅 Windows 11 ARM（x64 模拟）；Win10 ARM 上只有 32 位组件能跑，**WeaselServer 等 x64 组件直接不可用**；
- **代价**：性能差、t9s2t 推理慢、不满足"原生支持"的任何预期；
- 结论：**只能在紧急情况下当临时方案，不建议作为目标方案**。

### 路线 C（不推荐）：等上游

上游小狼毫已有 ARM64 实现，理论上可以合并上游的安装器改动。但由于本 fork 的 `install.nsi` 已被大幅改写（广告面板、t9s2t 分发、计划任务自启等），
**上游的分发逻辑无法直接 cherry-pick**，仍然要手工重构。等价于路线 A 的 P1。

---

## 八、最终结论

1. **可行性：成立。** 这个项目对 ARM 的支持**不是从零开始**——C++ 内核的 ARM64 工程配置、ARM64X 重定向方案、甚至 `WeaselSetup` 里的 ARM64 机器判定与 ARM32 WOW 处理**都已经写在源码里了**，`build.bat arm64` 也是现成的。缺的是"从来没构建过、从来没过安装器"。
2. **约束满足性：可以在不改任何 C++/C# 源码的前提下完成核心适配。** 需要的动作集中在：安装工具链、重建依赖、改造 `.nsi` 打包脚本、调整 `.csproj` 位数配置。唯一可能被迫动源码的是 WeaselDeployer/WeaselSetup 的 ARM64 原生化（可以不做，用 Win32 模拟顶）。
3. **最关键的两个硬阻塞**：① **本机没有任何 ARM64 编译器**；② **t9s2t 的语音引擎 x64 绑定**（但上游已有 ARM64 预编译，属于"要准备文件"而不是"要写代码"）。
4. **最大的非技术风险是"没有真机可验证"**——建议在投入 P2 之前先把这件事解决；否则建议只做到 P1，其余等有设备再说。
5. **建议的下一步**（如果决定推进）：
   - 先确认目标用户群里是否真的存在 ARM64 设备（决定要不要做）；
   - 若要做，从 **P0 装工具链并跑通 `build.bat arm64`** 开始，这一步就能暴露 80% 的未知；
   - 备好一台 Windows 11 ARM64 设备（或虚拟机）用于验证。
