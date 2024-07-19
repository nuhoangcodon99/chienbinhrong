<?php

require_once '../KhanhDTK/head.php';
if ($_login === null) {
    echo '<script>window.location.href = "login-admin";</script>';
}

// chỉ cho phép tài khoản có admin = 1 truy cập
if ($_admin != 1) {
    echo '<script>window.location.href="../home"</script>';
    exit;
}
?>
<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="home" style="color: white">Quay lại menu admin</a> </div>
    </div>
</div>
<div class="container pt-5 pb-5" id="pageHeader">
    <div class="row pb-2 pt-2">
        <div class="col-lg-6">
            <br>
            <br>
            <h4>Kiểm Tra Thông Tin Nhân Vật - Máy Chủ 1</h4><br>
            <b class="text text-danger">Lưu Ý: </b><br>
            - Điền tên tài khoản để kiểm tra thông tin tài khoản (không phải thông tin nhân vật như: hành trang)
            <br>
            - Thông tin tài khoản bao gồm cả tên nhân vật, chỉ số và nhiệm vụ
            <br>
            <br>
            <br>
            <!-- Hiển thị biến $_alert -->
            <div id="alertContainer">
            </div>
            <form method="POST">
                <div class="mb-3">
                    <label>Tên Tài Khoản:</label>
                    <input type="username" class="form-control" name="username" id="username"
                        placeholder="Nhập tên tài khoản" required autocomplete="username">
                </div>
                <button class="btn btn-main form-control" type="submit">Thực Hiện</button>
            </form>
            <br>
            <br>
            <br>
        </div>
        <div class="col-lg-6 htop border-left">

            <?php
            if ($_SERVER['REQUEST_METHOD'] === 'POST') {
                $username = isset($_POST['username']) ? trim($_POST['username']) : '';

                if (!empty($username)) {
                    $query = "SELECT p.name, p.gender, p.pet, COALESCE(p.data_point, '{}') AS data_point, p.data_task, a.username, a.active, a.mkc2, a.vnd, a.tongnap, a.gmail, a.gioithieu, a.tichdiem
            FROM player p
            LEFT JOIN account a ON p.account_id = a.id
            WHERE a.username = ?";

                    $statement = $conn->prepare($query);
                    $statement->bindParam(1, $username, PDO::PARAM_STR);
                    $statement->execute();

                    $result = $statement->fetch();

                    if ($result) {
                        $tenNhanVat = $result['name'];
                        $gioiTinhNhanVat = $result['gender'];
                        $petNhanVat = $result['pet'];
                        $chuoiChiSo = $result['data_point'];
                        $chuoiNhiemVu = json_decode($result['data_task'], true);
                        $diemTichLuy = $result['tichdiem'];
                        $matkhaucap2 = $result['mkc2'];

                        if (isset($result['gmail']) && is_string($result['gmail'])) {
                            $emailMasked = $result['gmail'] !== '' ? substr($result['gmail'], 0, 2) . str_repeat("*", strlen($result['gmail']) - 2) . "@gmail.com" : "Chưa cập nhật";
                        } else {
                            $emailMasked = "Chưa cập nhật";
                        }

                        $danhHieu = '';
                        $mauSac = '';
                        if ($diemTichLuy >= 200) {
                            $danhHieu = "Chuyên Gia";
                            $mauSac = "#800000";
                        } elseif ($diemTichLuy >= 100) {
                            $danhHieu = "Hỏi Đáp";
                            $mauSac = "#A0522D";
                        } elseif ($diemTichLuy >= 35) {
                            $danhHieu = "Người Bắt Chuyện";
                            $mauSac = "#6A5ACD";
                        } else {
                            $danhHieu = "Chưa sở hữu";
                        }

                        echo '<div class="container pt-5 pb-5" id="pageHeader">';
                        echo '<div class="row pb-2 pt-2">';
                        echo '<div class="col-lg-6">';
                        echo "<h8>TÀI KHOẢN:</h8><br>";
                        echo "<span>- Tài khoản: $username</span><br>";
                        echo "<span>- Thành Viên: " . ($result['active'] == 0 ? "Chưa mở" : "Đã mở") . "</span><br><br>";
                        echo "<span>- Số dư: " . number_format($result['vnd']) . " VNĐ</span><br>";
                        echo "<span>- Tổng nạp: {$result['tongnap']} VNĐ</span><br><br>";
                        echo $danhHieu !== "" ? "- Danh hiệu: <span style='color:$mauSac !important'>$danhHieu</span><br>" : "";
                        echo "<span>- Tích điểm: $diemTichLuy</span><br>";
                        echo "<span>- Gmail: $emailMasked</span><br>";
                        echo "<span>- Mã bảo vệ: " . ($result['mkc2'] != null ? "Đã cập nhật" : "Chưa có") . "</span><br><br>";
                        echo "</div>";

                        echo '<div class="col-lg-6">';
                        echo '<h8>NHÂN VẬT:</h8><br>';
                        echo "<span>- Tên: $tenNhanVat</span><br>";
                        echo "<span>- Hành Tinh: " . ($gioiTinhNhanVat == '0' ? 'Trái Đất' : ($gioiTinhNhanVat == '1' ? 'Namec' : ($gioiTinhNhanVat == '2' ? 'Xayda' : 'Không xác định'))) . "</span><br>";

                        $nhiemVuQuery = "SELECT name FROM task_main_template WHERE id = ?";
                        $nhiemVuStatement = $conn->prepare($nhiemVuQuery);
                        $nhiemVuID = $chuoiNhiemVu[0];
                        $nhiemVuStatement->bindParam(1, $nhiemVuID, PDO::PARAM_INT);
                        $nhiemVuStatement->execute();

                        $nhiemVuResult = $nhiemVuStatement->fetch();

                        $tenNhiemVu = $nhiemVuResult ? $nhiemVuResult['name'] : '';

                        echo "<span>- Nhiệm Vụ: $tenNhiemVu</span><br><br>";

                        echo '<h8>CHỈ SỐ:</h8><br>';

                        $chiSo = json_decode($chuoiChiSo, true);

                        $chiSoCanLay = array_intersect_key($chiSo, array_flip(['1', '2', '4', '5', '6', '7', '8']));

                        foreach ($chiSoCanLay as $key => $value) {
                            switch ($key) {
                                case '1':
                                    echo "<span>- Sức Mạnh: " . number_format($value) . "</span><br>";
                                    break;

                                case '2':
                                    echo "<span>- Tiềm Năng: " . number_format($value) . "</span><br>";
                                    break;

                                case '4':
                                    echo "<span>- Máu: " . number_format($value) . "</span><br>";
                                    break;

                                case '5':
                                    echo "<span>- Thể Lực: " . number_format($value) . "</span><br>";
                                    break;

                                case '6':
                                    echo "<span>- Sức Đánh Gốc: " . number_format($value) . "</span><br>";
                                    break;

                                case '7':
                                    echo "<span>- Giáp Gốc: " . number_format($value) . "</span><br>";
                                    break;

                                case '8':
                                    echo "<span>- Chí Mạng: " . number_format($value) . "</span><br>";
                                    break;
                            }
                        }
                        echo "</div>";
                        echo '</div>';
                        echo '</div>';
                    } else {
                        echo "Không tìm thấy thông tin nhân vật.";
                    }

                    $conn = null;
                } else {
                    echo "Vui lòng nhập tên tài khoản.";
                }
            }
            ?>
        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>

</body><!-- Bootstrap core JavaScript -->

</html>