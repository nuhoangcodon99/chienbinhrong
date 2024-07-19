<?php

require_once '../../KhanhDTK/head.php';
?>

<style>
    th,
    td {
        white-space: nowrap;
        padding: 2px 4px !important;
        font-size: 11px;
    }
</style>
<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="home" style="color: white">Quay lại diễn đàn</a> </div>
    </div>
</div>
<div class="container color-forum pt-2">
    <div class="row">
        <div class="col">
            <?php
            $currentMonth = date('m');
            $currentYear = date('Y');

            $vietnameseMonths = array(
                1 => 'Tháng 1',
                2 => 'Tháng 2',
                3 => 'Tháng 3',
                4 => 'Tháng 4',
                5 => 'Tháng 5',
                6 => 'Tháng 6',
                7 => 'Tháng 7',
                8 => 'Tháng 8',
                9 => 'Tháng 9',
                10 => 'Tháng 10',
                11 => 'Tháng 11',
                12 => 'Tháng 12'
            );

            $query = "SELECT player.`Name`, account.`tongnap`, player.`id` FROM `user` AS account CROSS JOIN `character` AS player ON account.`character` = player.id WHERE account.tongnap > 0 ORDER BY Cast(account.`tongnap` as Int) DESC LIMIT 10;";

            $stmt = $conn->prepare($query);
            $stmt->execute();

            $stt = 1;
            $monthName = $vietnameseMonths[intval($currentMonth)];

            echo '<h6 class="text-center">BẢNG XẾP HẠNG ĐUA TOP NẠP - ' . $monthName . '</h6>';
            if ($stmt->rowCount() > 0) {

                echo '
    <table class="table table-borderless text-center">
        <tbody>
            <tr>
                <th>#</th>
                <th>Nhân Vật</th>
                <th>Tổng Nạp</th>
            </tr>
        </tbody>
        <tbody>';

                while ($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
                    echo '
            <tr>
                <td>' . $stt . '</td>
                <td>' . $row['Name'] . '</td>
                <td>' . number_format($row['tongnap'], 0, ',') . 'đ</td>
            </tr>';
                    $stt++;
                }

                echo '</tbody></table>';
            } else {
                echo '<center><h6>Chưa có thống kê bảng xếp hạng top nạp tháng này!</h6></center>';
            }

            $conn = null;
            ?>

            <div class="text-right">
                <small>Cập nhật lúc:
                    <?php echo date('H:i d/m/Y'); ?>
                </small>
            </div>
        </div>
    </div>
</div>
<?php
include_once '../../KhanhDTK/footer.php';
?>
</div>
</div>
</body><!-- Bootstrap core JavaScript -->

</html>