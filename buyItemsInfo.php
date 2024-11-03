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

$itemId = $data->itemId ?? 0;
$date = $data->date ?? '';
$playerId = $data->playerId ?? 0;


// Preparar y ejecutar la consulta
$stmt = $conn->prepare("INSERT INTO BuyItemsInfo (itemId, date, playerId) VALUES (?, ?, ?)");
$stmt->bind_param("isi", $itemId, $date,$playerId);

if ($stmt->execute()) {
    echo json_encode(["message" => "Datos guardados correctamente"]);
} else {
    http_response_code(500);
    echo json_encode(["message" => "Error al guardar datos"]);
}

$stmt->close();
$conn->close();
?>