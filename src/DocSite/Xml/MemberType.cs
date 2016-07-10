namespace DocSite.Xml
{
    /// <summary>
    /// Enum representing the various member types.
    /// </summary>
    public enum MemberType
    {
        /// <summary>
        /// A namespace member.
        /// </summary>
        Namespace,
        /// <summary>
        /// A type member.
        /// </summary>
        Type,

        /// <summary>
        /// A field member, including constants.
        /// </summary>
        Field,

        /// <summary>
        /// A property member.
        /// </summary>
        Property,

        /// <summary>
        /// A method member, including constructors.
        /// </summary>
        Method,

        /// <summary>
        /// An event member.
        /// </summary>
        Event,

        /// <summary>
        /// An errored member.
        /// </summary>
        Error
    }
}