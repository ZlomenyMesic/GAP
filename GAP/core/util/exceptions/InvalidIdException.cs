namespace GapCore.util.exceptions;

public class InvalidIdException : Exception {
    public InvalidIdException(string id) : base($"Invalid ID: {id}") { }
}

public class DuplicateIdException : Exception {
    public DuplicateIdException(string id) : base($"Detected duplicate ID '{id}'") { }
}