<?php
require_once '../../KhanhDTK/session.php';
require_once '../../KhanhDTK/connect.php';

// Set the appropriate headers for Server-Sent Events
header('Content-Type: text/event-stream');
header('Cache-Control: no-cache');
header('Connection: keep-alive');

// Set a buffer size and flush output
ob_end_clean();
ob_start();
ob_flush();
flush();

// Prepare the SQL query
$stmt = $conn->prepare("SELECT xacminh, thoigian_xacminh FROM user WHERE username = :username");
$stmt->bindParam(":username", $_username, PDO::PARAM_STR);

while (true) {
    $stmt->execute();

    // Fetch the result
    $row = $stmt->fetch(PDO::FETCH_ASSOC);
    $xacminh = $row['xacminh'];
    $thoigian_xacminh = $row['thoigian_xacminh'];

    // Check the 'xacminh' column and calculate the remaining time
    if ($xacminh == 1) {
        $currentTimestamp = time();
        $remainingSeconds = $thoigian_xacminh - $currentTimestamp;
        $remainingMinutes = ceil($remainingSeconds / 60);

        if ($remainingMinutes <= 0) {
            echo "data: Thời gian xác minh đã hết\n\n";

            // Update the 'xacminh' column and 'thoigian_xacminh' column to reset
            $updateStmt = $conn->prepare("UPDATE user SET xacminh = 0, thoigian_xacminh = 0 WHERE username = :username");
            $updateStmt->bindParam(":username", $_username, PDO::PARAM_STR);
            $updateStmt->execute();
        } else {
            echo "data: Thời gian còn lại: $remainingMinutes phút\n\n";
        }
    } else {
        echo "data: \n\n"; // No data to display
    }

    // Flush the output
    ob_flush();
    flush();

    // Wait for a while before checking again (e.g., 10 seconds)
    sleep(10);
}