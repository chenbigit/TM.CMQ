
internal static class StringHelperClass {
    internal static string[] StringSplit(string source, string regexDelimiter, bool trimTrailingEmptyStrings) {
        string[] splitArray = System.Text.RegularExpressions.Regex.Split(source, regexDelimiter);

        if (trimTrailingEmptyStrings) {
            if (splitArray.Length > 1) {
                for (int i = splitArray.Length; i > 0; i--) {
                    if (splitArray[i - 1].Length > 0) {
                        if (i < splitArray.Length)
                            System.Array.Resize(ref splitArray, i);

                        break;
                    }
                }
            }
        }

        return splitArray;
    }
}