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
            <h4>Kích Hoạt Thành Viên</h4><br>
            <b class="text text-danger">Lưu Ý: </b><br>
            - Hãy thoát game trước khi mở thành viên tránh lỗi không mong muốn!
            <br>
            - Chỉ dùng cho những tài khoản không bị khóa do vi phạm
            <br>
            <br>
            <?php
            $_alert = '';

            if ($_SERVER["REQUEST_METHOD"] == "POST") {
                $_name = $_POST["username"];

                try {
                    // Check if the account exists
                    $stmt_check = $conn->prepare("SELECT * FROM `character` WHERE Name = :username");
                    $stmt_check->bindParam(":username", $_name);
                    $stmt_check->execute();
                    $result_check = $stmt_check->fetch();

                    if (!$result_check) {
                        $_alert = '<div class="alert alert-danger">Lỗi: Tài khoản không tồn tại!</div>';
                    } else {
                        if (isset($result_check["ban"]) && $result_check["ban"] == 1) {
                            $_alert = '<div class="alert alert-danger">Lỗi: Tài khoản đã bị vi phạm và không thể kích hoạt!</div>';
                        } else {
                            // Assuming $_IsPremium is defined and holds a valid column name
                            $stmt_update = $conn->prepare("UPDATE `character` SET infoChar = JSON_SET(infoChar, '$.IsPremium', 'true') WHERE Name = :username");
                            $stmt_update->bindParam(":username", $_name);
                            if ($stmt_update->execute()) {
                                if ($stmt_update->rowCount() == 1) {
                                    $_alert = '<div class="alert alert-success">Kích hoạt thành viên thành công cho tài khoản: ' . $_name . '!</div>';
                                } else {
                                    $_alert = '<div class="alert alert-danger">Tài khoản: ' . $_name . ' đã kích hoạt thành viên rồi!</div>';
                                }
                            } else {
                                $_alert = '<div class="alert alert-danger">Lỗi: Không thể kích hoạt thành viên cho tài khoản!</div>';
                            }
                        }
                    }
                } catch (PDOException $e) {
                    $_alert = '<div class="alert alert-danger">Lỗi: ' . $e->getMessage() . '</div>';
                } finally {
                    $stmt_check = null;
                    $stmt_update = null;
                    $conn = null;
                }
            }

            ?>
            <!-- Hiển thị biến $_alert -->
            <?php
            echo $_alert;
            ?>
            <form method="POST">
                <div class="mb-3">
                    <label>Tên Nhân Vật:</label>
                    <input type="username" class="form-control" name="username" id="username"
                        placeholder="Nhập tên tài khoản" required autocomplete="username">
                </div>
                <button class="btn btn-main form-control" type="submit">Kích Hoạt</button>
            </form>
        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>

</body><!-- Bootstrap core JavaScript -->

</html>