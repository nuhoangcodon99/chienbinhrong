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
            <h4>BÁO LỖI MÁY CHỦ</h4>
            <form method="POST" enctype="multipart/form-data">
                <div class="form-group">
                    <label><span class="text-danger">*</span> Tiêu đề:</label>
                    <input class="form-control" type="text" name="tieude" id="tieude"
                        placeholder="Nhập tiêu đề bài viết" required>

                    <label><span class="text-danger">*</span> Nội dung:</label>
                    <textarea class="form-control" type="text" name="noidung" id="noidung"
                        placeholder="Nhập nội dung bài viết" required></textarea>

                    <?php
                    if (isset($_SESSION['success'])) {
                        $successMessage = $_SESSION['success'];
                        echo "<p class='text-danger'>$successMessage</p>";
                        unset($_SESSION['success']);
                    } elseif (isset($_SESSION['message'])) {
                        $errorMessage = $_SESSION['message'];
                        echo "<p class='text-danger'>$errorMessage</p>";
                        unset($_SESSION['message']);
                    }
                    ?>
                </div>

                <button class="btn btn-main form-control" type="submit">ĐĂNG BÀI</button>
            </form>
            <script>
                const form = document.querySelector("form");
                const submitBtn = form.querySelector('button[type="submit"]');
                const submitError = form.querySelector("#submit-error");

                const submitForm = async (event) => {
                    event.preventDefault();

                    const titleLength = document.getElementById("tieude").value.trim().length;
                    const contentLength = document.getElementById("noidung").value.trim().length;

                    if (titleLength < 1 || contentLength < 1) {
                        submitError.innerHTML =
                            "<strong>Lỗi:</strong> Tiêu đề và nội dung không được để trống!";
                        submitError.style.display = "block";
                        submitBtn.scrollIntoView({ behavior: "smooth", block: "start" });
                        return;
                    }

                    try {
                        const xhr = new XMLHttpRequest();
                        xhr.open('POST', 'request/gmail/bao-loi-api.php', true);
                        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                        xhr.onreadystatechange = () => {
                            if (xhr.readyState === XMLHttpRequest.DONE) {
                                if (xhr.status === 200) {
                                    const response = xhr.responseText;
                                    if (response === "success") {
                                        alert("Gửi gmail thành công");
                                        updateRemainingTime();
                                        location.reload(); // Tải lại trang nếu gửi yêu cầu thành công
                                    } else {
                                        console.error(response);
                                        location.reload(); // Tải lại trang nếu gửi yêu cầu thành công
                                    }
                                } else {
                                    console.error(xhr.statusText);
                                    location.reload(); // Tải lại trang nếu gửi yêu cầu thành công
                                }
                            }
                        };
                        const formData = new FormData(form);
                        xhr.send(new URLSearchParams(formData).toString());

                    } catch (error) {
                        console.error(error);
                    }
                };

                form.addEventListener("submit", submitForm);
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