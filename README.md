# PM_Ban_Do_An_Nhanh

Ứng dụng Windows Forms (C#) — quản lý bán đồ ăn nhanh.

## Tổng quan
- Entry point: `PM_Ban_Do_An_Nhanh\Program.cs` (chạy `frmLogin`).
- Target framework: .NET Framework 4.8 (Project file `*.csproj`).
- Một số thư viện được commit trong `packages/` (BouncyCastle, iTextSharp).

## Hướng dẫn mở và chạy (Windows, Visual Studio)
1. Mở `PM_Ban_Do_An_Nhanh.sln` bằng Visual Studio 2019/2022 (hoặc bất kỳ phiên bản hỗ trợ .NET Framework 4.8).
2. Build solution (Ctrl+Shift+B).
3. Chạy (F5) — ứng dụng sẽ khởi động màn hình đăng nhập.

Lưu ý:
- Nếu Visual Studio yêu cầu restore packages, bạn có thể dùng NuGet Package Manager hoặc giữ nguyên `packages/` đã có sẵn.
- Cấu hình kết nối DB (nếu cần) nằm trong `App.config` — kiểm tra connection string trước khi chạy.

## Tệp quan trọng
- `PM_Ban_Do_An_Nhanh.sln` — solution.
- `PM_Ban_Do_An_Nhanh\PM_Ban_Do_An_Nhanh.csproj` — project.
- `PM_Ban_Do_An_Nhanh\Program.cs` — entry point.
- `App.config` — cấu hình, connection string.

Nếu bạn muốn, tôi có thể:
- Thêm hướng dẫn chi tiết để chạy trên máy khác (tạo installer hoặc hướng dẫn publish),
- Kiểm tra nhanh mã nguồn để tìm các hard-coded connection strings hoặc credentials,
- Chạy một build tự động hoặc thêm CI minimal.
