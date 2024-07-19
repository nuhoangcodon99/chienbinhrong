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
<div class="container pt-5 pb-5" id="pageHeader">
    <div class="row pb-2 pt-2">
        <div class="col-lg-6">
            <?php
            $query = "SELECT password, email FROM user WHERE username = :username";
            $statement = $conn->prepare($query);
            $statement->bindParam(":username", $_username);
            $statement->execute();
            $row = $statement->fetch();
            $primaryPassword = $row['password'];
            $primaryGmail = $row['email'];

            if ($_SERVER['REQUEST_METHOD'] === 'POST') {
                $password = $_POST['password'] ?? '';
                $newGmail = $_POST['new_gmail'] ?? '';
                $newGmailConfirm = $_POST['new_gmail_confirm'] ?? '';

                if (!empty($primaryGmail)) {
                    $oldGmail = $_POST['old_gmail'] ?? '';

                    if (!empty($password) && !empty($newGmail) && !empty($newGmailConfirm) && !empty($oldGmail)) {
                        if ($password !== $primaryPassword) {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Sai mật khẩu hiện tại</div>";
                        } elseif ($oldGmail !== $primaryGmail) {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Sai Gmail hiện tại</div>";
                        } elseif ($newGmail === $primaryGmail) {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Gmail mới không được giống với Gmail hiện tại</div>";
                        } elseif ($newGmail !== $newGmailConfirm) {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Gmail mới không giống nhau</div>";
                        } elseif (!filter_var($newGmail, FILTER_VALIDATE_EMAIL) || substr($newGmail, -10) !== "@gmail.com") {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Vui lòng nhập địa chỉ email Gmail (ví dụ: example@gmail.com)</div>";
                        } elseif (!filter_var($newGmailConfirm, FILTER_VALIDATE_EMAIL) || substr($newGmailConfirm, -10) !== "@gmail.com") {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Vui lòng nhập địa chỉ email Gmail (ví dụ: example@gmail.com)</div>";
                        } else {
                            // Update the new Gmail in the database
                            $updateQuery = "UPDATE user SET email = :newGmail WHERE username = :username";
                            $updateStatement = $conn->prepare($updateQuery);
                            $updateStatement->bindParam(":newGmail", $newGmail);
                            $updateStatement->bindParam(":username", $_username);

                            if ($updateStatement->execute()) {
                                echo "<div class='text-danger pb-2 font-weight-bold'>Cập nhật Gmail thành công</div>";
                            } else {
                                echo "<div class='text-danger pb-2 font-weight-bold'>Lỗi khi cập nhật Gmail</div>";
                            }
                        }
                    } else {
                        echo "<div class='text-danger pb-2 font-weight-bold'>Vui lòng điền đầy đủ thông tin trong form</div>";
                    }
                } else {
                    if (!empty($password) && !empty($newGmail) && !empty($newGmailConfirm)) {
                        if ($password !== $primaryPassword) {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Sai mật khẩu hiện tại</div>";
                        } elseif ($newGmail !== $newGmailConfirm) {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Gmail không giống nhau</div>";
                        } elseif (!filter_var($newGmail, FILTER_VALIDATE_EMAIL) || substr($newGmail, -10) !== "@gmail.com") {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Vui lòng nhập địa chỉ email Gmail (ví dụ: example@gmail.com)</div>";
                        } elseif (!filter_var($newGmailConfirm, FILTER_VALIDATE_EMAIL) || substr($newGmailConfirm, -10) !== "@gmail.com") {
                            echo "<div class='text-danger pb-2 font-weight-bold'>Vui lòng nhập địa chỉ email Gmail (ví dụ: example@gmail.com)</div>";
                        } else {
                            // Update Gmail in the database
                            $updateQuery = "UPDATE usser SET email = :newGmail WHERE username = :username";
                            $updateStatement = $conn->prepare($updateQuery);
                            $updateStatement->bindParam(":newGmail", $newGmail);
                            $updateStatement->bindParam(":username", $_username);

                            if ($updateStatement->execute()) {
                                echo "<div class='text-danger pb-2 font-weight-bold'>Cập nhật Gmail thành công</div>";
                            } else {
                                echo "<div class='text-danger pb-2 font-weight-bold'>Lỗi khi cập nhật Gmail</div>";
                            }
                        }
                    } else {
                        echo "<div class='text-danger pb-2 font-weight-bold'>Vui lòng điền đầy đủ thông tin trong form</div>";
                    }
                }
            }

            if (!empty($primaryGmail)) {
                ?>

                <p>Tài khoản của bạn đã được liên kết Gmail</p>

                <?php
                // Check the 'xacminh' column and calculate the remaining time
                if ($xacminh == 1) {
                    $currentTimestamp = time();
                    $remainingSeconds = $thoigian_xacminh - $currentTimestamp;
                    $remainingMinutes = ceil($remainingSeconds / 60);

                    if ($remainingMinutes <= 0) {
                        echo "data: Thời gian xác minh đã hết\n\n";

                        // Update the 'xacminh' column and 'thoigian_xacminh' column to reset
                        $updateStmt = $conn->prepare("UPDATE account SET xacminh = 0, thoigian_xacminh = 0 WHERE username = :username");
                        $updateStmt->bindParam(":username", $_username, PDO::PARAM_STR);
                        $updateStmt->execute();
                    } else {
                        echo "data: Thời gian còn lại: $remainingMinutes phút\n\n";
                    }
                }
                ?>

                <div>Gmail liên kết: <span class="font-weight-bold">
                        <?php echo $primaryGmail; ?>
                    </span></div>
            <?php } else {
                // Lấy thông báo và lớp thông báo từ tham số truy vấn
                $message = $_GET['message'] ?? '';
                $messageClass = $_GET['messageClass'] ?? '';

                // Hiển thị thông báo và lớp thông báo
                if ($message && $messageClass) {
                    echo '<div class="' . $messageClass . '">' . $message . '</div>';
                }
                ?>

                <form method="POST">
                    <div class="mb-3">
                        <label class="font-weight-bold">Mật khẩu hiện tại:</label>
                        <input type="password" class="form-control" name="password" id="password"
                            placeholder="Mật khẩu hiện tại" required autocomplete="password">
                    </div>
                    <div class="mb-3">
                        <label class="font-weight-bold">Gmail mới:</label>
                        <input type="text" class="form-control" name="new_gmail" id="new_gmail" placeholder="Gmail mới"
                            required autocomplete="new_gmail">
                    </div>
                    <div class="mb-3">
                        <label class="font-weight-bold">Xác nhận Gmail mới:</label>
                        <input type="text" class="form-control" name="new_gmail_confirm" id="new_gmail_confirm"
                            placeholder="Xác nhận Gmail mới" required autocomplete="new_gmail_confirm">
                    </div>
                    <button class="btn btn-main form-control" type="submit">Thực hiện</button>
                </form>
                <br>
                <br>
            <?php }
            ?>
            <div id="notification"></div>
        </div>
        <div class="col-lg-6 htop ">
            <br>
            <br>
            <h6> THÔNG TIN VỀ CẬP NHẬT THÔNG TIN</h6>
            1. Thông tin chung
            <br>
            - Cập nhật Gmail
            <br>
            - Dùng để lấy lại thông tin khi quên
            <br>
            - Có thể dùng hoặc không dùng
            <br>
            - Có thể đổi được gmail mới
            <br>
            - Nhấn vào nút HỦY GMAIL HIỆN TẠI là nó sẽ gửi gmail nha :3
            <br>
            <br>
            2. Hủy gmail hiện tại
            <br>
            - Gmail sẽ được huỷ luôn nếu như bạn xác nhận
            <br>
            - Vẫn có thể bật lại sau khi Hủy
            <br>
            <br>

            <?php if (!empty($primaryGmail)) { ?>
                <div class="mt-2 mb-2">
                    <?php if (!empty($primaryGmail)) { ?>
                        <div class="mt-2 mb-2">
                            <a class="btn btn-sm btn-main" href="#" id="sendEmailLink">
                                <i class="fas fa-ban text-danger"></i><span style="color: white;"> HỦY GMAIL HIỆN TẠI </span>
                            </a>

                            <script>
                                document.getElementById('sendEmailLink').addEventListener('click', function (event) {
                                    event.preventDefault();

                                    var xhr = new XMLHttpRequest();
                                    xhr.open('POST', 'request/gmail/guithu.php', true);
                                    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                                    xhr.onreadystatechange = function () {
                                        if (xhr.readyState === XMLHttpRequest.DONE) {
                                            if (xhr.status === 200) {
                                                var response = xhr.responseText;
                                                if (response === "success") {
                                                    alert("Gửi gmail thành công");
                                                    updateRemainingTime(); // Cập nhật thời gian sau khi gửi gmail thành công
                                                } else {
                                                    console.error(response);
                                                }
                                            } else {
                                                console.error(xhr.statusText);
                                            }
                                        }
                                    };
                                    xhr.send();
                                });

                                // Kiểm tra xác minh và thời gian xác minh
                                function checkVerificationStatus() {
                                    var xacminh = <?php echo $xacminh; ?>;
                                    var thoigian_xacminh = <?php echo $thoigian_xacminh; ?>;
                                    var sendEmailLink = document.getElementById('sendEmailLink');

                                    if (xacminh == 0 && thoigian_xacminh == 0) {
                                        sendEmailLink.innerHTML = '<i class="fas fa-ban text-danger"></i> <span style="color: white;">HỦY GMAIL HIỆN TẠI</span>';
                                        sendEmailLink.removeEventListener('click', sendEmailClickHandler);
                                    } else {
                                        sendEmailLink.innerHTML = '<i class="fa fa-spinner fa-spin"></i> <span style="color: white;">CHỜ XÁC NHẬN...</span>';
                                        sendEmailLink.addEventListener('click', sendEmailClickHandler);
                                    }
                                }

                                function sendEmailClickHandler(event) {
                                    event.preventDefault();
                                    console.log('<span style="color: white;">Đang xử lý...</span>');
                                }

                                // Gọi hàm kiểm tra xác minh và thời gian xác minh ban đầu
                                checkVerificationStatus();
                            </script>
                        </div>
                    <?php } ?>
                </div>
            <?php } ?>
        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>
</div>

</body><!-- Bootstrap core JavaScript -->

</html>