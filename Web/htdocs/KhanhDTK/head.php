<?php
require_once 'cauhinh.php';
require_once 'session.php';
require_once 'connect.php';
require_once 'config.php';
?>

<!DOCTYPE html>
<html lang="vi">

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>
        <?php echo $_title; ?>
    </title>
    <meta name="description"
        content="Website chính thức của <?php echo $_title; ?> Online – Game Bay Vien Ngoc Rong Mobile nhập vai trực tuyến trên máy tính và điện thoại về Game 7 Viên Ngọc Rồng hấp dẫn nhất hiện nay!">
    <link rel="stylesheet" type="text/css" href="../Assets/bootstrap.min.css" media="all">
    <link rel="icon" type="image/png" href="../Images/QRQ3R3QRQ3W.png">
    <script defer src="https://kit.fontawesome.com/c79383dd6c.js" crossorigin="anonymous"></script>
    <script async src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
</head>
<div class="container" style="border-radius: 15px; background: #ffaf4c; padding: 0px">
    <div class="container" style="background-color: #e67e22; border-radius: 15px 15px 0px 0px">
        <div class="row bg pb-3 pt-2">
            <div class="col">
                <div class="text-center mb-2">
                    <a href="../home" title="Trang chủ">
                        <img alt="<?php echo $_tenmaychu; ?>" src="../Images/QRQ3R3QRQ3W.png" id="logo" width="100"
                            height="250">
                    </a>

                </div>
                <div class="text-center pt-2">
                    <div style="display: inline-block;">
                        <a href="../android"> <img alt="<?php echo $_tenmaychu; ?>" class="icon-download"
                                style="object-fit: contain;" width="100" height="50"
                                src="../Images/android.svg"></a><br>
                        <small>
                            <?php echo $_android; ?>
                        </small>
                    </div>
                    <div style="display: inline-block;">
                        <a href="../windows"><img alt="<?php echo $_tenmaychu; ?>" class="icon-download"
                                style="object-fit: contain;" width="100" height="50" src="../Images/pc.svg">
                        </a><br>
                        <small>
                            <?php echo $_windows; ?>
                        </small>
                    </div>
                    <div style="display: inline-block;">
                        <a href="../iphone"><img alt="<?php echo $_tenmaychu; ?>" class="icon-download"
                                style="object-fit: contain;" width="100" height="50" src="../Images/ip.svg"></a><br>
                        <small>
                            <?php echo $_iphone; ?>
                        </small>
                    </div>
                    <div style="display: inline-block;">
                        <a href="../jar"><img alt="<?php echo $_tenmaychu; ?>" class="icon-download"
                                style="object-fit: contain;" width="100" height="50" src="../Images/jar.svg"></a><br>
                        <small>
                            <?php echo $_java; ?>
                        </small>
                    </div>
                    <div style="display: block;">
                        <img alt="<?php echo $_tenmaychu; ?>" style="object-fit: contain;" witdh="100" height="12"
                            src="../Images/12.png">
                        <small style="font-size: 10px">Dành cho người chơi trên 12 tuổi. Chơi quá 180
                            phút mỗi ngày sẽ hại sức khỏe.</small>
                    </div>
                </div>
            </div>
        </div>
        <?php
        if ($_login === null) {
            ?>
            <div class="container pb-2">
                <div class="text-center">
                    <div class="row">
                        <div class="col pr-0"> <a href="../dang-nhap" class="btn p-1 btn-header">Đăng nhập</a> </div>
                        <div class="col pr-0"> <a href="../dang-ky" class="btn p-1 btn-header">Đăng ký</a> </div>
                    </div>
                </div>
            </div>
        </div>
        <?php
        } else {
            ?>
        <div class="container pb-2">
            <div class="text-center">
                <div class="row">
                    <div class="col pr-0"> <a href="../home" class="btn p-1 btn-header">Thảo
                            luận</a> </div>
                    <div class="col pr-0"> <a href="../bao-loi" class="btn p-1 btn-header">Báo lỗi</a> </div>
                    <div class="col"> <a href="../gop-y" class="btn p-1 btn-header">Góp ý</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container pt-4 pb-1 color-main4">
        <div class="text-center">
            <?php
            $avatar_url = getUserAvatar($_admin, $_gender);

            $adminHtml = $_admin ? '<span class="text-danger font-weight-bold">' . $_name . '<br><i class="fas fa-star"></i></span>' : '<p class="text-main pt-1 mb-0">' . $_name . '</p>';

            echo '<div>
                <img alt="<?php echo $_tenmaychu; ?>"  src="' . $avatar_url . '" alt="Avatar" style="width: 50px; max-height: 80px;"></div>' . $adminHtml . '<p class="text-main pt-1 mb-0"></p>
                <p class="pt-0">Số dư: ' . number_format($_coin, 0, ',') . ' VNĐ | Coin: ' . $coins . '</p>';
            ?>
            <div class="text-center mb-2">
                <?php if ($_admin == 1): ?>
                    <a class="btn btn-main btn-sm" href="../cpanel/home">CPanel</a>
                <?php endif; ?>
                <a class="btn btn-main btn-sm" href="../card">Nạp số dư</a>
             
            </div>
            <div class="text-center mb-2">
                <a class="btn btn-main btn-sm" href="../changepass">Đổi mật khẩu</a>
                <a class="btn btn-main btn-sm" href="../update">Bảo Mật</a>
                <a class="btn btn-main btn-sm" href="../logout">Thoát</a>
            </div>
        </div>
    </div>
    <?php
        }
        ?>