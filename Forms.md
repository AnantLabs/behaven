# Introduction #

Forms allow scenario authors to execute steps with more complex data than simple the simple values they can type inline. They appear beneath steps using colons to separate one or more names from their associated values.

# Details #

Here's an example of what a form looks like:

```
Given a customer that looks like this:
    : First name : Homer
    :  Last name : Simpson
    :    Address : 742 Evergreen Terrace
    :       City : Springfield
    :   Zip code : 49007
```

The step definition for the above would need to look something like this:

```
public void given_a_customer_that_looks_like_this(Customer customer)
{
    // Use customer here.
}
```

The definition for the `Customer` type would need to look something like this:

```
public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
}
```

Those properties are all strings, but any simple property that can be converted from a string to a value would work (such as integers, decimals, and date times).

Each step can only accept a single form as input. If you want to accept multiple instances of some type, you need to use a [grid](Grids.md) instead.

Forms can also be used as output parameters. When you do this, the step definition is required to assign an object to the output parameter. BehaveN will then loop over all of the properties in the form in the scenario and compare them with the properties on the object. If any are different, the step fails.