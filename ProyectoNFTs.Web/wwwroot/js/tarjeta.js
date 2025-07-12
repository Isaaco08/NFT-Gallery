function getTarjetaById(idTarjeta, IdDiv) {
    const myRequest = "/Tarjeta/GetTarjetaById?id=" + idTarjeta;
    fetch(myRequest)
        .then((response) => response.json())
        .then((data) => {
            const div = document.getElementById(IdDiv);
            console.log(data);
            //Beware! Properties in CamelCase!
            div.textContent = data.idTarjeta + " - " + data.descripcion;
        })
        .catch((error) => {
            console.error('Error al obtener el país:', error);
        });
}