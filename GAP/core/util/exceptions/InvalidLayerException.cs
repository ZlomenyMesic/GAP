﻿namespace GapCore.util.exceptions;

internal class InvalidLayerException : Exception {
    internal InvalidLayerException(int errorId, string detail = "") : base(
        errorId switch {
            0 => $"unknown layer: {detail}",        // wrong layer name
            1 => $"not enough layers: {detail}",    // 0 or negative number of layers
            _ => $"{detail}"                        // potential custom errors
        }) { }
}
