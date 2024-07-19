<?php
require_once '../../KhanhDTK/connect.php';
require_once '../../KhanhDTK/session.php';

$response = array();

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Hàm kiểm tra từ cấm
    function isCensored($text, $censoredWords)
    {
        foreach ($censoredWords as $word) {
            if (stripos($text, $word) !== false) {
                return true;
            }
        }
        return false;
    }

    // Lấy dữ liệu từ biểu mẫu và làm sạch
    $tieude = filter_input(INPUT_POST, 'tieude', FILTER_SANITIZE_SPECIAL_CHARS);
    $noidung = filter_input(INPUT_POST, 'noidung', FILTER_SANITIZE_SPECIAL_CHARS);

    // Kiểm tra từ cấm
    $censoredWords = array(
        'sex', 'địt', 'súc vật', 'sv', 'fuck', 'loz', 'lozz', 'lozzz',
        'óc chó', 'ngu lồn', 'nguu lồn', 'nguu lồn', 'ngulon', 'nguu lonn',
        'ngu lon', 'occho', 'ditmemay', 'dmm', 'dcm', 'địt cụ mày',
        'địt con mẹ mày', 'fuck you', 'chịch', 'chịt', 'sẽ gầy'
    );

    if (isCensored($tieude, $censoredWords) || isCensored($noidung, $censoredWords)) {
        $response['type'] = 'info';
        $response['message'] = '<span class="text-danger pb-2">Thông Báo:</span> Tiêu đề hoặc nội dung chứa từ không cho phép.';
    } else {
        // Thêm hình ảnh nếu có
        $imageName = '';
        if (!empty($_FILES['anh']['name'])) {
            $targetDirectory = "../../Images/uploads/";
            $targetFile = $targetDirectory . basename($_FILES["anh"]["name"]);
            $uploadOk = 1;
            $imageFileType = strtolower(pathinfo($targetFile, PATHINFO_EXTENSION));

            if (file_exists($targetFile)) {
                $response['type'] = 'error';
                $response['message'] = 'Tệp ảnh đã tồn tại.';
                $uploadOk = 0;
            }

            if ($_FILES["anh"]["size"] > 5000000) { // 5MB
                $response['type'] = 'error';
                $response['message'] = 'Kích thước tệp ảnh quá lớn.';
                $uploadOk = 0;
            }

            $allowedExtensions = array("jpg", "jpeg", "png", "gif", "mp4");
            if (!in_array($imageFileType, $allowedExtensions)) {
                $response['type'] = 'error';
                $response['message'] = 'Chỉ chấp nhận tệp ảnh hoặc video.';
                $uploadOk = 0;
            }

            if ($uploadOk == 1 && move_uploaded_file($_FILES["anh"]["tmp_name"], $targetFile)) {
                $imageName = $targetFile;
            } else {
                $response['type'] = 'error';
                $response['message'] = 'Đã xảy ra lỗi khi tải lên tệp ảnh.';
            }
        }

        if (!isset($response['type'])) {
            // Thêm bài viết vào cơ sở dữ liệu
            $sql = "INSERT INTO posts (tieude, noidung, username, image) VALUES (:tieude, :noidung, :username, :image)";
            $stmt = $conn->prepare($sql);
            $stmt->bindParam(':tieude', $tieude, PDO::PARAM_STR);
            $stmt->bindParam(':noidung', $noidung, PDO::PARAM_STR);
            $stmt->bindParam(':username', $_name, PDO::PARAM_STR);
            $stmt->bindParam(':image', $imageName, PDO::PARAM_STR);

            if ($_status === 0) {
                $response['type'] = 'info';
                $response['message'] = '<span class="text-danger pb-2">Lỗi:</span> Yêu cầu mở thành viên để sử dụng chức năng này.';
            } elseif ($stmt->execute()) {
                // Cập nhật điểm cho tài khoản
                $sql_update = "UPDATE user SET tichdiem = (tichdiem + 1) WHERE username = :username";
                $stmt_update_account = $conn->prepare($sql_update);
                $stmt_update_account->bindParam(':username', $_username, PDO::PARAM_STR);
                $stmt_update_account->execute();

                $response['type'] = 'success';
                $response['message'] = 'Bài viết đã được đăng thành công.';
            } else {
                $response['type'] = 'error';
                $response['message'] = 'Đã có lỗi xảy ra khi đăng bài.';
            }
        }
    }
} else {
    $response['type'] = 'error';
    $response['message'] = 'Yêu cầu không hợp lệ!';
}

header('Content-Type: application/json');
echo json_encode($response);
exit;
