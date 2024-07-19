<?php
function _query($sql)
{
    global $conn;
    return $conn !== null ? $conn->query($sql) : null;
}


function _fetch($sql)
{
    return _query($sql)->fetch(PDO::FETCH_ASSOC);
}


function _select($select, $from, $where)
{
    return "SELECT $select FROM $from WHERE $where";
}

function _update($table, $set, $where)
{
    global $conn;

    if ($conn === null) {
        // Xử lý lỗi kết nối
        return false;
    }

    try {
        $update_sql = "UPDATE $table SET $set WHERE $where";
        $stmt = $conn->prepare($update_sql);
        return $stmt->execute();
    } catch (PDOException $e) {
        // Xử lý lỗi một cách tốt hơn, ví dụ ghi log hoặc thông báo thân thiện cho người dùng
        error_log("Lỗi truy vấn UPDATE: " . $e->getMessage());
        return false;
    }
}
