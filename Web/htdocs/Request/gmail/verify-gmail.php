<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

require_once '../../KhanhDTK/session.php';
require_once '../../KhanhDTK/connect.php';
require '../../vendor/autoload.php';
require '../../vendor/phpmailer/phpmailer/src/Exception.php';
require '../../vendor/phpmailer/phpmailer/src/PHPMailer.php';
require '../../vendor/phpmailer/phpmailer/src/SMTP.php';

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

function generateRandomCode($length = 32)
{
    return bin2hex(random_bytes($length));
}

function getuserDetails($username, $conn)
{
    $stmt = $conn->prepare("SELECT xacminh, thoigian_xacminh, gmail FROM user WHERE username = :username");
    $stmt->bindParam(":username", $username);
    $stmt->execute();
    $row = $stmt->fetch(PDO::FETCH_ASSOC);
    $stmt->closeCursor();
    return $row;
}

function updateuserVerificationStatus($username, $conn)
{
    $stmt = $conn->prepare("UPDATE user SET gmail = NULL, xacminh = 0, thoigian_xacminh = 0 WHERE username = :username");
    $stmt->bindParam(":username", $username);
    $stmt->execute();
    $stmt->closeCursor();
}

function redirectWithMessage($message, $messageClass)
{
    $url = "../cap-nhat-thong-tin.php?message=" . urlencode($message) . "&messageClass=" . urlencode($messageClass);
    header("Location: $url");
    exit;
}

// Kiểm tra xem mã xác nhận có hợp lệ hay không (thông qua cơ sở dữ liệu hoặc cách khác)
$isCodeValid = true; // Giả sử mã xác nhận hợp lệ

if ($isCodeValid) {
    $userDetails = getuserDetails($_username, $conn);

    $xacminh = $userDetails['xacminh'];
    $thoigian_xacminh = $userDetails['thoigian_xacminh'];
    $gmail = $userDetails['gmail'];

    // Kiểm tra trạng thái xác minh, thời gian xác minh và giá trị cột gmail
    if ($xacminh != 0 && $thoigian_xacminh != 0) {
        // Xác nhận thành công, xóa dữ liệu trong cột gmail và cập nhật trạng thái xác minh và thời gian xác minh
        updateuserVerificationStatus($_username, $conn);

        // Đóng kết nối đến cơ sở dữ liệu
        $conn = null;

        $message = "Xác nhận thành công! Dữ liệu trong cột gmail đã được xóa và trạng thái xác minh đã được cập nhật.<br>";
        $messageClass = "success";

        redirectWithMessage($message, $messageClass);
    } else {
        // Kiểm tra giá trị cột gmail
        if ($gmail === null) {
            $message = "Tài khoản chưa cập nhật thông tin!<br>";
        } else {
            $message = "Tài khoản không đủ điều kiện để sử dụng chức năng.<br>";
        }
        $messageClass = "error";

        redirectWithMessage($message, $messageClass);
    }
} else {
    $message = "Mã xác nhận không hợp lệ.<br>";
    $messageClass = "error";

    redirectWithMessage($message, $messageClass);
}
