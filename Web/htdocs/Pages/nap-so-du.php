<?php
$_alert = null;
require_once '../KhanhDTK/head.php';
if ($_login === null) {
    echo '<script>window.location.href = "../dang-nhap";</script>';
}

$thongbao = isset($thongbao) ? $thongbao : '';

if (isset($_POST['submit'])) {
    if (empty($_POST['telco']) || empty($_POST['amount']) || empty($_POST['serial']) || empty($_POST['code'])) {
        $thongbao = '<span style="color: red; font-size: 12px; font-weight: bold;">Vui lòng nhập đầy đủ thông tin.</span>';
    } else {
        $dataPost = array();
        $dataPost['request_id'] = rand(100000000, 999999999);
        $dataPost['code'] = $_POST['code'];
        $dataPost['partner_id'] = $partner_id;
        $dataPost['serial'] = $_POST['serial'];
        $dataPost['telco'] = $_POST['telco'];
        $dataPost['amount'] = $_POST['amount'];
        $dataPost['command'] = 'charging';
        $dataPost['sign'] = md5($partner_key . $_POST['code'] . $_POST['serial']);
        $data = http_build_query($dataPost);

        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, 'https://doithe1s.vn/chargingws/v2');
        curl_setopt($ch, CURLOPT_POST, 1);
        curl_setopt($ch, CURLOPT_POSTFIELDS, $data);
        $actual_link = (isset($_SERVER['HTTPS']) ? "https" : "http") . "://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";
        curl_setopt($ch, CURLOPT_REFERER, $actual_link);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        $result = curl_exec($ch);
        curl_close($ch);

        $obj = json_decode($result);

        if ($obj->status == 99) {
            // Gửi thẻ thành công, đợi duyệt.
            $thongbao = '<span style="color: orange; font-size: 12px; font-weight: bold;">' . $obj->message . '</span>';
            $telco = $_POST['telco'];
            $serial = $_POST['serial'];
            $code = $_POST['code'];
            $amount = $_POST['amount'];

            // Chuẩn bị câu lệnh SQL để chèn dữ liệu vào bảng "napthe"
            $insert_query = "INSERT INTO napthe (user_nap, telco, serial, code, amount, status)
                         VALUES (:user_nap, :telco, :serial, :code, :amount, 99)";

            $stmt = $conn->prepare($insert_query);
            $stmt->bindParam(':user_nap', $_username);
            $stmt->bindParam(':telco', $telco);
            $stmt->bindParam(':serial', $serial);
            $stmt->bindParam(':code', $code);
            $stmt->bindParam(':amount', $amount);

            // Thực hiện câu lệnh SQL
            if ($stmt->execute()) {
                $thongbao = '<span style="color: green; font-size: 12px; font-weight: bold;">Nạp thẻ thành công. Đợi duyệt.</span>';
            } else {
                $thongbao = '<span style="color: red; font-size: 12px; font-weight: bold;">Lỗi khi lưu dữ liệu vào cơ sở dữ liệu.</span>';
            }

        } elseif ($obj->status == 1) {
            // Thẻ đúng
            $thongbao = '<span style="color: green; font-size: 12px; font-weight: bold;">' . $obj->message . '</span>';
        } elseif ($obj->status == 2) {
            // Thẻ đúng nhưng sai mệnh giá
            $thongbao = '<span style="color: red; font-size: 12px; font-weight: bold;">' . $obj->message . '</span>';
        } elseif ($obj->status == 3) {
            // Thẻ lỗi
            $thongbao = '<span style="color: red; font-size: 12px; font-weight: bold;">' . $obj->message . '</span>';
        } elseif ($obj->status == 4) {
            // Bảo trì
            $thongbao = '<span style="color: red; font-size: 12px; font-weight: bold;">' . $obj->message . '</span>';
        } else {
            // Lỗi khác
            $thongbao = '<span style="color: orange; font-size: 12px; font-weight: bold;">' . $obj->message . '</span>';
        }
    }
}
?>

<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="../home" style="color: white">Quay lại diễn đàn</a> </div>
    </div>
</div>
<div class="container pt-3 pb-5">
    <div class="d-inline d-sm-flex justify-content-center">
        <div class="col-md-8 mb-5 mb-sm-4">
            <div class="d-flex align-items-center justify-content-between"><a href="/points"><small
                        class="fw-semibold">Xem
                        ưu đãi</small></a><small class="fw-semibold">Tích lũy:
                    <?php echo $_mocnap; ?>%
                </small></div>
            <div class="recharge-progress">
                <div class="progress-container">
                    <div class="progress-main">
                        <div class="progress-bar" style="width: <?php echo $_mocnap; ?>%;"></div>
                        <div class="progress-bg"></div>
                    </div>
                </div>
                <div class="_3Ne69qQgMJvF7eNZAIsp_D">
                    <div class="_38CkBz1hYpnEmyQwHHSmEJ">
                        <div class="NusvrwidhtE2W6NagO43R">
                            <div class="_1e8_XixJTleoS7HwwmyB-E">
                                <div class="_2kr5hlXQo0VVTYXPaqefA3 _2Nf9YEDFm2GHONqPnNHRWH" style="left: 1%;">
                                    <div class="_12VQKhFQP9a0Wy-denB6p6">
                                        <div>0</div>
                                        <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                    </div>
                                </div>
                                <div class="_2kr5hlXQo0VVTYXPaqefA3" style="left: 33.3333%;">
                                    <div class="_12VQKhFQP9a0Wy-denB6p6">
                                        <div class="_3KQP4x4OyaOj6NIpgE7cKm"><img alt="<?php echo $_tenmaychu; ?>"
                                                class="_2KchEf_H4jouWwDFDPi5hm" src="/Images/rank/silver.png"
                                                loading="lazy">
                                        </div>
                                        <div>1Tr</div>
                                    </div>
                                    <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                </div>
                                <div class="_2kr5hlXQo0VVTYXPaqefA3" style="left: 66.6667%;">
                                    <div class="_12VQKhFQP9a0Wy-denB6p6">
                                        <div class="_3KQP4x4OyaOj6NIpgE7cKm"><img alt="<?php echo $_tenmaychu; ?>"
                                                class="_2KchEf_H4jouWwDFDPi5hm" src="/Images/rank/gold.png"
                                                loading="lazy">
                                        </div>
                                        <div>2Tr</div>
                                    </div>
                                    <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                </div>
                                <div class="_2kr5hlXQo0VVTYXPaqefA3" style="left: 99%;">
                                    <div class="_12VQKhFQP9a0Wy-denB6p6">
                                        <div class="_3KQP4x4OyaOj6NIpgE7cKm"><img alt="<?php echo $_tenmaychu; ?>"
                                                class="_2KchEf_H4jouWwDFDPi5hm" src="/Images/rank/diamond.png"
                                                loading="lazy">
                                        </div>
                                        <div>5Tr</div>
                                    </div>
                                    <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <div class="text-center pb-3">
                <a href="/points" class="text-dark">
                    <i class="fas fa-hand-point-right"></i>Xem mốc tích luỹ<i
                        class="fas fa-hand-point-left"></i></a>
            </div>
            <h4 style="display: inline-block; margin-right: 10px;">NẠP SỐ DƯ</h4> |
            <h1 style="display: inline-block;"><a href="/banking"
                    style="font-size: 15px; padding: 5px 10px; text-decoration: none; vertical-align: middle;">[
                    Chuyển Khoản ]</a></h1>
                    <br>
            <?php echo $thongbao; ?>
            <form method="POST">
                <tbody>
                    <label>Loại thẻ:</label>
                    <select class="form-control form-control-alternative" name="telco" required>
                        <option value="">Chọn loại thẻ</option>
                        <option value="VIETTEL">Viettel</option>
                        <option value="VINAPHONE">Vinaphone</option>
                        <option value="MOBIFONE">Mobifone</option>
                    </select>
                    <label>Mệnh giá:</label>
                    <select class="form-control form-control-alternative" name="amount" required>
                        <option value="">Chọn mệnh giá</option>
                        <option value="10000">10.000</option>
                        <option value="20000">20.000</option>
                        <option value="30000">30.000 </option>
                        <option value="50000">50.000</option>
                        <option value="100000">100.000</option>
                        <option value="200000">200.000</option>
                        <option value="300000">300.000</option>
                        <option value="500000">500.000</option>
                        <option value="1000000">1.000.000</option>
                    </select>
                    <label>Số seri:</label>
                    <input type="text" class="form-control form-control-alternative" name="serial" required />
                    <label>Mã thẻ:</label>
                    <input type="text" class="form-control form-control-alternative" name="code" required /><br>
                    <button type="submit" class="btn btn-main form-control" name="submit">NẠP NGAY</button>

                </tbody>
            </form>
        </div>
    </div>
    <br>
    <br>
    <h6 class="text-center">LỊCH SỬ NẠP THẺ</h6>
    <hr>
    <style>
        .history-container {
            display: flex;
            overflow-x: hidden;
            /* Ẩn thanh scrollbar */
            position: relative;
        }

        .history-item {
            flex: 0 0 calc(35% - 5px);
            /* Thay đổi giá trị flex */
            padding: 10px;
            overflow: auto;
            position: relative;
        }

        .history-item p {
            margin: 0;
            white-space: nowrap;
        }

        .details-link {
            color: blue;
            text-decoration: underline;
            cursor: pointer;
            position: absolute;
            bottom: 10px;
            right: 10px;
        }
    </style>
    <?php
    if (isset($_name)) {
        $limit = 3; // Số lượng dữ liệu trên mỗi trang
        $stmt = $conn->prepare("SELECT COUNT(*) as total FROM `napthe` WHERE user_nap = :username");
        $stmt->bindParam(":username", $_name);
        $stmt->execute();
        $result = $stmt->fetch(PDO::FETCH_ASSOC);
        $totalRecords = $result['total'];

        $totalPages = ceil($totalRecords / $limit);

        $currentPage = isset($_GET['page']) ? $_GET['page'] : 1;
        $offset = ($currentPage - 1) * $limit;

        $stmt = $conn->prepare("SELECT * FROM `napthe` WHERE user_nap = :username ORDER BY created_at DESC LIMIT :limit OFFSET :offset");
        $stmt->bindParam(":username", $_name);
        $stmt->bindParam(":limit", $limit, PDO::PARAM_INT);
        $stmt->bindParam(":offset", $offset, PDO::PARAM_INT);
        $stmt->execute();
        $result = $stmt->fetchAll(PDO::FETCH_ASSOC);

        if (count($result) > 0) {
            echo '<div class="history-container">';
            foreach ($result as $row) {
                $status = '';

                switch ($row['status']) {
                    case 1:
                        $status = 'Thẻ sai';
                        break;
                    case 2:
                        $status = 'Thẻ đúng';
                        break;
                    default:
                        $status = 'Chờ Duyệt';
                }

                $date = new DateTime($row['created_at']);
                $formattedDate = $date->format('H:i d/m/Y');

                echo '<div class="history-item">';
                echo '<p><strong>ID:</strong> ' . $row['id'] . '</p>';
                echo '<p><strong>Tình trạng:</strong> ' . $status . '</p>';
                echo '<p><strong>Loại thẻ:</strong> ' . $row['telco'] . '</p>';
                echo '<p><strong>Mệnh giá:</strong> ' . number_format($row['amount']) . 'đ</p>';
                echo '<p><strong>Thông tin:</strong> ' . $row['code'] . ' / ' . $row['serial'] . '</p>';
                echo '<p><strong>Thời gian nạp:</strong> ' . $formattedDate . '</p>';
                echo '</div>';
            }
            echo '</div>';

            // Hiển thị nút phân trang
            echo '<div class="col-7 ml-auto">';
            echo '<ul class="pagination justify-content-end">';
            if ($currentPage > 1) {
                echo '<li><a class="btn btn-trangdem btn-light" href="?page=' . ($currentPage - 1) . '"><</a></li>';
            }
            $start_page = max(1, min($totalPages - 2, $currentPage - 1));
            $end_page = min($totalPages, max(2, $currentPage + 1));
            for ($i = 1; $i <= $totalPages; $i++) {
                if ($i >= $start_page && $i <= $end_page) {
                    $class_name = "btn btn-trangdem btn-light";
                    if ($i == $currentPage) {
                        $class_name = "btn btn-trangdem page-active";
                    }
                    echo '<li class="page-item"><a class="' . $class_name . '" href="?page=' . $i . '">' . $i . '</a></li>';
                }
            }
            if ($currentPage < $totalPages) {
                echo '<li><a class="btn btn-trangdem btn-light" href="?page=' . ($currentPage + 1) . '">></a></li>';
            }
            echo '</ul>';
            echo '</div>';

        }
    }
    ?>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>
</div>
</body><!-- Bootstrap core JavaScript -->

</html>