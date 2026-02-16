"""Tests for the task module."""

import unittest

from task import add, greet, is_even


class TestGreet(unittest.TestCase):
    def test_greet_returns_greeting(self):
        self.assertEqual(greet("World"), "Hello, World!")

    def test_greet_with_name(self):
        self.assertEqual(greet("Alice"), "Hello, Alice!")

    def test_greet_empty_string_raises(self):
        with self.assertRaises(ValueError):
            greet("")

    def test_greet_non_string_raises(self):
        with self.assertRaises(ValueError):
            greet(123)


class TestAdd(unittest.TestCase):
    def test_add_positive_numbers(self):
        self.assertEqual(add(2, 3), 5)

    def test_add_negative_numbers(self):
        self.assertEqual(add(-1, -2), -3)

    def test_add_floats(self):
        self.assertAlmostEqual(add(1.5, 2.5), 4.0)

    def test_add_non_numbers_raises(self):
        with self.assertRaises(TypeError):
            add("a", 1)


class TestIsEven(unittest.TestCase):
    def test_even_number(self):
        self.assertTrue(is_even(4))

    def test_odd_number(self):
        self.assertFalse(is_even(7))

    def test_zero_is_even(self):
        self.assertTrue(is_even(0))

    def test_non_integer_raises(self):
        with self.assertRaises(TypeError):
            is_even(3.5)


if __name__ == "__main__":
    unittest.main()
