# DateTime formats #

You can use natural language strings instead of hard coding dates in your tests.

Here are the named instants in time:

  * now
  * today
  * yesterday
  * tomorrow

You can also use relative dates:

  * in X units
  * X units ago

"X" can be any number. "units" can be any of the following:

  * years
  * months
  * days
  * hours
  * minutes
  * seconds

The trailing "s" is optional.

If the units are "year", "month", or "day", the time is relative to "today". Otherwise, it's relative "now".

You can be more specific about the instant your time is relative to by using one of the following formats:

  * X units from instant
  * X units before instant

In the above examples, "units" is the same as before. "instant" is any of the named instants ("now", "today", etc).