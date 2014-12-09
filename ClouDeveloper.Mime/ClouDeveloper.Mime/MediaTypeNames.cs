using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ClouDeveloper.Mime
{
    /// <summary>
    /// MediaTypeNames
    /// </summary>
    public static partial class MediaTypeNames
    {
        /// <summary>
        /// Gets the media type names.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetMediaTypeNames(string fileExtension)
        {
            fileExtension = String.Concat(fileExtension ?? String.Empty).Trim().ToUpperInvariant();

            if (!fileExtension.StartsWith(".", StringComparison.OrdinalIgnoreCase))
                fileExtension = String.Concat(".", fileExtension);

            if (fileExtension.Equals(".*", StringComparison.OrdinalIgnoreCase) ||
                fileExtension.Equals(".", StringComparison.OrdinalIgnoreCase))
                return new string[] { application.octet_stream };

            var dic = fileExtensions.Value;
            ConcurrentBag<string> list;

            if (!dic.TryGetValue(fileExtension, out list))
                return new string[] { application.octet_stream };

            return list.ToArray();
        }

        /// <summary>
        /// Gets the suppported extensions.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetSuppportedExtensions()
        {
            return fileExtensions.Value.Keys;
        }

        /// <summary>
        /// The file extensions
        /// </summary>
        private static readonly Lazy<ConcurrentDictionary<string, ConcurrentBag<string>>> fileExtensions =
            new Lazy<ConcurrentDictionary<string, ConcurrentBag<string>>>(() => EnumerateExtensionsInternal(), true);

        /// <summary>
        /// Enumerates the extensions internal.
        /// </summary>
        /// <returns></returns>
        private static ConcurrentDictionary<string, ConcurrentBag<string>> EnumerateExtensionsInternal()
        {
            Type thisType = typeof(MediaTypeNames);

            var types = new ConcurrentBag<Type>();
            types.Add(thisType);

            foreach (var eachNestedType in thisType.GetNestedTypes())
                types.Add(eachNestedType);

            var results = new Dictionary<string, List<string>>();

            foreach (Type eachType in types)
            {
                foreach (FieldInfo eachField in eachType.GetFields())
                {
                    if (!eachField.IsLiteral)
                        continue;

                    if (eachField.FieldType != typeof(string))
                        continue;

                    var fileExtAttr = eachField
                        .GetCustomAttributes(typeof(AssociatedFileExtensionAttribute), false)
                        .FirstOrDefault() as AssociatedFileExtensionAttribute;

                    if (fileExtAttr == null)
                        continue;

                    string fileExt = fileExtAttr.Extension;

                    if (String.IsNullOrWhiteSpace(fileExt))
                        continue;

                    string mimeTypeName = eachField.GetValue(null) as string;

                    if (String.IsNullOrWhiteSpace(mimeTypeName))
                        continue;

                    if (!results.ContainsKey(fileExt))
                        results.Add(fileExt, new List<string>());

                    List<string> candidates = results[fileExt];

                    if (candidates == null)
                        results[fileExt] = candidates = new List<string>();

                    foreach (string eachMimeTypeName in candidates)
                        if (eachMimeTypeName.Equals(mimeTypeName))
                            continue;

                    candidates.Add(mimeTypeName);
                }
            }

            Dictionary<string, ConcurrentBag<string>> temp =
                new Dictionary<string, ConcurrentBag<string>>();
            foreach (var kvp in results)
                temp.Add(kvp.Key, new ConcurrentBag<string>(kvp.Value));

            return new ConcurrentDictionary<string, ConcurrentBag<string>>(temp);
        }
    }
}
