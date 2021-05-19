using System.Collections.Generic;

namespace HispaniaCommon.ViewModel
{
    public class RepositoryViewModel
    {
        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static RepositoryViewModel instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static RepositoryViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RepositoryViewModel();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private RepositoryViewModel()
        {
        }

        #endregion

        #region Data

        public static SortedDictionary<int, string> Month = new SortedDictionary<int, string>()
        {
            { 0, "Tots" },
            { 1, "Gener" },
            { 2, "Febrer" },
            { 3, "Març" },
            { 4, "Abril" },
            { 5, "Maig" },
            { 6, "Juny" },
            { 7, "Juliol" },
            { 8, "Agost" },
            { 9, "Setembre" },
            { 10, "Octubre" },
            { 11, "Novembre" },
            { 12, "Dessembre" }
        };

        #endregion
    }
}