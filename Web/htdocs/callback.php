<?php
require_once 'KhanhDTK/head.php';
define('POINTS_PER_TOPUP', 1);

// Lấy dữ liệu từ request
$txtBody = file_get_contents('php://input');
$jsonBody = json_decode($txtBody);

if (isset($jsonBody->callback_sign)) {
    $callback_sign = md5($partner_key . $jsonBody->code . $jsonBody->serial);

    if ($jsonBody->callback_sign == $callback_sign) {
        // Display received data for debugging
        $getdata['status'] = $jsonBody->status;
        $getdata['message'] = $jsonBody->message;
        $getdata['request_id'] = $jsonBody->request_id;
        $getdata['trans_id'] = $jsonBody->trans_id;
        $getdata['declared_value'] = $jsonBody->declared_value;
        $getdata['value'] = $jsonBody->value;
        $getdata['amount'] = $jsonBody->amount;
        $getdata['code'] = $jsonBody->code;
        $getdata['serial'] = $jsonBody->serial;
        $getdata['telco'] = $jsonBody->telco;
        print_r($getdata);

        // Kiểm tra nếu đối chứng chữ ký hợp lệ
        $code = $jsonBody->code;
        $serial = $jsonBody->serial;

        // Kiểm tra xem có dữ liệu người dùng trong bảng "napthe" hay không
        $get_user_nap_sql = "SELECT * FROM napthe WHERE code = :code AND serial = :serial";
        $stmt = $conn->prepare($get_user_nap_sql);
        $stmt->bindParam(':code', $code);
        $stmt->bindParam(':serial', $serial);
        $stmt->execute();

        if ($stmt->rowCount() > 0) {
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            $user_nap = $row['user_nap'];
            $price = $row['amount'] * $giatri;

            // Tính điểm tương ứng với mỗi lần nạp thẻ
            $tichdiem = ($price > 100000) ? floor($price / 100000) * POINTS_PER_TOPUP : 0;
            $lixi = ($price > 10000) ? floor($price / 10000) * 10 : 0;

            // Tiến hành câu truy vấn UPDATE trong bảng "napthe" để cập nhật trạng thái (status)
            $update_status_sql = "UPDATE napthe SET status = :status WHERE code = :code AND serial = :serial";
            $stmt = $conn->prepare($update_status_sql);
            $stmt->bindParam(':status', $jsonBody->status);
            $stmt->bindParam(':code', $code);
            $stmt->bindParam(':serial', $serial);
            $stmt->execute();

            if ($stmt->execute()) {
                if ($jsonBody->status == 1) {
                    // Cập nhật cột "coin" và "vnd" trong bảng "account"
                    $update_account_sql = "UPDATE user SET mocnap = mocnap + :tichdiem, vnd = vnd + :price, tongnap = tongnap + :price WHERE username = :user_nap";
                    $stmt = $conn->prepare($update_account_sql);
                    $stmt->bindParam(':tichdiem', $tichdiem);
                    $stmt->bindParam(':price', $price);
                    $stmt->bindParam(':user_nap', $user_nap);
                    $stmt->execute();
                }
            }
        } else {
            // Ghi thông báo lỗi vào file "error.txt"
            $error_message = "Không tìm thấy dữ liệu người dùng trong bảng 'napthe' với code: $code và serial: $serial";
            file_put_contents('error.txt', $error_message, FILE_APPEND);
        }
    } else {
        // Ghi thông báo lỗi vào file "error.txt"
        $error_message = "Chữ ký không hợp lệ";
        file_put_contents('error.txt', $error_message, FILE_APPEND);
    }
} else {
    // Ghi thông báo lỗi vào file "error.txt"
    $error_message = "Thiếu thông tin callback_sign";
    file_put_contents('error.txt', $error_message, FILE_APPEND);
}