<?php
require_once 'KhanhDTK/head.php';
if ($_login === null) {
    echo '<script>window.location.href = "../dang-nhap";</script>';
}
$_alert = '';
?>
<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="home" style="color: white">Quay lại diễn đàn</a> </div>
    </div>
</div>
<div class="container pt-5 pb-5">
    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <h4>GÓP Ý PHÁT TRIỂN MÁY CHỦ</h4>
            <form method="POST" enctype="multipart/form-data">
                <div class="form-group">
                    <label><span class="text-danger">*</span> Tiêu đề:</label>
                    <input class="form-control" type="text" name="tieude" id="tieude"
                        placeholder="Nhập tiêu đề bài viết" required>

                    <label><span class="text-danger">*</span> Nội dung:</label>
                    <textarea class="form-control" type="text" name="noidung" id="noidung"
                        placeholder="Nhập nội dung bài viết" required></textarea>

                    <div id="submit-error" class="alert alert-danger mt-2" style="display: none;"></div>
                </div>

                <button class="btn btn-main form-control" type="submit">ĐĂNG BÀI</button>
            </form>
            <?php
            // Import thư viện PHPMailer
            use PHPMailer\PHPMailer\PHPMailer;
            use PHPMailer\PHPMailer\Exception;

            // Đường dẫn đến các tệp thư viện PHPMailer
            require 'vendor/autoload.php';
            require 'vendor/phpmailer/phpmailer/src/Exception.php';
            require 'vendor/phpmailer/phpmailer/src/PHPMailer.php';
            require 'vendor/phpmailer/phpmailer/src/SMTP.php';

            // Kiểm tra xem form đã được gửi hay chưa
            if ($_SERVER["REQUEST_METHOD"] == "POST") {
                // Lấy giá trị từ form
                $tieude = htmlspecialchars($_POST["tieude"]);
                $noidung = htmlspecialchars($_POST["noidung"]);
                $username = $_username; // Sử dụng thông tin username từ phiên đăng nhập

                // Kiểm tra từ cấm trong tiêu đề và nội dung
                $censoredWords = array(
                    'sex',
                    'địt',
                    'súc vật',
                    'fuck',
                    'loz',
                    'lozz',
                    'lozzz',
                    'óc chó',
                    'ngu lồn',
                    'nguu lồn',
                    'nguu lồn',
                    'ngulon',
                    'nguu lonn',
                    'ngu lon',
                    'occho',
                    'ditmemay',
                    'dmm',
                    'dcm',
                    'địt cụ mày',
                    'địt con mẹ mày',
                    'fuck you',
                    'chịch',
                    'chịt',
                    'sẽ gầy'
                );

                foreach ($censoredWords as $word) {
                    if (stripos($tieude, $word) !== false || stripos($noidung, $word) !== false) {
                        echo "<span class='text-danger pb-2'>Thông Báo:</span> Tiêu đề hoặc nội dung chứa từ không cho phép.";
                        exit;
                    }
                }

                // Gửi email
                $mail = new PHPMailer(true);

                try {
                    // Cấu hình thông tin email
                    $mail->isSMTP();
                    $mail->Host = 'smtp.gmail.com';
                    $mail->SMTPAuth = true;
                    $mail->Username = 'ngocrongdragonking@gmail.com'; // Tài khoản Gmail của bạn
                    $mail->Password = 'vscwwaluzuvwztwr'; // Mật khẩu Gmail của bạn
                    $mail->SMTPSecure = 'tls';
                    $mail->Port = 587;

                    // Thiết lập địa chỉ email người gửi và tên người gửi
                    $mail->setFrom('ngocrongdragonking@gmail.com', 'Góp Ý');

                    // Thiết lập địa chỉ email người nhận
                    $mail->addAddress('ngocrongdragonking@gmail.com');

                    // Thiết lập tiêu đề email
                    $mail->Subject = '=?UTF-8?B?' . base64_encode($tieude) . '?=';

                    // Thiết lập nội dung email
                    $mail->Body = "- Tên tài khoản: " . $username . "\n- Nội dung: $noidung";
                    $mail->CharSet = 'UTF-8';
                    $mail->Encoding = 'base64';

                    // Gửi email
                    $mail->send();

                    echo 'Email đã được gửi đi thành công.';
                } catch (Exception $e) {
                    echo 'Gửi email thất bại. Lỗi: ' . $mail->ErrorInfo;
                }
            }
            ?>
            <script>
                const form = document.querySelector('form');
                const submitBtn = form.querySelector('button[type="submit"]');
                const submitError = form.querySelector('#submit-error');

                form.addEventListener('submit', (event) => {
                    const titleLength = document.getElementById('tieude').value.trim().length;
                    const contentLength = document.getElementById('noidung').value.trim().length;

                    if (titleLength < 1 || contentLength < 1) {
                        event.preventDefault();

                        submitError.innerHTML = '<strong>Lỗi:</strong> Tiêu đề và nội dung phải có ít nhất 5 ký tự!';
                        submitError.style.display = 'block';
                        submitBtn.scrollIntoView({ behavior: 'smooth', block: 'start' });
                    }
                });
            </script>
        </div>
    </div>
</div>

</body><!-- Bootstrap core JavaScript -->

</html>
<div class="py-3">
    <?php
    include_once 'KhanhDTK/footer.php';
    ?>
</div>
</main>
</body>