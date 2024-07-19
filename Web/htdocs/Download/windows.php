<?php

require_once '../KhanhDTK/head.php';
?>
<div class="container color-forum pt-1 pb-1">
    <div class="row">
        <div class="col"> <a href="../home" style="color: white">Quay lại</a> </div>
    </div>
</div>
<div class="container pt-5 pb-5">
    <div class="row">
        <div class="col">
            <h4>Phiên bản PC Windows</h4>

            <!-- Hiển thị liên kết tải phiên bản Android -->
            <?php if ($windows): ?>
                <p>Link Tải Phiên Bản Windows: <a class="text-dark font-weight-bold"
                        href="<?php echo $windows; ?>">Tại đây</a></p>
            <?php else: ?>
                <p>Chưa có liên kết tải phiên bản Windows.</p>
            <?php endif; ?>
            <h5 class="mt-3">Danh sách lệnh chat hỗ trợ</h5>
            - <span class="font-weight-bold">sak</span>: tự động đánh<br>
            - <span class="font-weight-bold">scd</span>: tự động cho đệ đậu thần khi đệ kêu<br>
            - <span class="font-weight-bold">snhat</span>: tự động nhặt vật phẩm khi được đánh rơi từ quái, boss<br>
            - <span class="font-weight-bold">stbb</span>: hiển thị thông báo Boss ra màn hình<br>
            - <span class="font-weight-bold">sinfo</span>: hiển thị thông tin ngươi chơi ra màn hình<br>
            - <span class="font-weight-bold">sspl</span>: hiển thị thông tin số sao pha lê<br>
            - <span class="font-weight-bold">sk x</span>: đổi nhanh khu vực sang khu
            <span class="font-weight-bold">x</span><br>
            (vd: đổi nhanh sang khu vực 5, chat <span class="font-weight-bold">sk 5</span>)<br>
            - <span class="font-weight-bold">stocdo x</span>: tăng tốc độ game lên <span
                class="font-weight-bold">x</span><br> (vd: tăng tốc độ game lên 2, chat <span
                class="font-weight-bold">stocdo 2</span>)<br>
        </div>
    </div>
</div>
<?php
include_once '../KhanhDTK/footer.php';
?>

</body><!-- Bootstrap core JavaScript -->

</html>