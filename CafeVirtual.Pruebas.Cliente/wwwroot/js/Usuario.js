//#region selectors

const selectors = {
    idUsuario: '#IdUsuario',
    ddlRol: '#ddlRol',
    rol: '#txtRol',
    nombre: '#txtNombre',
    apellidoPaterno: '#txtApellidoPaterno',
    apellidoMaterno: '#txtApellidoMaterno',
    email: '#txtEmail',
    password: '#txtPassword',
    confirmPassword: '#txtPasswordConfirm',
    imagenPerfil: '#imagenPerfil',
    activo: '#chkBoxActivo',
    /*fecha: '#txtFechaCreacion',*/
    table: '#resultsTable'
};

//#endregion

//#region Usuario Manager

const UsuarioManager = {
    init: function () {
        const btnAgregar = document.querySelector('#btnGuardar');
        const btnActualizar = document.querySelector('#btnActualizar');
        const btnEliminar = document.querySelector('#confirmBtn');

        if (btnAgregar) btnAgregar.addEventListener('click', this.agregarUsuario);
        if (btnActualizar) btnActualizar.addEventListener('click', this.actualizarUsuario);
        if (btnEliminar) btnEliminar.addEventListener('click', this.eliminarUsuario);
    },

    agregarUsuario: function () {
        if (!validarFormulario('Insert')) return;

        if (!validarConfirmPassword(selectors.password, selectors.confirmPassword)) return;

        const data = obtenerDatosFormulario('Insert');

        ajaxRequest('/Usuario/AgregarUsuario', data);
    },

    actualizarUsuario: function () {
        if (!validarFormulario('Update')) return;

        const data = obtenerDatosFormularioUsuarioGeneral('Update');        

        $.ajax({
            url: '/Usuario/EditarUsuario',
            method: 'POST',
            data: data,
            processData: false,
            contentType: false,
            beforeSend: function () {
                $('#loading').show();
            },
            success: function (response) {
                setTimeout(() => {
                    $('#loading').hide();
                }, 2000);

                if (response.success) {

                    const idUsuarioClaim = document.querySelector('.my-form-container')?.dataset.idusuario;

                    if (parseInt(response.data.idUsuario) === parseInt(idUsuarioClaim)) {
                        const ruta = response.data.rutaImagen;
                        const nuevaRuta = ruta && ruta.trim()
                            ? '/' + ruta.replace(/^\/+/, '') // Empezar con "/"
                            : '/images/avatar-default.png'; // Ruta por defecto

                        const nombrePerfil = response.data.nombre + ' ' + response.data.apellidoPaterno;

                        $('img.user-profile-img').attr('src', nuevaRuta);
                        $('img.user-profile-menu-configuracion-img').attr('src', nuevaRuta);
                        $('img.user-profile-barra-lateral-img').attr('src', nuevaRuta);
                        $('.rounded-circle').attr('src', nuevaRuta);
                        $('.userName-perfil').text(nombrePerfil);
                    }

                    mostrarAlertaAlertify(null, response.message, 'success');

                    setTimeout(() => {
                        window.location.href = '/Usuario/Index';
                    }, 2000);

                } else {
                    mostrarAlertaAlertify(null, response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                setTimeout(() => {
                    $('#loading').hide();
                }, 2000);
                console.error("Error AJAX en AjaxRequest:", xhr.responseText + ' ' + error);
            }
        });
    },

    eliminarUsuario: function () {

        const idUsuario = $(selectors.idUsuario).val();

        const data = { idUsuario };

        ajaxRequest('/Usuario/EliminarUsuarioConfirm', data, '/Usuario/Index');
    }
};

//#endregion

//#region Funciones

function obtenerDatosFormulario(accion) {
    const formData = new FormData();

    if (accion === 'Update') {
        formData.append("IdUsuario", $(selectors.idUsuario).val());
    }

    formData.append("IdRol", $(selectors.ddlRol).val());
    formData.append("Activo", document.querySelector(selectors.activo).checked);
    //formData.append("FechaCreacion", $(selectors.fecha).val());
    formData.append("Nombre", $(selectors.nombre).val());
    formData.append("ApellidoPaterno", $(selectors.apellidoPaterno).val());
    formData.append("ApellidoMaterno", $(selectors.apellidoMaterno).val());
    formData.append("Email", $(selectors.email).val());
    formData.append("Password", $(selectors.password).val());

    const file = document.querySelector(selectors.imagenPerfil).files[0];
    if (file) {
        formData.append("imagenPerfil", file);
    }

    return formData;
}

function obtenerDatosFormularioUsuarioGeneral(accion) {
    const formData = new FormData();

    if (accion === 'Update') {
        formData.append("IdUsuario", $(selectors.idUsuario).val());
    }

    formData.append("IdRol", $(selectors.ddlRol).val());
    formData.append("Activo", document.querySelector(selectors.activo).checked);
    //formData.append("FechaCreacion", $(selectors.fecha).val());
    formData.append("Nombre", $(selectors.nombre).val());
    formData.append("ApellidoPaterno", $(selectors.apellidoPaterno).val());
    formData.append("ApellidoMaterno", $(selectors.apellidoMaterno).val());
    formData.append("Email", $(selectors.email).val());

    const file = document.querySelector(selectors.imagenPerfil).files[0];
    if (file) {
        formData.append("imagenPerfil", file);
    }

    return formData;
}


//#endregion

//#region Mostrar Modal Confirmacion

function mostrarModalConfirmacionUsuario() {
    $('#modalNombreUsuario').text($(selectors.nombre).val());
    $('#modalRolUsuario').text($(selectors.rol).val());
    $('#modalEmailUsuario').text($(selectors.email).val());

    $('#modalConfirmacion').modal('show');
}

//#endregion

//#region Validaciones

function validarFormulario(accion) {
    let camposBase = [
        { id: '#txtNombre', mensaje: 'Nombre es requerido' },
        { id: '#txtApellidoPaterno', mensaje: 'Apellido paterno es requerido' },
        { id: '#txtApellidoMaterno', mensaje: 'Apellido materno es requerido' },
        { id: '#txtEmail', mensaje: 'Email es requerido', validar: validarEmail, mensajeInvalido: 'Ingresa un email con el formato correcto' }
    ];

    let campos = [];

    switch (accion) {
        case 'Insert':
            campos = [
                ...camposBase,
                { id: '#ddlRol', mensaje: 'Rol es requerido' },
                { id: '#txtPassword', mensaje: 'Password es requerido' },
                { id: '#txtPasswordConfirm', mensaje: 'Confirmar password es requerido' }
                //{ id: '#txtFechaCreacion', mensaje: 'Fecha creación es requerida', validar: validarFecha, mensajeInvalido: 'Ingresa una fecha con el formato dd/mm/yyyy' }
            ];
            break;

        case 'Update':
            campos = [
                ...camposBase,
                { id: '#ddlRol', mensaje: 'Rol es requerido' }
                //{ id: '#txtFechaCreacion', mensaje: 'Fecha creación es requerida', validar: validarFecha, mensajeInvalido: 'Ingresa una fecha con el formato dd/mm/yyyy' }
            ];
            break;
    }

    for (const campo of campos) {
        const $input = $(campo.id);
        const valor = $input.val().trim();

        if (valor === '') {
            mostrarAlertaAlertify(campo.id, campo.mensaje, 'warning')
            return false;
        }

        if (campo.validar && !campo.validar(valor)) {
            mostrarAlertaAlertify(campo.id, campo.mensajeInvalido, 'warning')
            return false;
        }
    }
    return true;
}

//#endregion

//#region init

function inicializarPagina() {

}

// Inicializa Cliente Manager

document.addEventListener('DOMContentLoaded', function () {
    UsuarioManager.init();
    //inicializarPagina();
});

//#endregion