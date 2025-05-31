//#region Llamada Ajax General

const ajaxRequest = (url, data, redirectUrl = null) => {
    const isFormData = data instanceof FormData;
    const contieneLogin = url.includes("Login");    

    $.ajax({
        url: url,
        method: 'POST',
        data: data,
        processData: !isFormData ? true : false,
        contentType: !isFormData ? 'application/x-www-form-urlencoded; charset=UTF-8' : false,
        beforeSend: function () {
            $('#loading').show();
        },
        success: function (response) {
            setTimeout(() => {
                $('#loading').hide();
            }, 2000);

            if (response.success) {
                mostrarAlertaAlertify(null, response.message, 'success');
                clearForm('.form-container');

                if (redirectUrl) {
                    setTimeout(() => {
                        window.location.href = redirectUrl;
                    }, contieneLogin ? 5000 : 2000);
                }
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
};

//#endregion

//#region Modal de Confirmacion

function mostrarModalConfirmacion() {
    $('#modalConfirmacion').modal('show');
}

function ocultarModalConfirmacion() {
    $('#modalConfirmacion').modal('hide');
}

//#endregion

//#region Show Alertity

function mostrarAlertaAlertify(selector = null, mensaje = '', tipoAlerta = 'success') {
    if (!mensaje || typeof mensaje !== 'string') return;

    const control = selector ? $(selector) : null;

    switch (tipoAlerta.toLowerCase()) {
        case 'success':
            alertify.success(mensaje);
            break;

        case 'warning':
            alertify.warning(mensaje);

            if (control && control.length > 0) {
                control.removeClass('is-valid is-invalid').addClass('is-invalid');
                control[0].focus();
                control[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
            break;

        case 'error':
            alertify.error(mensaje);
            break;

        default:
            alertify.message(mensaje);
            break;
    }
}

//#endregion

//#region Formatear valores monetarios

function activarMascaraMoneda() {
    const inputsMoneda = document.querySelectorAll('.input-moneda');

    inputsMoneda.forEach(input => {
        if (!input.hasAttribute('data-mascara-aplicada')) {
            aplicarMascaraMoneda(input);
            input.setAttribute('data-mascara-aplicada', 'true');
        }
    });
}

function aplicarMascaraMoneda(input) {
    input.addEventListener('input', function (event) {
        let valor = event.target.value;

        // Eliminar todo excepto dígitos y punto decimal
        valor = valor.replace(/[^0-9.]/g, '');

        // Evitar múltiples puntos
        const partes = valor.split('.');
        if (partes.length > 2) {
            valor = partes[0] + '.' + partes[1];
        }

        // Separar parte entera y decimal
        let [entero, decimal] = valor.split('.');

        // Eliminar ceros iniciales innecesarios
        if (entero) {
            entero = entero.replace(/^0+(?!$)/, '');
        }

        // Formatear parte entera con separadores de miles
        if (entero) {
            entero = entero.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        }

        // Limitar a 2 decimales
        if (decimal) {
            decimal = decimal.substring(0, 2);
        }

        let resultado = '$' + entero;
        if (decimal !== undefined) {
            resultado += '.' + decimal;
        }

        event.target.value = resultado;
    });
}

function aplicarMascaraMonedaATabla(selectorTabla) {
    const tabla = document.querySelector(selectorTabla);
    if (!tabla) return;
    
    const celdasMoneda = tabla.querySelectorAll('td.moneda');

    celdasMoneda.forEach(celda => {
        const texto = celda.textContent.trim();

        // Elimina comas y signos si ya hay formato
        const valorNumerico = parseFloat(texto.replace(/[,$]/g, ''));

        // Aplica si es un número válido
        if (!isNaN(valorNumerico)) {
            celda.textContent = convertToMoneda(valorNumerico);
        }
    });
}

function convertToMoneda(valor) {
    const numero = parseFloat(valor);
    if (isNaN(numero)) return valor;

    return '$' + numero.toLocaleString('es-MX', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

function convertMonedaAValor(valorConMascara) {
    if (!valorConMascara) return 0.00;

    let valorLimpio = valorConMascara
        .replace(/\$/g, '')   // eliminar signo $
        .replace(/,/g, '')    // eliminar comas
        .trim();

    let valorNumerico = parseFloat(valorLimpio);

    // Validar que sea un número
    if (isNaN(valorNumerico)) {
        return 0.00;
    }

    // Retornar con dos decimales fijos
    return parseFloat(valorNumerico.toFixed(2));
}

//#endregion

//#region Formatear Cantidad

function aplicarMascaraCantidad(selectorInput) {
    const input = document.querySelector(selectorInput);
    if (!input) return;

    input.addEventListener('input', function (e) {
        let valor = e.target.value;

        // Eliminar caracteres no numéricos
        valor = valor.replace(/[^\d]/g, '');

        // Eliminar ceros iniciales innecesarios
        valor = valor.replace(/^0+/, '');

        // Si el valor es vacío o cero, limpiar el campo
        if (valor === '' || parseInt(valor) === 0) {
            e.target.value = '';
            return;
        }

        // Convertir a número y aplicar formato de miles
        const numero = parseInt(valor, 10);
        if (!isNaN(numero)) {
            const numeroFormateado = numero.toLocaleString('es-MX');
            e.target.value = numeroFormateado;
        }
    });
}

function aplicarMascaraCantidadATabla(selectorTabla) {
    const tabla = document.querySelector(selectorTabla);
    if (!tabla) return;

    const celdasCantidad = tabla.querySelectorAll('td.cantidad');

    celdasCantidad.forEach(celda => {
        const texto = celda.textContent.trim();

        // Convertir a entero
        const cantidad = parseInt(texto.replace(/[^\d]/g, ''), 10);

        // Validar que sea un número
        if (!isNaN(cantidad)) {
            celda.textContent = formatearCantidad(cantidad);
        }
    });
}

function formatearCantidad(valor) {
    const valorFormateado = valor.toLocaleString('en-US'); // Agrega comas como separador de miles

    return valor === 1
        ? `${valorFormateado} pza`
        : `${valorFormateado} pzas`;
}

//#endregion

//#region Formatear Cantidad,Monetarios - InputsReadOnly

function aplicarMascaraAInputsReadOnly() {
    const inputPrecio = document.getElementById('txtPrecio');
    const inputCantidad = document.getElementById('txtCantidad');

    if (inputPrecio && inputPrecio.hasAttribute('readonly')) {
        const valorNumerico = parseFloat(inputPrecio.value.replace(/[,$]/g, ''));
        if (!isNaN(valorNumerico)) {
            inputPrecio.value = convertToMoneda(valorNumerico);
        }
    }

    if (inputCantidad && inputCantidad.hasAttribute('readonly')) {
        const valorNumerico = parseInt(inputCantidad.value.replace(/[^\d]/g, ''), 10);
        if (!isNaN(valorNumerico)) {
            inputCantidad.value = valorNumerico.toLocaleString('es-MX');
        }
    }
}

//#endregion

//#region Limpiar Formulario

function clearForm(containerSelector) {
    const $container = $(containerSelector);

    // // Clear Inputs y Textareas
    $container.find('input:not([type="checkbox"]), textarea').each(function () {
        $(this).val('').removeClass('is-valid is-invalid');
    });

    // // Clear Checkboxes
    $container.find('input[type="checkbox"]').each(function () {
        this.checked = false;
        $(this).removeClass('is-valid is-invalid');
    });

    // // Clear Selects
    $container.find('select').each(function () {
        $(this).val('').removeClass('is-valid is-invalid');
        if ($(this).hasClass('select2-hidden-accessible')) {
            $(this).val(null).trigger('change'); // Reset y notificar a Select2
        }
    });
}

//#endregion

//#region Configuración Global Datepicker Español

$(function () {
    // Definir manualmente el idioma español si no está definido
    if (!$.datepicker.regional["es"]) {
        $.datepicker.regional["es"] = {
            closeText: "Cerrar",
            prevText: "&#x3C;Ant",
            nextText: "Sig&#x3E;",
            currentText: "Hoy",
            monthNames: [
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            ],
            monthNamesShort: [
                "ene", "feb", "mar", "abr", "may", "jun",
                "jul", "ago", "sep", "oct", "nov", "dic"
            ],
            dayNames: [
                "domingo", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado"
            ],
            dayNamesShort: [
                "dom", "lun", "mar", "mié", "jue", "vie", "sáb"
            ],
            dayNamesMin: [
                "D", "L", "M", "X", "J", "V", "S"
            ],
            weekHeader: "Sm",
            dateFormat: "dd/mm/yy",
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ""
        };
    }

    // Aplicar configuración por defecto en español
    $.datepicker.setDefaults($.datepicker.regional["es"]);

});

//#endregion

//#region Agregar "Ver mas" - "Ver menos" a una tabla

function agregarPopOverVerMas(selector, maxPalabras) {
    const celdas = document.querySelectorAll(selector);
    const isDark = document.body.classList.contains("dark-mode");

    celdas.forEach((celda) => {
        const textoCompleto = celda.textContent.trim();
        const palabras = textoCompleto.split(" ");

        if (palabras.length > maxPalabras) {
            const textoTruncado = palabras.slice(0, maxPalabras).join(" ");
            const verMasLink = document.createElement("a");
            verMasLink.href = "#";
            verMasLink.textContent = " Ver más";
            verMasLink.classList.add("ver-mas-popover", "text-link"); // Aplica estilo
            if (isDark) {
                verMasLink.classList.add("dark-text");
            }

            verMasLink.setAttribute("tabindex", "0");
            verMasLink.setAttribute("data-bs-toggle", "popover");
            verMasLink.setAttribute("data-bs-trigger", "focus");
            verMasLink.setAttribute("data-bs-placement", "auto");
            verMasLink.setAttribute("data-bs-html", "true");
            verMasLink.setAttribute("data-bs-content", `<div class="popover-body-custom">${textoCompleto}</div>`);

            celda.innerHTML = `${textoTruncado}... `;
            celda.appendChild(verMasLink);
        }
    });

    // Inicializar todos los popovers
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.forEach((popoverTriggerEl) => {
        new bootstrap.Popover(popoverTriggerEl);
    });
}


//#endregion

//#region Agregar Evento "Enter"

function agregarEventoEnter(selectorInput, callback) {
    $(selectorInput).on("keydown", function (e) {
        if (e.key === "Enter") {
            e.preventDefault();
            callback();
        }
    });
}

//#endregion
