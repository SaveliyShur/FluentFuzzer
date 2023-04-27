namespace FuzzerRunner
{
    public interface IConstructor
    {
        /// <summary>
        /// Construct object T
        /// </summary>
        /// <typeparam name="T">Type of construct object</typeparam>
        /// <returns>Constructed object</returns>
        T Construct<T>();

        /// <summary>
        /// Change all strings in object to section title name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        object ChangeAllStringToSectionTitles(object t);

        /// <summary>
        /// Use string in all files in folder and subfolder.
        /// You can write comments in the files, use # symbol
        /// </summary>
        /// <param name="folderPath">Path with files, which contains string corpuses</param>
        /// <returns></returns>
        Task UseStringCorpusAsync(string folderPath);

        /// <summary>
        /// Use string in all files in folder and subfolder.
        /// You can write comments in the files, use # symbol
        /// </summary>
        /// <param name="folderPath">Path with files, which contains string corpuses</param>
        /// <returns></returns>
        void UseStringCorpus(string folderPath);

        /// <summary>
        /// Use string in all files in folder and subfolder.
        /// You can write comments in the files, use # symbol
        /// The method set static corpus for all instance constructions
        /// </summary>
        /// <param name="folderPath">Path with files, which contains string corpuses</param>
        /// <returns></returns>
        Task UseStaticStringCorpusAsync(string folderPath);

        /// <summary>
        /// Use string in all files in folder and subfolder.
        /// You can write comments in the files, use # symbol
        /// The method set static corpus for all instance constructions
        /// </summary>
        /// <param name="folderPath">Path with files, which contains string corpuses</param>
        /// <returns></returns>
        void UseStaticStringCorpus(string folderPath);

        /// <summary>
        /// You can use predefined string corpus. By default - true
        /// </summary>
        /// <param name="isUsed">True - used predefined corpus, False - off</param>
        void UseStandartStringCorpus(bool isUsed = true);

        void AddStringToTestStringCorpus(string testString);
    }
}
