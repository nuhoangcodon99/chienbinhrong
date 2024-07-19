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
            <h6 class="text-center">BẢNG XẾP HẠNG ĐUA TOP NGỌC RỒNG</h6>
            <table class="table table-borderless text-center">
                <tbody>
                    <tr>
                        <th>#</th>
                        <th>Nhân vật</th>
                        <th>Sức Mạnh</th>
                        <th>Đệ Tử</th>
                        <th>Hành Tinh</th>
                        <th>Tổng</th>
                    </tr>
                <tbody>
                    <?php
                    $countTop = 1;
                    $query = "SELECT c.Name,
            JSON_UNQUOTE(JSON_EXTRACT(c.infoChar, '$.Gender')) AS Gender,
            CAST(JSON_UNQUOTE(JSON_EXTRACT(c.infoChar, '$.Power')) AS SIGNED) AS CharacterPower,
            JSON_EXTRACT(c.infoChar, '$.IsHavePet') AS IsHavePet,
            d.infoChar AS DiscipleInfoChar,
            CAST(JSON_UNQUOTE(JSON_EXTRACT(d.infoChar, '$.Power')) AS SIGNED) AS DisciplePower,
            CAST(JSON_UNQUOTE(JSON_EXTRACT(c.infoChar, '$.Power')) AS SIGNED) +
            CAST(COALESCE(JSON_UNQUOTE(JSON_EXTRACT(d.infoChar, '$.Power')), '0') AS SIGNED) AS TotalPower
          FROM `user` u
          JOIN `character` c ON u.character = c.id
          LEFT JOIN disciple d ON c.id = ABS(d.id)
          ORDER BY TotalPower DESC
          LIMIT 10;";

                    $statement = $conn->prepare($query);
                    $statement->execute();
                    $data = $statement->fetchAll(PDO::FETCH_ASSOC);

                    if (!empty($data)) {
                        foreach ($data as $row) {
                            ?>
                            <tr class="top_<?php echo $countTop; ?>">
                                <td>
                                    <?php echo $countTop++; ?>
                                </td>
                                <td>
                                    <?php echo htmlspecialchars($row['Name']); ?>
                                </td>
                                <td>
                                    <?php
                                    $characterPower = $row['CharacterPower'];

                                    if ($characterPower !== '') {
                                        echo formatNumber($characterPower);
                                    } else {
                                        echo '0';
                                    }
                                    ?>
                                </td>
                                <td>
                                    <?php
                                    $isHavePet = $row['IsHavePet'];

                                    if ($isHavePet === 'false') {
                                        echo 'Không đệ tử';
                                    } else {
                                        $disciplePower = $row['DisciplePower'];

                                        if ($disciplePower !== '') {
                                            echo formatNumber($disciplePower);
                                        } else {
                                            echo 'Chưa sở hữu';
                                        }
                                    }
                                    ?>
                                </td>
                                <td>
                                    <?php
                                    $gender = $row['Gender'];

                                    if ($gender == 0) {
                                        echo "Trái đất";
                                    } elseif ($gender == 1) {
                                        echo "Namec";
                                    } elseif ($gender == 2) {
                                        echo "Xayda";
                                    }
                                    ?>
                                </td>
                                <td>
                                    <?php
                                    $totalPower = $row['TotalPower'];

                                    if ($totalPower !== '') {
                                        echo formatNumber($totalPower);
                                    } else {
                                        echo '0';
                                    }
                                    ?>
                                </td>
                            </tr>
                            <?php
                        }
                    } else {
                        echo 'Máy Chủ 1 chưa có thông kê bảng xếp hạng!';
                    }

                    function formatNumber($number)
                    {
                        if ($number > 1000000000) {
                            return number_format($number / 1000000000, 1, '.', '') . ' tỷ';
                        } elseif ($number > 1000000) {
                            return number_format($number / 1000000, 1, '.', '') . ' Triệu';
                        } elseif ($number >= 1000) {
                            return number_format($number / 1000, 1, '.', '') . ' k';
                        } else {
                            return number_format($number, 0, ',', '');
                        }
                    }
                    ?>
                </tbody>
            </table>
            <script>
                // Cập nhật tự động sau mỗi 3 giây
                setInterval(function () {
                    $.ajax({
                        url: location.href, // URL hiện tại
                        success: function (result) {
                            var leaderboardTable = $(result).find('#leaderboard-table'); // Tìm bảng xếp hạng trong HTML mới nhận được
                            $('#leaderboard-table').html(leaderboardTable.html()); // Cập nhật HTML của bảng xếp hạng
                        }
                    });
                }, 3000);
            </script>
            <div class="text-right">
                <small>Chưa có dữ liệu cập nhật.</small>
                </small>
            </div>
        </div>
    </div>
</div>
<?php
include_once '../../KhanhDTK/footer.php';
?>
</body>

</html>