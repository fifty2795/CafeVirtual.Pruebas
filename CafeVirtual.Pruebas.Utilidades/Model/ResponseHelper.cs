using CafeVirtual.Pruebas.Utilidades.Interfaces;

namespace CafeVirtual.Pruebas.Utilidades.Model
{
    public static class ResponseHelper
    {
        public static ResponseViewModel<T> CrearRespuestaExito<T>(T? data, string mensajeExito)
        {
            var response = new ResponseViewModel<T>
            {
                Success = true,
                Message = mensajeExito,
                Data = data
            };            

            return response;
        }

        public static ResponseViewModel<T> CrearRespuestaError<T>(string mensajeError)
        {
            return new ResponseViewModel<T>
            {
                Success = false,
                Message = mensajeError,
                //Error = new ErrorViewModel(ex.Message, ex.StackTrace, ex.Source)
                //Error = new ErrorViewModel(mensajeError, code)
            };
        }
    }
}
