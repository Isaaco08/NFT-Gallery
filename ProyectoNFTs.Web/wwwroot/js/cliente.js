function getClienteById(idCliente, IdDiv) {
    const myRequest = "/cliente/GetClienteById?id=" + idCliente;
    fetch(myRequest)
        .then((response) => response.json())
        .then((data) => {
            const div = document.getElementById(IdDiv);
            console.log(data);
            //Beware! Properties in CamelCase!
            div.textContent = data.nombre + " " + data.apellido1 + " " + data.apellido2;
        })
        .catch((error) => {
            console.error('Error al obtener el cliente:', error);
        });
}