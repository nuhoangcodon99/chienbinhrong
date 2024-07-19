<?php
require_once 'KhanhDTK/head.php';
?>
<div class="container color-forum pt-2">
    <div class="row">
        <div class="col">
            <div class="box-stt">
                <div style="width: 40px; float:left; margin-right: 5px;">
                    <img alt="<?php echo $_tenmaychu; ?>" src="Images/avatar3.png" style="width: 30px; height: 35px;"
                        loading="lazy">
                </div>
                <div class="box-right">
                    <a href="pages/mo-thanh-vien" class="important"> Mở Thành Viên Máy Chủ <img
                            alt="<?php echo $_tenmaychu; ?>" src="Images/hot.gif" style="width: 20px; height: 10px;"
                            loading="lazy"></a>
                    <div class="box-name" style="font-size: 9px;"> bởi ADMIN </div>
                </div>
            </div>
            <div class="box-stt">
                <div style="width: 40px; float:left; margin-right: 5px;">
                    <img alt="<?php echo $_tenmaychu; ?>" src="Images/avatar6.png" style="width: 30px; height: 35px;"
                        loading="lazy">
                </div>
                <div class="box-right">
                    <a href="/power" class="important"> Bảng Xếp Hạng Đua Top <img alt="<?php echo $_tenmaychu; ?>"
                            src="Images/hot.gif" style="width: 20px; height: 5px;" loading="lazy"></a>
                    <div class="box-name" style="font-size: 9px;"> bởi Admin </div>
                </div>
            </div>
            <div class="box-stt">
                <div style="width: 40px; float:left; margin-right: 5px;">
                    <img alt="<?php echo $_tenmaychu; ?>" src="Images/avatar12.png" style="width: 30px; height: 35px;"
                        loading="lazy">
                </div>
                <div class="box-right">
                    <a href="/donate" class="important"> Bảng Xếp Hạng Nạp <img alt="<?php echo $_tenmaychu; ?>"
                            src="Images/hot.gif" style="width: 20px; height: 10px;" loading="lazy"></a>
                    <div class="box-name" style="font-size: 9px;"> bởi Admin </div>
                </div>

            </div>
            <?php
            $query = "SELECT posts.*, JSON_EXTRACT(`character`.infoChar, '$.Gender') AS Gender, MAX(user.admin) AS admin, COUNT(comments.id) AS comment_count FROM posts
        LEFT JOIN `character` ON posts.username = `character`.Name COLLATE utf8mb4_general_ci
        LEFT JOIN user ON `character`.id = user.`character`
        LEFT JOIN comments ON posts.id = comments.post_id
        WHERE posts.username = `character`.Name COLLATE utf8mb4_general_ci AND posts.ghimbai = 1 AND user.admin = 1
            GROUP BY posts.id, posts.tieude, posts.tinhtrang, posts.username, `character`.infoChar ORDER BY posts.id DESC;";


            $stmt = $conn->prepare($query);
            $stmt->execute();
            $rows = $stmt->fetchAll(PDO::FETCH_ASSOC);

            foreach ($rows as $row) {
                $Gender = $row['Gender'];
                $admin = $row['admin'];
                $avatar_url = getUserAvatar($admin, $Gender);

                echo '<div class="box-stt"><div style="width: 40px; float:left; margin-right: 5px;"><img alt="' . $_tenmaychu . '"  src="' . $avatar_url . '" style="width: 30px; max-height: 35px;" loading="lazy"></div>';
                echo '<div class="box-right">';

                $link = 'forum?id=' . $row['id'];

                if ($admin == 1) {
                    $tinhtrang = ($row['tinhtrang'] == 1) ? 'text-success' : (($row['tinhtrang'] == 2) ? 'text-primary' : 'text-info');
                    echo '<a href="' . $link . '"><span class="text-danger font-weight-bold">' . $row['tieude'] . '</span></a>';
                    $admin_text = ($row['tinhtrang'] == 1) ? 'Đã trả lời' : (($row['tinhtrang'] == 2) ? 'Đã ghi nhận' : 'Đang thảo luận');
                    echo '<div class="box-name" style="font-size: 9px;"> bởi <span class="text-danger font-weight-bold">' . $row['username'] . ' ' . ($admin == 1 ? '<i class="fas fa-star"></i>' : '') . '</span><span class=' . $tinhtrang . '> ' . $admin_text . '</span></div>';
                } else {
                    echo '<a href="' . $link . '">' . $row['tieude'] . '</a>';
                    echo '<div class="box-name" style="font-size: 9px;"> bởi ' . $row['username'] . '</div>';
                }

                echo '</div></div>';
            }
            ?>
        </div>
    </div>
</div>
<div class="container color-forum2 pt-2">
    <div class="row">
        <div class="col">
            <?php
            // Tính toán số lượng bài viết
            $query_count = "SELECT COUNT(*) AS count FROM posts";
            $result_count = $conn->query($query_count);
            $row_count = $result_count->fetch(PDO::FETCH_ASSOC);
            $count = $row_count['count'];

            // Thiết lập giới hạn cho mỗi trang
            $limit = 20;

            // Tính toán số lượng trang
            $total_pages = ceil($count / $limit);

            // Lấy số trang từ tham số URL
            $page = isset($_GET['page']) ? intval($_GET['page']) : 1;

            // Xác định vị trí của trang hiện tại trong danh sách các trang
            $page_position = min(max(1, $page - 1), max(1, $total_pages - 2));

            // Tính toán giới hạn kết quả truy vấn theo biến $limit và $page
            $offset = ($page - 1) * $limit;
            $query = "SELECT posts.*, JSON_EXTRACT(`character`.infoChar, '$.Gender') AS Gender, user.admin FROM posts
      LEFT JOIN `character` ON posts.username = `character`.Name COLLATE utf8mb4_general_ci
      LEFT JOIN user ON `character`.id = user.`character`
      WHERE posts.username = `character`.Name COLLATE utf8mb4_general_ci
      ORDER BY posts.id DESC LIMIT :limit OFFSET :offset";

            $stmt = $conn->prepare($query);
            $stmt->bindValue(':limit', $limit, PDO::PARAM_INT);
            $stmt->bindValue(':offset', $offset, PDO::PARAM_INT);
            $stmt->execute();
            $rows = $stmt->fetchAll(PDO::FETCH_ASSOC);

            foreach ($rows as $row) {
                // Kiểm tra cột ghimbai có giá trị là 1 hay không
                if ($row['ghimbai'] == 1) {
                    continue; // Bỏ qua bài viết có cột ghimbai bằng 1
                }
                $link = 'forum?id=' . $row['id'];

                // Retrieve user avatar and name
                $Gender = $row['Gender'];
                $admin = $row['admin'];
                $avatar_url = getUserAvatar($admin, $Gender);

                // Display post title and author name
                echo '<div class="box-stt"><div style="width: 30px; float:left; margin-right: 5px;"><img alt="<?php echo $_tenmaychu; ?>"  src="' . $avatar_url . '" style="width: 25px; max-height: 40px;" loading="lazy"></div>';
                echo '<div class="box-right">';

                if ($admin == 1) {
                    $tinhtrang = ($row['tinhtrang'] == 1) ? 'text-success' : (($row['tinhtrang'] == 2) ? 'text-primary' : 'text-info');
                    echo '<a href="' . $link . '"><span class="text-danger font-weight-bold">' . $row['tieude'] . '</span></a>';
                    $admin_text = ($row['tinhtrang'] == 1) ? 'Đã trả lời' : (($row['tinhtrang'] == 2) ? 'Đã ghi nhận' : 'Đang thảo luận');
                    echo '<div class="box-name" style="font-size: 9px;"> bởi <span class="text-danger font-weight-bold">' . $row['username'] . ' ' . ($admin == 1 ? '<i class="fas fa-star"></i>' : '') . '</span><span class=' . $tinhtrang . '>' . $admin_text . '</span></div>';
                } else {
                    echo '<a href="' . $link . '">' . $row['tieude'] . '</a>';
                    echo '<div class="box-name" style="font-size: 9px;"> bởi ' . $row['username'] . '</div>';
                }

                echo '</div></div>';
            }
            ?>
        </div>
    </div>
</div>
<div class="container pb-2">
    <div class="row mt-3">
        <div class="col-5">
            <?php
            if ($_login === null) {

            } else {
                echo '<a class="btn btn-light btn-sm" href="post">Đăng bài mới</a>';
            }
            ?>
        </div>
        <?php
        echo '<div class="col-7 text-right">';
        echo '<ul class="pagination" style="justify-content: flex-end;">';
        if ($page > 1) {
            echo '<li><a class="btn btn-trangdem btn-light" href="?page=' . ($page - 1) . '"><</a></li>';
        }
        $start_page = max(1, min($total_pages - 2, $page - 1));
        $end_page = min($total_pages, max(2, $page + 1));
        for ($i = 1; $i <= $total_pages; $i++) {
            if ($i >= $start_page && $i <= $end_page) {
                $class_name = "btn btn-trangdem btn-light";
                if ($i == $page) {
                    $class_name = "btn btn-trangdem page-active";
                }
                echo '<li><a class="' . $class_name . '" href="?page=' . $i . '">' . $i . '</a></li>';
            }
        }
        if ($page < $total_pages) {
            echo '<li><a class="btn btn-trangdem btn-light" href="?page=' . ($page + 1) . '">></a></li>';
        }
        echo '</ul>';
        echo '</div>';
        ?>
    </div>
</div>
<?php
include_once 'KhanhDTK/footer.php';
?>
</div>
</div>
</body>

</html>