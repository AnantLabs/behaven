# Introduction #

BehaveN forces you to break your test code up into modular step definitions. Those step definitions often need to share state in order to do their work. To make it easy to share state between step definitions, BehaveN can act as a pseudo-IoC container, passing "context objects" into the constructors of the classes containing your step definitions.

# Details #

Let's say you decided to split your step definitions across multiple classes. One step definition creates a customer. Another step definition (in a different class!) needs to use that customer to perform some operation. Here's how that could be done:

```
public class MyStepDefinitions
{
    private MyContext _context;

    public MyStepDefinitions(MyContext context)
    {
        _context = context;
    }

    public void Given_a_new_customer_named_arg1(string name)
    {
        _context.Customer = new Customer(name);
    }
}

public class MyOtherStepDefinitions
{
    private MyContext _context;

    public MyOtherStepDefinitions(MyContext context)
    {
        _context = context;
    }

    public void When_changing_the_status_on_the_customer_to_arg1(CustomerStatus status)
    {
        _context.Customer.ChangeStatusTo(status);
    }
}

public class MyContext
{
    public Customer Customer { get; set; }
}
```

Notice how the "context object" is a simple class. In this example, it's named `MyContext`, but you can name it whatever you want. You can put as many fields, properties, or even methods on it as you want. All BehaveN requires is that it have a parameterless constructor so that it can create an instance of it.

The two classes that require the a `MyContext` instance declare that with their constructors. BehaveN notices that the constructor requires an argument so creates an instance of the appropriate type. That same instance is used to for **all** classes containing step definitions that need it. A constructor can accept multiple arguments if it needs to and BehaveN will try to satisfy them all.

New instances of the classes containing your step definitions and the "context objects" they require are created for **each** scenario that gets executed. If the class containing your step definitions implements `IDisposable`, its `Dispose` method will be invoked when the scenario is done executing. The same is true for the context objects that BehaveN created to satisfy the step definitions.