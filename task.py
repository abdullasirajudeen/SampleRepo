"""A simple task module that performs basic operations."""


def greet(name):
    """Return a greeting message for the given name."""
    if not name or not isinstance(name, str):
        raise ValueError("Name must be a non-empty string")
    return f"Hello, {name}!"


def add(a, b):
    """Return the sum of two numbers."""
    if not isinstance(a, (int, float)) or not isinstance(b, (int, float)):
        raise TypeError("Both arguments must be numbers")
    return a + b


def is_even(n):
    """Return True if the number is even, False otherwise."""
    if not isinstance(n, int):
        raise TypeError("Argument must be an integer")
    return n % 2 == 0


if __name__ == "__main__":
    print(greet("World"))
    print(f"2 + 3 = {add(2, 3)}")
    print(f"4 is even: {is_even(4)}")
    print(f"7 is even: {is_even(7)}")
