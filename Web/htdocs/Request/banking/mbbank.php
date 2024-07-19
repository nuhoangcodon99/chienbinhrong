<?php
// Đường dẫn đến file cấu hình và kết nối
require_once('../../KhanhDTK/cauhinh.php');
require_once('../../KhanhDTK/connect.php');
// Đưa vào file chứa lớp API của ngân hàng
include('api.php');

define('POINTS_PER_TOPUP', 1);

// Kiểm tra xem key có được cung cấp không
if (!isset($_GET['users']) || $_GET['users'] !== $userloginmbbank_config) {
    // Nếu key không hợp lệ, có thể redirect hoặc hiển thị thông báo lỗi
    exit('Không tìm thấy key! Không thể truy cập.');
}

$MB = new MBBANK; // Tạo đối tượng API MBBANK

// Hàm lấy dữ liệu người dùng từ token
function layDuLieuNguoiDungTuToken($_token, $conn)
{
    $sql = "SELECT * FROM adminpanel WHERE token = ?";
    $KhanhDTK = $conn->prepare($sql);
    $KhanhDTK->execute([$_token]);
    return $KhanhDTK->fetch(PDO::FETCH_ASSOC);
}

// Hàm cập nhật dữ liệu người dùng
function capNhatDuLieuNguoiDung($duLieuNguoiDung, $MB, $conn)
{
    global $MB;
    $MB->deviceIdCommon_goc = $MB->generateImei();
    $MB->user = $duLieuNguoiDung['userlogin'];
    $MB->pass = $duLieuNguoiDung['password'];

    // Bypass captcha và thực hiện đăng nhập
    $textCaptcha = $MB->bypass_captcha_web2m('413145b2f6d981e32d0ee69a56b0e839');
    $phienDangNhap = json_decode($MB->login($textCaptcha), true);

    // Xử lý các trạng thái phản hồi khác nhau từ đăng nhập
    if ($phienDangNhap['result']['message'] == "Capcha code is invalid") {
        exit(json_encode(array('status' => '1', 'msg' => 'Captcha không chính xác')));
    } elseif ($phienDangNhap['result']['message'] == 'Customer is invalid') {
        exit(json_encode(array('status' => '1', 'msg' => 'Thông tin không chính xác')));
    } else {
        $sql = "UPDATE adminpanel SET name = ?, password = ?, sessionId = ?, deviceId = ?, time = ? WHERE userlogin = ?";
        $KhanhDTK = $conn->prepare($sql);
        $KhanhDTK->execute([$phienDangNhap['cust']['nm'], $duLieuNguoiDung['password'], $phienDangNhap['sessionId'], $MB->deviceIdCommon_goc, time(), $duLieuNguoiDung['userlogin']]);
    }
}

// Hàm xử lý giao dịch và cập nhật cơ sở dữ liệu tương ứng
function xuLyGiaoDich($giaoDich, $conn)
{
    $noiDung = $giaoDich['description'];
    $thoiGian = $giaoDich['transactionDate'];
    // Sử dụng biểu thức chính quy để lấy tên người dùng từ mô tả
    preg_match('/naptien\s+(\S+(\s+\S+)*)/', rtrim($noiDung, '.'), $matches);
    $tenDangNhap = isset($matches[1]) ? trim(str_replace(['-', '.', '+', 'in'], ' ', $matches[1])) : '';
    $soTien = $giaoDich['creditAmount'];
    $maGiaoDich = $giaoDich['refNo'];
    $tenTaiKhoanNhan = $giaoDich['benAccountName'] ?: "Ngân Hàng Quân Đội - MBBANK";
    $tenNganHang = $giaoDich['bankName'] ?: "Ngọc Rồng";
    $soTaiKhoan = $giaoDich['accountNo'];

    if ($soTien > 100000) {
        $tichdiem = floor($soTien / 100000) * POINTS_PER_TOPUP;
    } else {
        $tichdiem = 0;
    }
    // Kiểm tra nếu mô tả (description) chứa "naptien" và số tiền lớn hơn hoặc bằng 5000
    if (stripos($noiDung, 'naptien') !== false && $soTien >= 3000) {
        $sqlKiemTraMaGiaoDich = "SELECT tranid FROM atm_check WHERE tranid = ?";
        $KiemTraMaGiaoDich = $conn->prepare($sqlKiemTraMaGiaoDich);
        $KiemTraMaGiaoDich->execute([$maGiaoDich]);

        // Nếu mã giao dịch không được tìm thấy trong bảng kiểm tra, cập nhật tài khoản người dùng và lịch sử giao dịch
        if ($KiemTraMaGiaoDich->rowCount() == 0) {
            $sqlCapNhatTaiKhoan = "UPDATE user SET vnd = vnd + ?, tongnap = tongnap + ?, mocnap = mocnap + ? WHERE id = ?";
            $CapNhatTaiKhoan = $conn->prepare($sqlCapNhatTaiKhoan);
            $CapNhatTaiKhoan->execute([$soTien, $soTien, $tichdiem, $tenDangNhap]);

            $sqlLichSuGiaoDich = "INSERT INTO atm_lichsu (user_nap, magiaodich, thoigian, sotien, status, benAccountName, accountNo, bankName) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
            $LichSuGiaoDich = $conn->prepare($sqlLichSuGiaoDich);
            $LichSuGiaoDich->execute([$tenDangNhap, $maGiaoDich, $thoiGian, $soTien, 1, $tenTaiKhoanNhan, $soTaiKhoan, $tenNganHang]);

            $sqlMaGiaoDich = "INSERT INTO atm_check (tranid) VALUES (?)";
            $Trans_MaGiaoDich = $conn->prepare($sqlMaGiaoDich);
            $Trans_MaGiaoDich->execute([$maGiaoDich]);
        }
    }
}

// Hiển thị lịch sử giao dịch
if (isset($_token) && !empty($_token)) {
    $duLieuNguoiDung = layDuLieuNguoiDungTuToken($_token, $conn);
    if ($duLieuNguoiDung) {
        $lichSuGiaoDich = json_decode($MB->get_lsgd($duLieuNguoiDung['userlogin'], $duLieuNguoiDung['sessionId'], $duLieuNguoiDung['deviceId'], $duLieuNguoiDung['stk'], 2), true);
        if (isset($duLieuNguoiDung['time']) && $duLieuNguoiDung['time'] < time() - 60 && isset($lichSuGiaoDich['result']) && $lichSuGiaoDich['result']['message'] == 'Session invalid') {
            capNhatDuLieuNguoiDung($duLieuNguoiDung, $MB, $conn);
        }

        if (isset($lichSuGiaoDich['transactionHistoryList']) && is_array($lichSuGiaoDich['transactionHistoryList'])) {
            foreach ($lichSuGiaoDich['transactionHistoryList'] as $giaoDich) {
                xuLyGiaoDich($giaoDich, $conn);
            }
        }

        // Kiểm tra số dư hiện tại
        $soDu = json_decode($MB->get_balance($duLieuNguoiDung['userlogin'], $duLieuNguoiDung['sessionId'], $duLieuNguoiDung['deviceId']), true);
        if ($duLieuNguoiDung['time'] < time() - 60 && $soDu['result']['message'] == 'Session invalid') {
            capNhatDuLieuNguoiDung($duLieuNguoiDung, $MB, $conn);
        }

        $currentBalance = null; // Initialize the variable

        if (isset($soDu['result']) && $soDu['result']['message'] == 'OK') {
            foreach ($soDu['acct_list'] as $thongTinTaiKhoan) {
                if ($thongTinTaiKhoan['acctNo'] == $duLieuNguoiDung['stk']) {
                    $trangThai = true;
                    $thongBao = 'Hoạt Động';
                    if ($thongBao != '99') {
                        $currentBalance = $thongTinTaiKhoan['currentBalance']; // Assign the value to the variable
                        // echo json_encode(array('status' => '200', 'SoDu' => '' . $thongTinTaiKhoan['currentBalance'] . ''));
                    }
                }
            }
        } else {
            capNhatDuLieuNguoiDung($duLieuNguoiDung, $MB, $conn);
        }
    }
}

// Lấy và hiển thị lịch sử giao dịch cho người dùng trong ngày hiện tại
$sqlLayGiaoDich = "SELECT * FROM atm_lichsu";
$LayGiaoDich = $conn->prepare($sqlLayGiaoDich);
$LayGiaoDich->execute();
$ketQuaGiaoDich = $LayGiaoDich->fetchAll(PDO::FETCH_ASSOC);

// Số lượng dòng dữ liệu trên mỗi trang
$rowsPerPage = 100000;

// Tổng số trang
$totalPages = ceil(count($ketQuaGiaoDich) / $rowsPerPage);

// Trang hiện tại, nếu không được xác định thì mặc định là trang 1
$current_page = isset($_GET['page']) ? $_GET['page'] : 1;

// Xác định vị trí bắt đầu của dòng dữ liệu cho trang hiện tại
$startRow = ($current_page - 1) * $rowsPerPage;

// Lấy dữ liệu từ cơ sở dữ liệu với phân trang
$sqlLayGiaoDichPhanTrang = "SELECT * FROM atm_lichsu LIMIT $startRow, $rowsPerPage";
$LayGiaoDichPhanTrang = $conn->prepare($sqlLayGiaoDichPhanTrang);
$LayGiaoDichPhanTrang->execute();
$ketQuaGiaoDichPhanTrang = $LayGiaoDichPhanTrang->fetchAll(PDO::FETCH_ASSOC);

?>
<!DOCTYPE html>
<html lang="vi">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Lịch Sử Giao Dịch</title>
    <style>
        /* Reset CSS */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 20px;
        }

        /* Container styles */
        .container {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            flex-wrap: wrap;
        }

        /* Table styles */
        table {
            border-collapse: collapse;
            width: 100%;
            margin-top: 20px;
        }

        th,
        td {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }

        th {
            background-color: #f2f2f2;
        }

        /* Pagination styles */
        .pagination {
            display: flex;
            list-style: none;
            margin-top: 20px;
        }

        .pagination li {
            margin-right: 10px;
        }

        /* Status styles */
        .status {
            margin-top: 20px;
        }
    </style>
</head>

<body>
    <div class="container">
        <div>
            <h2>Lịch Sử Giao Dịch</h2>
            <!-- Hiển thị bảng lịch sử giao dịch -->
            <table>
                <thead>
                    <tr>
                        <th>Tên Tài Khoản</th>
                        <th>Số Tiền</th>
                        <th>Trạng Thái</th>
                        <th>Số Tài Khoản</th>
                        <th>Mã Giao Dịch</th>
                        <th>Thời Gian</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($ketQuaGiaoDichPhanTrang as $dong): ?>
                        <tr>
                            <td>
                                <?= $dong['user_nap'] ?>
                            </td>
                            <td>
                                <?= $dong['sotien'] ?>
                            </td>
                            <td>
                                <?= $dong['status'] == 1 ? 'Thành công' : 'Thất bại' ?>
                            </td>
                            <td>0
                                <?= $dong['accountNo'] ?>
                            </td>
                            <td>
                                <?= $dong['magiaodich'] ?>
                            </td>
                            <td>
                                <?= $dong['thoigian'] ?>
                            </td>
                        </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>
            <!-- Hiển thị phân trang -->
            <ul class="pagination">
                <!-- Sử dụng biến $page để xác định trang hiện tại -->
                <?php for ($page = 1; $page <= $totalPages; $page++): ?>
                    <li>
                        <a href="?key=KhanhDTK&page=<?= $page ?>" <?= ($page == $current_page) ? 'class="active"' : '' ?>>
                            <?= $page ?>
                        </a>
                    </li>
                <?php endfor; ?>
            </ul>
        </div>
    </div>
    <div class="status">
        <!-- Hiển thị thông báo hoạt động -->
        <?php if (isset($trangThai) && isset($thongBao)): ?>
            <p>Trạng thái hoạt động:
                <?= $thongBao ?>
            </p>
            <?php if (isset($thongTinTaiKhoan['currentBalance'])): ?>
                <p>Số Dư Tài Khoản:
                    <?= $currentBalance ?>
                </p>
            <?php else: ?>
                <p>Số Dư Tài Khoản: N/A</p>
            <?php endif; ?>
        <?php endif; ?>
    </div>
</body>

</html>