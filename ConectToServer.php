<?php
// Configuración de conexión a la base de datos
$servername = "localhost"; // Nota: "localhost" debe estar en minúsculas
$username = "danielmc11";
$password = "45933702b";
$dbname = "danielmc11";
    
// Crear la conexión
$conn = new mysqli($servername, $username, $password, $dbname);

// Verificar la conexión
if ($conn->connect_error) {
    http_response_code(500);
    echo json_encode(["message" => "Error de conexión a la base de datos"]);
    exit();
}
?>