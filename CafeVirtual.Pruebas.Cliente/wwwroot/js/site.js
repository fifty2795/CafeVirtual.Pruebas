//#region selectorsModal

const selectorsModal = {
    idUsuario: '#IdUsuarioModal',
    nombre: '#txtNombreModal',
    apellidoPaterno: '#txtApellidoPaternoModal',
    apellidoMaterno: '#txtApellidoMaternoModal',
    email: '#txtEmailModal',
    password: '#txtPasswordModal',
    confirmPassword: '#txtPasswordConfirmModal',
    imagenPerfil: '#imagenPerfilModal'
};

//#endregion

document.addEventListener("DOMContentLoaded", () => {
    const body = document.body;
    const toggleMode = document.getElementById("toggleMode");
    const toggleModeIcon = document.getElementById("toggleModeIcon");
    const toggleSidebar = document.getElementById("toggleSidebar");
    const sidebar = document.querySelector(".sidebar");
    const switchTemaOscuro = document.getElementById("switchTemaOscuro");

    // Lee el tema actual desde el atributo del body
    const currentTheme = body.dataset.theme;

    // Ajusta el ícono y el switch según el tema
    if (currentTheme === "dark") {
        if (toggleModeIcon) toggleModeIcon.className = "fas fa-sun";
        if (switchTemaOscuro) switchTemaOscuro.checked = true;
    } else {
        if (switchTemaOscuro) switchTemaOscuro.checked = false;
    }

    // Botón del header
    toggleMode?.addEventListener("click", () => {
        const isDark = body.classList.toggle("dark-mode");
        localStorage.setItem("theme", isDark ? "dark" : "light");
        document.cookie = `theme=${isDark ? "dark" : "light"}; path=/`;

        if (toggleModeIcon) toggleModeIcon.className = isDark ? "fas fa-sun" : "fas fa-moon";
        if (switchTemaOscuro) switchTemaOscuro.checked = isDark;
    });

    // Switch del modal
    switchTemaOscuro?.addEventListener("change", function () {
        const isDark = this.checked;
        body.classList.toggle("dark-mode", isDark);
        localStorage.setItem("theme", isDark ? "dark" : "light");
        document.cookie = `theme=${isDark ? "dark" : "light"}; path=/`;

        if (toggleModeIcon) toggleModeIcon.className = isDark ? "fas fa-sun" : "fas fa-moon";
    });

    // Colapsar sidebar y guardar estado
    toggleSidebar?.addEventListener("click", () => {
        sidebar.classList.toggle("collapsed");
        localStorage.setItem("sidebar-collapsed", sidebar.classList.contains("collapsed"));
    });
    
    // Recuperar el estado colapsado de barra lateral desde localStorage
    //const isCollapsed = localStorage.getItem("sidebar-collapsed") === "true";
    //if (isCollapsed) {
    //    sidebar.classList.add("collapsed");
    //}
});

$(document).ready(function () {
    const $sidebar = $('.sidebar');

    // Alternancia de submenús
    $('.menu-toggle').on('click', function () {
        const $group = $(this).closest('.menu-group');
        if ($group.hasClass('active')) {
            $group.removeClass('active');
        } else {
            $('.menu-group').removeClass('active');
            $group.addClass('active');
        }
    });

    // Popover dinámico al pasar el mouse
    $('.menu-toggle').each(function () {
        const $btn = $(this);

        // Mostrar popover al entrar el mouse en el ícono del menú
        $btn.on('mouseenter', function () {
            if ($sidebar.hasClass('collapsed')) {
                const isDark = $('body').hasClass('dark-mode');
                const $group = $btn.closest('.menu-group');
                const title = $btn.text().trim().split('\n')[0];

                const submenuItems = $group.find('.submenu a').map(function () {
                    const href = $(this).attr('href');
                    const text = $(this).text().trim();
                    return `<a href="${href}" class="popover-item d-block text-decoration-none ${isDark ? 'dark-text' : ''}">${text}</a><br>`;
                }).get().join("");

                $btn.popover('dispose');

                $btn.popover({
                    html: true,
                    trigger: 'manual',
                    placement: 'right',
                    container: 'body',
                    title: `<strong>${title}</strong>`,
                    content: submenuItems
                });

                $('.menu-toggle').not(this).popover('hide'); // Ocultar otros popovers
                $btn.popover('show'); // Mostrar el popover del botón actual
            }
        });

        // Mantener el popover visible mientras el mouse está sobre el popover
        $btn.on('mouseleave', function () {
            setTimeout(() => {
                if (!$('.popover:hover').length) {
                    $btn.popover('hide');
                }
            }, 200);
        });
    });

    // Ocultar popover al salir del área del popover
    $(document).on('mouseleave', '.popover', function () {
        $('.menu-toggle').popover('hide');
    });

    // Mantener visible si entra el mouse al popover directamente
    $(document).on('mouseenter', '.popover', function () {
        const $popoverBtn = $($('.popover').prev());
        $popoverBtn.popover('show');
    });
});

//#region mostrar Modal

function MostrarModalEditarPerfil() {

    $.ajax({
        url: '/Usuario/EditarUsuarioParcial',
        method: 'GET',
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (html) {
            $('#loading').hide();
            $('#modalContainer').html(html);
            $('#modalEditarPerfil').modal('show');
        },
        error: function (xhr, status, error) {
            $('#loading').hide();
            console.error("Error al cargar el modal:", error);
        }
    });
}

function showModalConfigurarPerfil() {
    $('#modalConfigurarPerfil').modal('show');
}

//#endregion

//#region Funciones Usuario

//#region Funciones

function actualizarUsuarioPartialView() {
    if (!validarModal()) return;

    if (!validarConfirmPassword(selectorsModal.password, selectorsModal.confirmPassword)) return;

    const data = obtenerDatosModal();

    $.ajax({
        url: '/Usuario/GuardarUsuarioParcial',
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

                $('#modalEditarPerfil').modal('hide');

                if (response.data) {
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

function obtenerDatosModal() {
    const formData = new FormData();

    formData.append("IdUsuario", $(selectorsModal.idUsuario).val());
    formData.append("Nombre", $(selectorsModal.nombre).val());
    formData.append("ApellidoPaterno", $(selectorsModal.apellidoPaterno).val());
    formData.append("ApellidoMaterno", $(selectorsModal.apellidoMaterno).val());
    formData.append("Email", $(selectorsModal.email).val());
    formData.append("Password", $(selectorsModal.password).val());

    const file = document.querySelector(selectorsModal.imagenPerfil).files[0];
    if (file) {
        formData.append("imagenPerfil", file);
    }

    return formData;
}

//#endregion

//#region Validaciones

function validarModal() {
    let campos = [
        { id: selectorsModal.nombre, mensaje: 'Nombre es requerido' },
        { id: selectorsModal.apellidoPaterno, mensaje: 'Apellido paterno es requerido' },
        { id: selectorsModal.apellidoMaterno, mensaje: 'Apellido materno es requerido' },
        { id: selectorsModal.email, mensaje: 'Email es requerido', validar: validarEmail, mensajeInvalido: 'Ingresa un email con el formato correcto' },
        { id: selectorsModal.password, mensaje: 'Password es requerido' },
        { id: selectorsModal.confirmPassword, mensaje: 'Confirmar password es requerido' }
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
}

//#endregion

//#endregion

//#region Restablecer focus modales

$(document).on('hide.bs.modal', '#modalEditarPerfil', function () {
    const returnFocusTarget = document.getElementById('btnMostrarModalEditarPerfil');
    if (returnFocusTarget) {
        returnFocusTarget.focus();
    }

    // Esto previene errores con aria-hidden si algún botón dentro del modal sigue con foco
    document.activeElement?.blur?.();
});

$(document).on('hide.bs.modal', '#modalConfigurarPerfil', function () {
    const returnFocusTarget = document.getElementById('btnMostrarModalConfiguracion');
    if (returnFocusTarget) {
        returnFocusTarget.focus();
    }

    // Esto previene errores con aria-hidden si algún botón dentro del modal sigue con foco
    document.activeElement?.blur?.();
});

//#endregion

// Maneja el evento del botón de la barra lateral
//document.getElementById("user-list-toggle").addEventListener("click", function (e) {
//    e.preventDefault();
//    toggleUserList();
//});

