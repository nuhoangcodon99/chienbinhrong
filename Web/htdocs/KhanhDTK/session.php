<?php

// Kiểm tra xem phiên làm việc đã được khởi động hay chưa
if (session_status() == PHP_SESSION_NONE) {
    // Nếu chưa khởi động, tiến hành khởi động phiên làm việc
    session_start();
}
require_once 'connect.php';
date_default_timezone_set('Asia/Ho_Chi_Minh');

function fetchUserData($conn, $username)
{
    $stmt = $conn->prepare("SELECT * FROM user WHERE username = :username");
    $stmt->bindParam(":username", $username);
    $stmt->execute();
    $user_arr = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$user_arr) {
        header("Location: /pages/logout");
        exit();
    }

    $user_sanitized = array_map(function ($value) {
        return $value !== null ? $value : '';
    }, $user_arr);

    $user_data = array_map('htmlspecialchars', $user_sanitized);
    return [
        "_id" => $user_data['character'],
		"_user_id" => $user_data['id'],
        "_username" => $user_data['username'],
        "_password" => $user_data['password'],
        "_gmail" => $user_data['email'],
        "_gioithieu" => $user_data['gioithieu'],
        "_admin" => $user_data['admin'],
        "_coin" => $user_data['vnd'],
        "_tcoin" => $user_data['tongnap'],
        "_status" => $user_data['active'],
        "_tichdiem" => $user_data['tichdiem'],
        "_xacminh" => $user_data['xacminh'],
        "_thoigian_xacminh" => $user_data['thoigian_xacminh'],
        "password2" => $user_data['mkc2'],
        "gmail" => $user_data['email'],
        "mocnap" => $user_data['mocnap'],
        "coin" => $user_data['coin'],
    ];
}

function fetchPlayerData($conn, $_id)
{
    $stmt = $conn->prepare("SELECT *, JSON_EXTRACT(`character`.infoChar, '$.Gender') AS Gender, JSON_UNQUOTE(JSON_EXTRACT(infoChar, '$.IsPremium')) AS IsPremium FROM `character` WHERE id = :account_id");
    $stmt->bindParam(":account_id", $_id);
    $stmt->execute();
    $player_arr = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$player_arr) {
        return null;
    }

    return [
        "_account_id" => $player_arr['id'],
        "_name" => $player_arr['Name'],
        "_gender" => $player_arr['Gender'],
        "_IsPremium" => $player_arr['IsPremium'],
        "_ThoiVang" => $player_arr['ItemBag'],
    ];
}


$id_post = $_GET["id"] ?? null;

function fetchPosts($conn, $id_post)
{
    $stmt = $conn->prepare("SELECT * FROM posts WHERE id = :id");
    $stmt->bindParam(":id", $id_post);
    $stmt->execute();
    $posts_protect = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$posts_protect) {
        return null;
    }

    $posts_protect = array_map(function ($value) {
        return $value !== null ? $value : '';
    }, $posts_protect);

    return [
        "_postsid" => $posts_protect['id'],
        "_noidung" => $posts_protect['noidung'],
        "_tieude" => $posts_protect['tieude'],
        "_userposts" => $posts_protect['username'],
        "_theloai" => $posts_protect['theloai'],
        "_ghimbai" => $posts_protect['ghimbai'],
        "_trangthai" => $posts_protect['trangthai'],
        "_tinhtrang" => $posts_protect['tinhtrang'],
        "_imagepost" => $posts_protect['image'],
    ];
}

function fetchComments($conn, $id_post)
{
    $stmt = $conn->prepare("SELECT id FROM comments WHERE post_id = :post_id");
    $stmt->bindParam(":post_id", $id_post);
    $stmt->execute();
    $commentIds = $stmt->fetchAll(PDO::FETCH_COLUMN);

    if (!$commentIds) {
        return null;
    }

    return $commentIds;
}

function Callback($conn)
{
    $stmt = $conn->prepare("SELECT * FROM adminpanel");
    $stmt->execute();
    $callback = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$callback) {
        return null;
    }

    return [
        "giatri" => $callback['giatri'],
    ];
}

$_login = null;
$_user = $_SESSION['account'] ?? null;

if ($_user !== null) {
    $_login = "on";
    $user_data = fetchUserData($conn, $_user);
    $user_sanitized = array_map('htmlspecialchars', $user_data);
    $_id = $user_sanitized['_id'];
	$_user_id = $user_sanitized['_user_id'];
    $_username = $user_sanitized["_username"];
    $_password = $user_sanitized["_password"];
    $_gmail = $user_sanitized["_gmail"];
    $_gioithieu = $user_sanitized["_gioithieu"];
    $_admin = $user_sanitized["_admin"];
    $_coin = $user_sanitized["_coin"];
    $_tcoin = $user_sanitized["_tcoin"];
    $_status = $user_sanitized["_status"];
    $_tichdiem = $user_sanitized["_tichdiem"];
    $xacminh = $user_sanitized["_xacminh"];
    $thoigian_xacminh = $user_sanitized["_thoigian_xacminh"];
    $password2 = $user_sanitized["password2"];
    $_mocnap = $user_sanitized["mocnap"];
    $coins = $user_sanitized["coin"];

    $player_data = fetchPlayerData($conn, $_id);
    $player_sanitized = array_map('htmlspecialchars', $player_data);
    $_account_id = $player_sanitized['_account_id'];
    $_name = $player_sanitized['_name'];
    $_gender = $player_sanitized['_gender'];

}

$posts_data = fetchPosts($conn, $id_post);
if ($posts_data !== null) {
    $posts_sanitized = array_map('htmlspecialchars', $posts_data);
    $_postsid = $posts_sanitized['_postsid'];
    $_noidung = $posts_sanitized['_noidung'];
    $_tieude = $posts_sanitized['_tieude'];
    $_userposts = $posts_sanitized['_userposts'];
    $_theloai = $posts_sanitized['_theloai'];
    $_ghimbai = $posts_sanitized['_ghimbai'];
    $_trangthai = $posts_sanitized['_trangthai'];
    $_tinhtrang = $posts_sanitized['_tinhtrang'];
    $_imagepost = $posts_sanitized['_imagepost'];
} else {
    $_postsid = null;
    $_noidung = null;
    $_tieude = null;
    $_userposts = null;
    $_theloai = null;
    $_ghimbai = null;
    $_trangthai = null;
    $_tinhtrang = null;
    $_imagepost = null;
}

$comments_data = fetchComments($conn, $id_post);
if ($comments_data !== null) {
    $comments_sanitized = $comments_data;
    $_id_Cmt = $comments_sanitized[0]; // Access the first comment ID in the array
} else {
    $comments_sanitized = null;
}

$cpanel = Callback($conn);
if ($cpanel !== null) {
    $nduckien = array_map('htmlspecialchars', $cpanel);
    $giatri = $nduckien['giatri'];
}

function getUserAvatar($admin, $gender)
{
    $avatar_base_path = '../Images/avatar';
    $suffix = $admin == 1 ? ($gender == 0 ? '10' : ($gender == 1 ? '11' : '12')) : ($gender == 0 ? '0' : ($gender == 1 ? '1' : '2'));

    if (date('m') == 12 && date('d') == 25) {
        $suffix .= '8';
    }

    return "{$avatar_base_path}{$suffix}.png";
}

function getUserRole($_mocnap)
{
    if ($_mocnap >= 100) {
        return 'Kim Cương';
    } elseif ($_mocnap >= 50) {
        return 'Bạch Kim';
    } elseif ($_mocnap >= 20) {
        return 'Vàng';
    } elseif ($_mocnap >= 10) {
        return 'Bạc';
    } else {
        return '';
    }
}

$colorMap = [
    'Kim Cương' => 'bg-blue-100 text-blue-800',
    'Bạch Kim' => 'bg-purple-100 text-purple-800',
    'Vàng' => 'bg-yellow-100 text-yellow-800',
    'Bạc' => 'bg-gray-100 text-gray-800',
];

if (isset($_mocnap)) {
    $userRole = getUserRole($_mocnap);
    $colorClass = $colorMap[$userRole] ?? '';
} else {
    $colorClass = '';
}


function getTimeAgo($created_at)
{
    $dateFromDatabase = new DateTime($created_at);
    $now = new DateTime();
    $interval = $now->diff($dateFromDatabase);

    $timeUnits = ['y' => 'năm', 'm' => 'tháng', 'd' => 'tuần', 'h' => 'giờ', 'i' => 'phút'];

    $timeAgo = 'Vừa mới';
    foreach ($timeUnits as $unit => $label) {
        if ($interval->$unit > 0) {
            $timeAgo = $interval->$unit . ' ' . $label . ' trước';
            break;
        }
    }

    return $timeAgo;
}

function formatCurrency($number)
{
    $suffix = '';
    if ($number >= 1000000000000) {
        $number /= 1000000000000;
        $suffix = ' Tỷ';
    } elseif ($number >= 1000000000) {
        $number /= 1000000000;
        $suffix = ' Tỷ';
    } elseif ($number >= 1000000) {
        $number /= 1000000;
        $suffix = ' Triệu';
    } elseif ($number >= 1000) {
        $number /= 1000;
        $suffix = ' K';
    }
    return number_format($number) . $suffix;
}

function isValidInput($input)
{
    return preg_match('/^[a-zA-Z0-9_]+$/', $input);
}

function validateCaptcha($input, $captchaText)
{
    return strtoupper($input) === strtoupper($captchaText);
}

function generateCaptcha($length = 6)
{
    $characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    $captcha = '';
    for ($i = 0; $i < $length; $i++) {
        $captcha .= $characters[rand(0, strlen($characters) - 1)];
    }
    return $captcha;
}

function checkExistingUsername($conn, $username)
{
    $stmt = $conn->prepare("SELECT COUNT(*) FROM user WHERE username = :username");
    $stmt->bindValue(':username', $username, PDO::PARAM_STR);
    $stmt->execute();
    return $stmt->fetchColumn() > 0;
}

function insertAccount($conn, $username, $password, $ip_address)
{
    $stmt = $conn->prepare("INSERT INTO user (username, password) VALUES (:username, :password)");
    $stmt->bindValue(':username', $username, PDO::PARAM_STR);
    $stmt->bindValue(':password', $password, PDO::PARAM_STR);
    return $stmt->execute();
}

if (!isset($_POST["captcha"])) {
    $_SESSION['captcha'] = generateCaptcha(6);
}

if (isset($_GET['out'])) {
    if ($_login == "on") {
        header("Location: /");
        exit();
    } else {
        session_destroy();
        header("Location: /");
        exit();
    }
}