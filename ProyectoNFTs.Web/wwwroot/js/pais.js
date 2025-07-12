function getPaisById(idPais, IdDiv) {
    const myRequest = "/pais/GetPaisById?id=" + idPais;
    fetch(myRequest)
        .then((response) => response.json())
        .then((data) => {
            const div = document.getElementById(IdDiv);
            console.log(data);
            //Beware! Properties in CamelCase!
            div.textContent = data.descripcion;
        })
        .catch((error) => {
            console.error('Error al obtener el país:', error);
        });
}