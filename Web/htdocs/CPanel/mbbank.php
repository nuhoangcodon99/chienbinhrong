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

include('../request/banking/api.php');
$MBBANK = new MBBANK;

function getUserDataFromToken($_token, $conn)
{
    $sql = "SELECT * FROM adminpanel WHERE token = ?";
    $stmt = $conn->prepare($sql);
    $stmt->execute([$_token]);
    return $stmt->fetch(PDO::FETCH_ASSOC);
}

function calculateBalanceAndStatus($_token, $MBBANK, $conn)
{
    $SoDu = '';
    $TrangThai = '';

    if (isset($_token) && !empty($_token)) {
        $getData = getUserDataFromToken($_token, $conn);
        if ($getData) {
            $balance = json_decode($MBBANK->get_balance($getData['userlogin'], $getData['sessionId'], $getData['deviceId']), true);
            if ($balance['result']['message'] == 'OK') {
                foreach ($balance['acct_list'] as $data) {
                    if ($data['acctNo'] == $getData['stk']) {
                        $status = true;
                        $message = 'Giao dịch thành công';
                        $SoDu = $message != '99' ? number_format($data['currentBalance']) : '';
                        $TrangThai = $message != '99' ? 'Hoạt động' : 'Đang tắt';
                    }
                }
            }
        }
    }

    return [$SoDu, $TrangThai];
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
            <h6 class="text-center">CẤU HÌNH MBBANK</h6>
            <div class="transaction-list">
                <?php
                list($SoDu, $TrangThai) = calculateBalanceAndStatus($_token, $MBBANK, $conn);

                $query = "SELECT * FROM adminpanel";
                $stmt = $conn->prepare($query);
                $stmt->execute();

                while ($row = $stmt->fetch()) {
                    echo '<div class="transaction-item">
                    <p><strong>TÊN ĐĂNG NHẬP:</strong> ' . $row['userlogin'] . '</p>
                    <p><strong>SỐ TÀI KHOẢN:</strong> ' . $row['stk'] . '</p>
                    <p><strong>TÊN THẺ:</strong> ' . $row['name'] . '</p>
                    <p><strong>SỐ DƯ:</strong> ' . $SoDu . ' VNĐ</p>
                    <p><strong>TRẠNG THÁI:</strong> ' . $TrangThai . '</p></div>';
                }
                ?>
            </div>
            <br>
            <hr>
            <a class="btn btn-main btn-sm" href="./request/banking/login?user=<?php echo $userloginmbbank_config; ?>"
                onclick="event.preventDefault(); Login()">Login Mbbank</a>
            <br>
            <br>
            <a class="btn btn-main btn-sm" href="./request/banking/mbbank?token=<?php echo $_token; ?>"
                onclick="event.preventDefault(); Cron()">Chạy Mbbank</a>
            <script>
                function Login() {
                    $.ajax({
                        url: '../request/banking/login?user=<?php echo $userloginmbbank_config; ?>',
                        success: function (response) {
                            alert(response);
                        }
                    });
                }

                function Cron() {
                    $.ajax({
                        url: '../request/banking/mbbank?token=<?php echo $_token; ?>',
                        success: function (response) {
                            alert(response);
                        }
                    });
                }
            </script>
            <hr>
            <br>
            <b class="text text-danger">Phổ Biến Thông Tin: </b><br>
            <b>- Ví Dụ:
                <br>
                + Trạng Thái: hoạt động (đang chạy) - Đang tắt (Không hoạt động)
                <br>
                + Không chạy cron nó sẽ không hiển thị trạng thái mbbank nhá!
                <br>
                + Hiển thị cấu hình mbbank đã cài, xem lịch sử nạp toàn bộ máy chủ
                <br>
                <br>
            </b>
            <?php
            if ($_login === false) {
                ?>
                <a href="#" class="btn btn-sm btn-success" data-toggle="modal" data-target="#napthe">Đăng Nhập</a>
                <?php
            } else {

            } ?>
        </div>
        <div class="col-lg-6 htop border-left">
            <h6 class="text-center">LỊCH SỬ CHUYỂN KHOẢN MBBANK</h6>
            <br>
            <?php
            $itemsPerPage = 5;
            $queryTotal = "SELECT COUNT(*) as total FROM atm_lichsu";
            $stmtTotal = $conn->prepare($queryTotal);
            $stmtTotal->execute();
            $totalResult = $stmtTotal->fetch();
            $totalItems = $totalResult['total'];
            $totalPages = ceil($totalItems / $itemsPerPage);
            $currentPage = isset($_GET['page']) ? max(1, intval($_GET['page'])) : 1;
            $startIndex = ($currentPage - 1) * $itemsPerPage;

            $query = "SELECT * FROM atm_lichsu ORDER BY id DESC LIMIT :startIndex, :itemsPerPage";
            $statement = $conn->prepare($query);
            $statement->bindParam(':startIndex', $startIndex, PDO::PARAM_INT);
            $statement->bindParam(':itemsPerPage', $itemsPerPage, PDO::PARAM_INT);
            $statement->execute();
            $results = $statement->fetchAll(PDO::FETCH_ASSOC);
            if (!empty($results)) {
                echo '<div class="transaction-list">';

                foreach ($results as $row) {
                    echo '<div class="transaction-item">';
                    echo '<p><strong>TÀI KHOẢN:</strong> ' . $row['user_nap'] . '</p>';
                    echo '<p><strong>SỐ TIỀN:</strong> ' . number_format($row['sotien'], 0, '.', ',') . 'đ</p>';
                    echo '<p><strong>THỜI GIAN:</strong> ' . KhanhDTK_time($row['thoigian']) . '</p>';
                    echo '<hr></div>';
                }

                echo '</div>';

                // Display pagination links
                echo '<div class="col text-right">';
                echo '<ul class="pagination justify-content-end pagination-custom-style">';
                if ($currentPage > 1) {
                    echo '<li><a class="btn btn-sm btn-light" href="?page=' . ($currentPage - 1) . '"><</a></li>';
                }

                // Determine the page range to display
                $pageRangeStart = max(1, $currentPage - 1);
                $pageRangeEnd = min($totalPages, $pageRangeStart + 2);

                // Display the page links within the range
                for ($i = $pageRangeStart; $i <= $pageRangeEnd; $i++) {
                    if ($i == $currentPage) {
                        echo '<li><a class="btn btn-sm page-active">' . $i . '</a></li>';
                    } else {
                        echo '<li><a class="btn btn-sm btn-light" href="?page=' . $i . '">' . $i . '</a></li>';
                    }
                }

                if ($currentPage < $totalPages) {
                    echo '<li><a class="btn btn-sm btn-light" href="?page=' . ($currentPage + 1) . '">></a></li>';
                }
                echo '</ul>';
                echo '</div>';
            } else {
                echo 'Không có lịch sử giao dịch';
            }
            ?>

            <style type="text/css">
                .pagination-custom-style li {
                    display: inline-block;
                    margin-right: 5px;
                    /* Adjust this value as needed for spacing */
                }

                .pagination-custom-style li:last-child {
                    margin-right: 0;
                    /* Remove the right margin from the last button */
                }
            </style>

        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>

</body><!-- Bootstrap core JavaScript -->

</html>