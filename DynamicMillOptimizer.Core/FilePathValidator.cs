namespace DynamicMillOptimizer.Core;

public class FilePathValidator
{
    public static (FilePathValidationResult ValidationResult, FileInfo? File) Validate(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return (FilePathValidationResult.InvalidFilePath, null);
        }

        var filePathWithoutQuotes = filePath.Replace("\"", "");

        var file = new FileInfo(filePathWithoutQuotes);

        if (!file.Exists)
        {
            return (FilePathValidationResult.FileNotFound, null);
        }

        return (FilePathValidationResult.Valid, file);
    }
}