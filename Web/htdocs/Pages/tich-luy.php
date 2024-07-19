<?php

require_once '../KhanhDTK/head.php';
if ($_login === null) {
    echo '<script>window.location.href = "../dang-nhap";</script>';
}


?>

<style>
    .thongbao {
        background-color: #f2dede;
        border: 1px solid #ebccd1;
        color: #a94442;
        padding: 15px;
        margin-bottom: 20px;
        border-radius: 4px;
    }
</style>
<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="index" style="color: white">Quay lại diễn đàn</a> </div>
    </div>
</div>
<div class="container pt-5 pb-5">
    <div class="row">
        <div class="col">
            <h4>TÍCH ĐIỂM MỐC NẠP</h4><br>
            <div class="d-inline d-sm-flex justify-content-center">
                <div class="col-md-8 mb-5 mb-sm-4">
                    <div class="d-flex align-items-center justify-content-between"><a href="/ranking"><small
                                class="fw-semibold">Xem
                                ưu đãi</small></a><small class="fw-semibold">Tích lũy:
                            <?php echo $_mocnap; ?>%
                        </small></div>
                    <div class="recharge-progress">
                        <div class="progress-container">
                            <div class="progress-main">
                                <div class="progress-bar" style="width: <?php echo $_mocnap; ?>%;"></div>
                                <div class="progress-bg"></div>
                            </div>
                        </div>
                        <div class="_3Ne69qQgMJvF7eNZAIsp_D">
                            <div class="_38CkBz1hYpnEmyQwHHSmEJ">
                                <div class="NusvrwidhtE2W6NagO43R">
                                    <div class="_1e8_XixJTleoS7HwwmyB-E">
                                        <div class="_2kr5hlXQo0VVTYXPaqefA3 _2Nf9YEDFm2GHONqPnNHRWH" style="left: 1%;">
                                            <div class="_12VQKhFQP9a0Wy-denB6p6">
                                                <div>0</div>
                                                <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                            </div>
                                        </div>
                                        <div class="_2kr5hlXQo0VVTYXPaqefA3" style="left: 10%;">
                                            <div class="_12VQKhFQP9a0Wy-denB6p6">
                                                <div class="_3KQP4x4OyaOj6NIpgE7cKm"><img
                                                        alt="<?php echo $_tenmaychu; ?>" alt=""
                                                        class="_2KchEf_H4jouWwDFDPi5hm" src="/Images/rank/silver.png">
                                                </div>
                                                <div>1Tr</div>
                                            </div>
                                            <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                        </div>
                                        <div class="_2kr5hlXQo0VVTYXPaqefA3" style="left: 20%;">
                                            <div class="_12VQKhFQP9a0Wy-denB6p6">
                                                <div class="_3KQP4x4OyaOj6NIpgE7cKm"><img
                                                        alt="<?php echo $_tenmaychu; ?>" alt=""
                                                        class="_2KchEf_H4jouWwDFDPi5hm" src="/Images/rank/silver.png">
                                                </div>
                                                <div>2Tr</div>
                                            </div>
                                            <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                        </div>
                                        <div class="_2kr5hlXQo0VVTYXPaqefA3" style="left: 50%;">
                                            <div class="_12VQKhFQP9a0Wy-denB6p6">
                                                <div class="_3KQP4x4OyaOj6NIpgE7cKm"><img
                                                        alt="<?php echo $_tenmaychu; ?>" alt=""
                                                        class="_2KchEf_H4jouWwDFDPi5hm" src="/Images/rank/gold.png">
                                                </div>
                                                <div>5Tr</div>
                                            </div>
                                            <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                        </div>
                                        <div class="_2kr5hlXQo0VVTYXPaqefA3" style="left: 100%;">
                                            <div class="_12VQKhFQP9a0Wy-denB6p6">
                                                <div class="_3KQP4x4OyaOj6NIpgE7cKm"><img
                                                        alt="<?php echo $_tenmaychu; ?>" alt=""
                                                        class="_2KchEf_H4jouWwDFDPi5hm" src="/Images/rank/diamond.png">
                                                </div>
                                                <div>10Tr</div>
                                            </div>
                                            <div class="_3toQ_1IrcIyWvRGrIm2fHJ"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container pt-5 pb-5">
        <div class="row pb-2 pt-2">
            <div class="col-lg-6">
                <br><br>
                <b class="text text-danger">Tích Luỹ (Xếp Hạng):</b>
                <br><br>
                <div class="dev">
                    <span class="rounded-sm <?php echo $colorClass; ?> px-3 py-1 text-xs font-medium">
                        <?php echo getUserRole($_mocnap); ?>
                    </span>
                </div>
                <br>
                <b class="text text-danger">Phổ Biến Luật Sự Kiện: </b><br>
                <b>- Đây là Tích Nạp của mỗi cư dân
                    <?php echo $_tenmaychu; ?>
                    <br>
                    - Người chơi phải nạp thành công mới được tính điểm
                    <br>
                    <br>
                    <span style="color:red"><strong>Quan Trọng : <span style="color:212529"></strong></span></span>
                    <br>
                    <b>- Các cư dân lưu ý không <span style="color:red">nhập sai thẻ</span> để tránh bị lỗi thẻ
                        <br>
                        - Tính điểm: Mỗi lần nạp <span style="color:red">100,000 VNĐ</span>, bạn sẽ nhận được <span
                            style="color:red">1 Điểm</span>. Đủ
                        điểm tích luỹ, bạn có thể đổi được GiftCode tương ứng.
                        <br>
                        - Khi bạn đạt đủ <span style="color:red">Điểm tích luỹ</span>, bạn sẽ thấy nút
                        <span style="color:red">Nhận GiftCode</span> hiển thị ở phần bên cạnh.
                    </b>

                </b>
                <br><br>
            </div>
            <div class="col-lg-6 htop border-left">

                <br>
                <h5 class="text-center">Giftcode</h5>
                <p>Giftcode bảo trì</p>
                <style type="text/css">
                    .pagination-custom-style li {
                        display: inline-block;
                        margin-right: 5px;
                    }

                    .pagination-custom-style li:last-child {
                        margin-right: 0;
                    }

                    .table-bordered {
                        border-collapse: collapse;
                        width: 100%;
                    }

                    .table-bordered th,
                    .table-bordered td {
                        border: 1px solid #dee2e6;
                        padding: 8px;
                    }

                    .table-bordered thead th {
                        background-color: #f8f9fa;
                    }

                    .table-bordered tbody tr:hover {
                        background-color: #f5f5f5;
                    }
                </style>
            </div>
        </div>
    </div>
    <?php
    include_once '../KhanhDTK/footer.php';
    ?>
    </body><!-- Bootstrap core JavaScript -->

    </html>