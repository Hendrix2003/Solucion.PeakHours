﻿namespace SolucionPeakHours.Shared.Common
{
    public class ResponseAPI<T>

    {
        public bool EsCorrecto { get; set; }

        public T? Valor { get; set; }

        public string? Mensaje { get; set; }
    }
}
