BehaveN allows you to practice [Behaviour-Driven Development](http://dannorth.net/introducing-bdd) by verifying specifications written in a simple DSL.

Features:

  * Works with .NET 2.0 and up
  * Usable with any testing framework or with its own runner
  * Specifications are authored in plain text files using the [Given/When/Then](GivenWhenThen.md) grammar.
  * Syntax for [forms](Forms.md) and [grids](Grids.md) with automatic conversion to .NET objects
  * Statically type-safe mechanism for sharing [contextual data](Contexts.md) across reusable components
  * Consistent approach to [cleaning up](TearDown.md) after a scenario runs (using `IDisposable`)

Like [Cucumber](http://cukes.info/), BehaveN can verify specifications written in a business-readable, domain-specific language. For example, this scenario from Dan North's [Introducing BDD](http://dannorth.net/introducing-bdd) article can be verified by BehaveN:

```
Scenario: Account is in credit

Given the account is in credit
And the card is valid
And the dispenser contains cash
When the customer requests cash
Then ensure the account is debited
And ensure cash is dispensed
And ensure the card is returned
```

Of course, a developer is going to need to implement the different steps named above, but that's done separately from the actual specifications themselves. This makes it possible for non-technical stakeholders to author the specifications, giving them the means to unambiguously communicate with the developers.

BehaveN works with any test framework so you don't have to give up using your favorite one or start using a new test runner. If you don't have a favorite, BehaveN has its own runner that you can use.

To learn more, read the [Tutorial](Tutorial.md).