#summary An introduction to BehaveN
#labels Featured

# Introduction #

This tutorial will describe features and scenarios and show a very
simple example of how to use BehaveN to achieve your goals.

During the course of the tutorial, you'll be creating an assembly so create a new Class Library project in Visual Studio. You'll be adding both text files and code to that project.

# Describe your feature #

Your product is unique and every feature it provides is unique. Only you (or your
customer or product owner) knows what those features are, why your
product needs them, and how to prove that it does. Sadly, it's not
uncommon for the people responsible for developing a feature to not
even be aware of why it's so important.

One very effective thing you can do to help focus your team, yourself
included, and ensuring you implement that feature correctly is to give
that feature a name and describe it as succinctly as possible. Doing
this should give everybody on the team something they can wrap their
head around, something they can talk about, something they can reason
about. More importantly, they'll understand the deeper meaning behind
the tasks they're doing and _why_ they have to be done.

One common way of describing features is to use this template:

```
Feature: <name>

As a <stakeholder>
I want <something>
So that <I can achieve some purpose>
```

Elizabeth Keogh suggests an alternate template that focuses more on
the vision by placing it first:

```
Feature: <name>

In order to <achieve the vision>
As a <stakeholder>
I want <something>
```

Regardless of how you describe your feature, create a new text file, type
it your description, and commit it to your source control system where all of the
developers can see it!

For this tutorial, pretend that you're adding a calculator to your
application. Your "feature" might look like this:

```
Feature: Calculator

In order to perform calculations without using any paper
As a user of the application
I want to be able to add, subtract, multiply, and divide
```

You can use Visual Studio or your favorite text editor to save that in a file called
Calculator.feature.

Once you've identified your goal, you can start to define the
scenarios that describe how it will be achieved.

# What's a scenario? #

You can think of scenarios as being the acceptance criteria for a
particular feature. Please note that a feature might need more than
one scenario in order to fully describe it.

Each scenario consists of a sequence of steps that, when executed,
should result in a successful outcome based on the requirements of the
feature.

Some of the steps in a scenario will set up any required context for
that scenario. Other steps will perform the actions or trigger the
events that the scenario covers. Finally, some steps will validate the
expected outcomes of the scenario. All of these steps must execute
correctly in order for a scenario to pass.

In order to communicate about scenarios succinctly, it's important to
give them names.

# Write your scenario #

Back in your text editor, open up Calculator.feature and add this
below the feature description:

```
Scenario: Adding two numbers

Given a new calculator
When adding 1 and 2
Then the result should be 3
```

The line that starts with "Scenario:" is required. You must give your
scenario a name.

The lines that start with "Given", "When", and "Then" are called
steps. You can have as many steps as you want in your scenarios. You
can have multiple givens, whens, or thens in your scenarios. The order
isn't restricted, either, even though it makes the most sense to start
with the givens, follow those with at least one when, and then finish
up with some thens.

You can have more than one scenario in a single file, but start
small and build your way up.

# Verify your scenario #

Here comes the fun part. The text in the file that you've been writing is actually a kind of domain-specific language that can be "run" to see if all of your expectations have been met.

There are multiple ways to "run" your specifications. The BehaveN.dll assembly exposes an API that lets you programmatically verify the contents of a file. You can also use the BehaveN.Tool.exe assembly to do the same thing from the command line or a build script.

Let's start out by using the API since that will probably be a common way to verify specifications since it allows you to do so using your favorite unit testing framework if you want to.

Add a reference to BehaveN.dll.

Add a class to the Visual Studio project that contains your text file. Give that class a single method and fill it in like this:

```
public class CalculatorFeature
{
    public void VerifyFeature()
    {
        BehaveN.Verify.File("Calculator.feature", typeof(CalculatorFeature).Assembly);
    }
}
```

The names of the class and method aren't important. Use whatever makes sense to you.

The first parameter is the name of the file you want to verify. You might want to change the "Copy to Output Directory" property to "Copy if newer" to make sure the file is in the same directory as the assembly.

The second parameter is the assembly that contains the step definitions for the file. That's the current assembly. You haven't defined any steps yet, but that's OK.

The thing you want to do now is to execute this method. If you have TestDriven.Net installed, you can right-click the method and select Test With -> In-Proc and it should construct an instance of your class and invoke the method.

If you don't have TestDriven.Net installed, you'll have to resort to the more traditional way of running tests. Add a reference to your favorite unit testing framework and put the appropriate attributes on your class and/or method to mark them as something that can be run. For example, if you're using NUnit, you would modify the class to look like this:

```
using NUnit.Framework;

[TestFixture]
public class CalculatorFeature
{
    [Test]
    public void VerifyFeature()
    {
        BehaveN.Verify.File("Calculator.feature", typeof(CalculatorFeature).Assembly);
    }
}
```

This is something you might want to do anyways so that it can be automatically discovered by your continuous integration system (such as CruiseControl or TeamCity).

Regardless of how you run it, you should see output similar to the following:

```
Scenario: Adding two numbers

? Given a new calculator

? When adding 1 and 2

? Then the result should be 3

---

Your undefined steps can be defined with the following code:

public void given_a_new_calculator()
{
    throw new NotImplementedException();
}

public void when_adding_arg1_and_arg2(int arg1, int arg2)
{
    throw new NotImplementedException();
}

public void then_the_result_should_be_arg1(int arg1)
{
    throw new NotImplementedException();
}
```

Those question mark characters (?) indicate that the step you're trying to execute is undefined. After the three dash characters (---) comes some code that BehaveN is suggesting could be used to define methods for those steps.

Copy those three methods and paste them into your project. You can put them in the same class you already have or in a new class. I prefer to put them in a new class myself, but it's totally up to you.

You should have something like this in your project now:

```
public class CalculatorStepDefinitions
{
    public void given_a_new_calculator()
    {
        throw new NotImplementedException();
    }

    public void when_adding_arg1_and_arg2(int arg1, int arg2)
    {
        throw new NotImplementedException();
    }

    public void then_the_result_should_be_arg1(int arg1)
    {
        throw new NotImplementedException();
    }
}
```

If you run your test again, you should get different output this time:

```
Scenario: Adding two numbers

! Given a new calculator

  When adding 1 and 2

  Then the result should be 3
```

That exclamation point character (!) indicates that that step failed. You should see the stack trace showing exactly what lines and files were involved. Given that the methods you pasted into your project all consist of a single line throwing a `NotImplementedException`, this result shouldn't be too surprising.

# Define the steps your scenarios need to run #

At this point, you have the stubs for you step definitions but need to fill them out with some real code.

In order to fill in your step definitions, you need some real code to exercise. Pretend that the following class is the domain object you're verifying:

```
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}
```

You can probably imagine how to fill in the three methods. If not, here they are:

```
public class CalculatorStepDefinitions
{
    private Calculator _calculator;
    private int _sum;

    public void given_a_new_calculator()
    {
        _calculator = new Calculator();
    }

    public void when_adding_arg1_and_arg2(int arg1, int arg2)
    {
        _sum = _calculator.Add(arg1, arg2);
    }

    public void then_the_result_should_be_arg1(int arg1)
    {
        Debug.Assert(_sum == arg1);
    }
}
```

Notice the two fields that were added to the class to help keep manage the state of the test between steps. A new instance of this class gets constructed for every scenario so you don't need to worry about cleaning up those fields in between scenarios.

Having filled in your step definitions, you should now see it passing.

# Other facts you should know #

The names of your parameters don't have to be `arg1` or `arg2`. You can give them any names you want. The placeholders within the method names, however, must follow the `arg#` pattern. For example, the "when" step from above could be rewritten like this:

```
public void when_adding_arg1_and_arg2(int x, int y)
{
    _sum = _calculator.Add(x, y);
}
```

BehaveN supports passing more complex data into step definitions. See the [forms](Forms.md) and [grids](Grids.md) pages for more information.

BehaveN also supports output parameters. Parameters that are marked with the `out` keyword are verified by the BehaveN runner. They act as if they're calling to `Assert.AreEqual`. For example, the "then" step from above could be rewritten like this:

```
public void then_the_result_should_be_arg1(out int arg1)
{
    arg1 = _sum;
}
```

That might seem pretty simple and it is. There are some advantages to doing this in terms of what gets output after a failed run. Using output parameters with forms and grids is where this feature really shines.

Besides being able to verify specifications in text files, the `BehaveN.Verify` class has methods for verifying embedded resources and strings.

To verify files with BehaveN.Tool.exe, you would issue a command like this:

```
BehaveN.Tool.exe Verify Calculator.feature CalculatorTests.dll
```

BehaveN.Tool.exe offers multiple commands, run `BehaveN.Tool.exe Help` to see them all.

There are many more features of BehaveN that aren't documented yet. Feel free to check out the source code or email me if you have any questions or suggestions.

# Who's supposed to do all this? #

Ideally, the customer or product owner would be describing the features and scenarios they want implemented. If this can't happen, perhaps QA could fill in. Otherwise, it falls on the developers. Developers can still get quite a bit of value out of defining specifications in this way. The manner in which BehaveN forces you to encapsulate logic in reusable steps makes it really easy to compose new tests. It's also very useful for communicating with the stakeholders and QA since the specifications are (as far as they can tell) just plain English.

# Further reading #

http://dannorth.net/introducing-bdd

http://www.infoq.com/articles/pulling-power