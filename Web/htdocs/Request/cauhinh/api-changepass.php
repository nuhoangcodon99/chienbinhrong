<?php
require_once '../../KhanhDTK/connect.php';
require_once '../../KhanhDTK/session.php';

$response = array(); // Khởi tạo mảng phản hồi

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $matKhauHienTai = $_POST['password'] ?? '';
    $matKhauMoi = $_POST['new_password'] ?? '';
    $xacNhanMatKhauMoi = $_POST['new_password_confirmation'] ?? '';

    if (empty($matKhauHienTai) || empty($matKhauMoi) || empty($xacNhanMatKhauMoi)) {
        $response['type'] = 'error';
        $response['message'] = 'Vui lòng điền đầy đủ thông tin trong form';
    } elseif ($matKhauHienTai !== $_password) {
        $response['type'] = 'error';
        $response['message'] = 'Sai mật khẩu hiện tại';
    } elseif ($matKhauMoi === $matKhauHienTai) {
        $response['type'] = 'error';
        $response['message'] = 'Mật khẩu mới không được giống mật khẩu hiện tại';
    } elseif ($matKhauMoi !== $xacNhanMatKhauMoi) {
        $response['type'] = 'error';
        $response['message'] = 'Mật khẩu mới không giống nhau';
    } else {
        // Cập nhật mật khẩu mới
        $stmt = $conn->prepare("UPDATE user SET password=:matKhauMoi WHERE username=:username");
        $stmt->bindParam(":matKhauMoi", $matKhauMoi);
        $stmt->bindParam(":username", $_username);

        if ($stmt->execute()) {
            $response['type'] = 'success';
            $response['message'] = 'Cập nhật mật khẩu mới thành công';
        } else {
            $response['type'] = 'error';
            $response['message'] = 'Lỗi khi cập nhật mật khẩu mới';
        }
    }

    // Trả về phản hồi dưới dạng JSON
    header('Content-Type: application/json');
    echo json_encode($response);
    exit;
}