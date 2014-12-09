using System;

namespace ClouDeveloper.Mime
{
    /// <summary>
    /// AssociatedFileExtensionAttribute
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class AssociatedFileExtensionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssociatedFileExtensionAttribute"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public AssociatedFileExtensionAttribute(string extension)
            : base()
        {
            this.extension = (extension ?? String.Empty).Trim().ToUpperInvariant();
        }

        /// <summary>
        /// The extension
        /// </summary>
        private string extension;

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public string Extension
        {
            get { return this.extension; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.extension;
        }
    }
}
