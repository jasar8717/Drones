using System.ComponentModel;

namespace Drones.Core.Utils
{
    public class UtilsProject
    {
        /// <summary>
        /// Gets enum description if Description is present
        /// </summary>
        /// <param name="enumValue">Value to get Description</param>
        /// <returns>Field Description or Empty if Description is not provided</returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            string description = "";
            var memberDescription = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute((memberDescription), typeof(DescriptionAttribute));

            if (descriptionAttribute != null)
            {
                description = descriptionAttribute.Description;
            }
            else
            {
                description = enumValue.ToString();
            }

            return description;
        }
    }
}
