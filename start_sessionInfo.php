<?php
include "ConectToServer.php";

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

$playerId = $data->player_id ?? 0;
$date = $data->start_date ?? '';

// Preparar y ejecutar la consulta
$stmt = $conn->prepare("INSERT INTO StartSessionInfo (player_id, start_date) VALUES (?, ?)");
$stmt->bind_param("ssids", $playerId, $date);

if ($stmt->execute()) {
    echo json_encode(["message" => "Datos guardados correctamente"]);
} else {
    http_response_code(500);
    echo json_encode(["message" => "Error al guardar datos"]);
}

$stmt->close();
$conn->close();
?>
