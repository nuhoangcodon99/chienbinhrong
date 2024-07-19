<?php

require_once '../KhanhDTK/head.php';
if ($_login === null) {
    echo '<script>window.location.href = "../dang-nhap";</script>';
}

?>

<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="../home" style="color: white">Quay lại diễn đàn</a> </div>
    </div>
</div>
<div class="container pt-5 pb-5">
    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <h4 style="display: inline-block; margin-right: 10px;">ĐỔI NGỌC XANH - TỰ ĐỘNG</h4> |
            <h1 style="display: inline-block;"><a href="/coin"
                    style="font-size: 15px; padding: 5px 10px; text-decoration: none; vertical-align: middle;">[
                    ĐỔI COIN ]</a></h1>
            <br>
            <span>- Máy chủ chưa hỗ trợ đổi trực tiếp khi đang trực tuyến trong máy chủ! </span>
            <p>- Vui lòng thoát game tránh lỗi khi đổi nhé:3 </p>
            <form method="POST">
                <label for="">Tên tài khoản:</label>
                <input type="text" value="<?php echo $_username; ?>" class="form-control"
                    style="padding-right:30px;font-size:16px;width:540px" readonly autocomplete="text"></input>
                <br>
                <label for="vnd_amount">Số Dư Cần Đổi:</label>
                <select class="form-control form-control-alternative" name="vnd_amount" id="vnd_amount" required>
                    <option value="">Chọn Số Dư</option>
                    <option value="10000">10,000 VNĐ</option>
                    <option value="20000">20,000 VNĐ</option>
                    <option value="30000">30,000 VNĐ</option>
                    <option value="50000">50,000 VNĐ</option>
                    <option value="100000">100,000 VNĐ</option>
                    <option value="200000">200,000 VNĐ</option>
                    <option value="300000">300,000 VNĐ</option>
                    <option value="500000">500,000 VNĐ</option>
                    <option value="1000000">1,000,000 VNĐ</option>
                </select>
                <label>Số ngọc xanh sẽ nhận: <span class="font-weight-bold" id="gold">0</span> ngọc</label>
                <br>
                <button class="btn btn-main form-control" name="doithoivang" type="submit">Thực hiện</button>
            </form>
            <?php
            function processDoiThoivang($conn, $_coin, $_id, $giatri)
            {
                if ($_SERVER['REQUEST_METHOD'] === 'POST') {
                    if (isset($_POST['doithoivang'])) {
                        $vnd_amount = isset($_POST['vnd_amount']) ? intval($_POST['vnd_amount']) : 0;

                        // Mệnh giá và số lượng thỏi vàng tương ứng
                        $options = array(
                            array("amount" => 10000, "quantity" => 25),
                            array("amount" => 20000, "quantity" => 60),
                            array("amount" => 30000, "quantity" => 90),
                            array("amount" => 50000, "quantity" => 160),
                            array("amount" => 100000, "quantity" => 360),
                            array("amount" => 200000, "quantity" => 670),
                            array("amount" => 500000, "quantity" => 1700),
                            array("amount" => 1000000, "quantity" => 3500),
                        );

                        $ngoc_quantity = 0;
                        foreach ($options as $option) {
                            if ($option["amount"] == $vnd_amount) {
                                $ngoc_quantity = $option["quantity"] * $giatri;
                                break;
                            }
                        }

                        if ($ngoc_quantity > 0) {

                            // Kiểm tra số dư VND đủ để trừ
                            if ($_coin >= $vnd_amount) {
                                // Cập nhật số dư VND
                                $new_vnd = $_coin - $vnd_amount;

                                // Update the database using prepared statements
                                $update_query = "UPDATE account SET vnd = :new_vnd, ngoc_xanh = :ngocxanh WHERE username = :username";
                                $stmt = $conn->prepare($update_query);
                                $stmt->bindParam(":new_vnd", $new_vnd, PDO::PARAM_INT);
                                $stmt->bindParam(":ngoc_xanh", $ngoc_quantity, PDO::PARAM_INT);
                                $stmt->bindParam(":username", $_id, PDO::PARAM_STR);
                                $stmt->execute();

                                echo '<div class="text-danger pb-2 font-weight-bold">';
                                echo "Đổi quà thành công! Nhận được $ngoc_quantity ngọc xanh.";
                                echo '</div>';
                            } else {
                                echo '<div class="text-danger pb-2 font-weight-bold">';
                                echo 'Số dư VND không đủ.';
                                echo '</div>';
                            }
                        } else {
                            echo '<div class="text-danger pb-2 font-weight-bold">';
                            echo 'Không tìm thấy món quà phù hợp.';
                            echo '</div>';
                        }
                    }
                }
            }

            processDoiThoivang($conn, $_coin, $_id, $giatri);
            ?>
            <script>
                document.getElementById('vnd_amount').addEventListener('change', function () {
                    var vndAmount = parseInt(this.value);
                    var goldQuantity = 0;
                    var options = [
                        { amount: 10000, quantity: 25 },
                        { amount: 20000, quantity: 60 },
                        { amount: 30000, quantity: 90 },
                        { amount: 50000, quantity: 160 },
                        { amount: 100000, quantity: 360 },
                        { amount: 200000, quantity: 670 },
                        { amount: 500000, quantity: 1700 },
                        { amount: 1000000, quantity: 3500 }
                    ];

                    for (var i = 0; i < options.length; i++) {
                        if (options[i].amount === vndAmount) {
                            goldQuantity = options[i].quantity * <?php echo $giatri; ?>;  // Multiply by $giatri
                            break;
                        }
                    }

                    document.getElementById('gold').textContent = goldQuantity;
                });
            </script>
        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>

</body><!-- Bootstrap core JavaScript -->

</html>