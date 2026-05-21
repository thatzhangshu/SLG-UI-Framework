# 🎮 SLG UI Framework Demo

> 基于 Unity 的企业级 SLG UI 架构与 World UI 管理 Demo

---

# 📌 项目简介

本项目是一个基于 Unity 开发的 SLG（策略类游戏）UI 框架 Demo，目标是模拟：

> 企业级 SLG 项目的 UI 架构设计与 World UI 管理方案

项目结合个人在网易《率土之滨》相关 UI 系统开发经验进行设计与拆分，重点关注：

- 🧱 大型 SLG UI 工程架构
- 🖥️ 通用弹窗与主界面系统
- 📜 ScrollView / 聊天系统 / 卡牌列表
- ♻️ UI 生命周期与层级管理
- ⚡ UI 性能优化
- 🌍 大地图海量 World UI 管理
- 🪖 行军名牌 / 城池名牌 / 血条系统
- 🧩 对象池与可见性裁剪

---

# 🎯 项目定位

本项目定位为：

> 工程化 SLG UI Demo

而不仅仅是简单的 UI 展示。

重点体现：

- Unity 客户端工程能力
- SLG UI 架构设计能力
- 大地图 World UI 管理能力
- 企业级 UI 性能优化思路

---

# 🧭 项目目标

---

# 🖥️ 第一部分：通用 UI / 弹窗系统

主要模拟：

> SLG 中与地图无关的常规 UI 系统

---

## ✨ 功能范围

- 🏠 主界面 HUD
- 📦 通用弹窗系统
- 🌫️ 背景模糊弹窗
- 📧 邮件系统
- 💬 聊天系统
- 🃏 武将卡牌系统
- 🎒 背包系统
- 📜 ScrollView 列表系统
- 🔘 通用按钮与界面模板

---

## 🧠 技术重点

- UIManager 架构
- 多层级 Canvas 管理
- UI 生命周期管理
- UI 资源复用
- ScrollView 虚拟列表
- 聊天列表性能优化
- UIItem 对象池
- Canvas 重建优化

---

# 🌍 第二部分：地图 World UI 系统

主要模拟：

> SLG 大地图中的世界空间 UI 管理

---

## ✨ 功能范围

- 🏰 城池名牌
- 🌲 资源点名牌
- 🪖 行军部队名牌
- ❤️ 血条系统
- 🛡️ 联盟标识
- 📍 地图状态图标
- ⚠️ 世界事件提示

---

## 🧠 技术重点

- WorldUIManager
- Screen Space 与世界坐标转换
- 海量名牌性能优化
- 可见性裁剪
- 屏幕外隐藏
- 分帧刷新
- 名牌对象池
- Camera 缩放联动

---

# 🏗️ 项目架构（规划中）

```plaintext
Assets
├── Scripts
│   ├── Framework
│   │   ├── UI
│   │   ├── WorldUI
│   │   ├── Event
│   │   ├── Pool
│   │   └── Manager
│   │
│   ├── Gameplay
│   └── Demo
│
├── Prefabs
│   ├── UI
│   └── WorldUI
│
├── Art
├── Materials
├── Animations
└── Scenes
````

---

# 🧰 技术关键词

---

## 🎨 UI 系统

* Canvas Layer 管理
* RectTransform
* EventSystem
* GraphicRaycaster
* ScrollView
* 虚拟列表
* UI 生命周期
* 通用弹窗架构

---

## ⚡ 性能优化

* Object Pool
* Canvas 拆分
* UIItem 复用
* 分帧刷新
* GC 控制
* 海量 UI 管理

---

## 🌍 World UI

* WorldToScreenPoint
* 世界 UI 跟随
* 名牌系统
* 行军 UI
* 可见性裁剪
* Camera 缩放联动

---

# 🚀 当前开发计划

---

# ✅ Phase 1

* UIManager 基础框架
* 多层级 Canvas
* 通用弹窗系统
* ScrollView Demo

---

# 🔄 Phase 2

* 聊天系统
* 虚拟列表
* UIItem 对象池
* UI 性能优化

---

# 🌍 Phase 3

* WorldUIManager
* 城池名牌系统
* 行军部队名牌
* 血条系统

---

# ⚡ Phase 4

* 海量名牌优化
* 可见性裁剪
* Camera 缩放联动
* 分帧刷新

---

# 🎯 项目适用方向

本项目适用于：

* Unity 客户端开发
* 游戏 UI 系统开发
* SLG 客户端岗位
* 技术策划
* UI 技术方向

---

# 💻 开发环境

| 项目              | 版本         |
| --------------- | ---------- |
| Unity           | 2022.3 LTS |
| Render Pipeline | URP        |
| Language        | C#         |

---

# 📈 后续计划

后续将逐步加入：

* ✨ UI 动画系统
* 📦 Addressables
* 🌍 世界地图交互
* 🗺️ 小地图系统
* ⚔️ 战斗 HUD
* 💥 UI 特效管理
* 📱 多分辨率适配
* 🌐 网络同步演示

---

# 📸 项目展示

> 后续将持续补充：

* UI 截图
* 功能演示 GIF
* World UI 效果展示
* ScrollView 性能演示
* 大地图名牌效果

---

# 🧠 项目核心目标

本项目重点展示：

* 🧱 Unity 客户端工程能力
* 🌍 SLG UI 架构设计能力
* ⚡ 大地图 UI 性能优化能力
* 🪖 海量 World UI 管理能力
* 🧩 工程化 UI 开发思维

---

# 👤 作者

基于个人 SLG 项目经验进行设计与实现。

持续更新中。 🚀

