<?php
require 'connect.php';

// PHIÊN BẢN FILE GAME
$_android = '2.3.7';
$_windows = '2.3.7';
$_java = '2.3.7';
$_iphone = '2.3.7';

#DoiThe1s.VN API (Nếu có bán lại thì xoá key đi tránh lộ!)
$partner_id = '1501911134'; // TẠO Ở DOITHE1S
$partner_key = '8849a5830f4f73d67b3f75341f37fe7a';  // TẠO Ở DOITHE1S

/*
https://thesieure.com/
https://doithe1s.vn/
https://azcard.vn//
*/
// Cuối Trang
$_group = '<small>Zalo: <a class="text-dark" href="">Ngọc Rồng 7 Sao</a></small><br>';
$_fanpage = '<small>Fanpage: <a class="text-dark" href="https://www.facebook.com/KhanhDTK.dzzz">Ngọc Rồng 7 Sao</a></small><br>';
$_copyright = '<small>https://www.facebook.com/KhanhDTK.dzzz</small>';

# Cấu hình API MBBank | STK
$userloginmbbank_config = '0587782080'; // Tài khoản đăng nhập Mbbank của bạn tại https://online.mbbank.com.vn
$passmbbank_config = 'Loc@228202'; // Mật khẩu đăng nhập Mbbank của bạn tại https://online.mbbank.com.vn
$deviceIdCommon_goc_config = 'e3u9uzfj-mbib-0000-0000-2024021522505015'; // Thay cái thông số mà bạn lấy được từ F12 vào đây
$stkmbbank_config = '0000031683129'; // Số tài khoản Mbbank
$chutaikhoan = 'Dương Tuấn Khanh'; // Tên Tài khoản Mbbank
$_mbbank = 'Ngân Hàng Quân Đội | Mbbank'; // Ngân hàng quân đội Mbbank

# Cấu hình API VietcomBank | GỐC
$username_vcb = ''; // tài khoản VCB
$password_vcb = ''; // Mật khẩu VCB
$account_vcb = ''; // số tài khoản vcb cần check


// Hàm tạo token (bạn có thể thay thế hàm này bằng cách tạo token phù hợp với yêu cầu của bạn)
function CreateToken()
{
    return md5(uniqid(rand(), true));
}

function nduckien_time($datetime, $full = false) {
    $now = new DateTime();
    $ago = DateTime::createFromFormat('d/m/Y H:i:s', $datetime);

    if ($ago === false) {
        // Handle invalid date format
        return 'Invalid date format';
    }

    $diff = $now->diff($ago);

    // Calculate weeks directly
    $weeks = floor($diff->days / 7);
    $days = $diff->days - ($weeks * 7);

    $time_units = array(
        'y' => 'năm',
        'm' => 'tháng',
        'w' => 'tuần',
        'd' => 'ngày',
        'h' => 'giờ',
        'i' => 'phút',
        's' => 'giây',
    );

    // Assign calculated weeks and days
    $diff->w = $weeks;
    $diff->d = $days;

    foreach ($time_units as $key => &$value) {
        if ($diff->$key) {
            $value = $diff->$key . ' ' . $value . ($diff->$key > 1 ? '' : '');
        } else {
            unset($time_units[$key]);
        }
    }

    if (!$full) {
        $time_units = array_slice($time_units, 0, 1);
    }

    return $time_units ? implode(', ', $time_units) . ' trước' : 'vừa xong';
}

# FORUM - NGUYEN DUC KIEN
function deletePostAndComments($conn, $post_id)
{
    $delete_comments_query = "DELETE FROM comments WHERE post_id = ?";
    $delete_posts_query = "DELETE FROM posts WHERE id = ?";

    $delete_comments_stmt = $conn->prepare($delete_comments_query);
    $delete_comments_stmt->bindParam(1, $post_id, PDO::PARAM_INT);
    if ($delete_comments_stmt->execute()) {
        $delete_posts_stmt = $conn->prepare($delete_posts_query);
        $delete_posts_stmt->bindParam(1, $post_id, PDO::PARAM_INT);
        return $delete_posts_stmt->execute();
    }
    return false;
}

function updatePostStatus($conn, $post_id, $column, $value)
{
    $update_query = "UPDATE posts SET $column = ? WHERE id = ?";
    $update_stmt = $conn->prepare($update_query);
    $update_stmt->bindParam(1, $value, PDO::PARAM_INT);
    $update_stmt->bindParam(2, $post_id, PDO::PARAM_INT);
    return $update_stmt->execute();
}


if (date('d') === '01') {
    $updateQuery = "UPDATE user SET mocnap = 0, moc_qua_1 = 0, moc_qua_2 = 0, moc_qua_3 = 0, moc_qua_4 = 0";
    $updateStmt = $conn->prepare($updateQuery);
    $updateStmt->execute();
}