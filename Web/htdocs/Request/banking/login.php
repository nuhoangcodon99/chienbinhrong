<?php
session_start();

require_once('../../KhanhDTK/cauhinh.php');
require_once('../../KhanhDTK/connect.php');
include('api.php');

// Kiểm tra xem key có được cung cấp không
if (!isset($_GET['users']) || $_GET['users'] !== $userloginmbbank_config) {
    // Nếu key không hợp lệ, có thể redirect hoặc hiển thị thông báo lỗi
    exit('Không tìm thấy key! Không thể truy cập.');
}

$mbbank = new MBBANK;
$userloginmbbank = $userloginmbbank_config;
$passmbbank = $passmbbank_config;
$stkmbbank = $stkmbbank_config;
$deviceId = $deviceIdCommon_goc_config;

// Kiểm tra dữ liệu đầu vào
if (empty($userloginmbbank) || empty($passmbbank) || empty($stkmbbank)) {
    exit('Vui lòng điền tài khoản đăng nhập, mật khẩu và số tài khoản');
}

$mbbank->user = $userloginmbbank;
$mbbank->pass = $passmbbank;
$time = time();

// Bypass captcha
$text_captcha = $mbbank->bypass_captcha_web2m('413145b2f6d981e32d0ee69a56b0e839');
$login = json_decode($mbbank->login($text_captcha), true);

// Kiểm tra lỗi login
if ($login['result']['message'] == "Capcha code is invalid" || $login['result']['message'] == 'Customer is invalid') {
    exit('Captcha không chính xác hoặc thông tin không chính xác');
}

// Sử dụng Prepared Statements để tránh SQL Injection
$KhanhDTK = $conn->prepare("SELECT userlogin FROM adminpanel WHERE userlogin = ?");
$KhanhDTK->execute([$userloginmbbank]);
$existingAccount = $KhanhDTK->fetch();

if ($existingAccount) {
    // Update thông tin tài khoản nếu đã tồn tại
    $KhanhDTK = $conn->prepare("UPDATE adminpanel SET stk = ?, name = ?, password = ?, sessionId = ?, deviceId = ?, token = ?, time = ? WHERE userlogin = ?");
    $KhanhDTK->execute([$stkmbbank, $login['cust']['nm'], $passmbbank, $login['sessionId'], $deviceId, CreateToken(), $time, $userloginmbbank]);

    if ($KhanhDTK->rowCount() > 0) {
        exit('Cập nhật tài khoản thành công');
    } else {
        exit('Lỗi khi cập nhật tài khoản');
    }
} else {
    // Thêm mới hoặc cập nhật tài khoản nếu không tồn tại
    $KhanhDTK = $conn->prepare("INSERT INTO adminpanel (userlogin, stk, name, password, sessionId, deviceId, token, time)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        ON DUPLICATE KEY UPDATE stk = VALUES(stk), name = VALUES(name), password = VALUES(password),
        sessionId = VALUES(sessionId), deviceId = VALUES(deviceId), token = VALUES(token), time = VALUES(time)");

    $KhanhDTK->execute([$userloginmbbank, $stkmbbank, $login['cust']['nm'], $passmbbank, $login['sessionId'], $deviceId, CreateToken(), $time]);

    if ($KhanhDTK->rowCount() > 0) {
        exit('Thêm mới/cập nhật tài khoản thành công');
    } else {
        exit('Lỗi khi thêm mới/cập nhật tài khoản');
    }
}