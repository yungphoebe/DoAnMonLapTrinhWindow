# Hướng dẫn sử dụng chức năng tạo danh sách công việc

## Tổng quan
Chức năng tạo danh sách công việc cho phép người dùng tạo và quản lý các project (danh sách công việc) với đầy đủ tính năng CRUD.

## Các tính năng chính

### 1. Tạo danh sách công việc mới
- **Cách sử dụng**: Nhấn vào card "TẠO DANH SÁCH" trên giao diện chính
- **Tính năng**:
  - Nhập tên danh sách
  - Chọn màu sắc cho danh sách
  - Upload icon tùy chỉnh (tùy chọn)
  - Tự động lưu vào database

### 2. Xem danh sách các project
- **Hiển thị**: Tất cả project của user được hiển thị dưới dạng card
- **Thông tin hiển thị**:
  - Tên project
  - Màu sắc project
  - Số công việc đang chờ
  - Thời gian dự kiến
  - Danh sách 5 công việc đầu tiên

### 3. Quản lý chi tiết project
- **Cách mở**: Nhấn vào card project bất kỳ
- **Tính năng**:
  - Xem thông tin project
  - Xem danh sách tất cả công việc
  - Thêm công việc mới
  - Đánh dấu hoàn thành công việc
  - Xóa công việc

### 4. Thêm công việc mới
- **Cách sử dụng**: Trong form chi tiết project, nhấn "Thêm công việc"
- **Thông tin cần nhập**:
  - Tiêu đề công việc (bắt buộc)
  - Mô tả chi tiết
  - Độ ưu tiên (Low/Medium/High)
  - Trạng thái (Pending/In Progress/Completed)
  - Hạn chót (tùy chọn)
  - Thời gian dự kiến (phút)

## Cấu trúc dữ liệu

### Project (Danh sách công việc)
- `ProjectId`: ID duy nhất
- `UserId`: ID người dùng sở hữu
- `ProjectName`: Tên danh sách
- `Description`: Mô tả
- `ColorCode`: Mã màu (HTML)
- `CreatedAt`: Ngày tạo
- `IsArchived`: Trạng thái lưu trữ

### Task (Công việc)
- `TaskId`: ID duy nhất
- `ProjectId`: ID danh sách chứa
- `UserId`: ID người dùng sở hữu
- `Title`: Tiêu đề công việc
- `Description`: Mô tả chi tiết
- `Priority`: Độ ưu tiên
- `Status`: Trạng thái
- `DueDate`: Hạn chót
- `EstimatedMinutes`: Thời gian dự kiến
- `CreatedAt`: Ngày tạo
- `IsDeleted`: Trạng thái xóa

## Luồng hoạt động

1. **Tạo project mới**:
   ```
   Form1 → CreateListForm → Lưu vào DB → Cập nhật UI
   ```

2. **Xem chi tiết project**:
   ```
   Form1 → ProjectDetailsForm → Hiển thị tasks
   ```

3. **Thêm task mới**:
   ```
   ProjectDetailsForm → AddTaskForm → Lưu vào DB → Cập nhật UI
   ```

## Xử lý lỗi

- **Lỗi kết nối database**: Hiển thị thông báo lỗi và cho phép thử lại
- **Lỗi validation**: Kiểm tra dữ liệu đầu vào trước khi lưu
- **Lỗi lưu dữ liệu**: Hiển thị thông báo lỗi chi tiết

## Cải tiến trong tương lai

1. **Chỉnh sửa project**: Cho phép sửa tên, màu sắc, mô tả
2. **Chỉnh sửa task**: Form chỉnh sửa task chi tiết
3. **Tìm kiếm và lọc**: Tìm kiếm project/task theo từ khóa
4. **Phân quyền**: Quản lý quyền truy cập project
5. **Thông báo**: Nhắc nhở hạn chót công việc
6. **Báo cáo**: Thống kê tiến độ công việc

## Lưu ý kỹ thuật

- Sử dụng Entity Framework Core để truy cập database
- Async/await pattern cho các thao tác database
- Dispose pattern để quản lý tài nguyên
- Error handling toàn diện
- UI responsive và user-friendly
