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

// Leer el contenido del cuerpo de la solicitud
$json = file_get_contents('php://input');

// Decodificar el JSON
$data = json_decode($json);

// Comprobar si la decodificación fue exitosa
if (json_last_error() !== JSON_ERROR_NONE) {
    http_response_code(400);
    echo json_encode(["message" => "Invalid JSON"]);
    exit();
}

// Acceder a los datos enviados
$name = $data->name ?? '';
$country = $data->country ?? '';
$age = $data->age ?? 0;
$gender = $data->gender ?? 0.0;
$date = $data->date ?? '';

// Preparar y ejecutar la consulta
$stmt = $conn->prepare("INSERT INTO MyGuests (name, country, age, gender, date) VALUES (?, ?, ?, ?, ?)");
$stmt->bind_param("ssids", $name, $country, $age, $gender, $date);

if ($stmt->execute()) {
    echo json_encode(["message" => "Datos guardados correctamente"]);
} else {
    http_response_code(500);
    echo json_encode(["message" => "Error al guardar datos"]);
}

$stmt->close();
$conn->close();
?>
