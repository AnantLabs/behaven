## Introduction ##

The given/when/then syntax allows scenario authors to form narratives in a "natural" language that describe the specifications they need fulfilled.

## Details ##

The differences between "given", "when", and "then" are purely cosmetic. BehaveN doesn't care what order they appear in or what they actually mean.

Typically, you should use "given" steps to set up the context for your scenario, "when" steps to execute the action, and "then" steps to assert the expected results.

The "and" keyword is used to repeat the previous keyword.

## Arguments ##

BehaveN allows "arguments" to be specified in the steps as inline values. For example:

```
Given a customer named "Homer" with an age of 40
```

The step definition for the above would need to look something like this:

```
public void given_a_customer_named_arg1_with_an_age_of_arg2(string name, int age)
{
    // Use arguments here.
}
```

Notice how "arg1" and "arg2" are used as placeholders within the method name to indicate where the inline values should appear.

More complex objects can also be passed in using [forms](Forms.md) and [grids](Grids.md).