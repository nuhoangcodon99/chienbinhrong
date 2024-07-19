<?php

require_once '../KhanhDTK/head.php';
if ($_login === null) {
    echo '<script>window.location.href = "../dang-nhap";</script>';
}
$_alert = '';
?>

<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="../home" style="color: white">Quay lại diễn đàn</a> </div>
    </div>
</div>
<div class="container pt-5 pb-5">
    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <div id="resultMessage"></div>
            <form method="POST" id="postForm" enctype="multipart/form-data">
                <div class="form-group">
                    <label><span class="text-danger">*</span> Tiêu đề:</label>
                    <input class="form-control" type="text" name="tieude" id="tieude"
                        placeholder="Nhập tiêu đề bài viết" required>

                    <label><span class="text-danger">*</span> Nội dung:</label>
                    <textarea class="form-control" name="noidung" id="noidung" placeholder="Nhập nội dung bài viết"
                        required></textarea>
                    <?php if ($_admin == 1) { ?>
                        <label>Ảnh đính kèm:</label>
                        <input class="form-control" type="file" name="anh" id="anh">
                    <?php } ?>
                </div>

                <button class="btn btn-main form control" id="btn" type="submit">ĐĂNG BÀI</button>
            </form>

            <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
            <script>
                $(document).ready(function () {
                    $('#postForm').submit(function (event) {
                        event.preventDefault();

                        var formData = new FormData(this);

                        $.ajax({
                            type: 'POST',
                            url: '../Request/post',
                            data: formData,
                            dataType: 'json',
                            contentType: false,
                            processData: false,
                            cache: false,
                            success: function (data) {
                                $('#resultMessage').html('<span class="text-' + data.type + ' pb-2">Thông Báo:</span> ' + data.message);
                            },
                            error: function () {
                                $('#resultMessage').html('<span class="text-danger pb-2">Thông Báo:</span> Xảy ra lỗi khi gửi dữ liệu.');
                            }
                        });
                    });
                });
            </script>
        </div>
    </div>
</div>

</body><!-- Bootstrap core JavaScript -->

</html>
<div class="py-3">
    <?php
    include_once '../KhanhDTK/footer.php';
    ?>
</div>
</main>
</body>