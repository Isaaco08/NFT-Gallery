function getRolById(idRol, IdDiv) {
    const myRequest = "/Rol/GetRolById?id=" + idRol;
    fetch(myRequest)
        .then((response) => response.json())
        .then((data) => {
            const div = document.getElementById(IdDiv);
            console.log(data);
            //Beware! Properties in CamelCase!
            div.textContent = data.idRol + " - " + data.descripcionRol;
        })
        .catch((error) => {
            console.error('Error al obtener el país:', error);
        });
}