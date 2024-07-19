<?php
require_once 'KhanhDTK/head.php';

$_alert = '';

$username = '';
$password = '';
$ip_address = $_SERVER['REMOTE_ADDR'];

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $username = htmlspecialchars(trim($_POST["username"]));
    $password = htmlspecialchars(trim($_POST["password"]));

    if (!isValidInput($username) || !isValidInput($password)) {
        $_alert = '<div class="text-danger pb-2 font-weight-bold">Tên đăng nhập và mật khẩu không được chứa kí tự đặc biệt.</div>';
        // Đặt lại captcha sau khi đăng ký thành công
        $_SESSION['captcha'] = generateCaptcha(6);
    } else {
        $captchaValue = isset($_POST["captcha"]) ? trim($_POST["captcha"]) : '';
        $captchaText = isset($_SESSION["captcha"]) ? $_SESSION["captcha"] : '';

        if (validateCaptcha($captchaValue, $captchaText)) {

            if (checkExistingUsername($conn, $username)) {
                $_alert = "<div class='text-danger pb-2 font-weight-bold'>Tài khoản đã tồn tại.</div>";
                // Đặt lại captcha sau khi đăng ký thành công
                $_SESSION['captcha'] = generateCaptcha(6);
            } else {
                if (insertAccount($conn, $username, $password, $ip_address)) {
                    $_alert = '<div class="text-danger pb-2 font-weight-bold">Đăng kí thành công!!</div>';
                    // Đặt lại captcha sau khi đăng ký thành công
                    $_SESSION['captcha'] = generateCaptcha(6);
                } else {
                    $_alert = '<div class="text-danger pb-2 font-weight-bold">Đăng ký thất bại.</div>';
                    // Đặt lại captcha sau khi đăng ký thành công
                    $_SESSION['captcha'] = generateCaptcha(6);
                }
            }

        } else {
            $_alert = '<div class="text-danger pb-2 font-weight-bold">Captcha không đúng. Vui lòng nhập lại!</div>';
            // Đặt lại captcha sau khi đăng ký thành công
            $_SESSION['captcha'] = generateCaptcha(6);
        }
    }
}

$conn = null;
?>
<div class="container" style="border-radius: 15px; background: #ffaf4c; padding: 0px">
    <div class="container pt-5 pb-5">
        <div class="row">
            <div class="col-lg-6 offset-lg-3">
                <h4>ĐĂNG KÝ</h4>
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
                    <label><span class="text-danger">*</span> Mã xác minh:</label>
                    <div class="row">
                        <div class="form-group col-6">
                            <input type="text" class="form-control" name="captcha" id="captcha" maxlength="6"
                                spellcheck="false" style="padding: 6px;" placeholder="Nhập captcha ...">
                        </div>
                        <div class="form-group">
                            <input class="form-control" id="captcha" style="background-color: #DCDCDC; font-weight: bold; color: #696969"
                                value=" <?php echo isset($_SESSION['captcha']) ? $_SESSION['captcha'] : ''; ?>" readonly>
                        </div>
                    </div>
                    <div class="form-check form-group">
                        <label class="form-check-label">
                            <input class="form-check-input" type="checkbox" name="accept" id="accept" checked="">
                            Đồng ý <a href="dieu-khoan" target="_blank">Điều khoản sử dụng</a>
                        </label>
                    </div>
                    <?php if (!empty($_alert)) {
                        echo $_alert;
                    } ?>
                    <div id="notify" class="text-danger pb-2 font-weight-bold"></div>
                    <button class="btn btn-main form-control" type="submit">ĐĂNG KÝ</button>
                </form>
                <script>
                    // Kiểm tra định dạng Gmail
                    document.getElementById("form").addEventListener("submit", function (event) {
                        var gmailInput = document.getElementById("gmail");
                        var gmailValue = gmailInput.value.trim();

                        if (gmailValue !== "") {
                            var gmailPattern = /^[a-zA-Z0-9._%+-]+@gmail\.com$/;
                            if (!gmailPattern.test(gmailValue)) {
                                event.preventDefault();
                                document.getElementById("notify").innerHTML = "<div class='text-danger pb-2 font-weight-bold'>Vui lòng nhập định dạng gmail: @gmail.com.</div>";
                            }
                        }
                    });

                    window.onload = function () {
                        var loggedIn = <?php echo ($_login ? 'true' : 'null'); ?>; // Lấy giá trị từ biến $_login
                        if (loggedIn) {
                            window.location.href = "../home"; // Chuyển hướng nếu đã đăng nhập
                        }
                    };
                </script>
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