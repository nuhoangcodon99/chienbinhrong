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
            <h4 style="display: inline-block; margin-right: 10px;">Nạp Tiền - Mbbank</h4>
            <h1 style="display: inline-block;"><a href="/card"
                    style="font-size: 15px; padding: 5px 10px; text-decoration: none; vertical-align: middle;">[
                    Thẻ Cào ]</a></h1>
            <p>Thông tin nạp Mbbank:</p>
            <br>
            <div class="payment-account active">
                <div class="payment-account__left">
                    <img alt="<?php echo $_tenmaychu; ?>" alt="" class="payment-account__qrcode"
                        style="width: 127px;height: 127px;"
                        src=""<?= $stkmbbank_config ?>-qr_only.png?&addInfo=naptien <?= $_user_id ?>&accountName=<?= $chutaikhoan ?>">
                </div>
                <div class="payment-account__right">
                    <p class="payment-account__text">Ngân Hàng Quân Đội | MBBANK</p>
                    <p class="payment-account__text">CTK:
                        <?= $chutaikhoan ?>
                    </p>
                    <div class="payment-account__row">
                        <span class="text-gradient">STK:</span>
                        <?= $stkmbbank_config ?>
                    </div>
                    <div class="payment-account__row">
                        <span class="text-gradient">Nội dung: naptien
                            <?php echo $_user_id; ?>
                        </span>
                    </div>
                </div>
            </div>
            <br>
            - Xây dựng, ủng hộ nro hoạt động.<br>
            <br>
            <br>
            <p><i>Vui lòng kiểm tra kỹ tài khoản! Chỉ nhận giá trị từ 5,000 VNĐ trở lên.</i></p>
            <p><i>Khi giao dịch hãy kiểm tra lại kĩ thông tin nhé!.</i></p>
            <p><i>Khi xác thực xong làm mới trang sau 1 - 3 phút để kiểm tra tình trạng nạp.</i></p>
        </div>
        <div class="col-lg-6 htop border-left">
            <?php
            if (isset($_id)) {

                $stmt = $conn->prepare("SELECT * FROM atm_lichsu WHERE user_nap = :id ORDER BY id DESC");
                $stmt->bindParam(":id", $_id);
                $stmt->execute();
                $result = $stmt->fetchAll(PDO::FETCH_ASSOC);

                echo '<h6 class="text-center">LỊCH SỬ CHUYỂN KHOẢN</h6>';
                echo '<hr><br>';

                if (count($result) > 0) {
                    $itemsPerPage = 3;
                    $totalItems = count($result);
                    $totalPages = ceil($totalItems / $itemsPerPage);

                    $currentPage = isset($_GET['page']) ? max(1, intval($_GET['page'])) : 1;

                    // Tính chỉ số bắt đầu và chỉ số kết thúc dựa trên trang hiện tại và thứ tự ngược của dãy giao dịch
                    $startIndex = ($currentPage - 1) * $itemsPerPage; // Chỉ số bắt đầu của trang hiện tại
                    $endIndex = min($startIndex + $itemsPerPage - 1, $totalItems - 1); // Chỉ số kết thúc của trang hiện tại
                    echo '<div class="transaction-list">';
                    $count = $totalItems - $startIndex; // Tính stt bắt đầu từ tổng số phần tử

                    // Display data for the current page
                    for ($i = $startIndex; $i <= $endIndex; $i++) {
                        $row = $result[$i];

                        $status = '';
                        switch ($row['status']) {
                            case 1:
                                $status = '<span>Đã thanh toán</span>';
                                break;
                            case 2:
                                $status = '<span>Chưa thanh toán</span>';
                                break;
                            default:
                                $status = '<span>Đang xử lý</span>';
                        }

                        echo '<div class="transaction-item">';
                        echo '<p><strong>GIAO DỊCH SỐ:</strong> ' . $count . '</p>';
                        echo '<p><strong>SỐ TIỀN:</strong> ' . number_format($row['sotien']) . 'đ</p>';
                        echo '<p><strong>TRẠNG THÁI:</strong> ' . $status . '</p>';
                        echo '<p><strong>MÃ GIAO DỊCH:</strong> ' . $row['magiaodich'] . '</p>';
                        echo '<p><strong>NGÀY THÁNG:</strong> ' . $row['thoigian'] . '</p>';
                        echo '</div>';

                        // Add <hr> tag if there are more items after this one
                        if ($i < $endIndex) {
                            echo '<hr>';
                        }
                        // Giảm giá trị của $count sau mỗi vòng lặp để cộng dần số thứ tự
                        $count--;
                    }

                    echo '</div>';

                    // Display pagination links
                    echo '<div class="col text-right">';
                    echo '<ul class="pagination justify-content-end pagination-custom-style">';
                    if ($currentPage > 1) {
                        echo '<li><a class="btn btn-sm btn-light" href="?page=' . ($currentPage - 1) . '"><</a></li>';
                    }

                    // Determine the page range to display
                    $pageRangeStart = max(1, $currentPage - 1);
                    $pageRangeEnd = min($totalPages, $pageRangeStart + 2);

                    // Display the page links within the range
                    for ($i = $pageRangeStart; $i <= $pageRangeEnd; $i++) {
                        if ($i == $currentPage) {
                            echo '<li><a class="btn btn-sm page-active">' . $i . '</a></li>';
                        } else {
                            echo '<li><a class="btn btn-sm btn-light" href="?page=' . $i . '">' . $i . '</a></li>';
                        }
                    }

                    if ($currentPage < $totalPages) {
                        echo '<li><a class="btn btn-sm btn-light" href="?page=' . ($currentPage + 1) . '">></a></li>';
                    }
                    echo '</ul>';
                    echo '</div>';
                } else {
                    echo '<div class="text-center">Không có giao dịch nào trong tháng này.</div>';
                }
            } else {
                echo '<div class="text-center">Không tìm thấy tên người dùng trong bảng account.</div>';
            }
            ?>

            <style type="text/css">
                .pagination-custom-style li {
                    display: inline-block;
                    margin-right: 5px;
                    /* Adjust this value as needed for spacing */
                }

                .pagination-custom-style li:last-child {
                    margin-right: 0;
                    /* Remove the right margin from the last button */
                }
            </style>

        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>

</body>

</html>