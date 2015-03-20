# Introduction #

Grids allow scenario authors to execute steps with more complex data than simple the simple values they can type inline. They appear beneath steps using vertical bars to separate the cells of each row in the grid. The first row contains the labels for the columns.

# Details #

Here's an example of what a grid looks like:

```
Given the following customers:
    | First name | Last name |
    |      Homer |   Simpson |
    |      Marge |   Simpson |
    |       Bart |   Simpson |
    |       Lisa |   Simpson |
    |     Maggie |   Simpson |
```

The step definition for the above would need to look something like this:

```
public void given_the_following_customers(List<Customer> customers)
{
    // Use customers here.
}
```

The definition for the `Customer` type would need to look something like this:

```
public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

Those properties are all strings, but any simple property that can be converted from a string to a value would work (such as integers, decimals, and date times).

Each step can only accept a single grid as input. If you only need to accept a single instance of some type, you could use a [form](Forms.md) instead.

Grids can also be used as output parameters. When you do this, the step definition is required to assign an enumerable of objects to the output parameter. BehaveN will then loop over all of the rows in the grid in the scenario and compare them with the objects in the enumerable. If any are different, the step fails.