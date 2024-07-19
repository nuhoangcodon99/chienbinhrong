<?php
ob_start();

require_once '../KhanhDTK/head.php';
if (isset($_GET['id'])) {
    ?>
    <style>
        .link {
            color: blue;
            text-decoration: underline;
        }

        .img-thumbnail {
            padding: 0.25rem;
            background-color: #fff;
            border: 1px solid #dee2e6;
            border-radius: 0.25rem;
            max-width: 100%;
            height: auto;
        }

        img {
            vertical-align: middle;
            border-style: none;
        }
    </style>

    <body>
        <div class="container color-forum pt-1 pb-1">
            <div class="row">
                <div class="col"> <a href="../home" style="color: white">Quay lại</a>
                </div>
            </div>
        </div>
        <div class="container pt-5 pb-5">
            <div class="row">
                <div class="col">
                    <table cellpadding="0" cellspacing="0" width="99%" style="font-size: 13px;">
                        <tbody>
                            <tr>
                                <td width="60px;" style="vertical-align: top">
                                    <div class="text-center" style="margin-left: -10px;">
                                        <br>
                                        <div style="font-size: 9px; padding-top: 5px">
                                            <?php
                                            // Xử lý lấy thông tin bài viết từ CSDL
                                            $query = "SELECT posts.*, JSON_EXTRACT(`character`.infoChar, '$.Gender') AS Gender, user.tichdiem, user.admin, posts.image, posts.tinhtrang FROM posts
                                    LEFT JOIN `character` ON posts.username = `character`.Name COLLATE utf8mb4_general_ci
                                    LEFT JOIN user ON `character`.id = user.`character`
                                    WHERE posts.id = :post_id";

                                            $stmt = $conn->prepare($query);
                                            $stmt->bindParam(":post_id", $_postsid, PDO::PARAM_INT);
                                            $stmt->execute();
                                            $row = $stmt->fetch(PDO::FETCH_ASSOC);

                                            if ($row) {
                                                $gender = $row['Gender'];
                                                $hanhtinh = $row['Gender'];
                                                $tichdiem = $row['tichdiem'];

                                                // Lấy Avatar và tên của người dùng
                                                $admin = $row['admin'];
                                                $tinhtrang = $row['tinhtrang'];
                                                $avatar_url = getUserAvatar($admin, $gender);

                                                $name_hanhtinh = "";
                                                if ($hanhtinh == 1) {
                                                    $name_hanhtinh = "(Namec)";
                                                } elseif ($hanhtinh == 2) {
                                                    $name_hanhtinh = "(Xayda)";
                                                } else {
                                                    $name_hanhtinh = "(Trái Đất)";
                                                }

                                                echo '<div class="text-center"><img alt="<?php echo $_tenmaychu; ?>"  src="' . $avatar_url . '" alt="Avatar" style="width: 30px"><br></div>';
                                                if ($row['admin'] == 1) {
                                                    echo '<span class="text-danger font-weight-bold">' . $row['username'] . '</span><br>';
                                                    echo '<span class="text-danger pt-1 mb-0"><i class="fas fa-star"></i></span>';
                                                    echo '<div style="font-size: 8px">Điểm:' . $tichdiem;
                                                } else {
                                                    echo '<div style="font-size: 9px; padding-top: 5px">' . $row['username'] . '</div>';
                                                    echo '<div style="font-size: 8px">Điểm:' . $tichdiem;
                                                }
                                                echo '</div>';
                                                echo '</div>';
                                                echo '</td>';
                                                echo '<td class="bg bg-light" style=" border-radius: 7px">';
                                                echo '<div class="row" style="padding: 0 7px 15px 7px">';
                                                $created_at = strtotime($row['created_at']);
                                                $now = time();
                                                $time_diff = $now - $created_at;
                                                echo '<div class="col"><div style="font-size: 9px; padding-top: 5px">';
                                                if ($time_diff < 60) {
                                                    echo $time_diff . ' giây trước';
                                                } elseif ($time_diff < 3600) {
                                                    echo floor($time_diff / 60) . ' phút trước';
                                                } elseif ($time_diff < 86400) {
                                                    echo floor($time_diff / 3600) . ' giờ trước';
                                                } elseif ($time_diff < 2592000) {
                                                    echo floor($time_diff / 86400) . ' ngày trước';
                                                } elseif ($time_diff < 31536000) {
                                                    echo floor($time_diff / 2592000) . ' tháng trước';
                                                } else {
                                                    echo floor($time_diff / 31536000) . ' năm trước';
                                                }
                                                echo '<span style="float: right;">';

                                                // Kiểm tra và hiển thị chức năng "Đã trả lời" nếu có quyền admin và người dùng đã đăng nhập
                                                if ($_login === null) {

                                                } else {
                                                    // Kiểm tra và hiển thị chức năng "Đã trả lời"
                                                    $ghinhan = ($tinhtrang == 2);
                                                    $datraloi = ($tinhtrang == 1);
                                                    $thaoluan = ($tinhtrang == 0);

                                                    if ($_admin == 1) {
                                                        if ($tinhtrang == 0) {
                                                            if (!$ghinhan) {
                                                                echo '<a href="../request/tinhtrang?id=' . $_postsid . '&tinhtrang=2">[Ghi nhận]</a> ';
                                                            }
                                                            if (!$datraloi) {
                                                                echo '<a href="../request/tinhtrang?id=' . $_postsid . '&tinhtrang=1">[Đã trả lời]</a> ';
                                                            }
                                                        } else if ($tinhtrang == 1) {
                                                            if (!$ghinhan) {
                                                                echo '<a href="../request/tinhtrang?id=' . $_postsid . '&tinhtrang=2">[Ghi nhận]</a> ';
                                                            }
                                                            if (!$thaoluan) {
                                                                echo '<a href="../request/tinhtrang?id=' . $_postsid . '&tinhtrang=0">[Chưa trả lời]</a> ';
                                                            }
                                                        } else if ($tinhtrang == 2) {
                                                            if (!$datraloi) {
                                                                echo '<a href="../request/tinhtrang?id=' . $_postsid . '&tinhtrang=1">[Đã trả lời]</a> ';
                                                            }
                                                            if (!$thaoluan) {
                                                                echo '<a href="../request/tinhtrang?id=' . $_postsid . '&tinhtrang=0">[Chưa trả lời]</a> ';
                                                            }
                                                        }
                                                    }
                                                }

                                                echo '#0</span>';
                                                echo '</div>';

                                                echo '<div class="col"><span class="font-weight-bold">' . $row['tieude'] . '</span>';
                                                echo '<br>';
                                                $content = $row['noidung'];
                                                $content = preg_replace('/(https?:\/\/[^\s]+(\.[^\s]+)+)/', '<a href="$1" class="link">$1</a>', $content);
                                                echo '<span style="white-space: pre-wrap;">' . $content . '</span><br>';

                                                $image_filenames = null;
                                                if (!empty($row['image'])) {
                                                    $image_filenames = explode(',', $row['image']);
                                                }

                                                if (is_array($image_filenames) && !empty($image_filenames)) {
                                                    foreach ($image_filenames as $image_filename) {
                                                        $image_path = "../Images/uploads/" . trim($image_filename);
                                                        if (file_exists($image_path)) {
                                                            echo '<img alt="Nguyen Duc Kien"  src="' . $image_path . '" alt="Ảnh" class="img-thumbnail">';
                                                        } else {
                                                            echo 'Không tìm thấy hình ảnh';
                                                        }
                                                    }
                                                }

                                            }
                                            ?>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <?php

        // Tính toán số lượng comment cho bài viết hiện tại
        $query = "SELECT COUNT(*) AS count FROM comments WHERE post_id = :post_id";
        $stmt = $conn->prepare($query);
        $stmt->bindParam(":post_id", $_postsid, PDO::PARAM_INT);
        $stmt->execute();
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        $total_comments = $row['count'];

        // Thiết lập giới hạn cho mỗi trang
        $limit = 3;

        // Tính toán số lượng trang
        $total_pages = ceil($total_comments / $limit);

        // Calculate the highest page number
        $highest_page = max(1, $total_pages);

        // Lấy số trang từ tham số URL, nếu không có thì sử dụng trang cao nhất
        $page = isset($_GET['page']) ? intval($_GET['page']) : $highest_page;

        // Xác định vị trí của trang hiện tại trong danh sách các trang
        $page_position = min(max(1, $page), max(1, $total_pages - 2));

        // Tính toán giới hạn kết quả truy vấn theo biến $limit và $page
        $offset = max(0, ($page - 1) * $limit);

        // Query the `comments` table to retrieve all comments for the current post, along with the author name
        $query = "SELECT nguoidung, traloi, created_at, gender, image FROM comments WHERE post_id = :post_id ORDER BY comments.post_id ASC LIMIT :limit OFFSET :offset";
        $stmt = $conn->prepare($query);
        $stmt->bindParam(":post_id", $_postsid, PDO::PARAM_INT);
        $stmt->bindParam(":limit", $limit, PDO::PARAM_INT);
        $stmt->bindParam(":offset", $offset, PDO::PARAM_INT);
        $stmt->execute();
        $comments = $stmt->fetchAll();

        // Hiển thị số thứ tự của bình luận
        $count_start = ($page - 1) * $limit; // Tính toán giá trị ban đầu cho $count
        $count = $count_start; // Gán giá trị ban đầu cho $count
        ?>
        <div class="container pt-5 pb-5">
            <?php
            foreach ($comments as $comment):
                $count++; // Tăng giá trị của $count cho mỗi bình luận
                ?>
                <div class="row pt-3">
                    <div class="col-md-12">
                        <table cellpadding="0" cellspacing="0" width="99%" style="font-size: 13px;">
                            <tbody>
                                <tr>
                                    <td width="60px;" style="vertical-align: top">
                                        <div class="text-center" style="margin-left: -10px;">
                                            <div style="font-size: 9px; padding-top: 5px">
                                                <?php
                                                // Lấy Avatar và tên người dùng
                                                $gender = $comment['gender'];
                                                $nguoidung = $comment['nguoidung'];

                                                // Lấy thông tin tài khoản và điểm tích lũy
                                                $sql = "SELECT user.tichdiem, user.admin, `character`.id FROM user INNER JOIN `character` ON `character`.id = user.`character` WHERE `character`.Name COLLATE utf8mb4_general_ci = ?";
                                                $stmt = $conn->prepare($sql);
                                                $stmt->bindParam(1, $nguoidung, PDO::PARAM_STR);
                                                $stmt->execute();
                                                $result = $stmt->fetch(PDO::FETCH_ASSOC);

                                                if ($stmt->rowCount() > 0) {
                                                    $tichdiem = intval($result['tichdiem']);
                                                    $admin = $result['admin'];
                                                    $account_id = $result['id'];

                                                    $avatar_url = getUserAvatar($admin, $gender);

                                                    // Hiển thị avatar và tên người dùng
                                                    echo '<img alt="<?php echo $_tenmaychu; ?>"  src="' . $avatar_url . '" alt="Avatar" style="width: 30px">';
                                                    echo '<p>';

                                                    $query = "SELECT DISTINCT posts.*, user.admin, `character`.id FROM posts LEFT JOIN `character` ON posts.username = `character`.Name COLLATE utf8mb4_general_ci LEFT JOIN user ON `character`.id = user.`character` WHERE posts.username = ? ORDER BY posts.id DESC";
                                                    $stmt = $conn->prepare($query);
                                                    $stmt->bindParam(1, $nguoidung, PDO::PARAM_STR);
                                                    $stmt->execute();
                                                    $result2 = $stmt->fetchAll();

                                                    if ($admin == 1) {
                                                        // Nếu tìm thấy giá trị 'admin' bằng 1 trong vòng lặp foreach, hiển thị tên người dùng với chữ màu đỏ
                                                        echo '<span class="text-danger font-weight-bold">' . $nguoidung . '</span><br>';
                                                        echo '<span class="text-danger pt-1 mb-0"><i class="fas fa-star"></i></span><br>';

                                                    } else {
                                                        // Nếu không tìm thấy giá trị 'admin' bằng 1 hoặc biến $row không tồn tại, hiển thị tên người dùng với chữ màu đen.
                                                        echo '<span>' . $nguoidung . '</span><br>';
                                                    }

                                                    echo '<span style="font-size: 8px">Điểm: ' . $tichdiem . '</span>';
                                                }
                                                ?>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="bg bg-light" style="border-radius: 7px">
                                        <div class="row" style="padding: 0 7px 15px 7px">
                                            <div class="col">
                                                <small>
                                                    <?php
                                                    $created_at = strtotime($comment['created_at']);
                                                    $now = time();
                                                    $time_diff = $now - $created_at;
                                                    if ($time_diff < 60) {
                                                        echo $time_diff . ' giây trước';
                                                    } elseif ($time_diff < 3600) {
                                                        echo floor($time_diff / 60) . ' phút trước';
                                                    } elseif ($time_diff < 86400) {
                                                        echo floor($time_diff / 3600) . ' giờ trước';
                                                    } elseif ($time_diff < 2592000) {
                                                        echo floor($time_diff / 86400) . ' ngày trước';
                                                    } elseif ($time_diff < 31536000) {
                                                        echo floor($time_diff / 2592000) . ' tháng trước';
                                                    } else {
                                                        echo floor($time_diff / 31536000) . ' năm trước';
                                                    }

                                                    echo '<span style="float: right; font-size: 9px;">';

                                                    echo '<span style="float: right;">#' . $count . '</span>'; // Hiển thị số thứ tự của bình luận
                                                    echo '</span>';
                                                    ?>
                                                </small>
                                                <p class="text-dark pt-1 pb-1 mb-1">
                                                    <?php
                                                    $content = $comment['traloi'];

                                                    $content = preg_replace_callback('/(https?:\/\/[^\s]+(\.[^\s]+)+)/', function ($matches) {
                                                        $url = $matches[0];
                                                        return '<a href="' . $url . '" class="link">' . (filter_var($url, FILTER_VALIDATE_URL) ? $url : substr($url, 0, -1)) . '</a>';
                                                    }, $content);


                                                    echo '<span style="white-space: pre-wrap;">' . $content . '</span><br>';
                                                    ?>
                                                </p>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            <?php endforeach; ?>
            <?php
            if ($_login === null) {
                ?>
                <br>
                <div class="container pb-2">
                    <div class="row mt-3">
                        <div class="col-5">
                        </div>
                        <?php
            } else { // Lấy id bài viết từ URL

                // Hiển thị pagination
                echo '<div class="row">';
                echo '<div class="col text-right">';

                if ($page > 1) {
                    echo '<a class="btn btn-light btn-sm" href="forum?id=' . $_postsid . '&page=' . ($page - 1) . '"> &lt; </a>';
                }

                $start_page = max(1, min($total_pages - 2, $page - 1));
                $end_page = min($total_pages, max(2, $page + 1));

                for ($i = 1; $i <= $total_pages; $i++) {
                    if ($i >= $start_page && $i <= $end_page) {
                        $class_name = "btn btn-light btn-sm";
                        if ($i == $page) {
                            $class_name = "btn btn-sm page-active";
                        }
                        echo '<a class="' . $class_name . '" href="forum?id=' . $_postsid . '&page=' . $i . '">' . $i . '</a>';
                    }
                }

                if ($page < $total_pages) {
                    echo '<a class="btn btn-light btn-sm" href="forum?id=' . $_postsid . '&page=' . ($page + 1) . '"> &gt; </a>';
                }

                echo '</div>';
                echo '</div>';
                ?>
                    </div>
                    <div class="border-secondary border-top"></div>
                    <br>
                    <table cellpadding="0" cellspacing="0" width="99%" style="font-size: 13px;">
                        <tbody>
                            <tr>
                                <table cellpadding="0" cellspacing="0" width="99%" style="font-size: 13px;">
                                    <tbody>
                                        <tr>
                                            <td width="55px;" style="vertical-align: top">
                                                <div class="text-left" style="display: block;">
                                                    <?php
                                                    if (isset($_GET["id"])) {
                                                        if ($_trangthai == 0 && isset($_gender) && isset($_admin)) {

                                                            $avatar_url = getUserAvatar($admin, $gender);

                                                            echo '<img alt="<?php echo $_tenmaychu; ?>"  src="' . $avatar_url . '" alt="Avatar" style="width: 45px">';
                                                        }
                                                    }
                                                    ?>
                                                    <br>
                                                </div>
                                            </td>
                                            <td style="border-radius: 7px">
                                                <div class="row">
                                                    <div class="col">
                                                        <?php
                                                        ob_start(); // start buffering output
                                                        if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['traloi'])) {
                                                            $comment = filter_var($_POST['traloi'], FILTER_SANITIZE_STRING);

                                                            if (isset($_GET['id'])) {
                                                                $id = intval($_GET['id']);

                                                                // Lưu thông tin bình luận vào cơ sở dữ liệu (không có tệp tin ảnh)
                                                                $insert_stmt = $conn->prepare("INSERT INTO comments (post_id, nguoidung, gender, traloi) VALUES (?, ?, ?, ?)");
                                                                $insert_stmt->bindParam(1, $id, PDO::PARAM_INT);
                                                                $insert_stmt->bindParam(2, $_name, PDO::PARAM_STR);
                                                                $insert_stmt->bindParam(3, $_gender, PDO::PARAM_STR);
                                                                $insert_stmt->bindParam(4, $comment, PDO::PARAM_STR);
                                                                $insert_stmt->execute();
                                                            }
                                                        }

                                                        ob_end_flush();
                                                        ?>
                                                        <?php
                                                        if ($_trangthai == 0) {
                                                            ?>
                                                            <form id="form" method="POST" enctype="multipart/form-data" action="">
                                                                <div class="form-group position-relative">
                                                                    <div class="input-group">
                                                                        <textarea class="form-control" type="text" name="traloi"
                                                                            id="traloi" placeholder="Nhập bình luận của bạn..."
                                                                            required></textarea>
                                                                    </div>
                                                                </div>
                                                                <button class="btn btn-light btn-sm" id="btn-cmt" type="submit">Bình
                                                                    luận</button>
                                                            </form>
                                                            <?php
                                                        }
                                                        ?>
                                                        <br>
                                                        <script>

                                                            // Xử lý sự kiện khi bình luận được gửi
                                                            document.getElementById('form').addEventListener('submit', function (event) {
                                                                // Prevent form submission if $_status is 0
                                                                if (<?php echo $_status; ?> === 0) {
                                                                    event.preventDefault();
                                                                    var thongbaoElement = document.createElement('div');
                                                                    thongbaoElement.innerHTML = '<span class="text-danger pb-2">Thông Báo:</span> Yêu cầu mở thành viên để sử dụng chức năng này.<br><br>';
                                                                    document.getElementById('form').prepend(thongbaoElement);
                                                                    return;
                                                                }

                                                                event.preventDefault(); // Ngăn chặn gửi biểu mẫu một cách tự động

                                                                // Tạo một đối tượng FormData để chứa dữ liệu biểu mẫu
                                                                var formData = new FormData(this);

                                                                // Gửi yêu cầu AJAX
                                                                var xhr = new XMLHttpRequest();
                                                                xhr.open('POST', this.action, true);
                                                                xhr.onload = function () {
                                                                    if (xhr.status === 200) {
                                                                        // Xử lý thành công, tải lại trang
                                                                        window.location.href = 'forum?id=<?php echo $_postsid; ?>';
                                                                    } else {
                                                                        // Xử lý lỗi
                                                                        console.log('Đã xảy ra lỗi: ' + xhr.status);
                                                                    }
                                                                };

                                                                xhr.send(formData);
                                                            });
                                                        </script>
                                                        <?php
            }
}
ob_start();

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_POST['delete_post']) || isset($_POST['pin_post']) || isset($_POST['delete_pin_post']) || isset($_POST['block_comments']) || isset($_POST['unlock_comments'])) {
        $_postsid = isset($_GET['id']) ? intval($_GET['id']) : intval($_POST['post_id']);

        if ($_admin || ($_id && $_id === $post_author_id)) {
            if (isset($_POST['delete_post'])) {
                if (deletePostAndComments($conn, $_postsid)) {
                    header("Location: /home");
                    exit();
                } else {
                    echo "Error: Failed to delete post or comments.";
                }
            } elseif (isset($_POST['pin_post'])) {
                if (updatePostStatus($conn, $_postsid, 'ghimbai', 1)) {
                    header("Location: /forum?id=" . $_postsid);
                    exit();
                } else {
                    echo "Error: Failed to pin post.";
                }
            } elseif (isset($_POST['delete_pin_post'])) {
                if (updatePostStatus($conn, $_postsid, 'ghimbai', 0)) {
                    header("Location: /forum?id=" . $_postsid);
                    exit();
                } else {
                    echo "Error: Failed to unpin post.";
                }
            } elseif (isset($_POST['block_comments'])) {
                if (updatePostStatus($conn, $_postsid, 'trangthai', 1)) {
                    header("Location: /forum?id=" . $_postsid);
                    exit();
                } else {
                    echo "Error: Failed to block comments.";
                }
            } elseif (isset($_POST['unlock_comments'])) {
                if (updatePostStatus($conn, $_postsid, 'trangthai', 0)) {
                    header("Location: /forum?id=" . $_postsid);
                    exit();
                } else {
                    echo "Error: Failed to unlock comments.";
                }
            }
        } else {
            echo "Error: You don't have permission to perform this action.";
        }
    } else {
        echo "Error: Post not found.";
    }
}


if (isset($_admin) && $_admin == 1) {
    ?>
                                                    <form method="POST">
                                                        <button class="btn btn-light btn-sm" id="btn-delete"
                                                            name="delete_post" type="submit">Xoá Bài</button>
                                                        <button class="btn btn-light btn-sm" id="btn-pin" name="pin_post"
                                                            type="submit">Ghim Bài</button>
                                                        <button class="btn btn-light btn-sm" id="btn-delete-pin"
                                                            name="delete_pin_post" type="submit">Bỏ Ghim</button>
                                                        <button class="btn btn-light btn-sm" id="btn-block-comments"
                                                            name="block_comments" type="submit">Chặn Bình Luận</button>
                                                        <button class="btn btn-light btn-sm" id="btn-unlock-comments"
                                                            name="unlock_comments" type="submit">Mở Bình Luận</button>
                                                        <input type="hidden" name="post_id"
                                                            value="<?php echo $_postsid; ?>">
                                                    </form>
                                                    <?php
}
?>
                                                <script>
                                                    const form = document.querySelector('#form');
                                                    const submitBtn = form.querySelector('#btn-cmt');
                                                    const submitError = form.querySelector('#notify');
                                                    const traloiInput = document.getElementById('traloi');
                                                    form.addEventListener('submit', (event) => {
                                                        const traloi = traloiInput.value.trim().length;
                                                        if (traloi < script 1) {
                                                        event.preventDefault();
                                                        submitError.innerHTML =
                                                            '<strong>Lỗi:</strong> Bình luận phải có ít nhất 1 ký tự!';
                                                        submitError.style.display = 'block';
                                                        submitBtn.scrollIntoView({
                                                            behavior: 'smooth',
                                                            block: 'start'
                                                        });
                                                    }
                                                                                                                                                                                                                                                                                                                                                                                                                                 });
                                                    traloiInput.addEventListener('keydown', (event) => {
                                                        if (event.keyCode === 13 && !event.shiftKey) {
                                                            event.preventDefault();
                                                            submitBtn.click();
                                                        }
                                                    });
                                                </script>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <?php
                        include_once '../KhanhDTK/footer.php';
                        ?>
        </div>
    </div>
    </div>
    </div>
    </div>
</body>

</html>

</script>