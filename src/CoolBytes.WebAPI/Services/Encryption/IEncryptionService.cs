namespace CoolBytes.WebAPI.Services.Encryption
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts text.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <returns>A base64 encoded result.</returns>
        string Encrypt(string text);

        /// <summary>
        /// Decrypts encrypted text.
        /// </summary>
        /// <param name="encrypted">The encrypted text</param>
        /// <returns>The decrypted text</returns>
        string Decrypt(string encrypted);
    }
}