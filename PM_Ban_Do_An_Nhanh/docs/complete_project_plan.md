# Kế hoạch Toàn diện Dự án Quản lý Bán Đồ Ăn Nhanh

## 1. Tổng quan Dự án

### 1.1 Mục tiêu
Xây dựng hệ thống quản lý bán đồ ăn nhanh hoàn chỉnh với các chức năng:
- Quản lý thực đơn (món ăn, danh mục, hình ảnh)
- Bán hàng tại quầy (POS)
- Quản lý khách hàng
- Báo cáo doanh thu
- Phân quyền Admin/Nhân viên
- Quản lý tồn kho (trạng thái còn/hết hàng)

### 1.2 Công nghệ
- **Platform**: .NET Framework 4.8, WinForms
- **Database**: SQL Server (localhost, Windows Authentication)
- **Architecture**: 3-layer (DAL, BLL, UI)
- **Libraries**: iTextSharp (PDF), BouncyCastle (Crypto)

---

## 2. Kiến trúc Hệ thống

### 2.1 Cấu trúc thư mục
```
PM_Ban_Do_An_Nhanh/
├── DAL/              # Data Access Layer
├── BLL/              # Business Logic Layer
├── Entities/         # Data models
├── Helpers/          # Utility classes (ImageHelper)
├── Utils/            # UI helpers (ButtonStyleHelper)
├── UI/               # Custom controls (TableStyleHelper)
├── Images/           # Lưu trữ hình ảnh món ăn
│   └── MenuItems/
└── docs/             # Tài liệu dự án
```

### 2.2 Database Schema
**Bảng chính:**
- `TaiKhoan` (MaTK, TenDangNhap, MatKhau, Role, HoTen, Email)
- `DanhMuc` (MaDM, TenDM, MoTa)
- `MonAn` (MaMon, TenMon, Gia, MaDM, TrangThai, HinhAnh)
- `KhachHang` (MaKH, TenKH, SDT, DiaChi, DiemTichLuy)
- `DonHang` (MaDH, MaKH, MaTK, NgayLap, TongTien, TrangThai)
- `ChiTietDonHang` (MaCTDH, MaDH, MaMon, SoLuong, DonGia)

---

## 3. Roadmap Phát triển (A → Z)

### **GIAI ĐOẠN 1: Chuẩn bị & Thiết lập (Tuần 1)**

#### 1.1 Môi trường phát triển
- [x] Cài đặt Visual Studio 2022, SQL Server
- [x] Clone/setup repository
- [x] Tạo database `FastFood` và chạy script khởi tạo
- [x] Kiểm tra kết nối DB

#### 1.2 Thiết kế Database
- [ ] Review và hoàn thiện ERD
- [ ] Thêm cột `Role` vào bảng `TaiKhoan`
- [ ] Tạo stored procedures (nếu cần)
- [ ] Seed dữ liệu mẫu (admin, danh mục, món ăn)

#### 1.3 Cấu trúc dự án
- [x] Tổ chức thư mục DAL/BLL/Entities
- [x] Tạo DBConnection class
- [ ] Thiết lập logging framework (NLog/log4net)

---

### **GIAI ĐOẠN 2: Core Features (Tuần 2-3)**

#### 2.1 Quản lý Danh mục
- [x] CRUD danh mục món ăn
- [x] Validation input
- [x] UI với DataGridView

#### 2.2 Quản lý Món ăn
- [x] CRUD món ăn
- [x] Upload & quản lý hình ảnh (ImageHelper)
- [x] Hiển thị preview hình ảnh trong grid
- [x] Badge "Hết hàng" và highlight
- [ ] Tìm kiếm/lọc theo danh mục
- [ ] Import/Export danh sách món (Excel/CSV)

#### 2.3 Quản lý Khách hàng
- [x] CRUD khách hàng
- [x] Tìm kiếm theo SĐT
- [ ] Lịch sử mua hàng
- [ ] Điểm tích lũy & khuyến mãi

---

### **GIAI ĐOẠN 3: Bán hàng (POS) (Tuần 4-5)**

#### 3.1 Giao diện bán hàng
- [x] Hiển thị menu dạng card (MenuItemCard)
- [x] Badge & disable món hết hàng
- [x] Thêm món vào đơn hàng
- [x] Điều chỉnh số lượng (+/-)
- [ ] Tìm kiếm nhanh món ăn
- [ ] Lọc theo danh mục (tab ngang)

#### 3.2 Xử lý đơn hàng
- [x] Tính tổng tiền
- [x] Chọn khách hàng (hoặc khách lẻ)
- [x] Thanh toán & lưu đơn
- [ ] In hóa đơn (thermal printer)
- [ ] Hủy đơn với xác nhận
- [ ] Áp dụng giảm giá/voucher

#### 3.3 Lịch sử đơn hàng
- [x] Hiển thị danh sách hóa đơn
- [x] Xem chi tiết đơn
- [ ] In lại hóa đơn
- [x] Export PDF
- [ ] Lọc theo ngày/khách hàng

---

### **GIAI ĐOẠN 4: Báo cáo & Thống kê (Tuần 6)**

#### 4.1 Dashboard
- [ ] Doanh thu hôm nay/tuần/tháng
- [ ] Top món bán chạy
- [ ] Số đơn hàng
- [ ] Biểu đồ xu hướng (Chart control)

#### 4.2 Báo cáo chi tiết
- [ ] Báo cáo doanh thu theo khoảng thời gian
- [ ] Báo cáo theo món ăn
- [ ] Báo cáo theo nhân viên
- [ ] Export Excel/PDF

#### 4.3 Quản lý tồn kho
- [ ] Cảnh báo món sắp hết
- [ ] Lịch sử thay đổi trạng thái
- [ ] Đồng bộ tồn kho realtime

---

### **GIAI ĐOẠN 5: Phân quyền & Bảo mật (Tuần 7)**

#### 5.1 Hệ thống đăng nhập
- [x] Form đăng nhập
- [x] Mã hóa mật khẩu (BCrypt/SHA256)
- [ ] Quên mật khẩu (email reset)
- [ ] Khóa tài khoản sau N lần sai

#### 5.2 Role-based Access Control
- [ ] Thêm cột `Role` vào DB
- [ ] SessionContext lưu user hiện tại
- [ ] Ẩn/hiện menu theo role
- [ ] Kiểm tra quyền ở mỗi form
- [ ] Audit trail (log thao tác admin)

#### 5.3 Quản lý tài khoản
- [ ] Form quản lý user (Admin only)
- [ ] Thêm/sửa/khóa tài khoản
- [ ] Đặt role (Admin/Staff)
- [ ] Đổi mật khẩu

---

### **GIAI ĐOẠN 6: UI/UX Nâng cao (Tuần 8)**

#### 6.1 Cải thiện giao diện
- [x] Modern button styles (ButtonStyleHelper)
- [x] DataGridView styling
- [ ] Dark mode toggle
- [ ] Responsive layout (resize)
- [ ] Animation & transitions

#### 6.2 Trải nghiệm người dùng
- [ ] Keyboard shortcuts (F1-F12)
- [ ] Touch-friendly (tablet mode)
- [ ] Tooltip & help text
- [ ] Loading indicators
- [ ] Error handling thân thiện

#### 6.3 Accessibility
- [ ] High contrast mode
- [ ] Font size adjustment
- [ ] Screen reader support (nếu cần)

---

### **GIAI ĐOẠN 7: Tối ưu & Performance (Tuần 9)**

#### 7.1 Database
- [ ] Index các cột thường query
- [ ] Optimize stored procedures
- [ ] Connection pooling
- [ ] Backup tự động

#### 7.2 Application
- [ ] Lazy loading hình ảnh
- [ ] Cache danh sách món/danh mục
- [ ] Async/await cho I/O operations
- [ ] Memory leak detection

#### 7.3 Image Management
- [x] Resize ảnh khi upload
- [x] FileStream để tránh lock
- [ ] Thumbnail generation
- [ ] Cleanup ảnh không dùng

---

### **GIAI ĐOẠN 8: Testing (Tuần 10)**

#### 8.1 Unit Testing
- [ ] Test DAL methods
- [ ] Test BLL logic
- [ ] Mock database

#### 8.2 Integration Testing
- [ ] Test workflow bán hàng
- [ ] Test báo cáo
- [ ] Test phân quyền

#### 8.3 User Acceptance Testing
- [ ] Test với nhân viên thực tế
- [ ] Thu thập feedback
- [ ] Fix bugs & refine UX

---

### **GIAI ĐOẠN 9: Deployment (Tuần 11)**

#### 9.1 Chuẩn bị
- [ ] Tạo installer (ClickOnce/WiX)
- [ ] Hướng dẫn cài đặt
- [ ] Script setup database
- [ ] Backup & restore guide

#### 9.2 Triển khai
- [ ] Deploy lên máy thực tế
- [ ] Cấu hình SQL Server
- [ ] Import dữ liệu ban đầu
- [ ] Training nhân viên

#### 9.3 Monitoring
- [ ] Log errors to file
- [ ] Theo dõi performance
- [ ] Hotfix nếu cần

---

### **GIAI ĐOẠN 10: Bảo trì & Mở rộng (Ongoing)**

#### 10.1 Bảo trì
- [ ] Backup database định kỳ
- [ ] Update security patches
- [ ] Review logs
- [ ] Optimize khi cần

#### 10.2 Tính năng mở rộng
- [ ] Kiosk tự order (self-service)
- [ ] Mobile app (Xamarin/MAUI)
- [ ] Web dashboard
- [ ] Tích hợp thanh toán online
- [ ] Multi-store support
- [ ] Kitchen display system (KDS)
- [ ] Loyalty program nâng cao

---

## 4. Checklist Chất lượng

### 4.1 Code Quality
- [ ] Tuân thủ naming conventions
- [ ] Comment code phức tạp
- [ ] Refactor code trùng lặp
- [ ] Code review

### 4.2 Security
- [ ] SQL injection prevention (parameterized queries)
- [ ] XSS prevention (nếu có web component)
- [ ] Mã hóa mật khẩu
- [ ] Validate tất cả input

### 4.3 Documentation
- [ ] README.md
- [ ] API documentation
- [ ] User manual
- [ ] Developer guide

---

## 5. Rủi ro & Giải pháp

| Rủi ro | Mức độ | Giải pháp |
|---------|--------|-----------|
| Database connection fail | Cao | Retry logic, fallback offline mode |
| Image file lock | Trung bình | FileStream + clone bitmap (đã fix) |
| Performance khi nhiều món | Trung bình | Lazy loading, pagination |
| Nhân viên quên mật khẩu | Cao | Reset password workflow |
| Mất điện khi đang bán | Cao | Auto-save draft orders |

---

## 6. Timeline Tổng thể

```
Tuần 1:  Chuẩn bị & Setup
Tuần 2-3: Core Features (Danh mục, Món ăn, Khách hàng)
Tuần 4-5: POS & Bán hàng
Tuần 6:   Báo cáo & Dashboard
Tuần 7:   Phân quyền & Bảo mật
Tuần 8:   UI/UX Nâng cao
Tuần 9:   Tối ưu & Performance
Tuần 10:  Testing
Tuần 11:  Deployment
Tuần 12+: Bảo trì & Mở rộng
```

---

## 7. Tài liệu Tham khảo

- [Toast POS](https://pos.toasttab.com/) - UI/UX inspiration
- [Square for Restaurants](https://squareup.com/us/en/restaurants) - Workflow reference
- [Material Design Guidelines](https://material.io/design) - UI patterns
- [SCAMPER Framework](docs/scamper_analysis.md) - Innovation ideas
- [Design Thinking Process](docs/design_thinking.md) - User-centered design

---

## 8. Kết luận

Kế hoạch này cung cấp lộ trình chi tiết từ A đến Z để phát triển, triển khai và bảo trì hệ thống quản lý bán đồ ăn nhanh. Mỗi giai đoạn có checklist cụ thể, giúp theo dõi tiến độ và đảm bảo chất lượng sản phẩm cuối cùng.

**Ưu tiên hiện tại:**
1. Hoàn thiện phân quyền Admin/Staff
2. Cải thiện UI/UX (dark mode, keyboard shortcuts)
3. Tối ưu performance (caching, async)
4. Testing toàn diện trước khi deploy
