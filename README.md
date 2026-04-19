# 🎮 Hit Gun Game

![Game Header/Banner](docs/cover.gif)

> Gợi ý: thay `docs/cover.gif` bằng ảnh cover hoặc GIF gameplay thực tế của bạn.

## 📝 Giới thiệu
Hit Gun Game là game FPS góc nhìn thứ nhất, kết hợp bắn súng sinh tồn với cơ chế loot, mua bán và quản lý vật phẩm giữa nhiều scene. Người chơi vừa chiến đấu với AI Enemy, vừa thu thập vật phẩm để bán lấy điểm và chuẩn bị trang bị cho lượt chơi tiếp theo. Vòng lặp chính đi qua các scene Menu -> BaseCentral -> Mission (Military) với hệ thống lưu trạng thái liên scene.

## ✨ Tính năng nổi bật
- **Hệ thống Inventory 5 ô**
  - Chọn nhanh bằng phím `1`-`5`.
  - Nhặt và thả vật phẩm bằng `F`/`G`.
  - Hỗ trợ item thường, vũ khí và consumable (hồi máu).
- **Hệ thống vũ khí và đạn dược**
  - Vũ khí cấu hình bằng `ScriptableObject` (`GunData`).
  - Bắn, độ giật tản (spread), thay đạn, hiển thị ammo UI.
  - Dùng object pooling cho đạn để giảm Instantiate/Destroy liên tục.
- **AI Enemy theo State Machine**
  - Các trạng thái: `Patrol`, `Search`, `Attack`.
  - Dùng `NavMeshAgent`, tầm nhìn theo góc nhìn + raycast.
  - Có ability mở rộng (ví dụ `ImmortalAbility`).
- **Economy + gameplay loop**
  - Khu vực bán vật phẩm (`SellZone`) cộng điểm qua `PointManager`.
  - Mua vật phẩm từ máy tính (`ComputerUI`) theo giá trong `ItemData`.
- **Lưu trạng thái liên scene / liên phiên chơi**
  - Lưu điểm (`money_state.json`).
  - Lưu hàng trong xe (`truck_state.json`).
  - Lưu bố trí phòng (`room_state.json`).
  - Đồng bộ lưu qua `SaveGameFacade` khi chuyển scene/thoát game.
- **Scene loading bất đồng bộ**
  - Scene `Loading` hiển thị tiến trình và chờ người chơi nhấn phím để vào màn tiếp theo.
- **Rendering & hình ảnh**
  - Dự án dùng **URP** (`com.unity.render-pipelines.universal`) và Post Processing.

## 🛠 Công nghệ sử dụng
- **Engine:** Unity 6 (`6000.0.42f1`)
- **Ngôn ngữ:** C#
- **Input:** Unity Input System (`com.unity.inputsystem`)
- **AI Navigation:** `com.unity.ai.navigation`
- **Rendering:** URP + Post Processing
- **UI:** uGUI + TextMeshPro
- **Version Control:** Git

## 🗂 Cấu trúc chính
```text
Assets/
  Scenes/                # Menu, Loading, BaseCentral, Military, Level1, Level2
  Scripts/
    Enemy/               # Enemy AI, StateMachine, Ability
    GameLogic/           # Economy, Time, Truck/Room persistence
    Interactables/       # Computer, KeyPad, event interaction
    Manager/             # Game/Input/UI/Sound/Save facade
    Player/              # Movement, interact, inventory, actions, UI
    Weapons/             # GunData, Bullet, pooling
  Input/
    PlayerInput.inputactions
Packages/
  manifest.json          # Danh sách package Unity
ProjectSettings/
  EditorBuildSettings.asset
  ProjectVersion.txt
```

## 🎮 Điều khiển mặc định (Keyboard/Mouse)
- `W A S D` hoặc `Mũi tên`: Di chuyển
- `Chuột`: Nhìn
- `Space`: Nhảy
- `Left Ctrl`: Cúi
- `Left Shift`: Sprint
- `E`: Tương tác
- `Chuột trái`: Bắn / dùng vũ khí chính
- `R`: Thay đạn
- `F`: Nhặt item
- `G`: Thả item
- `1` `2` `3` `4` `5`: Đổi slot inventory

## 🚀 Cài đặt & Chạy thử
1. **Clone repo**
   ```bash
   git clone https://github.com/username/ten-game.git
   ```
2. **Mở project bằng Unity Hub**
   - Chọn đúng phiên bản Unity: **6000.0.42f1** (hoặc bản Unity 6 tương thích gần nhất).
3. **Mở scene để chạy**
   - Bắt đầu từ `Assets/Scenes/Menu.unity`.
4. **Play trong Editor**
   - Nhấn Play để chạy thử.

## 🧪 Build game (tuỳ chọn)
1. Vào **File > Build Settings**.
2. Kiểm tra scene trong build list:
   - `Menu`
   - `Loading`
   - `BaseCentral`
   - `Military`
3. Chọn platform, bấm **Build** hoặc **Build And Run**.

## 💾 Lưu dữ liệu
Game có cơ chế lưu dữ liệu runtime ra JSON qua các manager:
- `PointManager` (điểm)
- `TruckStateManager` (vật phẩm trong truck)
- `RoomStateManager` (bố trí phòng)

Các trạng thái này được gọi lưu qua `SaveGameFacade` khi chuyển màn hoặc thoát game.

## 📌 Ghi chú phát triển
- Dự án hiện dùng Unity Input System và một phần thao tác phím trực tiếp trong script (`F/G`, chọn slot `1-5`).
- Nếu bạn muốn public repo, nên bổ sung:
  - ảnh/video gameplay thật trong thư mục `docs/`
  - giấy phép (LICENSE)
  - phần Credits cho asset âm thanh/mô hình nếu có.
