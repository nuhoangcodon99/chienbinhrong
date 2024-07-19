<?php
require_once '../../KhanhDTK/connect.php';
require_once '../../KhanhDTK/session.php';
require '../../vendor/autoload.php';
require '../../vendor/phpmailer/phpmailer/src/Exception.php';
require '../../vendor/phpmailer/phpmailer/src/PHPMailer.php';
require '../../vendor/phpmailer/phpmailer/src/SMTP.php';
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

function generateRandomPassword($length = 6)
{
    $characters = '0123456789';
    $password = '';

    for ($i = 0; $i < $length; $i++) {
        $password .= $characters[rand(0, strlen($characters) - 1)];
    }

    return $password;
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $username = $_POST['username'];
    $gmail = $_POST['gmail'];

    try {
        // Kiểm tra tên người dùng và gmail trong cơ sở dữ liệu
        $checkQuery = "SELECT gmail FROM user WHERE username = :username";
        $stmt = $conn->prepare($checkQuery);
        $stmt->bindValue(':username', $username, PDO::PARAM_STR);
        $stmt->execute();
        $storedGmail = $stmt->fetchColumn();
        $stmt->closeCursor();

        if ($storedGmail === $gmail) {
            // Tạo một mật khẩu mới ngẫu nhiên
            $newPassword = generateRandomPassword();

            // Cập nhật mật khẩu mới vào cơ sở dữ liệu
            $updateQuery = "UPDATE user SET password = :newPassword WHERE username = :username";
            $stmt = $conn->prepare($updateQuery);
            $stmt->bindValue(':newPassword', $newPassword, PDO::PARAM_STR);
            $stmt->bindValue(':username', $username, PDO::PARAM_STR);
            $stmt->execute();
            $stmt->closeCursor();

            // Tạo một đối tượng PHPMailer và cấu hình
            $mail = new PHPMailer(true);

            // Cấu hình gửi email thông qua Gmail
            $mail->SMTPDebug = 0;
            $mail->isSMTP();
            $mail->Host = 'smtp.gmail.com';
            $mail->SMTPAuth = true;
            $mail->Username = 'ngocrongdragonking@gmail.com'; // Thay đổi thành email của bạn
            $mail->Password = 'erlthhyxknomaxgx'; // Thay đổi thành mật khẩu của bạn
            $mail->SMTPSecure = 'tls';
            $mail->Port = 587;

            // Thiết lập thông tin email
            $mail->setFrom('ngocrongdragonking@gmail.com', 'Ngọc Rồng DragonKing'); // Thay đổi thành email của bạn và tên của bạn
            $mail->addAddress($gmail);
            $mail->Subject = 'Quên Mật Khẩu - Ngọc Rồng DragonKing';
            $mail->Body = "Xin chào bạn,\n\nTài khoản $username đang thực hiện Quên mật khẩu.\n\nThông tin tài khoản của bạn:\n- Tài khoản: $username \n- Mật khẩu mới: $newPassword \n\nAdmin chân thành cảm ơn bạn đã tin tưởng và đồng hành cùng " . $_tenmaychu . "!\n\n" . $_tenmaychu;
            $mail->CharSet = 'UTF-8';
            $mail->Encoding = 'base64';

            // Gửi thư
            $mail->send();
            $response['type'] = 'success';
            $response['message'] = '<br>Email đã được gửi thành công đến địa chỉ: ' . $gmail;
        } else {
            $response['type'] = 'info';
            $response['message'] = '<br>Không tìm thấy Gmail của người dùng với tài khoản: ' . $username;
        }
    } catch (Exception $e) {
        $response['type'] = 'error';
        $response['message'] = '<br>Có lỗi xảy ra: ' . $e->getMessage();
    }
}