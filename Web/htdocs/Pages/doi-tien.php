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
            <h4 style="display: inline-block; margin-right: 10px;">ĐỔI COIN - TỰ ĐỘNG</h4> |
            <h1 style="display: inline-block;"><a href="/coin"
                    style="font-size: 15px; padding: 5px 10px; text-decoration: none; vertical-align: middle;">[
                    ĐỔI NGỌC XANH ]</a></h1>
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
                <label>Số coin sẽ nhận: <span class="font-weight-bold" id="coin">0</span> coin</label>
                <br>
                <button class="btn btn-main form-control" name="doithoivang" type="submit">Thực hiện</button>
            </form>
            <?php
            // Hàm xử lý việc đổi thưởng thành ngọc
            function processDoiThoivang($conn, $_coin, $_id)
            {
                if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['doithoivang'])) {
                    if (isset($_POST['vnd_amount'])) {
                        $vnd_amount = intval($_POST['vnd_amount']); // Chuyển đổi giá trị thành số nguyên
                        if ($_coin >= $vnd_amount) {
                            // Cập nhật số dư VND mới và coin
                            $new_vnd = $_coin - $vnd_amount;
                            $coin_to_add = $vnd_amount;

                            // Thực hiện cập nhật số dư VND và coin
                            $update_query = "UPDATE account SET vnd = :new_vnd, coin = coin + :coin_to_add WHERE id = :id";
                            $stmt = $conn->prepare($update_query);
                            $stmt->bindParam(":new_vnd", $new_vnd, PDO::PARAM_INT);
                            $stmt->bindParam(":coin_to_add", $coin_to_add, PDO::PARAM_INT);
                            $stmt->bindParam(":id", $_id, PDO::PARAM_INT);
                            $stmt->execute();

                            echo '<div class="text-danger pb-2 font-weight-bold">';
                            echo "Đổi quà thành công! Nhận được $coin_to_add coin.";
                            echo '</div>';
                        } else {
                            echo '<div class="text-danger pb-2 font-weight-bold">';
                            echo 'Số dư VND không đủ.';
                            echo '</div>';
                        }
                    } else {
                        echo '<div class="text-danger pb-2 font-weight-bold">';
                        echo 'Vui lòng chọn số dư cần đổi.';
                        echo '</div>';
                    }
                }
            }

            // Gọi hàm xử lý đổi thưởng, truyền $_coin và $_id vào
            processDoiThoivang($conn, $_coin, $_id);
            ?>

            <script>
                document.getElementById('vnd_amount').addEventListener('change', function () {
                    var vndAmount = parseInt(this.value);
                    var coinQuantity = vndAmount; // Số lượng coin sẽ bằng số tiền VND
                    document.getElementById('coin').textContent = coinQuantity;
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