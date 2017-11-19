namespace WhatIsHeDoing.DomainModels.Barcodes
{
    /// <summary>
    /// Base barcode contract.
    /// </summary>
    public interface IBarcode
    {
        /// <summary>
        /// Gets the string representation of this barcode.
        /// </summary>
        /// <returns>String</returns>
        string ToString();
    }
}
