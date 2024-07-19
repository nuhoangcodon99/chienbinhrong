<?php

require_once '../KhanhDTK/head.php';
if ($_login === null) {
    echo '<script>window.location.href = "../dang-nhap";</script>';
}
?>
<div id="main-content">
    <div class="container color-forum pt-1 pb-1">
        <div class="row">
            <div class="col"> <a href="../home" style="color: white">Quay lại diễn đàn</a> </div>
        </div>
    </div>
    <div class="container pt-5 pb-5">
        <div class="row">
            <div class="col-lg-6 offset-lg-3">
                <h4>MỞ THÀNH VIÊN</h4>
                <?php
                if (isset($_POST['submit'])) {
                    if ($_status == '1') {
                        $_alert = '<div class="text-danger pb-2 font-weight-bold">Tài khoản của bạn đã được kích hoạt!</div>';
                    } elseif (($_status == '0' || $_status == '-1') && $_coin < 20000) {
                        $_alert = '<div class="text-danger pb-2 font-weight-bold">Bạn không đủ 20.000 KCoin. Vui lòng nạp thêm tiền vào tài khoản để ' . ($_status == '0' ? 'kích hoạt nhé!' : 'mở lại tài khoản!</div>');
                    } elseif (($_status == '0' || $_status == '-1') && $_coin >= 20000) {
                        if ($_tcoin >= 20000) {
                            $coin = $_coin - 20000;
                            $stmt = $conn->prepare('UPDATE `charcter` SET infoChar = JSON_SET(infoChar, "$.IsPremium", "true") WHERE username = :username');
                            $stmt->bindValue(':username', $_name);
                            $stmt = $conn->prepare('UPDATE user SET infoChar = JSON_SET(infoChar, "$.IsPremium", "true"), vnd = :coin WHERE username = :username');
                            $stmt->bindValue(':coin', $coin);
                            $stmt->bindValue(':username', $_username);
                            if ($stmt->execute() && $stmt->rowCount() > 0) {
                                $_alert = '<div class="text-danger pb-2 font-weight-bold">Kích hoạt tài khoản thành công. Bây giờ bạn đã có thể đăng nhập vào game!</div>';
                                if ($_status == '-1') {
                                    $_alert = '<div class="text-danger pb-2 font-weight-bold">Mở khóa tài khoản thành công. Bây giờ bạn đã có thể đăng nhập vào game!</div>';
                                }
                            } else {
                                $_alert = '<div class="text-danger pb-2 font-weight-bold">Có lỗi gì đó xảy ra. Vui lòng liên hệ Admin!</div>';
                            }
                        } else {
                            $_alert = '<div class="text-danger pb-2 font-weight-bold">Bạn cần phải nạp ít nhất 20,000 VNĐ để dùng chức năng này!</div>';
                        }
                    }
                }
                ?>
                <form id="form" method="POST">
                    <div> Thông tin mở thành viên:
                        <br>- Mở thành viên với chỉ <strong>20.000 VNĐ</strong>.<img alt="<?php echo $_tenmaychu; ?>"  src="../Images/hot.gif">
                        <br>- Được miễn phí <strong>GiftCode Thành viên</strong>.<img alt="<?php echo $_tenmaychu; ?>"  src="../Images/hot.gif">
                        <br>- Tận hưởng trọn vẹn các tính năng.<img alt="<?php echo $_tenmaychu; ?>"  src="../Images/hot.gif">
                        <br>- Xây dựng, ủng hộ NgocRongLight.com hoạt động.
                    </div>
                    <div id="notify" class="text-danger pb-2 font-weight-bold"></div>
                    <?php if (isset($_POST['submit'])) {
                        echo $_alert;
                    } ?>
                    <?php if ($_status == '0' || $_status == '-1'): ?>
                        <button class="btn btn-main form-control" id="btn" type="submit" name="submit">MỞ NGAY</button>
                    <?php endif; ?>
                    <?php if ($_status == 1): ?>
                        <div class="text-danger pb-2 font-weight-bold">Bạn đã mở thành viên rồi!</div>
                    <?php endif; ?>
                </form>
            </div>
        </div>
    </div>
    <?php
    include_once '../KhanhDTK/footer.php';
    ?>


    </body><!-- Bootstrap core JavaScript -->

</div>
<noscript>You need to enable JavaScript to run this app.</noscript>
<script>
    // Check if JavaScript is enabled
    var isJavaScriptEnabled = true;
    document.write('<script>isJavaScriptEnabled = false;<\/script>');

    // Function to replace the content if JavaScript is enabled
    function replaceContent() {
        var mainContent = document.getElementById('main-content');
        if (mainContent) {
            mainContent.style.display = 'none';
        }

        var noscriptMsg = document.querySelector('noscript');
        if (noscriptMsg) {
            noscriptMsg.style.display = 'block';
        }
    }

    // Replace the content if JavaScript is enabled
    if (isJavaScriptEnabled) {
        replaceContent();
    }
</script>

</html>