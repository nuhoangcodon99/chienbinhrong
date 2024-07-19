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
<div class="container pt-5 pb-5">
    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <br>
            <br>
            <h6> THÔNG TIN VỀ GIÁ TRỊ NẠP</h6>
            1. Thông tin chung
            <br>
            - Giá trị thực nhận: x1
            <br>
            - Có thể thay đổi giá trị
            <br>
            - Chỉ áp dụng cho thẻ cào
            <br>
            2. Sửa đổi
            <br>
            - Giá trị nạp sẽ được áp dụng trực tiếp vào CallBack
            <br>
            - Vẫn có thể thay đổi giá trị
            <br>
            <br>
            <?php
            if ($_SERVER["REQUEST_METHOD"] == "POST") {
                // Lấy giá trị nhập từ form
                $giatri = $_POST['giatri'];

                // Kiểm tra giá trị nạp
                if ($giatri >= 1) {
                    // Cập nhật giá trị vào cột "giatri" trong bảng "trans_log"
                    $query = "UPDATE adminpanel SET giatri = :giatri";
                    $statement = $conn->prepare($query);
                    $statement->bindParam(':giatri', $giatri, PDO::PARAM_STR);

                    if ($statement->execute()) {
                        echo "Sửa giá nạp thành công!";
                    } else {
                        echo "Lỗi khi cập nhật giá trị";
                    }
                } else {
                    echo "Giá trị nạp không hợp lệ!";
                }
            }
            $conn = null;
            ?>

            <form method="POST">
                <div class="mb-3">
                    <label class="font-weight-bold">Giá trị nạp thẻ: x
                        <?php echo $giatri; ?>
                    </label>
                    <input type="number" class="form-control" name="giatri" id="giatri"
                        placeholder="Giá trị cần thay đổi nạp (số từ 1 trở đi)" required min="1">
                </div>
                <button class="btn btn-main form-control" type="submit">Thực hiện</button>
            </form>
            <div id="notification"></div>
        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>

</body><!-- Bootstrap core JavaScript -->

</html>