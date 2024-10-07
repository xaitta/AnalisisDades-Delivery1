<?php
$servername = "localHost";
$username = "danielmc11";
$password = "45933702b";
$dbname = "danielmc11";

$conn = new mysqli($servername, $username, $password,$dbname);

if($conn->connect_error){
    die("Conenction failed: " . $conn->connect_error);

}
echo "Connected successfully";

$sql = "INSERT INTO MyGuests (firstname, lastname, email) VALUES ('John', 'Doe', 'john@example.com')";

if($conn->query($sql) === TRUE)
{
    $last_id = $conn->insert_id;
    echo "New record created successfully. Last inserted ID is: " . $last_id;
}
else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}
?>