//#region Selectors

const selectors = {
    idRol: '#IdRol',
    selectRolMasivo: '#selectNuevoRolMasivo',
    selectRolModal: '#selectNuevoRol',
    nombre: '#txtNombre',
    descripcion: '#txtDescripcion',
    activo: '#chkBoxActivo',
    //fecha: '#txtFechaCreacion',
    table: '#resultsTable'
};

//#endregion

//#region Rol Manager

const RolManager = {
    init: function () {
        const btnAgregar = document.querySelector('#btnGuardar');
        const btnActualizar = document.querySelector('#btnActualizar');
        const btnEliminar = document.querySelector('#confirmBtn');

        if (btnAgregar) btnAgregar.addEventListener('click', this.agregarRol);
        if (btnActualizar) btnActualizar.addEventListener('click', this.actualizarRol);
        if (btnEliminar) btnEliminar.addEventListener('click', this.eliminarRol);
    },

    agregarRol: function () {
        if (!validarFormulario()) return;

        const data = obtenerDatosFormulario();       

        ajaxRequest('/Rol/AgregarRol', data);
    },

    actualizarRol: function () {
        if (!validarFormulario()) return;

        const idRol = $(selectors.idRol).val();
        const data = {
            ...obtenerDatosFormulario(),
            idRol: idRol
        };

        ajaxRequest('/Rol/EditarRol', data, '/Rol/Index');
    },

    eliminarRol: function () {
        const idRol = $(selectors.idRol).val();

        validarExisteUsuarioByRol(idRol);
    }
};

//#endregion

//#region Funciones

const obtenerDatosFormulario = () => {
    return {
        nombre: $(selectors.nombre).val(),
        descripcion: $(selectors.descripcion).val(),
        activo: document.querySelector(selectors.activo).checked,
        //fechaCreacion: $(selectors.fecha).val(),
        menusSeleccionados: obtenerMenusSeleccionados()
    };
};

function cargarUsuariosEnModal(response) {
    const idRol = $(selectors.idRol).val();
    const nombreRol = $(selectors.nombre).val();
    const tablaUsuarios = document.querySelector('#modalUsuariosBody');    
    const paginacionContainer = document.querySelector('#modalPaginacionContainer');

    tablaUsuarios.innerHTML = '';

    const paginatedData = response.data;    
    const listaUsuarios = paginatedData.items;

    $('#modalNombreRolUpdate').text(nombreRol);

    if (listaUsuarios.length === 0) {
        const fila = document.createElement('tr');
        fila.innerHTML = '<td colspan="4" class="text-center">No hay usuarios asociados a este rol.</td>';
        tablaUsuarios.appendChild(fila);

        $('#modalConfirmarEliminacion').modal('hide');
    } else {
        listaUsuarios.forEach(usuario => {
            const fila = document.createElement('tr');
            fila.id = `usuario-fila-${usuario.idUsuario}`;
            fila.setAttribute('data-id-rol', usuario.idRol);
            fila.innerHTML = `
                <td>${usuario.nombreCompleto}</td>
                <td>${usuario.email}</td>
                <td>${usuario.rolNombre}</td>    
                <td class="text-center">
                     <button type="button"
                        class="update-btn"
                        onclick="abrirModalEditarRol(${usuario.idUsuario}, '${usuario.nombreCompleto}')"
                        <span><i class="fa fa-pencil" aria-hidden="true"></i></span>
                        Cambiar rol
                    </button>                    
                </td>
            `;
            tablaUsuarios.appendChild(fila);
        });        
        
        // Agregar paginación dinámica
        const totalPages = paginatedData.totalPages;
        const currentPage = paginatedData.pageIndex;

        let paginacionHTML = `<nav aria-label="Paginación modal"><ul class="pagination">`;

        paginacionHTML += `
        <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
            <button class="page-link" onclick="validarExisteUsuarioByRol(${idRol}, ${currentPage - 1})">Anterior</button>
        </li>`;

        for (let i = 1; i <= totalPages; i++) {
            paginacionHTML += `
            <li class="page-item ${i === currentPage ? 'active' : ''}">
                <button class="page-link" onclick="validarExisteUsuarioByRol(${idRol}, ${i})">${i}</button>
            </li>`;
        }

        paginacionHTML += `
        <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
            <button class="page-link" onclick="validarExisteUsuarioByRol(${idRol}, ${currentPage + 1})">Siguiente</button>
        </li>
    </ul></nav>`;

        paginacionContainer.innerHTML = paginacionHTML;      

        // Solo mostrar si no está ya visible
        if (!$('#modalConfirmarEliminacion').hasClass('show')) {
            $('#modalConfirmarEliminacion').modal('show');
        }
    }
}

function confirmarCambioRol() {    
    const idUsuario = $('#modalIdUsuario').val();    
    const idRolSeleccionado = $('#selectNuevoRol').val();    
    const filaUsuario = document.querySelector(`#usuario-fila-${idUsuario}`);
    const paginaActual = parseInt($('#modalPaginacionContainer .pagination .page-item.active .page-link').text());

    if (!validarRolModalActualizacion(true)) return;

    if (filaUsuario) {
        filaUsuario.setAttribute('data-id-rol', idRolSeleccionado);        

        const data = {
            idUsuario: idUsuario,
            idRol: idRolSeleccionado,
            pageNumber: paginaActual
        };

        $.ajax({
            url: '/Rol/ActualizarUsuarioByRol',
            method: 'POST',
            data: data,            
            beforeSend: function () {
                $('#loading').show();
            },
            success: function (response) {
                setTimeout(() => {
                    $('#loading').hide();
                }, 2000);

                if (response.success) {
                    mostrarAlertaAlertify(null, response.message, 'success');

                    const idRol = $(selectors.idRol).val();

                    validarExisteUsuarioByRol(idRol);
                    
                } else {
                    mostrarAlertaAlertify(null, response.message, 'error');
                }                

                $('#modalEditarRolUsuario').modal('hide');    
            },
            error: function (xhr, status, error) {
                setTimeout(() => {
                    $('#loading').hide();
                }, 2000);
                console.error("Error AJAX en AjaxRequest:", xhr.responseText + ' ' + error);
                $('#modalEditarRolUsuario').modal('hide');    
            }
        });
    }  
}

function actualizarUsuarioByRolMasivo() {
    if (validarRolModalActualizacion(false)) {

        actualizarRolMasivoAsignado();      
    }
}

function actualizarRolMasivoAsignado() {
    const idRol = $(selectors.idRol).val();
    const idRolNuevo = $(selectors.selectRolMasivo).val();

    const data = {
        idRol: idRol,
        idRolNuevo: idRolNuevo
    };    

    $.ajax({
        url: '/Rol/ActualizarUsuarioByRolMasivo',
        method: 'POST',
        data: data,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (response) {
            setTimeout(() => {
                $('#loading').hide();
            }, 2000);

            if (response.success) {

                const idRol = $(selectors.idRol).val();
                const data = {
                    idRol: idRol
                };

                ajaxRequest('/Rol/EliminarRolConfirm', data, '/Rol/Index');

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
}

function obtenerMenusSeleccionados() {
    const menusSeleccionados = [];

    $('.menu-checkbox:checked').each(function () {
        menusSeleccionados.push(parseInt($(this).val()));
    });

    return menusSeleccionados;
}

//#endregion

//#region Mostrar Modal Confirmacion

function mostrarModalConfirmacionRol() {
    $('#modalNombreRol').text($(selectors.nombre).val());
    $('#modalDescripcionRol').text($(selectors.descripcion).val());

    $('#modalConfirmacion').modal('show');
}

function abrirModalEditarRol(idUsuario, nombreUsuario) {
    document.getElementById('modalUsuarioNombre').textContent = nombreUsuario;
    document.getElementById('modalIdUsuario').value = idUsuario;    
    $('#modalEditarRolUsuario').modal('show');    
}

//#endregion

//#region Validaciones

const validarFormulario = () => {
    const campos = [
        { id: '#txtNombre', mensaje: 'Nombre es requerido' },
        { id: '#txtDescripcion', mensaje: 'Descripción es requerido' }
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

    var menusSeleccionados = obtenerMenusSeleccionados();

    if (menusSeleccionados.length === 0) {
        mostrarAlertaAlertify(null, "Debe de seleccionar al menos un permiso para el rol", 'warning')
        return false;
    }

    return true;
};

function validarRolModalActualizacion(masivo) {

    let campos;

    if (masivo) {
        campos = [
            { id: selectors.selectRolModal, mensaje: 'Rol es requerido' }
        ];
    } else {
        campos = [
            { id: selectors.selectRolMasivo, mensaje: 'Rol es requerido' }
        ];
    }    

    for (const campo of campos) {
        const $input = $(campo.id);
        const valor = $input.val().trim();

        if (valor === '') {
            mostrarAlertaAlertify(campo.id, campo.mensaje, 'warning')
            return false;
        }

        //if (campo.validar && !campo.validar(valor)) {
        //    mostrarAlertaAlertify(campo.id, campo.mensajeInvalido, 'warning')
        //    return false;
        //}
    }
    return true;
}

function validarExisteUsuarioByRol(idRol, pageNumber) {
    const data = {
        idRol: idRol,
        pageNumber: pageNumber
    };

    $.ajax({
        url: '/Rol/ValidarExisteUsuarioByRol',
        method: 'POST',
        data: data,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (response) {
            $('#loading').hide();

            if (response.success) {                

                // Carga registros en modal
                cargarUsuariosEnModal(response);

                if (response.data.pageIndex === 1) {
                    mostrarAlertaAlertify(null, response.message, 'warning');
                }
            } else {
                const data = {
                    idRol: idRol                  
                };
                $('#modalConfirmarEliminacion').modal('hide');
                ajaxRequest('/Rol/EliminarRolConfirm', data, '/Rol/Index');
            }
        },
        error: function (xhr, status, error) {
            $('#loading').hide();
            mostrarAlertaAlertify(null, 'Ocurrio un error al consultar los productos asociados.', 'error');
            console.error("Error AJAX en AjaxRequest:", xhr.responseText + ' ' + error);
        }
    });
}

//#endregion

//#region init

function inicializarPagina() {

}

// Inicializa Cliente Manager

document.addEventListener('DOMContentLoaded', function () {
    RolManager.init();
    //inicializarPagina();
});

//#endregion
