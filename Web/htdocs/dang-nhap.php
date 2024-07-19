<?php
require_once 'KhanhDTK/head.php';

if ($_login != null) {
    echo '<script>window.location.href = "../home";</script>';
}

$_alert = ''; // Khai báo biến $_alert ở đầu mã

function isValidUsername($username)
{
    return ctype_alnum($username);
}

function loginUser($username, $password, $conn)
{
    global $_alert;

    try {
        $stmt = $conn->prepare("SELECT * FROM user WHERE username = :username");
        $stmt->bindParam(':username', $username, PDO::PARAM_STR);
        $stmt->execute();
        $select = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($select !== false && $select['password'] == $password) {
            $account_id = $select['character'];

            $stmt = $conn->prepare("SELECT * FROM `character` WHERE id = :account_id");
            $stmt->bindParam(':account_id', $account_id, PDO::PARAM_INT);
            $stmt->execute();
            $result = $stmt->fetch(PDO::FETCH_ASSOC);

            if ($result !== false) {
                if (empty($select['ip_address'])) {
                    $ip_address = $_SERVER['REMOTE_ADDR'];
                    $set = "ip_address = :ip_address";
                    $where = "id = :account_id";

                    $updateStmt = $conn->prepare("UPDATE user SET $set WHERE $where");
                    $updateStmt->bindParam(':ip_address', $ip_address, PDO::PARAM_STR);
                    $updateStmt->bindParam(':account_id', $account_id, PDO::PARAM_INT);

                    if ($updateStmt->execute() === false) {
                        $_alert = '<div class="text-danger pb-2 font-weight-bold">Có lỗi khi cập nhật IP address!</div>';
                    }
                }

                // Mã hoá đầu ra để tránh XSS khi hiển thị username
                $_SESSION['account'] = htmlspecialchars($username, ENT_QUOTES, 'UTF-8');
                $_SESSION['id'] = $select['id'];
                echo '<script>window.location.href = "../home";</script>';
                exit();
            } else {
                $_alert = '<div class="text-danger pb-2 font-weight-bold">Tài khoản này chưa tạo nhân vật!</div>';
            }
        } else {
            $_alert = '<div class="text-danger pb-2 font-weight-bold">Tên đăng nhập hoặc mật khẩu không hợp lệ, vui lòng kiểm tra lại!</div>';
        }
    } catch (PDOException $e) {
        // Xử lý lỗi một cách tốt hơn, ví dụ ghi log hoặc thông báo thân thiện cho người dùng
        // Logging: error_log("Lỗi truy vấn: " . $e->getMessage());
        $_alert = '<div class="text-danger pb-2 font-weight-bold">Có lỗi xảy ra trong quá trình xử lý. Vui lòng thử lại sau!</div>';
    }
}

if ($_login === true) {
    echo '<script>window.location.href = "../home";</script>';
    exit();
} elseif ($_login == null && isset($_POST['username']) && isset($_POST['password'])) {
    $username = htmlspecialchars(trim($_POST['username']), ENT_QUOTES, 'UTF-8');
    $password = htmlspecialchars(trim($_POST['password']), ENT_QUOTES, 'UTF-8');

    
        loginUser($username, $password, $conn);
    
} elseif ($_login == null && isset($_POST['submit'])) {
    $_alert = '<div class="text-danger pb-2 font-weight-bold">Vui lòng nhập tên đăng nhập và mật khẩu!</div>';
}
?>
<div class="container" style="border-radius: 15px; background: #ffaf4c; padding: 0px">
    <div class="container pt-5 pb-5">
        <div class="row">
            <div class="col-lg-6 offset-lg-3">
                <h4>ĐĂNG NHẬP</h4>
                <form id="form" method="POST">
                    <div class="form-group">
                        <label><span class="text-danger">*</span> Tài khoản:</label>
                        <input class="form-control" type="text" name="username" id="username"
                            placeholder="Nhập tài khoản">
                    </div>
                    <div class="form-group">
                        <label><span class="text-danger">*</span> Mật khẩu:</label>
                        <input class="form-control" type="password" name="password" id="password"
                            placeholder="Nhập mật khẩu">
                    </div>
                    <div class="form-check form-group">
                        <label class="form-check-label">
                            <input class="form-check-input" type="checkbox" name="accept" id="accept" checked="">
                            Ghi nhớ đăng nhập
                        </label>
                        <a href="forgot-password" class="text-dark" style="float: right;">Quên mật
                            khẩu</a>
                    </div>
                    <?php
                        echo $_alert;
                    ?>
                    <div id="notify" class="text-danger pb-2 font-weight-bold"></div>
                    <button class="btn btn-main form-control" type="sumbit">ĐĂNG
                        NHẬP</button>
                </form>
            </div>
        </div>
    </div>
    <?php
    include_once 'KhanhDTK/footer.php';
    ?>
</div>
</div>
</body>

</html>
