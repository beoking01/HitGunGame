Giới thiệu dự án
Tên dự án: Game bắn súng lấy bảo vật

Mô tả ngắn:
Người chơi vào vai một tên cướp chuyên đột nhập để lấy lại những món bảo vật đã bị thế lực xấu chiếm giữ. 
Trong quá trình thực hiện nhiệm vụ, người chơi phải vượt qua hệ thống phòng thủ gồm các robot vũ trang canh giữ khu vực. 
Mục tiêu là thu thập đủ số lượng vật phẩm hoặc tài nguyên cần thiết, tiêu diệt hoặc né tránh kẻ địch và thoát ra an toàn để chiến thắng.

Ý tưởng gameplay
Người chơi di chuyển trong bản đồ, quan sát và tương tác với vật thể.
Khi gặp robot bảo vệ, người chơi dùng súng để chiến đấu.
Người chơi có hệ thống máu, đạn trong băng, đạn dự trữ và nạp đạn.
Một số vật thể có thể tương tác để lấy tiền/bảo vật hoặc kích hoạt sự kiện.
Khi thu thập đủ điều kiện chiến thắng, game hiển thị màn hình thắng; nếu hết máu sẽ thua.
Các cơ chế này khớp với các script hiện có trong repo như tương tác bằng raycast, inventory, shooting, health bar và game over/game win.

Chức năng chính của dự án
Điều khiển người chơi
Nhìn xung quanh, di chuyển, tương tác với vật thể.
Hệ thống chiến đấu
Súng có tốc độ bắn, độ tản đạn, số đạn mỗi phát, băng đạn và thời gian nạp.
Hệ thống máu
Người chơi và kẻ địch đều có thanh máu và nhận sát thương.
Kẻ địch robot
Dự án có module riêng cho enemy, state machine, patrol path và ability, cho thấy robot địch đã được tổ chức theo hướng AI tuần tra/trạng thái.

Quản lý game
Có menu, các level, âm thanh, trạng thái thắng/thua.
