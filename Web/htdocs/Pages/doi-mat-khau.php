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
            <h4>ĐỔI MẬT KHẨU</h4>
            <form method="POST" id="updateForm">
                <div class="mb-3">
                    <label class="font-weight-bold">Mật Khẩu hiện tại:</label>
                    <input type="password" class="form-control" name="password" id="password"
                        placeholder="Mật khẩu hiện tại" required autocomplete="password">
                </div>
                <div class="mb-3">
                    <label class="font-weight-bold">Mật Khẩu Mới:</label>
                    <input type="password" class="form-control" name="new_password" id="new_password"
                        placeholder="Mật khẩu mới" required autocomplete="new_password">
                </div>
                <div class="mb-3">
                    <label class="font-weight-bold">Xác Nhận Mật Khẩu Mới:</label>
                    <input type="password" class="form-control" name="new_password_confirmation"
                        id="new_password_confirmation" placeholder="Xác nhận mật khẩu mới" required
                        autocomplete="new_password_confirmation">
                </div>
                <button class="btn btn-sm btn-main form-control" type="submit">Thực hiện</button>
            </form>
            <div id="resultMessage"></div>
            <script>
                $(document).ready(function () {
                    // Handle form submission
                    $('#updateForm').submit(function (event) {
                        event.preventDefault(); // Prevent the form from submitting normally

                        // Serialize the form data
                        var formData = $(this).serialize();

                        // Send the form data to the PHP script using AJAX
                        $.ajax({
                            type: 'POST',
                            url: '../request/changepass', // Replace with the actual URL of your PHP script
                            data: formData,
                            dataType: 'json', // Expect JSON response from the server
                            success: function (data) {
                                // Display the message in the resultMessage div
                                $('#resultMessage').html('<span class="text-danger pb-2' + data.type + ' pb-2">Thông Báo:</span> ' + data.message);
                            },
                            error: function () {
                                // Handle error if any
                                $('#resultMessage').html('<span class="text-danger pb-2">Thông Báo:</span> An error occurred while processing the request.');
                            }
                        });
                    });
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