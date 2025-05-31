//#region Selectors

const selectors = {
    nombre: '#txtNombre',
    email: '#txtEmail',    
    password: '#txtPassword',
    confirmPassword: '#txtConfirmPassword',    
    imagenPerfil: '#imagenPerfil'    
};

//#endregion

//#region Login Manager

const LoginManager = {
    init: function () {
        const btnLogin = document.querySelector('#btnLogin');        
        const btnRegistrarse = document.querySelector('#btnRegistrarse');        

        if (btnLogin) btnLogin.addEventListener('click', this.loginUsuario);        
        if (btnRegistrarse) btnRegistrarse.addEventListener('click', this.registrarUsuario);        
    },

    loginUsuario: function () {
        if (!validarFormulario('Login')) return;

        const email = $(selectors.email).val();
        const password = $(selectors.password).val();

        const data = {
            email: email,
            password: password
        }

        $.ajax({
            url: '/Login/Login',
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
                    window.location.href = response.redirectUrl;
                } else {
                    mostrarAlertaAlertify(null, response.message, 'warning');
                }
            },
            error: function (xhr, status, error) {
                setTimeout(() => {
                    $('#loading').hide();
                }, 2000);
                console.error("Error AJAX en AjaxRequest:", xhr.responseText + ' ' + error);
                mostrarAlertaAlertify(null, 'Ocurrio un error al iniciar sessión', 'error');
            }
        });
    },
    registrarUsuario: function () {
        if (!validarFormulario('Registro')) return;               

        const data = obtenerDatosFormulario();

        if (!validarConfirmPassword(selectors.password, selectors.confirmPassword)) return;        

        ajaxRequest('/Login/Registro', data, '/Login/Index');     
    }
};

//#endregion

//#region Eventos

function loadEventos() {

    agregarEventoEnter(selectors.password, LoginManager.loginUsuario);    
}

//#endregion

//#region Funciones

const obtenerDatosFormulario = () => {

    const nombreCompleto = $(selectors.nombre).val().trim();
    const partes = nombreCompleto.split(/\s+/);

    let nombre = "";
    let apellidoPaterno = "";
    let apellidoMaterno = "";

    if (partes.length >= 3) {
        apellidoMaterno = partes.pop();
        apellidoPaterno = partes.pop();
        nombre = partes.join(" ");
    } else if (partes.length === 2) {
        nombre = partes[0];
        apellidoPaterno = partes[1];
    } else if (partes.length === 1) {
        nombre = partes[0];
    }

    const formData = new FormData();

    formData.append("Nombre", nombre);
    formData.append("ApellidoPaterno", apellidoPaterno);
    formData.append("ApellidoMaterno", apellidoMaterno);    
    formData.append("Email", $(selectors.email).val());
    formData.append("Password", $(selectors.password).val());    

    const file = document.querySelector(selectors.imagenPerfil).files[0];
    if (file) {
        formData.append("imagenPerfil", file);
    }

    return formData;
};

//#endregion

//#region Validaciones

const validarFormulario = (tipo) => {

    let campos;

    if (tipo === 'Login') {
        campos = [
            { id: '#txtEmail', mensaje: 'Email es requerido', validar: validarEmail, mensajeInvalido: 'Ingresa un email con un formato correcto' },
            { id: '#txtPassword', mensaje: 'Password es requerido' }
        ];
    } else if(tipo === 'Registro') {
        campos = [
            { id: '#txtNombre', mensaje: 'Nombre es requerido' },
            { id: '#txtEmail', mensaje: 'Email es requerido', validar: validarEmail, mensajeInvalido: 'Ingresa un email con un formato correcto' },
            { id: '#txtPassword', mensaje: 'Password es requerido' },
            { id: '#txtConfirmPassword', mensaje: 'Confirmar password es requerido' }
        ];
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
};

//#endregion

//#region init

function inicializarPagina() {

}

// Inicializa Cliente Manager

document.addEventListener('DOMContentLoaded', function () {
    LoginManager.init();
    //inicializarPagina();
    loadEventos();
});

//#endregion