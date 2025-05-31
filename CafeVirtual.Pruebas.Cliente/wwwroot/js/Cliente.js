//#region Selectors

const selectors = {
    idCliente: '#IdCliente',
    nombre: '#txtNombre',
    descripcion: '#txtDescripcion',
    rfc: '#txtRFC',    
    email: '#txtEmail',    
    activo: '#chkBoxActivo',
    //fecha: '#txtFechaCreacion',
    table: '#resultsTable'
};

//#endregion

//#region Producto Manager

const ClienteManager = {
    init: function () {
        const btnAgregar = document.querySelector('#btnGuardar');
        const btnActualizar = document.querySelector('#btnActualizar');
        const btnEliminar = document.querySelector('#confirmBtn');

        if (btnAgregar) btnAgregar.addEventListener('click', this.agregarCliente);
        if (btnActualizar) btnActualizar.addEventListener('click', this.actualizarCliente);
        if (btnEliminar) btnEliminar.addEventListener('click', this.eliminarCliente);
    },

    agregarCliente: function () {
        if (!validarFormulario()) return;

        const data = obtenerDatosFormulario();

        ajaxRequest('/Cliente/AgregarCliente', data);
    },

    actualizarCliente: function () {
        if (!validarFormulario()) return;

        const idCliente = $(selectors.idCliente).val();
        const data = {
            ...obtenerDatosFormulario(),
            idCliente: idCliente
        };

        ajaxRequest('/Cliente/EditarCliente', data, '/Cliente/Index');
    },

    eliminarCliente: function () {
        const idCliente = $(selectors.idCliente).val();

        const data = { idCliente };

        ajaxRequest('/Cliente/EliminarClienteConfirm', data, '/Cliente/Index');
    }
};

//#endregion

//#region Funciones

const obtenerDatosFormulario = () => {
    return {        
        nombre: $(selectors.nombre).val(),
        descripcion: $(selectors.descripcion).val(),
        rfc: $(selectors.rfc).val(),
        email: $(selectors.email).val(),
        activo: document.querySelector(selectors.activo).checked
        /*fechaCreacion: $(selectors.fecha).val()*/
    };
};

//#endregion

//#region Mostrar Modal Confirmacion

function mostrarModalConfirmacionCliente() {
    $('#modalNombreCliente').text($(selectors.nombre).val());
    $('#modalDescripcionCliente').text($(selectors.descripcion).val());
    $('#modalRfcCliente').text($(selectors.rfc).val());
    $('#modalEmailCliente').text($(selectors.email).val());

    $('#modalConfirmacion').modal('show');
}

//#endregion

//#region Validaciones

const validarFormulario = () => {
    const campos = [
        { id: '#txtNombre', mensaje: 'Nombre es requerido' },
        { id: '#txtDescripcion', mensaje: 'Descripción es requerido' },
        { id: '#txtRFC', mensaje: 'RFC es requerido' },
        { id: '#txtEmail', mensaje: 'Email es requerido', validar: validarEmail, mensajeInvalido: 'Ingresa un email con un formato correcto' }
        //{ id: '#txtFechaCreacion', mensaje: 'Fecha creación es requerida', validar: validarFecha, mensajeInvalido: 'Ingresa una fecha con el formato dd/mm/yyyy' }
    ];

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
};

//#endregion

//#region init

function inicializarPagina() {
    // Agregar "Ver mas" - "Ver menos"
    agregarPopOverVerMas(".popover-form", 3);  // Selector + número de palabras

    // Inicializar todos los popovers con clase personalizada
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.forEach((popoverTriggerEl) => {
        new bootstrap.Popover(popoverTriggerEl, {
            customClass: 'popover-custom'  // Aplicar clase a popOver dinamico
        });
    });
}

// Inicializa Cliente Manager

document.addEventListener('DOMContentLoaded', function () {
    ClienteManager.init();
    inicializarPagina();
});

//#endregion
