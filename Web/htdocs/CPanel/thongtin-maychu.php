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
<div class="container pt-5 pb-5" id="pageHeader">
    <div class="row pb-2 pt-2">
        <div class="col-lg-6">
            <br>
            <br>
            <h4>THÔNG TIN MÁY CHỦ</h4><br>
            <b class="text text-danger">Lưu Ý: </b><br>
            - Tên Miền: Điền liên kết website của bạn vào!
            <br>
            - Logo: Điền liên kết ảnh hoặc nhập tên ảnh (Ví Dụ: logo.png) không cần thêm đuôi .png!
            <br>
            - Trạng Thái: Tình trạng website Bảo trì hoặc Hoạt Động
            <br>
            <br>
            - <b>Đại Lý Đang Dùng:
                <?php echo $_dailythe; ?>
            </b>
            <br>
            <?php
            // Tiến hành truy vấn cơ sở dữ liệu để lấy dữ liệu hiện tại từ cột 'domain', 'logo' và 'trangthai'
            $query_select = "SELECT * FROM adminpanel";
            $statement_select = $conn->prepare($query_select);
            $statement_select->execute();

            if ($statement_select->rowCount() == 0) {
                // Thông báo lỗi nếu không có dữ liệu
                $_alert = 'Không có dữ liệu trong cơ sở dữ liệu!';
            } else {
                $row = $statement_select->fetch();
                if ($row && is_array($row)) {
                    // TÊN MIỀN
                    $_domain = $row['domain'];
                    // TITLE MÁY CHỦ
                    $_tenmaychu = $row['tenmaychu'];
                    // TITLE HIỂN THỊ
                    $_title = $row['title'];

                    // LIÊN KẾT TẢI
                    $android = $row['android'];
                    $iphone = $row['iphone'];
                    $windows = $row['windows'];
                    $java = $row['java'];
                }
            }
            ?>
            <br>
            <br>
            <b>
                <div id="resultMessage"></div>
            </b>
            <form method="POST" id="updateForm">
                <div class="mb-3">
                    <label class="font-weight-bold">Tên Miền:</label>
                    <input type="text" class="form-control" name="domain" id="domain" placeholder="Nhập domain" required
                        autocomplete="off" value="<?php echo $_domain; ?>">

                    <label class="font-weight-bold">Tên Tiêu Đề:</label>
                    <input type="text" class="form-control" name="title" id="title" placeholder="Nhập title" required
                        autocomplete="off" value="<?php echo $_title; ?>">

                    <label class="font-weight-bold"><i class="fa-solid fa-link"></i> Api Card:</label>
                    <select name="dailythe" id="dailythe" class="form-control">
                        <option value="https://thesieure.com/" <?php if ($_dailythe == 'https://thesieure.com/')
                            echo 'selected'; ?>>TheSieuRe</option>
                        <option value="https://doithe1s.vn/" <?php if ($_dailythe == 'https://doithe1s.vn/')
                            echo 'selected'; ?>>Doithe1S</option>
                        <option value="https://azcard.vn/" <?php if ($_dailythe == 'https://azcard.vn/')
                            echo 'selected'; ?>>AzCard</option>
                        <option value="https://doithecaocs.vn/" <?php if ($_dailythe == 'https://doithecaocs.vn/')
                            echo 'selected'; ?>>DoiTheCaoCS</option>
                            <option value="https://gachthe1s.com/" <?php if ($_dailythe == 'https://gachthe1s.com/')
                            echo 'selected'; ?>>GachThe1S</option>
                    </select>

                    <label class="font-weight-bold">Tên Máy Chủ:</label>
                    <input type="text" class="form-control" name="tenmaychu" id="tenmaychu"
                        placeholder="Nhập Tên máy chủ (ví dụ: Ngọc Rồng Online)" required autocomplete="off"
                        value="<?php echo $_tenmaychu; ?>">
                </div>
                <button class="btn btn-main form-control" type="submit">Cập Nhật</button>
            </form>
            <script>
                document.getElementById('updateForm').addEventListener('submit', function (event) {
                    event.preventDefault(); // Ngăn chặn hành vi mặc định của biểu mẫu (tải lại trang)

                    var formData = new FormData(this); // Tạo một đối tượng FormData từ biểu mẫu

                    // Gửi yêu cầu AJAX
                    var xhr = new XMLHttpRequest();
                    xhr.open('POST', '../Request/cauhinh/api-thongtin', true);
                    xhr.onload = function () {
                        if (xhr.status === 200) {
                            var response = JSON.parse(xhr.responseText);
                            document.getElementById('resultMessage').innerHTML = response.message;
                            // Xử lý thông tin phản hồi ở đây (nếu cần)
                        } else {
                            document.getElementById('resultMessage').innerHTML = 'Đã xảy ra lỗi khi gửi yêu cầu.';
                        }
                    };
                    xhr.onerror = function () {
                        document.getElementById('resultMessage').innerHTML = 'Đã xảy ra lỗi khi gửi yêu cầu.';
                    };
                    xhr.send(formData); // Gửi dữ liệu biểu mẫu dưới dạng FormData
                });
            </script>
        </div>
        <div class="col-lg-6 htop border-left">
            <br>
            <br>
            <h4>THÔNG TIN LIÊN KẾT</h4><br><br>
            <div class="transaction-item">
                <?php
                // Hiển thị thông tin liên kết và nút sửa tương ứng
                function displayLinkField($fieldName, $fieldValue)
                {
                    $fileExtensions = array(
                        'android' => 'apk',
                        'windows' => 'zip',
                        'iphone' => 'ipa',
                        'java' => 'jar'
                    );

                    $displayValue = $fieldValue;
                    if ($fieldValue !== null && preg_match('/\.(apk|zip|ipa|jar)$/', $fieldValue)) {
                        $displayValue = basename($fieldValue);
                    }

                    echo '<p><strong>' . ucfirst($fieldName) . ':</strong> ';
                    if (!empty($fieldValue)) {
                        echo '<span id="' . $fieldName . '_link">' . $displayValue . '</span>';
                    } else {
                        echo '<span id="' . $fieldName . '_link" style="display: inline">Bạn chưa cài đặt liên kết</span>';
                    }

                    echo '<span id="' . $fieldName . '_edit">';
                    if (!empty($fieldValue)) {
                        echo ' |  <a href="" onclick="toggleEditInput(\'' . $fieldName . '_link\', \'' . $fieldName . '_edit\', \'' . $fieldName . '_input\', \'' . $fieldName . '_save\');">Sửa</a>';
                    } else {
                        echo ' |  <a href="" onclick="toggleEditInput(\'' . $fieldName . '_link\', \'' . $fieldName . '_edit\', \'' . $fieldName . '_input\', \'' . $fieldName . '_save\');">Thêm</a>';
                    }
                    echo '</span></p>';

                    echo '<input type="text" class="form-control" name="' . $fieldName . '" id="' . $fieldName . '_input" placeholder="Nhập liên kết hoặc tên file ' . $fieldName . '" required autocomplete="off" value="' . $displayValue . '" style="display: none;">';
                    echo '<button id="' . $fieldName . '_save" class="btn btn-main form-control" style="display: none;" onclick="saveFieldValue(\'' . $fieldName . '\', \'' . $fieldName . '_input\', \'' . $fieldName . '_link\', \'' . $fieldName . '_edit\', \'' . $fieldName . '_save\')">Lưu</button>';
                }


                // Hiển thị thông tin liên kết cho từng trường
                displayLinkField('android', $android);
                displayLinkField('iphone', $iphone);
                displayLinkField('windows', $windows);
                displayLinkField('java', $java);
                ?>
            </div>
            <br>
            <b class="text text-danger">Hướng Dẫn Liên Kết: </b><br>
            - Liên Kết (URL): Điền liên kết nơi lưu file!
            <br>
            - Tên File: Điền tên file vào các giá trị tương ứng sẽ tự thêm .apk, .ipa, .rar, .jar!
            <br>
            - Nhập Tên file hoặc Liên Kết nhé!
            <br>
        </div>
        <script>
            function saveFieldValue(fieldName, inputId, linkId, editId, saveId) {
                const inputElement = document.getElementById(inputId);
                const newValue = inputElement.value;

                // Validate the input here (disallow specific characters)
                const regex = /[.,'"!~@#$%^&*(){}[\]\-=_+><:;?/\\|]/;
                if (newValue === "" || regex.test(newValue)) {
                    alert("Vui lòng nhập một liên kết hợp lệ.");
                    return;
                }

                // Gửi yêu cầu AJAX để lưu dữ liệu
                const xhr = new XMLHttpRequest();
                xhr.open('POST', '../request/lienkettai', true);
                xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            // Xử lý phản hồi từ API
                            const response = JSON.parse(xhr.responseText);
                            if (response.status === 'success') {
                                // Hiển thị thông báo thành công (nếu cần)
                                alert(response.message);
                                // Tùy chỉnh các tùy chọn sau khi đã lưu thành công
                                const linkElement = document.getElementById(linkId);
                                const editElement = document.getElementById(editId);
                                const saveButton = document.getElementById(saveId);
                                linkElement.innerHTML = newValue;
                                linkElement.style.display = 'inline';
                                inputElement.style.display = 'none';
                                editElement.style.display = 'inline';
                                saveButton.style.display = 'none';
                            } else {
                                // Hiển thị thông báo lỗi (nếu cần)
                                alert(response.message);
                            }
                        } else {
                            // Hiển thị thông báo lỗi (nếu cần)
                            alert('Lỗi khi gửi yêu cầu AJAX.');
                        }
                    }
                };

                // Chuẩn bị dữ liệu để gửi trong yêu cầu POST
                const params = 'fieldName=' + encodeURIComponent(fieldName) + '&fieldValue=' + encodeURIComponent(newValue);

                // Gửi yêu cầu AJAX
                xhr.send(params);
            }

            function toggleEditInput(linkId, editId, inputId, saveId) {
                const linkElement = document.getElementById(linkId);
                const editElement = document.getElementById(editId);
                const inputElement = document.getElementById(inputId);
                const saveButton = document.getElementById(saveId);

                if (linkElement.style.display === 'inline') {
                    linkElement.style.display = 'none';
                    editElement.style.display = 'none';
                    inputElement.style.display = 'inline';
                    saveButton.style.display = 'inline';
                } else {
                    linkElement.style.display = 'inline';
                    editElement.style.display = 'inline';
                    inputElement.style.display = 'none';
                    saveButton.style.display = 'none';
                }
            }

            // Add an event listener to prevent the default behavior of the "Thêm" link
            document.addEventListener('DOMContentLoaded', function () {
                const addLinks = document.querySelectorAll('a[href=""]');
                addLinks.forEach(link => {
                    link.addEventListener('click', function (event) {
                        event.preventDefault(); // Prevent page reload
                        const fieldName = link.parentElement.getElementsByTagName('strong')[0].innerText.toLowerCase();
                        const inputId = fieldName + '_input';
                        const linkId = fieldName + '_link';
                        const editId = fieldName + '_edit';
                        const saveId = fieldName + '_save';
                        toggleEditInput(linkId, editId, inputId, saveId);
                    });
                });

                const saveButtons = document.querySelectorAll('button[id$="_save"]');
                saveButtons.forEach(button => {
                    button.addEventListener('click', function (event) {
                        event.preventDefault(); // Prevent page reload
                        const fieldName = button.id.replace('_save', '');
                        const inputId = fieldName + '_input';
                        const linkId = fieldName + '_link';
                        const editId = fieldName + '_edit';
                        saveFieldValue(fieldName, inputId, linkId, editId, button.id);
                    });
                });
            });
        </script>

    </div>
</div>
<br>
<br>
<?php
include_once '../KhanhDTK/footer.php';
?>
</body><!-- Bootstrap core JavaScript -->

</html>