<?php
require_once 'KhanhDTK/head.php';
if ($_login === true) {
    echo '<script>window.location.href = "../home";</script>';
} else {

}

?>
<div class="container pt-5 pb-5">
    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <h4 class="text-center">QUÊN MẬT KHẨU</h4>
            <form id="updateForm" method="POST">
                <div class="form-group">
                    <label>Tài khoản:</label>
                    <input class="form-control" type="text" name="username" id="username"
                        placeholder="Nhập tên tài khoản">
                </div>
                <div class="form-group">
                    <label>Gmail:</label>
                    <input class="form-control" type="gmail" name="gmail" id="gmail" placeholder="Nhập Gmail của bạn">
                </div>
                <div id="notify" class="text-danger pb-2 font-weight-bold"></div>
                <button class="btn btn-main form-control" type="submit">XÁC NHẬN</button>
            </form>
            <div id="resultMessage"></div>

            <script>
                $(document).ready(function () {
                    // Handle form submission
                    $('#updateForm').submit(function (event) {
                        event.preventDefault(); // Prevent the form from submitting normally
                        setBtnWait($("#btn"));

                        // Serialize the form data
                        var formData = $(this).serialize();

                        // Send the form data to the PHP script using AJAX
                        $.ajax({
                            type: 'POST',
                            url: 'request/gmail/quen-mat-khau.php', // Replace with the actual URL of your PHP script
                            data: formData,
                            dataType: 'json', // Expect JSON response from the server
                            success: function (data) {
                                // Display the message in the resultMessage div
                                $('#resultMessage').html('<span class="' + data.type + '">' + data.message + '</span>');
                            },
                            error: function () {
                                // Handle error if any
                                $('#resultMessage').html('<span class="text-danger pb-2">Thông Báo:</span> Không thể xử lý yêu cầu.');
                            },
                            complete: function () {
                                setBtnOk($("#btn")); // Reset the button after AJAX request is complete
                            },
                        });
                    });
                });
            </script>
            <br>
            <div class="text-center">
                <p>Bạn đã lấy lại tài khoản? <a href="/dang-nhap">Đăng nhập tại đây</a></p>
            </div>
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