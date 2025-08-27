# 🃏 Solitaire-Style Card Game (Unity)

This repository contains a **Solitaire-inspired card game** developed in Unity.  
The project started as a personal learning journey to explore **gameplay mechanics, user interface design, animations, and data-driven programming** in Unity. Over time, it evolved into a structured project with features such as drag-and-drop cards, animated menus, and sound effects.

The game provides a **classic single-player card experience** while showcasing how to combine Unity’s **UI system, ScriptableObjects, and DoTween animations** to build an interactive and polished game.

---

## ✨ Key Features

### 🎴 Card System
- A **complete deck of 52 cards** implemented through **ScriptableObjects**.  
- Two different card styles: **image-based cards** and **text-only cards**.  
- Card properties (rank, suit, icons, etc.) are stored as reusable data objects.  

### 🖱️ Gameplay Mechanics
- **Drag & Drop functionality**: cards follow the mouse and can be placed on valid zones.  
- **DropZone detection**: checks for valid moves and prevents invalid placement.  
- **Pile system**: supports stacked face-up and face-down cards with adjustable offsets.  

### 🎨 User Interface & Animations
- Built with Unity’s **Canvas system** for layered menus and HUD.  
- **DoTween-powered animations** for:
  - Smooth fade-in and fade-out transitions between menus.  
  - Animated settings panel sliding into view.  
- End-game **Game Over screen** with automatic triggers when the deck is cleared.  

### 🔊 Audio
- **Card movement SFX** and **win sound effects** integrated.  
- A **Sound Toggle Button** updates UI icons and saves state using `PlayerPrefs`.  
- **Game Over sound** plays automatically when the victory screen is shown.  

### ⚙️ Technical Highlights
- **ScriptableObjects** for clean, modular, and scalable card definitions.  
- **PlayerPrefs** to persist sound settings between sessions.  
- **CanvasGroup** and **RectTransform** for smooth UI control.  
- Unity project structured with clear separation between **Scripts, Prefabs, Sounds, and UI**.  

---

## 🎮 Gameplay Flow

1. At the start, **28 cards are dealt** into tableau piles, with the remaining cards in reserve.  
2. The player uses **mouse drag-and-drop** to move cards around.  
3. Cards can only be placed according to solitaire rules (e.g., alternating suits and descending ranks).  
4. The game continues until all cards are placed correctly.  
5. When the game ends, a **Game Over screen** fades in along with a sound effect.  

This flow replicates the **classic solitaire feel** while adding a custom design flavor.

---

