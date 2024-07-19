<?php
require_once '../KhanhDTK/head.php';

// Kiểm tra xem người dùng đã đăng nhập và có quyền admin hay không
if ($_login === null || $_admin != 1) {
    echo '<script>window.location.href = "login-admin";</script>';
    exit;
}

// Lấy các giá trị cần thiết từ URL (nếu có)
$_active = $_active ?? null;
$_tcoin = $_tcoin ?? 0;
$serverIP = $serverIP ?? '';
$serverPort = $serverPort ?? '';

// Đếm số lượng tài khoản trong cơ sở dữ liệu
$id_user_query = "SELECT COUNT(id) AS id FROM user";
$id_statement = $conn->prepare($id_user_query);
$id_statement->execute();
$id = $id_statement->fetchColumn();

// Đếm số lượng tài khoản bị cấm và số lượng tài khoản hoạt động
$ban_count_query = "SELECT COUNT(*) AS ban FROM user WHERE ban = 1";
$active_count_query = "SELECT COUNT(*) AS active FROM user WHERE active = 1";

$ban_statement = $conn->prepare($ban_count_query);
$active_statement = $conn->prepare($active_count_query);

$ban_statement->execute();
$active_statement->execute();

$_tongban = $ban_statement->fetchColumn();
$_tongactive = $active_statement->fetchColumn();
?>
<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="../home" style="color: white">Quay lại</a> </div>
    </div>
</div>
<div class="container pt-5 pb-5" id="pageHeader">
    <h4>MENU ADMIN</h4>
    <div class="row pb-2 pt-2">
        <div class="col-lg-6 text-left">
            <p>Tổng tài khoản:
                <?php echo $id; ?>
            </p>
            <p>Thành viên:
                <?php echo $_tongactive; ?>
            </p>
            <p>Tài khoản vi phạm:
                <?php echo $_tongban; ?>
            </p>
            <?php
            $sql = "SELECT c.id AS character_id, c.Name AS character_name, c.ItemBag, u.id AS user_id, u.vnd, u.coin
        FROM `character` AS c
        LEFT JOIN user AS u ON c.id = u.character
        WHERE u.id IS NOT NULL";
            $stmt = $conn->prepare($sql);
            $stmt->execute();
            $results = $stmt->fetchAll();

            if (is_array($results) && count($results) > 0) {
                $totalGold = 0;
                $playersGold = [];

                foreach ($results as $result) {
                    $itemsArray = json_decode($result['ItemBag'], true);

                    $totalGoldCharacter = 0;

                    if (is_array($itemsArray)) {
                        foreach ($itemsArray as $item) {
                            if (isset($item['Id']) && $item['Id'] == 457) {
                                $totalGoldCharacter += $item['Quantity'];
                            }
                        }
                    }

                    $playersGold[] = [
                        'character_name' => $result['character_name'],
                        'vnd' => $result['vnd'],
                        'coin' => $result['coin'],
                        'gold' => $totalGoldCharacter
                    ];

                    $totalGold += $totalGoldCharacter;
                }

                usort($playersGold, function ($a, $b) {
                    return $b['gold'] - $a['gold'];
                });

                ?>
                <br><br>
                <h6><b>TOP 10 SỞ HỮU THỎI VÀNG:</b></h6>
                <?php
                for ($i = 0; $i < min(10, count($playersGold)); $i++) {
                    $player = $playersGold[$i];
                    ?>
                    <p><b>Top
                            <?= $i + 1 ?>:
                        </b>
                        <?= $player['character_name'] ?> - Số Lượng:
                        <?= formatCurrency($player['gold']) ?> Thỏi vàng | Số Dư:
                        <?= formatCurrency($player['vnd']) ?> - Coin:
                        <?= formatCurrency($player['coin']) ?>
                    </p>
                    <?php
                }
                ?>
                <p>-------------------------------------</p>
                <p>Tổng thỏi vàng hiện tại:
                    <?= formatCurrency($totalGold) ?> Thỏi vàng
                </p>
                <?php
            } else {
                echo "Không có dữ liệu hoặc không có người chơi nào có thông tin người dùng.";
            }
            ?>
        </div>
        <div class="col-lg-6">
            <?php
            $currentMonth = date('m');
            $currentYear = date('Y');

            $query = "SELECT SUM(amount) AS tongtiennap FROM (
                SELECT amount FROM napthe WHERE status = 1 AND MONTH(created_at) = $currentMonth
                UNION ALL
                SELECT sotien FROM atm_lichsu WHERE status = 1 AND MONTH(STR_TO_DATE(thoigian, '%d/%m/%Y %H:%i:%s')) = $currentMonth
            ) AS tongtiennap";
            $statement = $conn->prepare($query);
            $statement->execute();
            $row = $statement->fetch();
            $tongtienthangnay = $row['tongtiennap'];

            if (is_numeric($tongtienthangnay)) {
                $tienthangnay = number_format($tongtienthangnay);
            } else {
                // Handle when $tongtienthangnay is not a numeric value
                $tienthangnay = 0; // Or any other default value depending on the case
            }


            $query = "SELECT user_nap, SUM(amount) AS tongnap FROM (
                SELECT user_nap, amount FROM napthe WHERE status = 1 AND MONTH(created_at) = $currentMonth
                UNION ALL
                SELECT user_nap, sotien FROM atm_lichsu WHERE status = 1 AND MONTH(STR_TO_DATE(thoigian, '%d/%m/%Y %H:%i:%s')) = $currentMonth
            ) AS total_amount
            GROUP BY user_nap ORDER BY tongnap DESC LIMIT 3";
            $statement = $conn->prepare($query);
            $statement->execute();

            if ($statement->rowCount() > 0) {
                echo "<h6><b>Danh Sách Top Nạp Tháng:</b></h6>";
                echo "<ol class='list-unstyled'>";

                $count = 1; // Biến đếm

                while ($row = $statement->fetch()) {
                    $name = $row['user_nap'];
                    $topnap = $row['tongnap'];

                    $tinhtopnap = number_format($topnap);

                    echo "<li class='mb-1'>TOP $count: $name - Tổng nạp: <span class='amount'>$tinhtopnap VNĐ</span></li> ";

                    $count++; // Tăng biến đếm sau mỗi lần lặp
                }
                echo "</ol><hr>";
                echo "<span class='mb-3'>- Tổng doanh thu tháng này: </span>$tienthangnay VNĐ";
            } else {
                echo "<p>Không có tài khoản nạp vào tháng này.</p>";
            }
            ?>
        </div>
    </div>
</div>
<style>
    /* Custom styles for the menu */
    .menu-container {
        padding: 10px;
        border-radius: 5px;
    }

    .menu-container .btn-main {
        margin-bottom: 5px;
        /* Add any additional custom styling you want for the buttons */
    }
</style>

<div class="container pt-5 pb-5 mt-5">
    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <div class="text-center mb-2 menu-container">
                <a class="btn btn-main btn-sm" href="thongtin-maychu">THÔNG TIN MÁY CHỦ</a>
                <a class="btn btn-main btn-sm" href="kiemtra-nhanvat">KIỂM TRA NHÂN VẬT</a>
                <a class="btn btn-main btn-sm" href="vipham">VI PHẠM</a>
                <a class="btn btn-main btn-sm" href="activetv">MỞ THÀNH VIÊN</a>
                <a class="btn btn-main btn-sm" href="giatri">KHUYẾN MÃI NẠP</a>
                <a class="btn btn-main btn-sm" href="sukien">CÀI ĐẶT SỰ KIỆN</a>
                <a class="btn btn-main btn-sm" href="congvnd">CỘNG TIỀN</a>
                <a class="btn btn-main btn-sm" href="mbbank">LSGD MBBANK</a>

            </div>
        </div>
    </div>
</div>


<?php
include_once '../KhanhDTK/footer.php';
?>
</div>

</body><!-- Bootstrap core JavaScript -->

</html>