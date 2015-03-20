# Expecting exceptions #

Normally, any exception thrown during the execution of a step will cause that step and the whole scenario to fail.

There are times when you might expect steps to fail.

To do that, you need to create a step definition that accepts an `Exception` as a parameter reference that step definition _immediately after_ the step that you expect to fail.

Here's an example where doing foo with "bar" is OK, but doing foo with "quux" will cause an exception to be thrown:

```
When doing foo with "bar"
Then the result should be "baz"
When doing foo with "quux"
Then doing foo should have failed
```

Here are the step definitions:

```
public void when_doing_foo_with_arg1(string arg) { ... }

public void then_the_result_should_be_arg1(string arg) { ... }

public void then_doing_foo_should_have_failed(Exception e)
{
    Assert.IsNotNull(e);
}
```

You have to check to see if the exception is not null because null will be passed in if the previous step did not throw an exception.

You can also examine the exception to do whatever custom checking you want:

```
public void then_the_step_should_have_failed_with_message_arg1(string message, Exception e)
{
    Assert.AreEqual(message, e.Message);
}
```

You could even let BehaveN do the assert for you:


```
public void then_the_step_should_have_failed_with_message_arg1(out string message, Exception e)
{
   message = e.Message;
}
```