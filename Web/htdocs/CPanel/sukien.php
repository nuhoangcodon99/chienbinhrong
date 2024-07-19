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
            <h6> CÀI ĐẶT SỰ KIỆN</h6>
            1. Thông tin chung
            <br>
            - Chỉnh sửa sự kiện tại website <strong><?php echo $_tenmaychu; ?></strong>
            <br>
            - Để sử dụng chức năng này, bạn cần mua sự kiện! <a href="https://www.facebook.com/KhanhDTK.dzzz"><strong>(Facebook: KhanhDTK)</a>
            <br>
            <br>
            <?php
            if ($_SERVER["REQUEST_METHOD"] == "POST") {
                // Lấy giá trị nhập từ form
                $sukien = $_POST['sukien'];

                if ($sukien >= 1) {
                    $query = "UPDATE adminpanel SET sukien = :sukien";
                    $statement = $conn->prepare($query);
                    $statement->bindParam(':sukien', $sukien, PDO::PARAM_STR);

                    if ($statement->execute()) {
                        echo "Cài đặt sự kiện!";
                    } else {
                        echo "Lỗi khi cài đặt sự kiện";
                    }
                } else {
                    echo "Sự kiện không hợp lệ!";
                }
            }
            $conn = null;
            ?>

            <form method="POST">
                <div class="mb-3">
                    <label class="font-weight-bold">Cài Đặt Sự Kiện:
                    </label>
                    <input type="number" class="form-control" name="giatri" id="giatri"
                        placeholder="Cài Đặt Sự Kiện (số từ 1 trở đi)" required min="1">
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