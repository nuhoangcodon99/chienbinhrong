<?php
require '../../KhanhDTK/connect.php';
$response = array();

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Lấy dữ liệu từ biểu mẫu và thêm tiền tố http:// nếu cần
    $_domain = $_POST['domain'];
    $_title = $_POST['title'];
    $_tenmaychu = $_POST['tenmaychu'];
    $_dailythe = $_POST['dailythe'];

    // Hàm thêm tiền tố http:// nếu cần
    //function add_http_prefix($domain)
    //{
     //   return preg_match("/^(http:\/\/|https:\/\/)/", $domain) ? $domain : 'http://' . $domain . '/';
    //}

    try {
        // Truy vấn để kiểm tra xem có dữ liệu trong cơ sở dữ liệu không
        $query_select = "SELECT * FROM adminpanel";
        $statement_select = $conn->prepare($query_select);
        $statement_select->execute();

        // Kiểm tra kết quả trả về từ truy vấn SELECT
        if ($statement_select->rowCount() == 0) {
            $response['type'] = 'error';
            $response['message'] = 'Không có dữ liệu trong cơ sở dữ liệu!';
        } else {
            // Lấy dữ liệu hiện tại từ cơ sở dữ liệu
            $row = $statement_select->fetch(PDO::FETCH_ASSOC);
            extract($row); // Extract dữ liệu thành các biến địa phương

            // Kiểm tra xem có cần cập nhật không
            if ($domain == $_domain && $title == $_title && $tenmaychu == $_tenmaychu && $dailythe == $_dailythe) {
                $response['type'] = 'info';
                $response['message'] = 'Bạn chưa thay đổi cấu hình nào!';
            } else {
                // Chuẩn bị truy vấn UPDATE
                $query_update = "UPDATE adminpanel SET domain = :domain, title = :title, tenmaychu = :tenmaychu, dailythe = :dailythe WHERE domain = :current_domain";
                $statement_update = $conn->prepare($query_update);
                $params = array(':domain' => $_domain, ':title' => $_title, ':tenmaychu' => $_tenmaychu, ':dailythe' => $_dailythe, ':current_domain' => $domain);

                // Thực thi truy vấn UPDATE
                $statement_update->execute($params);

                // Kiểm tra số dòng đã cập nhật
                $rows_updated = $statement_update->rowCount();

                if ($rows_updated > 0) {
                    $response['type'] = 'success';
                    $response['message'] = 'Thông tin đã được cập nhật thành công!';
                } else {
                    $response['type'] = 'error';
                    $response['message'] = 'Không thể cập nhật thông tin. Vui lòng thử lại sau!';
                }
            }
        }
    } catch (PDOException $e) {
        $response['type'] = 'error';
        $response['message'] = 'Lỗi: ' . $e->getMessage();
    }
} else {
    $response['type'] = 'error';
    $response['message'] = 'Yêu cầu không hợp lệ!';
}

echo json_encode($response);