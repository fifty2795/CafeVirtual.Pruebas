//#region Document ready - Validar controles

$(document).ready(function () {
    $('input, textarea, select').on('blur', function () {
        const control = this;
        const tipoValidacion = $(control).data('validate');

        switch (tipoValidacion) {
            case 'fecha':
                validarControl(control, validarFecha);
                break;
            case 'email':
                validarControl(control, validarEmail);
                break;
            case 'select':
                validarControl(control);
                break;
            default:
                validarControl(control); // Solo verifica que no esté vacío
                break;
        }
    });

    $('input[data-validate="fecha"]').on('change', function () {
        validarControl(this, validarFecha);
    });
});

//#endregion

//#region Funcion de Validacion

function validarControl(control, validadorPersonalizado = null) {

    const valor = $(control).val().trim();

    $(control).removeClass('is-valid is-invalid');

    // Validador personalizado
    if (validadorPersonalizado && !validadorPersonalizado(valor)) {
        $(control).addClass('is-invalid');
        return false;
    }    
    
    if ($(control).is(':checkbox')) {
        $(control).removeClass('is-valid is-invalid');
        return true;
    }

    if ($(control).is('input, textarea')) {
        if (valor === '') {
            $(control).addClass('is-invalid');
            return false;
        } else {
            $(control).addClass('is-valid');
            return true;
        }
    }

    if ($(control).is('select')) {
        if (valor === '0' || valor === '' || valor === null) {
            $(control).addClass('is-invalid');
            return false;
        } else {
            $(control).addClass('is-valid');
            return true;
        }
    }    

    return true;
}

//#endregion

//#region Validar Enteros y Decimales

function ValidarNumerosEnteros(event) {

    const tecla = event.key;

    // Permitir teclas especiales como backspace, tab, flechas, etc.
    if (event.ctrlKey || event.metaKey || tecla === "Backspace" || tecla === "Tab" || tecla === "ArrowLeft" || tecla === "ArrowRight" || tecla === "Delete") {
        return true;
    }

    // Solo permitir números del 0 al 9
    if (!/^[0-9]$/.test(tecla)) {
        event.preventDefault();
        return false;
    }
    return true;
}

function ValidarNumerosDecimales(event, input) {

    var valorInput = input.value;
    var teclaIngresada = String.fromCharCode(event.keyCode);

    // Expresión regular para validar números con hasta 2 decimales
    var regex = /^[0-9]*\.?[0-9]{0,2}$/;

    // Validar si la tecla ingresada es un número o un punto
    if (teclaIngresada.match(/[0-9\.]/)) {
        // Validar si el formato del input con la nueva tecla ingresada sigue siendo válido
        if (regex.test(valorInput + teclaIngresada)) {
            return true; // Permitir la entrada del carácter
        } else {
            event.preventDefault(); // Cancelar la entrada del carácter si no es válida según la expresión regular
            return false;
        }
    } else {
        event.preventDefault(); // Cancelar la entrada del carácter si no es un número ni un punto
        return false;
    }
}

//#endregion

//#region Validar Email

function validarEmail(valor) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(valor);
}

//#endregion

//#region Validar Fechas

function validarFecha(fecha) {

    // Expresión regular para verificar el formato dd/mm/yyyy
    let regex = /^(\d{2})\/(\d{2})\/(\d{4})$/;

    // Verificar si cumple con el formato
    let match = fecha.match(regex);

    if (!match) {
        return false;
    }

    // Extraer día, mes y año
    let dia = parseInt(match[1], 10);
    let mes = parseInt(match[2], 10) - 1; // Restamos 1 porque los meses van de 0 a 11 en JS
    let anio = parseInt(match[3], 10);

    let fechaObj = new Date(anio, mes, dia);

    // Retorna true o false si la fecha es valida
    return (fechaObj.getFullYear() === anio && fechaObj.getMonth() === mes && fechaObj.getDate() === dia);
}

function validarRangoFechasYFormatear(idInicio, idFinal) {
    const $fechaInicio = $(idInicio);
    const $fechaFinal = $(idFinal);
    const fechaInicio = $fechaInicio.val();
    const fechaFin = $fechaFinal.val();

    if (!fechaInicio && !fechaFin) {
        return true;
    }

    // Validar que ambas fechas se ingresen
    if (!fechaInicio) {
        mostrarAlertaAlertify(idInicio, 'Debe completar fecha inicio', 'warning');
        return false;
    } else if (!fechaFin) {
        mostrarAlertaAlertify(idFinal, 'Debe completar fecha fin', 'warning');
        return false;
    }

    // Validar formato de ambas fechas - Formato: 'dd/mm/yyyy'
    if (!validarFecha(fechaInicio)) {
        mostrarAlertaAlertify(idInicio, 'Ingresa una fecha inicio con el formato dd/mm/yyyy', 'warning');
        return false;
    } else if (!validarFecha(fechaFin)) {
        mostrarAlertaAlertify(idFinal, 'Ingresa una fecha fin con el formato dd/mm/yyyy', 'warning');
        return false;
    }

    const [diaI, mesI, anioI] = fechaInicio.split('/');
    const [diaF, mesF, anioF] = fechaFin.split('/');

    const inicio = new Date(`${anioI}-${mesI}-${diaI}`);
    const fin = new Date(`${anioF}-${mesF}-${diaF}`);

    if (inicio > fin) {
        mostrarAlertaAlertify(idInicio, 'La fecha de inicio no puede ser mayor que la fecha final', 'warning');
        return false;
    }

    // Convertir temporalmente antes del envío
    const originalInicio = fechaInicio;
    const originalFinal = fechaFin;

    $fechaInicio.val(`${anioI}-${mesI.padStart(2, '0')}-${diaI.padStart(2, '0')}`);
    $fechaFinal.val(`${anioF}-${mesF.padStart(2, '0')}-${diaF.padStart(2, '0')}`);

    setTimeout(() => {
        $fechaInicio.val(originalInicio);
        $fechaFinal.val(originalFinal);
    }, 100);

    return true;
}

//#endregion

//#region Validar Password

function validarConfirmPassword(selectorPassword, selectorConfirmPassword) {

    const controlPassword = selectorPassword ? $(selectorPassword) : null;
    const controlConfirmPassword = selectorConfirmPassword ? $(selectorConfirmPassword) : null;    

    const password = controlPassword.val();
    const confirmPassword = controlConfirmPassword.val();

    if (password.trim() === confirmPassword.trim()) {
        controlPassword.removeClass('is-valid is-invalid').addClass('is-valid');
        controlConfirmPassword.removeClass('is-valid is-invalid').addClass('is-valid');
        return true;
    }

    controlPassword.removeClass('is-valid is-invalid').addClass('is-invalid');
    controlConfirmPassword.removeClass('is-valid is-invalid').addClass('is-invalid');
    mostrarAlertaAlertify(null, 'Los passwords no coinciden', 'warning');

    return false;
}

//#endregion