# How to clean up after a scenario completes #

Sometimes, you might change some data in a persistent store and you want those changes to be reverted at the end of a scenario to prevent subsequent scenarios from failing when they encounter data that they don't expect.

# Details #

This is really easy. Just make the class that contains the step definitions implement `IDisposable`. In its `Dispose` method, write whatever you need to do your clean up. The `Dispose` method will get called even if the scenario failed.

Here's an example:

```
public class MyStepDefinitions : IDisposable
{
    public void when_changing_data_from_foo_to_bar()
    {
        // Change foo to bar.
    }

    public void Dispose()
    {
        // Change bar back to foo.
    }
}
```

Depending on how complex your class is (try not to let it get too complex!), you may want to set a flag in your step definition and check if that flag is set in your `Dispose` method.

```
public class MyStepDefinitions : IDisposable
{
    private bool _fooChangedToBar;

    public void when_changing_data_from_foo_to_bar()
    {
        // Change foo to bar.

        _fooChangedToBar = true;
    }

    public void Dispose()
    {
        if (_fooChangedToBar)
        {
            // Change bar back to foo.
        }
    }
}
```